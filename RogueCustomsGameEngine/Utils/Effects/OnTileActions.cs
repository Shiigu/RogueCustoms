using Newtonsoft.Json.Linq;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Exceptions;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public static class OnTileActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static async Task<bool> TransformTile(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                // Source must be a Character
                return false;

            if (Args.Target is not Tile t)
                // Target must be a Tile
                return false;

            if (!t.Type.CanBeTransformed)
                // Target must be a Tile that can be Transformed
                return false;

            if(!paramsObject.TileType.IsWalkable && t.IsOccupied)
                // Cannot convert an occupied Tile into a non-walkable Tile
                return false;

            if (paramsObject.TileType == TileType.Hallway || paramsObject.TileType == TileType.Stairs || paramsObject.TileType == TileType.Empty)
                // Cannot turn the tile into Hallway, Stairs or Empty
                return false;

            if (paramsObject.TileType == t.Type)
                // Cannot turn the tile into something it already is
                return false;

            if (Map.GetAdjacentTiles(t.Position, false).Exists(t => t.Type == TileType.Empty))
                // Cannot convert a Tile that is adjacent to an empty tile
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            var success = false;
            var oldType = t.Type;

            t.ChangeType(paramsObject.TileType);
            success = Map.Tiles.IsFullyConnected(t => t.IsWalkable);

            if (!success)
            {
                t.Type = oldType;
            }
            else
            {
                t.RemainingTransformationTurns = (int) paramsObject.TurnLength;
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    Map.AppendMessage(Map.Locale["CharacterConvertedTile"].Format(new { CharacterName = c.Name, TileType = Map.Locale[$"TileType{t.Type}"] }), Color.DeepSkyBlue, events);
                    if (!Map.IsDebugMode)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { Args.Target.Position, Map.GetConsoleRepresentationForCoordinates(Args.Target.Position.X, Args.Target.Position.Y) }
                        }
                        );
                    }
                }
                if(t.OnStood != null && t.LivingCharacter != null)
                    await t.OnStood.Do(t.LivingCharacter, t.LivingCharacter, false, false);
            }

            Map.DisplayEvents.Add(($"A {Map.Locale[$"TileType{t.Type}"]} got created", events));

            return success;
        }

        public static bool PlaceTrap(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();

            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                // Source must be a Character
                return false;

            if (Args.Target is not Tile t)
                // Target must be a Tile
                return false;

            if (!t.Type.AcceptsItems)
                // Target Tile must accept items
                return false;

            if (t.Trap != null)
                // Target Tile must not already have a Trap
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            var trapClass = Map.PossibleTrapClasses.Find(c => c.Id.Equals(paramsObject.Id));

            if (trapClass == null)
                // Must have a valid Trap class to spawn
                return false;

            var trap = Map.AddEntity(paramsObject.Id, 1, Args.Target.Position) as Trap;

            if (trap == null || trap.Position == null)
                return false;

            trap.Visible = trapClass.StartsVisible;
            trap.Faction = c.Faction;

            if (c == Map.Player || Map.Player.CanSee(c))
            {
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { Args.Target.Position, Map.GetConsoleRepresentationForCoordinates(Args.Target.Position.X, Args.Target.Position.Y) }
                    }
                    );
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.TrapSet }
                }
                ); 
                Map.AppendMessage(Map.Locale["CharacterCreatedATrap"].Format(new { CharacterName = c.Name, TrapName = trap.Name }), Color.DeepSkyBlue, events);
            }

            Map.DisplayEvents.Add(($"{c.Name} set up a trap", events));
            return true;
        }

        public static async Task<bool> SpawnNPC(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                // Source must be a Character
                return false;

            if (Args.Target is not Tile t)
                // Target must be a Tile
                return false;

            if (!t.IsWalkable)
                // Target Tile must be a Walkable
                return false;

            if (t.LivingCharacter != null)
                // Target Tile must NOT be occupied
                return false;

            var npcClass = Map.PossibleNPCClasses.Find(c => c.Id.Equals(paramsObject.Id));
            if (npcClass == null)
                // Must have a valid NPC class to spawn
                return false;
                        
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var level = (int) paramsObject.Level;

                if (await Map.AddEntity(npcClass.Id, level, t.Position) is not NonPlayableCharacter npc 
                    || npc.Position == null)
                    // Failed to spawn NPC
                    return false;
                npc.Faction = c.Faction;
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    if (!Map.IsDebugMode)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { t.Position, Map.GetConsoleRepresentationForCoordinates(t.Position.X, t.Position.Y) }
                        }
                        );
                    }
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.Summon }
                    }
                    );
                    Map.AppendMessage(Map.Locale["CharacterCreatedAnNPC"].Format(new { CharacterName = c.Name, NPCName = npc.Name }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} spawned {npc.Name}", events));
                return true;
            }
            return false;
        }

        public static bool ReviveNPC(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                // Source must be a Character
                return false;

            if (Args.Target is not Tile t)
                // Target must be a Tile
                return false;

            if (!t.IsWalkable)
                // Target Tile must be Walkable
                return false;

            if (!t.GetDeadCharacters().Exists(dc => dc.Faction == c.Faction || dc.Faction.IsAlliedWith(c.Faction)))
                // Attempted to Revive when there are no dead Characters in the Tile that are of an allied Faction to Source's.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var npc = t.GetDeadCharacters().Where(dc => dc.Faction == c.Faction || dc.Faction.IsAlliedWith(c.Faction)).TakeRandomElement(Rng) as NonPlayableCharacter;
                npc.HP.Current = npc.HP.Base;
                if (npc.MP != null)
                    npc.MP.Current = npc.MP.Base;
                npc.ExistenceStatus = EntityExistenceStatus.Alive;
                npc.ConsoleRepresentation.Character = npc.BaseConsoleRepresentation.Character;
                npc.ConsoleRepresentation.BackgroundColor = npc.BaseConsoleRepresentation.BackgroundColor;
                npc.ConsoleRepresentation.ForegroundColor = npc.BaseConsoleRepresentation.ForegroundColor;
                npc.ClearKnownCharacters();
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    if (!Map.IsDebugMode)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { t.Position, Map.GetConsoleRepresentationForCoordinates(t.Position.X, t.Position.Y) }
                        }
                    );
                    }
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.NPCRevive }
                    }
                    );
                    Map.AppendMessage(Map.Locale["CharacterRevivedAnNPC"].Format(new { CharacterName = c.Name, NPCName = npc.Name }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} revive {npc.Name}", events));
                return true;
            }
            return false;
        }
        public static bool UnlockDoor(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                // Source must be a Character
                return false;

            if (Args.Target is not Tile t)
                // Target must be a Tile
                return false;

            if (t.Type != TileType.Door)
                // Tile to be unlocked must be a Door
                return false;

            if (paramsObject.DoorId != t.DoorId)
                // Cannot unlock a Door of the wrong Id
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            t.Type = TileType.Hallway;
            t.DoorId = string.Empty;

            try
            {
                var existingValue = (int) Map.GetFlagValue($"Doors_{paramsObject.DoorId}");
                Map.SetFlagValue($"Doors_{paramsObject.DoorId}", existingValue - 1);
            }
            catch (FlagNotFoundException)
            {
                // If, for some reason, the Door Type flag disappeared, create it at 0 to destroy the Key afterwards
                Map.CreateFlag($"Doors_{paramsObject.DoorId}", 0, true);
            }

            if (c == Map.Player || Map.Player.CanSee(c))
            {
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { t.Position, Map.GetConsoleRepresentationForCoordinates(t.Position.X, t.Position.Y) }
                    }
                    );
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.DoorOpen }
                }
                );
                Map.AppendMessage(Map.Locale["CharacterUnlockedDoor"].Format(new { CharacterName = c.Name, DoorName = Map.Locale[$"DoorType{paramsObject.DoorId}"] }), Color.DeepSkyBlue, events);
            }

            Map.DisplayEvents.Add(($"{c.Name} unlocks door", events));
            return true;
        }
    }
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

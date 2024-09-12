using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
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

        public static bool TransformTile(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (Source is not Character c)
                // Source must be a Character
                return false;

            if (Target is not Tile t)
                // Target must be a Tile
                return false;

            if (paramsObject.TileType != TileType.Floor && paramsObject.TileType != TileType.Wall)
                // Cannot turn the tile into Hallway, Stairs or Empty
                return false;

            if (paramsObject.TileType == t.Type)
                // Cannot turn the tile into something it already is
                return false;

            if (Map.GetAdjacentTiles(t.Position, false).Exists(t => t.Type == TileType.Empty))
                // Cannot convert a Tile that is adjacent to an empty tile
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            var success = false;
            var oldType = t.Type;

            if (t.LivingCharacter == null && t.Trap == null && t.GetItems().Count == 0)
            {
                t.Type = paramsObject.TileType;
                success = Map.Tiles.IsFullyConnected(t => t.IsWalkable);
            }

            if (!success)
            {
                t.Type = oldType;
            }
            else if (c.EntityType == EntityType.Player
                || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
            {
                Map.AppendMessage(Map.Locale["CharacterConvertedTile"].Format(new { CharacterName = c.Name, TileType = Map.Locale[$"TileType{t.Type}"] }), Color.DeepSkyBlue);
            }

            return success;
        }

        public static bool PlaceTrap(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (Source is not Character c)
                // Source must be a Character
                return false;

            if (Target is not Tile t)
                // Target must be a Tile
                return false;

            if (t.Type != TileType.Floor && t.Type != TileType.Hallway)
                // Target Tile must be a Floor or Hallway
                return false;

            if (t.Trap != null)
                // Target Tile must not already have a Trap
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            var trapClass = Map.PossibleTrapClasses.Find(c => c.Id.Equals(paramsObject.Id));

            if (trapClass == null)
                // Must have a valid Trap class to spawn
                return false;

            var trap = Map.AddEntity(paramsObject.Id, 1, Target.Position) as Item;

            if (trap == null || trap.Position == null)
                return false;

            trap.Visible = trapClass.StartsVisible;
            trap.Faction = c.Faction;

            if (c.EntityType == EntityType.Player
                || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
            {
                Map.AddSpecialEffectIfPossible(SpecialEffect.TrapSet);
                Map.AppendMessage(Map.Locale["CharacterCreatedATrap"].Format(new { CharacterName = c.Name, TrapName = trap.Name }), Color.DeepSkyBlue);
            }

            return true;
        }

        public static bool SpawnNPC(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (Source is not Character c)
                // Source must be a Character
                return false;

            if (Target is not Tile t)
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
                        
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, null, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var level = (int) paramsObject.Level;

                if (Map.AddEntity(npcClass.Id, level, t.Position) is not NonPlayableCharacter npc 
                    || npc.Position == null)
                    // Failed to spawn NPC
                    return false;
                npc.Faction = c.Faction;
                if (c.EntityType == EntityType.Player
                    || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                {
                    Map.AddSpecialEffectIfPossible(SpecialEffect.Summon);
                    Map.AppendMessage(Map.Locale["CharacterCreatedAnNPC"].Format(new { CharacterName = c.Name, NPCName = npc.Name }), Color.DeepSkyBlue);
                }
                return true;
            }
            return false;
        }

        public static bool ReviveNPC(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (Source is not Character c)
                // Source must be a Character
                return false;

            if (Target is not Tile t)
                // Target must be a Tile
                return false;

            if (!t.IsWalkable)
                // Target Tile must be Walkable
                return false;

            if (!t.GetDeadCharacters().Exists(dc => dc.Faction == c.Faction || dc.Faction.AlliedWith.Contains(c.Faction)))
                // Attempted to Revive when there are no dead Characters in the Tile that are of an allied Faction to Source's.
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, null, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var npc = t.GetDeadCharacters().Where(dc => dc.Faction == c.Faction || dc.Faction.AlliedWith.Contains(c.Faction)).TakeRandomElement(Rng) as NonPlayableCharacter;
                npc.HP = npc.MaxHP;
                if (npc.UsesMP)
                    npc.MP = npc.MaxMP;
                npc.ExistenceStatus = EntityExistenceStatus.Alive;
                npc.ConsoleRepresentation.Character = npc.BaseConsoleRepresentation.Character;
                npc.ConsoleRepresentation.BackgroundColor = npc.BaseConsoleRepresentation.BackgroundColor;
                npc.ConsoleRepresentation.ForegroundColor = npc.BaseConsoleRepresentation.ForegroundColor;
                npc.ClearKnownCharacters();
                if (c.EntityType == EntityType.Player
                    || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                {
                    Map.AddSpecialEffectIfPossible(Enums.SpecialEffect.NPCRevive);
                    Map.AppendMessage(Map.Locale["CharacterRevivedAnNPC"].Format(new { CharacterName = c.Name, NPCName = npc.Name }), Color.DeepSkyBlue);
                }                
                return true;
            }
            return false;
        }
    }
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

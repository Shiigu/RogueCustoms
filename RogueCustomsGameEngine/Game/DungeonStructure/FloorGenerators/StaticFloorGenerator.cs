using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators.Interfaces;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators
{
    [Serializable]
    public class StaticFloorGenerator : IFloorGenerator
    {
        private Map _map;
        private RngHandler Rng => _map.Rng;
        private StaticGenerator _generatorToUse;
        private FloorType FloorConfigurationToUse => _map.FloorConfigurationToUse;
        private int Width => _map.Width;
        private int Height => _map.Height;
        public bool ReadyToFloodFill => true;
        private GamePoint PlayerPosition;

        public StaticFloorGenerator(Map map, StaticGenerator generatorToUse)
        {
            _map = map;
            _generatorToUse = generatorToUse;
        }

        #region Normal Tiles
        public void CreateNormalTiles()
        {
            _map.Rooms = new();
            _map.ResetAndCreateTilesIfNeeded();
            PlayerPosition = null;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _map.Tiles[y, x].Type = _generatorToUse.FloorGeometry[y, x];
                }
            }
        }

        #endregion

        #region Special Tiles
        public void CreateSpecialTiles()
        {
            foreach (var specialSpawn in _generatorToUse.SpecialSpawns)
            {
                if (specialSpawn.ObjectToSpawn is not TileType tt || tt == TileType.Stairs || tt == TileType.Door) continue;
                var tile = _map.Tiles[specialSpawn.Y, specialSpawn.X];
                tile.Type = tt;
            }
        }
        #endregion

        public async Task PlaceEntities()
        {
            var factionsAlliedToPlayers = new List<Faction>();
            var factionsNeutralToPlayers = new List<Faction>();
            var factionsEnemyToPlayers = new List<Faction>();
            var possibleNPCClasses = _map.PossibleNPCClasses.Where(npc => FloorConfigurationToUse.PossibleMonsters.Any(pm => pm.ClassId.Equals(npc.Id))).ToList();
            if (possibleNPCClasses.Count == 0)
                possibleNPCClasses = _map.PossibleNPCClasses;
            var possibleItemClasses = _map.PossibleItemClasses.Except(_map.UndroppableItemClasses).Where(i => FloorConfigurationToUse.PossibleItems.Any(pi => pi.ClassId.Equals(i.Id))).ToList();
            if (possibleItemClasses.Count == 0)
                possibleItemClasses = _map.PossibleItemClasses.Except(_map.UndroppableItemClasses).ToList();
            var possibleTrapClasses = _map.PossibleTrapClasses.Where(t => FloorConfigurationToUse.PossibleTraps.Any(pt => pt.ClassId.Equals(t.Id))).ToList();
            if (possibleTrapClasses.Count == 0)
                possibleTrapClasses = _map.PossibleTrapClasses;
            var playerClasses = _map.Player != null ? _map.PossiblePlayerClasses.Where(pc => pc.Id.Equals(_map.Player.ClassId)) : _map.PossiblePlayerClasses;
            foreach (var playerClass in _map.PossiblePlayerClasses)
            {
                if (playerClass?.Faction != null && !factionsAlliedToPlayers.Contains(playerClass.Faction))
                    factionsAlliedToPlayers.Add(playerClass.Faction);
                foreach (var faction in playerClass.Faction.AlliedWith)
                {
                    if(!factionsAlliedToPlayers.Contains(faction))
                        factionsAlliedToPlayers.Add(faction);
                }
                foreach (var faction in playerClass.Faction.NeutralWith)
                {
                    if (!factionsNeutralToPlayers.Contains(faction))
                        factionsNeutralToPlayers.Add(faction);
                }
                foreach (var faction in playerClass.Faction.EnemiesWith)
                {
                    if (!factionsEnemyToPlayers.Contains(faction))
                        factionsEnemyToPlayers.Add(faction);
                }
            }

            foreach (var specialSpawn in _generatorToUse.SpecialSpawns)
            {
                if (specialSpawn.ObjectToSpawn is EntityClass ec)
                {
                    if (ec.EntityType == EntityType.Key) continue;
                    var newEntity = await _map.AddEntity(ec, Math.Max(1, specialSpawn.Level), new(specialSpawn.X, specialSpawn.Y), false);
                    if (newEntity is Item i)
                    {
                        i.SetQualityLevel(specialSpawn.QualityLevel);
                    }
                }
                if (specialSpawn.ObjectToSpawn is string sid)
                {
                    var spawnPosition = new GamePoint(specialSpawn.X, specialSpawn.Y);
                    var possibleClasses = possibleNPCClasses;
                    if (PlayerPosition.Equals(spawnPosition) || _map.GetTileFromCoordinates(spawnPosition.X, spawnPosition.Y)?.LivingCharacter != null) continue;
                    if (sid == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER)
                    {
                        possibleClasses = possibleNPCClasses.Where(c => c.Faction != null && factionsAlliedToPlayers.Contains(c.Faction)).ToList();
                        if (possibleClasses.Count == 0)
                            possibleClasses = _map.PossibleNPCClasses;
                    }
                    else if (sid == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER)
                    {
                        possibleClasses = possibleNPCClasses.Where(c => c.Faction != null && factionsAlliedToPlayers.Contains(c.Faction)).ToList();
                        if (possibleClasses.Count == 0)
                            possibleClasses = possibleNPCClasses;
                    }
                    else if (sid == EngineConstants.SPAWN_ANY_NEUTRAL_CHARACTER)
                    {
                        possibleClasses = possibleNPCClasses.Where(c => c.Faction != null && factionsNeutralToPlayers.Contains(c.Faction)).ToList();
                        if (possibleClasses.Count == 0)
                            possibleClasses = possibleNPCClasses;
                    }
                    else if (sid == EngineConstants.SPAWN_ANY_ENEMY_CHARACTER)
                    {
                        possibleClasses = possibleNPCClasses.Where(c => c.Faction != null && factionsEnemyToPlayers.Contains(c.Faction)).ToList();
                        if (possibleClasses.Count == 0)
                            possibleClasses = possibleNPCClasses;
                    }
                    else if (sid == EngineConstants.SPAWN_ANY_ITEM)
                    {
                        possibleClasses = possibleItemClasses;
                    }
                    else if (sid == EngineConstants.SPAWN_ANY_TRAP)
                    {
                        possibleClasses = possibleTrapClasses;
                    }
                    else
                    {
                        var match = Regex.Match(sid, EngineConstants.CurrencyRegexPattern);
                        if (match.Success)
                        {
                            var currencyPileId = match.Groups[1].Value;
                            var currencyPile = _map.CurrencyData.Find(cp => cp.Id.Equals(currencyPileId));
                            if (currencyPile != null)
                            {
                                _map.CreateCurrency(currencyPile, new(specialSpawn.X, specialSpawn.Y), false);
                            }
                        }
                        continue;
                    }
                    if (possibleClasses.Count == 0) continue;
                    var pickedClass = possibleClasses.TakeRandomElement(Rng);
                    var newEntity = await _map.AddEntity(pickedClass, Math.Max(1, (sid != EngineConstants.SPAWN_ANY_TRAP) ? specialSpawn.Level : 1), new(specialSpawn.X, specialSpawn.Y), false);
                    if (newEntity is Item i)
                    {
                        i.SetQualityLevel(specialSpawn.QualityLevel);
                    }
                }
            }
        }
        public async Task PlaceKeysAndDoors()
        {
            foreach (var specialSpawn in _generatorToUse.SpecialSpawns)
            {
                if (specialSpawn.ObjectToSpawn is EntityClass ec)
                {
                    if (ec.EntityType != EntityType.Key) continue;
                    await _map.AddEntity(ec, 1, new(specialSpawn.X, specialSpawn.Y), false);
                }
                else if (specialSpawn.ObjectToSpawn is string sid && sid.StartsWith("DoorType"))
                {
                    var doorType = sid.TrimStart("DoorType");
                    if (string.IsNullOrWhiteSpace(doorType)) continue;
                    var tile = _map.GetTileFromCoordinates(specialSpawn.X, specialSpawn.Y);
                    if(tile == null) continue;
                    tile.Type = TileType.Door;
                    tile.DoorId = doorType;
                }
            }
        }
        public async Task PlacePlayerAndKeptNPCs()
        {
            var playerSpawns = _generatorToUse.SpecialSpawns.Where(ss => ss.ObjectToSpawn is string sid && (sid == EngineConstants.SPAWN_PLAYER_CHARACTER || sid == EngineConstants.SPAWN_ANY_CHARACTER || sid == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER));
            if (playerSpawns.Any())
            {
                var pickedSpawn = playerSpawns.TakeRandomElement(Rng);
                PlayerPosition = new(pickedSpawn.X, pickedSpawn.Y);
            }
            else
            {
                PlayerPosition = _map.PickEmptyPosition(false, false);
            }
            await _map.PlacePlayer(PlayerPosition);
            foreach (var npcToKeep in _map.AICharacters)
            {
                var npcSpawns = _generatorToUse.SpecialSpawns.Where(ss => ss.ObjectToSpawn is string sid && (sid == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER || sid == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER));
                npcSpawns = npcSpawns.Where(ns => !_map.GetTileFromCoordinates(ns.X, ns.Y).IsOccupied).ToList();
                npcToKeep.Position = null;
                if (npcSpawns.Any())
                {
                    var pickedSpawn = npcSpawns.TakeRandomElement(Rng);
                    npcToKeep.Position = new(pickedSpawn.X, pickedSpawn.Y);
                }
                else
                {
                    var tilesAdjacentToPlayer = _map.GetTilesWithinCenteredSquare(PlayerPosition, 5, true);
                    tilesAdjacentToPlayer = tilesAdjacentToPlayer.Where(t => t.IsWalkable && !t.IsOccupied).ToList();
                    if (tilesAdjacentToPlayer.Any())
                    {
                        var minDistance = tilesAdjacentToPlayer.Min(t => GamePoint.Distance(t.Position, PlayerPosition));
                        tilesAdjacentToPlayer = tilesAdjacentToPlayer.Where(t => GamePoint.Distance(t.Position, PlayerPosition) == minDistance).ToList();
                        var pickedSpawn = tilesAdjacentToPlayer.TakeRandomElement(Rng);
                        npcToKeep.Position = new(pickedSpawn.Position.X, pickedSpawn.Position.Y);
                    }
                }
                if (npcToKeep.Position != null)
                {
                    _map.RegisterPreexistingCharacter(npcToKeep);
                }
            }
        }
        public void PlaceStairs()
        {
            var stairsSpawns = _generatorToUse.SpecialSpawns.Where(ss => ss.ObjectToSpawn == TileType.Stairs);
            if (stairsSpawns.Any())
            {
                var pickedSpawn = stairsSpawns.TakeRandomElement(Rng);
                _map.StairsPosition = new(pickedSpawn.X, pickedSpawn.Y);
            }
            else
            {
                _map.StairsPosition = _map.PickEmptyPosition(true, false);
            }
            if (FloorConfigurationToUse.GenerateStairsOnStart)
                _map.SetStairs();
        }

        #region Solvability
        public bool IsFloorSolvable()
        {
            if (_map.Player == null || _map.Player.Position == null || PlayerPosition == null)
                return _map.Rooms.Count > 0;
            return ArePlayerAndStairsPositionsCorrect();
        }

        public bool IsFloorSolvableWithKeys()
        {
            var availableKeyTypes = _map.Keys.Select(k => k.Name).ToList();
            var usedKeyTypes = new List<string>();
            var foundNewKeys = false;
            var islandCount = -1;
            List<List<Tile>> islands = new();
            do
            {
                islands = _map.Tiles.GetIslands(t => t.IsWalkable || usedKeyTypes.Contains(t.DoorId));
                islandCount = islands.Count;
                if (islandCount == 1) break;
                var islandWithPlayer = islands.FirstOrDefault(i => i.Contains(_map.Player.ContainingTile));
                if (islandWithPlayer == null) return false;
                var newKeys = _map.Keys.Where(k => (k.Position != null && islandWithPlayer.Contains(k.ContainingTile)) || (k.Owner != null && islandWithPlayer.Contains(k.Owner.ContainingTile))).ToList();
                foundNewKeys = newKeys.Any(k => !usedKeyTypes.Contains(k.ClassId.Replace("KeyType", "")));
                usedKeyTypes.AddRange(newKeys.Select(k => k.ClassId.Replace("KeyType", "")));
                usedKeyTypes = usedKeyTypes.Distinct().ToList();
            }
            while (foundNewKeys);
            var playerIsland = islands.FirstOrDefault(i => i.Contains(_map.Player.ContainingTile));
            return playerIsland != null && playerIsland.Any(t => t.Position.Equals(_map.StairsPosition));
        }

        public bool ArePlayerAndStairsPositionsCorrect()
        {
            var islands = _map.Tiles.GetIslands(t => t.IsWalkable);
            var playerIsland = islands.FirstOrDefault(i => i.Any(t => t.Position.Equals(PlayerPosition)));
            return playerIsland != null && playerIsland.Any(t => t.Position.Equals(_map.StairsPosition));
        }

        #endregion
    }
}

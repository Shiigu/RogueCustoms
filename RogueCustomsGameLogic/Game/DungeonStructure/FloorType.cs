﻿using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class FloorType
    {
        public readonly int MinFloorLevel;
        public readonly int MaxFloorLevel;

        public readonly int Width;
        public readonly int Height;
        public readonly bool GenerateStairsOnStart;                     // If false, some entity must have a GenerateStairs effect. 

        private readonly List<ClassInFloorInfo> PossibleMonstersInfo;
        public List<ClassInFloor> PossibleMonsters { get; private set; }
        public readonly int SimultaneousMinMonstersAtStart;
        public readonly int SimultaneousMaxMonstersInFloor;
        public int TurnsPerMonsterGeneration { get; set; }
        private readonly List<ClassInFloorInfo> PossibleItemsInfo;
        public List<ClassInFloor> PossibleItems { get; private set; }
        public readonly int MinItemsInFloor;
        public readonly int MaxItemsInFloor;
        private readonly List<ClassInFloorInfo> PossibleTrapsInfo;
        public List<ClassInFloor> PossibleTraps { get; private set; }
        public readonly int MinTrapsInFloor;
        public readonly int MaxTrapsInFloor;

        public readonly int MaxConnectionsBetweenRooms;
        public readonly int OddsForExtraConnections;
        public readonly int RoomFusionOdds;

        public readonly List<GeneratorAlgorithm> PossibleGeneratorAlgorithms;

        public readonly List<ActionWithEffects> OnFloorStartActions;

        public FloorType(FloorInfo floorInfo)
        {
            MinFloorLevel = floorInfo.MinFloorLevel;
            MaxFloorLevel = floorInfo.MaxFloorLevel;
            Width = floorInfo.Width;
            Height = floorInfo.Height;
            GenerateStairsOnStart = floorInfo.GenerateStairsOnStart;
            PossibleMonstersInfo = floorInfo.PossibleMonsters;
            SimultaneousMinMonstersAtStart = floorInfo.SimultaneousMinMonstersAtStart;
            SimultaneousMaxMonstersInFloor = floorInfo.SimultaneousMaxMonstersInFloor;
            TurnsPerMonsterGeneration = floorInfo.TurnsPerMonsterGeneration;
            PossibleItemsInfo = floorInfo.PossibleItems;
            MinItemsInFloor = floorInfo.MinItemsInFloor;
            MaxItemsInFloor = floorInfo.MaxItemsInFloor;
            PossibleTrapsInfo = floorInfo.PossibleTraps;
            MinTrapsInFloor = floorInfo.MinTrapsInFloor;
            MaxTrapsInFloor = floorInfo.MaxTrapsInFloor;
            MaxConnectionsBetweenRooms = floorInfo.MaxConnectionsBetweenRooms;
            OddsForExtraConnections = floorInfo.OddsForExtraConnections;
            RoomFusionOdds = floorInfo.RoomFusionOdds;
            PossibleGeneratorAlgorithms = new List<GeneratorAlgorithm>();
            floorInfo.PossibleGeneratorAlgorithms.ForEach(pga => PossibleGeneratorAlgorithms.Add(new GeneratorAlgorithm
            {
                Type = (GeneratorAlgorithmTypes)Enum.Parse(typeof(GeneratorAlgorithmTypes), pga.Name),
                Rows = pga.Rows,
                Columns = pga.Columns
            }));
            OnFloorStartActions = new List<ActionWithEffects>();
            MapActions(OnFloorStartActions, floorInfo.OnFloorStartActions);

            if (!PossibleGeneratorAlgorithms.Any())
                throw new InvalidDataException("There's no valid generator algorithms for the current floor");
        }

        public void FillPossibleClassLists(List<EntityClass> classList)
        {
            PossibleMonsters = new List<ClassInFloor>();
            PossibleMonstersInfo.ForEach(pmi =>
            {
                var classForMonster = classList.Find(c => c.Id.Equals(pmi.ClassId) && c.EntityType == EntityType.NPC);
                if (classForMonster == null)
                    throw new InvalidDataException($"There's no class matching for {pmi.ClassId}!");
                PossibleMonsters.Add(new ClassInFloor
                {
                    Class = classForMonster,
                    MinLevel = pmi.MinLevel,
                    MaxLevel = pmi.MaxLevel,
                    OverallMaxForKindInFloor = pmi.OverallMaxForKindInFloor,
                    SimultaneousMaxForKindInFloor = pmi.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pmi.ChanceToPick,
                    CanSpawnOnFirstTurn = pmi.CanSpawnOnFirstTurn,
                    CanSpawnAfterFirstTurn = pmi.CanSpawnAfterFirstTurn
                });
            });
            PossibleItems = new List<ClassInFloor>();
            PossibleItemsInfo.ForEach(pii =>
            {
                var classForItem = classList.Find(c => c.Id.Equals(pii.ClassId) && (c.EntityType == EntityType.Weapon || c.EntityType == EntityType.Armor || c.EntityType == EntityType.Consumable));
                if (classForItem == null)
                    throw new InvalidDataException($"There's no class matching for {pii.ClassId}!");
                PossibleItems.Add(new ClassInFloor
                {
                    Class = classForItem,
                    SimultaneousMaxForKindInFloor = pii.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pii.ChanceToPick
                });
            });
            PossibleTraps = new List<ClassInFloor>();
            PossibleTrapsInfo.ForEach(pti =>
            {
                var classForTrap = classList.Find(c => c.Id.Equals(pti.ClassId) && c.EntityType == EntityType.Trap);
                if (classForTrap == null)
                    throw new InvalidDataException($"There's no class matching for {pti.ClassId}!");
                PossibleTraps.Add(new ClassInFloor
                {
                    Class = classForTrap,
                    SimultaneousMaxForKindInFloor = pti.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pti.ChanceToPick
                });
            });
        }
        protected void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList)
        {
            actionInfoList.ForEach(aa => actionList.Add(new ActionWithEffects(aa)));
        }
    }

    [Serializable]
    public class ClassInFloor
    {
        public EntityClass Class { get; set; }

        #region Monster-only
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int OverallMaxForKindInFloor { get; set; }
        public bool CanSpawnOnFirstTurn { get; set; }
        public bool CanSpawnAfterFirstTurn { get; set; }
        #endregion
        public int TotalGeneratedInFloor { get; set; }
        public int SimultaneousMaxForKindInFloor { get; set; }
        public int ChanceToPick { get; set; }
    }
}

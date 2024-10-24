﻿using System;
using System.Collections.Generic;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class FloorInfoV12
    {
        public int MinFloorLevel { get; set; }
        public int MaxFloorLevel { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public bool GenerateStairsOnStart { get; set; }                   // If false, some entity must have a GenerateStairs effect.

        public string TileSetId { get; set; }

        public List<ClassInFloorInfoV11> PossibleMonsters { get; set; }
        public int SimultaneousMinMonstersAtStart { get; set; }
        public int SimultaneousMaxMonstersInFloor { get; set; }
        public int TurnsPerMonsterGeneration { get; set; }

        public List<ClassInFloorInfoV11> PossibleItems { get; set; }
        public int MinItemsInFloor { get; set; }
        public int MaxItemsInFloor { get; set; }

        public List<ClassInFloorInfoV11> PossibleTraps { get; set; }
        public int MinTrapsInFloor { get; set; }
        public int MaxTrapsInFloor { get; set; }

        public int MaxConnectionsBetweenRooms { get; set; }
        public int OddsForExtraConnections { get; set; }
        public int RoomFusionOdds { get; set; }
        public decimal HungerDegeneration { get; set; }

        public List<GeneratorAlgorithmInfoV11> PossibleGeneratorAlgorithms { get; set; }
        public List<ActionWithEffectsInfoV12> OnFloorStartActions { get; set; } = new List<ActionWithEffectsInfoV12>();
    }

    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

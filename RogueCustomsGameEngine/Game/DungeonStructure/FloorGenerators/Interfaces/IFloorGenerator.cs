using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators.Interfaces
{
    public interface IFloorGenerator
    {
        void CreateNormalTiles();
        void CreateSpecialTiles();
        Task PlacePlayerAndKeptNPCs();
        void PlaceStairs();
        Task PlaceEntities();
        Task PlaceKeysAndDoors();
        bool IsFloorSolvable();
        bool IsFloorSolvableWithKeys();
        bool ArePlayerAndStairsPositionsCorrect();

        bool ReadyToFloodFill { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class TileType
    {
        public static TileType Empty;
        public static TileType Floor;
        public static TileType Wall;
        public static TileType Hallway;
        public static TileType Stairs;
        public static readonly TileType Door = new ()
        {
            Id = "Door",
            Name = "Door",
            IsVisible = true,
            IsWalkable = false,
            IsSolid = true,
            CanBeTransformed = false,
            CanVisiblyConnectWithOtherTiles = false,
            CanHaveMultilineConnections = false,
            CausesPartialInvisibility = false,
        };

        public override string ToString() => Id;

        public static List<TileType> NormalTypes => [Empty, Floor, Wall, Hallway, Stairs, Door];

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSpecial => !NormalTypes.Contains(this);
        public bool IsWalkable { get; set; }
        public bool IsSolid { get; set; } // If true, it will end Line of Sight, Projectiles and Diagonal Attacks
        public bool IsVisible { get; set; }
        public bool AcceptsItems { get; set; }
        public bool CanBeTransformed { get; set; }
        public bool CanVisiblyConnectWithOtherTiles { get; set; } // If true, it will make use of TopLeft, TopRight, Central, BottomVertical, etc. ConsoleRepresentations
        public bool CanHaveMultilineConnections { get; set; }
        public bool CausesPartialInvisibility { get; set; } // If true, it will make the Character invisible to Characters in Tiles of a different Type
        public ActionWithEffects OnStood { get; set; }
        public TileTypeSet TileTypeSet { get; set; }

        public TileType() { }

        public TileType(TileTypeInfo tileTypeInfo, List<ActionSchool> actionSchools)
        {
            if (tileTypeInfo.Id.Equals("Empty"))
                Empty = this;
            else if (tileTypeInfo.Id.Equals("Floor"))
                Floor = this;
            else if (tileTypeInfo.Id.Equals("Wall"))
                Wall = this;
            else if (tileTypeInfo.Id.Equals("Hallway"))
                Hallway = this;
            else if (tileTypeInfo.Id.Equals("Stairs"))
                Stairs = this;
            Id = tileTypeInfo.Id;
            Name = tileTypeInfo.Name;
            Description = tileTypeInfo.Description;
            IsVisible = tileTypeInfo.IsVisible;
            IsWalkable = tileTypeInfo.IsWalkable;
            IsSolid = tileTypeInfo.IsSolid;
            AcceptsItems = tileTypeInfo.AcceptsItems;
            CanBeTransformed = tileTypeInfo.CanBeTransformed;
            CanVisiblyConnectWithOtherTiles = tileTypeInfo.CanVisiblyConnectWithOtherTiles;
            CanHaveMultilineConnections = tileTypeInfo.CanHaveMultilineConnections;
            CausesPartialInvisibility = tileTypeInfo.CausesPartialInvisibility;
            OnStood = ActionWithEffects.Create(tileTypeInfo.OnStood, actionSchools);
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

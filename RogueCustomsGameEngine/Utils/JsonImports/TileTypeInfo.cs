using System;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class TileTypeInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsWalkable { get; set; }
        public bool IsSolid { get; set; } // If true, it will end Line of Sight, Projectiles and Diagonal Attacks
        public bool IsVisible { get; set; }
        public bool AcceptsItems { get; set; }
        public bool CanBeTransformed { get; set; }
        public bool CanVisiblyConnectWithOtherTiles { get; set; } // If true, it will make use of TopLeft, TopRight, Central, BottomVertical, etc. ConsoleRepresentations
        public bool CanHaveMultilineConnections { get; set; }
        public ActionWithEffectsInfo OnStood { get; set; }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

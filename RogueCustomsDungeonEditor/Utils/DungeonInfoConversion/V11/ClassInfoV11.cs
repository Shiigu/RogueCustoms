using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public abstract class ClassInfoV11
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
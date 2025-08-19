using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class PlayerClassInfo : CharacterInfo
    {
        public bool RequiresNamePrompt { get; set; }
        public string InitialEquippedWeapon { get; set; }
        public string InitialEquippedArmor { get; set; }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

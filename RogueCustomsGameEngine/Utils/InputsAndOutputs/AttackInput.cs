﻿
using System;

using RogueCustomsGameEngine.Utils.Enums;
namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class AttackInput
    {
        public string SelectionId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ActionSourceType SourceType { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class StatInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StatType { get; set; }
        public bool HasMax { get; set; }
        public string RegeneratesStatId { get; set; }
        public decimal MinCap { get; set; }
        public decimal MaxCap { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

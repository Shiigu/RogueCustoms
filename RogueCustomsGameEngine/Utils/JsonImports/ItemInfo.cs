using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ItemInfo : ClassInfo
    {
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public string EntityType { get; set; }
        public List<PassiveStatModifierInfo> StatModifiers { get; set; } = new List<PassiveStatModifierInfo>();
        public ActionWithEffectsInfo OnTurnStart { get; set; }
        public List<ActionWithEffectsInfo> OnAttack { get; set; }
        public ActionWithEffectsInfo OnAttacked { get; set; }
        public ActionWithEffectsInfo OnDeath { get; set; }
        public ActionWithEffectsInfo OnUse { get; set; }
    }

    [Serializable]
    public class PassiveStatModifierInfo
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
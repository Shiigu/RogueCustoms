using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ItemInfo : ClassInfo
    {
        public bool CanBePickedUp { get; set; }
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public string EntityType { get; set; }
        public ActionWithEffectsInfo OnTurnStart { get; set; } = new ActionWithEffectsInfo();
        public List<ActionWithEffectsInfo> OnAttack { get; set; } = new List<ActionWithEffectsInfo>();
        public ActionWithEffectsInfo OnAttacked { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnDeath { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnStepped { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnUse { get; set; } = new ActionWithEffectsInfo();
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
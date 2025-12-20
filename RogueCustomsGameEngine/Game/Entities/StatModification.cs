using System;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class StatModification
    {
        // Indicates which action, item or whatever created this alteration. Used for "CanBeStacked" checks.
        public string Id { get; set; }
        public string Source { get; set; }

        public decimal Amount { get; set; }

        // If RemainingTurns = -1, the effect is permanent until erased
        public int RemainingTurns { get; set; }

        public bool IsBuff => Amount > 0;
        public bool IsDebuff => Amount < 0;

        public bool InformOfExpiration { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

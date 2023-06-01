namespace RoguelikeGameEngine.Game.Entities
{
    public class StatModification
    {
        // TODO: Include stat name for when the stat themselves become customizable

        // Indicates which action, item or whatever created this alteration. Used for "CanBeStacked" checks.
        public string Id { get; set; }

        public decimal Amount { get; set; }

        // If RemainingTurns = -1, the effect is permanent until erased
        public int RemainingTurns { get; set; }

        public bool IsBuff => Amount > 0;
        public bool IsDebuff => Amount < 0;
    }
}

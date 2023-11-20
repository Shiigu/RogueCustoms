namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface IHasActions
    {
        void PerformOnTurnStart();
        void RefreshCooldownsAndUpdateTurnLength();
    }
}

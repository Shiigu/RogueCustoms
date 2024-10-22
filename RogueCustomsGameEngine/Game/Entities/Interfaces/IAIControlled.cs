using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface IAIControlled
    {
        void ProcessAI();
        void TryToMoveToTarget();
        void UpdateKnownCharacterList();
    }
}

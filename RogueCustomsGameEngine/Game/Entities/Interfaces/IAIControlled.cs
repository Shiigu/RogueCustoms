using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface IAIControlled
    {
        void PickTargetAndPath();
        void AttackOrMove();
        void MoveTo(GamePoint p);
        void UpdateKnownCharacterList();
    }
}

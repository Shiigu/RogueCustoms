using RoguelikeGameEngine.Utils.Representation;

namespace RoguelikeGameEngine.Game.Entities.Interfaces
{
    public interface IAIControlled
    {
        void PickTargetAndPath();
        void AttackOrMove();
        void MoveTo(Point p);
        void UpdateKnownCharacterList();
    }
}

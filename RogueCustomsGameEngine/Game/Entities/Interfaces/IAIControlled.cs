using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface IAIControlled
    {
        Task ProcessAI();
        Task TryToMoveToTarget();
        void UpdateKnownCharacterList();
    }
}

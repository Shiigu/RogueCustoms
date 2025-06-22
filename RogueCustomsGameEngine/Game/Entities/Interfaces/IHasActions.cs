using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface IHasActions
    {
        Task PerformOnTurnStart();
        Task RefreshCooldownsAndUpdateTurnLength();
    }
}

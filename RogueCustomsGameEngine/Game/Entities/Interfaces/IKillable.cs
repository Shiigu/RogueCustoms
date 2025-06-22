using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface IKillable
    {
        Task Die(Entity? attacker = null);
    }
}

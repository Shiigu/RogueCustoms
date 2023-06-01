namespace RoguelikeGameEngine.Game.Entities.Interfaces
{
    public interface IKillable
    {
        void Die(Entity? attacker = null);
        void TryRegenerateHP();
    }
}

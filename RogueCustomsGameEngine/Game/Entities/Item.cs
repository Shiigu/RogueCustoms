using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public class Item : Entity, IHasActions
    {
        public bool CanBePickedUp { get; set; }
        public string Power { get; set; }
        public Character Owner { get; set; }
        public ActionWithEffects OnStepped { get; set; }
        public ActionWithEffects OnUse { get; set; }

        public Item(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            Power = entityClass.Power;
            CanBePickedUp = entityClass.CanBePickedUp;
            Owner = null;
            OnStepped = MapClassAction(entityClass.OnStepped);
            if (entityClass.EntityType != EntityType.Trap)
                OnUse = MapClassAction(entityClass.OnUse);
            if (entityClass.EntityType != EntityType.Trap)
                OwnOnTurnStart = MapClassAction(entityClass.OnTurnStart);
            if (entityClass.EntityType != EntityType.Trap)
                MapClassActions(entityClass.OnAttack, OwnOnAttack);
            if (entityClass.EntityType != EntityType.Trap)
                OwnOnAttacked = MapClassAction(entityClass.OnAttacked);
        }

        public void Stepped(Entity stomper)
        {
            OnStepped?.Do(this, stomper);
        }
        public void Used(Entity user)
        {
            OnUse?.Do(this, user);
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            OwnOnAttack?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            if (OwnOnAttacked?.CooldownBetweenUses > 0 && OwnOnAttacked?.CurrentCooldown > 0)
                OwnOnAttacked.CurrentCooldown--;
            if (OwnOnTurnStart?.CooldownBetweenUses > 0 && OwnOnTurnStart?.CurrentCooldown > 0)
                OwnOnTurnStart.CurrentCooldown--;
            if (OnStepped?.CooldownBetweenUses > 0 && OnStepped?.CurrentCooldown > 0)
                OnStepped.CurrentCooldown--;
            if (OnUse?.CooldownBetweenUses > 0 && OnUse?.CurrentCooldown > 0)
                OnUse.CurrentCooldown--;
        }

        public void PerformOnTurnStartActions()
        {
            if(OwnOnTurnStart != null && Owner != null && OwnOnTurnStart.CanBeUsedOn(Owner, Map))
                OwnOnTurnStart?.Do(this, Owner);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

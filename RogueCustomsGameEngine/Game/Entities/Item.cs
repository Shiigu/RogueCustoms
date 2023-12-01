using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using static System.Collections.Specialized.BitVector32;
using RogueCustomsGameEngine.Utils;
using MathNet.Numerics.Statistics.Mcmc;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class Item : Entity, IHasActions
    {
        public bool IsEquippable => EntityType == EntityType.Weapon || EntityType == EntityType.Armor;
        public bool CanBePickedUp { get; set; }
        public string Power { get; set; }
        public Faction Faction { get; set; }                // Exclusively used to allow Traps to be visible only to certain characters.
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
            {
                OnUse = MapClassAction(entityClass.OnUse);
                OwnOnDeath = MapClassAction(entityClass.OnDeath);
                OwnOnTurnStart = MapClassAction(entityClass.OnTurnStart);
                MapClassActions(entityClass.OnAttack, OwnOnAttack);
                OwnOnAttacked = MapClassAction(entityClass.OnAttacked);
            }
        }

        public void Stepped(Character stomper)
        {
            var successfulEffects = OnStepped?.Do(this, stomper, true);
            if (successfulEffects != null && Constants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                stomper.AttackedBy(null);
        }
        public void Used(Entity user)
        {
            OnUse?.Do(this, user, true);
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

        public void PerformOnTurnStart()
        {
            if(OwnOnTurnStart != null && Owner != null && OwnOnTurnStart.ChecksCondition(Owner, Owner))
                OwnOnTurnStart?.Do(this, Owner, true);
        }

        public bool CanBeSeenBy(Character c)
        {
            if (c == null) return false;
            if (c.CanSee(this)) return true;
            if (!Visible && Faction != null && c.FOVTiles.Contains(ContainingTile))
            {
                if (Faction == c.Faction) return true;
                if (Faction.AlliedWith.Contains(c.Faction)) return true;
            }
            return false;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

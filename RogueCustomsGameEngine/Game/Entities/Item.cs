using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using static System.Collections.Specialized.BitVector32;
using RogueCustomsGameEngine.Utils;
using MathNet.Numerics.Statistics.Mcmc;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Drawing;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class Item : Entity, IHasActions, IPickable
    {
        public bool IsEquippable => EntityType == EntityType.Weapon || EntityType == EntityType.Armor;
        public string Power { get; set; }
        public List<PassiveStatModifier> StatModifiers { get; set; }

        public Character Owner { get; set; }
        public ActionWithEffects OnUse { get; set; }

        public Item(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            Power = entityClass.Power;
            Owner = null;
            OnUse = MapClassAction(entityClass.OnUse);
            OwnOnDeath = MapClassAction(entityClass.OnDeath);
            OwnOnTurnStart = MapClassAction(entityClass.OnTurnStart);
            MapClassActions(entityClass.OnAttack, OwnOnAttack);
            OwnOnAttacked = MapClassAction(entityClass.OnAttacked);
            StatModifiers = new List<PassiveStatModifier>(entityClass.StatModifiers);
        }
        public void Used(Entity user)
        {
            OnUse?.Do(this, user, true);

            if (user == Map.Player)
                Map.DisplayEvents.Add(($"{user.Name} used item", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemUse }
                    }
                }
                ));
            else if (user.EntityType == EntityType.NPC)
                Map.DisplayEvents.Add(($"{user.Name} used item", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.NPCItemUse }
                    }
                }
                ));
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            OwnOnAttack?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            if (OwnOnAttacked?.CooldownBetweenUses > 0 && OwnOnAttacked?.CurrentCooldown > 0)
                OwnOnAttacked.CurrentCooldown--;
            if (OwnOnTurnStart?.CooldownBetweenUses > 0 && OwnOnTurnStart?.CurrentCooldown > 0)
                OwnOnTurnStart.CurrentCooldown--;
            if (OnUse?.CooldownBetweenUses > 0 && OnUse?.CurrentCooldown > 0)
                OnUse.CurrentCooldown--;
        }

        public void PerformOnTurnStart()
        {
            if(OwnOnTurnStart != null && Owner != null && OwnOnTurnStart.ChecksCondition(Owner, Owner))
                OwnOnTurnStart?.Do(this, Owner, true);
        }

        public override void SetActionIds()
        {
            for (int i = 0; i < OwnOnAttack.Count; i++)
                OwnOnAttack[i].SelectionId = $"{Id}_{ClassId}_IA{i}_{OwnOnAttack[i].Id}";
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

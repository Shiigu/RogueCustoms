using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class AlteredStatus : Entity, IHasActions
    {
        private readonly EntityClass Class;
        public Character Target { get; set; }

        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }

        public bool FlaggedToRemove { get; set; }

        public int RemainingTurns { get; set; }

        public int TurnLength { get; set; }

        public decimal Power { get; set; }

        public ActionWithEffects BeforeAttack { get; set; }

        public ActionWithEffects OnApply { get; set; }

        public ActionWithEffects OnRemove { get; set; }

        public AlteredStatus(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            FlaggedToRemove = false;
            Class = entityClass;
            CanStack = entityClass.CanStack;
            CanOverwrite = entityClass.CanOverwrite;
            CleanseOnFloorChange = entityClass.CleanseOnFloorChange;
            CleansedByCleanseActions = entityClass.CleansedByCleanseActions;
            BeforeAttack = MapClassAction(entityClass.BeforeAttack);
            OwnOnAttacked = MapClassAction(entityClass.OnAttacked);
            OnApply = MapClassAction(entityClass.OnApply);
            OnRemove = MapClassAction(entityClass.OnRemove);
            OwnOnTurnStart = MapClassAction(entityClass.OnTurnStart);
        }

        public bool ApplyTo(Character target, decimal power, int turnLength)
        {
            if (!CanOverwrite && !CanStack && target.AlteredStatuses.Exists(als => als.ClassId.Equals(ClassId))) return false;
            if (CanOverwrite && !CanStack && target.AlteredStatuses.Exists(als => als.ClassId.Equals(ClassId)))
            {
                target.AlteredStatuses.RemoveAll(als => als.ClassId.Equals(ClassId));
            }
            var alteredStatusInstance = Clone();
            alteredStatusInstance.Power = power;
            alteredStatusInstance.Target = target;
            alteredStatusInstance.TurnLength = turnLength;
            alteredStatusInstance.RemainingTurns = turnLength;
            target.AlteredStatuses.Add(alteredStatusInstance);
            alteredStatusInstance.OnApply?.Do(this, target, false);
            return true;
        }

        private new AlteredStatus Clone()
        {
            return new AlteredStatus(Class, Map);
        }

        public override string ToString() => $"{Name} ({Description}) - {TurnLength - RemainingTurns} turns left";

        public void PerformOnTurnStart()
        {
            if (Target == null) return;
            if (OwnOnTurnStart?.ChecksCondition(Target, Target) == true)
                OwnOnTurnStart.Do(this, Target, false);
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            if (OwnOnTurnStart?.CooldownBetweenUses > 0 && OwnOnTurnStart?.CurrentCooldown > 0)
                OwnOnTurnStart.CurrentCooldown--;
            if (RemainingTurns > 0)
                RemainingTurns--;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

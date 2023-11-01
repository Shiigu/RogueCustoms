using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsGameEngine.Game.Entities
{
    public class AlteredStatus : Entity, IHasActions
    {
        private readonly EntityClass Class;
        public Character Target { get; set; }

        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }

        public int RemainingTurns { get; set; }

        public int TurnLength { get; set; }

        public decimal Power { get; set; }

        public ActionWithEffects OnApply { get; set; }

        public AlteredStatus(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            Class = entityClass;
            CanStack = entityClass.CanStack;
            CanOverwrite = entityClass.CanOverwrite;
            CleanseOnFloorChange = entityClass.CleanseOnFloorChange;
            CleansedByCleanseActions = entityClass.CleansedByCleanseActions;
            OnApply = MapClassAction(entityClass.OnApply);
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
            alteredStatusInstance.OnApply?.Do(this, target);
            return true;
        }

        private AlteredStatus Clone()
        {
            return new AlteredStatus(Class, Map);
        }

        public string ToString() => $"{Name} ({Description}) - {TurnLength - RemainingTurns} turns left";

        public void PerformOnTurnStartActions()
        {
            if (Target == null) return;
            if (OwnOnTurnStart?.MayBeUsed == true)
                OwnOnTurnStart.Do(this, Target);
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            if (OwnOnTurnStart?.CooldownBetweenUses > 0 && OwnOnTurnStart?.CurrentCooldown > 0)
                OwnOnTurnStart.CurrentCooldown--;
            if(RemainingTurns > 0)
                RemainingTurns--;
        }
    }
}

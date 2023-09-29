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

        public List<ActionWithEffects> OnStatusApplyActions { get; set; }

        public AlteredStatus(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            Class = entityClass;
            CanStack = entityClass.CanStack;
            CanOverwrite = entityClass.CanOverwrite;
            CleanseOnFloorChange = entityClass.CleanseOnFloorChange;
            CleansedByCleanseActions = entityClass.CleansedByCleanseActions;
            OnStatusApplyActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnStatusApplyActions, OnStatusApplyActions);
            MapClassActions(entityClass.OnTurnStartActions, OwnOnTurnStartActions);
        }

        public bool ApplyTo(Character target, decimal power, int turnLength)
        {
            if (!CanOverwrite && !CanStack && target.AlteredStatuses.Any(als => als.ClassId.Equals(ClassId))) return false;
            if (CanOverwrite && !CanStack && target.AlteredStatuses.Any(als => als.ClassId.Equals(ClassId)))
            {
                target.AlteredStatuses.RemoveAll(als => als.ClassId.Equals(ClassId));
            }
            var alteredStatusInstance = Clone();
            alteredStatusInstance.Power = power;
            alteredStatusInstance.Target = target;
            alteredStatusInstance.TurnLength = turnLength;
            alteredStatusInstance.RemainingTurns = turnLength;
            target.AlteredStatuses.Add(alteredStatusInstance);
            alteredStatusInstance.OnStatusApplyActions.ForEach(osaa => osaa.Do(this, target));
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
            OwnOnTurnStartActions?.Where(a => a.MayBeUsed).ForEach(a => a.Do(this, Target));
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            OwnOnTurnStartActions?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            if(RemainingTurns > 0)
                RemainingTurns--;
        }
    }
}

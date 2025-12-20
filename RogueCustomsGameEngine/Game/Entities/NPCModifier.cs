using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities
{
    [Serializable]
    public class NPCModifier
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GameColor NameColor { get; set; }
        public List<PassiveStatModifier> StatModifiers { get; set; }
        public ExtraDamage ExtraDamage { get; set; }
        public ActionWithEffects OwnOnSpawn { get; set; }
        public ActionWithEffects OwnOnTurnStart { get; set; }
        public ActionWithEffects OwnOnAttack { get; set; }
        public ActionWithEffects OwnOnAttacked { get; set; }
        public ActionWithEffects OwnOnDeath { get; set; }

        public NPCModifier(NPCModifierInfo info, Locale locale, List<Element> elements, List<ActionSchool> actionSchools)
        {
            Id = info.Id;
            Name = locale[info.Name];
            NameColor = info.NameColor;
            StatModifiers = [];
            if (info.StatModifiers != null)
            {
                foreach (var statModifier in info.StatModifiers)
                {
                    StatModifiers.Add(new PassiveStatModifier
                    {
                        Id = statModifier.Id,
                        Source = Name,
                        Amount = statModifier.Amount
                    });
                }
            }
            if (info.ExtraDamage != null && !string.IsNullOrWhiteSpace(info.ExtraDamage.Element))
            {
                ExtraDamage = new ExtraDamage
                {
                    MinimumDamage = info.ExtraDamage.MinDamage,
                    MaximumDamage = info.ExtraDamage.MaxDamage,
                    Element = elements.Find(e => e.Id.Equals(info.ExtraDamage.Element, StringComparison.InvariantCultureIgnoreCase))
                };
            }
            OwnOnSpawn = ActionWithEffects.Create(info.OnSpawn, actionSchools);
            OwnOnTurnStart = ActionWithEffects.Create(info.OnTurnStart, actionSchools);
            OwnOnAttack = ActionWithEffects.Create(info.OnAttack, actionSchools);
            OwnOnAttacked = ActionWithEffects.Create(info.OnAttacked, actionSchools);
            OwnOnDeath = ActionWithEffects.Create(info.OnDeath, actionSchools);
        }

        private NPCModifier()
        {
            // Do nothing, just here for Clone
        }

        private NPCModifier Clone()
        {
            return new NPCModifier()
            {
                Id = Id,
                Name = Name,
                NameColor = NameColor.Clone(),
                StatModifiers = StatModifiers.ConvertAll(sm => new PassiveStatModifier { Id = sm.Id, Source = sm.Source, Amount = sm.Amount }),
                ExtraDamage = ExtraDamage == null ? null : new ExtraDamage
                {
                    MinimumDamage = ExtraDamage.MinimumDamage,
                    MaximumDamage = ExtraDamage.MaximumDamage,
                    Element = ExtraDamage.Element
                },
                OwnOnSpawn = OwnOnSpawn?.Clone(),
                OwnOnTurnStart = OwnOnTurnStart?.Clone(),
                OwnOnAttack = OwnOnAttack?.Clone(),
                OwnOnAttacked = OwnOnAttacked?.Clone(),
                OwnOnDeath = OwnOnDeath?.Clone(),
            };
        }

        public void ApplyTo(NonPlayableCharacter npc)
        {
            var clonedModifier = Clone();
            if (clonedModifier.OwnOnSpawn != null)
            {
                clonedModifier.OwnOnSpawn.Map = npc.Map;
                clonedModifier.OwnOnSpawn.User = npc;
            }
            if (clonedModifier.OwnOnTurnStart != null)
            {
                clonedModifier.OwnOnTurnStart.Map = npc.Map;
                clonedModifier.OwnOnTurnStart.User = npc;
            }
            if (clonedModifier.OwnOnAttack != null)
            {
                clonedModifier.OwnOnAttack.Map = npc.Map;
                clonedModifier.OwnOnAttack.User = npc;
            }
            if (clonedModifier.OwnOnAttacked != null)
            { 
                clonedModifier.OwnOnAttacked.Map = npc.Map;
                clonedModifier.OwnOnAttacked.User = npc;
            }
            if (clonedModifier.OwnOnDeath != null)
            {
                clonedModifier.OwnOnDeath.Map = npc.Map;
                clonedModifier.OwnOnDeath.User = npc;
            }
            npc.Modifiers.Add(clonedModifier);
        }
    }
}

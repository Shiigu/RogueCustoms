using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.Entities
{
    [Serializable]
    public class Affix
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AffixType Type { get; set; }
        public int MinimumItemLevel { get; set; }
        public int ItemValueModifierPercentage { get; set; }
        public List<EntityType> AffectedItemTypes { get; set; }
        public List<PassiveStatModifier> StatModifiers { get; set; }
        public ExtraDamage ExtraDamage { get; set; }
        public ActionWithEffects OwnOnTurnStart { get; set; }
        public ActionWithEffects OwnOnAttack { get; set; }
        public ActionWithEffects OwnOnAttacked { get; set; }

        public Affix(AffixInfo info, Locale locale, List<Element> elements, List<ActionSchool> actionSchools)
        {
            Id = info.Id;
            Name = locale[info.Name];
            Type = Enum.Parse<AffixType>(info.AffixType);
            MinimumItemLevel = info.MinimumItemLevel;
            ItemValueModifierPercentage = info.ItemValueModifierPercentage;
            AffectedItemTypes = info.AffectedItemTypes.ConvertAll(Enum.Parse<EntityType>);
            StatModifiers = [];
            if (info.StatModifiers != null)
            {
                foreach (var statModifier in info.StatModifiers)
                {
                    StatModifiers.Add(new PassiveStatModifier
                    {
                        Id = statModifier.Id,
                        Amount = statModifier.Amount
                    });
                }
            }
            if (info.ExtraDamage != null)
            {
                ExtraDamage = new ExtraDamage
                {
                    MinimumDamage = info.ExtraDamage.MinDamage,
                    MaximumDamage = info.ExtraDamage.MaxDamage,
                    Element = elements.Find(e => e.Id.Equals(info.ExtraDamage.Element, StringComparison.InvariantCultureIgnoreCase))
                };
            }
            OwnOnTurnStart = ActionWithEffects.Create(info.OnTurnStart, actionSchools);
            OwnOnAttack = ActionWithEffects.Create(info.OnAttack, actionSchools);
            OwnOnAttacked = ActionWithEffects.Create(info.OnAttacked, actionSchools);
        }

        private Affix() 
        {
            // Do nothing, just here for Clone
        }

        private Affix Clone()
        {
            return new Affix()
            {
                Id = Id,
                Name = Name,
                Type = Type,
                MinimumItemLevel = MinimumItemLevel,
                ItemValueModifierPercentage = ItemValueModifierPercentage,
                AffectedItemTypes = new List<EntityType>(AffectedItemTypes),
                StatModifiers = StatModifiers.ConvertAll(sm => new PassiveStatModifier { Id = sm.Id, Amount = sm.Amount }),
                ExtraDamage = ExtraDamage == null ? null : new ExtraDamage
                {
                    MinimumDamage = ExtraDamage.MinimumDamage,
                    MaximumDamage = ExtraDamage.MaximumDamage,
                    Element = ExtraDamage.Element
                },
                OwnOnTurnStart = OwnOnTurnStart?.Clone(),
                OwnOnAttack = OwnOnAttack?.Clone(),
                OwnOnAttacked = OwnOnAttacked?.Clone()
            };
        }

        public void ApplyTo(Item item)
        {
            item.Affixes.Add(Clone());
        }
    }
}

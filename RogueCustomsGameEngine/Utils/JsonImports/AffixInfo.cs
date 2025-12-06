using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class AffixInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AffixType { get; set; }
        public int RequiredPlayerLevel { get; set; }
        public int MinimumItemLevel { get; set; }
        public int ItemValueModifierPercentage { get; set; }
        public List<string> AffectedItemTypes { get; set; }
        public List<PassiveStatModifierInfo> StatModifiers { get; set; } = new List<PassiveStatModifierInfo>();
        public ActionWithEffectsInfo OnTurnStart { get; set; }
        public ActionWithEffectsInfo OnAttacked { get; set; }
        public ActionWithEffectsInfo OnAttack { get; set; }
        public ExtraDamageInfo ExtraDamage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class NPCModifierInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GameColor NameColor { get; set; }
        public List<PassiveStatModifierInfo> StatModifiers { get; set; } = new List<PassiveStatModifierInfo>();
        public ActionWithEffectsInfo OnSpawn { get; set; }
        public ActionWithEffectsInfo OnTurnStart { get; set; }
        public ActionWithEffectsInfo OnAttack { get; set; }
        public ActionWithEffectsInfo OnAttacked { get; set; }
        public ActionWithEffectsInfo OnDeath { get; set; }
        public ExtraDamageInfo ExtraDamage { get; set; }
    }
}

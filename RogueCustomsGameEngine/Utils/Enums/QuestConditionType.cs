using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Enums
{
    [Serializable]
    public enum QuestConditionType
    {
        KillNPCs,
        DealDamage,
        HealDamage,
        StatusNPCs,
        StatusSelf,
        CollectItems,
        UseItems,
        ReachFloor,
        ReachLevel,
        ObtainCurrency
    }
}

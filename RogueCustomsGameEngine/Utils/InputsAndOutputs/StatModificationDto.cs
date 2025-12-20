using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    [Serializable]
    public class StatModificationDto
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsPermanent { get; set; }

        public StatModificationDto() { }

        // For Active Modifications
        public StatModificationDto(StatModification source, StatDto stat, Map map)
        {
            Source = map.Locale[source.Source];
            if (stat.IsDecimalStat)
                Amount = source.Amount;
            else
                Amount = (int)source.Amount;
            IsPercentage = stat.IsPercentileStat;
        }

        // For Passive Modifications
        public StatModificationDto(string source, decimal amount, StatDto stat, bool isPermanent, Map map)
        {
            Source = source;
            if (stat.IsDecimalStat)
                Amount = amount;
            else
                Amount = (int)amount;
            IsPercentage = stat.IsPercentileStat;
            IsPermanent = isPermanent;
        }

        // For Item Details
        public StatModificationDto(PassiveStatModifier source, Stat stat, Map map)
        {
            Name = stat.Name;
            if (stat.StatType == StatType.Regeneration || stat.StatType == StatType.Decimal)
                Amount = source.Amount;
            else
                Amount = (int)source.Amount;
            IsPercentage = stat.StatType == StatType.Percentage;
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

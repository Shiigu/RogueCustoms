using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class PlayerInfoDto
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public bool IsAtMaxLevel { get; set; }
        public int CurrentExperience { get; set; }
        public int ExperienceToNextLevel { get; set; }

        public List<StatDto> Stats { get; set; }

        public List<AlteredStatusDetailDto> AlteredStatuses { get; set; }

        public ItemDetailDto WeaponInfo { get; set; }
        public ItemDetailDto ArmorInfo { get; set; }

        public PlayerInfoDto() { }

        public PlayerInfoDto(PlayerCharacter character, Map map)
        {
            Name = character.Name;
            Level = character.Level;
            IsAtMaxLevel = character.Level == character.MaxLevel;
            if (!IsAtMaxLevel)
            {
                CurrentExperience = character.Experience;
                ExperienceToNextLevel = character.ExperienceToLevelUpDifference;
            }
            Stats = new();
            foreach (var stat in character.Stats)
            {
                var maxStatName = stat.StatType switch
                {
                    StatType.HP => map.Locale["CharacterMaxHPStat"],
                    StatType.MP => map.Locale["CharacterMaxMPStat"],
                    _ => string.Empty
                };
                var statIsVisible = true;
                if (stat.StatType == StatType.MP && !character.UsesMP)
                    statIsVisible = false;
                if (stat.StatType == StatType.Hunger && !character.UsesHunger)
                    statIsVisible = false;
                var statInfo = new StatDto
                {
                    Name = stat.Name,
                    MaxName = maxStatName,
                    Current = stat.Current,
                    Max = stat.BaseAfterModifications,
                    Base = stat.Base + (int)(stat.IncreasePerLevel * (character.Level - 1)),
                    IsDecimalStat = stat.IsDecimal,
                    IsPercentileStat = stat.StatType == StatType.Accuracy || stat.StatType == StatType.Evasion || stat.StatType == StatType.CustomPercentage,
                    HasMaxStat = stat.HasMax,
                    Visible = statIsVisible,
                    Modifications = new List<StatModificationDto>()
                };
                stat.Modifications.Where(m => m.RemainingTurns != 0).ForEach(m => statInfo.Modifications.Add(new StatModificationDto(m, statInfo, map)));
                Stats.Add(statInfo);
            }
            AlteredStatuses = new List<AlteredStatusDetailDto>();
            character.AlteredStatuses.ForEach(als => AlteredStatuses.Add(new AlteredStatusDetailDto(als)));
            WeaponInfo = new ItemDetailDto(character.Weapon);
            ArmorInfo = new ItemDetailDto(character.Armor);
        }
    }

    [Serializable]
    public class StatDto
    {
        public string Name { get; set; }
        public string MaxName { get; set; }
        public decimal Current { get; set; }
        public decimal? Max { get; set; }
        public decimal Base { get; set; }
        public bool IsDecimalStat { get; set; }
        public bool IsPercentileStat { get; set; }
        public bool HasMaxStat { get; set; }
        public bool Visible { get; set; }
        public List<StatModificationDto> Modifications { get; set; }
    }

    [Serializable]
    public class StatModificationDto
    {
        public string Source { get; set; }
        public decimal Amount { get; set; }

        public StatModificationDto() { }

        public StatModificationDto(StatModification source, StatDto stat, Map map)
        {
            Source = map.Locale[source.Id];
            if(stat.IsDecimalStat)
                Amount = source.Amount;
            else
                Amount = (int)source.Amount;
        }
    }

    [Serializable]
    public class AlteredStatusDetailDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RemainingTurns { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public AlteredStatusDetailDto() { }

        public AlteredStatusDetailDto(AlteredStatus als)
        {
            Name = als.Name;
            Description = als.Description;
            RemainingTurns = als.RemainingTurns;
            ConsoleRepresentation = als.ConsoleRepresentation;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

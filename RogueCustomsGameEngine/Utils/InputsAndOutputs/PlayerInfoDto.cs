using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;

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
        public int CurrencyCarried { get; set; }

        public List<StatDto> Stats { get; set; }

        public List<AlteredStatusDetailDto> AlteredStatuses { get; set; }

        public List<SimpleInventoryDto> Equipment { get; set; }
        public List<SimpleInventoryDto> Inventory { get; set; }

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
            CurrencyCarried = character.CurrencyCarried;
            Stats = new();
            foreach (var stat in character.UsedStats)
            {
                if (stat.Id == "HungerDegeneration") continue;
                var maxStatName = stat.StatType switch
                {
                    StatType.HP => map.Locale["CharacterMaxHPStat"],
                    StatType.MP => map.Locale["CharacterMaxMPStat"],
                    _ => string.Empty
                };
                var statInfo = new StatDto
                {
                    Name = stat.Name,
                    MaxName = maxStatName,
                    Current = stat.Current,
                    Max = stat.BaseAfterModifications,
                    Base = stat.BaseAfterLevelUp,
                    IsDecimalStat = stat.StatType == StatType.Regeneration || stat.StatType == StatType.Decimal,
                    IsPercentileStat = stat.StatType == StatType.Percentage,
                    HasMaxStat = stat.HasMax,
                    IsHP = stat.StatType == StatType.HP,
                    IsMinimized = stat.HasMax ? stat.Base <= stat.MinCap : stat.Current <= stat.MinCap,
                    IsMaxedOut = stat.HasMax ? stat.Base >= stat.MaxCap : stat.Current >= stat.MaxCap,
                    Modifications = new List<StatModificationDto>()
                };
                stat.ActiveModifications.Where(m => m.RemainingTurns != 0).ForEach(m => statInfo.Modifications.Add(new StatModificationDto(m, statInfo, map)));
                stat.PermanentPassiveModifications.ForEach(pm => statInfo.Modifications.Add(new StatModificationDto(pm.Source, pm.Amount, statInfo, true, map)));
                stat.ItemPassiveModifications.ForEach(pm => statInfo.Modifications.Add(new StatModificationDto(pm.Source, pm.Amount, statInfo, false, map)));
                Stats.Add(statInfo);
            }
            Equipment = new List<SimpleInventoryDto>();
            character.Equipment.OrderBy(item => map.Player.AvailableSlots.IndexOf(item.SlotsItOccupies[0])).ForEach(i => Equipment.Add(new SimpleInventoryDto(i)));
            AlteredStatuses = new List<AlteredStatusDetailDto>();
            character.AlteredStatuses.ForEach(als => AlteredStatuses.Add(new AlteredStatusDetailDto(als)));
            Inventory = new List<SimpleInventoryDto>();
            character.Inventory.ForEach(i => Inventory.Add(new SimpleInventoryDto(i)));
            character.KeySet.ForEach(i => Inventory.Add(new SimpleInventoryDto(i)));
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
        public bool IsHP { get; set; }
        public bool IsMinimized { get; set; }
        public bool IsMaxedOut { get; set; }
        public List<StatModificationDto> Modifications { get; set; }
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

    [Serializable]
    public class SimpleInventoryDto
    {
        public string Name { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public GameColor QualityLevelColor { get; set; }

        public SimpleInventoryDto(Key k)
        {
            Name = k.Name;
            ConsoleRepresentation = k.ConsoleRepresentation;
            QualityLevelColor = new(Color.White);
        }
        public SimpleInventoryDto(Item i)
        {
            Name = i.Name;
            ConsoleRepresentation = i.ConsoleRepresentation;
            QualityLevelColor = i.QualityLevel.ItemNameColor;
        }
    }

#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

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
                    Base = stat.Base + (int)(stat.IncreasePerLevel * (character.Level - 1)),
                    IsDecimalStat = stat.StatType == StatType.Regeneration || stat.StatType == StatType.Decimal,
                    IsPercentileStat = stat.StatType == StatType.Percentage,
                    HasMaxStat = stat.HasMax,
                    Modifications = new List<StatModificationDto>()
                };
                stat.ActiveModifications.Where(m => m.RemainingTurns != 0).ForEach(m => statInfo.Modifications.Add(new StatModificationDto(m, statInfo, map)));
                stat.PassiveModifications.ForEach(pm => statInfo.Modifications.Add(new StatModificationDto(pm.Source, pm.Amount, statInfo, map)));
                Stats.Add(statInfo);
            }
            AlteredStatuses = new List<AlteredStatusDetailDto>();
            character.AlteredStatuses.ForEach(als => AlteredStatuses.Add(new AlteredStatusDetailDto(als)));
            WeaponInfo = new ItemDetailDto(character.Weapon);
            ArmorInfo = new ItemDetailDto(character.Armor);
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

        public SimpleInventoryDto(Key k)
        {
            Name = k.Name;
            ConsoleRepresentation = k.ConsoleRepresentation;
        }
        public SimpleInventoryDto(Item i)
        {
            Name = i.Name;
            ConsoleRepresentation = i.ConsoleRepresentation;
        }
    }

#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

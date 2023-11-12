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
            var hpStat = new StatDto()
            {
                Name = map.Locale["CharacterHPStat"],
                MaxName = map.Locale["CharacterMaxHPStat"],
                Current = character.HP,
                Max = character.MaxHP,
                Base = character.BaseMaxHP + (int)(character.MaxHPIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = false,
                IsPercentileStat = false,
                HasMaxStat = true,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.MaxHPModifications.Where(m => m.RemainingTurns != 0).ForEach(hpm => hpStat.Modifications.Add(new StatModificationDto(hpm, hpStat)));
            var mpStat = new StatDto()
            {
                Name = map.Locale["CharacterMPStat"],
                MaxName = map.Locale["CharacterMaxMPStat"],
                Current = character.MP,
                Max = character.MaxMP,
                Base = character.BaseMaxMP + (int)(character.MaxMPIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = false,
                IsPercentileStat = false,
                HasMaxStat = true,
                Visible = character.UsesMP,
                Modifications = new List<StatModificationDto>()
            };
            character.MaxMPModifications.Where(m => m.RemainingTurns != 0).ForEach(mpm => mpStat.Modifications.Add(new StatModificationDto(mpm, mpStat)));
            var attackStat = new StatDto()
            {
                Name = map.Locale["CharacterAttackStat"],
                Current = character.Attack,
                Base = character.BaseAttack + (int)(character.AttackIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = false,
                IsPercentileStat = false,
                HasMaxStat = false,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.AttackModifications.Where(m => m.RemainingTurns != 0).ForEach(am => attackStat.Modifications.Add(new StatModificationDto(am, attackStat)));
            var defenseStat = new StatDto()
            {
                Name = map.Locale["CharacterDefenseStat"],
                Current = character.Defense,
                Base = character.BaseDefense + (int)(character.DefenseIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = false,
                IsPercentileStat = false,
                HasMaxStat = false,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.DefenseModifications.Where(m => m.RemainingTurns != 0).ForEach(dm => defenseStat.Modifications.Add(new StatModificationDto(dm, defenseStat)));
            var movementStat = new StatDto()
            {
                Name = map.Locale["CharacterMovementStat"],
                Current = character.Movement,
                Base = character.BaseMovement + (int)(character.MovementIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = false,
                IsPercentileStat = false,
                HasMaxStat = false,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.MovementModifications.Where(m => m.RemainingTurns != 0).ForEach(mm => movementStat.Modifications.Add(new StatModificationDto(mm, movementStat)));
            var hpRegenerationStat = new StatDto()
            {
                Name = map.Locale["CharacterHPRegenerationStat"],
                Current = character.HPRegeneration,
                Base = character.BaseHPRegeneration + (character.HPRegenerationIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = true,
                IsPercentileStat = false,
                HasMaxStat = false,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.HPRegenerationModifications.Where(m => m.RemainingTurns != 0).ForEach(hpm => hpRegenerationStat.Modifications.Add(new StatModificationDto(hpm, hpRegenerationStat)));
            var mpRegenerationStat = new StatDto()
            {
                Name = map.Locale["CharacterMPRegenerationStat"],
                Current = character.MPRegeneration,
                Base = character.BaseMPRegeneration + (character.MPRegenerationIncreasePerLevel * (character.Level - 1)),
                IsDecimalStat = true,
                IsPercentileStat = false,
                HasMaxStat = false,
                Visible = character.UsesMP,
                Modifications = new List<StatModificationDto>()
            };
            character.MPRegenerationModifications.Where(m => m.RemainingTurns != 0).ForEach(mpm => mpRegenerationStat.Modifications.Add(new StatModificationDto(mpm, mpRegenerationStat)));
            var accuracyStat = new StatDto()
            {
                Name = map.Locale["CharacterAccuracyStat"],
                Current = character.Accuracy,
                Base = character.BaseAccuracy,
                IsDecimalStat = false,
                IsPercentileStat = true,
                HasMaxStat = false,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.AccuracyModifications.Where(m => m.RemainingTurns != 0).ForEach(am => accuracyStat.Modifications.Add(new StatModificationDto(am, accuracyStat)));
            var evasionStat = new StatDto()
            {
                Name = map.Locale["CharacterEvasionStat"],
                Current = character.Evasion,
                Base = character.BaseEvasion,
                IsDecimalStat = false,
                IsPercentileStat = true,
                HasMaxStat = false,
                Visible = true,
                Modifications = new List<StatModificationDto>()
            };
            character.EvasionModifications.Where(m => m.RemainingTurns != 0).ForEach(em => evasionStat.Modifications.Add(new StatModificationDto(em, evasionStat)));
            Stats = new List<StatDto>
            {
                hpStat,
                mpStat,
                attackStat,
                defenseStat,
                movementStat,
                hpRegenerationStat,
                mpRegenerationStat,
                accuracyStat,
                evasionStat
            };
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

        public StatModificationDto(StatModification source, StatDto stat)
        {
            Source = source.Id;
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

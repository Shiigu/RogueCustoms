using RoguelikeGameEngine.Game.Entities;
using RoguelikeGameEngine.Utils.Representation;

namespace RoguelikeGameEngine.Utils.InputsAndOutputs
{
    public class PlayerInfoDto
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public bool IsAtMaxLevel { get; set; }
        public int CurrentExperience { get; set; }
        public int ExperienceToNextLevel { get; set; }

        public int CurrentHP { get; set; }
        public int MaxHP { get; set; }

        public int BaseMaxHP { get; set; }

        public List<StatModificationDto> MaxHPModifications { get; set; }

        public int CurrentAttack { get; set; }
        public int BaseAttack { get; set; }

        public List<StatModificationDto> AttackModifications { get; set; }

        public int CurrentDefense { get; set; }
        public int BaseDefense { get; set; }

        public List<StatModificationDto> DefenseModifications { get; set; }

        public int CurrentMovement { get; set; }
        public int BaseMovement { get; set; }

        public List<StatModificationDto> MovementModifications { get; set; }

        public decimal CurrentHPRegeneration { get; set; }
        public decimal BaseHPRegeneration { get; set; }

        public List<StatModificationDto> HPRegenerationModifications { get; set; }

        public List<AlteredStatusDetailDto> AlteredStatuses { get; set; }

        public EquippedItemDetailDto WeaponInfo { get; set; }
        public EquippedItemDetailDto ArmorInfo { get; set; }

        public PlayerInfoDto() { } 

        public PlayerInfoDto(PlayerCharacter character)
        {
            Name = character.Name;
            Level = character.Level;
            IsAtMaxLevel = character.Level == character.MaxLevel;
            if (!IsAtMaxLevel)
            {
                CurrentExperience = character.Experience;
                ExperienceToNextLevel = character.ExperienceToLevelUpDifference;
            }
            CurrentHP = character.HP;
            MaxHP = character.MaxHP;
            BaseMaxHP = character.BaseMaxHP + (int)(character.MaxHPIncreasePerLevel * (character.Level - 1));
            MaxHPModifications = new List<StatModificationDto>();
            character.MaxHPModifications.ForEach(mhm => MaxHPModifications.Add(new StatModificationDto(mhm, true)));
            CurrentAttack = character.Attack;
            BaseAttack = character.BaseAttack + (int)(character.AttackIncreasePerLevel * (character.Level - 1));
            AttackModifications = new List<StatModificationDto>();
            character.AttackModifications.ForEach(am => AttackModifications.Add(new StatModificationDto(am, true)));
            CurrentDefense = character.Defense;
            BaseDefense = character.BaseDefense + (int)(character.DefenseIncreasePerLevel * (character.Level - 1));
            DefenseModifications = new List<StatModificationDto>();
            character.DefenseModifications.ForEach(dm => DefenseModifications.Add(new StatModificationDto(dm, true)));
            CurrentMovement = character.Movement;
            BaseMovement = character.BaseMovement + (int)(character.MovementIncreasePerLevel * (character.Level - 1));
            MovementModifications = new List<StatModificationDto>();
            character.MovementModifications.ForEach(mm => MovementModifications.Add(new StatModificationDto(mm, true)));
            CurrentHPRegeneration = character.HPRegeneration;
            BaseHPRegeneration = character.BaseHPRegeneration + (character.HPRegenerationIncreasePerLevel * (character.Level - 1));
            HPRegenerationModifications = new List<StatModificationDto>();
            character.HPRegenerationModifications.ForEach(hm => HPRegenerationModifications.Add(new StatModificationDto(hm, false)));
            AlteredStatuses = new List<AlteredStatusDetailDto>();
            character.AlteredStatuses.ForEach(als => AlteredStatuses.Add(new AlteredStatusDetailDto(als)));
            WeaponInfo = new EquippedItemDetailDto(character.Weapon);
            ArmorInfo = new EquippedItemDetailDto(character.Armor);
        }
    }

    public class StatModificationDto
    {
        public string Source { get; set; }
        public decimal Amount { get; set; }

        public StatModificationDto() { }

        public StatModificationDto(StatModification source, bool truncate)
        {
            Source = source.Id;
            if(truncate)
                Amount = (int) source.Amount;
            else
                Amount = source.Amount;
        }
    }

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

    public class EquippedItemDetailDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public EquippedItemDetailDto() { }

        public EquippedItemDetailDto(Item i)
        {
            Name = i.Name;
            Description = i.Description;
            ConsoleRepresentation = i.ConsoleRepresentation;
        }
    }
}

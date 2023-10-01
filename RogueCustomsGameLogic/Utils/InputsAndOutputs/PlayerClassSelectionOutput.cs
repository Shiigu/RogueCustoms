using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    public class PlayerClassSelectionOutput
    {
        public List<CharacterClassDto> CharacterClasses { get; set; } = new List<CharacterClassDto>();
        public PlayerClassSelectionOutput() { }
        public PlayerClassSelectionOutput(Dungeon dungeon) {
            foreach (var playerClass in dungeon.Classes.Where(c => c.EntityType == EntityType.Player))
            {
                CharacterClasses.Add(new CharacterClassDto(playerClass, dungeon));
            }
        }
    }

    public class CharacterClassDto
    {
        public string ClassId { get; set; }
        public string Name { get; set; }
        public bool RequiresNamePrompt { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public List<CharacterClassStatDto> InitialStats { get; set; } = new List<CharacterClassStatDto>();
        public string SightRangeName { get; set; }
        public string SightRangeStat { get; set; }
        public string InventorySizeName { get; set; }
        public string InventorySizeStat { get; set; }

        public ItemDetailDto StartingWeapon { get; set; }
        public ItemDetailDto StartingArmor { get; set; }
        public List<ItemDetailDto> StartingInventory { get; set; } = new List<ItemDetailDto>();

        public CharacterClassDto(EntityClass characterClass, Dungeon dungeon)
        {
            ClassId = characterClass.Id;
            Name = dungeon.LocaleToUse[characterClass.Name];
            RequiresNamePrompt = characterClass.RequiresNamePrompt;
            Description = dungeon.LocaleToUse[characterClass.Description];
            ConsoleRepresentation = characterClass.ConsoleRepresentation;

            InitialStats = new List<CharacterClassStatDto>
            {
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterMaxHPStat"],
                    Base = characterClass.BaseHP,
                    IncreasePerLevel = characterClass.MaxHPIncreasePerLevel,
                    IsIntegerStat = true,
                    Visible = true
                },
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterMaxMPStat"],
                    Base = characterClass.BaseMP,
                    IncreasePerLevel = characterClass.MaxMPIncreasePerLevel,
                    IsIntegerStat = true,
                    Visible = characterClass.UsesMP
                },
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterAttackStat"],
                    Base = characterClass.BaseAttack,
                    IncreasePerLevel = characterClass.AttackIncreasePerLevel,
                    IsIntegerStat = true,
                    Visible = true
                },
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterDefenseStat"],
                    Base = characterClass.BaseDefense,
                    IncreasePerLevel = characterClass.DefenseIncreasePerLevel,
                    IsIntegerStat = true,
                    Visible = true
                },
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterMovementStat"],
                    Base = characterClass.BaseMovement,
                    IncreasePerLevel = characterClass.MovementIncreasePerLevel,
                    IsIntegerStat = true,
                    Visible = true
                },
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterHPRegenerationStat"],
                    Base = characterClass.BaseHPRegeneration,
                    IncreasePerLevel = characterClass.HPRegenerationIncreasePerLevel,
                    IsIntegerStat = false,
                    Visible = true
                },
                new CharacterClassStatDto
                {
                    Name = dungeon.LocaleToUse["CharacterMPRegenerationStat"],
                    Base = characterClass.BaseMPRegeneration,
                    IncreasePerLevel = characterClass.MPRegenerationIncreasePerLevel,
                    IsIntegerStat = false,
                    Visible = characterClass.UsesMP
                }
            };

            SightRangeName = dungeon.LocaleToUse["CharacterSightRangeStat"];

            if (characterClass.BaseSightRange == Constants.FullMapSightRange)
                SightRangeStat = dungeon.LocaleToUse["SightRangeStatFullMap"];
            else if (characterClass.BaseSightRange == Constants.FullRoomSightRange)
                SightRangeStat = dungeon.LocaleToUse["SightRangeStatFullRoom"];
            else
                SightRangeStat = dungeon.LocaleToUse["SightRangeStatFlatNumber"].Format(new { SightRange = characterClass.BaseSightRange.ToString() });

            InventorySizeName = dungeon.LocaleToUse["CharacterInventorySizeStat"];
            InventorySizeStat = dungeon.LocaleToUse["InventorySizeStatFlatNumber"].Format(new { InventorySize = characterClass.InventorySize.ToString() });

            var startingWeaponClass = dungeon.Classes.Find(c => c.EntityType == EntityType.Weapon && c.Id.Equals(characterClass.StartingWeaponId));
            StartingWeapon = new ItemDetailDto(startingWeaponClass, dungeon);
            var startingArmorClass = dungeon.Classes.Find(c => c.EntityType == EntityType.Armor && c.Id.Equals(characterClass.StartingArmorId));
            StartingArmor = new ItemDetailDto(startingArmorClass, dungeon);

            StartingInventory = new List<ItemDetailDto>();

            foreach (var itemId in characterClass.StartingInventoryIds)
            {
                var inventoryItemClass = dungeon.Classes.Find(c => (c.EntityType == EntityType.Weapon || c.EntityType == EntityType.Armor || c.EntityType == EntityType.Consumable) && c.Id.Equals(itemId));
                StartingInventory.Add(new ItemDetailDto(inventoryItemClass, dungeon));
            }
        }

        public CharacterClassDto() { }
    }

    public class CharacterClassStatDto {
        public string Name { get; set; }
        public string StatAsString { get; set; }
        public decimal Base { get; set; }
        public decimal IncreasePerLevel { get; set; }
        public bool IsIntegerStat { get; set; }
        public bool Visible { get; set; }
    }
}

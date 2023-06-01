using RoguelikeGameEngine.Utils.Representation;

namespace RoguelikeGameEngine.Utils.JsonImports
{
    [Serializable]
    public class ClassInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Faction { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public string EntityType { get; set; }
        public List<ActionWithEffectsInfo> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfo>();
        public bool CanBePickedUp { get; set; }
        public bool StartsVisible { get; set; }

        #region Only for NPCs
        public int BaseHP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseMovement { get; set; }
        public decimal BaseHPRegeneration { get; set; }
        public string BaseSightRange { get; set; }
        public int InventorySize { get; set; }
        public string StartingWeapon { get; set; }
        public string StartingArmor { get; set; }
        public List<ActionWithEffectsInfo> OnAttackActions { get; set; } = new List<ActionWithEffectsInfo>();
        public List<ActionWithEffectsInfo> OnAttackedActions { get; set; } = new List<ActionWithEffectsInfo>();
        public int MaxLevel { get; set; }
        public bool CanGainExperience { get; set; }
        public string ExperiencePayoutFormula { get; set; }
        public string ExperienceToLevelUpFormula { get; set; }
        public decimal MaxHPIncreasePerLevel { get; set; }
        public decimal AttackIncreasePerLevel { get; set; }
        public decimal DefenseIncreasePerLevel { get; set; }
        public decimal MovementIncreasePerLevel { get; set; }
        public decimal HPRegenerationIncreasePerLevel { get; set; }
        public List<ActionWithEffectsInfo> OnDeathActions { get; set; } = new List<ActionWithEffectsInfo>();
        #endregion

        public string Power { get; set; }

        #region Only for Items
        public List<ActionWithEffectsInfo> OnItemSteppedActions { get; set; } = new List<ActionWithEffectsInfo>();
        public List<ActionWithEffectsInfo> OnItemUseActions { get; set; } = new List<ActionWithEffectsInfo>();
        #endregion

        #region Only for Statuses
        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }
        public List<ActionWithEffectsInfo> OnStatusApplyActions { get; set; } = new List<ActionWithEffectsInfo>();
        #endregion
    }
}
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11;

using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public abstract class CharacterInfoV12 : ClassInfoV12
    {
        public string Faction { get; set; }
        public bool CanBePickedUp { get; set; }
        public bool StartsVisible { get; set; }
        public bool UsesMP { get; set; }
        public int BaseHP { get; set; }
        public int BaseMP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseMovement { get; set; }
        public decimal BaseHPRegeneration { get; set; }
        public decimal BaseMPRegeneration { get; set; }
        public string BaseSightRange { get; set; }
        public int BaseHunger { get; set; }
        public decimal HungerHPDegeneration { get; set; }
        public int InventorySize { get; set; }
        public string StartingWeapon { get; set; }
        public string StartingArmor { get; set; }
        public List<string> StartingInventory { get; set; }
        public int MaxLevel { get; set; }
        public bool CanGainExperience { get; set; }
        public string ExperiencePayoutFormula { get; set; }
        public string ExperienceToLevelUpFormula { get; set; }
        public decimal MaxHPIncreasePerLevel { get; set; }
        public decimal MaxMPIncreasePerLevel { get; set; }
        public decimal AttackIncreasePerLevel { get; set; }
        public decimal DefenseIncreasePerLevel { get; set; }
        public decimal MovementIncreasePerLevel { get; set; }
        public decimal HPRegenerationIncreasePerLevel { get; set; }
        public decimal MPRegenerationIncreasePerLevel { get; set; }
        public List<ActionWithEffectsInfoV12> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfoV12>();
        public List<ActionWithEffectsInfoV12> OnAttackActions { get; set; } = new List<ActionWithEffectsInfoV12>();
        public List<ActionWithEffectsInfoV12> OnAttackedActions { get; set; } = new List<ActionWithEffectsInfoV12>();
        public List<ActionWithEffectsInfoV12> OnDeathActions { get; set; } = new List<ActionWithEffectsInfoV12>();
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V14
{
    [Serializable]
    public abstract class CharacterInfoV14 : ClassInfo
    {
        public string Faction { get; set; }
        public bool StartsVisible { get; set; }
        public bool UsesMP { get; set; }
        public bool UsesHunger { get; set; }
        public int BaseHP { get; set; }
        public int BaseMP { get; set; }
        public int BaseAttack { get; set; }
        public int BaseDefense { get; set; }
        public int BaseMovement { get; set; }
        public int BaseAccuracy { get; set; }
        public int BaseEvasion { get; set; }
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
        public ActionWithEffectsInfo OnTurnStart { get; set; } = new ActionWithEffectsInfo();
        public List<ActionWithEffectsInfo> OnAttack { get; set; } = new List<ActionWithEffectsInfo>();
        public ActionWithEffectsInfo OnAttacked { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnDeath { get; set; } = new ActionWithEffectsInfo();
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

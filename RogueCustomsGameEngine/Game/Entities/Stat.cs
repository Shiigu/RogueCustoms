using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Game.Entities
{
    [Serializable]
    public class Stat
    {
        private static readonly List<StatType> StatTypesThatCanRegenerate = new() { StatType.HP, StatType.MP, StatType.Hunger };
        public static readonly List<string> StatsInUI = new() { "hp", "mp", "hunger", "attack", "defense", "movement", "accuracy", "evasion" };

        public string Id { get; set; }
        public string Name { get; set; }
        public StatType StatType { get; set; }
        public Character Character { get; set; }
        public decimal Base { get; set; } // If HasMax, this will contain the Max value. If !HasMax, this will contain the value without modifications
        public decimal BaseAfterLevelUp {
            get
            {
                var increaseAfterLevel = IncreasePerLevel * (Character.Level - 1);
                if (StatType != StatType.Decimal && StatType != StatType.Regeneration)
                    increaseAfterLevel = (int)increaseAfterLevel;
                return Base + increaseAfterLevel;
            }
        }
        private decimal _current { get; set; } // If !HasMax, Current = Value. If HasMax, Current is current (e.g. if HP is 14 out of 16, Value is 14).
        public decimal Current
        {
            get
            {
                if(HasMax)
                {
                    if (_current > BaseAfterModifications)
                        _current = BaseAfterModifications;
                    return _current;
                }
                return BaseAfterModifications;
            }
            set
            {
                _current = value;
            }
        }
        public bool HasMax { get; set; }
        public decimal IncreasePerLevel { get; set; }
        public List<StatModification> ActiveModifications { get; set; } = new();
        public List<(string Source, decimal Amount)> PassiveModifications
        {
            get
            {
                var passiveModifications = new List<(string Source, decimal Amount)>();

                if (Character.Weapon != null)
                {
                    var weaponModification = Character.Weapon.StatModifiers.FirstOrDefault(sm => sm.Id.Equals(Id));
                    if (weaponModification != null)
                        passiveModifications.Add((Character.Weapon.Name, weaponModification.Amount));
                }
                if(Character.Armor != null)
                {
                    var armorModification = Character.Armor.StatModifiers.FirstOrDefault(sm => sm.Id.Equals(Id));
                    if (armorModification != null)
                        passiveModifications.Add((Character.Armor.Name, armorModification.Amount));
                }
                Character.Inventory?.ForEach(i =>
                {
                    if (!i.IsEquippable)
                    {
                        var itemModification = i.StatModifiers.FirstOrDefault(sm => sm.Id.Equals(Id));
                        if (itemModification != null)
                            passiveModifications.Add((i.Name, itemModification.Amount));
                    }
                });

                return passiveModifications;
            }
        }

        public decimal TotalActiveModificationAmount => ActiveModifications.Where(a => a.RemainingTurns != 0).Sum(a => a.Amount);
        public decimal TotalPassiveModificationAmount => PassiveModifications.Sum(pm => pm.Amount);
        public decimal TotalModificationAmount => TotalActiveModificationAmount + TotalPassiveModificationAmount;

        public decimal BaseAfterModifications
        {
            get
            {
                try
                {
                    return (BaseAfterLevelUp + TotalModificationAmount).Clamp(MinCap, MaxCap);
                }
                catch // In case this is retrieved before the full Character data is ready.
                {
                    return BaseAfterLevelUp.Clamp(MinCap, MaxCap);
                }
            }
        }
        public decimal MinCap { get; set; }
        public decimal MaxCap { get; set; }

        public string RegenerationTargetId { get; set; }
        public Stat? RegenerationTarget { get; set; }
        public decimal CarriedRegeneration { get; set; } = 0;

        public override string ToString() 
        { 
            if(HasMax)
            {
                return (RegenerationTarget != null)
                    ? $"{Name} - Base: {BaseAfterLevelUp}, Current: {Current}/{BaseAfterModifications}, Regenerates {RegenerationTarget.Name}"
                    : $"{Name} - Base: {BaseAfterLevelUp}, Current: {Current}/{BaseAfterModifications}";
            }
            else
            {
                return (RegenerationTarget != null)
                    ? $"{Name} - Base: {BaseAfterLevelUp}, Current: {BaseAfterModifications}, Regenerates {RegenerationTarget.Name}"
                    : $"{Name} - Base: {BaseAfterLevelUp}, Current: {BaseAfterModifications}";
            }
        }

        public void TryToRegenerate()
        {
            if (RegenerationTarget == null) return;
            if (Character.ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (!StatTypesThatCanRegenerate.Contains(RegenerationTarget.StatType)) return;
            if (RegenerationTarget.Current > RegenerationTarget.BaseAfterModifications) RegenerationTarget.Current = RegenerationTarget.BaseAfterModifications;
            var regenerationToUse = StatType == StatType.HP && Character.IsStarving ? BaseAfterModifications * -1 : BaseAfterModifications;
            if (StatType == StatType.HP && Character.IsStarving && CarriedRegeneration > 0)
            {
                CarriedRegeneration = 0;
                return;
            }
            if (Character.Map.TurnCount == 1 || (regenerationToUse > 0 && RegenerationTarget.Current == RegenerationTarget.BaseAfterModifications) || regenerationToUse == 0)
            {
                CarriedRegeneration = 0;
                return;
            }
            CarriedRegeneration += regenerationToUse;
            if (CarriedRegeneration >= 1)
            {
                var wholePart = Math.Truncate(CarriedRegeneration);
                var fractionalPart = CarriedRegeneration - wholePart;
                RegenerationTarget.Current = Math.Min(RegenerationTarget.BaseAfterModifications, RegenerationTarget.Current + (int)wholePart);
                CarriedRegeneration = fractionalPart;
            }
            else if (CarriedRegeneration <= -1)
            {
                var oldCurrent = RegenerationTarget.Current;
                var wholePart = Math.Truncate(CarriedRegeneration);
                var fractionalPart = CarriedRegeneration - wholePart;
                RegenerationTarget.Current = Math.Max(RegenerationTarget.MinCap, RegenerationTarget.Current + (int)wholePart);
                CarriedRegeneration = fractionalPart;
                if (oldCurrent > RegenerationTarget.Current && StatType == StatType.HP && Character.IsStarving)
                {
                    Character.Map.AppendMessage(Character.Map.Locale["CharacterTakesDamageFromHunger"].Format(new { CharacterName = Name, DamageDealt = oldCurrent - RegenerationTarget.Current, CharacterHPStat = RegenerationTarget.Name, CharacterHungerStat = Name }));
                    Character.Map.DisplayEvents.Add(($"{Character.Name} is starving", new List<DisplayEventDto>
                    {
                        new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.PlayerDamaged }
                        }
                    }));
                }
            }
        }

        public Stat Clone()
        {
            return new Stat
            {
                Id = Id,
                Name = Name,
                StatType = StatType,
                Base = Base,
                _current = _current,
                HasMax = HasMax,
                IncreasePerLevel = IncreasePerLevel,
                MinCap = MinCap,
                MaxCap = MaxCap,
                RegenerationTargetId = RegenerationTargetId
            };
        }
    }

    public enum StatType
    {
        HP,
        MP,
        Hunger,
        Regeneration,
        Integer,
        Decimal,
        Percentage
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

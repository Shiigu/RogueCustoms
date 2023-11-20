using org.matheval;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Data;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;
using System.IO;

namespace RogueCustomsGameEngine.Game.Entities
{
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public abstract class Character : Entity, IHasActions, IKillable
    {
        public Faction Faction { get; set; }

        public readonly string StartingWeaponId;
        public readonly string StartingArmorId;
        public Item StartingWeapon { get; set; }
        public Item StartingArmor { get; set; }
        public Item EquippedWeapon { get; set; }
        public Item EquippedArmor { get; set; }

        public Item Weapon => EquippedWeapon ?? StartingWeapon;
        public Item Armor => EquippedArmor ?? StartingArmor;

        public List<Item> Inventory { get; set; }

        public readonly string ExperiencePayoutFormula;
        public int ExperiencePayout => ParseArgForFormulaAndCalculate(ExperiencePayoutFormula, false);
        public int Experience { get; set; }
        public readonly string ExperienceToLevelUpFormula;
        public int ExperienceToLevelUp => LastLevelUpExperience + ParseArgForFormulaAndCalculate(ExperienceToLevelUpFormula, false);
        public int LastLevelUpExperience { get; set; }
        public int ExperienceToLevelUpDifference => ExperienceToLevelUp - Experience;
        public int Level { get; set; }
        public int MaxLevel { get; set; }
        public bool CanGainExperience { get; set; }
        public bool CanTakeAction { get; set; }
        public bool UsesMP { get; set; }
        public int HP { get; set; }

        public readonly int BaseMaxHP;
        public readonly decimal MaxHPIncreasePerLevel;
        public List<StatModification> MaxHPModifications { get; set; }
        public int TotalMaxHPIncrements => MaxHPModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int)a.Amount);
        public int MaxHP => Math.Min(BaseMaxHP + (int)(MaxHPIncreasePerLevel * (Level - 1)) + TotalMaxHPIncrements, Constants.RESOURCE_STAT_CAP);
        public int MP { get; set; }

        public readonly int BaseMaxMP;
        public readonly decimal MaxMPIncreasePerLevel;
        public List<StatModification> MaxMPModifications { get; set; }
        public int TotalMaxMPIncrements => MaxMPModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int)a.Amount);
        public int MaxMP => Math.Min(BaseMaxMP + (int)(MaxMPIncreasePerLevel * (Level - 1)) + TotalMaxMPIncrements, Constants.RESOURCE_STAT_CAP);

        public readonly int BaseAttack;
        public readonly decimal AttackIncreasePerLevel;
        public List<StatModification> AttackModifications { get; set; }
        public int TotalAttackIncrements => AttackModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int)a.Amount);
        public int Attack => Math.Min(BaseAttack + (int)(AttackIncreasePerLevel * (Level - 1)) + TotalAttackIncrements, Constants.NORMAL_STAT_CAP);
        public string Damage
        {
            get
            {
                if (Attack >= 0)
                    return $"{Weapon.Power}+{Attack}";
                return $"{Weapon.Power}-{Math.Abs(Attack)}";
            }
        }

        public readonly int BaseDefense;
        public readonly decimal DefenseIncreasePerLevel;
        public List<StatModification> DefenseModifications { get; set; }
        public int TotalDefenseIncrements => DefenseModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int)a.Amount);
        public int Defense => Math.Min(BaseDefense + (int)(DefenseIncreasePerLevel * (Level - 1)) + TotalDefenseIncrements, Constants.NORMAL_STAT_CAP);
        public string Mitigation
        {
            get
            {
                if (Defense >= 0)
                    return $"{Armor.Power}+{Defense}";
                return $"{Armor.Power}-{Math.Abs(Defense)}";
            }
        }

        public readonly int BaseMovement;
        public readonly decimal MovementIncreasePerLevel;

        public List<StatModification> MovementModifications { get; set; }
        public int TotalMovementIncrements => MovementModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int)a.Amount);
        public int Movement => Math.Min(BaseMovement + (int)(MovementIncreasePerLevel * (Level - 1)) + TotalMovementIncrements, Constants.MOVEMENT_STAT_CAP);

        public int RemainingMovement { get; set; }
        public bool TookAction { get; set; }

        public readonly decimal BaseHPRegeneration;
        public readonly decimal HPRegenerationIncreasePerLevel;
        public List<StatModification> HPRegenerationModifications { get; set; }
        public decimal TotalHPRegenerationIncrements => HPRegenerationModifications.Where(a => a.RemainingTurns != 0).Sum(a => a.Amount);
        public decimal HPRegeneration => Math.Min(BaseHPRegeneration + (HPRegenerationIncreasePerLevel * (Level - 1)) + TotalHPRegenerationIncrements, Constants.REGEN_STAT_CAP);
        private decimal CarriedHPRegeneration;

        public readonly decimal BaseMPRegeneration;
        public readonly decimal MPRegenerationIncreasePerLevel;
        public List<StatModification> MPRegenerationModifications { get; set; }
        public decimal TotalMPRegenerationIncrements => MPRegenerationModifications.Where(a => a.RemainingTurns != 0).Sum(a => a.Amount);
        public decimal MPRegeneration => Math.Min(BaseMPRegeneration + (MPRegenerationIncreasePerLevel * (Level - 1)) + TotalMPRegenerationIncrements, Constants.REGEN_STAT_CAP);
        private decimal CarriedMPRegeneration;

        public readonly int BaseSightRange;
        public int TotalSightRangeIncrements { get; set; } = 0;
        public int SightRange => BaseSightRange + TotalSightRangeIncrements;

        public readonly int BaseAccuracy;
        public List<StatModification> AccuracyModifications { get; set; }
        public decimal TotalAccuracyIncrements => AccuracyModifications.Where(a => a.RemainingTurns != 0).Sum(a => a.Amount);
        public decimal Accuracy => Math.Max(Constants.MIN_ACCURACY_CAP, Math.Min(Constants.MAX_ACCURACY_CAP, BaseAccuracy + TotalAccuracyIncrements));

        public readonly int BaseEvasion;
        public List<StatModification> EvasionModifications { get; set; }
        public decimal TotalEvasionIncrements => EvasionModifications.Where(a => a.RemainingTurns != 0).Sum(a => a.Amount);
        public decimal Evasion => Math.Max(Constants.MIN_EVASION_CAP, Math.Min(Constants.MAX_EVASION_CAP, BaseEvasion + TotalEvasionIncrements));

        public List<AlteredStatus> AlteredStatuses { get; set; }

        public readonly int InventorySize;
        public List<ActionWithEffects> OnTurnStart
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnTurnStart != null)
                    actionList.Add(OwnOnTurnStart);
                if (Weapon?.OwnOnTurnStart != null)
                    actionList.Add(Weapon.OwnOnTurnStart);
                if (Armor?.OwnOnTurnStart != null)
                    actionList.Add(Armor.OwnOnTurnStart);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnTurnStart != null && !i.IsEquippable)
                        actionList.Add(i.OwnOnTurnStart);
                });
                return actionList;
            }
        }
        public List<ActionWithEffects> OnAttack
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnAttack != null)
                    actionList.AddRange(OwnOnAttack);
                if (Weapon?.OwnOnAttack != null)
                    actionList.AddRange(Weapon.OwnOnAttack);
                if (Armor?.OwnOnAttack != null)
                    actionList.AddRange(Armor.OwnOnAttack);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnAttack != null && !i.IsEquippable)
                        actionList.AddRange(i.OwnOnAttack);
                });

                for (int i = 0; i < actionList.Count; i++)
                {
                    actionList[i].ActionId = i;
                }

                return actionList;
            }
        }
        public List<ActionWithEffects> OnAttacked
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnAttacked != null)
                    actionList.Add(OwnOnAttacked);
                if (Weapon?.OwnOnAttacked != null)
                    actionList.Add(Weapon.OwnOnAttacked);
                if (Armor?.OwnOnAttacked != null)
                    actionList.Add(Armor.OwnOnAttacked);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnAttacked != null && !i.IsEquippable)
                        actionList.Add(i.OwnOnAttacked);
                });
                AlteredStatuses?.Where(als => als.RemainingTurns != 0).ForEach(als =>
                {
                    if (als?.OwnOnAttacked != null)
                        actionList.Add(als.OwnOnAttacked);
                });
                return actionList;
            }
        }
        public List<ActionWithEffects> OnDeath
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnDeath != null)
                    actionList.Add(OwnOnDeath);
                if (Weapon?.OwnOnDeath != null)
                    actionList.Add(Weapon.OwnOnDeath);
                if (Armor?.OwnOnDeath != null)
                    actionList.Add(Armor.OwnOnDeath);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnDeath != null && !i.IsEquippable)
                        actionList.Add(i.OwnOnDeath);
                });
                return actionList;
            }
        }

        private List<Tile> _fovTiles;
        public List<Tile> FOVTiles
        {
            get
            {
                return _fovTiles ??= ComputeFOVTiles();
            }
            set { _fovTiles = value; }
        }

        public List<(string StatName, List<StatModification> Modifications)> StatModifications
        {
            get
            {
                var modifications = new List<(string StatName, List<StatModification> Modifications)>();

                modifications.Add((Map.Locale["CharacterMaxHPStat"], MaxHPModifications));
                if(UsesMP)
                    modifications.Add((Map.Locale["CharacterMaxMPStat"], MaxMPModifications));
                modifications.Add((Map.Locale["CharacterAttackStat"], AttackModifications));
                modifications.Add((Map.Locale["CharacterDefenseStat"], DefenseModifications));
                modifications.Add((Map.Locale["CharacterMovementStat"], MovementModifications));
                modifications.Add((Map.Locale["CharacterHPRegenerationStat"], HPRegenerationModifications));
                modifications.Add((Map.Locale["CharacterAccuracyStat"], AccuracyModifications));
                modifications.Add((Map.Locale["CharacterEvasionStat"], EvasionModifications));
                if (UsesMP) 
                    modifications.Add((Map.Locale["CharacterMPRegenerationStat"], MPRegenerationModifications));

                return modifications;
            }
        }

        protected Character(EntityClass entityClass, int level, Map map) : base(entityClass, map)
        {
            Faction = entityClass.Faction;
            StartingWeaponId = entityClass.StartingWeaponId;
            StartingArmorId = entityClass.StartingArmorId;
            UsesMP = entityClass.UsesMP;
            HP = entityClass.BaseHP;
            BaseMaxHP = entityClass.BaseHP;
            MaxHPIncreasePerLevel = entityClass.MaxHPIncreasePerLevel;
            MP = entityClass.BaseMP;
            BaseMaxMP = entityClass.BaseMP;
            MaxMPIncreasePerLevel = entityClass.MaxMPIncreasePerLevel;
            BaseAttack = entityClass.BaseAttack;
            AttackIncreasePerLevel = entityClass.AttackIncreasePerLevel;
            BaseDefense = entityClass.BaseDefense;
            DefenseIncreasePerLevel = entityClass.DefenseIncreasePerLevel;
            BaseMovement = entityClass.BaseMovement;
            MovementIncreasePerLevel = entityClass.MovementIncreasePerLevel;
            BaseHPRegeneration = entityClass.BaseHPRegeneration;
            HPRegenerationIncreasePerLevel = entityClass.HPRegenerationIncreasePerLevel;
            BaseMPRegeneration = entityClass.BaseMPRegeneration;
            MPRegenerationIncreasePerLevel = entityClass.MPRegenerationIncreasePerLevel;
            ExperiencePayoutFormula = entityClass.ExperiencePayoutFormula;
            ExperienceToLevelUpFormula = entityClass.ExperienceToLevelUpFormula;
            CanGainExperience = entityClass.CanGainExperience;
            Level = level;
            MaxLevel = entityClass.MaxLevel;
            if (Level > 1 && CanGainExperience)
            {
                Level = level - 1;
                Experience = ParseArgForFormulaAndCalculate(ExperienceToLevelUpFormula, false);
                Level++;
            }
            else
            {
                Experience = 0;
            }

            MaxHPModifications = new List<StatModification>();
            MaxMPModifications = new List<StatModification>();
            AttackModifications = new List<StatModification>();
            DefenseModifications = new List<StatModification>();
            MovementModifications = new List<StatModification>();
            HPRegenerationModifications = new List<StatModification>();
            MPRegenerationModifications = new List<StatModification>();
            AccuracyModifications = new List<StatModification>();
            EvasionModifications = new List<StatModification>();

            BaseSightRange = entityClass.BaseSightRange;
            BaseAccuracy = entityClass.BaseAccuracy;
            BaseEvasion = entityClass.BaseEvasion;
            InventorySize = entityClass.InventorySize;
            Inventory = new List<Item>(InventorySize);

            AlteredStatuses = new List<AlteredStatus>();

            LastLevelUpExperience = Experience;
            CarriedHPRegeneration = 0;
            CarriedMPRegeneration = 0;

            OwnOnTurnStart = MapClassAction(entityClass.OnTurnStart);
            OwnOnAttack = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnAttack, OwnOnAttack);
            OwnOnAttacked = MapClassAction(entityClass.OnAttacked);
            OwnOnDeath = MapClassAction(entityClass.OnDeath);
        }

        public List<Tile> ComputeFOVTiles()
        {
            if (SightRange == Constants.FullMapSightRange)
            {
                return Map.Tiles.ToList();
            }
            else
            {
                if (SightRange == Constants.FullRoomSightRange)
                {
                    if (ContainingTile.Type == TileType.Hallway)
                        return Map.GetFOVTilesWithinDistance(Position, Constants.FullRoomSightRangeForHallways);
                    return Map.GetTilesInRoom(ContainingRoom);
                }
                else
                {
                    return Map.GetFOVTilesWithinDistance(Position, SightRange);
                }
            }
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            OnAttack?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            OnAttacked?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            OnTurnStart?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            foreach (var modification in StatModifications)
            {
                modification.Modifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            }
            AlteredStatuses?.Where(a => a.RemainingTurns != 0).ForEach(als => als.RefreshCooldownsAndUpdateTurnLength());
            foreach (var modification in StatModifications)
            {
                modification.Modifications?.RemoveAll(a => a.RemainingTurns == 0);
            }
            AlteredStatuses?.Where(a => a.RemainingTurns == 0 && !a.FlaggedToRemove).ForEach(als => als.OnRemove?.Do(als, this, false));
            AlteredStatuses?.RemoveAll(als => als.RemainingTurns == 0);
            Inventory?.ForEach(i => i.RefreshCooldownsAndUpdateTurnLength());
        }

        public void PerformOnTurnStart()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            FOVTiles = ComputeFOVTiles();
            CanTakeAction = true;
            var modificationsThatMightBeNeutralized = new List<(List<StatModification> modificationList, string statName, bool mightBeNeutralized)>();
            var alteredStatusesThatMightBeNeutralized = new List<(List<AlteredStatus> alteredStatusList, string statusName, bool mightEnd)>();

            if (this == Map.Player || Map.Player.CanSee(this))
            {
                foreach (var (statName, modifications) in StatModifications)
                {
                    modificationsThatMightBeNeutralized.Add((modifications, statName, modifications.Any() && modifications.Exists(mhm => mhm.RemainingTurns > 1)));
                }

                alteredStatusesThatMightBeNeutralized = AlteredStatuses
                    .Where(als => als.RemainingTurns > 0)
                    .GroupBy(als => als.ClassId)
                    .Select(group => (
                        group.ToList(),
                        group.FirstOrDefault()?.Name ?? "",
                        !group.Any(als => als.RemainingTurns > 1)
                    ))
                    .ToList();
            }
            RefreshCooldownsAndUpdateTurnLength();
            OnTurnStart.Where(otsa => otsa.ChecksCondition(this, this)).ForEach(otsa => otsa?.Do(otsa.User, this, true));
            AlteredStatuses?.ForEach(als => als.PerformOnTurnStart());
            foreach (var (modificationList, statName, mightBeNeutralized) in modificationsThatMightBeNeutralized)
            {
                if (mightBeNeutralized && modificationList?.TrueForAll(mhm => mhm.RemainingTurns == 0) == true)
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = Name, StatName = statName }), Color.DeepSkyBlue);
            }
            foreach (var (alteredStatusList, statusName, mightBeNeutralized) in alteredStatusesThatMightBeNeutralized)
            {
                if (mightBeNeutralized && alteredStatusList?.TrueForAll(mhm => mhm.RemainingTurns == 0) == true)
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = Name, StatusName = statusName }), Color.DeepSkyBlue);
            }
            TryRegenerateHP();
            TryRegenerateMP();
            if (HP <= 0)
                Die();
        }

        public void TryRegenerateHP()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (HP > MaxHP) HP = MaxHP;
            if ((HPRegeneration > 0 && HP == MaxHP) || HPRegeneration == 0)
            {
                CarriedHPRegeneration = 0;
                return;
            }
            CarriedHPRegeneration += HPRegeneration;
            if (CarriedHPRegeneration > 1)
            {
                var wholePart = Math.Truncate(CarriedHPRegeneration);
                var fractionalPart = CarriedHPRegeneration - wholePart;
                HP = Math.Min(MaxHP, HP + (int)wholePart);
                CarriedHPRegeneration = fractionalPart;
            }
            else if (CarriedHPRegeneration < -1)
            {
                var wholePart = Math.Truncate(CarriedHPRegeneration);
                var fractionalPart = CarriedHPRegeneration - wholePart;
                HP = Math.Max(0, HP + (int)wholePart);
                CarriedHPRegeneration = fractionalPart;
            }
        }

        public void TryRegenerateMP()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (!UsesMP) return;
            if (MP > MaxMP) MP = MaxMP;
            if ((MPRegeneration > 0 && MP == MaxMP) || MPRegeneration == 0)
            {
                CarriedMPRegeneration = 0;
                return;
            }
            CarriedMPRegeneration += MPRegeneration;
            if (CarriedMPRegeneration > 1)
            {
                var wholePart = Math.Truncate(CarriedMPRegeneration);
                var fractionalPart = CarriedMPRegeneration - wholePart;
                MP = Math.Min(MaxMP, MP + (int)wholePart);
                CarriedMPRegeneration = fractionalPart;
            }
            else if (CarriedMPRegeneration < -1)
            {
                var wholePart = Math.Truncate(CarriedMPRegeneration);
                var fractionalPart = CarriedMPRegeneration - wholePart;
                MP = Math.Max(0, MP + (int)wholePart);
                CarriedMPRegeneration = fractionalPart;
            }
        }

        public bool CanSee(Entity entity)
        {
            return this == entity
                || (entity.Visible
                && entity.Position != null
                && ComputeFOVTiles().Contains(entity.ContainingTile));
        }

        public void GainExperience(int pointsToAdd)
        {
            if (!CanGainExperience) return;
            Experience += pointsToAdd;
            if (Experience >= ExperienceToLevelUp)
            {
                LastLevelUpExperience = ExperienceToLevelUp;
                Level++;
                Color forecolorToUse;
                if (this == Map.Player || Faction.AlliedWith.Contains(Map.Player.Faction))
                    forecolorToUse = Color.Lime;
                else if (Faction.EnemiesWith.Contains(Map.Player.Faction))
                    forecolorToUse = Color.Red;
                else
                    forecolorToUse = Color.DeepSkyBlue;
                Map.AppendMessage(Map.Locale["CharacterLevelsUpMessage"].Format(new { CharacterName = Name, Level = Level}), forecolorToUse);
                HP = MaxHP;
                MP = MaxMP;
            }
        }

        private int ParseArgForFormulaAndCalculate(string arg, bool capIfLevelIsMax)
        {
            if (Level == MaxLevel && capIfLevelIsMax) return Experience;
            var parsedArg = arg.ToLowerInvariant();

            parsedArg = parsedArg.Replace("level", Level.ToString());

            try
            {
                return new Expression(parsedArg).Eval<int>();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Attempting to parse formula \"{arg}\" for {Name} failed: {ex.Message}");
            }
        }

        public abstract void DropItem(Item item);

        public abstract void PickItem(Item item);

        public void SwapWithEquippedItem(Item equippedItem, Item itemToEquip)
        {
            if (itemToEquip.EntityType == EntityType.Weapon)
                EquippedWeapon = itemToEquip;
            else if (itemToEquip.EntityType == EntityType.Armor)
                EquippedArmor = itemToEquip;
            var itemToEquipWasInTheBag = itemToEquip.Position == null;
            if (itemToEquipWasInTheBag)
                Inventory.Remove(itemToEquip);
            if (equippedItem != null)
            {
                if (!itemToEquipWasInTheBag)
                {
                    equippedItem.Position = itemToEquip.Position;
                    equippedItem.Owner = null!;
                    equippedItem.ExistenceStatus = EntityExistenceStatus.Alive;
                    Map.AppendMessage(Map.Locale["PlayerPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = equippedItem.Name }));
                }
                else
                {
                    Inventory.Add(equippedItem);
                    Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = equippedItem.Name }));
                }
            }
            if (!itemToEquipWasInTheBag)
            {
                itemToEquip.Position = null;
                itemToEquip.ExistenceStatus = EntityExistenceStatus.Gone;
                itemToEquip.Owner = this;
            }
        }

        public void TryToPickItem(Item item)
        {
            if (item.CanBePickedUp && Inventory.Count < InventorySize)
            {
                PickItem(item);
            }
            else
            {
                item.Stepped(this);
            }
        }

        public void AttackCharacter(Character target, ActionWithEffects action)
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            AlteredStatuses.Where(als => als.RemainingTurns > 0).ForEach(als => als.BeforeAttack?.Do(this, target, true));
            if (UsesMP)
                MP = Math.Max(0, MP - action.MPCost);
            var successfulEffects = action?.Do(this, target, true);
            if(Constants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                target.AttackedBy(this);
            if(action?.FinishesTurnWhenUsed == true)
                TookAction = true;
            RemainingMovement = 0;
        }

        public void InteractWithCharacter(Character target, ActionWithEffects action)
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (UsesMP)
                MP = Math.Max(0, MP - action.MPCost);
            action?.Do(this, target, true);
            if (action?.FinishesTurnWhenUsed == true)
                TookAction = true;
            RemainingMovement = 0;
        }

        public virtual void AttackedBy(Character source)
        {
            OnAttacked.Where(oaa => oaa.ChecksCondition(this, source)).ForEach(oaa => oaa?.Do(this, source, false));
        }

        public TargetType CalculateTargetTypeFor(Character target)
        {
            if (target == this)
                return TargetType.Self;
            if (Faction.AlliedWith.Contains(target.Faction))
                return TargetType.Ally;
            if (Faction.NeutralWith.Contains(target.Faction))
                return TargetType.Neutral;
            if (Faction.EnemiesWith.Contains(target.Faction))
                return TargetType.Enemy;
            if (Faction.Id.Equals(target.Faction.Id))
                return TargetType.Neutral;

            throw new InvalidDataException($"Cannot identify target relationship between {Name} and {target.Name}!");
        }

        public abstract void Die(Entity? attacker = null);
    }
}

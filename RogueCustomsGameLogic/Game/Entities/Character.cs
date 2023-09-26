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
        public int HP { get; set; }

        public readonly int BaseMaxHP;
        public readonly decimal MaxHPIncreasePerLevel;
        public List<StatModification> MaxHPModifications { get; set; }
        public int TotalMaxHPIncrements => MaxHPModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int) a.Amount);
        public int MaxHP => BaseMaxHP + (int)(MaxHPIncreasePerLevel * (Level - 1)) + TotalMaxHPIncrements;

        public readonly int BaseAttack;
        public readonly decimal AttackIncreasePerLevel;
        public List<StatModification> AttackModifications { get; set; }
        public int TotalAttackIncrements => AttackModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int) a.Amount);
        public int Attack => BaseAttack + (int)(AttackIncreasePerLevel * (Level - 1)) + TotalAttackIncrements;
        public string Damage
        {
            get
            {
                if(Attack >= 0)
                    return $"{Weapon.Power}+{Attack}";
                return $"{Weapon.Power}-{Math.Abs(Attack)}";
            }
        }

        public readonly int BaseDefense;
        public readonly decimal DefenseIncreasePerLevel;
        public List<StatModification> DefenseModifications { get; set; }
        public int TotalDefenseIncrements => DefenseModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int) a.Amount);
        public int Defense => BaseDefense + (int)(DefenseIncreasePerLevel * (Level - 1)) + TotalDefenseIncrements;
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
        public int TotalMovementIncrements => MovementModifications.Where(a => a.RemainingTurns != 0).Sum(a => (int) a.Amount);
        public int Movement => BaseMovement + (int)(MovementIncreasePerLevel * (Level - 1)) + TotalMovementIncrements;

        public int RemainingMovement { get; set; }

        public readonly decimal BaseHPRegeneration;
        public readonly decimal HPRegenerationIncreasePerLevel;
        public List<StatModification> HPRegenerationModifications { get; set; }
        public decimal TotalHPRegenerationIncrements => HPRegenerationModifications.Where(a => a.RemainingTurns != 0).Sum(a => a.Amount);
        public decimal HPRegeneration => BaseHPRegeneration + HPRegenerationIncreasePerLevel * (Level - 1) + TotalHPRegenerationIncrements;
        private decimal CarriedHPRegeneration;

        public readonly int BaseSightRange;
        public int TotalSightRangeIncrements { get; set; } = 0;
        public int SightRange => BaseSightRange + TotalSightRangeIncrements;

        public List<AlteredStatus> AlteredStatuses { get; set; }

        public readonly int InventorySize;
        public List<ActionWithEffects> OnTurnStartActions
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnTurnStartActions != null)
                    actionList.AddRange(OwnOnTurnStartActions);
                if (Weapon?.OwnOnTurnStartActions != null)
                    actionList.AddRange(Weapon.OwnOnTurnStartActions);
                if (Armor?.OwnOnTurnStartActions != null)
                    actionList.AddRange(Armor.OwnOnTurnStartActions);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnAttackActions != null && i?.EntityType != EntityType.Weapon && i?.EntityType != EntityType.Armor)
                        actionList.AddRange(i.OwnOnTurnStartActions);
                });
                return actionList;
            }
        }
        public List<ActionWithEffects> OnAttackActions
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnAttackActions != null)
                    actionList.AddRange(OwnOnAttackActions);
                if (Weapon?.OwnOnAttackActions != null)
                    actionList.AddRange(Weapon.OwnOnAttackActions);
                if (Armor?.OwnOnAttackActions != null)
                    actionList.AddRange(Armor.OwnOnAttackActions);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnAttackActions != null && i?.EntityType != EntityType.Weapon && i?.EntityType != EntityType.Armor)
                        actionList.AddRange(i.OwnOnAttackActions);
                });
                return actionList;
            }
        }
        public List<ActionWithEffects> OnAttackedActions
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnAttackedActions != null)
                    actionList.AddRange(OwnOnAttackedActions);
                if (Weapon?.OwnOnAttackedActions != null)
                    actionList.AddRange(Weapon.OwnOnAttackedActions);
                if (Armor?.OwnOnAttackedActions != null)
                    actionList.AddRange(Armor.OwnOnAttackedActions);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnAttackedActions != null && i?.EntityType != EntityType.Weapon && i?.EntityType != EntityType.Armor)
                        actionList.AddRange(i.OwnOnAttackedActions);
                });
                return actionList;
            }
        }
        public List<ActionWithEffects> OnDeathActions
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnDeathActions != null)
                    actionList.AddRange(OwnOnDeathActions);
                if (Weapon?.OwnOnDeathActions != null)
                    actionList.AddRange(Weapon.OwnOnDeathActions);
                if (Armor?.OwnOnDeathActions != null)
                    actionList.AddRange(Armor.OwnOnDeathActions);
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnAttackActions != null && i?.EntityType != EntityType.Weapon && i?.EntityType != EntityType.Armor)
                        actionList.AddRange(i.OwnOnDeathActions);
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

        public Character(EntityClass entityClass, int level, Map map) : base(entityClass, map)
        {
            Faction = entityClass.Faction;
            StartingWeaponId = entityClass.StartingWeaponId;
            StartingArmorId = entityClass.StartingArmorId;
            HP = entityClass.BaseHP;
            BaseMaxHP = entityClass.BaseHP;
            MaxHPIncreasePerLevel = entityClass.MaxHPIncreasePerLevel;
            BaseAttack = entityClass.BaseAttack;
            AttackIncreasePerLevel = entityClass.AttackIncreasePerLevel;
            BaseDefense = entityClass.BaseDefense;
            DefenseIncreasePerLevel = entityClass.DefenseIncreasePerLevel;
            BaseMovement = entityClass.BaseMovement;
            MovementIncreasePerLevel = entityClass.MovementIncreasePerLevel;
            BaseHPRegeneration = entityClass.BaseHPRegeneration;
            HPRegenerationIncreasePerLevel = entityClass.HPRegenerationIncreasePerLevel;
            ExperiencePayoutFormula = entityClass.ExperiencePayoutFormula;
            ExperienceToLevelUpFormula = entityClass.ExperienceToLevelUpFormula;
            CanGainExperience = entityClass.CanGainExperience;
            Level = level;
            MaxLevel = entityClass.MaxLevel;
            if (Level > 1)
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
            AttackModifications = new List<StatModification>();
            DefenseModifications = new List<StatModification>();
            MovementModifications = new List<StatModification>();
            HPRegenerationModifications = new List<StatModification>();

            BaseSightRange = entityClass.BaseSightRange;
            InventorySize = entityClass.InventorySize;
            Inventory = new List<Item>(InventorySize);

            AlteredStatuses = new List<AlteredStatus>();

            LastLevelUpExperience = Experience;
            CarriedHPRegeneration = 0;

            OwnOnTurnStartActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnTurnStartActions, OwnOnTurnStartActions);
            OwnOnAttackActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnAttackActions, OwnOnAttackActions);
            OwnOnAttackedActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnAttackedActions, OwnOnAttackedActions);
            OwnOnDeathActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnDeathActions, OwnOnDeathActions);
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
                    if (ContainingTile.Type == TileType.Hallway)
                        return Map.GetFOVTilesWithinDistance(Position, Math.Max(SightRange / 6, 1));
                    return Map.GetFOVTilesWithinDistance(Position, SightRange);
                }
            }
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            OnAttackActions?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            OnAttackedActions?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            OnTurnStartActions?.Where(a => a.CooldownBetweenUses > 0 && a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            MaxHPModifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            AttackModifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            DefenseModifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            MovementModifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            HPRegenerationModifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            AlteredStatuses?.Where(a => a.RemainingTurns != 0).ForEach(als => als.RefreshCooldownsAndUpdateTurnLength());
            MaxHPModifications?.RemoveAll(a => a.RemainingTurns == 0);
            AttackModifications?.RemoveAll(a => a.RemainingTurns == 0);
            DefenseModifications?.RemoveAll(a => a.RemainingTurns == 0);
            MovementModifications?.RemoveAll(a => a.RemainingTurns == 0);
            HPRegenerationModifications?.RemoveAll(a => a.RemainingTurns == 0);
            AlteredStatuses?.RemoveAll(als => als.RemainingTurns == 0);
            Inventory?.ForEach(i => i.RefreshCooldownsAndUpdateTurnLength());
        }

        public void PerformOnTurnStartActions()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            FOVTiles = ComputeFOVTiles();
            CanTakeAction = true;
            var modificationsThatMightBeNeutralized = new List<(List<StatModification> modificationList, string statName, bool mightBeNeutralized)>();
            var alteredStatusesThatMightBeNeutralized = new List<(List<AlteredStatus> alteredStatusList, string statusName, bool mightEnd)>();

            if (this == Map.Player || Map.Player.CanSee(this))
            {
                modificationsThatMightBeNeutralized.AddRange(new List<(List<StatModification> modificationList, string statName, bool mightBeNeutralized)>
                {
                    (MaxHPModifications, Map.Locale["CharacterMaxHPStat"], MaxHPModifications?.Any() == true && MaxHPModifications?.Any(mhm => mhm.RemainingTurns > 1) == false),
                    (AttackModifications, Map.Locale["CharacterAttackStat"], AttackModifications?.Any() == true && AttackModifications?.Any(mhm => mhm.RemainingTurns > 1) == false),
                    (DefenseModifications, Map.Locale["CharacterDefenseStat"], DefenseModifications?.Any() == true && DefenseModifications?.Any(mhm => mhm.RemainingTurns > 1) == false),
                    (MovementModifications, Map.Locale["CharacterMovementStat"], MovementModifications?.Any() == true && MovementModifications?.Any(mhm => mhm.RemainingTurns > 1) == false),
                    (HPRegenerationModifications, Map.Locale["CharacterHPRegenerationStat"], HPRegenerationModifications?.Any() == true && HPRegenerationModifications?.Any(mhm => mhm.RemainingTurns > 1) == false)
                });

                foreach (var alteredStatus in AlteredStatuses)
                {
                    if (AlteredStatuses.Any(als => als.ClassId.Equals(alteredStatus.ClassId) && als.RemainingTurns > 0))
                        alteredStatusesThatMightBeNeutralized.Add((AlteredStatuses.Where(als => als.ClassId.Equals(alteredStatus.ClassId)).ToList(), alteredStatus.Name, !AlteredStatuses.Any(als => als.ClassId.Equals(alteredStatus.ClassId) && als.RemainingTurns > 1)));
                }
            }
            RefreshCooldownsAndUpdateTurnLength();
            OnTurnStartActions.Where(otsa => otsa.CanBeUsed).ForEach(otsa => otsa.Do(otsa.User, this));
            AlteredStatuses?.ForEach(als => als.PerformOnTurnStartActions());
            foreach (var (modificationList, statName, mightBeNeutralized) in modificationsThatMightBeNeutralized)
            {
                if (mightBeNeutralized && modificationList?.All(mhm => mhm.RemainingTurns == 0) == true)
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = Name, StatName = statName }), Color.DeepSkyBlue);
            }
            foreach (var (alteredStatusList, statusName, mightBeNeutralized) in alteredStatusesThatMightBeNeutralized)
            {
                if (mightBeNeutralized && alteredStatusList?.All(mhm => mhm.RemainingTurns == 0) == true)
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = Name, StatusName = statusName }), Color.DeepSkyBlue);
            }
            TryRegenerateHP();
        }

        public void TryRegenerateHP()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (HP > MaxHP) HP = MaxHP;
            if (HP == MaxHP || HPRegeneration == 0)
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
        }

        public bool CanSee(Entity entity)
        {
            return entity.Visible
                && entity.Position != null
                && entity.ContainingTile != null
                && entity.ContainingTile.Discovered
                && entity.ContainingTile.Visible
                && ComputeFOVTiles().Contains(entity.ContainingTile);
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
            }
        }

        private int ParseArgForFormulaAndCalculate(string arg, bool capIfLevelIsMax)
        {
            if (Level == MaxLevel && capIfLevelIsMax) return Experience;
            var parsedArg = arg.ToLowerInvariant();

            parsedArg = parsedArg.Replace("level", Level.ToString());

            return new Expression(parsedArg).Eval<int>();
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
            var successfulEffects = action.Do(this, target);
            if(Constants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                target.AttackedBy(this);
            RemainingMovement = 0;
        }

        public virtual void AttackedBy(Character source)
        {
            OnAttackedActions.ForEach(oaa => oaa.Do(this, source));
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

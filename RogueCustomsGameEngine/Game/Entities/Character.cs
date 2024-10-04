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
using GamePoint = RogueCustomsGameEngine.Utils.Representation.GamePoint;
using System.Numerics;

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
        public List<Key> KeySet { get; set; }
        public List<IPickable> FullInventory => Inventory.Cast<IPickable>().Union(KeySet.Cast<IPickable>()).ToList();

        public int ItemCount => Inventory.Where(i => i.EntityType != EntityType.Key).Count();

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
        public bool UsesHunger { get; set; }
        public Stat HP { get; set; }
        public decimal MaxHP => HP.BaseAfterModifications;
        public Stat HPRegeneration { get; set; }
        public Stat MP { get; set; }
        public decimal MaxMP => MP != null ? MP.BaseAfterModifications : 0;
        public Stat MPRegeneration { get; set; }
        public Stat Hunger { get; set; }
        public decimal MaxHunger => Hunger != null ? Hunger.BaseAfterModifications : 0;
        public Stat HungerDegeneration { get; set; }
        public bool IsStarving => Hunger.Current <= 0;
        public Stat Attack { get; set; }
        public string Damage
        {
            get
            {
                if (Attack.BaseAfterModifications >= 0)
                    return $"{Weapon.Power}+{Attack.BaseAfterModifications}";
                return $"{Weapon.Power}-{Math.Abs((int)Attack.BaseAfterModifications)}";
            }
        }

        public Stat Defense { get; set; }
        public string Mitigation
        {
            get
            {
                if (Defense.BaseAfterModifications >= 0)
                    return $"{Armor.Power}+{Defense.BaseAfterModifications}";
                return $"{Armor.Power}-{Math.Abs((int)Defense.BaseAfterModifications)}";
            }
        }

        public Stat Movement { get; set; }
        public int RemainingMovement { get; set; }
        public Stat Accuracy { get; set; }
        public Stat Evasion { get; set; }

        private List<Stat> BasicStats => new() { HP, HPRegeneration, MP, MPRegeneration, Hunger, HungerDegeneration, Attack, Defense, Movement, Accuracy, Evasion };

        public List<Stat> Stats => BasicStats.Where(s => s != null).ToList();

        public readonly int BaseSightRange;
        public int TotalSightRangeIncrements { get; set; } = 0;
        public int SightRange => BaseSightRange + TotalSightRangeIncrements;

        public bool TookAction { get; set; }
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
                KeySet?.ForEach(k =>
                {
                    if (k?.OwnOnAttack != null)
                        actionList.AddRange(k.OwnOnAttack);
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

                modifications.Add((HP.Name, HP.ActiveModifications));
                modifications.Add((HPRegeneration.Name, HPRegeneration.ActiveModifications));
                if (UsesMP)
                {
                    modifications.Add((MP.Name, MP.ActiveModifications));
                    modifications.Add((MPRegeneration.Name, MPRegeneration.ActiveModifications));
                }
                modifications.Add((Attack.Name, Attack.ActiveModifications));
                modifications.Add((Defense.Name, Defense.ActiveModifications));
                modifications.Add((Movement.Name, Movement.ActiveModifications));
                modifications.Add((Accuracy.Name, Accuracy.ActiveModifications));
                modifications.Add((Evasion.Name, Evasion.ActiveModifications));

                return modifications;
            }
        }

        protected Character(EntityClass entityClass, int level, Map map) : base(entityClass, map)
        {
            Faction = entityClass.Faction;
            StartingWeaponId = entityClass.StartingWeaponId;
            StartingArmorId = entityClass.StartingArmorId;
            UsesMP = entityClass.UsesMP;
            UsesHunger = entityClass.UsesHunger;
            HP = new()
            {
                Id = "HP",
                StatType = StatType.HP,
                Name = Map.Locale["CharacterHPStat"],
                Base = entityClass.BaseHP,
                Current = entityClass.BaseHP,
                HasMax = true,
                IsDecimal = false,
                IncreasePerLevel = entityClass.MaxHPIncreasePerLevel,
                MinCap = 0,
                MaxCap = Constants.RESOURCE_STAT_CAP,
                Character = this
            };
            HPRegeneration = new()
            {
                Id = "HPRegeneration",
                StatType = StatType.Regeneration,
                Name = Map.Locale["CharacterHPRegenerationStat"],
                Base = entityClass.BaseHPRegeneration,
                Current = entityClass.BaseHPRegeneration,
                HasMax = false,
                IsDecimal = true,
                IncreasePerLevel = entityClass.HPRegenerationIncreasePerLevel,
                MinCap = 0,
                MaxCap = Constants.REGEN_STAT_CAP,
                Character = this,
                RegenerationTarget = HP
            };
            if(UsesMP)
            {
                MP = new()
                {
                    Id = "MP",
                    StatType = StatType.MP,
                    Name = Map.Locale["CharacterMPStat"],
                    Base = entityClass.BaseMP,
                    Current = entityClass.BaseMP,
                    HasMax = true,
                    IsDecimal = false,
                    IncreasePerLevel = entityClass.MaxMPIncreasePerLevel,
                    MinCap = 0,
                    MaxCap = Constants.RESOURCE_STAT_CAP,
                    Character = this
                };
                MPRegeneration = new()
                {
                    Id = "MPRegeneration",
                    StatType = StatType.Regeneration,
                    Name = Map.Locale["CharacterMPRegenerationStat"],
                    Base = entityClass.BaseMPRegeneration,
                    Current = entityClass.BaseMPRegeneration,
                    HasMax = false,
                    IsDecimal = true,
                    IncreasePerLevel = entityClass.MPRegenerationIncreasePerLevel,
                    MinCap = 0,
                    MaxCap = Constants.REGEN_STAT_CAP,
                    Character = this,
                    RegenerationTarget = MP
                };
            }
            if (UsesHunger)
            {
                Hunger = new()
                {
                    Id = "Hunger",
                    StatType = StatType.Hunger,
                    Name = Map.Locale["CharacterHungerStat"],
                    Base = entityClass.BaseHunger,
                    Current = entityClass.BaseHunger,
                    HasMax = true,
                    IsDecimal = false,
                    IncreasePerLevel = 0,
                    MinCap = 0,
                    MaxCap = Constants.RESOURCE_STAT_CAP,
                    Character = this
                };
                HungerDegeneration = new()
                {
                    Id = "HungerDegeneration",
                    StatType = StatType.Regeneration,
                    Name = Map.Locale["CharacterHungerRegenerationStat"],
                    Base = 0,
                    Current = 0,
                    HasMax = false,
                    IsDecimal = true,
                    IncreasePerLevel = 0,
                    MinCap = -Constants.REGEN_STAT_CAP,
                    MaxCap = Constants.REGEN_STAT_CAP,
                    Character = this,
                    RegenerationTarget = Hunger
                };
            }
            Attack = new()
            {
                Id = "Attack",
                StatType = StatType.Attack,
                Name = Map.Locale["CharacterAttackStat"],
                Base = entityClass.BaseAttack,
                Current = entityClass.BaseAttack,
                HasMax = false,
                IsDecimal = false,
                IncreasePerLevel = entityClass.AttackIncreasePerLevel,
                MinCap = 0,
                MaxCap = Constants.NORMAL_STAT_CAP,
                Character = this
            };
            Defense = new()
            {
                Id = "Defense",
                StatType = StatType.Defense,
                Name = Map.Locale["CharacterDefenseStat"],
                Base = entityClass.BaseDefense,
                Current = entityClass.BaseDefense,
                HasMax = false,
                IsDecimal = false,
                IncreasePerLevel = entityClass.DefenseIncreasePerLevel,
                MinCap = 0,
                MaxCap = Constants.NORMAL_STAT_CAP,
                Character = this
            };
            Movement = new()
            {
                Id = "Movement",
                StatType = StatType.Movement,
                Name = Map.Locale["CharacterMovementStat"],
                Base = entityClass.BaseMovement,
                Current = entityClass.BaseMovement,
                HasMax = false,
                IsDecimal = false,
                IncreasePerLevel = entityClass.MovementIncreasePerLevel,
                MinCap = 0,
                MaxCap = Constants.MOVEMENT_STAT_CAP,
                Character = this
            };
            Accuracy = new()
            {
                Id = "Accuracy",
                StatType = StatType.Accuracy,
                Name = Map.Locale["CharacterAccuracyStat"],
                Base = entityClass.BaseAccuracy,
                Current = entityClass.BaseAccuracy,
                HasMax = false,
                IsDecimal = false,
                IncreasePerLevel = 0,
                MinCap = Constants.MIN_ACCURACY_CAP,
                MaxCap = Constants.MAX_ACCURACY_CAP,
                Character = this
            };
            Evasion = new()
            {
                Id = "Evasion",
                StatType = StatType.Evasion,
                Name = Map.Locale["CharacterEvasionStat"],
                Base = entityClass.BaseEvasion,
                Current = entityClass.BaseEvasion,
                HasMax = false,
                IsDecimal = false,
                IncreasePerLevel = 0,
                MinCap = Constants.MIN_EVASION_CAP,
                MaxCap = Constants.MAX_EVASION_CAP,
                Character = this
            };
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

            BaseSightRange = entityClass.BaseSightRange;
            InventorySize = entityClass.InventorySize;
            Inventory = new List<Item>(InventorySize);
            KeySet = new List<Key>();

            AlteredStatuses = new List<AlteredStatus>();

            LastLevelUpExperience = Experience;

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
                    modificationsThatMightBeNeutralized.Add((modifications, statName, modifications.Any() && modifications.Exists(mhm => mhm.RemainingTurns == 1)));
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
                {
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = Name, StatName = statName }), Color.DeepSkyBlue);
                    if (EntityType == EntityType.Player)
                    {
                        var totalNeutralization = modificationList?.Where(mhm => mhm.RemainingTurns == 0).Sum(mhm => mhm.Amount);
                        if(totalNeutralization < 0)
                            Map.AddSpecialEffectIfPossible(SpecialEffect.StatBuff);
                        else if (totalNeutralization > 0)
                            Map.AddSpecialEffectIfPossible(SpecialEffect.StatNerf);
                    }
                }
            }
            foreach (var (alteredStatusList, statusName, mightBeNeutralized) in alteredStatusesThatMightBeNeutralized)
            {
                if (mightBeNeutralized && alteredStatusList?.TrueForAll(mhm => mhm.RemainingTurns == 0) == true)
                {
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = Name, StatusName = statusName }), Color.DeepSkyBlue);
                    if (EntityType == EntityType.Player)
                        Map.AddSpecialEffectIfPossible(SpecialEffect.StatusLeaves);
                }
            }
            foreach (var regeneration in Stats.Where(s => s.RegenerationTarget != null))
            {
                regeneration.TryToRegenerate();
            }
            if (HP.Current <= 0)
                Die();
        }


        public bool CanSee(Entity entity)
        {
            return this == entity
                || (entity.Visible
                && entity.Position != null
                && ComputeFOVTiles().Contains(entity.ContainingTile));
        }

        public bool HasNoObstructionsTowards(Entity entity)
        {
            if (this == entity) return true;
            if (this.Position == null || entity.Position == null) return false;
            var tilesInTheLine = MathAlgorithms.BresenhamLine(ContainingTile, entity.ContainingTile, t => t.Position.X, t => t.Position.Y, (x, y) => Map.GetTileFromCoordinates(x, y), t => t.IsWalkable);
            return !tilesInTheLine.Any(t => !t.IsWalkable);
        }

        public void GainExperience(int GamePointsToAdd)
        {
            if (!CanGainExperience) return;
            Experience += GamePointsToAdd;
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
                HP.Current = MaxHP;
                if(MP != null)
                    MP.Current = MaxMP;
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

        public abstract void PickItem(Item item, bool informToPlayer);
        public abstract void PickKey(Key key, bool informToPlayer);

        public void EquipItem(Item item)
        {
            if (!item.IsEquippable)
                throw new InvalidOperationException("Attempted to equip an unequippable item!");

            var currentlyEquippedItemInSlot = item.EntityType == EntityType.Weapon ? EquippedWeapon : EquippedArmor;
            SwapWithEquippedItem(currentlyEquippedItemInSlot, item);
            if (this == Map.Player)
                Map.AddSpecialEffectIfPossible(SpecialEffect.ItemEquip);
        }

        private void SwapWithEquippedItem(Item equippedItem, Item itemToEquip)
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

        public void TryToPickItem(IPickable p)
        {
            if (!Visible) return;
            var item = p as Item;
            var key = p as Key;
            if(item == null && key == null) return;
            if(key != null && EntityType == EntityType.Player)
            {
                PickKey(key, true);
            }
            else if (item != null)
            {
                if(ItemCount < InventorySize)
                {
                    PickItem(item, true);
                }
                else
                {
                    Map.AppendMessage(Map.Locale["ItemSteppedText"].Format(new { CharacterName = Name, ItemName = item.Name }));
                }
            }
        }

        public void AttackCharacter(Character target, ActionWithEffects action)
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            AlteredStatuses.Where(als => als.RemainingTurns > 0).ForEach(als => als.BeforeAttack?.Do(this, target, true));
            if (UsesMP)
                MP.Current = Math.Max(0, MP.Current - action.MPCost);
            var successfulEffects = action?.Do(this, target, true);
            if(successfulEffects != null && Constants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                target.AttackedBy(this);
            if(action?.FinishesTurnWhenUsed == true)
                TookAction = true;
            RemainingMovement = 0;
        }

        public void InteractWithCharacter(Character target, ActionWithEffects action)
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (UsesMP)
                MP.Current = Math.Max(0, MP.Current - action.MPCost);
            action?.Do(this, target, true);
            if (action?.FinishesTurnWhenUsed == true)
                TookAction = true;
            RemainingMovement = 0;
        }

        public void InteractWithTile(Tile target, ActionWithEffects action)
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (UsesMP)
                MP.Current = Math.Max(0, MP.Current - action.MPCost);
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

        public virtual void Die(Entity? attacker = null)
        {
            if (attacker == null || attacker is Character)
                OnDeath?.Where(oda => attacker == null || oda?.ChecksCondition(this, attacker as Character) == true).ForEach(oda => oda?.Do(this, attacker, true));
            if(HP.Current <= 0)
            {
                ExistenceStatus = EntityExistenceStatus.Dead;
                Passable = true;
                StatModifications.ForEach(m => m.Modifications.Clear());
                AlteredStatuses.Clear();
            }
        }
    }
}

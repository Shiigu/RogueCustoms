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
using System.Security.Cryptography;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text;

namespace RogueCustomsGameEngine.Game.Entities
{
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    [Serializable]
    public abstract class Character : Entity, IHasActions, IKillable
    {
        public Faction Faction { get; set; }
        public List<ItemSlot> AvailableSlots { get; set; }
        public readonly List<string> InitialEquipmentIds;
        public List<Item> Equipment { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Key> KeySet { get; set; }
        public List<IPickable> FullInventory => Inventory.Cast<IPickable>().Union(KeySet.Cast<IPickable>()).ToList();

        public int ItemCount => Inventory.Where(i => i.EntityType != EntityType.Key).Count();

        public readonly string BaseExperiencePayoutFormula;
        public string ExperiencePayoutFormula { get; set; }
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
        private List<Stat> Stats = new();
        public List<Stat> UsedStats => Stats.Where(s => s != null).ToList();
        public Stat HP => Stats.Find(s => s.Id.Equals("HP", StringComparison.InvariantCultureIgnoreCase));
        public decimal MaxHP => HP.BaseAfterModifications;
        public Stat HPRegeneration => Stats.Find(s => s.Id.Equals("HPRegeneration", StringComparison.InvariantCultureIgnoreCase));
        public Stat MP => Stats.Find(s => s.Id.Equals("MP", StringComparison.InvariantCultureIgnoreCase));
        public decimal MaxMP => MP != null ? MP.BaseAfterModifications : 0;
        public Stat MPRegeneration => Stats.Find(s => s.Id.Equals("MPRegeneration", StringComparison.InvariantCultureIgnoreCase));
        public Stat Hunger => Stats.Find(s => s.Id.Equals("Hunger", StringComparison.InvariantCultureIgnoreCase));
        public decimal MaxHunger => Hunger != null ? Hunger.BaseAfterModifications : 0;
        public Stat HungerDegeneration { get; set; }
        public bool IsStarving => Hunger != null && Hunger.Current <= 0;
        public Stat Attack => Stats.Find(s => s.Id.Equals("Attack", StringComparison.InvariantCultureIgnoreCase));
        public string Damage
        {
            get
            {
                if (Attack.BaseAfterModifications >= 0)
                    return $"{DamageFromEquipment}+{Attack.BaseAfterModifications}";
                return $"{DamageFromEquipment}-{Math.Abs((int)Attack.BaseAfterModifications)}";
            }
        }
        public string DamageFromEquipment
        {
            get
            {
                var damageParts = Equipment.Where(i => i.ItemType.PowerType == ItemPowerType.Damage).Select(i => i.Power);
                var damageFormula = string.Join("+", damageParts);

                var reducedFormula = damageFormula.Replace("+-", "-").ReduceDiceNotation();

                return string.IsNullOrWhiteSpace(reducedFormula) ? "0" : reducedFormula;
            }
        }

        public Stat Defense => Stats.Find(s => s.Id.Equals("Defense", StringComparison.InvariantCultureIgnoreCase));
        public string Mitigation
        {
            get
            {
                if (Defense.BaseAfterModifications >= 0)
                    return $"{MitigationFromEquipment}+{Defense.BaseAfterModifications}";
                return $"{MitigationFromEquipment}-{Math.Abs((int)Defense.BaseAfterModifications)}";
            }
        }
        public string MitigationFromEquipment
        {
            get
            {
                var mitigationParts = Equipment.Where(i => i.ItemType.PowerType == ItemPowerType.Mitigation).Select(i => i.Power);
                var mitigationFormula = string.Join("+", mitigationParts);

                var reducedFormula = mitigationFormula.Replace("+-", "-").ReduceDiceNotation();

                return string.IsNullOrWhiteSpace(reducedFormula) ? "0" : reducedFormula;
            }
        }

        public Stat Movement => Stats.Find(s => s.Id.Equals("Movement", StringComparison.InvariantCultureIgnoreCase));
        public int RemainingMovement { get; set; }
        public Stat Accuracy => Stats.Find(s => s.Id.Equals("Accuracy", StringComparison.InvariantCultureIgnoreCase));
        public Stat Evasion => Stats.Find(s => s.Id.Equals("Evasion", StringComparison.InvariantCultureIgnoreCase));

        public readonly int BaseSightRange;
        public StatModification SightRangeModification { get; set; }
        public int SightRange
        {
            get
            {
                if (SightRangeModification == null)
                    return BaseSightRange;
                return (int) SightRangeModification.Amount;
            }
        }

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
                Equipment?.ForEach(i =>
                {
                    if (i.IsEquippable && i?.OnTurnStart != null)
                        actionList.AddRange(i?.OnTurnStart);
                });
                Inventory?.ForEach(i =>
                {
                    if (!i.IsEquippable && i?.OnTurnStart != null)
                        actionList.AddRange(i?.OnTurnStart);
                });
                return actionList;
            }
        }
        public ActionWithEffects DefaultOnAttack { get; private set; }
        public List<ActionWithEffects> OnAttack
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnAttack != null)
                    actionList.AddRange(OwnOnAttack);
                Equipment?.ForEach(i =>
                {
                    if (i.IsEquippable && i?.OnAttack != null)
                        actionList.AddRange(i?.OnAttack);
                });
                Inventory?.ForEach(i =>
                {
                    if (!i.IsEquippable && i?.OnAttack != null)
                        actionList.AddRange(i?.OnAttack);
                });
                KeySet?.ForEach(k =>
                {
                    if (k?.OwnOnAttack != null)
                        actionList.AddRange(k.OwnOnAttack);
                });

                return actionList.Count > 0 ? actionList : [DefaultOnAttack];
            }
        }
        public List<ActionWithEffects> OnAttacked
        {
            get
            {
                var actionList = new List<ActionWithEffects>();
                if (OwnOnAttacked != null)
                    actionList.Add(OwnOnAttacked);
                Equipment?.ForEach(i =>
                {
                    if (i.IsEquippable && i?.OnAttacked != null)
                        actionList.AddRange(i?.OnAttacked);
                });
                Inventory?.ForEach(i =>
                {
                    if (!i.IsEquippable && i?.OnAttacked != null)
                        actionList.AddRange(i?.OnAttacked);
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
                Equipment?.ForEach(i =>
                {
                    if (i?.OwnOnDeath != null && i.IsEquippable)
                        actionList.Add(i.OwnOnDeath);
                });
                Inventory?.ForEach(i =>
                {
                    if (i?.OwnOnDeath != null && !i.IsEquippable)
                        actionList.Add(i.OwnOnDeath);
                });
                return actionList;
            }
        }

        public ActionWithEffects OnLevelUp { get; private set; }

        private HashSet<Tile> _fovTiles;
        public HashSet<Tile> FOVTiles
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

                foreach (var stat in UsedStats)
                {
                    modifications.Add((stat.Name, stat.ActiveModifications));
                }

                return modifications;
            }
        }

        public List<GamePoint> LatestPositions { get; private set; } = new List<GamePoint>(4);

        [JsonIgnore]
        public bool PickedForSwap { get; set; } = false;
        public int CurrencyCarried { get; set; }

        public List<ExtraDamage> ExtraDamage
        {
            get
            {
                var list = new List<ExtraDamage>();
                foreach (var item in Equipment)
                {
                    if (!item.IsEquippable) continue;
                    foreach (var extraDamage in item?.ExtraDamage ?? [])
                    {
                        var correspondingExtraDamage = list.Find(ed => ed.Element.Id.Equals(extraDamage.Element.Id, StringComparison.InvariantCultureIgnoreCase));
                        if (correspondingExtraDamage == null)
                        {
                            list.Add(extraDamage);
                        }
                        else
                        {
                            correspondingExtraDamage.MinimumDamage += extraDamage.MinimumDamage;
                            correspondingExtraDamage.MaximumDamage += extraDamage.MaximumDamage;
                        }
                    }
                }
                foreach (var item in Inventory)
                {
                    if (item.IsEquippable) continue;
                    foreach (var extraDamage in item?.ExtraDamage ?? [])
                    {
                        var correspondingExtraDamage = list.Find(ed => ed.Element.Id.Equals(extraDamage.Element.Id, StringComparison.InvariantCultureIgnoreCase));
                        if (correspondingExtraDamage == null)
                        {
                            list.Add(extraDamage);
                        }
                        else
                        {
                            correspondingExtraDamage.MinimumDamage += extraDamage.MinimumDamage;
                            correspondingExtraDamage.MaximumDamage += extraDamage.MaximumDamage;
                        }
                    }
                }
                return list;
            }
        }

        protected Character(EntityClass entityClass, int level, Map map) : base(entityClass, map)
        {
            Faction = entityClass.Faction;
            Stats = new();
            foreach (var stat in entityClass.Stats)
            {
                var clonedStat = stat.Clone();
                clonedStat.Character = this;
                Stats.Add(clonedStat);
            }
            foreach (var stat in Stats)
            {
                var correspondingRegenerationTarget = Stats.Find(s => s.Id.Equals(stat.RegenerationTargetId, StringComparison.InvariantCultureIgnoreCase));
                stat.RegenerationTarget = correspondingRegenerationTarget;
            }
            if (Hunger != null)
            {
                HungerDegeneration = new()
                {
                    Id = "HungerDegeneration",
                    StatType = StatType.Regeneration,
                    Name = string.Empty,
                    Base = 0,
                    Current = 0,
                    HasMax = false,
                    IncreasePerLevel = 0,
                    MinCap = -EngineConstants.REGEN_STAT_CAP,
                    MaxCap = EngineConstants.REGEN_STAT_CAP,
                    Character = this,
                    RegenerationTarget = Hunger
                };
                Stats.Add(HungerDegeneration);
            }
            BaseExperiencePayoutFormula = entityClass.ExperiencePayoutFormula;
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
            OnLevelUp = MapClassAction(entityClass.OnLevelUp);
            AvailableSlots = entityClass.AvailableSlots;
            DefaultOnAttack = MapClassAction(entityClass.DefaultOnAttack);
            InitialEquipmentIds = new(entityClass.InitialEquipmentIds);
            Equipment = [];
            CurrencyCarried = 0;
        }

        public HashSet<Tile> ComputeFOVTiles()
        {
            if (SightRange == EngineConstants.FullMapSightRange)
            {
                return new HashSet<Tile>(Map.Tiles.ToList());
            }
            else
            {
                if (SightRange == EngineConstants.FullRoomSightRange)
                {
                    if (ContainingTile.Type == TileType.Hallway || ContainingRoom == null || ContainingTile.IsConnectorTile)
                        return new HashSet<Tile>(Map.GetFOVTilesWithinDistance(Position, EngineConstants.FullRoomSightRangeForHallways));
                    return new HashSet<Tile>(Map.GetTilesInRoom(ContainingRoom));
                }
                else
                {
                    return new HashSet<Tile>(Map.GetFOVTilesWithinDistance(Position, SightRange));
                }
            }
        }

        public async Task RefreshCooldownsAndUpdateTurnLength()
        {
            if(SightRangeModification != null)
            {
                SightRangeModification.RemainingTurns--;
                if(SightRangeModification.RemainingTurns == 0)
                    SightRangeModification = null;
            }
            OnAttack?.Where(a => a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            OnAttacked?.Where(a => a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            OnTurnStart?.Where(a => a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            foreach (var modification in StatModifications)
            {
                modification.Modifications?.Where(a => a.RemainingTurns > 0).ForEach(a => a.RemainingTurns--);
            }
            AlteredStatuses?.Where(a => a.RemainingTurns != 0).ForEach(als => als.RefreshCooldownsAndUpdateTurnLength());
            foreach (var modification in StatModifications)
            {
                if(this == Map.Player && modification.Modifications?.Any(a => a.RemainingTurns == 0) == true)
                {
                    var stat = modification.StatName != null ? UsedStats.Find(s => s.Name.Equals(modification.StatName)) : null;
                    if (stat != null)
                    {
                        var events = new List<DisplayEventDto>();
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.ModifyStat, stat.Id, stat.BaseAfterModifications }
                        });
                        if (stat.HasMax)
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdatePlayerData,
                                Params = new() { UpdatePlayerDataType.ModifyMaxStat, stat.Id, stat.BaseAfterModifications }
                            });
                        }
                        Map.DisplayEvents.Add(($"Update player {stat.Name} display data", events));
                    }
                }
                modification.Modifications?.RemoveAll(a => a.RemainingTurns == 0);
            }
            foreach (var als in AlteredStatuses?.Where(a => a.RemainingTurns == 0 && !a.FlaggedToRemove && a.OnRemove != null))
            {
                await als.OnRemove.Do(als, this, false);
            }
            var alteredStatusesBeforeUpdate = AlteredStatuses.Count;
            AlteredStatuses?.RemoveAll(als => als.RemainingTurns == 0);
            if(this == Map.Player && alteredStatusesBeforeUpdate > AlteredStatuses.Count)
            {
                Map.DisplayEvents.Add(("Update player Altered Status display data", new List<DisplayEventDto>
                {
                    new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.UpdateAlteredStatuses, this.AlteredStatuses.Select(als => new SimpleEntityDto(als)).ToList() }
                    }
                }));
            }
            Inventory?.ForEach(i => i.RefreshCooldownsAndUpdateTurnLength());
        }

        public async Task PerformOnTurnStart()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;
            FOVTiles = ComputeFOVTiles();
            CanTakeAction = true;
            var hadSightRangeModification = SightRangeModification != null;
            var hadSightRangeModificationToInform = hadSightRangeModification && SightRangeModification.InformOfExpiration;
            var priorSightRange = hadSightRangeModificationToInform ? SightRange : BaseSightRange;
            var modificationsThatMightBeNeutralized = new List<(List<StatModification> modificationList, string statName, bool mightBeNeutralized)>();
            var alteredStatusesThatMightBeNeutralized = new List<(List<AlteredStatus> alteredStatusList, string statusId, string statusName, bool mightEnd)>();

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
                        group.First().ClassId,
                        group.FirstOrDefault()?.Name ?? "",
                        !group.Any(als => als.RemainingTurns > 1)
                    ))
                    .ToList();
            }
            await RefreshCooldownsAndUpdateTurnLength();
            foreach (var otsa in OnTurnStart.Where(otsa => otsa != null && otsa.ChecksCondition(this, this)))
            {
                await otsa.Do(otsa.User, this, true);
            }
            await Task.WhenAll(AlteredStatuses?.Select(als => als.PerformOnTurnStart()));
            foreach (var (modificationList, statName, mightBeNeutralized) in modificationsThatMightBeNeutralized)
            {
                if (mightBeNeutralized && modificationList?.TrueForAll(mhm => mhm.RemainingTurns == 0) == true)
                {
                    var events = new List<DisplayEventDto>();
                    if (this == Map.Player || Map.Player.CanSee(this))
                    {
                        var stat = UsedStats.Find(s => s.Name.Equals(statName));
                        var totalNeutralization = modificationList?.Where(mhm => mhm.RemainingTurns == 0).Sum(mhm => mhm.Amount);
                        if (this == Map.Player && statName.ToLowerInvariant().Equals("movement"))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.SetCanMove,
                                Params = new() { stat.BaseAfterModifications > 0 }
                            });
                        }
                        if (totalNeutralization < 0)
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.StatBuff }
                            });
                        }
                        else if (totalNeutralization > 0)
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.StatNerf }
                            });
                        }
                    }
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = Name, StatName = statName }), Color.DeepSkyBlue, events);
                    Map.DisplayEvents.Add(($"{Name} lost all the {statName} modifications", events));
                }
            }
            if(SightRangeModification == null && hadSightRangeModification)
            {
                var events = new List<DisplayEventDto>();
                if(hadSightRangeModificationToInform)
                {
                    var sightRangeWasHigher = (priorSightRange == EngineConstants.FullMapSightRange && BaseSightRange != EngineConstants.FullMapSightRange)
                        || (priorSightRange == EngineConstants.FullRoomSightRange && BaseSightRange != EngineConstants.FullMapSightRange && BaseSightRange != EngineConstants.FullRoomSightRange)
                        || priorSightRange > BaseSightRange;
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { sightRangeWasHigher ? SpecialEffect.StatNerf : SpecialEffect.StatBuff }
                    });
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterSightRangeStat"] }), Color.DeepSkyBlue, events);
                }
                if (this == Map.Player || Map.Player.CanSee(this))
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.RedrawMap,
                        Params = new() { Map.Snapshot.GetTiles() }
                    });
                }
                Map.DisplayEvents.Add(($"{Name} sight range returned to normal", events));
            }
            var gotToUpdateStatuses = false;
            foreach (var (alteredStatusList, statusId, statusName, mightBeNeutralized) in alteredStatusesThatMightBeNeutralized)
            {
                if (mightBeNeutralized && alteredStatusList?.TrueForAll(mhm => mhm.RemainingTurns == 0) == true)
                {
                    var events = new List<DisplayEventDto>();

                    if (this == Map.Player || Map.Player.CanSee(this))
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.StatusLeaves }
                        });
                        if (this == Map.Player)
                            gotToUpdateStatuses = true;
                    }
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = Name, StatusName = statusName }), Color.DeepSkyBlue, events);
                    Map.DisplayEvents.Add(($"{Name} lost the {statusName} status", events));
                }
            }
            foreach (var regeneration in Stats.Where(s => s.RegenerationTarget != null))
            {
                await regeneration.TryToRegenerate();
            }
            if (HP.Current <= 0)
            {
                if(ExistenceStatus == EntityExistenceStatus.Alive)
                    await Die();
                ExistenceStatus = EntityExistenceStatus.Dead;
                Passable = true;
                StatModifications.ForEach(m => m.Modifications.Clear());
                AlteredStatuses.Clear();
            }
        }

        public bool CanSee(Entity entity)
        {
            return this == entity
                || (entity.Visible
                && entity.Position != null
                && ComputeFOVTiles().Contains(entity.ContainingTile))
                && (!entity.ContainingTile.Type.CausesPartialInvisibility || (entity.ContainingTile.Type == ContainingTile.Type))
                && (!ContainingTile.Type.CausesPartialInvisibility || (entity.ContainingTile.Type == ContainingTile.Type));
        }

        public bool CanSee(Tile tile)
        {
            return ContainingTile == tile || ComputeFOVTiles().Contains(tile);
        }

        public bool HasNoObstructionsTowards(Entity entity)
        {
            if (this == entity) return true;
            if (this.Position == null || entity.Position == null) return false;
            var tilesInTheLine = MathAlgorithms.BresenhamLine(ContainingTile, entity.ContainingTile, t => t.Position.X, t => t.Position.Y, (x, y) => Map.GetTileFromCoordinates(x, y), t => t.IsWalkable);
            return !tilesInTheLine.Any(t => !t.IsWalkable);
        }

        public virtual async Task GainExperience(int GamePointsToAdd)
        {
            if (!CanGainExperience) return;
            Experience += GamePointsToAdd;
            if (Experience >= ExperienceToLevelUp)
            {
                LastLevelUpExperience = ExperienceToLevelUp;
                Level++;
                Color forecolorToUse;
                if (this == Map.Player || Faction.IsAlliedWith(Map.Player.Faction))
                    forecolorToUse = Color.Lime;
                else if (Faction.IsEnemyWith(Map.Player.Faction))
                    forecolorToUse = Color.Red;
                else
                    forecolorToUse = Color.DeepSkyBlue;
                Map.AppendMessage(Map.Locale["CharacterLevelsUpMessage"].Format(new { CharacterName = Name, Level = Level }), forecolorToUse);
                HP.Current = MaxHP;
                if(MP != null)
                    MP.Current = MaxMP;
                if (OnLevelUp != null)
                    await OnLevelUp.Do(this, this, false);
            }
        }

        private int ParseArgForFormulaAndCalculate(string arg, bool capIfLevelIsMax)
        {
            if (string.IsNullOrWhiteSpace(arg)) return 0;
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

        public abstract void DropItem(IPickable pickable);
        public abstract void PickItem(IPickable pickable, bool informToPlayer);
        public abstract void PickKey(Key key, bool informToPlayer);
        public abstract void EquipItem(Item item);

        public void TryToPickItem(IPickable p)
        {
            if (EntityType != EntityType.Player && !Visible) return;
            var item = p as Item;
            var key = p as Key;
            var currency = p as Currency;
            if (item == null && key == null && currency == null) return;
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
            else if (currency != null)
            {
                PickItem(currency, true);
            }
        }

        public async Task AttackCharacter(Character target, ActionWithEffects action)
        {
            if (action == null || ExistenceStatus != EntityExistenceStatus.Alive) return;
            foreach (var als in AlteredStatuses.Where(als => als.RemainingTurns > 0 && als.BeforeAttack != null))
            {
                await als.BeforeAttack.Do(this, target, true);
            }
            if (MP != null)
                MP.Current = Math.Max(0, MP.Current - action.MPCost);
            var successfulEffects = await action.Do(this, target, true);
            if (successfulEffects != null && EngineConstants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                await target.AttackedBy(this);
            if (action.FinishesTurnWhenUsed)
            {
                TookAction = true;
                RemainingMovement = 0;
            }
        }

        public async Task InteractWithCharacter(Character target, ActionWithEffects action)
        {
            if (action == null || ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (MP != null)
                MP.Current = Math.Max(0, MP.Current - action.MPCost);
            await action.Do(this, target, true);
            if (action.FinishesTurnWhenUsed)
            {
                TookAction = true;
                RemainingMovement = 0;
            }
        }

        public async Task InteractWithTile(Tile target, ActionWithEffects action)
        {
            if (action == null || ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (MP != null)
                MP.Current = Math.Max(0, MP.Current - action.MPCost);
            await action.Do(this, target, true);
            if (action.FinishesTurnWhenUsed)
            {
                TookAction = true;
                RemainingMovement = 0;
            }
        }

        public virtual async Task AttackedBy(Character source)
        {
            foreach (var oaa in OnAttacked.Where(oaa => oaa.ChecksCondition(this, source) && oaa != null))
            {
                await oaa.Do(this, source, false);
            }
        }

        public TargetType CalculateTargetTypeFor(Character target)
        {
            if (target == this)
                return TargetType.Self;
            if (Faction.IsAlliedWith(target.Faction))
                return TargetType.Ally;
            if (Faction.IsNeutralWith(target.Faction))
                return TargetType.Neutral;
            if (Faction.IsEnemyWith(target.Faction))
                return TargetType.Enemy;
            if (Faction.Id.Equals(target.Faction.Id))
                return TargetType.Neutral;

            throw new InvalidDataException($"Cannot identify target relationship between {Name} and {target.Name}!");
        }

        public virtual async Task Die(Entity? attacker = null)
        {
            foreach (var oda in OnDeath?.Where(oda => oda != null && (attacker == null || attacker is not Character || oda.ChecksCondition(this, attacker as Character) == true)))
            {
                await oda.Do(this, attacker, true);
            }
            if(HP.Current <= 0)
            {
                ExistenceStatus = EntityExistenceStatus.Dead;
                Passable = true;
                if (this is NonPlayableCharacter && attacker is Character c && ExperiencePayout > 0)
                    await GiveExperienceTo(c);
            }
        }

        public async Task GiveExperienceTo(Character character, int? amount = null)
        {
            if (!character.CanGainExperience || character.Level == character.MaxLevel) return;
            var amountToGive = amount ?? ExperiencePayout;
            if (amountToGive == 0) return;
            if (character == Map.Player || Map.Player.CanSee(character))
                Map.AppendMessage(Map.Locale["CharacterGainsExperience"].Format(new { CharacterName = character.Name, Amount = amountToGive.ToString() }), Color.DeepSkyBlue);
            await character.GainExperience(amountToGive);
        }

        public override void SetActionIds()
        {
            for (int i = 0; i < OwnOnAttack.Count; i++)
            {
                OwnOnAttack[i].SelectionId = $"{Id}_{ClassId}_CA{i}_{OwnOnAttack[i].Id}";
                if (OwnOnAttack[i].IsScript)
                    OwnOnAttack[i].SelectionId += "_S";
            }
        }

        public Item ItemInSlot(string slotId)
        {
            var slot = AvailableSlots.Find(s => s.Id.Equals(slotId, StringComparison.InvariantCultureIgnoreCase));
            if (slot == null) return null;
            return Equipment.Find(i => i.ItemType != null && i.ItemType.SlotsItOccupies.Contains(slot));
        }
    }
}
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

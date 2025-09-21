using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using static System.Collections.Specialized.BitVector32;
using RogueCustomsGameEngine.Utils;
using MathNet.Numerics.Statistics.Mcmc;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class Item : Entity, IHasActions, IPickable
    {
        private string _unidentifiedName => ItemType.UnidentifiedItemName;
        private string _unidentifiedDescription => ItemType.UnidentifiedItemDescription;
        private string _trueName;
        private string _trueDescription;
        public string UnidentifiedActionName => ItemType.UnidentifiedItemActionName;
        public string UnidentifiedActionDescription => ItemType.UnidentifiedItemActionDescription;
        public bool GotSpecificallyIdentified { get; set; }
        public bool IsIdentified => GotSpecificallyIdentified || !Map.Player.NeedsToIdentifyItems || IsHeldByAnNPC || DoesNotReachMinimumUnidentifiedMaximumAffixes || HasNoAffixesAndPlayerPreviouslyIdentifiedThisClass;
        public bool IsHeldByThePlayer => Owner != null && Owner == Map.Player;
        public bool IsHeldByAnNPC => Owner != null && Owner != Map.Player;
        public bool DoesNotReachMinimumUnidentifiedMaximumAffixes => ItemType.MinimumQualityLevelForUnidentified == null || (QualityLevel != null && ItemType.MinimumQualityLevelForUnidentified.MaximumAffixes > QualityLevel.MaximumAffixes);
        public bool HasNoAffixesAndPlayerPreviouslyIdentifiedThisClass => QualityLevel != null && QualityLevel.MaximumAffixes == 0 && Map.Player.IdentifiedItemClasses.Contains(ClassId);
        public bool IsEquippable => ItemType.Usability == ItemUsability.Equip;
        public bool IsConsumable => ItemType.Usability == ItemUsability.Use;
        public List<ItemSlot> SlotsItOccupies => ItemType.SlotsItOccupies;
        public bool SpawnedInTheFloor { get; set; }
        public string Power { get; set; }
        public ItemType ItemType { get; set; }
        private List<PassiveStatModifier> OwnStatModifiers { get; set; }
        public List<PassiveStatModifier> StatModifiers
        {
            get
            {
                var list = new List<PassiveStatModifier>();
                foreach (var statModifier in OwnStatModifiers)
                {
                    list.Add(new()
                    {
                        Id = statModifier.Id,
                        Amount = statModifier.Amount
                    });
                }
                if (!IsIdentified) return list;
                foreach (var affix in Affixes)
                {
                    foreach (var statModifier in affix.StatModifiers)
                    {
                        var correspondingStatModifier = list.Find(sm => sm.Id.Equals(statModifier.Id, StringComparison.InvariantCultureIgnoreCase));
                        if (correspondingStatModifier == null)
                        {
                            list.Add(new()
                            {
                                Id = statModifier.Id,
                                Amount = statModifier.Amount
                            });
                        }
                        else
                        {
                            correspondingStatModifier.Amount += statModifier.Amount;
                        }
                    }
                }
                return list;
            }
        }

        public Character Owner => Map.GetCharacters().Find(c => c.ExistenceStatus == EntityExistenceStatus.Alive && (c.Equipment.Contains(this) || c.Inventory.Contains(this)));
        public ActionWithEffects OnUse { get; set; }
        public string BaseName { get; set; }
        public bool CanDrop { get; set; }
        public int BaseValue { get; set; }
        public int Value
        {
            get
            {
                if (!IsIdentified) return Math.Max(1, BaseValue / 2);
                float totalValue = BaseValue;
                foreach (var affix in Affixes)
                {
                    var valueToAdd = (int) Math.Ceiling(BaseValue * (100 + affix.ItemValueModifierPercentage) / 100f);
                    totalValue += valueToAdd;
                }
                return (int) totalValue;
            }
        }
        public int ItemLevel { get; set; }
        public QualityLevel MinimumQualityLevel { get; set; }
        public QualityLevel MaximumQualityLevel { get; set; }
        public List<QualityLevelOdds> QualityLevelOdds { get; set; }
        public QualityLevel QualityLevel { get; set; }
        public List<Affix> Affixes { get; set; }
        public List<ExtraDamage> ExtraDamage
        {
            get
            {
                var list = new List<ExtraDamage>();
                if (!IsIdentified) return list;
                foreach (var affix in Affixes)
                {
                    if (affix.ExtraDamage == null || affix.ExtraDamage.MaximumDamage == 0 || affix.ExtraDamage.Element == null) continue;
                    var correspondingExtraDamage = list.Find(ed => ed.Element.Id.Equals(affix.ExtraDamage.Element.Id, StringComparison.InvariantCultureIgnoreCase));
                    if (correspondingExtraDamage == null)
                    {
                        list.Add(new()
                        {
                            Element = affix.ExtraDamage.Element,
                            MinimumDamage = affix.ExtraDamage.MinimumDamage,
                            MaximumDamage = affix.ExtraDamage.MaximumDamage
                        });
                    }
                    else
                    {
                        correspondingExtraDamage.MinimumDamage += affix.ExtraDamage.MinimumDamage;
                        correspondingExtraDamage.MaximumDamage += affix.ExtraDamage.MaximumDamage;
                    }
                }
                return list;
            }
        }

        public List<ActionWithEffects> OnTurnStart
        {
            get
            {
                var list = new List<ActionWithEffects>();
                if (OwnOnTurnStart != null)
                    list.AddRange(OwnOnTurnStart);
                if (!IsIdentified) return list;

                foreach (var affix in Affixes)
                {
                    if (affix.OwnOnTurnStart != null)
                        list.Add(affix.OwnOnTurnStart);
                }

                return list;
            }
        }

        public List<ActionWithEffects> OnAttack
        {
            get
            {
                var list = new List<ActionWithEffects>();
                if (OwnOnAttack != null)
                    list.AddRange(OwnOnAttack);
                if (!IsIdentified) return list;

                foreach (var affix in Affixes)
                {
                    if(affix.OwnOnAttack != null)
                        list.Add(affix.OwnOnAttack);
                }

                return list;
            }
        }

        public List<ActionWithEffects> OnAttacked
        {
            get
            {
                var list = new List<ActionWithEffects>();
                if(OwnOnAttacked != null)
                    list.AddRange(OwnOnAttacked);
                if (!IsIdentified) return list;

                foreach (var affix in Affixes)
                {
                    if (affix.OwnOnAttacked != null)
                        list.Add(affix.OwnOnAttacked);
                }

                return list;
            }
        }

        public Item(EntityClass entityClass, int level, Map map) : base(entityClass, map)
        {
            BaseName = _trueName = entityClass.Name;
            _trueDescription = entityClass.Description;
            Power = entityClass.Power;
            OnUse = MapClassAction(entityClass.OnUse);
            OwnOnDeath = MapClassAction(entityClass.OnDeath);
            OwnOnTurnStart = MapClassAction(entityClass.OnTurnStart);
            MapClassActions(entityClass.OnAttack, OwnOnAttack);
            OwnOnAttacked = MapClassAction(entityClass.OnAttacked);
            OwnStatModifiers = new List<PassiveStatModifier>(entityClass.StatModifiers);
            BaseValue = entityClass.BaseValue;
            ItemLevel = level;
            MinimumQualityLevel = entityClass.MinimumQualityLevel;
            MaximumQualityLevel = entityClass.MaximumQualityLevel;
            QualityLevelOdds = entityClass.QualityLevelOdds;
            Affixes = [];
            ItemType = entityClass.ItemType;
            CanDrop = entityClass.CanDrop;
        }

        public void SetQualityLevel(QualityLevel qualityLevel = null)
        {
            if (MinimumQualityLevel != MaximumQualityLevel)
            {
                var qualityLevelToPick = qualityLevel ?? QualityLevelOdds.TakeRandomElementWithWeights(ql => ql.ChanceToPick, Rng).QualityLevel;
                if (qualityLevelToPick.MaximumAffixes > MaximumQualityLevel.MaximumAffixes)
                    QualityLevel = MaximumQualityLevel;
                else if (qualityLevelToPick.MaximumAffixes < MinimumQualityLevel.MaximumAffixes)
                    QualityLevel = MinimumQualityLevel;
                else
                    QualityLevel = qualityLevelToPick;
            }
            else
            {
                QualityLevel = MinimumQualityLevel;
            }
            SetAffixes();
            SetItemName();
        }

        private void SetAffixes()
        {
            Affixes = [];
            if (QualityLevel == null || QualityLevel.MaximumAffixes == 0) return;
            var affixesToApply = Rng.NextInclusive(QualityLevel.MinimumAffixes, QualityLevel.MaximumAffixes);
            if (affixesToApply == 0) return;
            var prefixesToApply = affixesToApply / 2;
            var suffixesToApply = affixesToApply / 2;
            if(prefixesToApply + suffixesToApply < affixesToApply)
            {
                if(Rng.RollProbability() <= 50)
                    prefixesToApply++;
                else
                    suffixesToApply++;
            }
            for (int i = 0; i < prefixesToApply; i++)
            {
                var validPrefixes = Map.Prefixes.Where(p => !Affixes.Any(a => a.Id.Equals(p.Id, StringComparison.InvariantCultureIgnoreCase)));
                if (!validPrefixes.Any()) break;
                var aPrefix = validPrefixes.Where(p => p.AffectedItemTypes.Contains(ItemType) && p.MinimumItemLevel <= ItemLevel).TakeRandomElement(Rng);
                if (aPrefix == null) continue;
                aPrefix.ApplyTo(this);
            }
            for (int i = 0; i < suffixesToApply; i++)
            {
                var validSuffixes = Map.Suffixes.Where(p => !Affixes.Any(a => a.Id.Equals(p.Id, StringComparison.InvariantCultureIgnoreCase)));
                if (!validSuffixes.Any()) break;
                var aSuffix = validSuffixes.Where(p => p.AffectedItemTypes.Contains(ItemType) && p.MinimumItemLevel <= ItemLevel).TakeRandomElement(Rng);
                if (aSuffix == null) continue;
                aSuffix.ApplyTo(this);
            }
        }

        private void SetItemName()
        {
            if (QualityLevel.AttachesWhatToItemName == QualityLevelNameAttachment.QualityLevel)
            {
                Name = QualityLevel.Name.Replace("{BaseName}", BaseName, StringComparison.InvariantCultureIgnoreCase);
            }
            else if (QualityLevel.AttachesWhatToItemName == QualityLevelNameAttachment.Affixes && Affixes.Count > 0)
            {
                var baseAffixedName = string.Empty;
                foreach (var affix in Affixes)
                {
                    if (baseAffixedName?.Length == 0)
                        baseAffixedName = affix.Name;
                    else
                        baseAffixedName = baseAffixedName.Replace("{BaseName}", affix.Name, StringComparison.InvariantCultureIgnoreCase);
                }
                Name = !string.IsNullOrWhiteSpace(baseAffixedName) ? baseAffixedName.Replace("{BaseName}", BaseName, StringComparison.InvariantCultureIgnoreCase) : BaseName;
            }
            else
            {
                Name = BaseName;
            }
            _trueName = Name;
        }

        public async Task Used(Entity user)
        {
            if (ItemType.Usability == ItemUsability.Use && (IsIdentified && OnUse == null) || (!IsIdentified && OnUse == null && OnAttack.Count == 0)) return;

            var isOnAttack = false;
            var actionToPerform = OnUse;

            if (!IsIdentified)
            {
                if (OnUse == null && OnAttack.Count(oa => oa != null && !oa.TargetTypes.Contains(TargetType.Tile)) > 0)
                {
                    isOnAttack = true;
                    actionToPerform = OnAttack.Where(oa => oa != null && !oa.TargetTypes.Contains(TargetType.Tile)).TakeRandomElement(Rng);
                }
                GotSpecificallyIdentified = true;
                UpdateNameIfNeeded();
                if (user == Map.Player)
                {
                    Map.DisplayEvents.Add(($"{_trueName} was identified", [new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.Identify }
                        }]));
                }
                Map.AppendMessage(Map.Locale["ItemWasIdentified"].Format(new { FakeName = _unidentifiedName, TrueName = _trueName }), Color.Yellow);
            }

            if (user == Map.Player)
            {
                if(!Map.Player.IdentifiedItemClasses.Contains(ClassId))
                    Map.Player.IdentifiedItemClasses.Add(ClassId);
                Map.DisplayEvents.Add(($"{user.Name} used item", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemUse }
                    }
                }
                ));
            }
            else if (user.EntityType == EntityType.NPC && Map.Player.CanSee(user))
            {
                Map.DisplayEvents.Add(($"{user.Name} used item", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.NPCItemUse }
                    }
                }
                ));
            }

            if(isOnAttack)
                await actionToPerform.Do(user, user, true);
            else
                await actionToPerform.Do(this, user, false);
        }

        public Task RefreshCooldownsAndUpdateTurnLength()
        {
            OwnOnAttack?.Where(a => a.CurrentCooldown > 0).ForEach(a => a.CurrentCooldown--);
            if (OwnOnAttacked?.CurrentCooldown > 0)
                OwnOnAttacked.CurrentCooldown--;
            if (OwnOnTurnStart?.CurrentCooldown > 0)
                OwnOnTurnStart.CurrentCooldown--;
            if (OnUse?.CurrentCooldown > 0)
                OnUse.CurrentCooldown--;
            return Task.CompletedTask;
        }

        public void UpdateNameIfNeeded()
        {
            if (!Map.Player.NeedsToIdentifyItems) return;
            if (!IsIdentified && !Name.Equals(_unidentifiedName))
            {
                Name = _unidentifiedName;
                Description = _unidentifiedDescription;
            }
            if (IsIdentified && Name.Equals(_unidentifiedName))
            {
                Name = _trueName;
                Description = _trueDescription;
            }
        }

        public async Task PerformOnTurnStart()
        {
            if(OwnOnTurnStart != null && Owner != null && OwnOnTurnStart.ChecksCondition(Owner, Owner))
                await OwnOnTurnStart.Do(this, Owner, true);
        }

        public override void SetActionIds()
        {
            for (int i = 0; i < OwnOnAttack.Count; i++)
            {
                OwnOnAttack[i].SelectionId = $"{Id}_{ClassId}_CA{i}_{OwnOnAttack[i].Id}";
                if (OwnOnAttack[i].IsScript)
                    OwnOnAttack[i].SelectionId += "_S";
            }
            if(OnUse != null)
                OnUse.SelectionId = $"{Id}_CA_UNIDENTIFIED";
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

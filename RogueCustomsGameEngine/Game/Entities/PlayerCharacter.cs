using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System;
using System.Collections.Generic;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class PlayerCharacter : Character
    {
        public readonly float SaleValuePercentage;

        public PlayerCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
            SaleValuePercentage = entityClass.SaleValuePercentage;
        }

        public int CalculateExperienceBarPercentage()
        {
            var experienceInCurrentLevel = Experience - LastLevelUpExperience;
            var experienceBetweenLevels = ExperienceToLevelUp - LastLevelUpExperience;
            return (int)((float)experienceInCurrentLevel / experienceBetweenLevels * 100);
        }

        public override async Task GainExperience(int GamePointsToAdd)
        {
            var events = new List<DisplayEventDto>();
            var oldLevel = Level;
            var statsPreExpGain = new List<(string Name, decimal Amount)>();
            var statsAfterExpGain = new List<(string Name, decimal Amount)>();
            foreach (var stat in UsedStats)
                statsPreExpGain.Add((stat.Name, stat.BaseAfterLevelUp));
            await base.GainExperience(GamePointsToAdd);
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.UpdateExperienceBar,
                Params = new() { Experience, ExperienceToLevelUp, CalculateExperienceBarPercentage() }
            });
            if (Level > oldLevel)
            {
                foreach (var stat in UsedStats)
                {
                    statsAfterExpGain.Add((stat.Name, stat.BaseAfterLevelUp));
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyStat, stat.Id, stat.Current }
                    });
                    if (stat.HasMax)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.ModifyMaxStat, stat.Id, stat.BaseAfterModifications }
                        });
                    }
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                    Params = new() { UpdatePlayerDataType.ModifyStat, "Level", Level }
                });
                events.Add(
                    new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.LevelUp }
                    }
                );
                var levelUpMessage = new StringBuilder(Map.Locale["CharacterLevelsUpMessage"].Format(new { CharacterName = Name, Level = Level }));
                levelUpMessage.AppendLine();
                levelUpMessage.AppendLine();

                foreach (var statAfterExpGain in statsAfterExpGain)
                {
                    var correspondingStatPreExpGain = statsPreExpGain.Find(s => s.Name.Equals(statAfterExpGain.Name));
                    if (correspondingStatPreExpGain == default) // This shouldn't happen, but just in case...
                        continue;
                    var correspondingStat = UsedStats.Find(s => s.Name.Equals(statAfterExpGain.Name));
                    if (correspondingStat == null) // This shouldn't happen, but just in case...
                        continue;
                    if (statAfterExpGain.Amount > correspondingStatPreExpGain.Amount)
                    {
                        if(correspondingStat.StatType == StatType.Decimal || correspondingStat.StatType == StatType.Regeneration)
                            levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = statAfterExpGain.Name, Amount = (statAfterExpGain.Amount - correspondingStatPreExpGain.Amount).ToString("0.#####") }));
                        else
                            levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = statAfterExpGain.Name, Amount = (statAfterExpGain.Amount - correspondingStatPreExpGain.Amount).ToString() }));
                    }
                }
                events.Add(
                    new()
                    {
                        DisplayEventType = DisplayEventType.AddMessageBox,
                        Params = new() {
                            new MessageBoxDto
                            {
                                Title = Map.Locale["CharacterLevelsUpHeader"],
                                Message = levelUpMessage.ToString(),
                                ButtonCaption = "OK",
                                WindowColor = new GameColor(Color.Lime)
                            }
                        }
                    }
                );
            }
            Map.DisplayEvents.Add(($"Player {Name} gained experience", events));
        }

        public void UpdateVisibility()
        {
            Map.Tiles.ForEach(t => t.Visible = false);
            var tiles = ComputeFOVTiles();
            foreach (var tile in tiles)
            {
                var mapTile = Map.GetTileFromCoordinates(tile.Position);
                mapTile.Discovered = mapTile.Visible = true;
            }
            FOVTiles = tiles;
        }

        public override async Task Die(Entity? attacker = null)
        {
            Map.DisplayEvents.Add(($"Player {Name} dies", new()
            {
                new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.GameOver }
                }
            }));
            await base.Die(attacker);
            if (ExistenceStatus == EntityExistenceStatus.Dead)
            {
                var events = new List<DisplayEventDto>();
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { Position, Map.GetConsoleRepresentationForCoordinates(Position.X, Position.Y) }
                    });
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.SetDungeonStatus,
                    Params = new() { DungeonStatus.GameOver }
                });
                Map.DisplayEvents.Add(($"Player {Name} is really dead", events));
            }
        }

        public override void EquipItem(Item itemToEquip)
        {
            if (!itemToEquip.IsEquippable)
                throw new InvalidOperationException("Attempted to equip an unequippable item!");

            if (!itemToEquip.ItemType.SlotsItOccupies.All(s => AvailableSlots.Contains(s)))
            {
                Map.AppendMessage(Map.Locale["CharacterCannotEquipItem"].Format(new { CharacterName = Name, ItemName = itemToEquip.Name }), Color.Yellow);
            }

            var events = new List<DisplayEventDto>();

            var itemsToUnequip = new List<Item>();

            foreach (var equippedItem in Equipment)
            {
                if (equippedItem.ItemType.SlotsItOccupies.Intersect(itemToEquip.ItemType.SlotsItOccupies).Any())
                    itemsToUnequip.Add(equippedItem);
            }
            var itemToEquipWasInTheBag = itemToEquip.Position == null;
            if (itemToEquipWasInTheBag)
            {
                Inventory.Remove(itemToEquip);
            }
            else
            {
                itemToEquip.Position = null;
                itemToEquip.ExistenceStatus = EntityExistenceStatus.Gone;
            }
            foreach (var equippedItem in itemsToUnequip)
            {
                Equipment.Remove(equippedItem);
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                    Params = new() { UpdatePlayerDataType.UpdateInventory, Inventory.Cast<Entity>().Union(KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                });
                if (Inventory.Count < InventorySize)
                {
                    Inventory.Add(equippedItem);
                    Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = equippedItem.Name }), events);
                }
                else
                {
                    Inventory.Remove(equippedItem);
                    DropItem(equippedItem);
                }
            }
            Equipment.Add(itemToEquip);
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                Params = new() { SpecialEffect.ItemEquip }
            });
            InformRefreshedPlayerData(events);
            
            Map.AppendMessage(Map.Locale["PlayerEquippedItem"].Format(new { CharacterName = Name, ItemName = itemToEquip.Name }), Color.Yellow, events);

            Map.DisplayEvents.Add(($"{Name} equips {itemToEquip.Name}", events));
        }

        public override void DropItem(IPickable pickable)
        {
            var pickableAsEntity = pickable as Entity;
            var events = new List<DisplayEventDto>();
            var centralTile = Map.GetTileFromCoordinates(Position);
            Tile pickedEmptyTile = null;
            if (!centralTile.GetPickableObjects().Exists(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (centralTile.Trap == null || centralTile.Trap.ExistenceStatus != EntityExistenceStatus.Alive))
                pickedEmptyTile = centralTile;
            if (pickedEmptyTile == null)
            {
                var closeEmptyTiles = Map.Tiles.GetElementsWithinDistanceWhere(Position.Y, Position.X, 5, true, t => t.AllowsDrops).ToList();
                if (centralTile?.AllowsDrops == true)
                    closeEmptyTiles.Add(centralTile);
                closeEmptyTiles = closeEmptyTiles.Where(t => t.LivingCharacter == null || t.LivingCharacter.ExistenceStatus != EntityExistenceStatus.Alive || t.LivingCharacter == this).ToList();
                var closestDistance = closeEmptyTiles.Any() ? closeEmptyTiles.Min(t => (int)GamePoint.Distance(t.Position, Position)) : -1;
                var closestEmptyTiles = closeEmptyTiles.Where(t => (int)GamePoint.Distance(t.Position, Position) <= closestDistance);
                if (closestEmptyTiles.Any())
                {
                    pickedEmptyTile = closestEmptyTiles.TakeRandomElement(Rng);
                }
                else
                {
                    pickableAsEntity.Position = null;
                    pickableAsEntity.ExistenceStatus = EntityExistenceStatus.Gone;
                    Map.AppendMessage(Map.Locale["NPCItemCannotBePutOnFloor"].Format(new { ItemName = pickableAsEntity.Name }), events);
                    if (pickableAsEntity is Item i)
                        Map.Items.Remove(i);
                }
            }
            if (pickedEmptyTile != null)
            {
                pickableAsEntity.Position = pickedEmptyTile.Position;
                pickableAsEntity.ExistenceStatus = EntityExistenceStatus.Alive;
                Map.AppendMessage(Map.Locale["PlayerPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = pickableAsEntity.Name }), events);
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.ItemDrop }
                });
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { pickableAsEntity.Position, Map.GetConsoleRepresentationForCoordinates(pickableAsEntity.Position.X, pickableAsEntity.Position.Y) }
                    }
                    );
                }
            }
            if (pickable is Item item)
            {
                if (Inventory.Contains(item))
                    Inventory.Remove(item);
                if (Equipment.Contains(item))
                    Equipment.Remove(item);
            }
            InformRefreshedPlayerData(events);
            Map.DisplayEvents.Add(($"Player {Name} put item on floor", events));
        }

        public override void PickItem(IPickable pickable, bool informToPlayer)
        {
            var events = new List<DisplayEventDto>();
            var pickableAsEntity = pickable as Entity;
            var isCurrency = false;
            if (pickable is Item i)
            {
                Inventory.Add(i);
            }
            else if (pickable is Currency c)
            {
                isCurrency = true;
                CurrencyCarried += c.Amount;
            }
            pickableAsEntity.Position = null;
            pickableAsEntity.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                if (isCurrency)
                {
                    Map.AppendMessage(Map.Locale["CharacterPicksCurrency"].Format(new { CharacterName = Name, CurrencyName = pickableAsEntity.Name }));
                    Map.DisplayEvents.Add(($"Player {Name} picked currency", new()
                    {
                        new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.UpdateCurrency, CurrencyCarried }
                        },
                        new() {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.Currency }
                        }
                    }
                    ));
                }
                else
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemGet }
                    });
                    InformRefreshedPlayerData(events);
                    Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = pickableAsEntity.Name }));
                    Map.DisplayEvents.Add(($"Player {Name} picked item on floor", events));
                }
            }
        }

        public override void PickKey(Key key, bool informToPlayer)
        {
            KeySet.Add(key);
            key.Owner = this;
            key.Position = null;
            key.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                Map.DisplayEvents.Add(($"Player {Name} picked key", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.KeyGet }
                    }
                }
                ));
                Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = key.Name }));
            }
        }

        public void InformRefreshedPlayerData(List<DisplayEventDto> events)
        {
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.UpdatePlayerData,
                Params = new() { UpdatePlayerDataType.UpdateCurrency, CurrencyCarried }
            });
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.UpdatePlayerData,
                Params = new() { UpdatePlayerDataType.UpdateInventory, Inventory.Cast<Entity>().Union(KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
            });
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.UpdatePlayerData,
                Params = new() { UpdatePlayerDataType.UpdateEquipment, Equipment.OrderBy(item => AvailableSlots.IndexOf(item.SlotsItOccupies[0])).ToList().ConvertAll(i => new SimpleEntityDto(i)) }
            });
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.UpdatePlayerData,
                Params = new() { UpdatePlayerDataType.ModifyDamageFromEquipment, DamageFromEquipment }
            });
            events.Add(new()
            {
                DisplayEventType = DisplayEventType.UpdatePlayerData,
                Params = new() { UpdatePlayerDataType.ModifyMitigationFromEquipment, MitigationFromEquipment }
            });
            foreach (var stat in UsedStats)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                    Params = new() { UpdatePlayerDataType.ModifyStat, stat.Id, stat.Current }
                });
                if (stat.HasMax)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyMaxStat, stat.Id, stat.BaseAfterModifications }
                    });
                }
            }
        }

        public override void SetActionIds()
        {
            base.SetActionIds();
        }
    }
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

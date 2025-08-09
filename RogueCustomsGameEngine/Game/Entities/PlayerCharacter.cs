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

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class PlayerCharacter : Character
    {
        public PlayerCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
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
                    statsAfterExpGain.Add((stat.Name, stat.BaseAfterLevelUp));
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

        public void DropItem(int slot)
        {
            var item = Inventory.ElementAtOrDefault(slot);
            if (item != null)
            {
                DropItem(item);
            }
        }

        public override void DropItem(Item item)
        {
            if (item == EquippedWeapon)
                EquippedWeapon = null;
            else if (item == EquippedArmor)
                EquippedArmor = null;
            else
                Inventory.Remove(item);
            item.Position = Position;
            item.Owner = null!;
            item.ExistenceStatus = EntityExistenceStatus.Alive;
            Map.DisplayEvents.Add(($"Player {Name} put item on floor", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemDrop }
                    }
                }
            ));
            Map.AppendMessage(Map.Locale["PlayerPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = item.Name }));
        }

        public override void PickItem(Item item, bool informToPlayer)
        {
            Inventory.Add(item);
            item.Owner = this;
            item.Position = null;
            item.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                Map.DisplayEvents.Add(($"Player {Name} picked item on floor", new()
                {
                    new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.UpdateInventory, Inventory.Cast<Entity>().Union(KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                    },
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemGet }
                    }
                }
                ));
                Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = item.Name }));
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

        public override void SetActionIds()
        {
            base.SetActionIds();
        }
    }
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

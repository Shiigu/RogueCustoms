using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Concurrent;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using System.Text.Json.Serialization;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class DungeonDto
    {
        public string DungeonName { get; private set; }
        public string FloorName { get; private set; }
        public DungeonStatus DungeonStatus { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int TurnCount { get; set; }
        private Map _map;

        public bool AwaitingPromptInput
        {
            set
            {
                _map.AwaitingPromptInput = value;
            }
        }

        [JsonIgnore]
        public bool Read { get; set; }

        [JsonIgnore]
        public bool JustLoaded { get; set; }

        public List<TileDto> Tiles { get; private set; }
        public ConsoleRepresentation EmptyTile { get; private set; }

        public EntityDto PlayerEntity { get; private set; }
        public Queue<(string Name, List<DisplayEventDto> Events)> DisplayEvents { get; set; }
        public List<GamePoint> PickableItemPositions { get; set; }

        public bool IsHardcoreMode { get; set; }

        public DungeonDto() { }

        public DungeonDto(Dungeon dungeon, Map map)
        {
            _map = map;
            DungeonName = dungeon.Name;
            IsHardcoreMode = dungeon.IsHardcoreMode;
            FloorName = map.FloorName;
            DungeonStatus = dungeon.DungeonStatus;
            Width = map.Width;
            Height = map.Height;
            TurnCount = map.TurnCount;
            EmptyTile = map.TileSet.Empty;
            Tiles = GetTiles();
            PlayerEntity = new EntityDto(map.Player, map);
            var playerEntity = map.Player;
            playerEntity.Equipment.OrderBy(item => map.Player.AvailableSlots.IndexOf(item.SlotsItOccupies[0])).ForEach(i => PlayerEntity.Equipment.Add(new SimpleEntityDto(i)));
            playerEntity.AlteredStatuses.ForEach(als => PlayerEntity.AlteredStatuses.Add(new SimpleEntityDto(als)));
            playerEntity.Inventory.ForEach(i => PlayerEntity.Inventory.Add(new SimpleEntityDto(i)));
            playerEntity.KeySet.ForEach(i => PlayerEntity.Inventory.Add(new SimpleEntityDto(i)));
        }

        public void GetInfoFromMap()
        {

        }

        public List<TileDto> GetTiles()
        {
            _map.Player.UpdateVisibility();
            var _tiles = new ConcurrentBag<TileDto>();
            if (DungeonStatus != DungeonStatus.Completed)
            {
                Parallel.For(0, _map.Tiles.GetLength(0), y =>
                {
                    Parallel.For(0, _map.Tiles.GetLength(1), x =>
                    {
                        var tile = _map.Tiles[y, x];
                        _tiles.Add(new TileDto(tile, _map));
                    });
                });
                return _tiles.ToList();
            }
            else
            {
                return new List<TileDto>();
            }
        }

        public void RefreshTiles()
        {
            Tiles = GetTiles();
        }
    }

    [Serializable]
    public class TileDto
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Discovered { get; private set; }
        public bool Visible { get; private set; }
        public bool Targetable { get; private set; }
        public bool IsStairs { get; private set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public TileDto() { }

        public TileDto(Tile tile, Map map)
        {
            X = tile.Position.X;
            Y = tile.Position.Y;
            IsStairs = tile.Type == TileType.Stairs;
            Discovered = tile.Discovered;
            Visible = tile.Visible;
            Targetable = tile.Targetable;
            ConsoleRepresentation = map.GetConsoleRepresentationForCoordinates(tile.Position.X, tile.Position.Y);
        }
    }

    [Serializable]
    public class EntityDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; private set; }
        public bool OnTop { get; private set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        #region Player-only fields
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExperienceToLevelUp { get; set; }
        public int CurrentExperiencePercentage { get; set; }
        public string HPStatName { get; private set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }

        public bool UsesMP { get; private set; }
        public string MPStatName { get; private set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public string DamageStatName { get; private set; }
        public string Damage { get; private set; }
        public string WeaponDamage { get; set; }
        public int Attack { get; set; }
        public string MitigationStatName { get; private set; }
        public string Mitigation { get; private set; }
        public string ArmorMitigation { get; set; }
        public int Defense { get; set; }
        public string MovementStatName { get; private set; }
        public int Movement { get; set; }
        public int BaseMovement { get; private set; }
        public string AccuracyStatName { get; private set; }
        public decimal BaseAccuracy { get; private set; }
        public decimal Accuracy { get; set; }
        public string EvasionStatName { get; private set; }
        public decimal BaseEvasion { get; private set; }
        public decimal Evasion { get; set; }
        public bool UsesHunger { get; private set; }
        public string HungerStatName { get; private set; }
        public int Hunger { get; set; }
        public int MaxHunger { get; set; }
        public List<SimpleEntityDto> Equipment { get; set; }
        public List<SimpleEntityDto> AlteredStatuses { get; set; }
        public List<SimpleEntityDto> Inventory { get; set; }
        public CurrencyDto Currency { get; private set; }
        #endregion

        public EntityDto() { }

        public EntityDto(Entity entity, Map map)
        {
            X = entity.Position.X;
            Y = entity.Position.Y;
            Name = entity.Name;
            OnTop = !entity.Passable;
            ConsoleRepresentation = entity.ConsoleRepresentation.Clone();
            if (entity is PlayerCharacter pc)
            {
                Level = pc.Level;
                Experience = pc.Experience;
                ExperienceToLevelUp = pc.ExperienceToLevelUp;
                CurrentExperiencePercentage = pc.CalculateExperienceBarPercentage();
                HPStatName = map.Locale["CharacterHPStat"];
                HP = (int) pc.HP.Current;
                MaxHP = (int)pc.MaxHP;
                UsesMP = pc.MP != null;
                if (UsesMP)
                {
                    MPStatName = map.Locale["CharacterMPStat"];
                    MP = (int)pc.MP.Current;
                    MaxMP = (int)pc.MaxMP;
                }
                DamageStatName = map.Locale["CharacterDamageStat"];
                Damage = pc.Damage;
                WeaponDamage = pc.DamageFromEquipment;
                Attack = (int)pc.Attack.BaseAfterModifications;
                MitigationStatName = map.Locale["CharacterMitigationStat"];
                Mitigation = pc.Mitigation;
                ArmorMitigation = pc.MitigationFromEquipment;
                Defense = (int)pc.Defense.BaseAfterModifications;
                MovementStatName = map.Locale["CharacterMovementStat"];
                BaseMovement = (int) pc.Movement.Base;
                Movement = (int) pc.Movement.Current;
                AccuracyStatName = map.Locale["CharacterAccuracyStat"];
                BaseAccuracy = (int)pc.Accuracy.Base;
                Accuracy = (int)pc.Accuracy.Current;
                EvasionStatName = map.Locale["CharacterEvasionStat"];
                BaseEvasion = (int)pc.Evasion.Base;
                Evasion = (int)pc.Evasion.Current;
                UsesHunger = pc.Hunger != null;
                if (UsesHunger)
                {
                    HungerStatName = map.Locale["CharacterHungerStat"];
                    Hunger = (int)pc.Hunger.Current;
                    MaxHunger = (int)pc.Hunger.Base;
                }
                Equipment = new List<SimpleEntityDto>();
                AlteredStatuses = new List<SimpleEntityDto>();
                Inventory = new List<SimpleEntityDto>();
                Currency = new(map.CurrencyClass, pc.CurrencyCarried);
            }
        }
    }

    [Serializable]
    public class SimpleEntityDto
    {
        public string Name { get; private set; }
        public ConsoleRepresentation ConsoleRepresentation { get; private set; }
        public GameColor NameColor { get; private set; }

        public SimpleEntityDto(Entity? e)
        {
            if (e == null) return;
            Name = e is Item i ? i.BaseName : e.Name;
            ConsoleRepresentation = e.ConsoleRepresentation;
            NameColor = e is Item it && it.QualityLevel != null ? it.QualityLevel.ItemNameColor : new GameColor(System.Drawing.Color.White);
        }
        public SimpleEntityDto(EntityClass? ec)
        {
            if (ec == null) return;
            Name = ec.Name;
            ConsoleRepresentation = ec.ConsoleRepresentation;
        }
    }

    [Serializable]
    public class CurrencyDto
    {
        public string Name { get; private set; }
        public ConsoleRepresentation ConsoleRepresentation { get; private set; }
        public int Amount { get; private set; }

        public CurrencyDto(EntityClass ec, int amount)
        {
            Name = ec.Name;
            ConsoleRepresentation = ec.ConsoleRepresentation;
            Amount = amount;
        }

    }
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

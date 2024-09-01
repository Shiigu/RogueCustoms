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

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class DungeonDto
    {
        public string DungeonName { get; set; }
        public string FloorName { get; set; }
        public int DungeonId { get; set; }
        public DungeonStatus DungeonStatus { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int TurnCount { get; set; }

        public List<TileDto> Tiles { get; set; }
        public List<EntityDto> Entities { get; set; }
        public List<MessageDto> LogMessages { get; set; }
        public List<MessageBoxDto> MessageBoxes { get; set; }
        public ConsoleRepresentation EmptyTile { get; set; }

        public EntityDto PlayerEntity => Entities.Find(e => e.Type == EntityDtoType.Player);
        public bool IsAlive => PlayerEntity.HP > 0;
        public bool PlayerTookDamage { get; set; }
        public bool PlayerGotHealed { get; set; }
        public bool PlayerGotMPBurned { get; set; }
        public bool PlayerGotMPReplenished { get; set; }
        public bool PlayerGotStatusChange { get; set; }

        public DungeonDto() { }

        public DungeonDto(Dungeon dungeon, Map map)
        {
            DungeonId = dungeon.Id;
            DungeonName = dungeon.Name;
            FloorName = map.FloorName;
            DungeonStatus = dungeon.DungeonStatus;
            Width = map.Width;
            Height = map.Height;
            TurnCount = map.TurnCount;
            EmptyTile = map.TileSet.Empty;
            Entities = new List<EntityDto>();
            var _tiles = new ConcurrentBag<TileDto>();
            PlayerTookDamage = map.PlayerTookDamage;
            PlayerGotHealed = map.PlayerGotHealed;
            PlayerGotMPBurned = map.PlayerGotMPBurned;
            PlayerGotMPReplenished = map.PlayerGotMPReplenished;
            PlayerGotStatusChange = map.PlayerGotStatusChange;
            if (DungeonStatus != DungeonStatus.Completed)
            {
                Parallel.For(0, map.Tiles.GetLength(0), y =>
                {
                    Parallel.For(0, map.Tiles.GetLength(1), x =>
                    {
                        var tile = map.Tiles[y, x];
                        _tiles.Add(new TileDto(tile, map));
                    });
                });
                Tiles = _tiles.ToList();
            }
            else
            {
                Tiles = new List<TileDto>();
            }
            var playerEntity = map.Player;
            map.GetEntities()
                .Where(e => e.ExistenceStatus != EntityExistenceStatus.Gone && playerEntity?.CanSee(e) == true)
                .ForEach(e => Entities.Add(new EntityDto(e, map)));
            playerEntity.AlteredStatuses.ForEach(als => PlayerEntity.AlteredStatuses.Add(new SimpleEntityDto(als)));
            playerEntity.Inventory.ForEach(i => PlayerEntity.Inventory.Add(new SimpleEntityDto(i)));
            LogMessages = dungeon.Messages.TakeLast(Constants.LogMessagesToSend).ToList();
            MessageBoxes = new List<MessageBoxDto>(dungeon.MessageBoxes);
        }
    }

    [Serializable]
    public class TileDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Discovered { get; set; }
        public bool Visible { get; set; }
        public bool Targetable { get; set; }
        public bool IsStairs { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public TileDto() { }

        public TileDto(Tile tile, Map map)
        {
            X = tile.Position.X;
            Y = tile.Position.Y;
            IsStairs = tile.Type == TileType.Stairs;
            Discovered = tile.Discovered;
            Visible = tile.Visible;
            Targetable = tile.IsWalkable && tile.Visible;
            ConsoleRepresentation = map.GetConsoleRepresentationForCoordinates(tile.Position.X, tile.Position.Y);
        }
    }

    [Serializable]
    public class EntityDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public bool OnTop { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public EntityDtoType Type { get; set; }

        public bool IsPlayer => Type == EntityDtoType.Player;

        #region Player-only fields
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExperienceToLevelUp { get; set; }
        public int CurrentExperiencePercentage { get; set; }

        private static int CalculateExperienceBarPercentage(Character entity)
        {
            var experienceInCurrentLevel = entity.Experience - entity.LastLevelUpExperience;
            var experienceBetweenLevels = entity.ExperienceToLevelUp - entity.LastLevelUpExperience;
            return (int)((float)experienceInCurrentLevel / experienceBetweenLevels * 100);
        }
        public string HPStatName { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }

        public bool UsesMP { get; set; }
        public string MPStatName { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public SimpleEntityDto Weapon { get; set; }
        public string DamageStatName { get; set; }
        public string Damage { get; set; }
        public string WeaponDamage { get; set; }
        public int StatDamage { get; set; }
        public SimpleEntityDto Armor { get; set; }
        public string MitigationStatName { get; set; }
        public string Mitigation { get; set; }
        public string ArmorMitigation { get; set; }
        public int StatMitigation { get; set; }
        public string MovementStatName { get; set; }
        public int Movement { get; set; }
        public int BaseMovement { get; set; }
        public string AccuracyStatName { get; set; }
        public decimal BaseAccuracy { get; set; }
        public decimal Accuracy { get; set; }
        public string EvasionStatName { get; set; }
        public decimal BaseEvasion { get; set; }
        public decimal Evasion { get; set; }
        public bool UsesHunger { get; set; }
        public string HungerStatName { get; set; }
        public int Hunger { get; set; }
        public int MaxHunger { get; set; }
        public bool CanMove { get; set; }
        public bool CanTakeAction { get; set; }
        public List<SimpleEntityDto> AlteredStatuses { get; set; }
        public List<SimpleEntityDto> Inventory { get; set; }
        #endregion

        public EntityDto() { }

        public EntityDto(Entity entity, Map map)
        {
            X = entity.Position.X;
            Y = entity.Position.Y;
            Name = entity.Name;
            OnTop = !entity.Passable;
            ConsoleRepresentation = entity.ConsoleRepresentation;
            switch (entity.EntityType)
            {
                case EntityType.Player:
                    Type = EntityDtoType.Player;
                    break;
                case EntityType.Weapon:
                case EntityType.Armor:
                case EntityType.Consumable:
                    Type = EntityDtoType.PickableObject;
                    break;
                case EntityType.NPC:
                    Type = EntityDtoType.Character;
                    break;
                case EntityType.Trap:
                    Type = EntityDtoType.Trap;
                    break;
            }
            if (IsPlayer && entity is PlayerCharacter pc)
            {
                Level = pc.Level;
                Experience = pc.Experience;
                ExperienceToLevelUp = pc.ExperienceToLevelUp;
                CurrentExperiencePercentage = CalculateExperienceBarPercentage(pc);
                HPStatName = map.Locale["CharacterHPStat"];
                HP = pc.HP;
                MaxHP = pc.MaxHP;
                UsesMP = pc.UsesMP;
                MPStatName = map.Locale["CharacterMPStat"];
                MP = pc.MP;
                MaxMP = pc.MaxMP;
                Weapon = new SimpleEntityDto(pc.Weapon);
                DamageStatName = map.Locale["CharacterDamageStat"];
                Damage = pc.Damage;
                WeaponDamage = pc.Weapon.Power;
                StatDamage = pc.Attack;
                Armor = new SimpleEntityDto(pc.Armor);
                MitigationStatName = map.Locale["CharacterMitigationStat"];
                Mitigation = pc.Mitigation;
                ArmorMitigation = pc.Armor.Power;
                StatMitigation = pc.Defense;
                MovementStatName = map.Locale["CharacterMovementStat"];
                BaseMovement = pc.BaseMovement;
                Movement = pc.Movement;
                AccuracyStatName = map.Locale["CharacterAccuracyStat"];
                BaseAccuracy = pc.BaseAccuracy;
                Accuracy = pc.Accuracy;
                EvasionStatName = map.Locale["CharacterEvasionStat"];
                BaseEvasion = pc.BaseEvasion;
                Evasion = pc.Evasion;
                CanMove = pc.Movement > 0;
                CanTakeAction = pc.CanTakeAction;
                UsesHunger = pc.UsesHunger;
                HungerStatName = map.Locale["CharacterHungerStat"];
                Hunger = pc.Hunger;
                MaxHunger = pc.MaxHunger;
                AlteredStatuses = new List<SimpleEntityDto>();
                Inventory = new List<SimpleEntityDto>();
            }
        }
    }

    [Serializable]
    public class SimpleEntityDto
    {
        public string Name { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public SimpleEntityDto(Entity e)
        {
            if (e == null) return;
            Name = e.Name;
            ConsoleRepresentation = e.ConsoleRepresentation;
        }
    }

    public enum EntityDtoType
    {
        Player = 0,
        Character = 1,
        PickableObject = 2,
        Trap = 3
    }
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

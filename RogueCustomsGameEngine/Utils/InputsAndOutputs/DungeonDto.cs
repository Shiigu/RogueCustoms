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

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class DungeonDto
    {
        public string DungeonName { get; private set; }
        public string FloorName { get; private set; }
        public int DungeonId { get; private set; }
        public DungeonStatus DungeonStatus { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int TurnCount { get; private set; }

        public List<TileDto> Tiles { get; private set; }
        public List<EntityDto> Entities { get; private set; }
        public List<MessageDto> LogMessages { get; private set; }
        public List<MessageBoxDto> MessageBoxes { get; private set; }
        public ConsoleRepresentation EmptyTile { get; private set; }

        public EntityDto PlayerEntity => Entities.Find(e => e.Type == EntityDtoType.Player);
        public bool IsAlive => PlayerEntity.HP > 0;
        public List<SpecialEffect> SpecialEffectsThatHappened { get; private set; }

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
            map.MoveSpecialEffectToTheEndIfPossible(SpecialEffect.LevelUp);
            map.MoveSpecialEffectToTheEndIfPossible(SpecialEffect.StairsReveal);
            map.MoveSpecialEffectToTheEndIfPossible(SpecialEffect.GameOver);
            SpecialEffectsThatHappened = new(map.SpecialEffectsThatHappened);
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
            playerEntity.KeySet.ForEach(i => PlayerEntity.Inventory.Add(new SimpleEntityDto(i)));
            LogMessages = dungeon.Messages.TakeLast(EngineConstants.LogMessagesToSend).ToList();
            MessageBoxes = new List<MessageBoxDto>(dungeon.MessageBoxes);
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
        public ConsoleRepresentation ConsoleRepresentation { get; private set; }

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
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Name { get; private set; }
        public bool OnTop { get; private set; }
        public ConsoleRepresentation ConsoleRepresentation { get; private set; }

        public EntityDtoType Type { get; private set; }

        public bool IsPlayer => Type == EntityDtoType.Player;

        #region Player-only fields
        public int Level { get; private set; }
        public int Experience { get; private set; }
        public int ExperienceToLevelUp { get; private set; }
        public int CurrentExperiencePercentage { get; private set; }

        private static int CalculateExperienceBarPercentage(Character entity)
        {
            var experienceInCurrentLevel = entity.Experience - entity.LastLevelUpExperience;
            var experienceBetweenLevels = entity.ExperienceToLevelUp - entity.LastLevelUpExperience;
            return (int)((float)experienceInCurrentLevel / experienceBetweenLevels * 100);
        }
        public string HPStatName { get; private set; }
        public int HP { get; private set; }
        public int MaxHP { get; private set; }

        public bool UsesMP { get; private set; }
        public string MPStatName { get; private set; }
        public int MP { get; private set; }
        public int MaxMP { get; private set; }
        public SimpleEntityDto Weapon { get; private set; }
        public string DamageStatName { get; private set; }
        public string Damage { get; private set; }
        public string WeaponDamage { get; private set; }
        public int Attack { get; private set; }
        public SimpleEntityDto Armor { get; private set; }
        public string MitigationStatName { get; private set; }
        public string Mitigation { get; private set; }
        public string ArmorMitigation { get; private set; }
        public int Defense { get; private set; }
        public string MovementStatName { get; private set; }
        public int Movement { get; private set; }
        public int BaseMovement { get; private set; }
        public string AccuracyStatName { get; private set; }
        public decimal BaseAccuracy { get; private set; }
        public decimal Accuracy { get; private set; }
        public string EvasionStatName { get; private set; }
        public decimal BaseEvasion { get; private set; }
        public decimal Evasion { get; private set; }
        public bool UsesHunger { get; private set; }
        public string HungerStatName { get; private set; }
        public int Hunger { get; private set; }
        public int MaxHunger { get; private set; }
        public bool CanMove { get; private set; }
        public bool CanTakeAction { get; private set; }
        public List<SimpleEntityDto> AlteredStatuses { get; private set; }
        public List<SimpleEntityDto> Inventory { get; private set; }
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
                HP = (int) pc.HP.Current;
                MaxHP = (int)pc.MaxHP;
                UsesMP = pc.MP != null;
                if (UsesMP)
                {
                    MPStatName = map.Locale["CharacterMPStat"];
                    MP = (int)pc.MP.Current;
                    MaxMP = (int)pc.MaxMP;
                }
                Weapon = new SimpleEntityDto(pc.Weapon);
                DamageStatName = map.Locale["CharacterDamageStat"];
                Damage = pc.Damage;
                WeaponDamage = pc.Weapon.Power;
                Attack = (int)pc.Attack.BaseAfterModifications;
                Armor = new SimpleEntityDto(pc.Armor);
                MitigationStatName = map.Locale["CharacterMitigationStat"];
                Mitigation = pc.Mitigation;
                ArmorMitigation = pc.Armor.Power;
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
                CanMove = pc.Movement.Current > 0;
                CanTakeAction = pc.CanTakeAction;
                UsesHunger = pc.Hunger != null;
                if (UsesHunger)
                {
                    HungerStatName = map.Locale["CharacterHungerStat"];
                    Hunger = (int)pc.Hunger.Current;
                    MaxHunger = (int)pc.Hunger.Base;
                }
                AlteredStatuses = new List<SimpleEntityDto>();
                Inventory = new List<SimpleEntityDto>();
            }
        }
    }

    [Serializable]
    public class SimpleEntityDto
    {
        public string Name { get; private set; }
        public ConsoleRepresentation ConsoleRepresentation { get; private set; }

        public SimpleEntityDto(Entity? e)
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

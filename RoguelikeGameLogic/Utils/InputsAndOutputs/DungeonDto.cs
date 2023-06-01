using RoguelikeGameEngine.Game.Entities;
using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.Helpers;
using RoguelikeGameEngine.Utils.Representation;
using System.Collections.Concurrent;
using RoguelikeGameEngine.Utils.Enums;

namespace RoguelikeGameEngine.Utils.InputsAndOutputs
{
    public class DungeonDto
    {
        public string Name { get; set; }
        public int DungeonId { get; set; }
        public DungeonStatus DungeonStatus { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int TurnCount { get; set; }

        public List<TileDto> Tiles { get; set; }
        public List<AlteredStatusDto> AlteredStatuses { get; set; }
        public List<EntityDto> Entities { get; set; }
        public List<string> LogMessages { get; set; }
        public List<MessageBoxDto> MessageBoxes { get; set; }

        public EntityDto PlayerEntity => Entities.Find(e => e.Type == EntityDtoType.Player);
        public bool IsAlive => PlayerEntity.HP > 0;

        public DungeonDto() { }

        public DungeonDto(Dungeon dungeon)
        {
            DungeonId = dungeon.Id;
            Name = dungeon.CurrentFloor.FloorName;
            DungeonStatus = dungeon.DungeonStatus;
            Width = dungeon.CurrentFloor.Width;
            Height = dungeon.CurrentFloor.Height;
            TurnCount = dungeon.CurrentFloor.TurnCount;
            Entities = new List<EntityDto>();
            var _tiles = new ConcurrentBag<TileDto>();
            AlteredStatuses = new List<AlteredStatusDto>();
            if(DungeonStatus != DungeonStatus.Completed)
            {
                Parallel.For(0, dungeon.CurrentFloor.Tiles.GetLength(0), y =>
                {
                    Parallel.For(0, dungeon.CurrentFloor.Tiles.GetLength(1), x =>
                    {
                        var tile = dungeon.CurrentFloor.Tiles[y, x];
                        _tiles.Add(new TileDto(tile, dungeon.CurrentFloor));
                    });
                });
                Tiles = _tiles.ToList();
            }
            else
            {
                Tiles = new List<TileDto>();
            }
            var playerEntity = dungeon.CurrentFloor.Player;
            var tileIsOccupied = dungeon.CurrentFloor.GetEntitiesFromCoordinates(playerEntity.Position).Any(e => e.Passable
                    && (e.EntityType == EntityType.Weapon || e.EntityType == EntityType.Armor || e.EntityType == EntityType.Consumable)
                    && e.ExistenceStatus != EntityExistenceStatus.Gone);
            dungeon.CurrentFloor.Entities
                .Where(e => e.ExistenceStatus != EntityExistenceStatus.Gone && playerEntity?.CanSee(e) == true)
                .ForEach(e => Entities.Add(new EntityDto(e)));
            playerEntity.AlteredStatuses.ForEach(als => AlteredStatuses.Add(new AlteredStatusDto(als)));
            LogMessages = dungeon.Messages.TakeLast(Constants.LogMessagesToSend).ToList();
            MessageBoxes = new List<MessageBoxDto>(dungeon.MessageBoxes);
        }
    }

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
            Targetable = tile.IsWalkable;
            ConsoleRepresentation = map.GetConsoleRepresentationForCoordinates(tile.Position.X, tile.Position.Y);
        }
    }

    public class AlteredStatusDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }

        public AlteredStatusDto() { }

        public AlteredStatusDto(AlteredStatus status)
        {
            Name = status.Name;
            Description = status.ToString();
            ConsoleRepresentation = status.ConsoleRepresentation;
        }
    }

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

        private int CalculateExperienceBarPercentage(Character entity)
        {
            var experienceInCurrentLevel = entity.Experience - entity.LastLevelUpExperience;
            var experienceBetweenLevels = entity.ExperienceToLevelUp - entity.LastLevelUpExperience;
            return (int)((float)experienceInCurrentLevel / experienceBetweenLevels * 100);
        }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public string Weapon { get; set; }
        public string Damage { get; set; }
        public string Armor { get; set; }
        public string Mitigation { get; set; }
        public int Movement { get; set; }
        #endregion

        public EntityDto() { }

        public EntityDto(Entity entity)
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
                HP = pc.HP;
                MaxHP = pc.MaxHP;
                Weapon = pc.Weapon.Name;
                Damage = pc.Damage;
                Armor = pc.Armor.Name;
                Mitigation = pc.Mitigation;
                Movement = pc.Movement;
            }
        }
    }

    public enum EntityDtoType
    {
        Player = 0,
        Character = 1,
        PickableObject = 2,
        Trap = 3
    }
}

using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RoguelikeGameEngine.Game.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassId { get; set; }
        public string Description { get; set; }
        public Point? Position { get; set; }
        public Tile ContainingTile => Map.GetTileFromCoordinates(Position.X, Position.Y);
        public Room ContainingRoom => Map.GetRoomInCoordinates(Position.X, Position.Y);

        public readonly ConsoleRepresentation ConsoleRepresentation;
        public bool Visible { get; set; }
        public EntityExistenceStatus ExistenceStatus { get; set; }
        public bool Passable { get; set; }
        public EntityType EntityType { get; set; }
        public Map Map { get; set; }
        public Random Rng => Map.Rng;

        public List<ActionWithEffects> OwnOnTurnStartActions { get; set; }
        public List<ActionWithEffects> OwnOnAttackActions { get; set; }
        public List<ActionWithEffects> OwnOnAttackedActions { get; set; }
        public List<ActionWithEffects> OwnOnDeathActions { get; set; }

        public Entity(EntityClass entityClass, Map map)
        {
            Map = map;
            Name = entityClass.Name;
            ClassId = entityClass.Id;
            Description = entityClass.Description;
            ConsoleRepresentation = new ConsoleRepresentation
            {
                Character = entityClass.ConsoleRepresentation.Character,
                ForegroundColor = entityClass.ConsoleRepresentation.ForegroundColor,
                BackgroundColor = entityClass.ConsoleRepresentation.BackgroundColor,
            };
            EntityType = entityClass.EntityType;
            Passable = entityClass.Passable;
            Visible = entityClass.StartsVisible;
            ExistenceStatus = EntityExistenceStatus.Alive;
            OwnOnTurnStartActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnTurnStartActions, OwnOnTurnStartActions);
            OwnOnAttackActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnAttackActions, OwnOnAttackActions);
            OwnOnAttackedActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnAttackedActions, OwnOnAttackedActions);
            OwnOnDeathActions = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnDeathActions, OwnOnDeathActions);
        }

        protected void MapClassActions(List<ActionWithEffects> classActions, List<ActionWithEffects> entityActions)
        {
            classActions.ForEach(a =>
            {
                var actionInstance = a.Clone();
                actionInstance.Map = Map;
                actionInstance.Name = Map.Locale[actionInstance.Name];
                actionInstance.User = this;
                entityActions.Add(actionInstance);
            });
        }

        public override string ToString() => $"Position: {Position}; Name: {Name}; Char: {ConsoleRepresentation.Character}";
    }

    public enum EntityType
    {
        Player,
        NPC,
        Consumable,
        Weapon,
        Armor,
        Trap,
        AlteredStatus
    }
    public enum EntityExistenceStatus
    {
        Alive,
        Dead,
        Gone
    }
}
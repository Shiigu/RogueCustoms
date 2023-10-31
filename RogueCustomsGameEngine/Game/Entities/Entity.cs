using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Game.Entities
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
        public RngHandler Rng => Map.Rng;

        public ActionWithEffects OwnOnTurnStart { get; set; }
        public List<ActionWithEffects> OwnOnAttack { get; set; }
        public ActionWithEffects OwnOnAttacked { get; set; }
        public ActionWithEffects OwnOnDeath { get; set; }

        protected Entity(EntityClass entityClass, Map map)
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
            OwnOnAttack = new List<ActionWithEffects>();
        }

        protected void MapClassActions(List<ActionWithEffects> classActions, List<ActionWithEffects> entityActions)
        {
            classActions.ForEach(a =>
            {
                var actionInstance = a.Clone();
                actionInstance.Map = Map;
                actionInstance.Name = Map.Locale[actionInstance.Name];
                actionInstance.Description = Map.Locale[actionInstance.Description];
                actionInstance.User = this;
                entityActions.Add(actionInstance);
            });
        }
        protected ActionWithEffects MapClassAction(ActionWithEffects classAction)
        {
            if (classAction == null) return null;
            var actionInstance = classAction.Clone();
            actionInstance.Map = Map;
            actionInstance.Name = Map.Locale[actionInstance.Name];
            actionInstance.Description = Map.Locale[actionInstance.Description];
            actionInstance.User = this;
            return actionInstance;
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
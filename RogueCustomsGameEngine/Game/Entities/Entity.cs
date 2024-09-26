using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public abstract class Entity : ITargetable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassId { get; set; }
        public string Description { get; set; }
        public GamePoint? Position { get; set; }
        public Tile ContainingTile => Map.GetTileFromCoordinates(Position.X, Position.Y);
        public Room ContainingRoom => Map.GetRoomInCoordinates(Position.X, Position.Y);

        public readonly ConsoleRepresentation BaseConsoleRepresentation;
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
            if (entityClass == null) throw new ArgumentException("Cannot create an Entity from a null EntityClass");
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
            BaseConsoleRepresentation = ConsoleRepresentation.Clone();
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

        public Entity Clone()
        {
            return JsonSerializer.Deserialize<Entity>(JsonSerializer.Serialize(this));
        }
    }

    public enum EntityType
    {
        Player,
        NPC,
        Consumable,
        Weapon,
        Armor,
        Trap,
        Key,
        AlteredStatus
    }
    public enum EntityExistenceStatus
    {
        Alive,
        Dead,
        Gone
    }
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
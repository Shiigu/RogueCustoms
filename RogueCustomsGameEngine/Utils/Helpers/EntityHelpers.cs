using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class EntityHelpers
    {
        public static KeyType Parse(this KeyTypeInfo keyType, Dungeon dungeon)
        {
            var keyClassTemplate = new ItemInfo()
            {
                Id = $"KeyType{keyType.KeyTypeName}",
                Name = $"KeyType{keyType.KeyTypeName}",
                ConsoleRepresentation = keyType.KeyConsoleRepresentation,
                Description = "KeyDescription",
                StartsVisible = true,
                Power = "0",
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                OnAttack = [Key.GetOpenDoorActionForKey(keyType.KeyTypeName)],
            };
            return new()
            {
                KeyTypeName = keyType.KeyTypeName,
                CanLockItems = keyType.CanLockItems,
                CanLockStairs = keyType.CanLockStairs,
                DoorConsoleRepresentation = keyType.DoorConsoleRepresentation,
                KeyClass = new EntityClass(keyClassTemplate, dungeon, [])
                {
                    EntityType = EntityType.Key
                }
            };
        }

        public static EntityClass Parse(this CurrencyInfo currency, Dungeon dungeon)
        {
            var currencyClassTemplate = new ItemInfo()
            {
                Id = $"Currency",
                Name = dungeon.LocaleToUse[currency.Name],
                Description = dungeon.LocaleToUse[currency.Description],
                ConsoleRepresentation = currency.ConsoleRepresentation,
                StartsVisible = true,
                Power = "0",
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                OnAttack = [],
            };
            return new EntityClass(currencyClassTemplate, dungeon, [])
            {
                EntityType = EntityType.Currency
            };
        }
    }
}
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

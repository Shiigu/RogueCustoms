using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using org.matheval;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Utils.Expressions
{
    public static class TileExpressionFunctions
    {
        public static string TILEISOCCUPIED(Entity This, Entity Source, Tile Target, string[] parameters)
        {
            return (Target.Type == TileType.Door || Target.IsOccupied).ToString();
        }
        public static string TILEHASDEADALLIES(Entity This, Entity Source, Tile Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for TileHasDeadAllies.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = entityName.ToLowerInvariant() switch
            {
                "this" => This,
                "source" => Source,
                "player" => This.Map.Player,
                _ => throw new ArgumentException("Invalid entity name in TileHasDeadAllies.")
            };

            if (entityToCheck is not Character c)
                return false.ToString();

            var entityFaction = c.Faction;

            return (Target.GetDeadCharacters().Any(c => c.Faction.IsAlliedWith(entityFaction))).ToString();
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

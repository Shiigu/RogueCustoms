using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Utils.ExpressionFunctions
{
    public static class DummyExpressionFunctions
    {
        public static string RNG(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "1";
        }

        public static string FLAGEXISTS(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "true";
        }

        public static string HASSTATUS(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "true";
        }

        public static string DOESNOTHAVESTATUS(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "false";
        }

        public static string TILEISOCCUPIED(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "true";
        }
        public static string TILEHASDEADALLIES(Entity This, Entity Source, Tile Target, string[] parameters)
        {
            return "true";
        }

        public static string CONCAT(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string REPLACE(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string REVERSE(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string LOWER(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string UPPER(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string TRIM(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string FLOOR(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "1";
        }

        public static string CEILING(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "1";
        }

        public static string USESSTAT(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "true";
        }

        public static string CURRENTWEAPON(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string CURRENTARMOR(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "\"\"";
        }

        public static string DISTANCEBETWEEN(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "1";
        }

        public static string AREINTHESAMEROOM(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            return "true";
        }
    }
}

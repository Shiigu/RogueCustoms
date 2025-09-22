using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Helpers;

namespace RogueCustomsDungeonEditor.Utils.ExpressionFunctions
{
    public static class DummyExpressionFunctions
    {
        public static string RNG(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string FLAGEXISTS(EffectCallerParams args, string[] parameters)
        {
            return "true";
        }

        public static string HASSTATUS(EffectCallerParams args, string[] parameters)
        {
            return "true";
        }

        public static string DOESNOTHAVESTATUS(EffectCallerParams args, string[] parameters)
        {
            return "false";
        }

        public static string TILEISOCCUPIED(EffectCallerParams args, string[] parameters)
        {
            return "true";
        }
        public static string TILEHASDEADALLIES(EffectCallerParams args, string[] parameters)
        {
            return "true";
        }

        public static string CONCAT(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string REPLACE(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string REVERSE(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string LOWER(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string UPPER(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string TRIM(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string FLOOR(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string CEILING(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string USESSTAT(EffectCallerParams args, string[] parameters)
        {
            return "true";
        }

        public static string CURRENTWEAPON(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string CURRENTARMOR(EffectCallerParams args, string[] parameters)
        {
            return "\"\"";
        }

        public static string DISTANCEBETWEEN(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string AREINTHESAMEROOM(EffectCallerParams args, string[] parameters)
        {
            return "true";
        }

        public static string ROLLACLASS(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string ROLLANITEM(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string CURRENTFLOORLEVEL(EffectCallerParams args, string[] parameters)
        {
            return "1";
        }

        public static string ISINVENTORYFULL(EffectCallerParams args, string[] parameters)
        {
            return "false";
        }

        public static string ROLLANACTION(EffectCallerParams args, string[] parameters)
        {
            return "false";
        }

        public static string HASAWAYPOINT(EffectCallerParams args, string[] parameters)
        {
            return "false";
        }
        public static string ISONWAYPOINT(EffectCallerParams args, string[] parameters)
        {
            return "false";
        }

        public static string HASACTIVEAFFIX(EffectCallerParams args, string[] parameters)
        {
            return "false";
        }
    }
}

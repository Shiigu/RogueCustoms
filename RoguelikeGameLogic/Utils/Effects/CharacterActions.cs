using RoguelikeGameEngine.Game.Entities;
using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.Helpers;
using System;

namespace RoguelikeGameEngine.Utils.Effects
{
    // Represents Actions that are only expected to be used by Characters.
    public static class CharacterActions
    {
        public static Random Rng;
        public static Map Map;


        public static bool ReplaceConsoleRepresentation(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (ExpandoObjectHelper.HasProperty(paramsObject, "Character"))
                Source.ConsoleRepresentation.Character = paramsObject.Character;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "Color"))
                Source.ConsoleRepresentation.ForegroundColor = paramsObject.Color;
            _ = 0;
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Expressions;
using org.matheval;
using System.Collections;

#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
namespace RogueCustomsGameEngine.Utils.Effects
{
    public static class ControlBlockActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool While(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            var ctx = ExecutionContext.Current
                ?? throw new InvalidOperationException("No execution context.");
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            var conditionResult = new Expression(paramsObject.Condition).Eval<bool>();

            if (conditionResult)
                ctx.LoopStack.Push(new WhileFrame(ctx.CurrentEffect));

            return conditionResult;
        }

        public static bool For(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            var ctx = ExecutionContext.Current
                ?? throw new InvalidOperationException("No execution context.");
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);

            var counterFlagKey = paramsObject.CounterFlagKey;
            var start = int.Parse(paramsObject.InitialValue);
            var end = int.Parse(paramsObject.EndValue);
            var increment = int.Parse(paramsObject.Increment);

            if (!Map.HasFlag(counterFlagKey))
                Map.CreateFlag(counterFlagKey, start, true);
            else
                Map.SetFlagValue(counterFlagKey, start);

            ctx.LoopStack.Push(new ForFrame(ctx.CurrentEffect, end, increment, counterFlagKey, Map));

            return start <= end;
        }
    }
}
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

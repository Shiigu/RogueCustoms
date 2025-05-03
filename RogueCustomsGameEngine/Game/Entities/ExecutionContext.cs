using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CA2211 // Los campos no constantes no deben ser visibles

    public class ExecutionContext
    {
        public static ExecutionContext? Current;

        public Stack<LoopFrame> LoopStack { get; } = new Stack<LoopFrame>();

        public Effect? CurrentEffect { get; set; }
    }

    public abstract class LoopFrame(Effect start)
    {
        public Effect StartEffect { get; } = start;
    }

    public class WhileFrame(Effect start) : LoopFrame(start)
    {
    }

    public class ForFrame(Effect start, int end, int increment, string counterName, Map map) : LoopFrame(start)
    {
        private readonly int End = end;
        private readonly int Increment = increment;
        private readonly string CounterFlagKey = counterName;
        private readonly Map map = map;

        public bool ShouldContinue()
        {
            int current = (int)map.GetFlagValue(CounterFlagKey);
            return current <= End;
        }

        public void Advance()
        {
            int current = (int)map.GetFlagValue(CounterFlagKey);
            map.SetFlagValue(CounterFlagKey, current + Increment);
        }
    }
    
    #pragma warning restore CA2211 // Los campos no constantes no deben ser visibles
}

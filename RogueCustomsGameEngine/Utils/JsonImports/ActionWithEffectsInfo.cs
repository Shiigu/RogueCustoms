using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ActionWithEffectsInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsScript { get; set; }

        #region Exclusive to Attacks or Item Uses
        public int MinimumRange { get; set; }
        public int MaximumRange { get; set; }
        public int CooldownBetweenUses { get; set; }
        public int MaximumUses { get; set; }
        public List<string> TargetTypes { get; set; }
        public string UseCondition { get; set; }
        public string AIUseCondition { get; set; }
        public int MPCost { get; set; }
        public bool FinishesTurnWhenUsed { get; set; }
        #endregion

        public int StartingCooldown { get; set; }

        public EffectInfo Effect { get; set; }
    }

    [Serializable]
    public class EffectInfo
    {
        public string EffectName { get; set; }
        public Parameter[] Params { get; set; }
        public EffectInfo Then { get; set; }
        public EffectInfo OnSuccess { get; set; }
        public EffectInfo OnFailure { get; set; }
    }

    [Serializable]
    public class Parameter
    {
        public string ParamName { get; set; }
        public string Value { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

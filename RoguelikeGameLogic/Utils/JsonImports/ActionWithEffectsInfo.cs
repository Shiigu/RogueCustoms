using System;
using System.Collections.Generic;

namespace RoguelikeGameEngine.Utils.JsonImports
{
    [Serializable]
    public class ActionWithEffectsInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }

        #region Exclusive to Attacks or Item Uses
        public int MinimumRange { get; set; }
        public int MaximumRange { get; set; }
        public int CooldownBetweenUses { get; set; }
        public int MaximumUses { get; set; }
        public List<string> TargetTypes { get; set; }
        #endregion

        #region Exclusive to Altered Statuses
        public int StartingCooldown { get; set; }
        #endregion

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
}

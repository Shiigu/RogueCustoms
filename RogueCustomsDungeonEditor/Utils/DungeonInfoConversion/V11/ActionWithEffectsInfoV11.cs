using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ActionWithEffectsInfoV11
    {
        public string Name { get; set; }
        public string Description { get; set; }

        #region Exclusive to Attacks or Item Uses
        public int MinimumRange { get; set; }
        public int MaximumRange { get; set; }
        public int CooldownBetweenUses { get; set; }
        public int MaximumUses { get; set; }
        public List<string> TargetTypes { get; set; }
        public string UseCondition { get; set; }
        public int MPCost { get; set; }
        #endregion

        public int StartingCooldown { get; set; }

        public EffectInfoV11 Effect { get; set; }
    }

    [Serializable]
    public class EffectInfoV11
    {
        public string EffectName { get; set; }
        public ParameterV11[] Params { get; set; }
        public EffectInfoV11 Then { get; set; }
        public EffectInfoV11 OnSuccess { get; set; }
        public EffectInfoV11 OnFailure { get; set; }
    }

    [Serializable]
    public class ParameterV11
    {
        public string ParamName { get; set; }
        public string Value { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

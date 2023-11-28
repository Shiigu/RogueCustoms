using RogueCustomsGameEngine.Game.DungeonStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.EffectInfos
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class EffectTypeData
    {
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool CanBeUsedOnEntity { get; set; }
        public bool CanBeUsedOnTile { get; set; }
        public bool CanHaveThenChild { get; set; }
        public bool CanHaveOnSuccessOnFailureChild { get; set; }
        public List<EffectParameter> Parameters { get; set; }
    }

    public class EffectParameter
    {
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ParameterType Type => (ParameterType) Enum.Parse(typeof(ParameterType), ParameterType);
        public string ParameterType { get; set; }
        public string Default { get; set; }
        public List<EffectParameterValidValue> ValidValues { get; set; }
        public bool Required { get; set; }
        public List<string> OptionalIfFieldsHaveValue { get; set; }
    }

    public class EffectParameterValidValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public enum ParameterType
    {
        ComboBox,
        Formula,
        Odds,
        Boolean,
        Character,
        Color,
        Text,
        AlteredStatus,
        Trap,
        Number,
        BooleanExpression,
        Key
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

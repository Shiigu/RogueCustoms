using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.EffectInfos
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class EffectTypeData
    {
        public string InternalName { get; set; }
        public string ComboBoxDisplayName { get; set; }
        public string TreeViewDisplayText { get; set; }
        public string Description { get; set; }
        public bool CanBeUsedOnEntity { get; set; }
        public bool CanBeUsedOnTile { get; set; }
        public bool CanHaveThenChild { get; set; }
        public bool CanHaveOnSuccessOnFailureChild { get; set; }
        public bool IsControlBlock { get; set; }
        public List<EffectParameter> Parameters { get; set; }

        public string GetParsedTreeViewDisplayName(Parameter[] effectParameters, List<string> statIds)
        {
            string pattern = @"\{([^{}]+)\}";
            Regex regex = new Regex(pattern);

            string parsedText = regex.Replace(TreeViewDisplayText, match =>
            {
                string placeholder = match.Groups[1].Value;

                // Find the parameter with matching key
                var matchingParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals(placeholder, StringComparison.InvariantCultureIgnoreCase));

                // If parameter not found, handle it differently (e.g., using a switch statement)
                if (matchingParam != null)
                {
                    return matchingParam.Value;
                }
                else
                {
                    return HandleSpecialCase(placeholder, effectParameters, statIds);
                }
            });

            return parsedText;
        }

        private string HandleSpecialCase(string placeholder, Parameter[] effectParameters, List<string> statIds)
        {
            // Add logic to handle special cases (e.g., using a switch statement)
            switch (placeholder.ToLowerInvariant())
            {
                case "itemtype":
                    var output = "[UNDEFINED]";
                    var canStealEquippablesParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals("CanStealEquippables", StringComparison.InvariantCultureIgnoreCase));
                    var canStealConsumablesParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals("CanStealConsumables", StringComparison.InvariantCultureIgnoreCase));

                    var canStealEquippables = canStealEquippablesParam != null && bool.Parse(canStealEquippablesParam.Value);
                    var canStealConsumables = canStealConsumablesParam != null && bool.Parse(canStealConsumablesParam.Value);

                    if (canStealEquippables && canStealConsumables)
                    {
                        output = "Equippables and Consumables";
                    }
                    if (canStealEquippables && !canStealConsumables)
                    {
                        output = "Equippables";
                    }
                    if (!canStealEquippables && canStealConsumables)
                    {
                        output = "Equippables";
                    }

                    return output;
                case "stat":
                    var statParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals("Stat", StringComparison.InvariantCultureIgnoreCase));
                    if (statParam == null) return "[UNDEFINED]";
                    var statValue = statParam.Value;
                    var statValidValueForParam = statIds.Find(vv => vv.Equals(statValue, StringComparison.InvariantCultureIgnoreCase));
                    return statValidValueForParam ?? "[UNDEFINED]";
                case "wherefrom":
                    var fromInventoryParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals("FromInventory", StringComparison.InvariantCultureIgnoreCase));
                    if (fromInventoryParam == null) return "[UNDEFINED]";
                    var fromInventoryValue = fromInventoryParam.Value;
                    if (!bool.TryParse(fromInventoryValue, out bool fromInventoryParsed))
                        return "[UNDEFINED]";
                    if (fromInventoryParsed)
                        return "From Inventory";
                    return "Out of Thin Air";
                case "actionsfromschool":
                    var actionSchoolParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals("ActionSchool", StringComparison.InvariantCultureIgnoreCase));
                    if (actionSchoolParam == null) return "[UNDEFINED]";
                    var actionSchoolValue = actionSchoolParam.Value;
                    if (actionSchoolValue.Equals("All", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return "All Actions";
                    }
                    else if (actionSchoolValue.Equals("<<CUSTOM>>", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var actionIdParam = effectParameters.FirstOrDefault(p => p.ParamName.Equals("CustomId", StringComparison.InvariantCultureIgnoreCase));
                        if (actionIdParam == null) return "[UNDEFINED]";
                        var actionIdValue = actionIdParam.Value;
                        return $"Action {actionIdValue}";
                    }
                    return $"All {actionSchoolValue} Actions";
                default:
                    return "[UNDEFINED]";
            }
        }
    }

    public class EffectParameter
    {
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ParameterType Type => Enum.Parse<ParameterType>(ParameterType);
        public string ParameterType { get; set; }
        public string Default { get; set; }
        public List<EffectParameterValidValue> ValidValues { get; set; }
        public string Required { get; set; }
        public string ReadOnly { get; set; }
        public List<EffectParameterColumn> Columns { get; set; }
    }

    public class EffectParameterValidValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class EffectParameterColumn
    {
        public string Key { get; set; }
        public string Header { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
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
        NPC,
        Item,
        Trap,
        ActionSchool,
        TileType,
        AlteredStatus,
        Number,
        Area,
        BooleanExpression,
        Key,
        Stat,
        Element,
        Script,
        Table
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

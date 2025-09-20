using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RogueCustomsGameEngine.Utils
{
    public static class EngineConstants
    {
        public readonly static string CurrentDungeonJsonVersion = Assembly.GetExecutingAssembly()
                                        .GetCustomAttribute<AssemblyFileVersionAttribute>()
                                        .Version;

        public const int MaxGenerationTries = 50;
        public const int MaxGenerationTriesForHallway = 25;
        public const int MaxGenerationTriesForRiver = 25;
        public const int MaxGenerationTriesForLake = 25;
        public const int LogMessagesToSend = 200;
        public const int MinRoomWidthOrHeight = 5;

        public const int FullRoomSightRange = -1;
        public const int FullRoomSightRangeForHallways = 1;
        public const int FullMapSightRange = -2;

        public static readonly string ExpressionSplitterRegexPattern = @"(==|!=|<=|>=|&&|\|\||<|>)";

        public static readonly string DiceNotationRegexPattern = @"(\d+)?d(\d+)([kK]\d+)?([+-]\d+)?([+-]\d+d\d+)?([*/]\d+)??";

        public static readonly string IntervalRegexPattern = @"\[(\d+);(\d+)\]";

        public static readonly string FlagRegexPattern = @"\[(?!\d+;\d+\])([^\[\];]+)\]";

        public static readonly Regex CirclePattern = new Regex(@"Circle\s*\(Diametre\s*(\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex SquarePattern = new Regex(@"Square\s*\((\d+)x\d+\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly string CurrencyRegexPattern = @"^Currency \(([^)]+)\)$";

        public static readonly string[] EffectsThatTriggerOnAttacked = new[]
        {
            "DealDamage", "StealItem", "ApplyAlteredStatus", "CleanseAlteredStatus", "CleanseAllAlteredStatuses",
            "ApplyStatAlteration", "CleanseStatAlteration", "CleanseStatAlterations", "ForceSkipTurn", "Teleport"
        };

        public const int RESOURCE_STAT_CAP = 99999;
        public const int NORMAL_STAT_CAP = 9999;
        public const int MOVEMENT_STAT_CAP = 9;
        public const decimal REGEN_STAT_CAP = 999;

        public const int MIN_ACCURACY_CAP = 0;
        public const int MAX_ACCURACY_CAP = 200;
        public const int MIN_EVASION_CAP = -100;
        public const int MAX_EVASION_CAP = 100;

        public static readonly string LOOT_NO_DROP = "No Drop";
        public static readonly string LOOT_EQUIPPABLE = "Equippable";

        public static readonly string[] SPECIAL_LOOT_ENTRIES = [ LOOT_NO_DROP, LOOT_EQUIPPABLE ];

        public static readonly string SPAWN_ANY_CHARACTER = "AnyCharacter";
        public static readonly string SPAWN_PLAYER_CHARACTER = "PlayerCharacter";
        public static readonly string SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER = "AnyAllyIncludingPlayer";
        public static readonly string SPAWN_ANY_ALLIED_CHARACTER = "AnyAlly";
        public static readonly string SPAWN_ANY_NEUTRAL_CHARACTER = "AnyNeutral";
        public static readonly string SPAWN_ANY_ENEMY_CHARACTER = "AnyEnemy";
        public static readonly string SPAWN_ANY_ITEM = "AnyItem";
        public static readonly string SPAWN_ANY_TRAP = "AnyTrap";
    }
}

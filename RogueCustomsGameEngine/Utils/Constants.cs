using System.Collections.Generic;
using System.Reflection;

namespace RogueCustomsGameEngine.Utils
{
    public static class Constants
    {
        public readonly static string CurrentDungeonJsonVersion = Assembly.GetExecutingAssembly()
                                        .GetCustomAttribute<AssemblyFileVersionAttribute>()
                                        .Version;

        public const int MaxGenerationTries = 1000;
        public const int MaxGenerationTriesForHallway = 10;
        public const int LogMessagesToSend = 200;
        public const int MinRoomWidthOrHeight = 5;

        public const int FullRoomSightRange = -1;
        public const int FullRoomSightRangeForHallways = 1;
        public const int FullMapSightRange = -2;

        public static readonly string DiceNotationRegexPattern = "(\\d+)?d(\\d+)([\\+\\-]\\d+)?";

        public static readonly string[] EffectsThatTriggerOnAttacked = new[]
        {
            "DealDamage", "StealItem", "ApplyAlteredStatus", "CleanseAlteredStatus", "CleanseAllAlteredStatuses",
            "ApplyStatAlteration", "CleanseStatAlteration", "CleanseStatAlterations", "ForceSkipTurn"
        };

        public const int RESOURCE_STAT_CAP = 99999;
        public const int NORMAL_STAT_CAP = 9999;
        public const int MOVEMENT_STAT_CAP = 9;
        public const decimal REGEN_STAT_CAP = 999;

        public const int HOURS_BEFORE_DUNGEON_CACHE_DELETION = 2;
    }
}

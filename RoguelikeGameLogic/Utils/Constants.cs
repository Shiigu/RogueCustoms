namespace RoguelikeGameEngine.Utils
{
    public static class Constants
    {
        public const int MaxGenerationTries = 1000;
        public const int MaxGenerationTriesForHallway = 10;
        public const int LogMessagesToSend = 200;
        public const int MinRoomWidthOrHeight = 5;

        public const int FullRoomSightRange = -1;
        public const int FullRoomSightRangeForHallways = 1;
        public const int FullMapSightRange = -2;

        public static readonly string DiceNotationRegexPattern = "(\\d+)?d(\\d+)([\\+\\-]\\d+)?";
    }
}

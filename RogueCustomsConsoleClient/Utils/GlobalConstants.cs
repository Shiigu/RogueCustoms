using System.Reflection;

namespace RogueCustomsConsoleClient.Utils
{
    public static class GlobalConstants
    {
        public const int ScreenCellWidth = 121;
        public const int ScreenCellHeight = 70;

        public static readonly string GameVersion = Assembly.GetExecutingAssembly()
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion;
    }
}

using SadConsole;
using System.Text;
using RoguelikeConsoleClient.UI.Consoles;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.Utils;
using RoguelikeConsoleClient.Resources.Localization;
using System;

namespace RoguelikeConsoleClient
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            SadConsole.Settings.AllowWindowResize = false;
            SadConsole.Settings.ResizeMode = SadConsole.Settings.WindowResizeOptions.Fit;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            BackendHandler.CreateInstance(Settings.Default.IsLocal, Settings.Default.ServerAddress);
            LocalizationManager.Build();
            SadConsole.Settings.WindowTitle = LocalizationManager.GetString("GameTitle").ToAscii();

            Game.Create(GlobalConstants.ScreenCellWidth, GlobalConstants.ScreenCellHeight, "fonts/IBMCGA.font");

            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();

            Environment.Exit(0);
        }

        static void Init()
        {
            RootScreen container = new();
            Game.Instance.Screen = container;
            Game.Instance.Screen.IsFocused = true;
            Game.Instance.DestroyDefaultStartingConsole();

            container.Render(TimeSpan.FromMilliseconds(1000 / (double) 24));
        }
    }
}
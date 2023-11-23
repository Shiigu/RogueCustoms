using SadConsole;
using SadConsole.Configuration;
using System.Text;
using RogueCustomsConsoleClient.UI.Consoles;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.Utils;
using RogueCustomsConsoleClient.Resources.Localization;
using System;

namespace RogueCustomsConsoleClient
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

            var gameStartup = new Builder()
                .SetScreenSize(GlobalConstants.ScreenCellWidth, GlobalConstants.ScreenCellHeight)
                .UseDefaultConsole()
                .OnStart(OnStart)
                .IsStartingScreenFocused(true)
                .ConfigureFonts((fontConfig, game) =>
                {
                    fontConfig.UseCustomFont("fonts/IBMCGA.font");
                });

            Game.Create(gameStartup);
            Game.Instance.Keyboard.InitialRepeatDelay = 0.6f;
            Game.Instance.Keyboard.RepeatDelay = 0.2f;
            Game.Instance.Run();
            Game.Instance.Dispose();

            Environment.Exit(0);
        }

        private static void OnStart(object sender, GameHost gameHost)
        {
            RootScreen container = new();
            Game.Instance.Screen = container;
            Game.Instance.Screen.IsFocused = true;
            Game.Instance.DestroyDefaultStartingConsole();

            container.Render(TimeSpan.FromMilliseconds(1000 / (double) 24));
        }
    }
}
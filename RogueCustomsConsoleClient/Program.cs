using SadConsole;
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

            Game.Create(GlobalConstants.ScreenCellWidth, GlobalConstants.ScreenCellHeight, "fonts/IBMCGA.font");

            Game.Instance.Keyboard.InitialRepeatDelay = 0.6f;
            Game.Instance.Keyboard.RepeatDelay = 0.1f;
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
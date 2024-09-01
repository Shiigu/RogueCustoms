using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using RogueCustomsGodotClient.Utils;

using FileAccess = Godot.FileAccess;

namespace RogueCustomsGodotClient
{
    public partial class ExceptionLogger : Node
    {
        private GlobalState _globalState;
        public override void _Ready()
        {
            _globalState = GetNode<GlobalState>("/root/GlobalState");

            // Unhandled exceptions in the main thread and other threads.
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // Unobserved task exceptions.
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            LogMessage(ex);
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogMessage(e.Exception);
            e.SetObserved();
        }

        private void LogMessage(Exception ex)
        {
            if (!_globalState.CanWriteLogs) return;
            using var file = FileAccess.Open(_globalState.LogFilePath, FileAccess.ModeFlags.ReadWrite);
            file.StoreLine($"AT {DateTime.Now:s}:");
            file.StoreLine("");
            file.StoreLine(ex.GetType().Name);
            file.StoreLine(ex.Message);
            file.StoreLine("-------------");
            file.StoreLine("Inner exception:");
            file.StoreLine(ex.InnerException?.Message);
            file.StoreLine(ex.InnerException?.StackTrace);
            file.StoreLine("-------------");
            file.StoreLine(ex.StackTrace);
            file.StoreLine("");
            file.StoreLine("------------------------------------");
            _globalState.MessageScreenType = MessageScreenType.Error;
            GetTree().ChangeSceneToFile("res://Screens/MessageScreen.tscn");
        }
    }
}

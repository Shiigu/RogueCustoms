using Godot;

using RogueCustomsGameEngine.Management;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;

public partial class GlobalState : Node
{
	public DungeonListDto PossibleDungeonInfo { get; set; }
	public DungeonDto DungeonInfo { get; set; }
	public DungeonManager DungeonManager { get; set; } = new();
	public MessageScreenType MessageScreenType { get; set; }
	public ControlMode PlayerControlMode { get; set; }
    public int DungeonId { get; set; }
    public static string GameLocale => TranslationServer.Translate("LanguageLocale");
	public bool MustUpdateGameScreen { get; set; }
    public bool HasSaveGame => Godot.FileAccess.FileExists(SaveGamePath);
    public readonly string SettingsPath = "./Settings.cfg";
    public readonly string SaveGamePath = "./savedDungeon.rcs";
    private readonly string LogFolder = "./Logs";
    public readonly string DungeonsFolder = "./JSON";
    public string LogFilePath { get; private set; }
    public bool CanWriteLogs;
    public string ExceptionText { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var dir = DirAccess.Open("./");
        if (!dir.DirExists("Logs"))
        {
            Error err = dir.MakeDir("Logs");
            if (err == Error.Ok)
            {
                GD.Print("Logs folder created");
            }
            else
            {
                GD.PrintErr("Failed to create Logs directory");
                CanWriteLogs = false;
                return;
            }
        }
        if (!dir.DirExists("JSON"))
        {
            Error err = dir.MakeDir("JSON");
            if (err == Error.Ok)
            {
                GD.Print("JSON folder created");
            }
            else
            {
                GD.PrintErr("Failed to create JSON directory");
                CanWriteLogs = false;
                return;
            }
        }
        LogFilePath = $"{LogFolder}/{DateTime.Now.ToString("s").Replace(":", "-")}.txt";
        using var f = FileAccess.Open(LogFilePath, FileAccess.ModeFlags.Write);
        CanWriteLogs = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

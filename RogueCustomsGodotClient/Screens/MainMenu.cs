using Godot;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Entities;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

using FileAccess = Godot.FileAccess;

public partial class MainMenu : Control
{
    private ExceptionLogger _exceptionLogger;
    private Button _startDungeonButton;
    private Button _loadSavedDungeonButton;
    private Button _optionsButton;
    private Button _exitButton;
    private Label _versionLabel;
    private GlobalState _globalState;
    private InputManager _inputManager;
    private List<Button> _buttons;
    private int _index = -1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _startDungeonButton = GetNode<Button>("StartDungeonButton");
        _loadSavedDungeonButton = GetNode<Button>("LoadSavedDungeonButton");
        _optionsButton = GetNode<Button>("OptionsButton");
        _exitButton = GetNode<Button>("ExitButton");
        _versionLabel = GetNode<Label>("VersionLabel");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");

        _startDungeonButton.Pressed += OnStartDungeonPressed;
        _loadSavedDungeonButton.Pressed += OnLoadSavedDungeonPressed;
        _optionsButton.Pressed += () => _ = OnOptionsPressed();
        _exitButton.Pressed += OnExitPressed;

        _buttons = new List<Button> { _startDungeonButton, _loadSavedDungeonButton, _optionsButton, _exitButton };

        LoadSavedSettings();

        GetSaveGames();
        _globalState.PlayerControlMode = ControlMode.NormalMove;
    }

    public override void _EnterTree()
    {
        CallDeferred(nameof(PrepareFocus));
        base._EnterTree();
    }

    private void PrepareFocus()
    {
        _buttons[0].GrabFocus();
        _index = 0;
    }

    private void GetSaveGames()
    {
        _globalState.SavedGames = new();
        _loadSavedDungeonButton.Disabled = true;
        
        using var dir = DirAccess.Open(_globalState.SaveGameFolder);
        if (dir == null) return;

        dir.ListDirBegin();

        string fileName;
        while ((fileName = dir.GetNext()) != "")
        {
            if (!fileName.EndsWith(_globalState.SaveGameExtension))
                continue;

            string filePath = $"{_globalState.SaveGameFolder}/{fileName}";

            try
            {
                var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
                try
                {
                    var json = file.GetAsText();
                    var save = JsonSerializer.Deserialize<SaveGame>(json);

                    if (save?.DungeonVersion.Equals(GlobalConstants.CurrentDungeonJsonVersion) == true)
                    {
                        _globalState.SavedGames.Add((filePath, save));
                    }
                }
                finally
                {
                    file.Close();
                }
            }
            catch
            {
                // Do nothing, if we can't load, we pretend it's not there
            }
        }

        dir.ListDirEnd();
        _loadSavedDungeonButton.Disabled = _globalState.SavedGames.Count == 0;
    }

    private void UpdateUIWithLocalization()
    {
        _startDungeonButton.Text = TranslationServer.Translate("SelectDungeonText");
        _loadSavedDungeonButton.Text = TranslationServer.Translate("LoadDungeonText");
        _optionsButton.Text = TranslationServer.Translate("OptionsButtonText");
        _exitButton.Text = TranslationServer.Translate("ExitButtonText");
        var gameVersionText = TranslationServer.Translate("GameVersionText").ToString();
        _versionLabel.Text = gameVersionText.Format(new { GameVersion = GlobalConstants.GameVersion });
    }

    private void LoadSavedSettings()
    {
        if(_globalState.Options == null)
        {
            _globalState.Options = new();
            var config = new ConfigFile();
            Error err = config.Load(_globalState.SettingsPath);
            GD.Print(err.ToString());
            if (err == Error.Ok)
            {
                var savedLocale = (string)config.GetValue("Localization", "locale", "English");
                var savedSortActionMode = (string)config.GetValue("GameOptions", "sortactionmode", nameof(SortActionMode.Default));
                var savedFlashEffectModeMode = (string)config.GetValue("GameOptions", "flasheffectmode", nameof(FlashEffectMode.FullScreen));
                var savedHighlightPlayerOnFloorStart = (string)config.GetValue("GameOptions", "highlightplayeronfloorstart", false.ToString());
                var savedInactiveControlShowMode = (string)config.GetValue("GameOptions", "inactivecontrolshowmode", nameof(InactiveControlShowMode.Hide));
                TranslationServer.SetLocale(savedLocale);
                _globalState.Options.SortActionMode = Enum.Parse<SortActionMode>(savedSortActionMode);
                _globalState.Options.FlashEffectMode = Enum.Parse<FlashEffectMode>(savedFlashEffectModeMode);
                _globalState.Options.HighlightPlayerOnFloorStart = bool.Parse(savedHighlightPlayerOnFloorStart);
                _globalState.Options.InactiveControlShowMode = Enum.Parse<InactiveControlShowMode>(savedInactiveControlShowMode);
            }
            else
            {
                GD.Print("No settings file found. Using default settings.");
                var defaultLocale = TranslationServer.GetLoadedLocales()[0];
                TranslationServer.SetLocale(defaultLocale);
                _globalState.Options.SortActionMode = SortActionMode.Default;
                _globalState.Options.FlashEffectMode = FlashEffectMode.FullScreen;
                _globalState.Options.HighlightPlayerOnFloorStart = false;
                _globalState.Options.InactiveControlShowMode = InactiveControlShowMode.Hide;
            }
            SaveSettings();
        }
        UpdateUIWithLocalization();
    }

    private void SaveSettings()
    {
        var config = new ConfigFile();
        config.SetValue("Localization", "locale", _globalState.Options.Localization);
        config.SetValue("GameOptions", "sortactionmode", nameof(SortActionMode.Default));
        config.SetValue("GameOptions", "flasheffectmode", nameof(FlashEffectMode.FullScreen));
        config.SetValue("GameOptions", "highlightplayeronfloorstart", false.ToString());
        config.SetValue("GameOptions", "inactivecontrolshowmode", nameof(InactiveControlShowMode.Hide));
        config.Save(_globalState.SettingsPath);
        GD.Print($"Saved settings.");
    }

    private void OnStartDungeonPressed()
    {
        try
        {
            if (GetChildren().Any(c => c.IsPopUp())) return;
            _globalState.PossibleDungeonInfo = _globalState.DungeonManager.GetPickableDungeonList(_globalState.Options.Localization);
            GetTree().ChangeSceneToFile("res://Screens/PickDungeon.tscn");
        }
        catch (Exception ex)
        {
            _exceptionLogger.LogMessage(ex);
        }
    }

    private void OnLoadSavedDungeonPressed()
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        this.CreateLoadSaveGamePopup();
    }

    private async Task OnOptionsPressed()
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        await this.CreateOptionsPopup();
        UpdateUIWithLocalization();
    }

    private void OnExitPressed()
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        GetTree().Quit();
    }

    public override void _Input(InputEvent @event)
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            MoveFocus(-1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            MoveFocus(1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_accept"))
        {
            if (_index != -1 && !_buttons[_index].Disabled)
            {
                _buttons[_index].GrabFocus();
                _buttons[_index].EmitSignal("pressed");
                _buttons[_index].ButtonPressed = true;
            }
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            _exitButton.GrabFocus();
            _exitButton.EmitSignal("pressed");
            _exitButton.ButtonPressed = true;
            AcceptEvent();
        }
    }

    private void MoveFocus(int step)
    {
        do
        {
            _index = (_index + step + _buttons.Count) % _buttons.Count;
            _buttons[_index].GrabFocus();
        }
        while (_buttons[_index].Disabled);
    }
}

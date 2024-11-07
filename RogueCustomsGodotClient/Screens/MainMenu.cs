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
using System.Text.Json;

using FileAccess = Godot.FileAccess;

public partial class MainMenu : Control
{
    private ExceptionLogger _exceptionLogger;
    private Button _startDungeonButton;
    private Button _loadSavedDungeonButton;
    private OptionButton _languageDropdown;
    private Button _exitButton;
    private Label _versionLabel;
    private string[] _possibleLocales;
    private GlobalState _globalState;
    private InputManager _inputManager;
    private List<Button> _buttons;
    private int _selectedIndex = -1;

    private readonly StyleBoxFlat normalButtonStyle = GD.Load<StyleBoxFlat>("res://Styles/ButtonNormal.tres");
    private readonly StyleBoxFlat hoverButtonStyle = GD.Load<StyleBoxFlat>("res://Styles/ButtonHover.tres");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _startDungeonButton = GetNode<Button>("StartDungeonButton");
        _loadSavedDungeonButton = GetNode<Button>("LoadSavedDungeonButton");
        _languageDropdown = GetNode<OptionButton>("LanguageDropdown");
        _exitButton = GetNode<Button>("ExitButton");
        _versionLabel = GetNode<Label>("VersionLabel");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");

        _startDungeonButton.Pressed += OnStartDungeonPressed;
        _loadSavedDungeonButton.Pressed += OnLoadSavedDungeonPressed;
        _languageDropdown.ItemSelected += OnLanguageSelected;
        _exitButton.Pressed += OnExitPressed;

        _languageDropdown = GetNode<OptionButton>("LanguageDropdown");
        _buttons = new List<Button> { _startDungeonButton, _loadSavedDungeonButton, _languageDropdown, _exitButton };

        LoadSavedLocalization();

        SetupLocalizationOptions();

        GetSaveGames();
        _globalState.PlayerControlMode = ControlMode.NormalMove;
    }

    private void SelectButton(int index)
    {
        if (_buttons[index].Disabled)
        {
            if (index == _buttons.Count - 1)
                index = 0;
            if (index < _selectedIndex)
                index--;
            else
                index++;
        }
        foreach (var button in _buttons)
        {
            button.AddThemeStyleboxOverride("normal", normalButtonStyle);
        }
        _selectedIndex = index;
        _buttons[_selectedIndex].AddThemeStyleboxOverride("normal", hoverButtonStyle);
    }

    private void SetupLocalizationOptions()
    {
        string[] availableLocales = TranslationServer.GetLoadedLocales();
        _possibleLocales = new string[availableLocales.Length];
        int defaultLocaleIndex = -1;
        GD.Print($"Found {availableLocales.Length} locale(s)");
        var initialLocale = TranslationServer.GetLocale();

        for (int i = 0; i < availableLocales.Length; i++)
        {
            GD.Print($"Existing locale: {availableLocales[i]}");
            var locale = availableLocales[i];
            TranslationServer.SetLocale(locale);
            var localeName = TranslationServer.Translate("LanguageName");
            _languageDropdown.AddItem(localeName);
            _possibleLocales[i] = locale;

            if (locale == initialLocale)
            {
                defaultLocaleIndex = i;
            }
        }

        TranslationServer.SetLocale(initialLocale);
        if (defaultLocaleIndex > -1)
            _languageDropdown.Select(defaultLocaleIndex);
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
            if (fileName.EndsWith(_globalState.SaveGameExtension))
            {
                string filePath = $"{_globalState.SaveGameFolder}/{fileName}";

                var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
                var fileContent = file.GetAsText();
                var saveGame = JsonSerializer.Deserialize<SaveGame>(fileContent);

                if (saveGame.DungeonVersion.Equals(GlobalConstants.CurrentDungeonJsonVersion))
                    _globalState.SavedGames.Add((filePath, saveGame));
            }
        }

        dir.ListDirEnd();
        _loadSavedDungeonButton.Disabled = _globalState.SavedGames.Count == 0;
    }

    private void OnLanguageSelected(long index)
    {
        string selectedLocale = _possibleLocales[index];
        TranslationServer.SetLocale(selectedLocale);

        SaveLocalization(selectedLocale);

        UpdateUIWithLocalization();
    }

    private void UpdateUIWithLocalization()
    {
        _startDungeonButton.Text = TranslationServer.Translate("SelectDungeonText");
        _loadSavedDungeonButton.Text = TranslationServer.Translate("LoadDungeonText");
        _exitButton.Text = TranslationServer.Translate("ExitButtonText");
        var gameVersionText = TranslationServer.Translate("GameVersionText").ToString();
        _versionLabel.Text = gameVersionText.Format(new { GameVersion = GlobalConstants.GameVersion });
    }

    private void SaveLocalization(string locale)
    {
        var config = new ConfigFile();
        config.SetValue("Localization", "locale", locale);
        config.Save(_globalState.SettingsPath);
        GD.Print($"Saved default locale {locale}");
    }

    private void LoadSavedLocalization()
    {
        var config = new ConfigFile();
        Error err = config.Load(_globalState.SettingsPath);
        GD.Print(err.ToString());
        if (err == Error.Ok)
        {
            string savedLocale = (string)config.GetValue("Localization", "locale", "English");
            TranslationServer.SetLocale(savedLocale);
            UpdateUIWithLocalization();
        }
        else
        {
            GD.Print("No settings file found. Using default localization.");
            var defaultLocale = TranslationServer.GetLoadedLocales()[0];
            TranslationServer.SetLocale(defaultLocale);
            SaveLocalization(defaultLocale);
            UpdateUIWithLocalization();
        }
    }

    private void OnStartDungeonPressed()
    {
        try
        {
            _globalState.PossibleDungeonInfo = _globalState.DungeonManager.GetPickableDungeonList(GlobalState.GameLocale);
            GetTree().ChangeSceneToFile("res://Screens/PickDungeon.tscn");
        }
        catch (Exception ex)
        {
            _exceptionLogger.LogMessage(ex);
        }
    }

    private void OnLoadSavedDungeonPressed()
    {
        this.CreateLoadSaveGamePopup();
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }

    public override void _Input(InputEvent @event)
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            if (_selectedIndex == -1)
                SelectButton(0);
            else if (_selectedIndex == 0)
                SelectButton(_buttons.Count - 1);
            else
                SelectButton(_selectedIndex - 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            if (_selectedIndex == -1)
                SelectButton(0);
            else if (_selectedIndex == _buttons.Count - 1)
                SelectButton(0);
            else
                SelectButton(_selectedIndex + 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_accept"))
        {
            _buttons[_selectedIndex].GrabFocus();
            _buttons[_selectedIndex].EmitSignal("pressed");
            _buttons[_selectedIndex].ButtonPressed = true;
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
}

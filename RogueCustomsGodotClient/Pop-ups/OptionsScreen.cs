using Godot;
using Godot.Collections;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Utils;

using System;

public partial class OptionsScreen : Control
{
    private GlobalState _globalState;
    private InputManager _inputManager;
    private ExceptionLogger _exceptionLogger;
    private Label _titleLabel;
    private ScalableLabel _languageLabel, _sortActionLabel, _setFlashLabel, _inactiveControlsLabel;
    private CheckButton _highlightPositionCheckButton;
    private Button _applyButton, _cancelButton;
    private OptionButton _languageDropdown;
    private LocalizableOptionButton _sortActionDropdown, _setFlashDropdown, _inactiveControlsDropdown;

    private Panel _border;
    private MarginContainer _marginContainer;
    private ScrollContainer _scrollContainer;

    private string[] _possibleLocales;
    private string _initialLocale;

    private float _baseMarginContainerWidth, _baseBorderWidth, _baseScrollContainerWidth;

    [Signal]
    public delegate void PopupClosedEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _border = GetNode<Panel>("Border");
        _marginContainer = GetNode<MarginContainer>("MarginContainer");
        _scrollContainer = GetNode<ScrollContainer>("ScrollContainer");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _languageLabel = GetNode<ScalableLabel>("ScrollContainer/VBoxContainer/LanguageContainer/LanguageLabel");
        _languageDropdown = GetNode<OptionButton>("ScrollContainer/VBoxContainer/LanguageContainer/LanguageDropdown");
        _sortActionLabel = GetNode<ScalableLabel>("ScrollContainer/VBoxContainer/SortActionContainer/SortActionLabel");
        _sortActionDropdown = GetNode<LocalizableOptionButton>("ScrollContainer/VBoxContainer/SortActionContainer/SortActionDropdown");
        _setFlashLabel = GetNode<ScalableLabel>("ScrollContainer/VBoxContainer/SetFlashContainer/SetFlashLabel");
        _setFlashDropdown = GetNode<LocalizableOptionButton>("ScrollContainer/VBoxContainer/SetFlashContainer/SetFlashDropdown");
        _inactiveControlsLabel = GetNode<ScalableLabel>("ScrollContainer/VBoxContainer/InactiveControlsContainer/InactiveControlsLabel");
        _inactiveControlsDropdown = GetNode<LocalizableOptionButton>("ScrollContainer/VBoxContainer/InactiveControlsContainer/InactiveControlsDropdown");
        _highlightPositionCheckButton = GetNode<CheckButton>("ScrollContainer/VBoxContainer/HighlightPositionContainer/HighlightPositionCheckButton");
        _applyButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/ApplyButton");
        _cancelButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/CancelButton");
    }

    public void Show(Action onCloseCallback)
    {
        _baseMarginContainerWidth = _marginContainer.Size.X;
        _baseBorderWidth = _border.Size.X;
        _baseScrollContainerWidth = _scrollContainer.Size.X;

        SetupDropdowns();

        _highlightPositionCheckButton.ButtonPressed = _globalState.Options.HighlightPlayerOnFloorStart;

        SetupLocalizationOptions();
        UpdateUIWithLocalization();

        _scrollContainer.Resized += () =>
        {
            _border.Size = new(_baseBorderWidth + (_scrollContainer.Size.X - _baseScrollContainerWidth), _border.Size.Y);
            _marginContainer.Size = new(_baseMarginContainerWidth + (_scrollContainer.Size.X - _baseScrollContainerWidth), _marginContainer.Size.Y);

            var screenSize = GetViewportRect().Size;
            Position = (screenSize - _border.Size) / 2;
        };

        _languageDropdown.ItemSelected += OnLanguageSelected;

        _cancelButton.Pressed += () =>
        {
            TranslationServer.SetLocale(_initialLocale);
            EmitSignal(nameof(PopupClosed));
            onCloseCallback?.Invoke();
            QueueFree();
        };

        _applyButton.Pressed += () =>
        {
            _globalState.Options.SortActionMode = _sortActionDropdown.Selected switch
            {
                0 => SortActionMode.Default,
                1 => SortActionMode.UsableActionsFirst,
                2 => SortActionMode.CursorOnFirstUsableAction,
                _ => SortActionMode.Default
            };
            _globalState.Options.FlashEffectMode = _setFlashDropdown.Selected switch
            {
                0 => FlashEffectMode.FullScreen,
                1 => FlashEffectMode.MapSection,
                2 => FlashEffectMode.Hide,
                _ => FlashEffectMode.FullScreen
            };
            _globalState.Options.InactiveControlShowMode = _inactiveControlsDropdown.Selected switch
            {
                0 => InactiveControlShowMode.Hide,
                1 => InactiveControlShowMode.Dim,
                _ => InactiveControlShowMode.Hide
            };
            _globalState.Options.HighlightPlayerOnFloorStart = _highlightPositionCheckButton.ButtonPressed;
            SaveSettings();
            onCloseCallback?.Invoke();
            EmitSignal(nameof(PopupClosed));
            QueueFree();
        };

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;
    }

    private void SetupLocalizationOptions()
    {
        string[] availableLocales = TranslationServer.GetLoadedLocales();
        _possibleLocales = new string[availableLocales.Length];
        int defaultLocaleIndex = -1;
        GD.Print($"Found {availableLocales.Length} locale(s)");
        _initialLocale = TranslationServer.GetLocale();

        for (int i = 0; i < availableLocales.Length; i++)
        {
            GD.Print($"Existing locale: {availableLocales[i]}");
            var locale = availableLocales[i];
            TranslationServer.SetLocale(locale);
            var localeName = TranslationServer.Translate("LanguageName");
            _languageDropdown.AddItem(localeName);
            _possibleLocales[i] = locale;

            if (locale == _initialLocale)
            {
                defaultLocaleIndex = i;
            }
        }

        TranslationServer.SetLocale(_initialLocale);
        if (defaultLocaleIndex > -1)
            _languageDropdown.Select(defaultLocaleIndex);
    }

    private void SetupDropdowns()
    {
        _sortActionDropdown.SetTranslationKeys(["OptionsSortActionsByDefaultText", "OptionsSortActionsByUsableText", "OptionsSortActionsByCursorOnUsableText"]);

        _sortActionDropdown.Selected = _globalState.Options.SortActionMode switch
        {
            SortActionMode.Default => 0,
            SortActionMode.UsableActionsFirst => 1,
            SortActionMode.CursorOnFirstUsableAction => 2,
            _ => 0
        };

        _setFlashDropdown.SetTranslationKeys(["OptionsFlashCoversFullScreenText", "OptionsFlashCoversMapText", "OptionsFlashCoversNoneText"]);

        _setFlashDropdown.Selected = _globalState.Options.FlashEffectMode switch
        {
            FlashEffectMode.FullScreen => 0,
            FlashEffectMode.MapSection => 1,
            FlashEffectMode.Hide => 2,
            _ => 0
        };

        _inactiveControlsDropdown.SetTranslationKeys(["OptionsInactiveControlsHideText", "OptionsInactiveControlsDimText"]);

        _inactiveControlsDropdown.Selected = _globalState.Options.InactiveControlShowMode switch
        {
            InactiveControlShowMode.Hide => 0,
            InactiveControlShowMode.Dim => 1,
            _ => 0
        };
    }

    private void OnLanguageSelected(long index)
    {
        string selectedLocale = _possibleLocales[index];
        TranslationServer.SetLocale(selectedLocale);

        UpdateUIWithLocalization();
    }

    private void UpdateUIWithLocalization()
    {
        _titleLabel.Text = TranslationServer.Translate("OptionsWindowTitleText");

        _languageLabel.SetText(TranslationServer.Translate("OptionsLanguageText"));
        _sortActionLabel.SetText(TranslationServer.Translate("OptionsSortActionsByText"));
        _sortActionDropdown.RefreshItems();
        _setFlashLabel.SetText(TranslationServer.Translate("OptionsFlashCoversText"));
        _setFlashDropdown.RefreshItems();
        _inactiveControlsLabel.SetText(TranslationServer.Translate("OptionsInactiveControlsText"));
        _inactiveControlsDropdown.RefreshItems();
        _highlightPositionCheckButton.Text = TranslationServer.Translate("OptionsHighlightPlayerOnFloorStartText");

        _applyButton.Text = TranslationServer.Translate("ApplyButtonText");
        _cancelButton.Text = TranslationServer.Translate("CancelButtonText");
    }

    private void SaveSettings()
    {
        var config = new ConfigFile();
        config.SetValue("Localization", "locale", _globalState.Options.Localization);
        config.SetValue("GameOptions", "sortactionmode", _globalState.Options.SortActionMode.ToString());
        config.SetValue("GameOptions", "flasheffectmode", _globalState.Options.FlashEffectMode.ToString());
        config.SetValue("GameOptions", "highlightplayeronfloorstart", _globalState.Options.HighlightPlayerOnFloorStart.ToString());
        config.SetValue("GameOptions", "inactivecontrolshowmode", _globalState.Options.InactiveControlShowMode.ToString());
        config.Save(_globalState.SettingsPath);
        GD.Print($"Saved settings.");
    }
}

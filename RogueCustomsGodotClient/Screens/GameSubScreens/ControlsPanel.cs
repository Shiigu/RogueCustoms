using Godot;

using RogueCustomsGodotClient.Screens.GameSubScreens;
using RogueCustomsGodotClient.Utils;

using System;

public partial class ControlsPanel : GamePanel
{
    private GlobalState _globalState;
    private Label _controlsLabel1, _controlsLabel2, _controlsLabel3;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _controlsLabel1 = GetNode<Label>("ControlsContainer/ControlsLabel1");
        _controlsLabel2 = GetNode<Label>("ControlsContainer/ControlsLabel2");
        _controlsLabel3 = GetNode<Label>("ControlsContainer/ControlsLabel3");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void Update()
    {
        if(_globalState.Options.InactiveControlShowMode == InactiveControlShowMode.Hide)
        {
            _controlsLabel1.Text = "";
            _controlsLabel2.Text = "";
            _controlsLabel3.Text = "";
        }
        else if (_globalState.Options.InactiveControlShowMode == InactiveControlShowMode.Dim)
        {
            _controlsLabel1.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            _controlsLabel2.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            _controlsLabel3.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }

        switch (_globalState.PlayerControlMode)
        {
            case ControlMode.NormalMove:
                _controlsLabel1.Text = TranslationServer.Translate("MoveModeNormalControlsText");
                _controlsLabel2.Text = TranslationServer.Translate("MoveModeControlsSubText");
                _controlsLabel3.Text = TranslationServer.Translate("MenuControlsSubText");
                break;
            case ControlMode.NormalOnStairs:
                _controlsLabel1.Text = TranslationServer.Translate("MoveModeOnStairsControlsText");
                _controlsLabel2.Text = TranslationServer.Translate("MoveModeControlsSubText");
                _controlsLabel3.Text = TranslationServer.Translate("MenuControlsSubText");
                break;
            case ControlMode.Immobilized:
                _controlsLabel1.Text = TranslationServer.Translate("MoveModeImmobilizedControlsText");
                _controlsLabel2.Text = TranslationServer.Translate("MoveModeControlsSubText");
                _controlsLabel3.Text = TranslationServer.Translate("MenuControlsSubText");
                break;
            case ControlMode.ImmobilizedOnStairs:
                _controlsLabel1.Text = TranslationServer.Translate("MoveModeImmobilizedOnStairsControlsText");
                _controlsLabel2.Text = TranslationServer.Translate("MoveModeControlsSubText");
                _controlsLabel3.Text = TranslationServer.Translate("MenuControlsSubText");
                break;
            case ControlMode.MustSkipTurn:
                _controlsLabel1.Text = TranslationServer.Translate("MoveModeCannotActControlsText");
                _controlsLabel2.Text = TranslationServer.Translate("MoveModeControlsSubText");
                _controlsLabel3.Text = TranslationServer.Translate("MenuControlsSubText");
                break;
            case ControlMode.Targeting:
                _controlsLabel1.Text = "";
                _controlsLabel2.Text = TranslationServer.Translate("AimModeControlsText");
                _controlsLabel3.Text = "";
                break;
            case ControlMode.PreMoveHighlight:
                _controlsLabel1.Text = "";
                _controlsLabel2.Text = TranslationServer.Translate("PreMoveHighlightControlsText");
                _controlsLabel3.Text = "";
                break;
            case ControlMode.Waiting:
                if (_globalState.Options.InactiveControlShowMode == InactiveControlShowMode.Hide)
                {
                    _controlsLabel1.Text = "";
                    _controlsLabel2.Text = "";
                    _controlsLabel3.Text = "";
                }
                else if (_globalState.Options.InactiveControlShowMode == InactiveControlShowMode.Dim)
                {
                    _controlsLabel1.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
                    _controlsLabel2.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
                    _controlsLabel3.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
                }
                break;
            case ControlMode.None:
                _controlsLabel1.Text = "";
                _controlsLabel2.Text = TranslationServer.Translate("MoveModeNoneControlsText");
                _controlsLabel3.Text = "";
                break;
        }
    }
}

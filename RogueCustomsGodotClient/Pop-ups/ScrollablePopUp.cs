using Godot;

using RogueCustomsGodotClient.Utils;

using System;

public partial class ScrollablePopUp : Control
{
    private GlobalState _globalState;
    private InputManager _inputManager;
    private Label _titleLabel;
    private ScrollContainer _scrollContainer;
    private RichTextLabel _innerTextLabel;
    private Button _closeButtonText;

    private Panel _border;

    private const int _scrollStep = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _scrollContainer = GetNode<ScrollContainer>("MarginContainer/VBoxContainer/ScrollContainer");
        _innerTextLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/ScrollContainer/InnerTextLabel");
        _closeButtonText = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/CloseButton");
        _border = GetNode<Panel>("Border");
    }

    public void Show(string titleText, string innerText, Color borderColor, Action okCallback, bool scrollToEnd)
    {
        _titleLabel.Text = titleText;
        _closeButtonText.Text = TranslationServer.Translate("CloseButtonText");
        _innerTextLabel.ScrollFollowing = scrollToEnd;
        _innerTextLabel.SizeFlagsVertical = SizeFlags.ExpandFill;
        _innerTextLabel.Text = innerText;
        _innerTextLabel.FitContent = true;

        _closeButtonText.Pressed += () =>
        {
            okCallback?.Invoke();
            QueueFree();
        };

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = borderColor;
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = borderColor;
        _border.AddThemeStyleboxOverride("panel", borderStyleBox);

        _scrollContainer.ScrollVertical = 0;
        _scrollContainer.GrabFocus();

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") || @event.IsActionPressed("ui_cancel"))
        {
            _closeButtonText.GrabFocus();
            _closeButtonText.EmitSignal("pressed");
            _closeButtonText.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            _scrollContainer.ScrollVertical -= _scrollStep;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            _scrollContainer.ScrollVertical += _scrollStep;
            AcceptEvent();
        }
    }
}

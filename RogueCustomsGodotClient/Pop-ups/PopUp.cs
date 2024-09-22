using Godot;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class PopUp : Control
{
    private GlobalState _globalState;
    private Label _titleLabel;
    private RichTextLabel _innerTextLabel;
    private HBoxContainer _buttonContainer;
    private Panel _border;
    private MarginContainer _marginContainer;
    private VBoxContainer _vBoxContainer;
    private string _innerTextWithoutBbCode;
    private readonly List<PopUpButton> _buttons = new();
    private Vector2 _originalLabelSize;

    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _innerTextLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/InnerTextLabel");
        _buttonContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer/ButtonContainer");
        _border = GetNode<Panel>("Border");
        _marginContainer = GetNode<MarginContainer>("MarginContainer");
        _vBoxContainer = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
    }

    public void UpdateSize()
    {
        var labelSize = _innerTextWithoutBbCode.GetSizeToFitForDimensionsWithoutBbCode(_innerTextLabel.GetThemeDefaultFont(), 900, 900);
        _innerTextLabel.CustomMinimumSize = new Vector2(labelSize.X, labelSize.Y + 12);

        _marginContainer.Size = new Vector2(Mathf.Min(_innerTextLabel.Size.X, 900), Mathf.Min(_titleLabel.Size.Y + _innerTextLabel.Size.Y + _buttonContainer.Size.Y - 12, 900));
        _border.Position = new Vector2(0, 25);
        _border.Size = new Vector2(_marginContainer.Size.X, _marginContainer.Size.Y - 50);

        _titleLabel.Size = new Vector2(Mathf.Min(_titleLabel.Size.X + 600, 900), _titleLabel.Size.Y);

        CustomMinimumSize = _marginContainer.CustomMinimumSize;
    }

    public void Show(string titleText, string innerText, PopUpButton[] buttons, Color borderColor, Action modalCloseCallback)
    {
        _originalLabelSize = _innerTextLabel.Size;
        _innerTextWithoutBbCode = Regex.Replace(innerText, @"\[(.*?)\]", string.Empty);
        _titleLabel.Text = titleText;
        _innerTextLabel.BbcodeEnabled = true;
        _innerTextLabel.Text = innerText.ToBbCodeAppropriateString();

        foreach (var child in _buttonContainer.GetChildren())
        {
            child.QueueFree();
        }

        var normalButtonStyleBox = (StyleBoxFlat)GlobalConstants.ButtonNormalStyle.Duplicate();
        normalButtonStyleBox.BorderColor = borderColor;
        var hoverButtonStyleBox = (StyleBoxFlat)GlobalConstants.ButtonHoverStyle.Duplicate();
        hoverButtonStyleBox.BorderColor = borderColor;

        foreach (PopUpButton popUpButton in buttons)
        {
            var button = new Button
            {
                Text = popUpButton.Text,
                CustomMinimumSize = new Vector2 { X = 50, Y = 26 },
                FocusMode = FocusModeEnum.Click
            };
            button.Pressed += () =>
            {
                popUpButton.Callback?.Invoke();
                modalCloseCallback?.Invoke();
                QueueFree();
            };
            button.AddThemeStyleboxOverride("normal", normalButtonStyleBox);
            button.AddThemeStyleboxOverride("hover", hoverButtonStyleBox);
            button.AddThemeStyleboxOverride("pressed", normalButtonStyleBox);
            _buttonContainer.AddChild(button);
            popUpButton.AssociateButton(button);
            _buttons.Add(popUpButton);
        }

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = borderColor;
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = borderColor;
        _border.AddThemeStyleboxOverride("panel", borderStyleBox);

        UpdateSize();

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;
    }

    public override void _Input(InputEvent @event)
    {
        foreach (var button in _buttons)
        {
            if (@event.IsActionPressed(button.ActionPress))
            {
                button.AssociatedButton.GrabFocus();
                button.AssociatedButton.EmitSignal("pressed");
                button.AssociatedButton.ButtonPressed = true;
                AcceptEvent();
            }
        }
    }
}

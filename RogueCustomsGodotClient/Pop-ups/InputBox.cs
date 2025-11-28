using Godot;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.Linq;
using System.Text.RegularExpressions;

public partial class InputBox : Control
{
    private GlobalState _globalState;
    private Label _titleLabel;
    private RichTextLabel _promptTextLabel;
    private LineEdit _inputTextBox;
    private HBoxContainer _buttonContainer;
    private Button _okButton, _cancelButton;
    private Panel _border;
    private MarginContainer _marginContainer;
    private VBoxContainer _vBoxContainer;
    private string _innerTextWithoutBbCode;

    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _promptTextLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/PromptTextLabel");
        _buttonContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer/ButtonContainer");
        _okButton = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/OkButton");
        _cancelButton = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/CancelButton");
        _inputTextBox = GetNode<LineEdit>("MarginContainer/VBoxContainer/InputTextBox");
        _border = GetNode<Panel>("Border");
        _marginContainer = GetNode<MarginContainer>("MarginContainer");
        _vBoxContainer = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
    }

    public void UpdateSize()
    {
        var labelSize = _innerTextWithoutBbCode.GetSizeToFitForDimensionsWithoutBbCode(_promptTextLabel.GetThemeDefaultFont(), 900, 900);
        _promptTextLabel.CustomMinimumSize = new Vector2(labelSize.X, labelSize.Y + 12);

        _marginContainer.Size = new Vector2(Mathf.Min(_promptTextLabel.Size.X, 900), Mathf.Min(_titleLabel.Size.Y + _promptTextLabel.Size.Y + _inputTextBox.Size.Y + _buttonContainer.Size.Y + 20, 900));
        _border.Position = new Vector2(0, 25);
        _border.Size = new Vector2(_marginContainer.Size.X, _marginContainer.Size.Y - 53);

        _titleLabel.Size = new Vector2(Mathf.Min(_titleLabel.Size.X + 600, 900), _titleLabel.Size.Y);

        CustomMinimumSize = _marginContainer.CustomMinimumSize;
    }

    public void Show(string titleText, string promptText, string placeholderText, Color borderColor, Action<string> okCallback, Action cancelCallback)
    {
        _innerTextWithoutBbCode = Regex.Replace(promptText, @"\[(.*?)\]", string.Empty);
        _titleLabel.Text = titleText;
        _promptTextLabel.Text = "";
        _promptTextLabel.BbcodeEnabled = true;
        var innerTextLines = promptText.Split('\n');
        for (int i = 0; i < innerTextLines.Length; i++)
        {
            _promptTextLabel.AppendText("[p]");
            _promptTextLabel.AppendText(innerTextLines[i]);
            if (string.IsNullOrEmpty(innerTextLines[i]))
                _promptTextLabel.AppendText(" ");
        }


        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = borderColor;
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = borderColor;
        _border.AddThemeStyleboxOverride("panel", borderStyleBox);

        _okButton.Text = TranslationServer.Translate("InputBoxAffirmativeButtonText");
        _okButton.Pressed += () =>
        {
            if (GetChildren().Any(c => c.IsPopUp())) return;
            if (string.IsNullOrEmpty(_inputTextBox.Text))
                _inputTextBox.Text = _inputTextBox.PlaceholderText;
            okCallback?.Invoke(_inputTextBox.Text);
            QueueFree();
        };
        _cancelButton.Text = TranslationServer.Translate("CancelButtonText");
        _cancelButton.Pressed += () =>
        {
            if (GetChildren().Any(c => c.IsPopUp())) return;
            cancelCallback?.Invoke();
            QueueFree();
        };

        var normalButtonStyleBox = (StyleBoxFlat)GlobalConstants.ButtonNormalStyle.Duplicate();
        normalButtonStyleBox.BorderColor = borderColor;
        var hoverButtonStyleBox = (StyleBoxFlat)GlobalConstants.ButtonHoverStyle.Duplicate();
        hoverButtonStyleBox.BorderColor = borderColor;

        _okButton.AddThemeStyleboxOverride("normal", normalButtonStyleBox);
        _okButton.AddThemeStyleboxOverride("hover", hoverButtonStyleBox);
        _okButton.AddThemeStyleboxOverride("pressed", normalButtonStyleBox);
        _cancelButton.AddThemeStyleboxOverride("normal", normalButtonStyleBox);
        _cancelButton.AddThemeStyleboxOverride("hover", hoverButtonStyleBox);
        _cancelButton.AddThemeStyleboxOverride("pressed", normalButtonStyleBox);

        _inputTextBox.MaxLength = 15;
        _inputTextBox.PlaceholderText = placeholderText;
        _inputTextBox.GrabFocus();

        UpdateSize();

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            _okButton.GrabFocus();
            _okButton.EmitSignal("pressed");
            _okButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            _cancelButton.GrabFocus();
            _cancelButton.EmitSignal("pressed");
            _cancelButton.ButtonPressed = true;
            AcceptEvent();
        }
    }
}

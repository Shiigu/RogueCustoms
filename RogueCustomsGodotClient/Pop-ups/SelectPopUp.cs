using Godot;

using MathNet.Numerics.Distributions;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

public partial class SelectPopUp : Control
{
    private GlobalState _globalState;
    private Label _titleLabel;
    private RichTextLabel _innerTextLabel;
    private HBoxContainer _buttonContainer;
    private Panel _border;
    private MarginContainer _marginContainer;
    private VBoxContainer _vBoxContainer;
    private string _innerTextWithoutBbCode;
    private VBoxContainer _selectionList;
    private Vector2 _originalLabelSize;
    private Button _selectButton, _cancelButton;
    private InputManager _inputManager;

    private int _selectedIndex = -1;

    private readonly StyleBoxFlat normalItemStyleBox = (StyleBoxFlat)GlobalConstants.NormalItemStyleBox.Duplicate();
    private readonly StyleBoxFlat selectItemStyleBox = (StyleBoxFlat)GlobalConstants.SelectedItemStyleBox.Duplicate();

    private readonly string SelectButtonText = TranslationServer.Translate("SelectButtonText");
    private readonly string CancelButtonText = TranslationServer.Translate("CancelButtonText");

    private SelectionItem[] _choices;

    private readonly static Color BlackColor = new() { R8 = 0, G8 = 0, B8 = 0, A8 = 255 };
    private readonly static Color WhiteColor = new() { R8 = 255, G8 = 255, B8 = 255, A8 = 255 };
    private readonly static Color ItemColor = new() { R8 = 0, G8 = 170, B8 = 170, A8 = 255 };

[Signal]
    public delegate void PopupClosedEventHandler();
    public override void _Ready()
    {
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _innerTextLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/InnerTextLabel");
        _buttonContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer/ButtonContainer");
        _border = GetNode<Panel>("Border");
        _marginContainer = GetNode<MarginContainer>("MarginContainer");
        _vBoxContainer = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        _selectionList = GetNode<VBoxContainer>("MarginContainer/VBoxContainer/SelectionList");
        _selectButton = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/SelectButton");
        _cancelButton = GetNode<Button>("MarginContainer/VBoxContainer/ButtonContainer/CancelButton");
    }

    public void UpdateSize()
    {
        var labelSize = _innerTextWithoutBbCode.GetSizeToFitForDimensions(_innerTextLabel.GetThemeDefaultFont(), 900, 900);
        _innerTextLabel.CustomMinimumSize = new Vector2(labelSize.X, labelSize.Y);

        _selectionList.Size = new Vector2(_vBoxContainer.Size.X, _selectionList.GetChildCount() * 32);

        _marginContainer.Size = new Vector2(
            Mathf.Min(_innerTextLabel.Size.X, 900),
            Mathf.Min(_titleLabel.Size.Y + _innerTextLabel.Size.Y + _selectionList.Size.Y + _buttonContainer.Size.Y - 12, 900));

        _border.Position = new Vector2(0, 25);
        _border.Size = new Vector2(_marginContainer.Size.X, _marginContainer.Size.Y - 56);

        _titleLabel.Size = new Vector2(Mathf.Min(_titleLabel.Size.X + 600, 900), _titleLabel.Size.Y);

        CustomMinimumSize = _marginContainer.CustomMinimumSize;

        foreach (Control child in _selectionList.GetChildren().Cast<Control>())
        {
            child.CustomMinimumSize = new Vector2(_vBoxContainer.Size.X, 16);
        }
    }

    public void Show(string titleText, string innerText, SelectionItem[] choices, Color borderColor)
    {
        _originalLabelSize = _innerTextLabel.Size;
        _innerTextWithoutBbCode = innerText.ToStringWithoutBbcode();
        _titleLabel.Text = titleText;
        _innerTextLabel.BbcodeEnabled = true;
        _innerTextLabel.Text = innerText.ToBbCodeAppropriateString();

        for (int i = 0; i < choices.Length; i++)
        {
            var choice = choices[i];
            var choiceLabel = new ScalableRichTextLabel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ShrinkCenter,
                MinFontSize = 8,
                DefaultFontSize = 12,
                BbcodeEnabled = true,
                CustomMinimumSize = new() { X = _selectionList.Size.X, Y = 16 },
                Size = new() { X = _selectionList.Size.X, Y = 16 }
            };
            choiceLabel.SetText($"[color={WhiteColor.ToHtml()}]{i + 1}[/color][color={ItemColor.ToHtml()}]>[/color] [color={WhiteColor.ToHtml()}]{choice.Name}[/color]");
            choiceLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            _selectionList.AddChild(choiceLabel);
        }

        _choices = choices;

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = borderColor;
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = borderColor;
        _border.AddThemeStyleboxOverride("panel", borderStyleBox);

        UpdateSize();

        _selectButton.Text = SelectButtonText;

        _selectButton.Pressed += () =>
        {
            if (_selectedIndex != -1)
            {
                EmitSignal(nameof(PopupClosed), _choices[_selectedIndex].Id);
                QueueFree();
            }
        };

        _cancelButton.Text = CancelButtonText;

        _cancelButton.Pressed += () =>
        {
            if (_selectedIndex != -1)
            {
                EmitSignal(nameof(PopupClosed), null);
                QueueFree();
            }
        };

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;

        SelectRow(0);
    }

    private void SelectRow(int index)
    {
        if (_selectedIndex != -1)
        {
            var selectedLabel = (ScalableRichTextLabel)_selectionList.GetChildren()[_selectedIndex];
            selectedLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            selectedLabel.SetText($"[color={WhiteColor.ToHtml()}]{_selectedIndex + 1}[/color][color={ItemColor.ToHtml()}]>[/color] [color={WhiteColor.ToHtml()}]{_choices[_selectedIndex].Name}[/color]");
        }
        _selectedIndex = index;
        var newSelectedLabel = (ScalableRichTextLabel)_selectionList.GetChildren()[_selectedIndex];
        newSelectedLabel.AddThemeStyleboxOverride("normal", selectItemStyleBox);
        newSelectedLabel.SetText($"[color={BlackColor.ToHtml()}]{_selectedIndex + 1}[/color][color={ItemColor.ToHtml()}]>[/color] [color={BlackColor.ToHtml()}]{_choices[_selectedIndex].Name}[/color]");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            if (_selectedIndex == 0)
                SelectRow(_selectionList.GetChildCount() - 1);
            else
                SelectRow(_selectedIndex - 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            if (_selectedIndex == _selectionList.GetChildCount() - 1)
                SelectRow(0);
            else
                SelectRow(_selectedIndex + 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_accept"))
        {
            _selectButton.GrabFocus();
            _selectButton.EmitSignal("pressed");
            _selectButton.ButtonPressed = true;
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

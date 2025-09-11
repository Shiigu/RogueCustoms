using Godot;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;

public partial class SelectClass : Control
{
    private GlobalState _globalState;
    private InputManager _inputManager;
    private ExceptionLogger _exceptionLogger;
    private List<CharacterClassDto> _possibleClasses;
    private Label _titleLabel;

    private Label _classNameLabel;
    private Button _leftButton, _rightButton;

    private Label _detailsSectionLabel;
    private RichTextLabel _classDescriptionLabel;

    private Button _selectButton, _cancelButton;

    private Panel _border;

    private int _selectionIndex;
    private const int _scrollStep = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _classNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/ClassNameLabel");
        _leftButton = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/LeftButton");
        _rightButton = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/RightButton");
        _detailsSectionLabel = GetNode<Label>("VBoxContainer2/DetailsSectionLabel");
        _classDescriptionLabel = GetNode<RichTextLabel>("VBoxContainer2/ClassDescriptionLabel");
        _selectButton = GetNode<Button>("VBoxContainer2/ButtonContainer/SelectButton");
        _cancelButton = GetNode<Button>("VBoxContainer2/ButtonContainer/CancelButton");
        _border = GetNode<Panel>("Border");
        try
        {
            _possibleClasses = _globalState.DungeonManager.GetPlayerClassSelection().CharacterClasses;
        }
        catch (Exception ex)
        {
            _exceptionLogger.LogMessage(ex);
        }
    }

    public void Show(Action<string> selectCallBack, Action cancelCallback)
    {
        _titleLabel.Text = TranslationServer.Translate("PlayerClassWindowTitleText");
        _detailsSectionLabel.Text = TranslationServer.Translate("DetailsSubconsoleTitleText");
        _classDescriptionLabel.SizeFlagsVertical = SizeFlags.ExpandFill;
        ShowClassDetails(0);

        _selectButton.Text = TranslationServer.Translate("SelectButtonText");
        _selectButton.Pressed += () =>
        {
            selectCallBack?.Invoke(_possibleClasses[_selectionIndex].ClassId);
        };
        _cancelButton.Text = TranslationServer.Translate("CancelButtonText");
        _cancelButton.Pressed += () =>
        {
            cancelCallback?.Invoke();
        };
        _leftButton.Text = TranslationServer.Translate("LeftButtonText");
        _leftButton.Pressed += () => ShowClassDetails(_selectionIndex - 1);
        _rightButton.Text = TranslationServer.Translate("RightButtonText");
        _rightButton.Pressed += () => ShowClassDetails(_selectionIndex + 1);

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;
    }

    public void ShowClassDetails(int selectionIndex)
    {
        _selectionIndex = selectionIndex;
        _leftButton.Disabled = (selectionIndex == 0);
        _rightButton.Disabled = (selectionIndex == _possibleClasses.Count - 1);
        var currentClass = _possibleClasses[selectionIndex];
        _classNameLabel.Text = currentClass.Name;
        _classDescriptionLabel.Text = "";
        _classDescriptionLabel.BbcodeEnabled = true;
        _classDescriptionLabel.Size = new Vector2I(850, 0);
        _classDescriptionLabel.AppendText($"[center]{currentClass.Name}[/center][p]");
        _classDescriptionLabel.AppendText($"[center]{currentClass.ConsoleRepresentation.ToBbCodeRepresentation()}[/center][p] ");
        _classDescriptionLabel.AppendText($"{currentClass.Description.ToBbCodeAppropriateString()}[p] ");
        _classDescriptionLabel.AppendText($"[center]{TranslationServer.Translate("PlayerClassStatsHeader")}[/center][p] ");
        foreach (var stat in currentClass.InitialStats)
        {
            PrintPlayerStatsInfo(stat);
        }

        _classDescriptionLabel.AppendText($"[p] [p]{currentClass.SightRangeName}: {currentClass.SightRangeStat}[p] ");

        if(currentClass.StartingEquipment.Count != 0)
        {
            _classDescriptionLabel.AppendText($"[p] [p][center]{TranslationServer.Translate("PlayerClassStartingInventoryHeader")}[/center][p] ");

            _classDescriptionLabel.AppendText("[p]");
            foreach (var item in currentClass.StartingEquipment)
            {
                _classDescriptionLabel.AppendText($"{item.ConsoleRepresentation.ToBbCodeRepresentation()} - {item.Name}[p]");
            }
        }

        _classDescriptionLabel.AppendText($"[p] [p][center]{TranslationServer.Translate("PlayerClassStartingInventoryHeader")}[/center][p] ");
        
        if (currentClass.StartingInventory.Count != 0)
        {
            _classDescriptionLabel.AppendText("[p]");
            foreach (var item in currentClass.StartingInventory)
            {
                _classDescriptionLabel.AppendText($"{item.ConsoleRepresentation.ToBbCodeRepresentation()} - {item.Name}[p]");
            }
        }
        else
        {
            _classDescriptionLabel.AppendText($"[p][center]{TranslationServer.Translate("PlayerClassNoStartingInventoryText")}[/center]");
        }
        _classDescriptionLabel.AppendText($"[p] [p]{currentClass.InventorySizeName}: {currentClass.InventorySizeStat}");
        _leftButton.ReleaseFocus();
        _rightButton.ReleaseFocus();
        _classDescriptionLabel.GetVScrollBar().Value = 0;
    }

    private void PrintPlayerStatsInfo(CharacterClassStatDto stat)
    {
        if (stat.IsDecimalStat)
            _classDescriptionLabel.AppendText($"[p] [p]{stat.Name}: {stat.Base:0.#####}");
        else if (stat.IsPercentileStat)
            _classDescriptionLabel.AppendText($"[p] [p]{stat.Name}: {stat.Base}%");
        else
            _classDescriptionLabel.AppendText($"[p] [p]{stat.Name}: {(int)stat.Base}");
        if (stat.HasIncreasePerLevel)
        {
            var increasePerLevelString = TranslationServer.Translate("PlayerClassIncreasePerLevelText").ToString().Format(new
            {
                Increase = $"{stat.IncreasePerLevel:+0.#####;-0.#####;0}"
            });
            _classDescriptionLabel.AppendText($"[p]     {increasePerLevelString}");
        }
    }

    private void PrintPlayerEquipmentInfo(string itemTypeHeader, ItemDetailDto item)
    {
        _classDescriptionLabel.AppendText($"[center]{itemTypeHeader}[/center][p] ");
        _classDescriptionLabel.AppendText($"[p][center]{item.Name}[/center][p]");
        _classDescriptionLabel.AppendText($"[p][center]{item.ConsoleRepresentation.ToBbCodeRepresentation()}[/center][p] ");
        _classDescriptionLabel.AppendText($"[p]{item.Description.ToBbCodeAppropriateString()}[p]");
    }


    public override void _Input(InputEvent @event)
    {
        if (GetParent().GetChildren().Any(c => c.IsPopUp() && c is not SelectClass)) return;
        if (@event.IsActionPressed("ui_left") && !_leftButton.Disabled)
        {
            _leftButton.GrabFocus();
            _leftButton.EmitSignal("pressed");
            _leftButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_right") && !_rightButton.Disabled)
        {
            _rightButton.GrabFocus();
            _rightButton.EmitSignal("pressed");
            _rightButton.ButtonPressed = true;
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
        else if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            _classDescriptionLabel.GetVScrollBar().Value -= _scrollStep;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            _classDescriptionLabel.GetVScrollBar().Value += _scrollStep;
            AcceptEvent();
        }
    }
}

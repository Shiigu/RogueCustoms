using Godot;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class PlayerSelectItem : Control
{
    private GlobalState _globalState;
    private ExceptionLogger _exceptionLogger;
    private InputManager _inputManager;

    private Label _titleLabel;
    private VBoxContainer _selectionList;
    private RichTextLabel _itemDescriptionLabel;
    private Button _dropButton, _equipButton, _swapButton, _useButton, _doButton, _cancelButton;

    private Panel _outerBorder, _verticalBorder, _overlappingVerticalBorder;

    private int _selectedIndex = -1;
    private bool _isReadOnly;

    private InventoryDto _itemListInfo;
    private ActionListDto _actionListInfo;

    private readonly string DoButtonText = TranslationServer.Translate("UseButtonText");
    private readonly string UseButtonText = TranslationServer.Translate("UseButtonText");
    private readonly string EquipButtonText = TranslationServer.Translate("EquipButtonText");
    private readonly string DropButtonText = TranslationServer.Translate("DropButtonText");
    private readonly string SwapButtonText = TranslationServer.Translate("SwapButtonText");
    private readonly string CancelButtonText = TranslationServer.Translate("CancelButtonText");

    private readonly StyleBoxFlat normalItemStyleBox = (StyleBoxFlat)GlobalConstants.NormalItemStyleBox.Duplicate();
    private readonly StyleBoxFlat selectItemStyleBox = (StyleBoxFlat)GlobalConstants.SelectedItemStyleBox.Duplicate();
    private readonly StyleBoxFlat unusableSelectItemStyleBox = (StyleBoxFlat)GlobalConstants.UnusableSelectedItemStyleBox.Duplicate();

    private const int _scrollStep = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _itemDescriptionLabel = GetNode<RichTextLabel>("ItemDescriptionLabel");
        _selectionList = GetNode<VBoxContainer>("ScrollContainer/SelectionList");
        _equipButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/EquipButton");
        _swapButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/SwapButton");
        _dropButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/DropButton");
        _useButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/UseButton");
        _doButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/DoButton");
        _cancelButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/CancelButton");
        _outerBorder = GetNode<Panel>("OuterBorder");
        _verticalBorder = GetNode<Panel>("VerticalBorder");
        _overlappingVerticalBorder = GetNode<Panel>("OverlappingVerticalBorder");
    }

    public void Show(InventoryDto itemInfo, ActionListDto actionInfo, Vector2I? targetCoords = null, Action onCloseCallback = null)
    {
        if (_globalState.DungeonInfo == null || _globalState.DungeonInfo.PlayerEntity == null) return;
        _isReadOnly = _globalState.PlayerControlMode == ControlMode.MustSkipTurn || _globalState.PlayerControlMode == ControlMode.None;
        if (actionInfo != null && actionInfo.Actions.Any())
        {
            _actionListInfo = actionInfo;
            ShowActionScreen(targetCoords);
        }
        else if (itemInfo != null && itemInfo.InventoryItems.Any())
        {
            _itemListInfo = itemInfo;
            ShowInventoryScreen();
        }
        else
        {
            onCloseCallback?.Invoke();
            QueueFree();
            return;
        }

        _equipButton.Text = EquipButtonText;

        _equipButton.Pressed += () =>
        {
            try
            {
                onCloseCallback?.Invoke();
                QueueFree();
                _globalState.MustUpdateGameScreen = true;
                _globalState.DungeonManager.PlayerUseItemFromInventory(_itemListInfo.InventoryItems[_selectedIndex].ItemId);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
        };

        _swapButton.Text = SwapButtonText;

        _swapButton.Pressed += () =>
        {
            try
            {
                onCloseCallback?.Invoke();
                QueueFree();
                _globalState.MustUpdateGameScreen = true;
                _globalState.DungeonManager.PlayerSwapFloorItemWithInventoryItem(_itemListInfo.InventoryItems[_selectedIndex].ItemId);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
        };

        _dropButton.Text = DropButtonText;

        _dropButton.Pressed += () =>
        {
            try
            {
                onCloseCallback?.Invoke();
                QueueFree();
                _globalState.MustUpdateGameScreen = true;
                _globalState.DungeonManager.PlayerDropItemFromInventory(_itemListInfo.InventoryItems[_selectedIndex].ItemId);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
        };

        _useButton.Text = UseButtonText;

        _useButton.Pressed += () =>
        {
            try
            {
                onCloseCallback?.Invoke();
                QueueFree();
                _globalState.MustUpdateGameScreen = true;
                _globalState.DungeonManager.PlayerUseItemFromInventory(_itemListInfo.InventoryItems[_selectedIndex].ItemId);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
        };

        _doButton.Text = DoButtonText;


        _doButton.Pressed += () => DoButton_Pressed(targetCoords, onCloseCallback).ContinueWith(t =>
        {
            if (t.Exception != null)
            {
                _exceptionLogger.LogMessage(t.Exception);
            }
        });

        _cancelButton.Text = CancelButtonText;

        _cancelButton.Pressed += () =>
        {
            onCloseCallback?.Invoke();
            QueueFree();
        };

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _outerBorder.Size) / 2;
    }

    private Task DoButton_Pressed(Vector2I? targetCoords, Action onCloseCallback)
    {
        var selectedAction = _actionListInfo.Actions.ElementAtOrDefault(_selectedIndex);

        var attackInput = new AttackInput
        {
            SelectionId = selectedAction.SelectionId,
            X = targetCoords.Value.X,
            Y = targetCoords.Value.Y,
            SourceType = selectedAction.SourceType
        };
        onCloseCallback?.Invoke();
        QueueFree();
        _globalState.MustUpdateGameScreen = true;
        return _globalState.DungeonManager.PlayerAttackTargetWith(attackInput);
    }

    private void ShowInventoryScreen()
    {
        _doButton.Visible = false;

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = new Color { R8 = 255, G8 = 255, B8 = 0, A = 1 };
        _outerBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _verticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _overlappingVerticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = new Color { R8 = 255, G8 = 255, B8 = 0, A = 1 };
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);
        _titleLabel.Text = TranslationServer.Translate("InventoryWindowTitleText");

        foreach (var item in _itemListInfo.InventoryItems)
        {
            var itemLabel = new ScalableLabel { HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                                SizeFlagsVertical = SizeFlags.ShrinkCenter,
                                MinFontSize = 8,
                                DefaultFontSize = 16,
                                Size = new() { X = _selectionList.Size.X, Y = 16 }
            };
            if (item.IsEquipped)
                itemLabel.SetText($"{TranslationServer.Translate("EquippedItemNamePrefix")} {item.Name}");
            else if (item.IsInFloor)
                itemLabel.SetText($"{TranslationServer.Translate("FloorItemNamePrefix")} {item.Name}");
            else
                itemLabel.SetText(item.Name);
            itemLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            if(item.CanBeUsed)
                itemLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            else
                itemLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            _selectionList.AddChild(itemLabel);
        }

        SelectRow(0);
    }

    private void ShowActionScreen(Vector2I? targetCoords = null)
    {
        if (targetCoords == null) return;
        _dropButton.Visible = false;
        _equipButton.Visible = false;
        _swapButton.Visible = false;
        _useButton.Visible = false;
        _doButton.Visible = true;

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = new Color { R8 = 255, G8 = 0, B8 = 0, A = 1 };
        _outerBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _verticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _overlappingVerticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = new Color { R8 = 139, G8 = 0, B8 = 0, A = 1 };
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);
        _titleLabel.Text = TranslationServer.Translate("ActionWindowTitleText").ToString().Format(new { TargetName = _actionListInfo.TargetName });

        foreach (var action in _actionListInfo.Actions)
        {
            var actionLabel = new ScalableLabel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ShrinkCenter,
                MinFontSize = 8,
                DefaultFontSize = 16,
                Size = new() { X = _selectionList.Size.X, Y = 16}
            };
            actionLabel.SetText(action.Name);
            actionLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            if(action.CanBeUsed)
                actionLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            else
                actionLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            _selectionList.AddChild(actionLabel);
        }

        SelectRow(0);
    }

    private void SelectRow(int index)
    {
        var selectedItem = _itemListInfo != null && _selectedIndex != -1
            ? _itemListInfo.InventoryItems[_selectedIndex]
            : null;
        var selectedAction = _actionListInfo != null && _selectedIndex != -1
            ? _actionListInfo.Actions[_selectedIndex]
            : null;
        if (_selectedIndex != -1)
        {
            var selectedLabel = (Label) _selectionList.GetChildren()[_selectedIndex];
            selectedLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            if((selectedItem != null && !selectedItem.CanBeUsed)
                || (selectedAction != null && !selectedAction.CanBeUsed))
                selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            else
                selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }
        _selectedIndex = index;
        var newSelectedLabel = (Label)_selectionList.GetChildren()[_selectedIndex];
        selectedItem = _itemListInfo != null
            ? _itemListInfo.InventoryItems[_selectedIndex]
            : null;
        selectedAction = _actionListInfo != null
            ? _actionListInfo.Actions[_selectedIndex]
            : null;
        if ((selectedItem != null && !selectedItem.CanBeUsed)
            || (selectedAction != null && !selectedAction.CanBeUsed))
            newSelectedLabel.AddThemeStyleboxOverride("normal", unusableSelectItemStyleBox);
        else
            newSelectedLabel.AddThemeStyleboxOverride("normal", selectItemStyleBox);
        newSelectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });

        _itemDescriptionLabel.Text = "";

        if(selectedItem != null)
        {
            _itemDescriptionLabel.AppendText($"{selectedItem.Name}[p] [p]{selectedItem.ConsoleRepresentation.ToBbCodeRepresentation()}[p] [p]");
            _itemDescriptionLabel.AppendText(selectedItem.Description.ToBbCodeAppropriateString());

            if(!string.IsNullOrWhiteSpace(selectedItem.Power) && !selectedItem.Power.Trim().Equals("0") && !string.IsNullOrWhiteSpace(selectedItem.PowerName))
            {
                _itemDescriptionLabel.AppendText($"[p] [p]{selectedItem.PowerName}: {selectedItem.Power}");
            }

            if (selectedItem.StatModifications.Any())
            {
                var situationText = selectedItem.IsEquippable ? TranslationServer.Translate("InventoryWindowOnEquipText") : TranslationServer.Translate("InventoryWindowOnCarryText");
                _itemDescriptionLabel.AppendText($"[p] [p][color=#8B83D9]{TranslationServer.Translate("InventoryWindowStatModifiersHeaderText").ToString().Format(new { Situation = situationText })}[/color]");
                foreach (var modifiedStat in selectedItem.StatModifications)
                {
                    var amountString = modifiedStat.Amount.ToString("+0.####;-0.####") + (modifiedStat.IsPercentage ? "%" : "");
                    _itemDescriptionLabel.AppendText($"[p][color=#8B83D9]   {TranslationServer.Translate("InventoryWindowStatModifierText").ToString().Format(new { Number = amountString, StatName = modifiedStat.Name })}[/color]");
                }
            }

            if (selectedItem.OnAttackActions.Any())
            {
                _itemDescriptionLabel.AppendText($"[p] [p][color=#8B83D9]{TranslationServer.Translate("InventoryWindowActionsHeaderText")}[/color]");
                foreach (var action in selectedItem.OnAttackActions)
                {
                    _itemDescriptionLabel.AppendText($"[p][color=#8B83D9]   {action}[/color]");
                }
            }

            if (selectedItem.IsInFloor)
            {
                _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("FloorItemDescriptionText")}");
            }
            else if (selectedItem.IsEquipped)
            {
                _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("EquippedItemDescriptionText")}");
                if (_itemListInfo.TileIsOccupied)
                    _itemDescriptionLabel.AppendText($"[p]{TranslationServer.Translate("OccupiedTileDescriptionText")}");
            }
        }
        else
        {
            _itemDescriptionLabel.AppendText(selectedAction.Description.ToBbCodeAppropriateString());
        }
        if(selectedItem != null)
        {
            _dropButton.Visible = !_itemListInfo.TileIsOccupied && selectedItem.CanBeDropped;
            _dropButton.Disabled = _isReadOnly;
            _swapButton.Visible = _itemListInfo.TileIsOccupied && !selectedItem.IsInFloor && selectedItem.CanBeDropped;
            _swapButton.Disabled = _isReadOnly;
            _useButton.Visible = !selectedItem.IsEquippable;
            _useButton.Disabled = _isReadOnly || !selectedItem.CanBeUsed;
            _equipButton.Visible = selectedItem.IsEquippable;
            _equipButton.Disabled = _isReadOnly || selectedItem.IsEquipped;
        }
        else
        {
            _doButton.Disabled = _isReadOnly || !selectedAction.CanBeUsed;
        }
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
            Button buttonToUse = null;
            if(_doButton.Visible && !_doButton.Disabled)
                buttonToUse = _doButton;
            else if (_useButton.Visible && !_useButton.Disabled)
                buttonToUse = _useButton;
            else if (_equipButton.Visible && !_equipButton.Disabled)
                buttonToUse = _equipButton;
            if(buttonToUse == null)
            {
                AcceptEvent();
                return;
            }
            buttonToUse.GrabFocus();
            buttonToUse.EmitSignal("pressed");
            buttonToUse.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_swap"))
        {
            if (!_swapButton.Visible || _swapButton.Disabled)
            {
                AcceptEvent();
                return;
            }
            _swapButton.GrabFocus();
            _swapButton.EmitSignal("pressed");
            _swapButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_drop"))
        {
            if (!_dropButton.Visible || _dropButton.Disabled)
            {
                AcceptEvent();
                return;
            }
            _dropButton.GrabFocus();
            _dropButton.EmitSignal("pressed");
            _dropButton.ButtonPressed = true;
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
    }
}

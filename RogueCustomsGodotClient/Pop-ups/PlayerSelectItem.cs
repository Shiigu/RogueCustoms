using Godot;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

public partial class PlayerSelectItem : Control
{
    private GlobalState _globalState;
    private ExceptionLogger _exceptionLogger;
    private InputManager _inputManager;

    private Label _titleLabel;
    private VBoxContainer _selectionList;
    private RichTextLabel _itemDescriptionLabel;
    private Button _dropButton, _equipButton, _swapButton, _useButton, _doButton, _cancelButton, _selectButton, _buyButton, _sellButton, _abandonButton;

    private Panel _outerBorder, _verticalBorder, _overlappingVerticalBorder;

    private int _selectedIndex = -1;
    private bool _isReadOnly;

    private InventoryDto _itemListInfo;
    private ActionListDto _actionListInfo;
    private List<QuestDto> _questListInfo;
    private SelectionMode _selectionMode;
    private string Title;

    [Signal]
    public delegate void PopupClosedEventHandler();

    private readonly string DoButtonText = TranslationServer.Translate("UseButtonText");
    private readonly string UseButtonText = TranslationServer.Translate("UseButtonText");
    private readonly string EquipButtonText = TranslationServer.Translate("EquipButtonText");
    private readonly string DropButtonText = TranslationServer.Translate("DropButtonText");
    private readonly string SwapButtonText = TranslationServer.Translate("SwapButtonText");
    private readonly string CancelButtonText = TranslationServer.Translate("CancelButtonText");
    private readonly string SelectButtonText = TranslationServer.Translate("SelectButtonText");
    private readonly string BuyButtonText = TranslationServer.Translate("BuyButtonText");
    private readonly string SellButtonText = TranslationServer.Translate("SellButtonText");
    private readonly string AbandonButtonText = TranslationServer.Translate("AbandonButtonText");

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
        _selectButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/SelectButton");
        _cancelButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/CancelButton");
        _buyButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/BuyButton");
        _sellButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/SellButton");
        _abandonButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/AbandonButton");
        _outerBorder = GetNode<Panel>("OuterBorder");
        _verticalBorder = GetNode<Panel>("VerticalBorder");
        _overlappingVerticalBorder = GetNode<Panel>("OverlappingVerticalBorder");
    }

    public void Show(InventoryDto itemInfo, SelectionMode selectionMode, bool showCancelButton, Action onCloseCallback = null, string title = null)
    {
        if (_globalState.DungeonInfo == null || _globalState.DungeonInfo.PlayerEntity == null)
        {
            onCloseCallback?.Invoke();
            QueueFree();
            return;
        }

        if (itemInfo != null && itemInfo.InventoryItems.Any())
        {
            _itemListInfo = itemInfo;

            _selectionMode = selectionMode;
            _isReadOnly = _globalState.PlayerControlMode == ControlMode.MustSkipTurn || _globalState.PlayerControlMode == ControlMode.None;
            Title = title ?? TranslationServer.Translate("InventoryWindowTitleText");

            ShowInventoryScreen();
        }
        else
        {
            onCloseCallback?.Invoke();
            QueueFree();
            return;
        }

        if(_selectionMode == SelectionMode.Inventory)
        {
            _equipButton.Text = EquipButtonText;

            _equipButton.Pressed += () =>
            {
                try
                {
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
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
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
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
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
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
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
                    _globalState.DungeonManager.PlayerUseItemFromInventory(_itemListInfo.InventoryItems[_selectedIndex].ItemId);
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
            };
        }
        else if (_selectionMode == SelectionMode.SelectItem)
        {
            _selectButton.Text = SelectButtonText;

            _selectButton.Pressed += () =>
            {
                try
                {
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
                    EmitSignal(nameof(PopupClosed), _itemListInfo.InventoryItems[_selectedIndex].ItemId, _itemListInfo.InventoryItems[_selectedIndex].ClassId);
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
            };
        }
        else if (_selectionMode == SelectionMode.Buy)
        {
            _buyButton.Text = BuyButtonText;

            _buyButton.Pressed += () =>
            {
                try
                {
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
                    EmitSignal(nameof(PopupClosed), _itemListInfo.InventoryItems[_selectedIndex].ItemId);
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
            };
        }
        else if (_selectionMode == SelectionMode.Sell)
        {
            _sellButton.Text = SellButtonText;

            _sellButton.Pressed += () =>
            {
                try
                {
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    onCloseCallback?.Invoke();
                    QueueFree();
                    _globalState.MustUpdateGameScreen = true;
                    _globalState.DungeonManager.RefreshDisplay(true);
                    EmitSignal(nameof(PopupClosed), _itemListInfo.InventoryItems[_selectedIndex].ItemId);
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
            };
        }

        if (showCancelButton)
        {
            _cancelButton.Text = CancelButtonText;

            _cancelButton.Pressed += () =>
            {
                if (GetChildren().Any(c => c.IsPopUp())) return;
                _selectedIndex = -1;
                EmitSignal(nameof(PopupClosed), null);
                onCloseCallback?.Invoke();
                QueueFree();
            };
        }
        else
        {
            _cancelButton.Visible = false;
            _cancelButton.Disabled = true;
        }

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _outerBorder.Size) / 2;
    }

    public void Show(ActionListDto actionInfo, SelectionMode selectionMode, bool showCancelButton, Vector2I? targetCoords = null, Action onCloseCallback = null, string title = null)
    {
        if (_globalState.DungeonInfo == null || _globalState.DungeonInfo.PlayerEntity == null)
        {
            onCloseCallback?.Invoke();
            QueueFree();
            return;
        }

        if (actionInfo != null && actionInfo.Actions.Any())
        {
            if(_globalState.Options.SortActionMode == SortActionMode.UsableActionsFirst)
            {
                actionInfo.Actions.Sort((a1, a2) =>
                {
                    if (a1.CanBeUsed && !a2.CanBeUsed)
                        return -1;
                    else if (!a1.CanBeUsed && a2.CanBeUsed)
                        return 1;
                    else
                        return 0;
                });
            }

            _actionListInfo = actionInfo;

            Title = title ?? TranslationServer.Translate("ActionWindowTitleText").ToString().Format(new { TargetName = _actionListInfo.TargetName });
            _selectionMode = selectionMode;
            _isReadOnly = _globalState.PlayerControlMode == ControlMode.MustSkipTurn || _globalState.PlayerControlMode == ControlMode.None;

            ShowActionScreen(targetCoords);
        }
        else
        {
            onCloseCallback?.Invoke();
            QueueFree();
            return;
        }

        if(_selectionMode == SelectionMode.Interact)
        {
            _doButton.Text = DoButtonText;

            _doButton.Pressed += () => DoButton_Pressed(targetCoords, onCloseCallback).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    _exceptionLogger.LogMessage(t.Exception);
                }
            });
        }
        else if (_selectionMode == SelectionMode.SelectAction)
        {
            _selectButton.Text = SelectButtonText;

            _selectButton.Pressed += () =>
            {
                try
                {
                    if (GetChildren().Any(c => c.IsPopUp())) return;
                    EmitSignal(nameof(PopupClosed), _actionListInfo.Actions[_selectedIndex].SelectionId);
                    onCloseCallback?.Invoke();
                    QueueFree();
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
            };
        }

        if (showCancelButton)
        {
            _cancelButton.Text = CancelButtonText;

            _cancelButton.Pressed += () =>
            {
                if (GetChildren().Any(c => c.IsPopUp())) return;
                _selectedIndex = -1;
                EmitSignal(nameof(PopupClosed), null);
                onCloseCallback?.Invoke();
                QueueFree();
            };
        }
        else
        {
            _cancelButton.Visible = false;
            _cancelButton.Disabled =  true;
        }

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _outerBorder.Size) / 2;
    }

    public void Show(List<QuestDto> questList, Action onCloseCallback = null)
    {
        if (_globalState.DungeonInfo == null || _globalState.DungeonInfo.PlayerEntity == null || questList.Count == 0)
        {
            onCloseCallback?.Invoke();
            QueueFree();
            return;
        }

        _selectionMode = SelectionMode.Quest;

        _abandonButton.Text = AbandonButtonText;

        _abandonButton.Pressed += () => _ = AbandonButton_Pressed(onCloseCallback);

        _cancelButton.Text = CancelButtonText;

        _cancelButton.Pressed += () =>
        {
            if (GetChildren().Any(c => c.IsPopUp())) return;
            _selectedIndex = -1;
            EmitSignal(nameof(PopupClosed), null);
            onCloseCallback?.Invoke();
            QueueFree();
        };

        _questListInfo = questList;

        Title = TranslationServer.Translate("JournalWindowTitleText");
        ShowJournalScreen();

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _outerBorder.Size) / 2;
    }

    private async Task AbandonButton_Pressed(Action onCloseCallback = null)
    {
        try
        {
            if (GetChildren().Any(c => c.IsPopUp())) return;
            if (_selectedIndex == -1) return;

            await this.CreateStandardPopup(TranslationServer.Translate("AbandonQuestHeaderText"),
                                        TranslationServer.Translate("AbandonQuestPromptText"),
                                        new PopUpButton[]
                                        {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () => CallAbandonQuest(onCloseCallback), ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                        }, new Color() { R8 = 255, G8 = 255, B8 = 0, A = 1 });
        }
        catch (Exception ex)
        {
            _exceptionLogger.LogMessage(ex);
        }
    }

    private void CallAbandonQuest(Action onCloseCallback = null)
    {
        var selectedQuest = _questListInfo[_selectedIndex];

        onCloseCallback?.Invoke();
        QueueFree();
        _globalState.MustUpdateGameScreen = true;
        _globalState.DungeonManager.RefreshDisplay(true);
        _globalState.DungeonManager.PlayerAbandonQuest(selectedQuest.Id);
    }

    private Task DoButton_Pressed(Vector2I? targetCoords, Action onCloseCallback)
    {
        if (GetChildren().Any(c => c.IsPopUp())) return Task.CompletedTask;
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
        _globalState.DungeonManager.RefreshDisplay(true);
        return _globalState.DungeonManager.PlayerAttackTargetWith(attackInput);
    }

    private void ShowInventoryScreen()
    {
        if (_selectionMode == SelectionMode.Inventory)
        {
            _dropButton.Visible = true;
            _equipButton.Visible = true;
            _swapButton.Visible = true;
            _useButton.Visible = true;
            _doButton.Visible = false;
            _selectButton.Visible = false;
            _sellButton.Visible = false;
            _buyButton.Visible = false;
            _abandonButton.Visible = false;
        }
        else if (_selectionMode == SelectionMode.SelectItem)
        {
            _dropButton.Visible = false;
            _equipButton.Visible = false;
            _swapButton.Visible = false;
            _useButton.Visible = false;
            _doButton.Visible = false;
            _selectButton.Visible = true;
            _sellButton.Visible = false;
            _buyButton.Visible = false;
            _abandonButton.Visible = false;
        }
        else if (_selectionMode == SelectionMode.Sell || _selectionMode == SelectionMode.Buy)
        {
            _dropButton.Visible = false;
            _equipButton.Visible = false;
            _swapButton.Visible = false;
            _useButton.Visible = false;
            _doButton.Visible = false;
            _selectButton.Visible = false;
            _abandonButton.Visible = false;
            _sellButton.Visible = _selectionMode == SelectionMode.Sell;
            _buyButton.Visible = _selectionMode == SelectionMode.Buy;
        }

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = new Color { R8 = 255, G8 = 255, B8 = 0, A = 1 };
        _outerBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _verticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _overlappingVerticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = new Color { R8 = 255, G8 = 255, B8 = 0, A = 1 };
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);
        _titleLabel.Text = Title;

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
            _selectionList.AddChild(itemLabel);
            itemLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            if(item.CanBeUsed || _selectionMode == SelectionMode.SelectItem)
                itemLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            else
                itemLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            if (item.IsEquipped)
                itemLabel.SetText($"{TranslationServer.Translate("EquippedItemNamePrefix")} {item.Name}");
            else if (item.IsInFloor)
                itemLabel.SetText($"{TranslationServer.Translate("FloorItemNamePrefix")} {item.Name}");
            else
                itemLabel.SetText(item.Name);
        }

        SelectInventoryRow(0);
    }

    private void SelectInventoryRow(int index)
    {
        var selectedItem = _selectedIndex != -1
            ? _itemListInfo.InventoryItems[_selectedIndex]
            : null;
        if (_selectedIndex != -1)
        {
            var selectedLabel = (ScalableLabel)_selectionList.GetChildren()[_selectedIndex];
            selectedLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            if (selectedItem?.CanBeUsed == false && _selectionMode != SelectionMode.SelectItem)
                selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            else
                selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }
        _selectedIndex = index;
        var newSelectedLabel = (ScalableLabel)_selectionList.GetChildren()[_selectedIndex];
        selectedItem = _itemListInfo.InventoryItems[_selectedIndex];

        if (selectedItem?.CanBeUsed == false && _selectionMode != SelectionMode.SelectItem)
            newSelectedLabel.AddThemeStyleboxOverride("normal", unusableSelectItemStyleBox);
        else
            newSelectedLabel.AddThemeStyleboxOverride("normal", selectItemStyleBox);
        newSelectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });

        _itemDescriptionLabel.Text = "";

        if (selectedItem != null)
        {
            _itemDescriptionLabel.AppendText($"{selectedItem.Name.ToColoredString(selectedItem.QualityColor)}[p] [p]{selectedItem.ConsoleRepresentation.ToBbCodeRepresentation()}[p] [p]");

            if (!string.IsNullOrWhiteSpace(selectedItem.QualityLevel))
            {
                var qualityDescription = selectedItem.QualityLevel.Replace("{basename}", selectedItem.ItemType, StringComparison.InvariantCultureIgnoreCase);

                _itemDescriptionLabel.AppendText($"[p]{qualityDescription.ToColoredString(selectedItem.QualityColor)}[p] [p]");
            }

            if (selectedItem.ItemLevel > 0)
                _itemDescriptionLabel.AppendText($"[p]{TranslationServer.Translate("InventoryWindowItemLevelText").ToString().Format(new { Level = selectedItem.ItemLevel.ToString() })}");

            var requiredLevelColor = selectedItem.RequiredPlayerLevel > _globalState.DungeonInfo.PlayerEntity.Level
                ? "#FF0000"
                : "#FFFFFF";

            if (selectedItem.RequiredPlayerLevel > 0)
            {
                _itemDescriptionLabel.AppendText($"[p][color={requiredLevelColor}]{TranslationServer.Translate("InventoryWindowRequiredPlayerLevelText").ToString().Format(new { Level = selectedItem.RequiredPlayerLevel.ToString() })}[/color][p] ");
            }

            _itemDescriptionLabel.AppendText($"[p]{selectedItem.Description.ToBbCodeAppropriateString()}");

            if (!string.IsNullOrWhiteSpace(selectedItem.Power) && !selectedItem.Power.Trim().Equals("0") && !string.IsNullOrWhiteSpace(selectedItem.PowerName))
            {
                _itemDescriptionLabel.AppendText($"[p] [p]{selectedItem.PowerName}: {selectedItem.Power}");
            }

            if (selectedItem.SlotsItOccupies.Any())
            {
                var slotsList = string.Join(", ", selectedItem.SlotsItOccupies);
                _itemDescriptionLabel.AppendText($"[p] [p][color=#8B83D9]{TranslationServer.Translate("InventoryWindowOccupiesSlotsText").ToString().Format(new { SlotsList = slotsList })}[/color]");
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

            if (selectedItem.ExtraDamages.Any())
            {
                foreach (var extraDamage in selectedItem.ExtraDamages)
                {
                    _itemDescriptionLabel.AppendText($"[p] [p][color=#8B83D9]{TranslationServer.Translate("InventoryWindowExtraDamageText").ToString().Format(new { Damage = extraDamage.DamageString, Element = extraDamage.Element })}[/color]");
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

            if (selectedItem.IsEquippable && !selectedItem.CanBeEquipped)
            {
                _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("InventoryWindowCannotEquipText")}");
            }

            if (selectedItem.Value > 0)
            {
                if (_selectionMode != SelectionMode.Buy)
                {
                    _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("InventoryWindowSaleValueText").ToString().Format(new { Symbol = _itemListInfo.CurrencyConsoleRepresentation.ToBbCodeRepresentation(), Amount = selectedItem.Value.ToString() })}");
                }
                else
                {
                    _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("InventoryWindowBuyValueText").ToString().Format(new { Symbol = _itemListInfo.CurrencyConsoleRepresentation.ToBbCodeRepresentation(), Amount = selectedItem.Value.ToString() })}");
                    if (!selectedItem.CanBeUsed)
                    {
                        _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("InventoryWindowCannotAffordText").ToString().Format(new { Symbol = _itemListInfo.CurrencyConsoleRepresentation.ToBbCodeRepresentation() })}");
                    }
                }
            }
            else
            {
                _itemDescriptionLabel.AppendText($"[p] [p]{TranslationServer.Translate("InventoryWindowCannotBeSoldText").ToString().Format(new { Symbol = _itemListInfo.CurrencyConsoleRepresentation.ToBbCodeRepresentation() })}");
            }
        }

        if (_selectionMode == SelectionMode.Inventory)
        {
            _dropButton.Visible = !_itemListInfo.TileIsOccupied && selectedItem.CanBeDropped;
            _dropButton.Disabled = _isReadOnly;
            _swapButton.Visible = _itemListInfo.TileIsOccupied && !selectedItem.IsInFloor && selectedItem.CanBeDropped;
            _swapButton.Disabled = _isReadOnly;
            _useButton.Visible = !selectedItem.IsEquippable;
            _useButton.Disabled = _isReadOnly || !selectedItem.CanBeUsed;
            _equipButton.Visible = selectedItem.IsEquippable;
            _equipButton.Disabled = _isReadOnly || selectedItem.IsEquipped || !selectedItem.CanBeEquipped;
            _selectButton.Visible = false;
            _selectButton.Disabled = true;
            _abandonButton.Visible = false;
        }
        else if (_selectionMode == SelectionMode.SelectItem)
        {
            _selectButton.Visible = true;
            _selectButton.Disabled = false;
            _abandonButton.Visible = false;
        }
        else if (_selectionMode == SelectionMode.Sell || _selectionMode == SelectionMode.Buy)
        {
            _dropButton.Visible = false;
            _equipButton.Visible = false;
            _swapButton.Visible = false;
            _useButton.Visible = false;
            _doButton.Visible = false;
            _selectButton.Visible = false;
            _abandonButton.Visible = false;
            _sellButton.Visible = _selectionMode == SelectionMode.Sell;
            _sellButton.Disabled = !selectedItem.CanBeUsed;
            _buyButton.Visible = _selectionMode == SelectionMode.Buy;
            _buyButton.Disabled = !selectedItem.CanBeUsed;
        }
    }

    private void ShowActionScreen(Vector2I? targetCoords = null)
    {
        if (targetCoords == null) return;

        if (_selectionMode == SelectionMode.Interact)
        {
            _dropButton.Visible = false;
            _equipButton.Visible = false;
            _swapButton.Visible = false;
            _useButton.Visible = false;
            _doButton.Visible = true;
            _selectButton.Visible = false;
            _abandonButton.Visible = false;
        }
        else if (_selectionMode == SelectionMode.SelectAction)
        {
            _dropButton.Visible = false;
            _equipButton.Visible = false;
            _swapButton.Visible = false;
            _useButton.Visible = false;
            _doButton.Visible = false;
            _selectButton.Visible = true;
            _abandonButton.Visible = false;
        }

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = new Color { R8 = 255, G8 = 0, B8 = 0, A = 1 };
        _outerBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _verticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _overlappingVerticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = new Color { R8 = 139, G8 = 0, B8 = 0, A = 1 };
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);
        _titleLabel.Text = Title;

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
            if(action.CanBeUsed || _selectionMode == SelectionMode.SelectAction)
                actionLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            else
                actionLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            _selectionList.AddChild(actionLabel);
        }

        var selectionIndex = 0;

        if(_globalState.Options.SortActionMode == SortActionMode.CursorOnFirstUsableAction)
        {
            selectionIndex = Math.Max(0, _actionListInfo.Actions.FindIndex(a => a.CanBeUsed));
        }

        SelectActionRow(selectionIndex);
    }

    private void SelectActionRow(int index)
    {
        var selectedAction = _selectedIndex != -1
            ? _actionListInfo.Actions[_selectedIndex]
            : null;
        if (_selectedIndex != -1)
        {
            var selectedLabel = (ScalableLabel) _selectionList.GetChildren()[_selectedIndex];
            selectedLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            if(selectedAction?.CanBeUsed == false && _selectionMode != SelectionMode.SelectAction)
                selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 64, G8 = 64, B8 = 64, A = 1 });
            else
                selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }
        _selectedIndex = index;
        var newSelectedLabel = (ScalableLabel)_selectionList.GetChildren()[_selectedIndex];
        selectedAction = _actionListInfo.Actions[_selectedIndex];
        if (selectedAction?.CanBeUsed == false && _selectionMode != SelectionMode.SelectAction)
            newSelectedLabel.AddThemeStyleboxOverride("normal", unusableSelectItemStyleBox);
        else
            newSelectedLabel.AddThemeStyleboxOverride("normal", selectItemStyleBox);
        newSelectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });

        _itemDescriptionLabel.Text = selectedAction.Description.ToBbCodeAppropriateString();

        if (_selectionMode == SelectionMode.Interact)
        {
            _doButton.Disabled = _isReadOnly || !selectedAction.CanBeUsed;
            _selectButton.Visible = false;
            _selectButton.Disabled = true;
        }
        else if (_selectionMode == SelectionMode.SelectAction)
        {
            _selectButton.Visible = true;
            _selectButton.Disabled = false;
        }
    }

    private void ShowJournalScreen()
    {
        _dropButton.Visible = false;
        _equipButton.Visible = false;
        _swapButton.Visible = false;
        _useButton.Visible = false;
        _doButton.Visible = false;
        _selectButton.Visible = false;
        _abandonButton.Visible = true;

        var borderStyleBox = (StyleBoxFlat)GlobalConstants.PopUpBorderStyle.Duplicate();
        borderStyleBox.BorderColor = new Color { R8 = 0, G8 = 191, B8 = 255, A = 1 };
        _outerBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _verticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);
        _overlappingVerticalBorder.AddThemeStyleboxOverride("panel", borderStyleBox);

        var titleStyleBox = (StyleBoxFlat)GlobalConstants.PopUpTitleStyle.Duplicate();
        titleStyleBox.BgColor = new Color { R8 = 0, G8 = 191, B8 = 255, A = 1 };
        _titleLabel.AddThemeStyleboxOverride("normal", titleStyleBox);
        _titleLabel.Text = TranslationServer.Translate("JournalWindowTitleText");

        foreach (var quest in _questListInfo)
        {
            var questLabel = new ScalableLabel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ShrinkCenter,
                MinFontSize = 8,
                DefaultFontSize = 16,
                Size = new() { X = _selectionList.Size.X, Y = 16 }
            };
            questLabel.SetText(quest.Name);
            questLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            questLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            _selectionList.AddChild(questLabel);
        }

        SelectQuestRow(0);
    }

    private void SelectQuestRow(int index)
    {
        var selectedQuest = _selectedIndex != -1
            ? _questListInfo[_selectedIndex]
            : null;
        if (_selectedIndex != -1)
        {
            var selectedLabel = (ScalableLabel)_selectionList.GetChildren()[_selectedIndex];
            selectedLabel.AddThemeStyleboxOverride("normal", normalItemStyleBox);
            selectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }
        _selectedIndex = index;
        var newSelectedLabel = (ScalableLabel)_selectionList.GetChildren()[_selectedIndex];
        selectedQuest = _questListInfo[_selectedIndex];
        newSelectedLabel.AddThemeStyleboxOverride("normal", selectItemStyleBox);
        newSelectedLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });

        _itemDescriptionLabel.Text = "";

        if (selectedQuest != null)
        {
            _itemDescriptionLabel.AppendText($"{selectedQuest.Description}[p] [p]");

            _itemDescriptionLabel.AppendText($"{selectedQuest.CompletionTypeMessage.ToColoredString(new(System.Drawing.Color.DeepSkyBlue))}[p] [p]");

            foreach (var condition in selectedQuest.Conditions)
            {
                var conditionColor = condition.IsFulfilled
                    ? "#00FF00"
                    : "#00BFFF";

                var conditionText = $"- {condition.Description} ({condition.CurrentValue}/{condition.TargetValue})";

                _itemDescriptionLabel.AppendText($"[p][color={conditionColor}]{conditionText}[/color]");
            }
        }
    }

    private void SelectRow(int index)
    {
        if (_selectionMode == SelectionMode.Inventory || _selectionMode == SelectionMode.SelectItem || _selectionMode == SelectionMode.Buy || _selectionMode == SelectionMode.Sell)
        {
            if (index < 0 || index >= _itemListInfo.InventoryItems.Count)
                return;
            SelectInventoryRow(index);
        }
        else if (_selectionMode == SelectionMode.Interact || _selectionMode == SelectionMode.SelectAction)
        {
            if (index < 0 || index >= _actionListInfo.Actions.Count)
                return;
            SelectActionRow(index);
        }
        else if (_selectionMode == SelectionMode.Quest)
        {
            if (index < 0 || index >= _questListInfo.Count)
                return;
            SelectQuestRow(index);
        }
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_up") && _inputManager.IsActionAllowed("ui_up"))
        {
            if (_selectedIndex == 0)
                SelectRow(_selectionList.GetChildCount() - 1);
            else
                SelectRow(_selectedIndex - 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") && _inputManager.IsActionAllowed("ui_down"))
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
            else if (_selectButton.Visible && !_selectButton.Disabled)
                buttonToUse = _selectButton;
            else if (_sellButton.Visible && !_sellButton.Disabled)
                buttonToUse = _sellButton;
            else if (_buyButton.Visible && !_buyButton.Disabled)
                buttonToUse = _buyButton;
            if (buttonToUse == null)
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
        else if (@event.IsActionPressed("ui_abandon"))
        {
            if (!_abandonButton.Visible || _abandonButton.Disabled)
            {
                AcceptEvent();
                return;
            }
            _abandonButton.GrabFocus();
            _abandonButton.EmitSignal("pressed");
            _abandonButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            if (!_cancelButton.Visible || _cancelButton.Disabled)
            {
                AcceptEvent();
                return;
            }
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

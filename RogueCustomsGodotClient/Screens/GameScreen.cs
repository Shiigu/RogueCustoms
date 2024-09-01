using Godot;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Screens.GameSubScreens;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class GameScreen : Control
{
    private GlobalState _globalState;
    private GamePanel _experienceBarPanel, _controlsPanel;
    private MapPanel _mapPanel;
    private InfoPanel _infoPanel;
    private MessageLogPanel _messageLogPanel;
    private Button _saveGameButton, _exitButton;
    private ScreenFlash _screenFlash;
    private int _lastTurn;
    private ControlMode _previousControlMode;

    private List<GamePanel> _children;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _saveGameButton = GetNode<Button>("ButtonsBorder/ButtonsPanel/SaveGameButton");
        _exitButton = GetNode<Button>("ButtonsBorder/ButtonsPanel/ExitButton");
        _mapPanel = GetNode<MapPanel>("MapPanel");
        _infoPanel = GetNode<InfoPanel>("InfoPanel");
        _messageLogPanel = GetNode<MessageLogPanel>("MessageLogPanel");
        _experienceBarPanel = GetNode<GamePanel>("ExperienceBarPanel");
        _controlsPanel = GetNode<GamePanel>("ControlsPanel");
        _children = new List<GamePanel> { _mapPanel, _infoPanel, _messageLogPanel, _experienceBarPanel, _controlsPanel };
        _screenFlash = GetNode<ScreenFlash>("ScreenFlash");
        SetUp();
    }

    private void SetUp()
    {
        _globalState.MustUpdateGameScreen = true;
        _lastTurn = -1;
        _saveGameButton.Pressed += SaveGameButton_Pressed;
        _exitButton.Pressed += ExitButton_Pressed;
        _Process(0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (_globalState.MustUpdateGameScreen)
        {
            _globalState.DungeonInfo = _globalState.DungeonManager.GetDungeonStatus(_globalState.DungeonId);
            var dungeonStatus = _globalState.DungeonInfo;
            if (dungeonStatus.DungeonStatus == DungeonStatus.Completed)
            {
                _globalState.MessageScreenType = MessageScreenType.Ending;
                GetTree().ChangeSceneToFile("res://Screens/MessageScreen.tscn");
                return;
            }
            else if (dungeonStatus.DungeonStatus == DungeonStatus.GameOver)
            {
                _globalState.PlayerControlMode = ControlMode.None;
                _saveGameButton.Disabled = true;
            }

            if (_globalState.PlayerControlMode != ControlMode.Targeting && _globalState.PlayerControlMode != ControlMode.None)
            {
                _saveGameButton.Disabled = false;
                var playerEntity = dungeonStatus.PlayerEntity;
                if (!playerEntity.CanTakeAction)
                    _globalState.PlayerControlMode = ControlMode.MustSkipTurn;
                else if (playerEntity.Movement == 0)
                {
                    _globalState.PlayerControlMode = ControlMode.Immobilized;
                    if (dungeonStatus.IsPlayerOnStairs())
                        _globalState.PlayerControlMode = ControlMode.ImmobilizedOnStairs;
                }
                else
                {
                    _globalState.PlayerControlMode = ControlMode.NormalMove;
                    if (dungeonStatus.IsPlayerOnStairs())
                        _globalState.PlayerControlMode = ControlMode.NormalOnStairs;
                }
            }

            if(_globalState.PlayerControlMode == ControlMode.None)
            {
                _mapPanel.StopTargeting();
                _saveGameButton.Disabled = true;
            }

            foreach (var child in _children)
            {
                child.Update();
            }

            if (dungeonStatus.PlayerTookDamage)
            {
                _screenFlash.Flash(new Color { R8 = 255, G8 = 0, B8 = 0, A = 1 });
            }
            else if (dungeonStatus.PlayerGotStatusChange)
            {
                _screenFlash.Flash(new Color { R8 = 238, G8 = 130, B8 = 238, A = 1 });
            }
            else if (dungeonStatus.PlayerGotHealed)
            {
                _screenFlash.Flash(new Color { R8 = 0, G8 = 255, B8 = 0, A = 1 });
            }
            else if (dungeonStatus.PlayerGotMPBurned)
            {
                _screenFlash.Flash(new Color { R8 = 0, G8 = 0, B8 = 139, A = 1 });
            }
            else if (dungeonStatus.PlayerGotMPReplenished)
            {
                _screenFlash.Flash(new Color { R8 = 0, G8 = 0, B8 = 139, A = 1 });
            }

            if (dungeonStatus.TurnCount != _lastTurn)
                ShowMessagesIfNeeded(0);
            _globalState.MustUpdateGameScreen = false;
            _lastTurn = dungeonStatus.TurnCount;
        }
    }
    private void ShowMessagesIfNeeded(int index = 0)
    {
        if (_globalState.PlayerControlMode == ControlMode.Targeting)
        {
            _mapPanel.StopTargeting();
            _globalState.PlayerControlMode = _previousControlMode;
        }
        _controlsPanel.Update();
        var messageBox = _globalState.DungeonInfo.MessageBoxes.ElementAtOrDefault(index);
        if (messageBox != null)
        {
            this.CreateStandardPopup(messageBox.Title, messageBox.Message,
                new PopUpButton[]
                {
                    new() { Text = messageBox.ButtonCaption, ActionPress = "ui_accept", Callback = () => ShowMessagesIfNeeded(index + 1) }
                }, new Color { R8 = messageBox.WindowColor.R, G8 = messageBox.WindowColor.G, B8 = messageBox.WindowColor.B, A8 = messageBox.WindowColor.A });
        }
    }

    private void SaveGameButton_Pressed()
    {
        try
        {
            if (_globalState.PlayerControlMode == ControlMode.Targeting)
            {
                _mapPanel.StopTargeting();
                _globalState.PlayerControlMode = _previousControlMode;
            }
            _controlsPanel.Update();
            var output = _globalState.DungeonManager.SaveDungeon(_globalState.DungeonId);
            if (output != null)
            {
                using var file = FileAccess.Open(_globalState.SaveGamePath, FileAccess.ModeFlags.Write);
                foreach (var @byte in output.DungeonData)
                {
                    file.Store8(@byte);
                }

                this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                            TranslationServer.Translate("SuccessfulSavePromptText"),
                                            new PopUpButton[]
                                            {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () => GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn"), ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                            }, new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
            }
        }
        catch
        {
            this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                        TranslationServer.Translate("FailedSaveText"),
                                        new PopUpButton[]
                                        {
                                            new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" }
                                        }, new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });
        }
    }

    private void ExitButton_Pressed()
    {
        _mapPanel.StopTargeting();
        _globalState.PlayerControlMode = _previousControlMode;
        _controlsPanel.Update();
        this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                    TranslationServer.Translate("ExitPromptText"),
                                    new PopUpButton[]
                                    {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () => GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn"), ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                    }, new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });
    }

    public override void _Input(InputEvent @event)
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        _infoPanel.DetailsButton.Disabled = _globalState.PlayerControlMode == ControlMode.Targeting;
        _messageLogPanel.MessageWindowButton.Disabled = _globalState.PlayerControlMode == ControlMode.Targeting;
        _saveGameButton.Disabled = _globalState.PlayerControlMode == ControlMode.Targeting;
        _exitButton.Disabled = _globalState.PlayerControlMode == ControlMode.Targeting;
        switch (_globalState.PlayerControlMode)
        {
            case ControlMode.NormalMove:
            case ControlMode.NormalOnStairs:
                CheckNormalModeInput(@event);
                CheckButtonsInput(@event);
                break;
            case ControlMode.Immobilized:
            case ControlMode.ImmobilizedOnStairs:
                CheckImmobilizedModeInput(@event);
                CheckButtonsInput(@event);
                break;
            case ControlMode.MustSkipTurn:
                CheckCannotActModeInput(@event);
                CheckButtonsInput(@event);
                break;
            case ControlMode.None:
                CheckNoneModeInput(@event);
                break;
            case ControlMode.Targeting:
                CheckTargetingModeInput(@event);
                break;
        }
    }

    private void CheckNormalModeInput(InputEvent @event)
    {
        var coordinateInput = new CoordinateInput{
            X = 0,
            Y = 0
        };
        var didTurn = false;

        if (_globalState.PlayerControlMode == ControlMode.NormalOnStairs && @event.IsActionPressed("ui_use"))
        {
            this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                        TranslationServer.Translate("StairsPromptText"),
                                        new PopUpButton[]
                                        {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () =>
                                                    {
                                                        _globalState.DungeonManager.PlayerTakeStairs(_globalState.DungeonId);
                                                        _globalState.MustUpdateGameScreen = true;
                                                    }, ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                        }, new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_skip_turn"))
        {
            _globalState.DungeonManager.MovePlayer(_globalState.DungeonId, coordinateInput);
            AcceptEvent();
            _globalState.MustUpdateGameScreen = true;
            return;
        }
        else if (@event.IsActionPressed("ui_inventory"))
        {
            this.CreateInventoryWindow(_globalState.DungeonManager.GetPlayerInventory(_globalState.DungeonId));
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_aim"))
        {
            _mapPanel.StartTargeting();
            _previousControlMode = _globalState.PlayerControlMode;
            _globalState.PlayerControlMode = ControlMode.Targeting;
            _controlsPanel.Update();
            AcceptEvent();
            return;
        }
        else
        {
            if (@event.IsActionPressed("ui_up"))
                coordinateInput.Y = -1;
            else if (@event.IsActionPressed("ui_down"))
                coordinateInput.Y = 1;
            if (@event.IsActionPressed("ui_left"))
                coordinateInput.X = -1;
            else if (@event.IsActionPressed("ui_right"))
                coordinateInput.X = 1;
            if (coordinateInput.X != 0 || coordinateInput.Y != 0)
                didTurn = true;
        }

        if (didTurn)
        {
            _globalState.DungeonManager.MovePlayer(_globalState.DungeonId, coordinateInput);
            AcceptEvent();
            _globalState.MustUpdateGameScreen = true;
        }
    }

    private void CheckImmobilizedModeInput(InputEvent @event)
    {
        var coordinateInput = new CoordinateInput
        {
            X = 0,
            Y = 0
        };

        if (_globalState.PlayerControlMode == ControlMode.ImmobilizedOnStairs && @event.IsActionPressed("ui_use"))
        {
            this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                        TranslationServer.Translate("StairsPromptText"),
                                        new PopUpButton[]
                                        {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () =>
                                                    {
                                                        _globalState.DungeonManager.PlayerTakeStairs(_globalState.DungeonId);
                                                        _globalState.MustUpdateGameScreen = true;
                                                    }, ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                        }, new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_skip_turn"))
        {
            _globalState.DungeonManager.MovePlayer(_globalState.DungeonId, coordinateInput);
            AcceptEvent();
            _globalState.MustUpdateGameScreen = true;
            return;
        }
        else if (@event.IsActionPressed("ui_inventory"))
        {
            this.CreateInventoryWindow(_globalState.DungeonManager.GetPlayerInventory(_globalState.DungeonId));
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_aim"))
        {
            _mapPanel.StartTargeting();
            _previousControlMode = _globalState.PlayerControlMode;
            _globalState.PlayerControlMode = ControlMode.Targeting;
            _controlsPanel.Update();
            AcceptEvent();
            return;
        }
    }

    private void CheckCannotActModeInput(InputEvent @event)
    {
        var coordinateInput = new CoordinateInput
        {
            X = 0,
            Y = 0
        };

        if (@event.IsActionPressed("ui_skip_turn"))
        {
            _globalState.DungeonManager.MovePlayer(_globalState.DungeonId, coordinateInput);
            AcceptEvent();
            _globalState.MustUpdateGameScreen = true;
            return;
        }
        else if (@event.IsActionPressed("ui_inventory"))
        {
            this.CreateInventoryWindow(_globalState.DungeonManager.GetPlayerInventory(_globalState.DungeonId));
            AcceptEvent();
            return;
        }
    }

    private void CheckTargetingModeInput(InputEvent @event)
    {
        var coordinateInput = new Vector2(0, 0);

        if (@event.IsActionPressed("ui_cancel"))
        {
            _mapPanel.StopTargeting();
            _globalState.PlayerControlMode = _previousControlMode;
            _controlsPanel.Update();
        }
        else if (@event.IsActionPressed("ui_aim"))
        {
            this.CreateActionSelectWindow(_globalState.DungeonManager.GetPlayerAttackActions(_globalState.DungeonId, _mapPanel.CursorCoords.Value.X, _mapPanel.CursorCoords.Value.Y), _mapPanel.CursorCoords.Value);
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_view_entity"))
        {
            var entityDetails = _globalState.DungeonManager.GetDetailsOfEntity(_globalState.DungeonId, _mapPanel.CursorCoords.Value.X, _mapPanel.CursorCoords.Value.Y);
            if (entityDetails != null)
            {
                var entityWindowText = new StringBuilder();
                entityWindowText.Append($"[center]{entityDetails.Name}[/center]\n\n");
                entityWindowText.Append($"[center]{entityDetails.ConsoleRepresentation.ToBbCodeRepresentation()}[/center]\n\n");
                entityWindowText.Append($"{entityDetails.Description}");
                this.CreateStandardPopup(TranslationServer.Translate("EntityDetailTitleText"),
                                            entityWindowText.ToString(),
                                            new PopUpButton[]
                                            {
                                            new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" }
                                            }, new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
            }
            AcceptEvent();
            return;
        }
        else
        {
            if (@event.IsActionPressed("ui_up"))
                coordinateInput = new(coordinateInput.X, -1);
            else if (@event.IsActionPressed("ui_down"))
                coordinateInput = new(coordinateInput.X, 1);
            if (@event.IsActionPressed("ui_left"))
                coordinateInput = new(-1, coordinateInput.Y);
            else if (@event.IsActionPressed("ui_right"))
                coordinateInput = new(1, coordinateInput.Y);
            if (coordinateInput.X != 0 || coordinateInput.Y != 0)
                _mapPanel.MoveTarget(coordinateInput);
        }

        AcceptEvent();
    }

    private void CheckNoneModeInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            _exitButton.GrabFocus();
            _exitButton.EmitSignal("pressed");
            _exitButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_open_details"))
        {
            _infoPanel.DetailsButton.GrabFocus();
            _infoPanel.DetailsButton.EmitSignal("pressed");
            _infoPanel.DetailsButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_open_log"))
        {
            _messageLogPanel.MessageWindowButton.GrabFocus();
            _messageLogPanel.MessageWindowButton.EmitSignal("pressed");
            _messageLogPanel.MessageWindowButton.ButtonPressed = true;
            AcceptEvent();
        }
    }

    private void CheckButtonsInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_save"))
        {
            _saveGameButton.GrabFocus();
            _saveGameButton.EmitSignal("pressed");
            _saveGameButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            _exitButton.GrabFocus();
            _exitButton.EmitSignal("pressed");
            _exitButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_open_details"))
        {
            _infoPanel.DetailsButton.GrabFocus();
            _infoPanel.DetailsButton.EmitSignal("pressed");
            _infoPanel.DetailsButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_open_log"))
        {
            _messageLogPanel.MessageWindowButton.GrabFocus();
            _messageLogPanel.MessageWindowButton.EmitSignal("pressed");
            _messageLogPanel.MessageWindowButton.ButtonPressed = true;
            AcceptEvent();
        }
    }
}

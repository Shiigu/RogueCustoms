using Godot;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Entities;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Invokers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Screens.GameSubScreens;
using RogueCustomsGodotClient.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable AsyncFixer03 // Fire-and-forget async-void methods or delegates
public partial class GameScreen : Control
{
    private GlobalState _globalState;
    private ExceptionLogger _exceptionLogger;
    private ExperienceBarPanel _experienceBarPanel;
    private GamePanel _controlsPanel;
    private MapPanel _mapPanel;
    private InfoPanel _infoPanel;
    private MessageLogPanel _messageLogPanel;
    private Button _saveGameButton, _exitButton;
    private ScreenFlash _screenFlash;
    private int _lastTurn;
    private ControlMode _previousControlMode;
    private InputManager _inputManager;

    private List<GamePanel> _children;
    private CoordinateInput _coords;
    private AudioStreamPlayer _audioStreamPlayer;

    private bool _soundIsPlaying, _popUpIsOpen, _processingEvents;
    private TaskCompletionSource<bool> _soundFinished;

    private List<(SpecialEffect SpecialEffect, Color Color)> SpecialEffectsWithFlash;
    private List<(SpecialEffect SpecialEffect, string Path)> SpecialEffectsWithSound;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SpecialEffectsWithFlash = new()
        {
            (SpecialEffect.NPCDamaged, new Color { R8 = 0, G8 = 100, B8 = 0, A = 1 }),
            (SpecialEffect.PlayerDamaged, new Color { R8 = 255, G8 = 0, B8 = 0, A = 1 }),
            (SpecialEffect.Statused, new Color { R8 = 238, G8 = 130, B8 = 238, A = 1 }),
            (SpecialEffect.HPUp, new Color { R8 = 0, G8 = 255, B8 = 0, A = 1 }),
            (SpecialEffect.MPDown, new Color { R8 =  0, G8 = 0, B8 = 139, A = 1 }),
            (SpecialEffect.MPUp, new Color { R8 = 0, G8 = 0, B8 = 139, A = 1 }),
        };

        SpecialEffectsWithSound = new()
        {
            (SpecialEffect.GameOver, "res://Sounds/gameover.wav"),
            (SpecialEffect.StairsReveal, "res://Sounds/stairsreveal.wav"),
            (SpecialEffect.Bumped, "res://Sounds/bump.wav"),
            (SpecialEffect.LevelUp, "res://Sounds/levelup.wav"),
            (SpecialEffect.Miss, "res://Sounds/miss.wav"),
            (SpecialEffect.PlayerDamaged, "res://Sounds/playerdamaged.wav"),
            (SpecialEffect.NPCDamaged, "res://Sounds/npcdamaged.wav"),
            (SpecialEffect.HPUp, "res://Sounds/hpup.wav"),
            (SpecialEffect.MPDown, "res://Sounds/mpdown.wav"),
            (SpecialEffect.MPUp, "res://Sounds/mpup.wav"),
            (SpecialEffect.HungerDown, "res://Sounds/hungerdown.wav"),
            (SpecialEffect.HungerUp, "res://Sounds/hungerup.wav"),
            (SpecialEffect.NPCDeath, "res://Sounds/npcdeath.wav"),
            (SpecialEffect.NPCRevive, "res://Sounds/npcrevive.wav"),
            (SpecialEffect.ItemUse, "res://Sounds/itemuse.wav"),
            (SpecialEffect.NPCItemUse, "res://Sounds/npcitemuse.wav"),
            (SpecialEffect.ItemDrop, "res://Sounds/itemdrop.wav"),
            (SpecialEffect.ItemGet, "res://Sounds/itemget.wav"),
            (SpecialEffect.ItemEquip, "res://Sounds/itemequip.wav"),
            (SpecialEffect.NPCItemGet, "res://Sounds/npcitemget.wav"),
            (SpecialEffect.StatBuff, "res://Sounds/statbuff.wav"),
            (SpecialEffect.StatNerf, "res://Sounds/statnerf.wav"),
            (SpecialEffect.Statused, "res://Sounds/statused.wav"),
            (SpecialEffect.StatusLeaves, "res://Sounds/statusleaves.wav"),
            (SpecialEffect.Summon, "res://Sounds/summon.wav"),
            (SpecialEffect.TakeStairs, "res://Sounds/3steps.wav"),
            (SpecialEffect.Teleport, "res://Sounds/teleport.wav"),
            (SpecialEffect.TrapActivate, "res://Sounds/trapactivate.wav"),
            (SpecialEffect.TrapSet, "res://Sounds/trapset.wav"),
            (SpecialEffect.KeyGet, "res://Sounds/keyget.wav"),
            (SpecialEffect.DoorClosed, "res://Sounds/doorclosed.wav"),
            (SpecialEffect.DoorOpen, "res://Sounds/dooropen.wav"),
            (SpecialEffect.MonsterHouseAlarm, "res://Sounds/monsterhousealarm.wav"),
        };



        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _saveGameButton = GetNode<Button>("ButtonsBorder/ButtonsPanel/SaveGameButton");
        _exitButton = GetNode<Button>("ButtonsBorder/ButtonsPanel/ExitButton");
        _mapPanel = GetNode<MapPanel>("MapPanel");
        _infoPanel = GetNode<InfoPanel>("InfoPanel");
        _messageLogPanel = GetNode<MessageLogPanel>("MessageLogPanel");
        _experienceBarPanel = GetNode<ExperienceBarPanel>("ExperienceBarPanel");
        _controlsPanel = GetNode<GamePanel>("ControlsPanel");
        _children = new List<GamePanel> { _mapPanel, _infoPanel, _messageLogPanel, _experienceBarPanel, _controlsPanel };
        _screenFlash = GetNode<ScreenFlash>("ScreenFlash");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

        SetUp();
    }

    private void SetUp()
    {
        _globalState.DungeonManager.SetPromptInvoker(new PromptInvoker(this));
        _globalState.MustUpdateGameScreen = true;
        _lastTurn = -1;
        _saveGameButton.Pressed += SaveGameButton_Pressed;
        _exitButton.Pressed += ExitButton_Pressed;
        _audioStreamPlayer.Finished += OnSoundFinished;

        _coords = new CoordinateInput
        {
            X = 0,
            Y = 0
        };

        _processingEvents = false;
        _Process(0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override async void _Process(double delta)
    {
        try
        {
            _globalState.DungeonInfo = await _globalState.DungeonManager.GetDungeonStatus();
            var dungeonStatus = _globalState.DungeonInfo;
            if ((_globalState.MustUpdateGameScreen || !dungeonStatus.Read) && !_processingEvents)
            {
                if (dungeonStatus.DungeonStatus == DungeonStatus.Completed)
                {
                    _globalState.MessageScreenType = MessageScreenType.Ending;
                    GetTree().ChangeSceneToFile("res://Screens/MessageScreen.tscn");
                    return;
                }
                else if (dungeonStatus.DungeonStatus == DungeonStatus.GameOver)
                {
                    if (_globalState.IsHardcoreMode)
                    {
                        _ = this.CreateStandardPopup(
                            TranslationServer.Translate("HardcoreModeDeathHeaderText"),
                            TranslationServer.Translate("HardcoreModeDeathText"),
                            new PopUpButton[]
                            {
                    new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" },
                            },
                            new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });

                        if (FileAccess.FileExists(_globalState.CurrentSavePath))
                        {
                            var dir = DirAccess.Open(_globalState.SaveGameFolder);
                            dir.Remove(_globalState.CurrentSavePath);
                        }
                    }
                    _globalState.PlayerControlMode = ControlMode.None;
                    _saveGameButton.Disabled = true;
                }

                if (_globalState.PlayerControlMode != ControlMode.Targeting && _globalState.PlayerControlMode != ControlMode.None)
                {
                    _saveGameButton.Disabled = false;
                }

                if (_globalState.PlayerControlMode == ControlMode.None)
                {
                    _mapPanel.StopTargeting();
                    _saveGameButton.Disabled = true;
                }

                foreach (var child in _children)
                {
                    child.Update();
                }

                if (!dungeonStatus.Read)
                    await UpdateUIViaEvents();

                _mapPanel.UpdateTurnCount(dungeonStatus.TurnCount);
                _lastTurn = dungeonStatus.TurnCount;
            }

            if (_globalState.PlayerControlMode == ControlMode.Waiting)
            {
                _coords = new CoordinateInput
                {
                    X = 0,
                    Y = 0
                };
                return;
            }
            if (_globalState.PlayerControlMode == ControlMode.NormalMove || _globalState.PlayerControlMode == ControlMode.NormalOnStairs || _globalState.PlayerControlMode == ControlMode.Targeting)
            {
                await HandleMovementKeys();
            }
        }
        catch (Exception ex)
        {
            _exceptionLogger.LogMessage(ex);
        }
    }

    private async Task HandleMovementKeys()
    {
        if (_globalState.PlayerControlMode == ControlMode.Waiting) return;
        if (GetChildren().Any(c => c.IsPopUp())) return;

        if (Input.IsActionPressed("ui_up") && _inputManager.IsActionAllowed("ui_up") && _coords.Y == 0)
        {
            _coords.Y = -1;
        }
        if (Input.IsActionPressed("ui_down") && _inputManager.IsActionAllowed("ui_down") && _coords.Y == 0)
        {
            _coords.Y = 1;
        }
        if (Input.IsActionPressed("ui_left") && _inputManager.IsActionAllowed("ui_left") &&  _coords.X == 0)
        {
            _coords.X = -1;
        }
        if (Input.IsActionPressed("ui_right") && _inputManager.IsActionAllowed("ui_right") && _coords.X == 0)
        {
            _coords.X = 1;
        }
        if (_coords.X != 0 || _coords.Y != 0)
        {
            var coordsToSend = _coords;
            _coords = new CoordinateInput
            {
                X = 0,
                Y = 0
            };
            if (_globalState.PlayerControlMode == ControlMode.Targeting)
            {
                _mapPanel.MoveTarget(new(coordsToSend.X, coordsToSend.Y));
            }
            else
            {
                _globalState.MustUpdateGameScreen = true;
                _globalState.PlayerControlMode = ControlMode.Waiting;
                await _globalState.DungeonManager.MovePlayer(coordsToSend);
            }
        }
    }

    private async Task UpdateUIViaEvents()
    {
        _processingEvents = true;
        var controlModeToPick = ControlMode.NormalMove;
        _globalState.PlayerControlMode = ControlMode.Waiting;
        _saveGameButton.Disabled = true;
        _exitButton.Disabled = true;
        _infoPanel.DetailsButton.Disabled = true;
        _messageLogPanel.MessageWindowButton.Disabled = true;
        _controlsPanel.Update();
        var unimportantDisplayEventTypes = new List<DisplayEventType> { DisplayEventType.ClearLogMessages, DisplayEventType.AddMessageBox, DisplayEventType.AddLogMessage };
        _mapPanel.StopTargeting();
            
        foreach (var displayEventList in _globalState.DungeonInfo.DisplayEvents)
        {
            if (_soundIsPlaying && displayEventList.Events.Any(e => e.DisplayEventType == DisplayEventType.PlaySpecialEffect))
                await _soundFinished.Task;
            foreach (var displayEvent in displayEventList.Events)
            {
                switch (displayEvent.DisplayEventType)
                {
                    case DisplayEventType.PlaySpecialEffect:
                        var specialEffect = (SpecialEffect)displayEvent.Params[0];
                        var correspondingFlash = SpecialEffectsWithFlash.Find(se => se.SpecialEffect == specialEffect);
                        var correspondingSound = SpecialEffectsWithSound.Find(se => se.SpecialEffect == specialEffect);
                        if (_soundIsPlaying)
                            await _soundFinished.Task;
                        if (correspondingFlash != default)
                            await _screenFlash.Flash(correspondingFlash.Color);
                        if (correspondingSound != default)
                        {
                            _soundIsPlaying = true;
                            _soundFinished = new TaskCompletionSource<bool>();

                            _audioStreamPlayer.Stream = (AudioStream)GD.Load(correspondingSound.Path);
                            _audioStreamPlayer.Play();
                        }
                        break;
                    case DisplayEventType.AddLogMessage:
                        var message = displayEvent.Params[0] as MessageDto;
                        _messageLogPanel.Append(message);
                        break;
                    case DisplayEventType.ClearLogMessages:
                        _messageLogPanel.Clear();
                        break;
                    case DisplayEventType.AddMessageBox:
                        var messageBox = displayEvent.Params[0] as MessageBoxDto;
                        await ShowMessageBox(messageBox);
                        break;
                    case DisplayEventType.UpdateTileRepresentation:
                        var position = displayEvent.Params[0] as GamePoint;
                        var consoleRepresentation = displayEvent.Params[1] as ConsoleRepresentation;
                        _mapPanel.UpdateTileRepresentation(new Vector2I { X = position.X, Y = position.Y }, consoleRepresentation);
                        break;
                    case DisplayEventType.SetDungeonStatus:
                        var dungeonStatus = (DungeonStatus)displayEvent.Params[0];
                        if (dungeonStatus == DungeonStatus.Completed)
                        {
                            _globalState.MessageScreenType = MessageScreenType.Ending;
                            GetTree().ChangeSceneToFile("res://Screens/MessageScreen.tscn");
                            return;
                        }
                        if (dungeonStatus == DungeonStatus.GameOver)
                        {
                            controlModeToPick = ControlMode.None;
                            if (_globalState.IsHardcoreMode)
                            {
                                _ = this.CreateStandardPopup(
                                    TranslationServer.Translate("HardcoreModeDeathHeaderText"),
                                    TranslationServer.Translate("HardcoreModeDeathText"),
                                    new PopUpButton[]
                                    {
                                            new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" },
                                    },
                                    new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });

                                if (!string.IsNullOrWhiteSpace(_globalState.CurrentSavePath) && FileAccess.FileExists(_globalState.CurrentSavePath))
                                    DirAccess.RemoveAbsolute(_globalState.CurrentSavePath);
                            }
                        }
                        break;
                    case DisplayEventType.SetOnStairs:
                        var onStairs = (bool)displayEvent.Params[0];
                        if (onStairs)
                        {
                            if (controlModeToPick == ControlMode.NormalMove)
                                controlModeToPick = ControlMode.NormalOnStairs;
                            if (controlModeToPick == ControlMode.Immobilized)
                                controlModeToPick = ControlMode.ImmobilizedOnStairs;
                        }
                        else
                        {
                            if (controlModeToPick == ControlMode.NormalOnStairs)
                                controlModeToPick = ControlMode.NormalMove;
                            if (controlModeToPick == ControlMode.ImmobilizedOnStairs)
                                controlModeToPick = ControlMode.Immobilized;
                        }
                        break;
                    case DisplayEventType.SetCanMove:
                        var canMove = (bool)displayEvent.Params[0];
                        if (canMove)
                        {
                            if (controlModeToPick == ControlMode.ImmobilizedOnStairs)
                                controlModeToPick = ControlMode.NormalOnStairs;
                            if (controlModeToPick == ControlMode.Immobilized)
                                controlModeToPick = ControlMode.NormalMove;
                        }
                        else
                        {
                            if (controlModeToPick == ControlMode.NormalOnStairs)
                                controlModeToPick = ControlMode.ImmobilizedOnStairs;
                            if (controlModeToPick == ControlMode.NormalMove)
                                controlModeToPick = ControlMode.Immobilized;
                        }
                        break;
                    case DisplayEventType.SetCanAct:
                        var canAct = (bool)displayEvent.Params[0];
                        if (!canAct)
                            controlModeToPick = ControlMode.MustSkipTurn;
                        break;
                    case DisplayEventType.UpdatePlayerData:
                        _infoPanel.UpdatePlayerData(displayEvent.Params);
                        break;
                    case DisplayEventType.UpdatePlayerPosition:
                        position = displayEvent.Params[0] as GamePoint;
                        _globalState.DungeonInfo.PlayerEntity.X = position.X;
                        _globalState.DungeonInfo.PlayerEntity.Y = position.Y;
                        break;
                    case DisplayEventType.UpdateExperienceBar:
                        var experience = (int) displayEvent.Params[0];
                        var experienceToLevelUp = (int) displayEvent.Params[1];
                        var currentExperiencePercentage = (int) displayEvent.Params[2];
                        _experienceBarPanel.UpdateExperienceBar(experience, experienceToLevelUp, currentExperiencePercentage);
                        break;
                }
            }
            while (_screenFlash.Visible)
                await Task.Delay(50);
            if (displayEventList.Events.Any(e => e.DisplayEventType == DisplayEventType.PlaySpecialEffect))
                await Task.Delay(50);
            if (!displayEventList.Events.Any(e => unimportantDisplayEventTypes.Contains(e.DisplayEventType)))
                await ToSignal(GetTree(), "process_frame");
        }
        if (_soundIsPlaying)
            await Task.Delay(50);
        _soundIsPlaying = false;
        _globalState.PlayerControlMode = controlModeToPick;
        _controlsPanel.Update();
        _globalState.DungeonInfo.Read = true;
        _globalState.MustUpdateGameScreen = false;

        _infoPanel.DetailsButton.Disabled = false;
        _exitButton.Disabled = false;
        _messageLogPanel.MessageWindowButton.Disabled = false;
        if (_globalState.PlayerControlMode != ControlMode.Targeting && _globalState.PlayerControlMode != ControlMode.None)
            _saveGameButton.Disabled = false;
        if (_globalState.PlayerControlMode == ControlMode.None)
            _saveGameButton.Disabled = true;
        _processingEvents = false;
    }

    private void SaveGameButton_Pressed()
    {
        try
        {
            if (_globalState.PlayerControlMode == ControlMode.Waiting) return;
            if (_globalState.PlayerControlMode == ControlMode.Targeting)
            {
                _mapPanel.StopTargeting();
                _globalState.PlayerControlMode = _previousControlMode;
            }
            _controlsPanel.Update();
            var dungeonStatus = _globalState.DungeonInfo;
            var output = _globalState.DungeonManager.SaveDungeon();
            if (output != null)
            {
                var saveData = new SaveGame()
                {
                    DungeonName = dungeonStatus.DungeonName,
                    FloorName = dungeonStatus.FloorName,
                    DungeonData = Convert.ToBase64String(output.DungeonData),
                    DungeonVersion = output.DungeonVersion,
                    PlayerName = dungeonStatus.PlayerEntity.Name,
                    PlayerLevel = dungeonStatus.PlayerEntity.Level,
                    PlayerRepresentation = dungeonStatus.PlayerEntity.ConsoleRepresentation,
                    IsPlayerDead = dungeonStatus.PlayerEntity.HP <= 0,
                    IsHardcoreMode = dungeonStatus.IsHardcoreMode,
                    SaveDate = DateTime.Now
                };

                var saveDataAsJSON = JsonSerializer.Serialize(saveData);
                var saveName = dungeonStatus.IsHardcoreMode ? $"{output.FileName}_H.rcs" : $"{output.FileName}.rcs";
                var filePath = $"{_globalState.SaveGameFolder}/{saveName}";

                using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
                file.StoreString(saveDataAsJSON);

                _globalState.CurrentSavePath = filePath;

                _ = this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
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
            _ = this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
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
        _ = this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                    TranslationServer.Translate("ExitPromptText"),
                                    new PopUpButton[]
                                    {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () => GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn"), ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                    }, new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });
    }

    public override void _Input(InputEvent @event)
    {
        if (_globalState.PlayerControlMode == ControlMode.Waiting) return;
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
        if (_globalState.PlayerControlMode == ControlMode.NormalOnStairs && @event.IsActionPressed("ui_use"))
        {
            TakeStairsPrompt();
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_skip_turn"))
        {
            try
            {
                _globalState.DungeonManager.MovePlayer(new CoordinateInput
                {
                    X = 0,
                    Y = 0
                });
                AcceptEvent();
                _globalState.MustUpdateGameScreen = true;
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
            return;
        }
        else if (@event.IsActionPressed("ui_inventory"))
        {
            try
            {
                this.CreateInventoryWindow(_globalState.DungeonManager.GetPlayerInventory());
                AcceptEvent();
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
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

    private void CheckImmobilizedModeInput(InputEvent @event)
    {
        if (_globalState.PlayerControlMode == ControlMode.ImmobilizedOnStairs && @event.IsActionPressed("ui_use"))
        {
            TakeStairsPrompt();
            AcceptEvent();
            return;
        }
        else if (@event.IsActionPressed("ui_skip_turn"))
        {
            try
            {
                _globalState.DungeonManager.MovePlayer(new CoordinateInput
                {
                    X = 0,
                    Y = 0
                });
                AcceptEvent();
                _globalState.MustUpdateGameScreen = true;
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
            return;
        }
        else if (@event.IsActionPressed("ui_inventory"))
        {
            try
            {
                this.CreateInventoryWindow(_globalState.DungeonManager.GetPlayerInventory());
                AcceptEvent();
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
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
            try
            {
                _globalState.DungeonManager.MovePlayer(new CoordinateInput
                {
                    X = 0,
                    Y = 0
                });
                AcceptEvent();
                _globalState.MustUpdateGameScreen = true;
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
            return;
        }
        else if (@event.IsActionPressed("ui_inventory"))
        {
            try
            {
                this.CreateInventoryWindow(_globalState.DungeonManager.GetPlayerInventory());
                AcceptEvent();
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
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
            try
            {
                this.CreateActionSelectWindow(_globalState.DungeonManager.GetPlayerAttackActions(_mapPanel.CursorCoords.Value.X, _mapPanel.CursorCoords.Value.Y), _mapPanel.CursorCoords.Value);
                AcceptEvent();
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
            return;
        }
        else if (@event.IsActionPressed("ui_view_entity"))
        {
            try
            {
                var entityDetails = _globalState.DungeonManager.GetDetailsOfEntity(_mapPanel.CursorCoords.Value.X, _mapPanel.CursorCoords.Value.Y);
                if (entityDetails != null)
                {
                    var entityWindowText = new StringBuilder();
                    if (entityDetails.ShowEntityDescription)
                    {
                        entityWindowText.Append($"[center]{entityDetails.EntityName}[/center]\n\n");
                        entityWindowText.Append($"[center]{entityDetails.EntityConsoleRepresentation.ToBbCodeRepresentation()}[/center]\n\n");
                        entityWindowText.Append($"{entityDetails.EntityDescription}");
                    }
                    if (entityDetails.ShowTileDescription)
                    {
                        if (entityDetails.ShowEntityDescription)
                            entityWindowText.Append($"\n\n");
                        entityWindowText.Append($"[center]{entityDetails.TileName}[/center]\n\n");
                        entityWindowText.Append($"[center]{entityDetails.TileConsoleRepresentation.ToBbCodeRepresentation()}[/center]\n\n");
                        entityWindowText.Append($"{entityDetails.TileDescription}");
                    }
                    _ = this.CreateStandardPopup(TranslationServer.Translate("EntityDetailTitleText"),
                                                entityWindowText.ToString(),
                                                new PopUpButton[]
                                                {
                                            new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" }
                                                }, new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
                }
                AcceptEvent();
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogMessage(ex);
            }
            return;
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

    private void TakeStairsPrompt()
    {
        _ = this.CreateStandardPopup(_globalState.DungeonInfo.DungeonName,
                                    TranslationServer.Translate("StairsPromptText"),
                                    new PopUpButton[]
                                    {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = async () =>
                                                    {
                                                        try
                                                        {
                                                            await _globalState.DungeonManager.PlayerTakeStairs();
                                                            _globalState.MustUpdateGameScreen = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            _exceptionLogger.LogMessage(ex);
                                                        }
                                                    }, ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                    }, new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
    }

    private async Task ShowMessageBox(MessageBoxDto messageBox)
    {
        while (_popUpIsOpen)
            await Task.Delay(10);

        _popUpIsOpen = true;
        await this.CreateStandardPopup(messageBox.Title, messageBox.Message,
            new PopUpButton[]
            {
                new() { Text = messageBox.ButtonCaption, ActionPress = "ui_accept" }
            },
            new Color { R8 = messageBox.WindowColor.R, G8 = messageBox.WindowColor.G, B8 = messageBox.WindowColor.B, A8 = messageBox.WindowColor.A });

        _popUpIsOpen = false;
    }
    private void OnSoundFinished()
    {
        _soundIsPlaying = false;
        _soundFinished.SetResult(true);
    }
}
#pragma warning restore AsyncFixer03 // Fire-and-forget async-void methods or delegates

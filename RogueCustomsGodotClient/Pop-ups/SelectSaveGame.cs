using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient.Entities;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Utils;

using System;
using System.Drawing;
using System.Linq;
using System.Text;
using static Godot.Control;

public partial class SelectSaveGame : Control
{
    private GlobalState _globalState;
    private InputManager _inputManager;
    private Button _loadButton, _cancelButton;
    private VBoxContainer _saveGameTable;
    private Panel _border;

    private Label _titleLabel;

    private int _selectedIndex;
    
	
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _border = GetNode<Panel>("Border");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        _titleLabel = GetNode<Label>("MarginContainer/VBoxContainer/TitleLabel");
        _saveGameTable = GetNode<VBoxContainer>("ScrollContainer/SaveGameTable");
        _loadButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/LoadButton");
        _cancelButton = GetNode<Button>("MarginContainer/VBoxContainer2/ButtonContainer/CancelButton");

        _loadButton.Pressed += OnLoadButtonPressed;
    }

    public void Show(Action onCancelCallback)
    {
        _titleLabel.Text = TranslationServer.Translate("LoadSaveGameHeaderText");
        _loadButton.Text = TranslationServer.Translate("LoadButtonText");
        _cancelButton.Text = TranslationServer.Translate("CancelButtonText");

        for (int i = 0; i < (int) Math.Ceiling(_globalState.SavedGames.Count / (float) 2); i++)
        {
            var hbox = new HBoxContainer
            {
                Name = $"Row_{i}",
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
            };
            for (int j = i * 2; j < (i * 2) + 2; j++)
            {
                if (j >= _globalState.SavedGames.Count) break;
                hbox.AddChild(BuildCell(_globalState.SavedGames[j].SaveGame, j));
            }
            hbox.AddThemeConstantOverride("separation", 0);
            _saveGameTable.AddChild(hbox);
        }

        _cancelButton.Pressed += () =>
        {
            onCancelCallback?.Invoke();
            QueueFree();
        };

        var screenSize = GetViewportRect().Size;
        Position = (screenSize - _border.Size) / 2;

        SelectCell(int.MinValue);
    }

    private void OnLoadButtonPressed() {
        if (_selectedIndex == int.MinValue) return;
        var selectedSave = _globalState.SavedGames[_selectedIndex];
        _globalState.DungeonManager.LoadSavedDungeon(new DungeonSaveGameDto
        {
            DungeonData = Convert.FromBase64String(selectedSave.SaveGame.DungeonData),
        });
        _globalState.IsHardcoreMode = selectedSave.SaveGame.IsHardcoreMode;
        _globalState.CurrentSavePath = selectedSave.Path;
        GetTree().ChangeSceneToFile("res://Screens/GameScreen.tscn");
    }

    private RichTextLabel BuildCell(SaveGame saveGame, int cellIndex)
    {
        var saveGameLabel = new RichTextLabel { CustomMinimumSize = new(250, 150), Name = $"Cell_{cellIndex}", BbcodeEnabled = true };
        var labelContents = new StringBuilder();

        labelContents.Append($"[center]{saveGame.DungeonName}[/center]");
        labelContents.Append($"[p] [/p][center]{saveGame.PlayerName}[/center][p][center]{saveGame.PlayerRepresentation.ToBbCodeRepresentation()}[/center]");
        labelContents.Append($"[p][center]{TranslationServer.Translate("PlayerLevelText").ToString().Format(new { CurrentLevel = saveGame.PlayerLevel.ToString() })}[/center][p]");
        if (saveGame.IsPlayerDead || saveGame.IsHardcoreMode)
            labelContents.Append("[p] [/p]");
        if (saveGame.IsPlayerDead)
            labelContents.Append($"[center][color=#FF0000FF]{TranslationServer.Translate("DeadPlayerText")}[/color][/center][p]");
        if (saveGame.IsHardcoreMode)
            labelContents.Append($"[center][color=#FF0000FF]{TranslationServer.Translate("HardcoreModeSaveGameText")}[/color][/center][p]");        
        labelContents.Append("[p] [/p]");
        labelContents.Append($"[center][font_size=12]{saveGame.SaveDate.ToShortDateString()} - {saveGame.SaveDate.ToShortTimeString()}[/font_size][/center]");

        saveGameLabel.AddThemeFontSizeOverride("normal_font_size", 14);
        saveGameLabel.AddThemeConstantOverride("text_highlight_v_padding", 0);
        saveGameLabel.AddThemeConstantOverride("text_highlight_h_padding", 0);
        saveGameLabel.AddThemeStyleboxOverride("normal", GlobalConstants.NormalSaveGameCellStyleBox);
        saveGameLabel.GuiInput += (eventArgs) => OnCellClicked(eventArgs, cellIndex);
        saveGameLabel.Text = labelContents.ToString();

        return saveGameLabel;
    }

    private void OnCellClicked(InputEvent @event, int cellIndex)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            SelectCell(cellIndex);
        }
    }

    private void SelectCell(int cellIndex)
    {
        var indexToPick = cellIndex > int.MinValue ? cellIndex % _globalState.SavedGames.Count : int.MinValue;

        if (cellIndex < 0 && cellIndex > int.MinValue)
            indexToPick = 0;

        if (_selectedIndex != int.MinValue)
        {
            if (_selectedIndex == cellIndex && cellIndex == indexToPick)
            {
                _selectedIndex = int.MinValue;
                _loadButton.Disabled = true;
                return;
            }
        }

        if (indexToPick == int.MinValue)
        {
            _selectedIndex = indexToPick;
            _loadButton.Disabled = true;
            return;
        }

        _loadButton.Disabled = false;

        for (int i = 0; i < (int)Math.Ceiling(_globalState.SavedGames.Count / (float)2); i++)
        {
            var hbox = _saveGameTable.GetNode<HBoxContainer>($"Row_{i}");

            foreach (RichTextLabel label in hbox.GetChildren())
            {
                if (label.Name.Equals($"Cell_{indexToPick}"))
                {
                    label.AddThemeStyleboxOverride("normal", GlobalConstants.SelectedSaveGameCellStyleBox); var scrollContainer = _saveGameTable.GetParent() as ScrollContainer;

                    // This code automatically adjust the scroll bar to the currently-selected cell
                    var cellPosition = label.GlobalPosition - _saveGameTable.GlobalPosition;
                    var cellTop = cellPosition.Y;
                    var cellBottom = cellTop + label.Size.Y;

                    var visibleTop = scrollContainer.ScrollVertical;
                    var visibleBottom = visibleTop + scrollContainer.Size.Y;

                    if (cellTop < visibleTop)
                    {
                        scrollContainer.ScrollVertical = (int)cellTop;
                    }
                    else if (cellBottom > visibleBottom)
                    {
                        scrollContainer.ScrollVertical = (int)(cellBottom - scrollContainer.Size.Y);
                    }
                }
                else
                {
                    label.AddThemeStyleboxOverride("normal", GlobalConstants.NormalSaveGameCellStyleBox);
                }
            }
        }
        _selectedIndex = indexToPick;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            if (_selectedIndex == -1)
                SelectCell(0);
            else
                SelectCell(_selectedIndex - 2);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            if (_selectedIndex == -1)
                SelectCell(0);
            else
                SelectCell(_selectedIndex + 2);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_left") || (_inputManager.IsActionAllowed("ui_left") && @event.IsActionPressed("ui_left", true)))
        {
            if (_selectedIndex == -1)
                SelectCell(0);
            else
                SelectCell(_selectedIndex - 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_right") || (_inputManager.IsActionAllowed("ui_right") && @event.IsActionPressed("ui_right", true)))
        {
            if (_selectedIndex == -1)
                SelectCell(0);
            else
                SelectCell(_selectedIndex + 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_accept"))
        {
            _loadButton.GrabFocus();
            _loadButton.EmitSignal("pressed");
            _loadButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_add"))
        {
            _loadButton.GrabFocus();
            _loadButton.EmitSignal("pressed");
            _loadButton.ButtonPressed = true;
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

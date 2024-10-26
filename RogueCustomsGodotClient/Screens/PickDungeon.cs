using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGodotClient.Helpers;

using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.IO;
using System.Linq;
using System.Xml;

using FileAccess = Godot.FileAccess;

public partial class PickDungeon : Control
{
    private Label _titleLabel;
    private Button _pickDungeonButton, _addDungeonButton, _returnToMainMenuButton;
	private VBoxContainer _dungeonTableHeader, _dungeonTable, _noDungeonsTextContainer;
    private RichTextLabel _noDungeonsText;
    private GlobalState _globalState;
    private InputManager _inputManager;
    private FileDialog _openFileDialog;

    private int _selectedIndex = -1;
    private DungeonPickDto SelectedItem => _globalState.PossibleDungeonInfo.Dungeons[_selectedIndex];


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _titleLabel = GetNode<Label>("TitleLabel");
        _dungeonTableHeader = GetNode<VBoxContainer>("DungeonTableHeader");
        _dungeonTable = GetNode<VBoxContainer>("ScrollContainer/DungeonTable");
        _pickDungeonButton = GetNode<Button>("PickDungeonButton");
        _addDungeonButton = GetNode<Button>("AddDungeonButton");
        _returnToMainMenuButton = GetNode<Button>("ReturnToMainMenuButton");
        _noDungeonsTextContainer = GetNode<VBoxContainer>("NoDungeonsTextContainer");
        _noDungeonsText = GetNode<RichTextLabel>("NoDungeonsTextContainer/NoDungeonsText");
        _openFileDialog = GetNode<FileDialog>("OpenFileDialog");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _inputManager = GetNode<InputManager>("/root/InputManager");
        SetUp();
    }

    public void SetUp()
    {
        _titleLabel.Text = TranslationServer.Translate("PickDungeonHeaderText");
        _pickDungeonButton.Text = TranslationServer.Translate("PickButtonText");
        _addDungeonButton.Text = TranslationServer.Translate("AddDungeonButtonText");
        _returnToMainMenuButton.Text = TranslationServer.Translate("ReturnToMainMenuText");

        _openFileDialog.Title = TranslationServer.Translate("AddDungeonDialogTitleText");
        _openFileDialog.OkButtonText = TranslationServer.Translate("AddDungeonOkDialogText");
        _openFileDialog.CancelButtonText = TranslationServer.Translate("AddDungeonCancelDialogText");

        _noDungeonsText.Text = $"[center][color=#FF0000FF]{TranslationServer.Translate("NoDungeonsText")}[/color][/center][p] [p][center]{TranslationServer.Translate("NoLocalDungeonsSubtext")}[/center]";

        if(_globalState.PossibleDungeonInfo.Dungeons.Any())
        {
            _noDungeonsTextContainer.Visible = false;
            _dungeonTable.Visible = true;
            _dungeonTableHeader.Visible = true;

            AddRow(TranslationServer.Translate("DungeonNameHeaderText"), TranslationServer.Translate("AuthorHeaderText"), TranslationServer.Translate("VersionHeaderText"), true);

            for (int i = 0; i < _globalState.PossibleDungeonInfo.Dungeons.Count; i++)
            {
                var dungeon = _globalState.PossibleDungeonInfo.Dungeons[i];
                AddRow(dungeon.Name, dungeon.Author, dungeon.Version, false, i);
            }
        }
        else
        {
            _noDungeonsTextContainer.Visible = true;
            _dungeonTable.Visible = false;
            _dungeonTableHeader.Visible = false;
        }

        _pickDungeonButton.Pressed += PickDungeonButton_Pressed;
        _addDungeonButton.Pressed += AddDungeonButton_Pressed;
        _returnToMainMenuButton.Pressed += ReturnToMainMenuButton_Pressed;
        _openFileDialog.FileSelected += OpenFileDialog_FileSelected;
    }

    private void OpenFileDialog_FileSelected(string path)
    {
        _openFileDialog.Visible = false;
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var error = FileAccess.GetOpenError();
        if(error == Error.Ok)
        {
            var dungeonAlreadyExists = FileAccess.FileExists($"{_globalState.DungeonsFolder}/{Path.GetFileName(path)}");
            if(dungeonAlreadyExists)
            {
                this.CreateStandardPopup(TranslationServer.Translate("AddDungeonOverwriteHeaderText"),
                                            TranslationServer.Translate("AddDungeonOverwritePromptText").ToString().Format(new { FileName = Path.GetFileName(path) }),
                                            new PopUpButton[]
                                            {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () => AddOrOverwriteDungeon(path, true), ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                            }, new Color() { R8 = 255, G8 = 255, B8 = 0, A = 1 });
                return;
            }
            AddOrOverwriteDungeon(path, false);
            return;
        }
        this.CreateStandardPopup(
            TranslationServer.Translate("AddDungeonFailureHeaderText"),
            TranslationServer.Translate("AddDungeonFailureText").ToString(),
            new PopUpButton[]
            {
                    new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" },
            },
            new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });
    }

    private void AddOrOverwriteDungeon(string path, bool isOverwrite)
    {
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var fileContents = file.GetAsText();
        var success = _globalState.DungeonManager.AddDungeonIfPossible(path, fileContents);
        if (success)
        {
            this.CreateStandardPopup(
                isOverwrite ? TranslationServer.Translate("AddDungeonOverwriteSuccessHeaderText") : TranslationServer.Translate("AddDungeonSuccessHeaderText"),
                isOverwrite ? TranslationServer.Translate("AddDungeonOverwriteSuccessText") : TranslationServer.Translate("AddDungeonSuccessText"),
                new PopUpButton[]
                {
                    new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" },
                },
                new Color() { R8 = 0, G8 = 255, B8 = 0, A = 1 });
            _globalState.PossibleDungeonInfo = _globalState.DungeonManager.GetPickableDungeonList(GlobalState.GameLocale);

            foreach (var row in _dungeonTable.GetChildren())
            {
                row.Free();
            }

            _noDungeonsTextContainer.Visible = false;
            _dungeonTable.Visible = true;
            _dungeonTableHeader.Visible = true;
            _dungeonTable.GetChildren().Clear();

            AddRow(TranslationServer.Translate("DungeonNameHeaderText"), TranslationServer.Translate("AuthorHeaderText"), TranslationServer.Translate("VersionHeaderText"), true);

            for (int i = 0; i < _globalState.PossibleDungeonInfo.Dungeons.Count; i++)
            {
                var dungeon = _globalState.PossibleDungeonInfo.Dungeons[i];
                AddRow(dungeon.Name, dungeon.Author, dungeon.Version, false, i);
            }

            SelectRow(-1);
        }
        else
        {
            this.CreateStandardPopup(
                TranslationServer.Translate("AddDungeonFailureHeaderText"),
                TranslationServer.Translate("AddDungeonFailureText").ToString(),
                new PopUpButton[]
                {
                    new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" },
                },
                new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });
        }
    }

    private void AddDungeonButton_Pressed()
    {
        _openFileDialog.Visible = true;
    }

    private void PickDungeonButton_Pressed()
    {
        if (SelectedItem.IsAtCurrentVersion)
        {
            _globalState.MessageScreenType = MessageScreenType.Briefing;
            _globalState.DungeonManager.CreateDungeon(SelectedItem.InternalName, GlobalState.GameLocale);
            GetTree().ChangeSceneToFile("res://Screens/MessageScreen.tscn");
        }
        else
        {
            this.CreateStandardPopup(
                TranslationServer.Translate("IncompatibleDungeonMessageBoxHeader"),
                TranslationServer.Translate("IncompatibleDungeonMessageBoxText").ToString().Format(new { DungeonJsonVersion = SelectedItem.Version, RequiredDungeonJsonVersion = _globalState.PossibleDungeonInfo.CurrentVersion }),
                new PopUpButton[]
                {
                    new() { Text = TranslationServer.Translate("OKButtonText"), Callback = null, ActionPress = "ui_accept" },
                },
                new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 });
        }
    }
    
    private void ReturnToMainMenuButton_Pressed() => GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn");

    private void AddRow(string dungeonName, string author, string version, bool isHeader, int rowIndex = -1)
    {
        var nameLabel = new Label { Text = dungeonName, HorizontalAlignment = isHeader ? HorizontalAlignment.Center : HorizontalAlignment.Left, CustomMinimumSize = new Vector2(800, 30) };
        var authorLabel = new Label { Text = author, HorizontalAlignment = HorizontalAlignment.Center, CustomMinimumSize = new Vector2(300, 30) };
        var versionLabel = new Label { Text = version, HorizontalAlignment = HorizontalAlignment.Center, CustomMinimumSize = new Vector2(200, 30) };

        var hbox = new HBoxContainer
        {
            Name = $"Row_{rowIndex}",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
        };
        hbox.AddThemeConstantOverride("separation", 0);

        if (isHeader)
        {
            nameLabel.AddThemeStyleboxOverride("normal", GlobalConstants.HeaderCellStyleBox);
            nameLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });
            authorLabel.AddThemeStyleboxOverride("normal", GlobalConstants.HeaderCellStyleBox);
            authorLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });
            versionLabel.AddThemeStyleboxOverride("normal", GlobalConstants.HeaderCellStyleBox);
            versionLabel.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });
        }
        else
        {
            nameLabel.AddThemeStyleboxOverride("normal", GlobalConstants.NormalCellStyleBox);
            nameLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            authorLabel.AddThemeStyleboxOverride("normal", GlobalConstants.NormalCellStyleBox);
            authorLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
            versionLabel.AddThemeStyleboxOverride("normal", GlobalConstants.NormalCellStyleBox);
            versionLabel.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }

        hbox.AddChild(nameLabel);
        hbox.AddChild(authorLabel);
        hbox.AddChild(versionLabel);

        if (isHeader)
            _dungeonTableHeader.AddChild(hbox);
        else
        {
            hbox.GuiInput += (eventArgs) => OnRowClicked(eventArgs, rowIndex);
            _dungeonTable.AddChild(hbox);
        }
    }
    private void OnRowClicked(InputEvent @event, int rowIndex)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            SelectRow(rowIndex);
        }
    }

    private void SelectRow(int rowIndex)
    {
        if (_selectedIndex != -1)
        {
            DeselectRow(_selectedIndex);
            if (_selectedIndex == rowIndex)
            {
                _pickDungeonButton.Disabled = true;
                return;
            }
        }
        
        if(rowIndex == -1)
        {
            _selectedIndex = rowIndex;
            _pickDungeonButton.Disabled = true;
            return;
        }

        _pickDungeonButton.Disabled = false;

        var hbox = _dungeonTable.GetNode<HBoxContainer>($"Row_{rowIndex}");
        foreach (Label label in hbox.GetChildren())
        {
            label.AddThemeStyleboxOverride("normal", GlobalConstants.SelectedCellStyleBox);
            label.AddThemeColorOverride("font_color", new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 });
        }
        _selectedIndex = rowIndex;
    }

    private void DeselectRow(int rowIndex)
    {
        if (rowIndex == -1) return;
        var hbox = _dungeonTable.GetNode<HBoxContainer>($"Row_{rowIndex}");
        foreach (Label label in hbox.GetChildren())
        {
            label.AddThemeStyleboxOverride("normal", GlobalConstants.NormalCellStyleBox);
            label.AddThemeColorOverride("font_color", new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 });
        }
        _selectedIndex = -1;
    }

    public override void _Input(InputEvent @event)
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        if (@event.IsActionPressed("ui_up") || (_inputManager.IsActionAllowed("ui_up") && @event.IsActionPressed("ui_up", true)))
        {
            if (_selectedIndex == -1)
                SelectRow(0);
            else if (_selectedIndex == 0)
                SelectRow(_globalState.PossibleDungeonInfo.Dungeons.Count - 1);
            else
                SelectRow(_selectedIndex - 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_down") || (_inputManager.IsActionAllowed("ui_down") && @event.IsActionPressed("ui_down", true)))
        {
            if (_selectedIndex == -1)
                SelectRow(0);
            else if (_selectedIndex == _globalState.PossibleDungeonInfo.Dungeons.Count - 1)
                SelectRow(0);
            else
                SelectRow(_selectedIndex + 1);
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_accept"))
        {
            _pickDungeonButton.GrabFocus();
            _pickDungeonButton.EmitSignal("pressed");
            _pickDungeonButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_add"))
        {
            _addDungeonButton.GrabFocus();
            _addDungeonButton.EmitSignal("pressed");
            _addDungeonButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            _returnToMainMenuButton.GrabFocus();
            _returnToMainMenuButton.EmitSignal("pressed");
            _returnToMainMenuButton.ButtonPressed = true;
            AcceptEvent();
        }
    }
}

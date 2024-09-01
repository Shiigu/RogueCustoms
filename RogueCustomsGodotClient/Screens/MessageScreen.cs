using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.Linq;

public partial class MessageScreen : Control
{
    private Label _titleLabel;
    private ScrollContainer _scrollContainer;
    private RichTextLabel _messagelabel;
    private Button _pressEnterToContinueButton;
    private GlobalState _globalState;

    private readonly string briefingMessageHeaderText = TranslationServer.Translate("BriefingMessageHeader");
    private readonly Color briefingColor = new(255, 255, 255);

    private readonly string theEndMessageHeaderText = TranslationServer.Translate("TheEndMessageHeader");
    private readonly Color theEndColor = new(0, 255, 0);

    private readonly string errorMessageHeaderText = TranslationServer.Translate("ErrorMessageHeader");
    private readonly Color errorColor = new(255, 0, 0);

    private readonly string pressEnterToContinueText = TranslationServer.Translate("PressEnterText");

    private CharacterClassDto _chosenClass;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _titleLabel = GetNode<Label>("TitleLabel");
        _scrollContainer = GetNode<ScrollContainer>("ScrollContainer");
        _messagelabel = GetNode<RichTextLabel>("ScrollContainer/MessageLabel");
        _pressEnterToContinueButton = GetNode<Button>("PressEnterToContinueButton");
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        SetUp();
    }

    public void SetUp()
    {
        var messageText = string.Empty;

        switch(_globalState.MessageScreenType)
        {
            case MessageScreenType.Briefing:
                _titleLabel.Text = briefingMessageHeaderText;
                _titleLabel.Modulate = briefingColor;
                messageText = _globalState.DungeonManager.GetDungeonWelcomeMessage(_globalState.DungeonId);
                break;
            case MessageScreenType.Ending:
                _titleLabel.Text = theEndMessageHeaderText;
                _titleLabel.Modulate = theEndColor;
                messageText = _globalState.DungeonManager.GetDungeonEndingMessage(_globalState.DungeonId);
                break;
            case MessageScreenType.Error:
                _titleLabel.Text = errorMessageHeaderText;
                _titleLabel.Modulate = errorColor;
                messageText = TranslationServer.Translate("ErrorText");
                break;
        }
        _pressEnterToContinueButton.Text = TranslationServer.Translate("PressEnterText");
        _pressEnterToContinueButton.Pressed += PressEnterToContinueButton_Pressed;

        _messagelabel.Text = "";
        _messagelabel.BbcodeEnabled = true;
        var messageLines = messageText.Split('\n');
        for (int i = 0; i < messageLines.Length; i++)
        {
            _messagelabel.AppendText("[p]");
            _messagelabel.AppendText(messageLines[i]);
            if (string.IsNullOrEmpty(messageLines[i]))
                _messagelabel.AppendText(" ");
        }

        _scrollContainer.GrabFocus();
    }

    private void PressEnterToContinueButton_Pressed() {

        _pressEnterToContinueButton.ReleaseFocus();
        switch (_globalState.MessageScreenType)
        {
            case MessageScreenType.Briefing:
                AskForPlayerClass();
                break;
            case MessageScreenType.Ending:
            case MessageScreenType.Error:
                GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn");
                break;
        }
    }

    private void AskForPlayerClass()
    {
        var availablePlayerClasses = _globalState.DungeonManager.GetPlayerClassSelection(_globalState.DungeonId).CharacterClasses;
        if (availablePlayerClasses != null && availablePlayerClasses.Count > 1)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = GetViewportRect().Size
            };
            AddChild(overlay);
            this.CreateSelectClassWindow(
            (classId) =>
            {
                _chosenClass = availablePlayerClasses.First(c => c.ClassId.Equals(classId));
                AskForPlayerName();
            },
            () => this.CreateStandardPopup(TranslationServer.Translate("PlayerClassWindowTitleText"),
                                    TranslationServer.Translate("ExitPromptText"),
                                    new PopUpButton[]
                                    {
                                        new() { Text = TranslationServer.Translate("YesButtonText"), Callback = () => {
                                                overlay.QueueFree();
                                                GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn");
                                            }, ActionPress = "ui_accept" },
                                        new() { Text = TranslationServer.Translate("NoButtonText"), Callback = null, ActionPress = "ui_cancel" }
                                    }, new Color() { R8 = 255, G8 = 0, B8 = 0, A = 1 }));
        }
        else
        {
            _chosenClass = availablePlayerClasses.First();
            AskForPlayerName();
        }
    }

    private void AskForPlayerName()
    {
        if (_chosenClass != null)
        {
            if (!_chosenClass.RequiresNamePrompt)
            {
                SendClassSelection(_chosenClass.ClassId, _chosenClass.Name);
            }
            else
            {
                this.CreateInputBox(TranslationServer.Translate("InputBoxTitleText"), TranslationServer.Translate("InputBoxPromptText").ToString().Format(new { ClassName = _chosenClass.Name }), _chosenClass.Name, new Color() { R8 = 255, G8 = 200, B8 = 0, A = 1},
                    (chosenName) => {
                        SendClassSelection(_chosenClass.ClassId, chosenName); 
                    }, () => {});
            }
        }
    }

    private void SendClassSelection(string classId, string name)
    {
        _globalState.DungeonManager.SetPlayerClassSelection(_globalState.DungeonId, new PlayerClassSelectionInput
        {
            ClassId = classId,
            Name = name,
        });
        GetTree().ChangeSceneToFile("res://Screens/GameScreen.tscn");
    }

    public override void _Input(InputEvent @event)
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
        if (@event.IsActionPressed("ui_accept"))
        {
            _pressEnterToContinueButton.GrabFocus();
            _pressEnterToContinueButton.EmitSignal("pressed");
            _pressEnterToContinueButton.ButtonPressed = true;
            AcceptEvent();
        }
        else if (@event.IsActionPressed("ui_cancel"))
        {
            GetTree().ChangeSceneToFile("res://Screens/MainMenu.tscn");
            AcceptEvent();
        }
    }
}

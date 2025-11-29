using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

using System;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable AsyncFixer03 // Fire-and-forget async-void methods or delegates
public partial class MessageScreen : Control
{
    private ExceptionLogger _exceptionLogger;
    private Label _titleLabel;
    private ScrollContainer _scrollContainer;
    private RichTextLabel _messagelabel;
    private Button _pressEnterToContinueButton;
    private GlobalState _globalState;

    private readonly string briefingMessageHeaderText = TranslationServer.Translate("BriefingMessageHeader");
    private readonly Color briefingColor = new() { R8 = 255, G8 = 255, B8 = 255, A8 = 255 };

    private readonly string theEndMessageHeaderText = TranslationServer.Translate("TheEndMessageHeader");
    private readonly Color theEndColor = new() { R8 = 0, G8 = 255, B8 = 0, A8 = 255 };

    private readonly string errorMessageHeaderText = TranslationServer.Translate("ErrorMessageHeader");
    private readonly Color errorColor = new() { R8 = 255, G8 = 0, B8 = 0, A8 = 255 };

    private readonly string pressEnterToContinueText = TranslationServer.Translate("PressEnterText");

    private CharacterClassDto _chosenClass;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
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
                try
                {
                    _titleLabel.Text = briefingMessageHeaderText;
                    _titleLabel.Modulate = briefingColor;
                    messageText = _globalState.DungeonManager.GetDungeonWelcomeMessage();
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
                break;
            case MessageScreenType.Ending:
                try
                {
                    _titleLabel.Text = theEndMessageHeaderText;
                    _titleLabel.Modulate = theEndColor;
                    messageText = _globalState.DungeonManager.GetDungeonEndingMessage();
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogMessage(ex);
                }
                break;
            case MessageScreenType.Error:
                _titleLabel.Text = errorMessageHeaderText;
                _titleLabel.Modulate = errorColor;
                messageText = TranslationServer.Translate("ErrorText");
                break;
        }
        _pressEnterToContinueButton.Text = TranslationServer.Translate("PressEnterText");
        _pressEnterToContinueButton.Pressed += PressEnterToContinueButton_Pressed;

        _messagelabel.Text = messageText.ToBbCodeAppropriateString();

        _scrollContainer.GrabFocus();
    }

    private void PressEnterToContinueButton_Pressed()
    {
        if (GetChildren().Any(c => c.IsPopUp())) return;
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

    private async Task AskForPlayerClass()
    {
        var availablePlayerClasses = _globalState.DungeonManager.GetPlayerClassSelection().CharacterClasses;
        if (availablePlayerClasses != null && availablePlayerClasses.Count > 1)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = GetViewportRect().Size
            };
            AddChild(overlay);
            this.CreateSelectClassWindow(
            async (classId) =>
            {
                _chosenClass = availablePlayerClasses.First(c => c.ClassId.Equals(classId));
                await AskForPlayerName();
            },
            async () => await this.CreateStandardPopup(TranslationServer.Translate("PlayerClassWindowTitleText"),
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
            await AskForPlayerName();
        }
    }

    private async Task AskForPlayerName()
    {
        if (_chosenClass != null)
        {
            if (!_chosenClass.RequiresNamePrompt)
            {
                SendClassSelection(_chosenClass.ClassId, _chosenClass.Name);
            }
            else
            {
                await this.CreateInputBox(TranslationServer.Translate("InputBoxTitleText"), TranslationServer.Translate("InputBoxPromptText").ToString().Format(new { ClassName = _chosenClass.Name }), _chosenClass.Name, true, new Color() { R8 = 255, G8 = 200, B8 = 0, A = 1},
                    (chosenName) => {
                        SendClassSelection(_chosenClass.ClassId, chosenName); 
                    }, () => {});
            }
        }
    }

    private void SendClassSelection(string classId, string name)
    {
        _globalState.DungeonManager.SetPlayerClassSelection(new PlayerClassSelectionInput
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
#pragma warning restore AsyncFixer03 // Fire-and-forget async-void methods or delegates

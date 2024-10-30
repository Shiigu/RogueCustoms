using Godot;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Screens.GameSubScreens;

using System;
using System.Text;

public partial class MessageLogPanel : GamePanel
{
    private GlobalState _globalState;
    private Label _messageTitleLabel;
    private RichTextLabel _messageLogLabel;
    private StringBuilder _logContents;
    public Button MessageWindowButton { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _messageTitleLabel = GetNode<Label>("TitleContainer/MessagesTitleLabel");
        _messageLogLabel = GetNode<RichTextLabel>("MessageContainer/MessageLogLabel");
        MessageWindowButton = GetNode<Button>("ButtonContainer/MessageLogButton");
        SetUp();
    }
    private void SetUp()
    {
        _messageTitleLabel.Text = TranslationServer.Translate("MessageHeaderText");
        MessageWindowButton.Text = TranslationServer.Translate("MessageLogButtonText");
        MessageWindowButton.Pressed += MessageWindowButton_Pressed;
    }

    private void MessageWindowButton_Pressed()
    {
        (GetParent() as Control)?.CreateScrollablePopup(TranslationServer.Translate("MessageWindowTitleText"), _logContents.ToString(), new Color { R8 = 200, G8 = 100, B8 = 200, A8 = 255 }, true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Clear()
    {
        _messageLogLabel.Text = "";
    }

    public void Append(MessageDto message)
    {
        _logContents = new StringBuilder(_messageLogLabel.Text);
        _logContents.Append($"{MessageToBbCodeString(message)}[p]");
        _messageLogLabel.Text = _logContents.ToString();
    }

    private static string MessageToBbCodeString(MessageDto message)
    {
        return $"[bgcolor=#{message.BackgroundColor.R:X2}{message.BackgroundColor.G:X2}{message.BackgroundColor.B:X2}{message.BackgroundColor.A:X2}][color=#{message.ForegroundColor.R:X2}{message.ForegroundColor.G:X2}{message.ForegroundColor.B:X2}{message.ForegroundColor.A:X2}]{message.Message}[/color][/bgcolor]";
    }

    public override void Update()
    {
        // Do nothing
    }
}

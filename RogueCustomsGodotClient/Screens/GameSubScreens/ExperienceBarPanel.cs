using Godot;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Screens.GameSubScreens;

using System;

public partial class ExperienceBarPanel : GamePanel
{
    private GlobalState _globalState;
    private Label _experienceAmountLabel;
    private TextureProgressBar _experienceBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _experienceBar = GetNode<TextureProgressBar>("ExperienceBarContainer/ExperienceBar");
        _experienceAmountLabel = GetNode<Label>("ExperienceBarContainer/ExperienceAmountLabel");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void Update()
    {
        var dungeonStatus = _globalState.DungeonInfo;
        var playerEntity = dungeonStatus.PlayerEntity;
        if (playerEntity == null) return;

        UpdateExperienceBar(playerEntity.Experience, playerEntity.ExperienceToLevelUp, playerEntity.CurrentExperiencePercentage);
    }

    public void UpdateExperienceBar(int experience, int experienceToLevelUp, int currentExperiencePercentage)
    {
        var displayText = TranslationServer.Translate("ExperienceBarDisplayText").ToString().Format(new
        {
            CurrentExperience = experience.ToString(),
            ExperienceToLevelUp = experienceToLevelUp.ToString(),
            Percentage = currentExperiencePercentage.ToString()
        });

        _experienceAmountLabel.Text = displayText;

        _experienceBar.MaxValue = experienceToLevelUp;
        _experienceBar.Value = experience;
    }
}

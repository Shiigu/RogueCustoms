using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Screens.GameSubScreens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class InfoPanel : GamePanel
{
    private GlobalState _globalState;
    private Label _infoTitleLabel;

    private Label _playerNameLabel, _levelLabel;
    private RichTextLabel _playerRepresentationLabel;

    private Label _hpNameLabel, _hpAmountLabel;
    private TextureProgressBar _hpBar;

    private Label _mpNameLabel, _mpAmountLabel;
    private TextureProgressBar _mpBar;
    private MarginContainer _mpBarContainer;

    private Label _hungerNameLabel, _hungerAmountLabel;
    private TextureProgressBar _hungerBar;
    private VBoxContainer _hungerContainer;

    private Label _weaponHeaderLabel;
    private RichTextLabel _weaponNameLabel, _damageNumberLabel;

    private Label _armorHeaderLabel;
    private RichTextLabel _armorNameLabel, _mitigationNumberLabel;

    private RichTextLabel _movementLabel, _accuracyLabel, _evasionLabel;

    private Label _alteredStatusesHeaderLabel;
    private RichTextLabel _alteredStatusesIconsLabel;

    private Label _inventoryHeaderLabel;
    private RichTextLabel _inventoryIconsLabel;

    private string _tooManyText;
    private const int MaxIconsPerRow = 12;

    public Button DetailsButton { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _infoTitleLabel = GetNode<Label>("VBoxContainer/InfoTitleLabel");
        _playerNameLabel = GetNode<Label>("VBoxContainerContainer/PlayerDescriptorContainer/PlayerNameLabel");
        _playerRepresentationLabel = GetNode<RichTextLabel>("VBoxContainerContainer/PlayerDescriptorContainer/PlayerRepresentationLabel");
        _levelLabel = GetNode<Label>("VBoxContainerContainer/PlayerDescriptorContainer/PlayerLevelLabel");
        _hpNameLabel = GetNode<Label>("VBoxContainerContainer/PlayerBarsContainer/HPNameLabel");
        _hpBar = GetNode<TextureProgressBar>("VBoxContainerContainer/PlayerBarsContainer/HPBarContainer/HPBar");
        _hpAmountLabel = GetNode<Label>("VBoxContainerContainer/PlayerBarsContainer/HPBarContainer/HPAmountLabel");
        _mpNameLabel = GetNode<Label>("VBoxContainerContainer/PlayerBarsContainer/MPNameLabel");
        _mpBarContainer = GetNode<MarginContainer>("VBoxContainerContainer/PlayerBarsContainer/MPBarContainer");
        _mpBar = GetNode<TextureProgressBar>("VBoxContainerContainer/PlayerBarsContainer/MPBarContainer/MPBar");
        _mpAmountLabel = GetNode<Label>("VBoxContainerContainer/PlayerBarsContainer/MPBarContainer/MPAmountLabel");
        _hungerContainer = GetNode<VBoxContainer>("VBoxContainerContainer/HungerContainer");
        _hungerNameLabel = GetNode<Label>("VBoxContainerContainer/HungerContainer/HungerNameLabel");
        _hungerBar = GetNode<TextureProgressBar>("VBoxContainerContainer/HungerContainer/HungerBarContainer/HungerBar");
        _hungerAmountLabel = GetNode<Label>("VBoxContainerContainer/HungerContainer/HungerBarContainer/HungerAmountLabel");
        _weaponHeaderLabel = GetNode<Label>("VBoxContainerContainer/WeaponContainer/WeaponHeaderLabel");
        _weaponNameLabel = GetNode<RichTextLabel>("VBoxContainerContainer/WeaponContainer/WeaponNameLabel");
        _damageNumberLabel = GetNode<RichTextLabel>("VBoxContainerContainer/WeaponContainer/DamageNumberLabel");
        _armorHeaderLabel = GetNode<Label>("VBoxContainerContainer/ArmorContainer/ArmorHeaderLabel");
        _armorNameLabel = GetNode<RichTextLabel>("VBoxContainerContainer/ArmorContainer/ArmorNameLabel");
        _mitigationNumberLabel = GetNode<RichTextLabel>("VBoxContainerContainer/ArmorContainer/MitigationNumberLabel");
        _movementLabel = GetNode<RichTextLabel>("VBoxContainerContainer/OtherStatsContainer/MovementLabel");
        _accuracyLabel = GetNode<RichTextLabel>("VBoxContainerContainer/OtherStatsContainer/AccuracyLabel");
        _evasionLabel = GetNode<RichTextLabel>("VBoxContainerContainer/OtherStatsContainer/EvasionLabel");
        _alteredStatusesHeaderLabel = GetNode<Label>("VBoxContainerContainer/AlteredStatusesContainer/AlteredStatusesHeaderLabel");
        _alteredStatusesIconsLabel = GetNode<RichTextLabel>("VBoxContainerContainer/AlteredStatusesContainer/AlteredStatusesIconsLabel");
        _inventoryHeaderLabel = GetNode<Label>("VBoxContainerContainer/InventoryContainer/InventoryHeaderLabel");
        _inventoryIconsLabel = GetNode<RichTextLabel>("VBoxContainerContainer/InventoryContainer/InventoryIconsLabel");
        DetailsButton = GetNode<Button>("VBoxContainerContainer/ButtonContainer/DetailsButton");
        SetUp();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void SetUp()
    {
        DetailsButton.Text = TranslationServer.Translate("DetailsButtonText");
        _infoTitleLabel.Text = TranslationServer.Translate("PlayerInfoConsoleTitle");
        _tooManyText = $"[center][color=#FFFF00FF]{TranslationServer.Translate("PlayerInfoTooManyText")}[/color][/center]";
        DetailsButton.Pressed += DetailsButton_Pressed;
    }

    private void DetailsButton_Pressed()
    {
        var playerInfo = _globalState.DungeonManager.GetPlayerDetailInfo(_globalState.DungeonId);
        if (playerInfo == null) return;
        var titleText = TranslationServer.Translate("PlayerCharacterDetailWindowTitleText").ToString().Format(new { PlayerName = playerInfo.Name});
        var innerText = new StringBuilder();

        AddPlayerLevelInfo(innerText, playerInfo);

        innerText.Append($"[center]{TranslationServer.Translate("PlayerCharacterDetailStatsHeader")}[/center][p] [p]");
        playerInfo.Stats.ForEach(stat => AddPlayerStatsInfo(innerText, stat));
        AddPlayerAlteredStatusesInfo(innerText, playerInfo.AlteredStatuses);
        AddPlayerEquippedItemInfo(innerText, TranslationServer.Translate("PlayerCharacterDetailEquippedWeaponHeader"), playerInfo.WeaponInfo);
        AddPlayerEquippedItemInfo(innerText, TranslationServer.Translate("PlayerCharacterDetailEquippedArmorHeader"), playerInfo.ArmorInfo);
        AddPlayerInventoryInfo(innerText, TranslationServer.Translate("PlayerCharacterDetailInventoryHeader"), playerInfo.Inventory);

        Parent?.CreateScrollablePopup(titleText, innerText.ToString(), new Color { R8 = 200, G8 = 100, B8 = 200, A8 = 255 }, false);
    }
    
    public override void Update()
	{
        var dungeonStatus = _globalState.DungeonInfo;
        var playerEntity = dungeonStatus.PlayerEntity;
        if (playerEntity == null) return;
        _playerNameLabel.Text = playerEntity.Name;
        _playerRepresentationLabel.Text = $"[center]{playerEntity.ConsoleRepresentation.ToBbCodeRepresentation()}[/center]";
        _levelLabel.Text = TranslationServer.Translate("PlayerLevelText").ToString().Format(new { CurrentLevel = playerEntity.Level.ToString() });
        _hpNameLabel.Text = playerEntity.HPStatName;
        _hpBar.MaxValue = playerEntity.MaxHP;
        _hpBar.Value = playerEntity.HP;
        _hpAmountLabel.Text = $"{playerEntity.HP}/{playerEntity.MaxHP}";
        if(playerEntity.UsesMP)
        {
            _mpBarContainer.Visible = true;
            _mpNameLabel.Visible = true;
            _mpNameLabel.Text = playerEntity.MPStatName;
            _mpBar.MaxValue = playerEntity.MaxMP;
            _mpBar.Value = playerEntity.MP;
            _mpAmountLabel.Text = $"{playerEntity.MP}/{playerEntity.MaxMP}";
        }
        else
        {
            _mpBarContainer.Visible = false;
            _mpNameLabel.Visible = false;
        }
        if (playerEntity.UsesHunger)
        {
            _hungerContainer.Visible = true;
            _hungerNameLabel.Text = playerEntity.HungerStatName;
            _hungerBar.MaxValue = playerEntity.MaxHunger;
            _hungerBar.Value = playerEntity.Hunger;
            _hungerAmountLabel.Text = $"{playerEntity.Hunger}/{playerEntity.MaxHunger}";
        }
        else
        {
            _hungerContainer.Visible = false;
        }

        _weaponHeaderLabel.Text = TranslationServer.Translate("PlayerInfoWeaponHeader");
        _weaponNameLabel.Text = $"[center]{playerEntity.Weapon.ConsoleRepresentation.ToBbCodeRepresentation()} - {playerEntity.Weapon.Name}[/center]";
        _damageNumberLabel.Text = $"[center]{playerEntity.DamageStatName}: {GetColorizedItemInfluencedStat(playerEntity.WeaponDamage, playerEntity.Attack)}[/center]";

        _armorHeaderLabel.Text = TranslationServer.Translate("PlayerInfoArmorHeader");
        _armorNameLabel.Text = $"[center]{playerEntity.Armor.ConsoleRepresentation.ToBbCodeRepresentation()} - {playerEntity.Armor.Name}[/center]";
        _mitigationNumberLabel.Text = $"[center]{playerEntity.MitigationStatName}: {GetColorizedItemInfluencedStat(playerEntity.ArmorMitigation, playerEntity.Defense)}[/center]";

        if(playerEntity.Movement > playerEntity.BaseMovement)
            _movementLabel.Text = $"[center]{playerEntity.MovementStatName}: [color=#00FF00FF]{playerEntity.Movement}[/color][/center]";
        else if (playerEntity.Movement < playerEntity.BaseMovement)
            _movementLabel.Text = $"[center]{playerEntity.MovementStatName}: [color=#FF0000FF]{playerEntity.Movement}[/color][/center]";
        else
            _movementLabel.Text = $"[center]{playerEntity.MovementStatName}: [color=#FFFFFFFF]{playerEntity.Movement}[/color][/center]";

        if (playerEntity.Accuracy > playerEntity.BaseAccuracy)
            _accuracyLabel.Text = $"[center]{playerEntity.AccuracyStatName}: [color=#00FF00FF]{playerEntity.Accuracy:F0}%[/color][/center]";
        else if (playerEntity.Accuracy < playerEntity.BaseAccuracy)
            _accuracyLabel.Text = $"[center]{playerEntity.AccuracyStatName}: [color=#FF0000FF]{playerEntity.Accuracy:F0}%[/color][/center]";
        else
            _accuracyLabel.Text = $"[center]{playerEntity.AccuracyStatName}: [color=#FFFFFFFF]{playerEntity.Accuracy:F0}%[/color][/center]";

        if (playerEntity.Evasion > playerEntity.BaseEvasion)
            _evasionLabel.Text = $"[center]{playerEntity.EvasionStatName}: [color=#00FF00FF]{playerEntity.Evasion:F0}%[/color][/center]";
        else if (playerEntity.Evasion < playerEntity.BaseEvasion)
            _evasionLabel.Text = $"[center]{playerEntity.EvasionStatName}: [color=#FF0000FF]{playerEntity.Evasion:F0}%[/color][/center]";
        else
            _evasionLabel.Text = $"[center]{playerEntity.EvasionStatName}: [color=#FFFFFFFF]{playerEntity.Evasion:F0}%[/color][/center]";

        _alteredStatusesHeaderLabel.Text = TranslationServer.Translate("PlayerInfoStatusesHeader");
        _alteredStatusesIconsLabel.Text = "";
        _alteredStatusesIconsLabel.PushParagraph(HorizontalAlignment.Center);
        if (playerEntity.AlteredStatuses.Any())
        {
            if(playerEntity.AlteredStatuses.Count < MaxIconsPerRow)
            {
                foreach (var alteredStatus in playerEntity.AlteredStatuses)
                {
                    _alteredStatusesIconsLabel.AppendText(alteredStatus.ConsoleRepresentation.ToBbCodeRepresentation());
                }
            }
            else
            {
                _alteredStatusesIconsLabel.AppendText(TranslationServer.Translate("PlayerInfoTooManyText"));
            }
        }
        else
        {
            _alteredStatusesIconsLabel.AppendText(TranslationServer.Translate("PlayerHasNothingText"));
        }
        _alteredStatusesIconsLabel.PopAll();

        _inventoryHeaderLabel.Text = TranslationServer.Translate("PlayerInfoInventoryHeader");
        _inventoryIconsLabel.Text = "";
        _inventoryIconsLabel.PushParagraph(HorizontalAlignment.Center);
        if (playerEntity.Inventory.Any())
        {
            if (playerEntity.Inventory.Count < MaxIconsPerRow)
            {
                foreach (var item in playerEntity.Inventory)
                {
                    _inventoryIconsLabel.AppendText(item.ConsoleRepresentation.ToBbCodeRepresentation());
                }
            }
            else
            {
                _inventoryIconsLabel.AppendText(TranslationServer.Translate("PlayerInfoTooManyText"));
            }
        }
        else
        {
            _inventoryIconsLabel.AppendText(TranslationServer.Translate("PlayerHasNothingText"));
        }
        _inventoryIconsLabel.PopAll();
    }

    private static string GetColorizedItemInfluencedStat(string itemStat, int playerStat)
    {
        if (playerStat > 0)
            return $"[color=#FF00FFFF]{itemStat}[/color][color=#00FF00FF]+{playerStat}[/color]";
        else if (playerStat == 0)
            return $"[color=#FF00FFFF]{itemStat}[/color][color=#FFFFFFFF]+{playerStat}[/color]";
        return $"[color=#FF00FFFF]{itemStat}[/color][color=#FF0000FF]-{Math.Abs(playerStat)}[/color]";
    }

    #region Construct Details pop-up

    private static void AddPlayerLevelInfo(StringBuilder innerText, PlayerInfoDto playerInfo)
    {
        innerText.Append($"{TranslationServer.Translate("PlayerLevelText").ToString().Format(new { CurrentLevel = playerInfo.Level.ToString() })}[p] [p]");
        innerText.Append($"{TranslationServer.Translate("PlayerCharacterDetailCurrentExperienceText").ToString().Format(new { CurrentExperience = playerInfo.CurrentExperience.ToString() })}[p]");
        string experienceText;
        if (!playerInfo.IsAtMaxLevel)
        {
            experienceText = TranslationServer.Translate("PlayerCharacterDetailExperienceToLevelUpText").ToString().Format(new
            {
                NextLevel = playerInfo.Level + 1,
                RequiredExperience = playerInfo.ExperienceToNextLevel.ToString()
            });
        }
        else
        {
            experienceText = TranslationServer.Translate("PlayerCharacterDetailAtMaxLevelText");
        }
        innerText.Append($"{experienceText}[p] [p]");
    }

    private static void AddPlayerStatsInfo(StringBuilder innerText, StatDto stat)
    {
        if (!stat.Visible) return;
        innerText.Append("[p] [p]");

        if (stat.IsPercentileStat)
        {
            // Percentile stats will never have a Max
            innerText.Append($"{stat.Name}: {(int)stat.Current}%");
        }
        else
        {
            if (stat.HasMaxStat && stat.Max != null && !stat.IsDecimalStat)
                innerText.Append($"{stat.Name}: {stat.Current:0.#####}/{stat.Max:0.#####}");
            else if (stat.HasMaxStat && stat.Max != null && stat.IsDecimalStat)
                innerText.Append($"{stat.Name}: {(int)stat.Current}/{(int)stat.Max}");
            else if (!stat.HasMaxStat && stat.IsDecimalStat)
                innerText.Append($"{stat.Name}: {stat.Current:0.#####}");
            else if (!stat.HasMaxStat && !stat.IsDecimalStat)
                innerText.Append($"{stat.Name}: {(int)stat.Current}");
        }
        innerText.Append("[p]");
        AddStatDetails(innerText, stat);
        stat.Modifications.ForEach(mhm =>
        {
            innerText.Append("[p]     ");
            string modificationAmountText, sourceDisplayText;
            string modificationDisplayForegroundColor = "";
            sourceDisplayText = mhm.Source;

            if (stat.IsDecimalStat)
                modificationAmountText = $"{mhm.Amount:+0.#####;-0.#####;0}";
            else if (stat.IsPercentileStat)
                modificationAmountText = $"{mhm.Amount:+0;-0;0}%";
            else
                modificationAmountText = $"{mhm.Amount:+0;-0;0}";

            if (mhm.Amount > 0)
                modificationDisplayForegroundColor = "#00FF00FF";
            else if (mhm.Amount < 0)
                modificationDisplayForegroundColor = "#FF0000FF";

            var modificationText = TranslationServer.Translate("PlayerCharacterDetailStatAlterationText").ToString().Format(new
            {
                Alteration = modificationAmountText,
                Source = sourceDisplayText
            });

            innerText.Append($"[color={modificationDisplayForegroundColor}]{modificationText}[/color]");
        });
    }
    private static void AddStatDetails(StringBuilder innerText, StatDto stat)
    {
        string baseText = stat.HasMaxStat ? TranslationServer.Translate("PlayerCharacterDetailMaxBaseText") : TranslationServer.Translate("PlayerCharacterDetailBaseText");
        string baseValue = stat.IsDecimalStat ? stat.Base.ToString("0.#####") : ((int)stat.Base).ToString();
        string percentageIfNeeded = stat.IsPercentileStat ? "%" : "";

        string statDetail = $"[p]     {baseText}: {baseValue}{percentageIfNeeded}";

        innerText.Append(statDetail);
    }
    private static void AddPlayerAlteredStatusesInfo(StringBuilder innerText, List<AlteredStatusDetailDto> alteredStatuses)
    {
        innerText.Append("[p] [p]");
        innerText.Append("[p] [p]");
        innerText.Append($"[center]{TranslationServer.Translate("PlayerCharacterDetailAlteredStatusesHeaderText")}[/center]");
        innerText.Append("[p] [p]");
        if (!alteredStatuses.Any())
        {
            innerText.Append($"[center]{TranslationServer.Translate("PlayerHasNothingText")}[/center]");
        }
        else
        {
            alteredStatuses.ForEach(als =>
            {
                innerText.Append("[p]");
                innerText.Append(als.ConsoleRepresentation.ToBbCodeRepresentation());
                string statusDescriptionText = als.RemainingTurns > 0
                    ? TranslationServer.Translate("PlayerCharacterDetailAlteredStatusDescriptionTextWithTurns").ToString().Format(new
                    {
                        StatusName = als.Name,
                        StatusDescription = als.Description,
                        RemainingTurns = als.RemainingTurns
                    })
                    : TranslationServer.Translate("PlayerCharacterDetailAlteredStatusDescriptionText").ToString().Format(new
                    {
                        StatusName = als.Name,
                        StatusDescription = als.Description
                    });
                var backgroundColor = new Color { R8 = als.ConsoleRepresentation.BackgroundColor.R, G8 = als.ConsoleRepresentation.BackgroundColor.G, B8 = als.ConsoleRepresentation.BackgroundColor.B, A8 = als.ConsoleRepresentation.BackgroundColor.A };
                var foregroundColor = new Color { R8 = als.ConsoleRepresentation.ForegroundColor.R, G8 = als.ConsoleRepresentation.ForegroundColor.G, B8 = als.ConsoleRepresentation.ForegroundColor.B, A8 = als.ConsoleRepresentation.ForegroundColor.A };
                innerText.Append($" [bgcolor=#{backgroundColor.ToHtml()}][color=#{foregroundColor.ToHtml()}]{statusDescriptionText}[/color][/bgcolor]");
            });
        }
    }

    private static void AddPlayerEquippedItemInfo(StringBuilder innerText, string itemTypeHeader, ItemDetailDto item)
    {
        innerText.Append("[p] [p]");
        innerText.Append($"[center]{itemTypeHeader}[/center]");
        innerText.Append("[p] [p]");
        innerText.Append($"[center]{item.Name} - {item.ConsoleRepresentation.ToBbCodeRepresentation()}[/center]");
        innerText.Append("[p] [p]");
        innerText.Append(item.Description.ToBbCodeAppropriateString());
    }

    private static void AddPlayerInventoryInfo(StringBuilder innerText, string inventoryHeader, List<SimpleInventoryDto> items)
    {
        if (items == null || !items.Any()) return;
        innerText.Append("[p] [p]");
        innerText.Append($"[center]{inventoryHeader}[/center]");
        innerText.Append("[p]");
        foreach (var item in items)
        {
            innerText.Append($"[p]{item.ConsoleRepresentation.ToBbCodeRepresentation()} - {item.Name}");
        }
    }

    #endregion
}

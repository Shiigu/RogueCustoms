using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

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
        var playerInfo = _globalState.DungeonManager.GetPlayerDetailInfo();
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
        
        SetBar(null, _hpNameLabel, playerEntity.HPStatName, _hpBar, _hpAmountLabel, playerEntity.HP, playerEntity.MaxHP);
        if (playerEntity.UsesMP)
        {
            SetBar(_mpBarContainer, _mpNameLabel, playerEntity.MPStatName, _mpBar, _mpAmountLabel, playerEntity.MP, playerEntity.MaxMP);
        }
        else
        {
            _mpBarContainer.Visible = false;
            _mpNameLabel.Visible = false;
        }
        if (playerEntity.UsesHunger)
        {
            SetBar(_hungerContainer, _hungerNameLabel, playerEntity.HungerStatName, _hungerBar, _hungerAmountLabel, playerEntity.Hunger, playerEntity.MaxHunger);
        }
        else
        {
            _hungerContainer.Visible = false;
        }

        _weaponHeaderLabel.Text = TranslationServer.Translate("PlayerInfoWeaponHeader");
        _weaponNameLabel.Text = $"[center]{playerEntity.Weapon.ConsoleRepresentation.ToBbCodeRepresentation()} - {playerEntity.Weapon.Name}[/center]";
        SetCombatStatText(_damageNumberLabel, playerEntity.DamageStatName, playerEntity.WeaponDamage, playerEntity.Attack);

        _armorHeaderLabel.Text = TranslationServer.Translate("PlayerInfoArmorHeader");
        _armorNameLabel.Text = $"[center]{playerEntity.Armor.ConsoleRepresentation.ToBbCodeRepresentation()} - {playerEntity.Armor.Name}[/center]";
        SetCombatStatText(_mitigationNumberLabel, playerEntity.MitigationStatName, playerEntity.ArmorMitigation, playerEntity.Defense);
                
        SetNumericStat(_movementLabel, playerEntity.MovementStatName, playerEntity.Movement, playerEntity.BaseMovement);
        SetPercentageStat(_accuracyLabel, playerEntity.AccuracyStatName, playerEntity.Accuracy, playerEntity.BaseAccuracy);
        SetPercentageStat(_evasionLabel, playerEntity.EvasionStatName, playerEntity.Evasion, playerEntity.BaseEvasion);

        _alteredStatusesHeaderLabel.Text = TranslationServer.Translate("PlayerInfoStatusesHeader");
        FillIconList(_alteredStatusesIconsLabel, playerEntity.AlteredStatuses);

        _inventoryHeaderLabel.Text = TranslationServer.Translate("PlayerInfoInventoryHeader");
        FillIconList(_inventoryIconsLabel, playerEntity.Inventory);
    }

    private static void SetBar(Container? barContainer, Label statLabel, string statName, TextureProgressBar statBar, Label amountLabel, double current, double maximum)
    {
        if(barContainer != null) barContainer.Visible = true;
        statLabel.Visible = true;
        statLabel.Text = statName;
        statBar.MaxValue = maximum;
        statBar.Value = current;
        amountLabel.Text = $"{current}/{maximum}";
    }

    private static void SetCombatStatText(RichTextLabel statLabel, string statName, string itemStat, int playerStat)
    {
        statLabel.Text = $"[center]{statName}: {GetColorizedItemInfluencedStat(itemStat, playerStat)}[/center]";
    }

    private static string GetColorizedItemInfluencedStat(string itemStat, int playerStat)
    {
        if (playerStat > 0)
            return $"[color=#FF00FFFF]{itemStat}[/color][color=#00FF00FF]+{playerStat}[/color]";
        else if (playerStat == 0)
            return $"[color=#FF00FFFF]{itemStat}[/color][color=#FFFFFFFF]+{playerStat}[/color]";
        return $"[color=#FF00FFFF]{itemStat}[/color][color=#FF0000FF]-{Math.Abs(playerStat)}[/color]";
    }

    private static void SetNumericStat(RichTextLabel statLabel, string statName, int current, int @base)
    {
        if (current > @base)
            statLabel.Text = $"[center]{statName}: [color=#00FF00FF]{current}[/color][/center]";
        else if (current < @base)
            statLabel.Text = $"[center]{statName}: [color=#FF0000FF]{current}[/color][/center]";
        else
            statLabel.Text = $"[center]{statName}: [color=#FFFFFFFF]{current}[/color][/center]";
    }

    private static void SetPercentageStat(RichTextLabel statLabel, string statName, decimal current, decimal @base)
    {
        if (current > @base)
            statLabel.Text = $"[center]{statName}: [color=#00FF00FF]{current:F0}%[/color][/center]";
        else if (current < @base)
            statLabel.Text = $"[center]{statName}: [color=#FF0000FF]{current:F0}%[/color][/center]";
        else
            statLabel.Text = $"[center]{statName}: [color=#FFFFFFFF]{current:F0}%[/color][/center]";
    }

    private static void FillIconList(RichTextLabel iconsLabel, List<SimpleEntityDto> elementList)
    {
        iconsLabel.Text = "";
        iconsLabel.PushParagraph(HorizontalAlignment.Center);
        if (elementList.Any())
        {
            if (elementList.Count < MaxIconsPerRow)
            {
                foreach (var element in elementList)
                {
                    iconsLabel.AppendText(element.ConsoleRepresentation.ToBbCodeRepresentation());
                }
            }
            else
            {
                iconsLabel.AppendText(TranslationServer.Translate("PlayerInfoTooManyText"));
            }
        }
        else
        {
            iconsLabel.AppendText(TranslationServer.Translate("PlayerHasNothingText"));
        }
        iconsLabel.PopAll();
    }

    public void UpdatePlayerData(List<object> data)
    {
        var dungeonStatus = _globalState.DungeonInfo;
        var playerEntity = dungeonStatus.PlayerEntity;
        if (playerEntity == null) return;
        var updateType = (UpdatePlayerDataType) data[0];
        switch (updateType)
        {
            case UpdatePlayerDataType.ModifyStat:
                var statName = data[1].ToString().ToLowerInvariant();
                var value = Convert.ToDecimal(data[2]);

                switch(statName)
                {
                    case "level":
                        playerEntity.Level = (int) value;
                        _levelLabel.Text = TranslationServer.Translate("PlayerLevelText").ToString().Format(new { CurrentLevel = value });
                        break;
                    case "hp":
                        playerEntity.HP = (int) value;
                        SetBar(null, _hpNameLabel, playerEntity.HPStatName, _hpBar, _hpAmountLabel, playerEntity.HP, playerEntity.MaxHP);
                        break;
                    case "mp":
                        if (!playerEntity.UsesMP) break;
                        playerEntity.MP = (int)value;
                        SetBar(_mpBarContainer, _mpNameLabel, playerEntity.MPStatName, _mpBar, _mpAmountLabel, playerEntity.MP, playerEntity.MaxMP);
                        break;
                    case "hunger":
                        if (!playerEntity.UsesHunger) break;
                        playerEntity.Hunger = (int)value;
                        SetBar(_hungerContainer, _hungerNameLabel, playerEntity.HungerStatName, _hungerBar, _hungerAmountLabel, playerEntity.Hunger, playerEntity.MaxHunger);
                        break;
                    case "attack":
                        playerEntity.Attack = (int)value;
                        _damageNumberLabel.Text = $"[center]{playerEntity.DamageStatName}: {GetColorizedItemInfluencedStat(playerEntity.WeaponDamage, playerEntity.Attack)}[/center]";
                        break;
                    case "defense":
                        playerEntity.Defense = (int)value;
                        _mitigationNumberLabel.Text = $"[center]{playerEntity.MitigationStatName}: {GetColorizedItemInfluencedStat(playerEntity.ArmorMitigation, playerEntity.Defense)}[/center]";
                        break;
                    case "movement":
                        playerEntity.Movement = (int)value;
                        SetNumericStat(_movementLabel, playerEntity.MovementStatName, playerEntity.Movement, playerEntity.BaseMovement);
                        break;
                    case "accuracy":
                        playerEntity.Accuracy = value;
                        SetPercentageStat(_accuracyLabel, playerEntity.AccuracyStatName, playerEntity.Accuracy, playerEntity.BaseAccuracy);
                        break;
                    case "evasion":
                        playerEntity.Evasion = value;
                        SetPercentageStat(_evasionLabel, playerEntity.EvasionStatName, playerEntity.Evasion, playerEntity.BaseEvasion);
                        break;
                }
                break;
            case UpdatePlayerDataType.ModifyMaxStat:
                statName = data[1].ToString().ToLowerInvariant();
                value = Convert.ToDecimal(data[2]);
                switch (statName)
                {
                    case "hp":
                        playerEntity.MaxHP = (int)value;
                        SetBar(null, _hpNameLabel, playerEntity.HPStatName, _hpBar, _hpAmountLabel, playerEntity.HP, playerEntity.MaxHP);
                        break;
                    case "mp":
                        if (!playerEntity.UsesMP) break;
                        playerEntity.MaxMP = (int)value;
                        SetBar(_mpBarContainer, _mpNameLabel, playerEntity.MPStatName, _mpBar, _mpAmountLabel, playerEntity.MP, playerEntity.MaxMP);
                        break;
                    case "hunger":
                        if (!playerEntity.UsesHunger) break;
                        playerEntity.MaxHunger = (int)value;
                        SetBar(_hungerContainer, _hungerNameLabel, playerEntity.HungerStatName, _hungerBar, _hungerAmountLabel, playerEntity.Hunger, playerEntity.MaxHunger);
                        break;
                }
                break;
            case UpdatePlayerDataType.ModifyEquippedItem:
                var itemType = data[1].ToString();
                var entity = data[2] as SimpleEntityDto;
                var power = data[3].ToString();

                if (itemType.Equals("Weapon"))
                {
                    _weaponNameLabel.Text = $"[center]{entity.ConsoleRepresentation.ToBbCodeRepresentation()} - {entity.Name}[/center]";
                    _damageNumberLabel.Text = $"[center]{playerEntity.DamageStatName}: {GetColorizedItemInfluencedStat(power, playerEntity.Attack)}[/center]";
                }
                else if (itemType.Equals("Armor"))
                {
                    _armorNameLabel.Text = $"[center]{entity.ConsoleRepresentation.ToBbCodeRepresentation()} - {entity.Name}[/center]";
                    _mitigationNumberLabel.Text = $"[center]{playerEntity.MitigationStatName}: {GetColorizedItemInfluencedStat(power, playerEntity.Defense)}[/center]";
                }

                break;
            case UpdatePlayerDataType.UpdateAlteredStatuses:
                var alteredStatuses = data[1] as List<SimpleEntityDto>;
                playerEntity.AlteredStatuses = alteredStatuses;
                FillIconList(_alteredStatusesIconsLabel, playerEntity.AlteredStatuses);
                break;
            case UpdatePlayerDataType.UpdateInventory:
                var inventory = data[1] as List<SimpleEntityDto>;
                playerEntity.Inventory = inventory;
                FillIconList(_inventoryIconsLabel, playerEntity.Inventory);
                break;
            case UpdatePlayerDataType.UpdateConsoleRepresentation:
                var consoleRepresentation = data[1] as ConsoleRepresentation;
                playerEntity.ConsoleRepresentation = consoleRepresentation;
                _playerRepresentationLabel.Text = $"[center]{playerEntity.ConsoleRepresentation.ToBbCodeRepresentation()}[/center]";
                break;
        }
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

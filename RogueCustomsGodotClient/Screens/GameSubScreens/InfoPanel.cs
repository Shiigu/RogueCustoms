using Godot;

using MathNet.Numerics.Statistics.Mcmc;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

using RogueCustomsGodotClient;
using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Screens.GameSubScreens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class InfoPanel : GamePanel
{
    private GlobalState _globalState;
    private ExceptionLogger _exceptionLogger;
    private Label _infoTitleLabel;

    private ScalableRichTextLabel _playerNameLabel;
    private RichTextLabel _playerRepresentationLabel;
    private Label _levelLabel;

    private Label _hpNameLabel, _hpAmountLabel;
    private TextureProgressBar _hpBar;

    private Label _mpNameLabel, _mpAmountLabel;
    private TextureProgressBar _mpBar;
    private MarginContainer _mpBarContainer;

    private Label _hungerNameLabel, _hungerAmountLabel;
    private TextureProgressBar _hungerBar;
    private VBoxContainer _hungerContainer;

    private Label _equipmentHeaderLabel;
    private RichTextLabel _equipmentIconsLabel;

    private ScalableRichTextLabel _damageNumberLabel, _mitigationNumberLabel, _movementLabel, _accuracyLabel, _evasionLabel;

    private Label _alteredStatusesHeaderLabel;
    private RichTextLabel _alteredStatusesIconsLabel;

    private Label _inventoryHeaderLabel;
    private RichTextLabel _inventoryIconsLabel;

    private ScalableRichTextLabel _currencyLabel;

    private string _tooManyText;
    private const int MaxIconsPerRow = 12;

    private string _currencyName;
    private string _currencySymbolBBCode;

    public Button DetailsButton { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _globalState = GetNode<GlobalState>("/root/GlobalState");
        _exceptionLogger = GetNode<ExceptionLogger>("/root/ExceptionLogger");
        _infoTitleLabel = GetNode<Label>("VBoxContainer/InfoTitleLabel");
        _playerNameLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/PlayerDescriptorContainer/PlayerNameLabel");
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
        _equipmentHeaderLabel = GetNode<Label>("VBoxContainerContainer/EquipmentContainer/EquipmentHeaderLabel");
        _equipmentIconsLabel = GetNode<RichTextLabel>("VBoxContainerContainer/EquipmentContainer/EquipmentIconsLabel");
        _damageNumberLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/StatsContainer/DamageNumberLabel");
        _mitigationNumberLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/StatsContainer/MitigationNumberLabel");
        _movementLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/StatsContainer/MovementLabel");
        _accuracyLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/StatsContainer/AccuracyLabel");
        _evasionLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/StatsContainer/EvasionLabel");
        _alteredStatusesHeaderLabel = GetNode<Label>("VBoxContainerContainer/AlteredStatusesContainer/AlteredStatusesHeaderLabel");
        _alteredStatusesIconsLabel = GetNode<RichTextLabel>("VBoxContainerContainer/AlteredStatusesContainer/AlteredStatusesIconsLabel");
        _inventoryHeaderLabel = GetNode<Label>("VBoxContainerContainer/InventoryContainer/InventoryHeaderLabel");
        _inventoryIconsLabel = GetNode<RichTextLabel>("VBoxContainerContainer/InventoryContainer/InventoryIconsLabel");
        _currencyLabel = GetNode<ScalableRichTextLabel>("VBoxContainerContainer/CurrencyContainer/CurrencyLabel");
        DetailsButton = GetNode<Button>("ButtonContainer/DetailsButton");
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
        _playerNameLabel.DefaultFontSize = 16;
        _playerNameLabel.MinFontSize = 6;
        _damageNumberLabel.DefaultFontSize = 12;
        _damageNumberLabel.MinFontSize = 6;
        _mitigationNumberLabel.DefaultFontSize = 12;
        _mitigationNumberLabel.MinFontSize = 6;
        _movementLabel.DefaultFontSize = 12;
        _movementLabel.MinFontSize = 6;
        _accuracyLabel.DefaultFontSize = 12;
        _accuracyLabel.MinFontSize = 6;
        _evasionLabel.DefaultFontSize = 12;
        _evasionLabel.MinFontSize = 6;
        _currencyLabel.DefaultFontSize = 12;
        _currencyLabel.MinFontSize = 6;
    }

    private void DetailsButton_Pressed()
    {
        try
        {
            var playerInfo = _globalState.DungeonManager.GetPlayerDetailInfo();
            if (playerInfo == null) return;
            var titleText = TranslationServer.Translate("PlayerCharacterDetailWindowTitleText").ToString().Format(new { PlayerName = playerInfo.Name });
            var innerText = new StringBuilder();

            AddPlayerLevelInfo(innerText, playerInfo);

            innerText.Append($"[center]{TranslationServer.Translate("PlayerCharacterDetailStatsHeader")}[/center][p] [p]");
            playerInfo.Stats.ForEach(stat => AddPlayerStatsInfo(innerText, stat));
            innerText.Append($"[p] [p]{GetCurrencyText("PlayerCharacterDetailCurrencyText", _currencyName, _currencySymbolBBCode, playerInfo.CurrencyCarried)}[p] [p]");
            AddPlayerAlteredStatusesInfo(innerText, playerInfo.AlteredStatuses);
            AddPlayerItemListInfo(innerText, TranslationServer.Translate("PlayerCharacterDetailEquipmentHeader"), playerInfo.Equipment);
            AddPlayerItemListInfo(innerText, TranslationServer.Translate("PlayerCharacterDetailInventoryHeader"), playerInfo.Inventory);

            Parent?.CreateScrollablePopup(titleText, innerText.ToString(), new Color { R8 = 200, G8 = 100, B8 = 200, A8 = 255 }, false);
        }
        catch (Exception ex)
        {
            _exceptionLogger.LogMessage(ex);
        }
    }
    
    public override void Update()
	{
        var dungeonStatus = _globalState.DungeonInfo;
        var playerEntity = dungeonStatus.PlayerEntity;
        if (playerEntity == null) return;
        _playerNameLabel.SetText($"[center]{playerEntity.Name}[/center]");
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

        _equipmentHeaderLabel.Text = TranslationServer.Translate("PlayerInfoEquipmentHeader");
        FillIconList(_equipmentIconsLabel, playerEntity.Equipment);

        SetCombatStatText(_damageNumberLabel, playerEntity.DamageStatName, playerEntity.WeaponDamage, playerEntity.Attack);
        SetCombatStatText(_mitigationNumberLabel, playerEntity.MitigationStatName, playerEntity.ArmorMitigation, playerEntity.Defense);
        SetNumericStat(_movementLabel, playerEntity.MovementStatName, playerEntity.Movement, playerEntity.BaseMovement);
        SetPercentageStat(_accuracyLabel, playerEntity.AccuracyStatName, playerEntity.Accuracy, playerEntity.BaseAccuracy);
        SetPercentageStat(_evasionLabel, playerEntity.EvasionStatName, playerEntity.Evasion, playerEntity.BaseEvasion);

        _alteredStatusesHeaderLabel.Text = TranslationServer.Translate("PlayerInfoStatusesHeader");
        FillIconList(_alteredStatusesIconsLabel, playerEntity.AlteredStatuses);

        _inventoryHeaderLabel.Text = TranslationServer.Translate("PlayerInfoInventoryHeader");
        FillIconList(_inventoryIconsLabel, playerEntity.Inventory);

        _currencyName = playerEntity.Currency.Name;
        _currencySymbolBBCode = playerEntity.Currency.ConsoleRepresentation.ToBbCodeRepresentation();

        _currencyLabel.SetText($"[center]{GetCurrencyText("PlayerInfoCurrencyText", _currencyName, _currencySymbolBBCode, playerEntity.Currency.Amount)}[/center]");
    }

    private static string GetCurrencyText(string locale, string currencyName, string consoleRepresentationBBCode, int amount)
    {
        return $"{TranslationServer.Translate(locale).ToString().Format(new { CurrencyName = currencyName, Symbol = consoleRepresentationBBCode, Amount = amount.ToString() })}";
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

    private static void SetCombatStatText(ScalableRichTextLabel statLabel, string statName, string itemStat, int playerStat)
    {
        statLabel.SetText($"[center]{statName}: {GetColorizedItemInfluencedStat(itemStat, playerStat)}[/center]");
    }

    private static string GetColorizedItemInfluencedStat(string itemStat, int playerStat)
    {
        if (playerStat > 0)
            return $"[color=#FF00FFFF]{itemStat}[/color][color=#00FF00FF]+{playerStat}[/color]";
        else if (playerStat == 0)
            return $"[color=#FF00FFFF]{itemStat}[/color][color=#FFFFFFFF]+{playerStat}[/color]";
        return $"[color=#FF00FFFF]{itemStat}[/color][color=#FF0000FF]-{Math.Abs(playerStat)}[/color]";
    }

    private static void SetNumericStat(ScalableRichTextLabel statLabel, string statName, int current, int @base)
    {
        if (current > @base)
            statLabel.SetText($"[center]{statName}: [color=#00FF00FF]{current}[/color][/center]");
        else if (current < @base)
            statLabel.SetText($"[center]{statName}: [color=#FF0000FF]{current}[/color][/center]");
        else
            statLabel.SetText($"[center]{statName}: [color=#FFFFFFFF]{current}[/color][/center]");
    }

    private static void SetPercentageStat(ScalableRichTextLabel statLabel, string statName, decimal current, decimal @base)
    {
        if (current > @base)
            statLabel.SetText($"[center]{statName}: [color=#00FF00FF]{current:F0}%[/color][/center]");
        else if (current < @base)
            statLabel.SetText($"[center]{statName}: [color=#FF0000FF]{current:F0}%[/color][/center]");
        else
            statLabel.SetText($"[center]{statName}: [color=#FFFFFFFF]{current:F0}%[/color][/center]");
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
                        SetCombatStatText(_damageNumberLabel, playerEntity.DamageStatName, playerEntity.WeaponDamage, playerEntity.Attack);
                        break;
                    case "defense":
                        playerEntity.Defense = (int)value;
                        SetCombatStatText(_mitigationNumberLabel, playerEntity.MitigationStatName, playerEntity.ArmorMitigation, playerEntity.Defense);
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
            case UpdatePlayerDataType.ModifyDamageFromEquipment:
                var damageFromEquipment = data[1].ToString();
                playerEntity.WeaponDamage = damageFromEquipment;
                SetCombatStatText(_damageNumberLabel, playerEntity.DamageStatName, playerEntity.WeaponDamage, playerEntity.Attack);
                break;
            case UpdatePlayerDataType.ModifyMitigationFromEquipment:
                var mitigationFromEquipment = data[1].ToString();
                playerEntity.ArmorMitigation = mitigationFromEquipment;
                SetCombatStatText(_mitigationNumberLabel, playerEntity.MitigationStatName, playerEntity.ArmorMitigation, playerEntity.Defense);
                break;
            case UpdatePlayerDataType.UpdateEquipment:
                var equipment = data[1] as List<SimpleEntityDto>;
                playerEntity.Equipment = equipment;
                FillIconList(_equipmentIconsLabel, playerEntity.Equipment);
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
            case UpdatePlayerDataType.UpdateCurrency:
                var amount = (int) data[1];
                _currencyLabel.SetText($"[center]{TranslationServer.Translate("PlayerInfoCurrencyText").ToString().Format(new { CurrencyName = _currencyName, Symbol = _currencySymbolBBCode, Amount = amount.ToString() })}[/center]");
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
            innerText.Append($"{stat.Name}: [color={GetStatCapColor(stat)}]{(int)stat.Current}%[/color]");
        }
        else
        {
            if (stat.HasMaxStat && stat.Max != null && !stat.IsDecimalStat)
            {
                if (stat.IsHP)
                {
                    AppendHPStatWithColor(innerText, stat);
                }
                else
                {
                    innerText.Append($"{stat.Name}: {stat.Current:0.#####}/[color={GetStatCapColor(stat)}]{stat.Max:0.#####}[/color]");
                }
            }
            else if (stat.HasMaxStat && stat.Max != null && stat.IsDecimalStat)
            {
                innerText.Append($"{stat.Name}: {(int)stat.Current}/[color={GetStatCapColor(stat)}]{(int)stat.Max}[/color]");
            }
            else if (!stat.HasMaxStat && stat.IsDecimalStat)
            {
                innerText.Append($"{stat.Name}: [color={GetStatCapColor(stat)}]{stat.Current:0.#####}[/color]");
            }
            else if (!stat.HasMaxStat && !stat.IsDecimalStat)
            {
                innerText.Append($"{stat.Name}: [color={GetStatCapColor(stat)}]{(int)stat.Current}[/color]");
            }
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

    private static void AppendHPStatWithColor(StringBuilder innerText, StatDto stat)
    {
        string currentColor;

        if (stat.Current > stat.Max * 0.5M)
            currentColor = "#00FF00FF";
        else if (stat.Current > stat.Max * 0.25M)
            currentColor = "#FFA500FF";
        else if (stat.Current > 0)
            currentColor = "#FF5C00FF";
        else
            currentColor = "#FF0000FF";

        innerText.Append($"{stat.Name}: [color={currentColor}]{(int)stat.Current}[/color]/[color={GetStatCapColor(stat)}]{(int)stat.Max}[/color]");
    }

    private static string GetStatCapColor(StatDto stat)
    {
        string colorToUse = "#FFFFFFFF";

        if (stat.IsMaxedOut)
            colorToUse = "#C0A000FF";
        else if (stat.IsMinimized)
            colorToUse = "#FF0000FF";

        return colorToUse;
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

    private static void AddPlayerItemListInfo(StringBuilder innerText, string inventoryHeader, List<SimpleInventoryDto> items)
    {
        if (items == null || !items.Any()) return;
        innerText.Append("[p] [p]");
        innerText.Append($"[center]{inventoryHeader}[/center]");
        innerText.Append("[p]");
        foreach (var item in items)
        {
            innerText.Append($"[p]{item.ConsoleRepresentation.ToBbCodeRepresentation()} - {item.Name.ToColoredString(item.QualityLevelColor)}");
        }
    }

    #endregion
}

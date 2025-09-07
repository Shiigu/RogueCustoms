using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ItemTab : UserControl
    {
        private string PreviousItemType, PreviousTextBoxValue;
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemInfo LoadedItem { get; private set; }
        public event EventHandler TabInfoChanged;
        public ItemTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, ItemInfo item, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = dungeon;
            LoadedItem = item;
            EffectParamData = effectParamData;

            txtItemName.Text = item.Name;
            txtItemDescription.Text = item.Description;
            try
            {
                crsItem.Character = item.ConsoleRepresentation.Character;
                crsItem.BackgroundColor = item.ConsoleRepresentation.BackgroundColor;
                crsItem.ForegroundColor = item.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsItem.Character = '\0';
                crsItem.BackgroundColor = new GameColor(Color.Black);
                crsItem.ForegroundColor = new GameColor(Color.White);
            }
            cmbItemType.Text = "";
            cmbItemType.Items.Clear();
            cmbItemType.Items.AddRange(new string[] { "Weapon", "Armor", "Consumable" });
            PreviousItemType = "";
            var itemType = cmbItemType.Items.Cast<string>().FirstOrDefault(itemType => itemType.Equals(item.EntityType));

            var qualityLevelInfos = new List<QualityLevelOddsInfo>();
            foreach (var qualityLevel in ActiveDungeon.QualityLevelInfos)
            {
                var correspondingOdds = item.QualityLevelOdds.Find(qlo => qlo.Id.Equals(qualityLevel.Id, StringComparison.InvariantCultureIgnoreCase));
                qualityLevelInfos.Add(new QualityLevelOddsInfo()
                {
                    Id = qualityLevel.Id,
                    ChanceToPick = correspondingOdds?.ChanceToPick ?? 0
                });
            }

            qlsItem.QualityLevels = qualityLevelInfos;

            ItemStatsSheet.StatData = ActiveDungeon.CharacterStats;
            ItemStatsSheet.Stats = item.StatModifiers;
            if (itemType != null)
            {
                PreviousItemType = itemType;
                cmbItemType.Text = itemType;
            }
            ToggleItemTypeControlsVisibility();
            txtItemPower.Text = item.Power;
            chkItemStartsVisible.Checked = item.StartsVisible;
            SetSingleActionEditorParams(saeItemOnTurnStart, item.Id, item.OnTurnStart);
            SetMultiActionEditorParams(maeItemOnAttack, item.Id, item.OnAttack);
            SetSingleActionEditorParams(saeItemOnAttacked, item.Id, item.OnAttacked);
            SetSingleActionEditorParams(saeItemOnDeath, item.Id, item.OnDeath);
            SetSingleActionEditorParams(saeItemOnUse, item.Id, item.OnUse);
            nudItemBaseValue.Value = item.BaseValue;
            fklblWarningItemBaseValue.Visible = nudItemBaseValue.Value == 0;

            cmbItemMaximumQualityLevel.Items.Clear();
            foreach (var qualityLevel in dungeon.QualityLevelInfos.ConvertAll(ql => ql.Id))
            {
                cmbItemMaximumQualityLevel.Items.Add(qualityLevel);
                if (qualityLevel.Equals(item.MaximumQualityLevel))
                    cmbItemMaximumQualityLevel.Text = qualityLevel;
            }
        }

        public List<string> SaveData(string id)
        {
            qlsItem.EndEdit();
            ItemStatsSheet.EndEdit();
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtItemName.Text))
                validationErrors.Add("Enter an Item Name first.");
            if (string.IsNullOrWhiteSpace(txtItemDescription.Text))
                validationErrors.Add("Enter an Item Description first.");
            if (crsItem.Character == '\0')
                validationErrors.Add("This Item does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(cmbItemType.Text))
                validationErrors.Add("This Item does not have an Item Type.");
            if (string.IsNullOrWhiteSpace(txtItemPower.Text))
                validationErrors.Add("This Item does not have a Power.");

            var qualityLevel = ActiveDungeon.QualityLevelInfos.Find(ql => ql.Id.Equals(cmbItemMaximumQualityLevel.Text, StringComparison.InvariantCultureIgnoreCase));

            if (qualityLevel == null)
            {
                validationErrors.Add("This Item does not have a valid Quality Level.");
            }
            else
            {
                for (int i = 0; i < qlsItem.QualityLevels.Count; i++)
                {
                    var correspondingQualityLevel = ActiveDungeon.QualityLevelInfos.Find(ql => ql.Id.Equals(qlsItem.QualityLevels[i].Id, StringComparison.InvariantCultureIgnoreCase));

                    if (correspondingQualityLevel.MaximumAffixes > qualityLevel.MaximumAffixes && qlsItem.QualityLevels[i].ChanceToPick > 0)
                    {
                        validationErrors.Add($"This Item has a Maximum Quality Level of {qualityLevel.Id}, but has odds to spawn at {qlsItem.QualityLevels[i].Id} Quality, which has more maximum affixes and is thus superior.");
                    }
                    else if (qlsItem.QualityLevels[i].ChanceToPick < 0)
                    {
                        validationErrors.Add("At least one Quality Level Odds Entry has an invalid Weight value.\n\nIt must be an integer number equal to or higher than 0.");
                    }
                }
            }

            if (!validationErrors.Any())
            {
                LoadedItem = new();
                LoadedItem.Id = id;
                LoadedItem.Name = txtItemName.Text;
                LoadedItem.Description = txtItemDescription.Text;
                LoadedItem.ConsoleRepresentation = crsItem.ConsoleRepresentation;
                LoadedItem.StartsVisible = chkItemStartsVisible.Checked;
                LoadedItem.EntityType = cmbItemType.Text;
                LoadedItem.Power = txtItemPower.Text;
                LoadedItem.StatModifiers = ItemStatsSheet.Stats;
                LoadedItem.OnTurnStart = null;
                LoadedItem.OnAttacked = null;
                LoadedItem.OnUse = null;

                if (LoadedItem.EntityType == "Weapon" || LoadedItem.EntityType == "Armor")
                {
                    LoadedItem.OnTurnStart = saeItemOnTurnStart.Action;
                    if (LoadedItem.OnTurnStart != null)
                        LoadedItem.OnTurnStart.IsScript = false;
                    LoadedItem.OnAttacked = saeItemOnAttacked.Action;
                    if (LoadedItem.OnAttacked != null)
                        LoadedItem.OnAttacked.IsScript = false;
                }
                else if (LoadedItem.EntityType == "Consumable")
                {
                    LoadedItem.OnUse = saeItemOnUse.Action;
                    if (LoadedItem.OnUse != null)
                        LoadedItem.OnUse.IsScript = false;
                }

                LoadedItem.OnAttack = maeItemOnAttack.Actions;
                foreach (var action in LoadedItem.OnAttack)
                {
                    action.IsScript = false;
                }
                LoadedItem.OnDeath = saeItemOnDeath.Action;
                if (LoadedItem.OnDeath != null)
                    LoadedItem.OnDeath.IsScript = false;

                LoadedItem.BaseValue = (int) nudItemBaseValue.Value;
                LoadedItem.MaximumQualityLevel = cmbItemMaximumQualityLevel.Text;
                LoadedItem.QualityLevelOdds = qlsItem.QualityLevels.FindAll(ql => ql.ChanceToPick > 0);
            }

            return validationErrors;
        }

        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
            sae.ActionContentsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
        private void SetMultiActionEditorParams(MultiActionEditor mae, string classId, List<ActionWithEffectsInfo> actions)
        {
            mae.Actions = actions;
            mae.ClassId = classId;
            mae.Dungeon = ActiveDungeon;
            mae.EffectParamData = EffectParamData;
            mae.ActionContentsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtItemName_TextChanged(object sender, EventArgs e)
        {
            txtItemName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblItemNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtItemDescription_TextChanged(object sender, EventArgs e)
        {
            txtItemDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblItemDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void ToggleItemTypeControlsVisibility()
        {
            lblPower.Text = cmbItemType.Text switch
            {
                "Weapon" => "Weapon Damage",
                "Armor" => "Armor Mitigation",
                "Consumable" => "Consumable Power",
                _ => "Item Power"
            };
            lblStatsModifier.Text = cmbItemType.Text switch
            {
                "Weapon" => "When Equipped, it modifies:",
                "Armor" => "When Equipped, it modifies:",
                "Consumable" => "When in Inventory, it modifies:",
                _ => string.Empty
            };
            if (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor")
            {
                saeItemOnUse.Visible = false;
                saeItemOnUse.Action = null;
                saeItemOnTurnStart.Visible = true;
                maeItemOnAttack.Visible = true;
                maeItemOnAttack.ActionDescription = "The Item's Owner can\ninteract with someone else\nwith the following:";
                maeItemOnAttack.SourceDescription = "Whoever is equipping This";
                saeItemOnDeath.Visible = true;
                saeItemOnDeath.ActionDescription = "When someone equipping it dies...               ";
                saeItemOnDeath.SourceDescription = "Whoever is equipping This";
                saeItemOnAttacked.Visible = true;
                ItemStatsSheet.Visible = true;
            }
            else if (cmbItemType.Text == "Consumable")
            {
                saeItemOnUse.Visible = true;
                saeItemOnTurnStart.Visible = false;
                saeItemOnTurnStart.Action = null;
                maeItemOnAttack.Visible = true;
                maeItemOnAttack.ActionDescription = "Someone can use it to\ninteract with someone else\nwith the following:";
                maeItemOnAttack.SourceDescription = "Whoever is about to use This";
                saeItemOnDeath.Visible = true;
                saeItemOnDeath.ActionDescription = "When someone carrying it dies...                ";
                saeItemOnDeath.SourceDescription = "Whoever is has This in their Inventory";
                saeItemOnAttacked.Visible = false;
                saeItemOnAttacked.Action = null;
                ItemStatsSheet.Visible = true;
            }
            else
            {
                saeItemOnUse.Visible = false;
                saeItemOnUse.Action = null;
                saeItemOnTurnStart.Visible = false;
                saeItemOnTurnStart.Action = null;
                maeItemOnAttack.Visible = false;
                maeItemOnAttack.ActionDescription = "You shouldn't see this.";
                saeItemOnAttacked.Visible = false;
                saeItemOnAttacked.Action = null;
                saeItemOnDeath.Visible = false;
                saeItemOnDeath.Action = null;
                ItemStatsSheet.Visible = false;
            }
        }

        private void cmbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((PreviousItemType.Equals("Consumable") && (cmbItemType.Text.Equals("Weapon") || cmbItemType.Text.Equals("Armor")))
                || ((PreviousItemType.Equals("Weapon") || PreviousItemType.Equals("Armor")) && cmbItemType.Text.Equals("Consumable")))
            {
                var changeItemTypePrompt = (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor")
                    ? "Changing an Item Type from Consumable to Equippable will delete some saved Actions.\n\nNOTE: This is NOT reversible."
                    : "Changing an Item Type from Equippable to Consumable will delete some saved Actions.\n\nNOTE: This is NOT reversible.";
                var messageBoxResult = MessageBox.Show(
                    $"{changeItemTypePrompt}\n\nDo you wish to continue?",
                    "Change Item Type",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (messageBoxResult == DialogResult.No)
                {
                    cmbItemType.Text = PreviousItemType;
                    return;
                }
            }

            ToggleItemTypeControlsVisibility();

            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            PreviousItemType = cmbItemType.Text;
        }

        private void txtItemPower_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtItemPower.Text;
        }

        private void txtItemPower_Leave(object sender, EventArgs e)
        {
            if (!PreviousTextBoxValue.Equals(txtItemPower.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtItemPower.Text) && !txtItemPower.Text.IsDiceNotation() && !txtItemPower.Text.IsIntervalNotation() && !int.TryParse(txtItemPower.Text, out _))
                {
                    MessageBox.Show(
                        $"Item Power must be either a flat integer, an [X;Y] interval, or a Dice Notation expression",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtItemPower.Text = PreviousTextBoxValue;
                }
                else
                {
                    TabInfoChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            PreviousTextBoxValue = "";
        }

        private void chkItemStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkItemCanBePickedUp_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void crsItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudItemBaseValue_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            fklblWarningItemBaseValue.Visible = nudItemBaseValue.Value == 0;
        }
    }
}
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

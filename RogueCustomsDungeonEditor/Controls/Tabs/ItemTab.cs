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
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ItemTab : UserControl
    {
        private string PreviousItemType, PreviousTextBoxValue;
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
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

            if (itemType != null)
            {
                PreviousItemType = itemType;
                cmbItemType.Text = itemType;
            }
            ToggleItemTypeControlsVisibility();
            txtItemPower.Text = item.Power;
            chkItemStartsVisible.Checked = item.StartsVisible;
            chkItemCanBePickedUp.Checked = item.CanBePickedUp;
            SetSingleActionEditorParams(saeItemOnTurnStart, item.Id, item.OnTurnStart);
            SetMultiActionEditorParams(maeItemOnAttack, item.Id, item.OnAttack);
            SetSingleActionEditorParams(saeItemOnAttacked, item.Id, item.OnAttacked);
            SetSingleActionEditorParams(saeItemOnDeath, item.Id, item.OnDeath);
            SetSingleActionEditorParams(saeItemOnUse, item.Id, item.OnUse);
            SetSingleActionEditorParams(saeItemOnStepped, item.Id, item.OnStepped);
        }

        public List<string> SaveData(string id)
        {
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

            if (!validationErrors.Any())
            {
                LoadedItem = new();
                LoadedItem.Id = id;
                LoadedItem.Name = txtItemName.Text;
                LoadedItem.Description = txtItemDescription.Text;
                LoadedItem.ConsoleRepresentation = crsItem.ConsoleRepresentation;
                LoadedItem.StartsVisible = chkItemStartsVisible.Checked;
                LoadedItem.CanBePickedUp = chkItemCanBePickedUp.Checked;
                LoadedItem.EntityType = cmbItemType.Text;
                LoadedItem.Power = txtItemPower.Text;

                LoadedItem.OnTurnStart = null;
                LoadedItem.OnAttacked = null;
                LoadedItem.OnUse = null;

                if (LoadedItem.EntityType == "Weapon" || LoadedItem.EntityType == "Armor")
                {
                    LoadedItem.OnTurnStart = saeItemOnTurnStart.Action;
                    LoadedItem.OnAttacked = saeItemOnAttacked.Action;
                }
                else if (LoadedItem.EntityType == "Consumable")
                {
                    LoadedItem.OnUse = saeItemOnUse.Action;
                }

                LoadedItem.OnAttack = maeItemOnAttack.Actions;
                LoadedItem.OnStepped = saeItemOnStepped.Action;
                LoadedItem.OnDeath = saeItemOnDeath.Action;
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
            if (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor")
            {
                saeItemOnStepped.Visible = true;
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
            }
            else if (cmbItemType.Text == "Consumable")
            {
                saeItemOnStepped.Visible = true;
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
            }
            else
            {
                saeItemOnStepped.Visible = false;
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
                saeItemOnStepped.Visible = false;
                saeItemOnStepped.Action = null;
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
                if (!string.IsNullOrWhiteSpace(txtItemPower.Text) && !txtItemPower.Text.IsDiceNotation() && !int.TryParse(txtItemPower.Text, out _))
                {
                    MessageBox.Show(
                        $"Item Power must be either a flat integer or a Dice Notation expression",
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
    }
}

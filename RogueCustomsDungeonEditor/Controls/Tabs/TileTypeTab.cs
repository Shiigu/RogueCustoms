using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class TileTypeTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TileTypeInfo LoadedTileType { get; private set; }
        public event EventHandler TabInfoChanged;

        public TileTypeTab()
        {
            InitializeComponent();
        }

        public void LoadData(TileTypeInfo tileTypeInfoToLoad, DungeonInfo activeDungeon, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            EffectParamData = effectParamData;
            LoadedTileType = tileTypeInfoToLoad;
            txtTileTypeName.Text = tileTypeInfoToLoad.Name;
            txtTileTypeDescription.Text = tileTypeInfoToLoad.Description;
            chkTileTypeIsWalkable.Checked = LoadedTileType.IsWalkable;
            chkTileTypeIsSolid.Checked = LoadedTileType.IsSolid;
            chkTileTypeIsVisible.Checked = LoadedTileType.IsVisible;
            chkTileTypeAcceptsItems.Checked = LoadedTileType.AcceptsItems;
            chkTileTypeCanBeTransformed.Checked = LoadedTileType.CanBeTransformed;
            chkTileTypeCanVisiblyConnectWithOtherTiles.Checked = LoadedTileType.CanVisiblyConnectWithOtherTiles;
            chkTileTypeCanHaveMultilineConnections.Checked = LoadedTileType.CanHaveMultilineConnections;
            chkTileTypeCausesPartialInvisibility.Checked = LoadedTileType.CausesPartialInvisibility;
            SetSingleActionEditorParams(saeOnStood, tileTypeInfoToLoad.Id, tileTypeInfoToLoad.OnStood);
            ToggleCheckboxes();
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtTileTypeName.Text))
                validationErrors.Add("The Tile Type lacks a Name");

            if (string.IsNullOrWhiteSpace(txtTileTypeDescription.Text))
                validationErrors.Add("The Tile Type lacks a Description");

            if (!validationErrors.Any())
            {
                LoadedTileType = new()
                {
                    Id = id,
                    Name = txtTileTypeName.Text,
                    Description = txtTileTypeDescription.Text,
                    IsWalkable = chkTileTypeIsWalkable.Checked,
                    IsSolid = chkTileTypeIsSolid.Checked,
                    IsVisible = chkTileTypeIsVisible.Checked,
                    AcceptsItems = chkTileTypeAcceptsItems.Checked,
                    CanBeTransformed = chkTileTypeCanBeTransformed.Checked,
                    CanVisiblyConnectWithOtherTiles = chkTileTypeCanVisiblyConnectWithOtherTiles.Checked,
                    CanHaveMultilineConnections = chkTileTypeCanHaveMultilineConnections.Checked,
                    CausesPartialInvisibility = chkTileTypeCausesPartialInvisibility.Checked,
                    OnStood = saeOnStood.Action
                };
                if (LoadedTileType.OnStood != null)
                    LoadedTileType.OnStood.IsScript = false;
            }

            return validationErrors;
        }

        private void ToggleCheckboxes()
        {
            if (chkTileTypeIsWalkable.Checked)
            {
                chkTileTypeIsSolid.Enabled = false;
                chkTileTypeIsSolid.Checked = false;
                chkTileTypeIsVisible.Enabled = false;
                chkTileTypeIsVisible.Checked = true;
            }
            else
            {
                chkTileTypeIsSolid.Enabled = true;
                chkTileTypeIsVisible.Enabled = true;
            }
            if (chkTileTypeCanVisiblyConnectWithOtherTiles.Checked)
            {
                chkTileTypeCanHaveMultilineConnections.Enabled = true;
            }
            else
            {
                chkTileTypeCanHaveMultilineConnections.Enabled = false;
                chkTileTypeCanHaveMultilineConnections.Checked = false;
            }
        }

        private void chkTileTypeIsWalkable_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeIsSolid_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeIsVisible_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeAcceptsItems_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeCanBeTransformed_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeCanVisiblyConnectWithOtherTiles_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeCanHaveMultilineConnections_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void chkTileTypeCausesPartialInvisibility_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            ToggleCheckboxes();
        }

        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
        }

        private void saeOnStood_ActionContentsChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void txtTileTypeName_TextChanged(object sender, EventArgs e)
        {
            txtTileTypeName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblTileTypeNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtTileTypeDescription_TextChanged(object sender, EventArgs e)
        {
            txtTileTypeDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblTileTypeDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

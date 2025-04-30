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

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class TrapTab : UserControl
    {
        private string PreviousTextBoxValue;
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrapInfo LoadedTrap { get; private set; }
        public event EventHandler TabInfoChanged;
        public TrapTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, TrapInfo trap, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = dungeon;
            LoadedTrap = trap;
            EffectParamData = effectParamData;

            txtTrapName.Text = trap.Name;
            txtTrapDescription.Text = trap.Description;
            try
            {
                crsTrap.Character = trap.ConsoleRepresentation.Character;
                crsTrap.BackgroundColor = trap.ConsoleRepresentation.BackgroundColor;
                crsTrap.ForegroundColor = trap.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsTrap.Character = '\0';
                crsTrap.BackgroundColor = new GameColor(Color.Black);
                crsTrap.ForegroundColor = new GameColor(Color.White);
            }
            txtTrapPower.Text = trap.Power;
            chkTrapStartsVisible.Checked = trap.StartsVisible;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(chkTrapStartsVisible, "The 'spirit' of a Trap is that it spawns invisible.\n\nHowever, it can be enabled for debugging purposes.");
            SetSingleActionEditorParams(saeTrapOnStepped, trap.Id, trap.OnStepped);
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtTrapName.Text))
                validationErrors.Add("Enter an Trap Name first.");
            if (string.IsNullOrWhiteSpace(txtTrapDescription.Text))
                validationErrors.Add("Enter an Trap Description first.");
            if (crsTrap.Character == '\0')
                validationErrors.Add("This Trap does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(txtTrapPower.Text))
                validationErrors.Add("This Trap does not have a Power.");

            if (!validationErrors.Any())
            {
                LoadedTrap = new();
                LoadedTrap.Id = id;
                LoadedTrap.Name = txtTrapName.Text;
                LoadedTrap.Description = txtTrapDescription.Text;
                LoadedTrap.ConsoleRepresentation = crsTrap.ConsoleRepresentation;
                LoadedTrap.StartsVisible = chkTrapStartsVisible.Checked;
                LoadedTrap.Power = txtTrapPower.Text;

                LoadedTrap.OnStepped = saeTrapOnStepped.Action;
                if (LoadedTrap.OnStepped != null)
                    LoadedTrap.OnStepped.IsScript = false;
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

        private void txtTrapName_TextChanged(object sender, EventArgs e)
        {
            txtTrapName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblTrapNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtTrapDescription_TextChanged(object sender, EventArgs e)
        {
            txtTrapDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblTrapDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtTrapPower_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtTrapPower.Text;
        }

        private void txtTrapPower_Leave(object sender, EventArgs e)
        {
            if (!PreviousTextBoxValue.Equals(txtTrapPower.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtTrapPower.Text) && !txtTrapPower.Text.IsDiceNotation() && !txtTrapPower.Text.IsIntervalNotation() && !int.TryParse(txtTrapPower.Text, out _))
                {
                    MessageBox.Show(
                        $"Trap Power must be either a flat integer, an [X;Y] interval or a Dice Notation expression",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtTrapPower.Text = PreviousTextBoxValue;
                }
                else
                {
                    TabInfoChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            PreviousTextBoxValue = "";
        }

        private void chkTrapStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void crsTrap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

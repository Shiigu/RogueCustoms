using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class AlteredStatusTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AlteredStatusInfo LoadedAlteredStatus { get; private set; }
        public event EventHandler TabInfoChanged;
        public AlteredStatusTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, AlteredStatusInfo alteredStatus, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = dungeon;
            LoadedAlteredStatus = alteredStatus;
            EffectParamData = effectParamData;


            txtAlteredStatusName.Text = alteredStatus.Name;
            txtAlteredStatusDescription.Text = alteredStatus.Description;
            try
            {
                crsAlteredStatus.Character = alteredStatus.ConsoleRepresentation.Character;
                crsAlteredStatus.BackgroundColor = alteredStatus.ConsoleRepresentation.BackgroundColor;
                crsAlteredStatus.ForegroundColor = alteredStatus.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsAlteredStatus.Character = '\0';
                crsAlteredStatus.BackgroundColor = new GameColor(Color.Black);
                crsAlteredStatus.ForegroundColor = new GameColor(Color.White);
            }
            chkAlteredStatusCanStack.Checked = alteredStatus.CanStack;
            chkAlteredStatusCanOverwrite.Checked = alteredStatus.CanOverwrite;
            chkAlteredStatusCleanseOnFloorChange.Checked = alteredStatus.CleanseOnFloorChange;
            chkAlteredStatusCleansedOnCleanseActions.Checked = alteredStatus.CleansedByCleanseActions;
            saeAlteredStatusOnApply.SetActionEditorParams(alteredStatus.Id, alteredStatus.OnApply, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeAlteredStatusOnTurnStart.SetActionEditorParams(alteredStatus.Id, alteredStatus.OnTurnStart, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeAlteredStatusBeforeAttack.SetActionEditorParams(alteredStatus.Id, alteredStatus.BeforeAttack, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeAlteredStatusOnAttacked.SetActionEditorParams(alteredStatus.Id, alteredStatus.OnAttacked, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeAlteredStatusOnRemove.SetActionEditorParams(alteredStatus.Id, alteredStatus.OnRemove, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtAlteredStatusName.Text))
                validationErrors.Add("Enter an Altered Status Name first.");
            if (string.IsNullOrWhiteSpace(txtAlteredStatusDescription.Text))
                validationErrors.Add("Enter an Altered Status Description first.");
            if (crsAlteredStatus.Character == '\0')
                validationErrors.Add("This Altered Status does not have a Console Representation character.");

            if (!validationErrors.Any())
            {
                LoadedAlteredStatus = new();
                LoadedAlteredStatus.Id = id;
                LoadedAlteredStatus.Name = txtAlteredStatusName.Text;
                LoadedAlteredStatus.Description = txtAlteredStatusDescription.Text;
                LoadedAlteredStatus.ConsoleRepresentation = crsAlteredStatus.ConsoleRepresentation;
                LoadedAlteredStatus.CanStack = chkAlteredStatusCanStack.Checked;
                LoadedAlteredStatus.CanOverwrite = chkAlteredStatusCanOverwrite.Checked;
                LoadedAlteredStatus.CleanseOnFloorChange = chkAlteredStatusCleanseOnFloorChange.Checked;
                LoadedAlteredStatus.CleansedByCleanseActions = chkAlteredStatusCleansedOnCleanseActions.Checked;

                LoadedAlteredStatus.OnApply = saeAlteredStatusOnApply.Action;
                if (LoadedAlteredStatus.OnApply != null)
                    LoadedAlteredStatus.OnApply.IsScript = false;
                LoadedAlteredStatus.OnTurnStart = saeAlteredStatusOnTurnStart.Action;
                if (LoadedAlteredStatus.OnTurnStart != null)
                    LoadedAlteredStatus.OnTurnStart.IsScript = false;
                LoadedAlteredStatus.BeforeAttack = saeAlteredStatusBeforeAttack.Action;
                if (LoadedAlteredStatus.BeforeAttack != null)
                    LoadedAlteredStatus.BeforeAttack.IsScript = false;
                LoadedAlteredStatus.OnAttacked = saeAlteredStatusOnAttacked.Action;
                if (LoadedAlteredStatus.OnAttacked != null)
                    LoadedAlteredStatus.OnAttacked.IsScript = false;
                LoadedAlteredStatus.OnRemove = saeAlteredStatusOnRemove.Action;
                if (LoadedAlteredStatus.OnRemove != null)
                    LoadedAlteredStatus.OnRemove.IsScript = false;
            }

            return validationErrors;
        }

        private void txtAlteredStatusName_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtAlteredStatusDescription_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkAlteredStatusCanStack_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkAlteredStatusCanOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkAlteredStatusCleanseOnFloorChange_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkAlteredStatusCleansedOnCleanseActions_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void crsAlteredStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

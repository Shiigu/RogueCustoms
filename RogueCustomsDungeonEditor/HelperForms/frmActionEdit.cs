using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RogueCustomsGameEngine.Game.Entities.Effect;

namespace RogueCustomsDungeonEditor.HelperForms
{
    #pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public partial class frmActionEdit : Form
    {
        public ActionWithEffectsInfo ActionToSave { get; set; }
        public bool Saved { get; set; }
        public bool IsNewAction { get; set; }
        private DungeonInfo ActiveDungeon;
        private List<string> UsableAlteredStatusList;
        private List<EffectTypeData> SelectableEffects;

        private TreeNode SelectedNode;
        private bool HasThenChildNode;
        private bool HasOnSuccessFailureChildNodes;
        private bool RequiresDescription;
        private bool RequiresActionName;
        private bool RequiresCondition;
        private UsageCriteria UsageCriteria;
        private string ClassId;
        private string PlaceholderActionName;
        private string PreviousTextBoxValue;
        public frmActionEdit(ActionWithEffectsInfo actionToSave, DungeonInfo activeDungeon, string classId, string actionTypeText, bool requiresCondition, bool requiresDescription, bool requiresActionName, string placeholderActionNameIfNeeded, UsageCriteria usageCriteria, List<string> alteredStatusList, List<EffectTypeData> selectableEffects)
        {
            InitializeComponent();
            ActionToSave = actionToSave.Clone();
            ActiveDungeon = activeDungeon;
            ClassId = classId;
            if (actionToSave == null || actionToSave.Effect == null)
            {
                this.Text = "Action Editor - [New Action]";
                if (!string.IsNullOrWhiteSpace(ClassId))
                    lblTitle.Text = $"Create {actionTypeText} for {ClassId}";
                else
                    lblTitle.Text = $"Create {actionTypeText} for Floor Group";
            }
            else
            {
                this.Text = $"Action Editor - [{actionToSave.Name}]";
                if (!string.IsNullOrWhiteSpace(ClassId))
                    lblTitle.Text = $"Edit {actionTypeText} for {ClassId}";
                else
                    lblTitle.Text = $"Edit {actionTypeText} for Floor Group";
            }
            btnSaveAs.Enabled = requiresActionName;
            RequiresDescription = requiresDescription;
            RequiresActionName = requiresActionName;
            RequiresCondition = requiresCondition;

            if (!RequiresActionName)
                PlaceholderActionName = placeholderActionNameIfNeeded;

            UsageCriteria = usageCriteria;

            if (RequiresDescription)
            {
                txtActionDescription.Enabled = true;
                txtActionDescription.Text = ActionToSave.Description;
            }
            else
            {
                txtActionDescription.Enabled = false;
                txtActionDescription.Text = "";
            }

            if (RequiresCondition)
            {
                txtActionCondition.Enabled = true;
                txtActionCondition.Text = ActionToSave.UseCondition;
                fklblConditionWarning.Visible = !string.IsNullOrWhiteSpace(txtActionCondition.Text);
            }
            else
            {
                txtActionCondition.Enabled = false;
                txtActionCondition.Text = "";
                fklblConditionWarning.Visible = false;
            }

            UsableAlteredStatusList = alteredStatusList.Where(als => !als.Equals(classId)).ToList();
            SelectableEffects = selectableEffects;
            RefreshActionSequenceTree();

            if (usageCriteria == UsageCriteria.AnyTargetAnyTime)
            {
                gbSelectionCriteria.Enabled = false;
            }
            else if (usageCriteria == UsageCriteria.AnyTarget)
            {
                chkAllies.Enabled = false;
                chkEnemies.Enabled = false;
                chkSelf.Enabled = false;
                nudMinRange.Enabled = false;
                nudMaxRange.Enabled = false;
                nudMPCost.Enabled = false;
            }
            else
            {
                chkAllies.Checked = ActionToSave.TargetTypes?.Contains("Ally", StringComparer.InvariantCultureIgnoreCase) == true;
                chkEnemies.Checked = ActionToSave.TargetTypes?.Contains("Enemy", StringComparer.InvariantCultureIgnoreCase) == true;
                chkSelf.Checked = ActionToSave.TargetTypes?.Contains("Self", StringComparer.InvariantCultureIgnoreCase) == true;
                nudMinRange.Value = ActionToSave.MinimumRange;
                nudMaxRange.Value = ActionToSave.MaximumRange;
                nudCooldown.Value = ActionToSave.CooldownBetweenUses;
                nudInitialCooldown.Value = ActionToSave.StartingCooldown;
                nudMaximumUses.Value = ActionToSave.MaximumUses;
                nudMPCost.Value = ActionToSave.MPCost;
                lblNoCooldown.Visible = nudCooldown.Value < 2;
                lblInfiniteUse.Visible = nudMaximumUses.Value == 0;
            }
        }

        private void RefreshActionSequenceTree()
        {
            tvEffectSequence.SelectedNode = null;
            tvEffectSequence.Nodes.Clear();
            if (ActionToSave == null)
                ActionToSave = new ActionWithEffectsInfo();
            if (ActionToSave.Effect == null)
                ActionToSave.Effect = new EffectInfo();
            AddActionNode(new EffectInfoDto(ActionToSave.Effect, null, SelectableEffects), null, ActionToSave.Effect);
            tvEffectSequence.ExpandAll();
        }

        private void AddActionNode(EffectInfoDto effectDto, TreeNode parentNode, EffectInfo tag)
        {
            var effectNode = new TreeNode($"{effectDto.Moment} - {effectDto.DisplayName}")
            {
                Tag = tag
            };
            var tooltipBuilder = new StringBuilder();
            tooltipBuilder.Append("Description: ").Append(effectDto.Description).AppendLine("\n\nParameters:");

            if (effectDto.Parameters.Any())
            {
                foreach (var parameter in effectDto.Parameters)
                {
                    tooltipBuilder.Append("- ").Append(parameter.DisplayName).Append(": ").AppendLine(parameter.Value);
                }
            }
            else
            {
                tooltipBuilder.AppendLine("None");
            }

            effectNode.ToolTipText = tooltipBuilder.ToString();

            if (parentNode == null)
            {
                tvEffectSequence.Nodes.Add(effectNode);
            }
            else
            {
                parentNode.Nodes.Add(effectNode);
            }

            if (tag.Then != null)
            {
                AddActionNode(new EffectInfoDto(tag.Then, tag, SelectableEffects), effectNode, tag.Then);
            }
            else if (tag.OnSuccess != null || tag.OnFailure != null)
            {
                if (tag.OnSuccess != null)
                {
                    AddActionNode(new EffectInfoDto(tag.OnSuccess, tag, SelectableEffects), effectNode, tag.OnSuccess);
                }
                else
                {
                    var nullEffectNode = new TreeNode("ON SUCCESS - Do nothing")
                    {
                        Tag = null,
                    };
                    effectNode.Nodes.Add(nullEffectNode);
                }
                if (tag.OnFailure != null)
                {
                    AddActionNode(new EffectInfoDto(tag.OnFailure, tag, SelectableEffects), effectNode, tag.OnFailure);
                }
                else
                {
                    var nullEffectNode = new TreeNode("ON FAILURE - Do nothing")
                    {
                        Tag = null,
                    };
                    effectNode.Nodes.Add(nullEffectNode);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nudCooldown_ValueChanged(object sender, EventArgs e)
        {
            lblNoCooldown.Visible = nudCooldown.Value < 2;
        }

        private void nudMaximumUses_ValueChanged(object sender, EventArgs e)
        {
            lblInfiniteUse.Visible = nudMaximumUses.Value == 0;
        }

        private void tvEffectSequence_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedNode = tvEffectSequence.SelectedNode;
            HasThenChildNode = false;
            HasOnSuccessFailureChildNodes = false;
            if (SelectedNode != null)
            {
                var selectedEffect = (EffectInfo)SelectedNode.Tag;
                var selectedEffectData = SelectableEffects.Find(se => se.InternalName.Equals(selectedEffect?.EffectName));
                HasThenChildNode = SelectedNode.Nodes.Cast<TreeNode>().Any(node => node.Text.Contains("THEN"));

                if (!HasThenChildNode)
                {
                    foreach (TreeNode node in SelectedNode.Nodes)
                    {
                        if (node.Text.Contains("ON SUCCESS") || node.Text.Contains("ON FAILURE"))
                        {
                            HasOnSuccessFailureChildNodes = true;
                            break;
                        }
                    }
                }
                btnEdit.Enabled = true;
                btnRemove.Enabled = true;
                btnNewThen.Enabled = selectedEffectData?.CanHaveThenChild == true && !HasThenChildNode;
                btnNewOnSuccessFailure.Enabled = selectedEffectData?.CanHaveOnSuccessOnFailureChild == true && selectedEffect != null && !HasOnSuccessFailureChildNodes;
            }
            else
            {
                btnEdit.Enabled = false;
                btnRemove.Enabled = false;
                btnNewThen.Enabled = false;
                btnNewOnSuccessFailure.Enabled = false;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null) return;
            var currentEffect = (EffectInfo)SelectedNode.Tag;
            var parentEffect = SelectedNode.Parent != null ? (EffectInfo)SelectedNode.Parent.Tag : null;
            var currentEffectDisplayName = SelectableEffects.Find(se => se.InternalName.Equals(currentEffect?.EffectName))?.DisplayName;
            var inputBoxPrompt = currentEffect?.EffectName != ""
                ? "You may change the function if you wish.\n\nAll parameters will display as default if you do, however."
                : "Please indicate the function that will be executed in this step.";
            var effectTypeSelection = ComboInputBox.Show(inputBoxPrompt, "Edit Step", SelectableEffects.Where(se => se.InternalName != "Equip").Select(se => se.DisplayName).ToList(), currentEffectDisplayName);
            if (effectTypeSelection == null) return;
            var selectedEffectTypeData = SelectableEffects.Find(se => se.DisplayName.Equals(effectTypeSelection));
            if (selectedEffectTypeData == null) return;
            if (!selectedEffectTypeData.InternalName.Equals(currentEffect.EffectName) && (!string.IsNullOrWhiteSpace(currentEffect.Then?.EffectName)
                || !string.IsNullOrWhiteSpace(currentEffect.OnSuccess?.EffectName)
                || !string.IsNullOrWhiteSpace(currentEffect.OnFailure?.EffectName)))
            {
                var messageBoxResult = MessageBox.Show(
                    "This Action has child steps. If you change the function and save it, the child steps will be completely erased!\n\nAre you sure you want to continue?",
                    "Add THEN Step",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.No)
                    return;
            }
            if (selectedEffectTypeData.InternalName.Equals(currentEffect.EffectName) && ((currentEffect.Then != null && string.IsNullOrWhiteSpace(currentEffect.Then?.EffectName))
                || (currentEffect.OnSuccess != null && string.IsNullOrWhiteSpace(currentEffect.OnSuccess?.EffectName))
                || (currentEffect.OnFailure != null && string.IsNullOrWhiteSpace(currentEffect.OnFailure?.EffectName))))
            {
                var messageBoxResult = MessageBox.Show(
                    "This Action has 'Do Nothing' child steps. If you save any changes you make, the child steps will be completely erased!\n\nAre you sure you want to continue?",
                    "Add THEN Step",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.No)
                    return;
            }
            var frmActionParameter = new frmActionParameters(currentEffect, ActiveDungeon, selectedEffectTypeData, UsableAlteredStatusList, selectedEffectTypeData.InternalName.Equals(currentEffect?.EffectName));
            frmActionParameter.ShowDialog();
            if (frmActionParameter.Saved)
            {
                if (parentEffect != null)
                {
                    if (SelectedNode.Text.Contains("THEN"))
                        parentEffect.Then = frmActionParameter.EffectToSave;
                    else if (SelectedNode.Text.Contains("ON SUCCESS"))
                        parentEffect.OnSuccess = frmActionParameter.EffectToSave;
                    else if (SelectedNode.Text.Contains("ON FAILURE"))
                        parentEffect.OnFailure = frmActionParameter.EffectToSave;
                }
                else if (SelectedNode.Text.Contains("INITIAL"))
                {
                    ActionToSave.Effect = frmActionParameter.EffectToSave;
                }
            }
            RefreshActionSequenceTree();
            tvEffectSequence.SelectNodeByTag(frmActionParameter.EffectToSave);
        }

        private void btnNewThen_Click(object sender, EventArgs e)
        {
            var currentEffect = (EffectInfo)SelectedNode.Tag;
            if (HasOnSuccessFailureChildNodes)
            {
                var messageBoxResult = MessageBox.Show(
                    "This step has On Success/On Failure steps associated with it.\n\nAdding a Then step will remove them and all subsequent steps. Are you sure you want to continue?",
                    "Add THEN Step",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.Yes)
                {
                    if (currentEffect.OnSuccess != null)
                        RemoveEffect(currentEffect.OnSuccess, currentEffect);
                    if (currentEffect.OnFailure != null)
                        RemoveEffect(currentEffect.OnFailure, currentEffect);
                }
                else
                {
                    return;
                }
            }

            var newEffect = new EffectInfo();
            currentEffect.Then = newEffect;
            RefreshActionSequenceTree();
            tvEffectSequence.SelectNodeByTag(newEffect);
        }

        private void btnNewOnSuccessFailure_Click(object sender, EventArgs e)
        {
            var currentEffect = (EffectInfo)SelectedNode.Tag;
            if (HasThenChildNode)
            {
                var messageBoxResult = MessageBox.Show(
                    "This step has a Then step associated with it.\n\nAdding On Success and On Failure steps will remove it and all subsequent steps. Are you sure you want to continue?",
                    "Add THEN Step",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.Yes)
                {
                    if (currentEffect.Then != null)
                        RemoveEffect(currentEffect.Then, currentEffect);
                }
                else
                {
                    return;
                }
            }

            var newOnSuccess = new EffectInfo();
            currentEffect.OnSuccess = newOnSuccess;
            var newOnFailure = new EffectInfo();
            currentEffect.OnFailure = newOnFailure;
            RefreshActionSequenceTree();
            tvEffectSequence.SelectNodeByTag(newOnSuccess);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var messageBoxPrompt = (SelectedNode.Nodes.Count > 0)
                ? "Do you wish to remove this Step?\n\nWARNING: This will also remove ALL steps that follow it."
                : "Do you wish to remove this Step?";
            var messageBoxResult = MessageBox.Show(
                messageBoxPrompt,
                "Delete Step",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                var selectedEffect = (EffectInfo)SelectedNode.Tag;
                EffectInfo parentEffect = null;
                if (SelectedNode.Parent != null)
                    parentEffect = (EffectInfo)SelectedNode.Parent.Tag;
                RemoveEffect(selectedEffect, parentEffect);

                if (parentEffect == null)
                {
                    ActionToSave.Effect = new EffectInfo();
                }

                HasThenChildNode = false;
                HasOnSuccessFailureChildNodes = false;
                RefreshActionSequenceTree();

                btnEdit.Enabled = false;
                btnRemove.Enabled = false;
                btnNewThen.Enabled = false;
                btnNewOnSuccessFailure.Enabled = false;

                if (parentEffect != null)
                    tvEffectSequence.SelectNodeByTag(parentEffect);
            }
        }

        private void RemoveEffect(EffectInfo effect, EffectInfo parentEffect)
        {
            if (effect.Then != null)
                RemoveEffect(effect.Then, effect);
            if (effect.OnSuccess != null)
                RemoveEffect(effect.OnSuccess, effect);
            if (effect.OnFailure != null)
                RemoveEffect(effect.OnFailure, effect);
            if (parentEffect != null)
            {
                if (parentEffect.Then == effect)
                    parentEffect.Then = null;
                if (parentEffect.OnSuccess == effect)
                    parentEffect.OnSuccess = null;
                if (parentEffect.OnFailure == effect)
                    parentEffect.OnFailure = null;
            }
        }

        private void ScrubNullEffects(EffectInfo effect, EffectInfo parentEffect)
        {
            if (effect.Then != null)
                ScrubNullEffects(effect.Then, effect);
            if (effect.OnSuccess != null)
                ScrubNullEffects(effect.OnSuccess, effect);
            if (effect.OnFailure != null)
                ScrubNullEffects(effect.OnFailure, effect);
            if (parentEffect != null)
            {
                if (parentEffect.Then == effect && string.IsNullOrWhiteSpace(effect.EffectName))
                    parentEffect.Then = null;
                if (parentEffect.OnSuccess == effect && string.IsNullOrWhiteSpace(effect.EffectName))
                    parentEffect.OnSuccess = null;
                if (parentEffect.OnFailure == effect && string.IsNullOrWhiteSpace(effect.EffectName))
                    parentEffect.OnFailure = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ActionToSave.Name))
            {
                btnSaveAs_Click(sender, e);
            }
            else
            {
                if (!ValidateStepsBeforeSave(out var errorMessages))
                {
                    MessageBox.Show(
                        $"These steps could not be saved due to the following errors:\n- {string.Join("\n- ", errorMessages)}",
                        "Invalid step data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                else
                {
                    if (ActionToSave == null || ActionToSave.Effect == null || string.IsNullOrWhiteSpace(ActionToSave.Effect.EffectName))
                    {
                        var messageBoxResult = MessageBox.Show(
                            "This Action has NO steps. If saved, it will be completely erased!\n\nAre you sure you want to continue?",
                            "Add THEN Step",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );
                        if (messageBoxResult == DialogResult.Yes)
                        {
                            ActionToSave = null;
                            this.Saved = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        if (gbSelectionCriteria.Enabled)
                        {
                            if (chkAllies.Checked || chkEnemies.Checked || chkSelf.Checked)
                            {
                                ActionToSave.TargetTypes = new List<string>();
                                if (chkAllies.Checked)
                                    ActionToSave.TargetTypes.Add("Ally");
                                if (chkEnemies.Checked)
                                    ActionToSave.TargetTypes.Add("Enemy");
                                if (chkSelf.Checked)
                                    ActionToSave.TargetTypes.Add("Self");
                            }
                            ActionToSave.MinimumRange = (int)nudMinRange.Value;
                            ActionToSave.MaximumRange = (int)nudMaxRange.Value;
                            ActionToSave.CooldownBetweenUses = (int)nudCooldown.Value;
                            ActionToSave.StartingCooldown = (int)nudInitialCooldown.Value;
                            ActionToSave.MaximumUses = (int)nudMaximumUses.Value;
                            ActionToSave.MPCost = (int)nudMPCost.Value;
                        }
                        ActionToSave.UseCondition = txtActionCondition.Text;
                        ActionToSave.Description = txtActionDescription.Text;
                        ScrubNullEffects(ActionToSave.Effect, null);
                        this.Saved = true;
                        this.Close();
                    }
                }
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (!ValidateStepsBeforeSave(out var errorMessages))
            {
                MessageBox.Show(
                    $"These steps could not be saved due to the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Invalid step data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                if (ActionToSave == null || ActionToSave.Effect == null || string.IsNullOrWhiteSpace(ActionToSave.Effect.EffectName))
                {
                    var messageBoxResult = MessageBox.Show(
                        "This Action has NO steps. Proceeding means it will not be saved.\n\nAre you sure you want to continue?",
                        "Add THEN Step",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        ActionToSave = null;
                        this.IsNewAction = RequiresActionName;
                        this.Saved = true;
                        this.Close();
                    }
                }
                else
                {
                    var nameToUse = (!RequiresActionName)
                           ? PlaceholderActionName
                           : InputBox.Show("Please write a name for the Action. If this is an Action for the Player Character or a usable Item, this will be its visible name.\nTo cancel Saving, leave the field empty.", "Give the Action a name", "");
                    if (!string.IsNullOrWhiteSpace(nameToUse))
                    {
                        if (gbSelectionCriteria.Enabled)
                        {
                            if (chkAllies.Checked || chkEnemies.Checked || chkSelf.Checked)
                            {
                                ActionToSave.TargetTypes = new List<string>();
                                if (chkAllies.Checked)
                                    ActionToSave.TargetTypes.Add("Ally");
                                if (chkEnemies.Checked)
                                    ActionToSave.TargetTypes.Add("Enemy");
                                if (chkSelf.Checked)
                                    ActionToSave.TargetTypes.Add("Self");
                            }
                            ActionToSave.MinimumRange = (int)nudMinRange.Value;
                            ActionToSave.MaximumRange = (int)nudMaxRange.Value;
                            ActionToSave.CooldownBetweenUses = (int)nudCooldown.Value;
                            ActionToSave.StartingCooldown = (int)nudInitialCooldown.Value;
                            ActionToSave.MaximumUses = (int)nudMaximumUses.Value;
                            ActionToSave.MPCost = (int)nudMPCost.Value;
                        }
                        ActionToSave.UseCondition = txtActionCondition.Text;
                        ActionToSave.Description = txtActionDescription.Text;
                        ScrubNullEffects(ActionToSave.Effect, null);
                        ActionToSave.Name = nameToUse;
                        this.IsNewAction = RequiresActionName;
                        this.Saved = true;
                        this.Close();
                    }
                }
            }
        }

        private bool ValidateStepsBeforeSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (UsageCriteria != UsageCriteria.AnyTargetAnyTime && UsageCriteria != UsageCriteria.AnyTarget)
            {
                if (!chkAllies.Checked && !chkEnemies.Checked && !chkSelf.Checked)
                {
                    errorMessages.Add("Action is not set to be targetable to anyone.");
                }
                else if ((chkAllies.Checked || chkEnemies.Checked) && !chkSelf.Checked && (int)nudMaxRange.Value < 1)
                {
                    errorMessages.Add("Action is set to be targetable to Allies or Enemies, but can only be aimed at the User's own Tile.");
                }
                else if (chkSelf.Checked && !chkAllies.Checked && !chkEnemies.Checked && (int)nudMinRange.Value > 0)
                {
                    errorMessages.Add("Action is set to be targetable only to Self, but cannot be aimed at the User's own Tile.");
                }
            }
            if (RequiresDescription && string.IsNullOrWhiteSpace(txtActionDescription.Text))
                errorMessages.Add("Action does not have a Description.");
            return !errorMessages.Any();
        }

        private void txtActionDescription_TextChanged(object sender, EventArgs e)
        {
            txtActionDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblActionDescriptionLocale);
        }

        private void txtActionCondition_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtActionCondition.Text;
        }

        private void txtActionCondition_Leave(object sender, EventArgs e)
        {
            if (!txtActionCondition.Text.TestBooleanExpression(out string errorMessage))
            {
                MessageBox.Show(
                    $"You have entered an invalid value: {errorMessage}.",
                    "Invalid condition",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                txtActionCondition.Text = PreviousTextBoxValue;
            }
            fklblConditionWarning.Visible = !string.IsNullOrWhiteSpace(txtActionCondition.Text);
        }
    }

    public class EffectInfoDto
    {
        public string Moment { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<(string DisplayName, string Value)> Parameters { get; set; } = new List<(string DisplayName, string Value)>();

        public EffectInfoDto(EffectInfo info, EffectInfo parentEffect, List<EffectTypeData> selectableEffects)
        {
            var effectData = selectableEffects.Find(se => se.InternalName.Equals(info.EffectName));
            try
            {
                if (parentEffect == null)
                {
                    Moment = "INITIAL";
                }
                else
                {
                    if (parentEffect.Then == info)
                        Moment = "THEN";
                    else if (parentEffect.OnSuccess == info)
                        Moment = "ON SUCCESS";
                    else if (parentEffect.OnFailure == info)
                        Moment = "ON FAILURE";
                }
                if (effectData != null)
                {
                    DisplayName = effectData.DisplayName;
                    Description = effectData.Description;
                    foreach (var parameter in info.Params)
                    {
                        var paramData = effectData.Parameters.Find(p => p.InternalName.Equals(parameter.ParamName, StringComparison.InvariantCultureIgnoreCase));
                        Parameters.Add((paramData.DisplayName, parameter.Value));
                    }
                }
                else
                {
                    DisplayName = "Do nothing";
                    Description = "No behaviour has been currently set for this step.";
                }
            }
            catch
            {
                // Ignore. Unmatchable effects will be skipped altogether.
            }
        }
    }

    public enum UsageCriteria
    {
        AnyTargetAnyTime,
        AnyTarget,
        FullConditions
    }
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

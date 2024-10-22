using RogueCustomsDungeonEditor.Clipboard;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using static RogueCustomsGameEngine.Game.Entities.Effect;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RogueCustomsDungeonEditor.HelperForms
{
#pragma warning disable S2259 // Null pointers should not be dereferenced
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public partial class frmActionEdit : Form
    {
        public ActionWithEffectsInfo ActionToSave { get; set; }
        public bool Saved { get; set; }
        public bool IsNewAction { get; set; }
        private readonly DungeonInfo ActiveDungeon;
        private readonly List<string> UsableNPCList;
        private readonly List<string> UsableItemList;
        private readonly List<string> UsableTrapList;
        private readonly List<string> UsableAlteredStatusList;
        private readonly List<EffectTypeData> SelectableEffects;

        private TreeNode SelectedNode;
        private bool HasThenChildNode;
        private bool HasOnSuccessFailureChildNodes;
        private readonly bool RequiresDescription;
        private readonly bool RequiresActionName;
        private readonly bool CanHaveCondition;
        private readonly TurnEndCriteria TurnEndCriteria;
        private readonly UsageCriteria UsageCriteria;
        private readonly string ClassId;
        private readonly string PlaceholderActionName;
        private string PreviousTextBoxValue;
        public frmActionEdit(ActionWithEffectsInfo? actionToSave, DungeonInfo activeDungeon, string classId, string actionTypeText, bool canHaveCondition, bool requiresDescription, bool requiresActionName, TurnEndCriteria turnEndCriteria, string placeholderActionNameIfNeeded, UsageCriteria usageCriteria, List<EffectTypeData> selectableEffects, string thisDescription, string sourceDescription, string targetDescription)
        {
            InitializeComponent();
            if (!actionToSave.IsNullOrEmpty())
                ActionToSave = actionToSave.Clone();
            else
            {
                ActionToSave = new ActionWithEffectsInfo();
                ActionToSave.FinishesTurnWhenUsed = turnEndCriteria != TurnEndCriteria.CannotEndTurn;
            }
            ActiveDungeon = activeDungeon;
            EffectInfoDto.StatIds = ActiveDungeon.CharacterStats.Select(s => s.Id).ToList();
            ClassId = classId;
            if (actionToSave.IsNullOrEmpty())
                this.Text = "Action Editor - [New Action]";
            else
                this.Text = $"Action Editor - [{actionToSave.Name}]";
            if (!string.IsNullOrWhiteSpace(ClassId))
                lblTitle.Text = $"Edit {actionTypeText} for {ClassId}";
            else
                lblTitle.Text = $"Edit {actionTypeText} for Floor Group";
            btnSaveAs.Enabled = requiresActionName;
            RequiresDescription = requiresDescription;
            RequiresActionName = requiresActionName;
            CanHaveCondition = canHaveCondition;
            TurnEndCriteria = turnEndCriteria;

            lblThis.Text = thisDescription;
            lblSource.Text = sourceDescription;
            lblTarget.Text = targetDescription;

            if (!RequiresActionName)
                PlaceholderActionName = placeholderActionNameIfNeeded;

            UsageCriteria = usageCriteria;

            if (RequiresDescription)
            {
                txtActionDescription.Enabled = true;
                txtActionDescription.Text = ActionToSave?.Description;
            }
            else
            {
                txtActionDescription.Enabled = false;
                txtActionDescription.Text = "";
            }

            if (CanHaveCondition)
            {
                txtActionCondition.Enabled = true;
                txtActionCondition.Text = ActionToSave?.UseCondition;
                fklblConditionWarning.Visible = !string.IsNullOrWhiteSpace(txtActionCondition.Text);
                txtActionAICondition.Enabled = true;
                txtActionAICondition.Text = ActionToSave?.AIUseCondition;
                fklblAIConditionWarning.Visible = !string.IsNullOrWhiteSpace(txtActionAICondition.Text);
            }
            else
            {
                txtActionCondition.Enabled = false;
                txtActionCondition.Text = "";
                fklblConditionWarning.Visible = false;
                txtActionAICondition.Enabled = false;
                txtActionAICondition.Text = "";
                fklblAIConditionWarning.Visible = false;
            }

            if (TurnEndCriteria == TurnEndCriteria.CannotEndTurn)
            {
                chkFinishesTurn.Enabled = false;
                chkFinishesTurn.Checked = false;
            }
            else if (TurnEndCriteria == TurnEndCriteria.MustEndTurn)
            {
                chkFinishesTurn.Enabled = false;
                chkFinishesTurn.Checked = true;
            }
            else
            {
                chkFinishesTurn.Enabled = true;
                chkFinishesTurn.Checked = ActionToSave.FinishesTurnWhenUsed;
            }

            UsableNPCList = activeDungeon.NPCs.ConvertAll(npc => npc.Id);
            UsableItemList = activeDungeon.Items.ConvertAll(i => i.Id);
            UsableTrapList = activeDungeon.Traps.ConvertAll(t => t.Id);
            UsableAlteredStatusList = activeDungeon.AlteredStatuses.ConvertAll(als => als.Id);
            SelectableEffects = selectableEffects;
            RefreshActionSequenceTree();

            if (usageCriteria == UsageCriteria.AnyTargetAnyTime)
            {
                rbEntity.Enabled = false;
                rbEntity.Checked = true;
                rbTile.Enabled = false;
                gbSelectionCriteria.Enabled = false;
            }
            else if (usageCriteria == UsageCriteria.AnyTarget)
            {
                chkAllies.Enabled = false;
                chkEnemies.Enabled = false;
                chkSelf.Enabled = false;
                rbEntity.Enabled = false;
                rbEntity.Checked = true;
                rbTile.Enabled = false;
                nudMinRange.Enabled = false;
                nudMaxRange.Enabled = false;
                nudMPCost.Enabled = false;
                nudCooldown.Value = ActionToSave?.CooldownBetweenUses ?? 0;
                nudInitialCooldown.Value = ActionToSave?.StartingCooldown ?? 0;
                nudMaximumUses.Value = ActionToSave?.MaximumUses ?? 0;
            }
            else
            {
                rbTile.Checked = ActionToSave?.TargetTypes?.Contains("Tile", StringComparer.InvariantCultureIgnoreCase) == true;
                rbEntity.Checked = !rbTile.Checked;
                chkAllies.Checked = ActionToSave?.TargetTypes?.Contains("Ally", StringComparer.InvariantCultureIgnoreCase) == true;
                chkEnemies.Checked = ActionToSave?.TargetTypes?.Contains("Enemy", StringComparer.InvariantCultureIgnoreCase) == true;
                chkSelf.Checked = ActionToSave?.TargetTypes?.Contains("Self", StringComparer.InvariantCultureIgnoreCase) == true;
                nudMinRange.Value = ActionToSave?.MinimumRange ?? 0;
                nudMaxRange.Value = ActionToSave?.MaximumRange ?? 0;
                nudCooldown.Value = ActionToSave?.CooldownBetweenUses ?? 0;
                nudInitialCooldown.Value = ActionToSave?.StartingCooldown ?? 0;
                nudMaximumUses.Value = ActionToSave?.MaximumUses ?? 0;
                nudMPCost.Value = ActionToSave?.MPCost ?? 0;
                lblNoCooldown.Visible = nudCooldown.Value < 2;
                lblInfiniteUse.Visible = nudMaximumUses.Value == 0;
            }
            ClipboardManager.ClipboardContentsChanged += ClipboardManager_ClipboardContentsChanged;
        }

        private void RefreshActionSequenceTree()
        {
            tvEffectSequence.SelectedNode = null;
            tvEffectSequence.Nodes.Clear();
            if (ActionToSave.IsNullOrEmpty())
                ActionToSave = new ActionWithEffectsInfo();
            if (ActionToSave.Effect.IsNullOrEmpty())
                ActionToSave.Effect = new EffectInfo();
            AddActionNode(new EffectInfoDto(ActionToSave.Effect, null, SelectableEffects), null, ActionToSave.Effect);
            tvEffectSequence.Invalidate();
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
                btnCopyStep.Enabled = true;
                btnPasteStep.Enabled = ClipboardManager.ContainsData(FormConstants.StepClipboardKey);
            }
            else
            {
                btnEdit.Enabled = false;
                btnRemove.Enabled = false;
                btnNewThen.Enabled = false;
                btnNewOnSuccessFailure.Enabled = false;
                btnCopyStep.Enabled = false;
                btnPasteStep.Enabled = false;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null) return;
            var currentEffect = (EffectInfo)SelectedNode.Tag;
            var parentEffect = SelectedNode.Parent != null ? (EffectInfo)SelectedNode.Parent.Tag : null;
            var currentEffectDisplayName = SelectableEffects.Find(se => se.InternalName.Equals(currentEffect?.EffectName))?.ComboBoxDisplayName;
            var inputBoxPrompt = currentEffect.IsNullOrEmpty()
                ? "You may change the function if you wish.\n\nAll parameters will display as default if you do, however."
                : "Please indicate the function that will be executed in this step.";
            var selectableEffectsToShow = rbEntity.Checked
                ? SelectableEffects.Where(se => se.CanBeUsedOnEntity).Select(se => se.ComboBoxDisplayName).ToList()
                : SelectableEffects.Where(se => se.CanBeUsedOnTile).Select(se => se.ComboBoxDisplayName).ToList();
            var effectTypeSelection = ComboInputBox.Show(inputBoxPrompt, "Edit Step", selectableEffectsToShow, currentEffectDisplayName);
            if (effectTypeSelection == null) return;
            var selectedEffectTypeData = SelectableEffects.Find(se => se.ComboBoxDisplayName.Equals(effectTypeSelection));
            if (selectedEffectTypeData == null) return;
            if (!currentEffect.IsNullOrEmpty() && !selectedEffectTypeData.InternalName.Equals(currentEffect?.EffectName) && (currentEffect.Then != null || currentEffect.OnSuccess != null || currentEffect.OnFailure != null))
            {
                if (!currentEffect.Then.IsNullOrEmpty() || !currentEffect.OnSuccess.IsNullOrEmpty() || !currentEffect.OnFailure.IsNullOrEmpty())
                {
                    var messageBoxResult = MessageBox.Show(
                        "This Function has child steps. If you change the function and save it, the child steps will be completely erased!\n\nAre you sure you want to continue?",
                        "Add THEN Step",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.No)
                        return;
                }
                else
                {
                    var messageBoxResult = MessageBox.Show(
                        "This Function has 'Do Nothing' child steps. If you save any changes you make, the child steps will be completely erased!\n\nAre you sure you want to continue?",
                        "Add THEN Step",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.No)
                        return;
                }
            }
            var frmActionParameter = new frmActionParameters(currentEffect,
                                                             ActiveDungeon,
                                                             selectedEffectTypeData,
                                                             UsableNPCList,
                                                             UsableItemList,
                                                             UsableTrapList,
                                                             UsableAlteredStatusList,
                                                             selectedEffectTypeData.InternalName.Equals(currentEffect?.EffectName));
            frmActionParameter.ShowDialog();
            if (frmActionParameter.Saved)
            {
                if (!parentEffect.IsNullOrEmpty())
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

                if (parentEffect.IsNullOrEmpty())
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

                if (!parentEffect.IsNullOrEmpty())
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
            if (!parentEffect.IsNullOrEmpty())
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
            if (!parentEffect.IsNullOrEmpty())
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
                    if (ActionToSave.IsNullOrEmpty())
                    {
                        var messageBoxResult = MessageBox.Show(
                            "This Function has NO steps. If saved, it will be completely erased!\n\nAre you sure you want to continue?",
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
                            if (rbEntity.Checked)
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
                            }
                            else if (rbTile.Checked)
                            {
                                ActionToSave.TargetTypes = new List<string> { "Tile" };
                            }
                            ActionToSave.MinimumRange = (int)nudMinRange.Value;
                            ActionToSave.MaximumRange = (int)nudMaxRange.Value;
                            ActionToSave.CooldownBetweenUses = (int)nudCooldown.Value;
                            ActionToSave.StartingCooldown = (int)nudInitialCooldown.Value;
                            ActionToSave.MaximumUses = (int)nudMaximumUses.Value;
                            ActionToSave.MPCost = (int)nudMPCost.Value;
                        }
                        ActionToSave.UseCondition = txtActionCondition.Text;
                        ActionToSave.FinishesTurnWhenUsed = chkFinishesTurn.Checked;
                        ActionToSave.Description = txtActionDescription.Text;
                        if (!RequiresActionName)
                            ActionToSave.Name = PlaceholderActionName;
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
                if (ActionToSave.IsNullOrEmpty())
                {
                    var messageBoxResult = MessageBox.Show(
                        "This Function has NO steps. Proceeding means it will not be saved.\n\nAre you sure you want to continue?",
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
                            if (rbEntity.Checked)
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
                            }
                            else if (rbTile.Checked)
                            {
                                ActionToSave.TargetTypes = new List<string> { "Tile" };
                            }
                            ActionToSave.MinimumRange = (int)nudMinRange.Value;
                            ActionToSave.MaximumRange = (int)nudMaxRange.Value;
                            ActionToSave.CooldownBetweenUses = (int)nudCooldown.Value;
                            ActionToSave.StartingCooldown = (int)nudInitialCooldown.Value;
                            ActionToSave.MaximumUses = (int)nudMaximumUses.Value;
                            ActionToSave.MPCost = (int)nudMPCost.Value;
                        }
                        ActionToSave.UseCondition = txtActionCondition.Text;
                        ActionToSave.FinishesTurnWhenUsed = chkFinishesTurn.Checked;
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
                if (rbEntity.Checked)
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

        private void txtActionAICondition_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtActionAICondition.Text;
        }

        private void txtActionAICondition_Leave(object sender, EventArgs e)
        {
            if (!txtActionAICondition.Text.TestBooleanExpression(out string errorMessage))
            {
                MessageBox.Show(
                    $"You have entered an invalid value: {errorMessage}.",
                    "Invalid condition",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                txtActionAICondition.Text = PreviousTextBoxValue;
            }
            fklblAIConditionWarning.Visible = !string.IsNullOrWhiteSpace(txtActionAICondition.Text);
        }

        private void rbEntity_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbTile.Checked)
            {
                var messageBoxResult = MessageBox.Show(
                    "Changing the target to an Entity will remove the current target types as well as ALL steps.\n\nThis action is NOT reversible.\n\nAre you sure you want to continue?",
                    "Change Target Types",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.No)
                {
                    rbEntity.Checked = false;
                }
                else
                {
                    ActionToSave.Effect = null;
                    RefreshActionSequenceTree();
                }
            }
            pnlCharacterTargets.Visible = rbEntity.Checked;
        }

        private void rbTile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTile.Checked && (chkAllies.Checked || chkEnemies.Checked || chkSelf.Checked))
            {
                var messageBoxResult = MessageBox.Show(
                    "Changing the target to a Tile will remove the current target types as well as ALL steps.\n\nThis action is NOT reversible.\n\nAre you sure you want to continue?",
                    "Change Target Types",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.No)
                {
                    rbTile.Checked = false;
                }
                else
                {
                    chkAllies.Checked = false;
                    chkEnemies.Checked = false;
                    chkSelf.Checked = false;
                    ActionToSave.Effect = null;
                    RefreshActionSequenceTree();
                }
            }
            pnlCharacterTargets.Visible = !rbTile.Checked;
        }

        private void btnCopyStep_Click(object sender, EventArgs e)
        {
            if (tvEffectSequence.SelectedNode == null) return;
            if (tvEffectSequence.SelectedNode?.Tag is not EffectInfo itemTag) return;
            if (itemTag.IsNullOrEmpty()) return;
            var clonedEffect = itemTag.Clone();
            clonedEffect.Then = null;
            clonedEffect.OnSuccess = null;
            clonedEffect.OnFailure = null;
            ClipboardManager.Copy(FormConstants.StepClipboardKey, clonedEffect);
        }

        private void btnPasteStep_Click(object sender, EventArgs e)
        {
            if (!ClipboardManager.ContainsData(FormConstants.StepClipboardKey)) return;
            var currentEffect = (EffectInfo)SelectedNode.Tag;
            var parentEffect = SelectedNode.Parent != null ? (EffectInfo)SelectedNode.Parent.Tag : null;
            if (!currentEffect.Then.IsNullOrEmpty() || !currentEffect.OnSuccess.IsNullOrEmpty() || !currentEffect.OnFailure.IsNullOrEmpty())
            {
                var messageBoxResult = MessageBox.Show(
                    "This Function has child steps. Pasting this Function will make the child steps be completely erased!\n\nAre you sure you want to continue?",
                    "Add THEN Step",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.No)
                    return;
            }
            var effect = ClipboardManager.Paste<EffectInfo>(FormConstants.StepClipboardKey);
            if (!parentEffect.IsNullOrEmpty())
            {
                if (parentEffect.Then == currentEffect)
                    parentEffect.Then = effect;
                else if (parentEffect.OnSuccess == currentEffect)
                    parentEffect.OnSuccess = effect;
                else if (parentEffect.OnFailure == currentEffect)
                    parentEffect.OnFailure = effect;
            }
            else
            {
                tvEffectSequence.Nodes.Clear();
                ActionToSave.Effect = effect;
            }
            RefreshActionSequenceTree();
            tvEffectSequence.SelectNodeByTag(SelectedNode.Tag);
        }

        private void ClipboardManager_ClipboardContentsChanged(object? sender, EventArgs e)
        {
            btnPasteStep.Enabled = SelectedNode != null && ClipboardManager.ContainsData(FormConstants.StepClipboardKey);
        }

        private void frmActionEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClipboardManager.RemoveData(FormConstants.StepClipboardKey);
        }

        private void tvEffectSequence_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            string keywordPattern = @"\(([^()]+|(?<Depth>)\(|(?<-Depth>)\))*(?(Depth)(?!))\)";
            var nodeWords = e.Node.Text.SplitStringWithPattern(keywordPattern).ToList();

            e.Node.BackColor = Color.LightSkyBlue;

            int x = e.Bounds.Left;
            var spaceWidth = (int)e.Graphics.MeasureString(" ", tvEffectSequence.Font).Width;

            var nodeAsItIsDrawn = string.Empty;

            foreach (var word in nodeWords)
            {
                if (string.IsNullOrEmpty(word)) continue;
                var brushColor = Regex.IsMatch(word, keywordPattern)
                    ? Color.Blue
                    : Color.Black;

                var spacesToRemove = 1;
                if (word == " - ") spacesToRemove--;
                if (word.Contains("Remove")) spacesToRemove++;

                var wordToDraw = brushColor == Color.Blue
                    ? word.Substring(1, word.Length - 2)
                    : word;

                nodeAsItIsDrawn += wordToDraw;

                using (Brush brush = new SolidBrush(brushColor))
                {
                    SizeF wordSize = e.Graphics.MeasureString(wordToDraw != " - " ? wordToDraw.Trim() : wordToDraw, tvEffectSequence.Font);
                    e.Graphics.DrawString(wordToDraw != " - " ? wordToDraw.Trim() : wordToDraw, tvEffectSequence.Font, brush, x, e.Bounds.Top);
                    x += (int)wordSize.Width - (spaceWidth * spacesToRemove); // Close the gap between words
                }
            }
        }
    }

    public class EffectInfoDto
    {
        public string Moment { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<(string DisplayName, string Value)> Parameters { get; set; } = new List<(string DisplayName, string Value)>();
        public static List<string> StatIds = new();

        public EffectInfoDto(EffectInfo info, EffectInfo parentEffect, List<EffectTypeData> selectableEffects)
        {
            var effectData = selectableEffects.Find(se => se.InternalName.Equals(info.EffectName));
            try
            {
                if (parentEffect.IsNullOrEmpty())
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
                    DisplayName = effectData.GetParsedTreeViewDisplayName(info.Params, StatIds);
                    Description = effectData.Description;
                    foreach (var parameter in info.Params)
                    {
                        var paramData = effectData.Parameters.Find(p => p.InternalName.Equals(parameter.ParamName, StringComparison.InvariantCultureIgnoreCase));
                        if(paramData != null)
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

    public enum TurnEndCriteria
    {
        CannotEndTurn,
        MustEndTurn,
        MayNotEndTurn
    }

    public enum UsageCriteria
    {
        AnyTargetAnyTime,
        AnyTarget,
        FullConditions
    }
#pragma warning restore S2259 // Null pointers should not be dereferenced
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

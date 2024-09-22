﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class NPCTab : UserControl
    {
        private string PreviousTextBoxValue;
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        private Dictionary<string, string> BaseSightRangeDisplayNames;
        private Dictionary<string, string> NPCAITypeDisplayNames;
        public NPCInfo LoadedNPC { get; private set; }
        public event EventHandler TabInfoChanged;
        public NPCTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, NPCInfo npc, List<EffectTypeData> effectParamData, Dictionary<string, string> baseSightRangeDisplayNames, Dictionary<string, string> npcAITypeDisplayNames)
        {
            ActiveDungeon = dungeon;
            LoadedNPC = npc;
            BaseSightRangeDisplayNames = baseSightRangeDisplayNames;
            NPCAITypeDisplayNames = npcAITypeDisplayNames;
            EffectParamData = effectParamData;

            txtNPCName.Text = npc.Name;
            txtNPCDescription.Text = npc.Description;
            try
            {
                crsNPC.Character = npc.ConsoleRepresentation.Character;
                crsNPC.BackgroundColor = npc.ConsoleRepresentation.BackgroundColor;
                crsNPC.ForegroundColor = npc.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsNPC.Character = '\0';
                crsNPC.BackgroundColor = new GameColor(Color.Black);
                crsNPC.ForegroundColor = new GameColor(Color.White);
            }
            cmbNPCFaction.Items.Clear();
            cmbNPCFaction.Text = "";
            foreach (var factionId in ActiveDungeon.FactionInfos.Select(fi => fi.Id))
            {
                cmbNPCFaction.Items.Add(factionId);
                if (factionId.Equals(npc.Faction))
                    cmbNPCFaction.Text = factionId;
            }
            chkNPCStartsVisible.Checked = npc.StartsVisible;
            chkNPCKnowsAllCharacterPositions.Checked = npc.KnowsAllCharacterPositions;
            txtNPCExperiencePayout.Text = npc.ExperiencePayoutFormula;

            ssNPC.StatsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            ssNPC.BaseSightRangeDisplayNames = BaseSightRangeDisplayNames;
            ssNPC.BaseHP = npc.BaseHP;
            ssNPC.HPPerLevelUp = npc.MaxHPIncreasePerLevel;
            ssNPC.BaseHPRegeneration = npc.BaseHPRegeneration;
            ssNPC.HPRegenerationPerLevelUp = npc.HPRegenerationIncreasePerLevel;
            ssNPC.UsesMP = npc.UsesMP;
            ssNPC.BaseMP = npc.BaseMP;
            ssNPC.MPPerLevelUp = npc.MaxMPIncreasePerLevel;
            ssNPC.BaseMPRegeneration = npc.BaseMPRegeneration;
            ssNPC.MPRegenerationPerLevelUp = npc.MPRegenerationIncreasePerLevel;
            ssNPC.BaseAttack = npc.BaseAttack;
            ssNPC.AttackPerLevelUp = npc.AttackIncreasePerLevel;
            ssNPC.BaseDefense = npc.BaseDefense;
            ssNPC.DefensePerLevelUp = npc.DefenseIncreasePerLevel;
            ssNPC.BaseMovement = npc.BaseMovement;
            ssNPC.MovementPerLevelUp = npc.MovementIncreasePerLevel;
            ssNPC.BaseAccuracy = npc.BaseAccuracy;
            ssNPC.BaseEvasion = npc.BaseEvasion;
            ssNPC.BaseSightRange = npc.BaseSightRange;
            ssNPC.UsesHunger = npc.UsesHunger;
            ssNPC.BaseHunger = npc.BaseHunger;
            ssNPC.HungerHPDegeneration = npc.HungerHPDegeneration;
            ssNPC.CanGainExperience = npc.CanGainExperience;
            ssNPC.ExperienceToLevelUpFormula = npc.ExperienceToLevelUpFormula;
            ssNPC.MaxLevel = npc.MaxLevel;
            cmbNPCStartingWeapon.Items.Clear();
            cmbNPCStartingWeapon.Text = "";
            foreach (var weaponId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Weapon")).Select(i => i.Id))
            {
                cmbNPCStartingWeapon.Items.Add(weaponId);
                if (weaponId.Equals(npc.StartingWeapon))
                    cmbNPCStartingWeapon.Text = weaponId;
            }
            cmbNPCStartingArmor.Items.Clear();
            cmbNPCStartingArmor.Text = "";
            foreach (var armorId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Armor")).Select(i => i.Id))
            {
                cmbNPCStartingArmor.Items.Add(armorId);
                if (armorId.Equals(npc.StartingArmor))
                    cmbNPCStartingArmor.Text = armorId;
            }
            nudNPCInventorySize.Value = npc.InventorySize;
            sisNPCStartingInventory.SelectableItems = ActiveDungeon.Items.ConvertAll(i => i.Id);
            sisNPCStartingInventory.InventorySize = npc.InventorySize;
            sisNPCStartingInventory.Inventory = npc.StartingInventory;
            sisNPCStartingInventory.InventoryContentsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            SetSingleActionEditorParams(saeNPCOnTurnStart, npc.Id, npc.OnTurnStart);
            SetSingleActionEditorParams(saeNPCOnSpawn, npc.Id, npc.OnSpawn);
            SetMultiActionEditorParams(maeNPCOnAttack, npc.Id, npc.OnAttack);
            SetSingleActionEditorParams(saeNPCOnAttacked, npc.Id, npc.OnAttacked);
            SetMultiActionEditorParams(maeNPCOnInteracted, npc.Id, npc.OnInteracted);
            SetSingleActionEditorParams(saeNPCOnDeath, npc.Id, npc.OnDeath);
            cmbNPCAIType.Items.Clear();
            cmbNPCAIType.Text = "";
            nudNPCOddsToTargetSelf.Value = npc.AIOddsToUseActionsOnSelf;
            foreach (var aiType in NPCAITypeDisplayNames)
            {
                cmbNPCAIType.Items.Add(aiType.Value);
                if (aiType.Key.Equals(npc.AIType))
                {
                    cmbNPCAIType.Text = aiType.Value;
                    cmbNPCAIType_SelectedIndexChanged(null, EventArgs.Empty);
                }
            }
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNPCName.Text))
                validationErrors.Add("Enter an NPC Name first.");
            if (string.IsNullOrWhiteSpace(txtNPCDescription.Text))
                validationErrors.Add("Enter an NPC Description first.");
            if (crsNPC.Character == '\0')
                validationErrors.Add("This NPC does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(ssNPC.BaseSightRange))
                validationErrors.Add("This NPC does not have a Sight Range set.");
            if (string.IsNullOrWhiteSpace(cmbNPCFaction.Text))
                validationErrors.Add("This NPC does not have a Faction.");
            if (string.IsNullOrWhiteSpace(cmbNPCStartingWeapon.Text))
                validationErrors.Add("This NPC does not have an Emergency Weapon.");
            if (string.IsNullOrWhiteSpace(cmbNPCStartingArmor.Text))
                validationErrors.Add("This NPC does not have an Emergency Armor.");
            if (string.IsNullOrWhiteSpace(txtNPCExperiencePayout.Text))
                validationErrors.Add("This NPC does not have an Experience Payout Formula.");
            if (ssNPC.CanGainExperience && string.IsNullOrWhiteSpace(ssNPC.ExperienceToLevelUpFormula))
                validationErrors.Add("This NPC can gain experience, but does not have a Level Up Formula.");
            if (ssNPC.CanGainExperience && ssNPC.MaxLevel == 1)
                validationErrors.Add("This NPC can gain experience, but cannot level up.");
            if (ssNPC.MaxLevel > 1 && string.IsNullOrWhiteSpace(ssNPC.ExperienceToLevelUpFormula))
                validationErrors.Add("This NPC has a maximum level above 1, but does not have a Level Up Formula.");
            if (ssNPC.UsesHunger && ssNPC.BaseHunger <= 0)
                validationErrors.Add("This NPC uses a Hunger System but has no Hunger.");
            if (string.IsNullOrWhiteSpace(cmbNPCAIType.Text))
                validationErrors.Add("This NPC does not have a set AI strategy.");

            if (!validationErrors.Any())
            {
                LoadedNPC = new();
                LoadedNPC.Id = id;
                LoadedNPC.Name = txtNPCName.Text;
                LoadedNPC.Description = txtNPCDescription.Text;
                LoadedNPC.ConsoleRepresentation = crsNPC.ConsoleRepresentation;
                LoadedNPC.Faction = cmbNPCFaction.Text;
                LoadedNPC.StartsVisible = chkNPCStartsVisible.Checked;
                LoadedNPC.KnowsAllCharacterPositions = chkNPCKnowsAllCharacterPositions.Checked;
                LoadedNPC.ExperiencePayoutFormula = txtNPCExperiencePayout.Text;
                LoadedNPC.BaseHP = ssNPC.BaseHP;
                LoadedNPC.UsesMP = ssNPC.UsesMP;
                LoadedNPC.BaseMP = ssNPC.BaseMP;
                LoadedNPC.BaseAttack = ssNPC.BaseAttack;
                LoadedNPC.BaseDefense = ssNPC.BaseDefense;
                LoadedNPC.BaseMovement = ssNPC.BaseMovement;
                LoadedNPC.BaseAccuracy = ssNPC.BaseAccuracy;
                LoadedNPC.BaseEvasion = ssNPC.BaseEvasion;
                LoadedNPC.BaseHPRegeneration = ssNPC.BaseHPRegeneration;
                LoadedNPC.BaseMPRegeneration = ssNPC.BaseMPRegeneration;
                LoadedNPC.BaseSightRange = ssNPC.BaseSightRange;
                LoadedNPC.CanGainExperience = ssNPC.CanGainExperience;
                LoadedNPC.ExperienceToLevelUpFormula = ssNPC.ExperienceToLevelUpFormula;
                LoadedNPC.MaxLevel = ssNPC.MaxLevel;
                LoadedNPC.MaxHPIncreasePerLevel = ssNPC.HPPerLevelUp;
                LoadedNPC.MaxMPIncreasePerLevel = ssNPC.MPPerLevelUp;
                LoadedNPC.AttackIncreasePerLevel = ssNPC.AttackPerLevelUp;
                LoadedNPC.DefenseIncreasePerLevel = ssNPC.DefensePerLevelUp;
                LoadedNPC.MovementIncreasePerLevel = ssNPC.MovementPerLevelUp;
                LoadedNPC.HPRegenerationIncreasePerLevel = ssNPC.HPRegenerationPerLevelUp;
                LoadedNPC.MPRegenerationIncreasePerLevel = ssNPC.MPRegenerationPerLevelUp;

                LoadedNPC.UsesHunger = ssNPC.UsesHunger;
                LoadedNPC.BaseHunger = ssNPC.BaseHunger;
                LoadedNPC.HungerHPDegeneration = ssNPC.HungerHPDegeneration;

                LoadedNPC.StartingWeapon = cmbNPCStartingWeapon.Text;
                LoadedNPC.StartingArmor = cmbNPCStartingArmor.Text;

                LoadedNPC.InventorySize = (int)nudNPCInventorySize.Value;
                LoadedNPC.StartingInventory = sisNPCStartingInventory.Inventory;

                LoadedNPC.OnSpawn = saeNPCOnSpawn.Action;
                LoadedNPC.OnTurnStart = saeNPCOnTurnStart.Action;
                LoadedNPC.OnAttack = maeNPCOnAttack.Actions;
                LoadedNPC.OnAttacked = saeNPCOnAttacked.Action;
                LoadedNPC.OnInteracted = maeNPCOnInteracted.Actions;
                LoadedNPC.OnDeath = saeNPCOnDeath.Action;
                foreach (var aiType in NPCAITypeDisplayNames)
                {
                    if (aiType.Value.Equals(cmbNPCAIType.Text))
                    {
                        LoadedNPC.AIType = aiType.Key;
                    }
                }
                LoadedNPC.AIOddsToUseActionsOnSelf = (int)nudNPCOddsToTargetSelf.Value;
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

        private void txtNPCExperiencePayout_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtNPCExperiencePayout.Text;
        }

        private void txtNPCExperiencePayout_Leave(object sender, EventArgs e)
        {
            if (!PreviousTextBoxValue.Equals(txtNPCExperiencePayout.Text))
            {
                var parsedPayoutFormula = Regex.Replace(txtNPCExperiencePayout.Text, @"\blevel\b", "1", RegexOptions.IgnoreCase);

                if (!string.IsNullOrWhiteSpace(parsedPayoutFormula) && !parsedPayoutFormula.TestNumericExpression(false, out string errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid Experience Formula: {errorMessage}",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtNPCExperiencePayout.Text = PreviousTextBoxValue;
                }
                else
                {
                    TabInfoChanged?.Invoke(null, EventArgs.Empty);
                }
            }

            PreviousTextBoxValue = "";
        }

        private void chkNPCKnowsAllCharacterPositions_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCOddsToTargetSelf_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtNPCName_TextChanged(object sender, EventArgs e)
        {
            txtNPCName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblNPCNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtNPCDescription_TextChanged(object sender, EventArgs e)
        {
            txtNPCDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblNPCDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkNPCStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCStartingWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCStartingArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCInventorySize_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            sisNPCStartingInventory.InventorySize = (int)nudNPCInventorySize.Value;
        }

        private void crsNPC_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCAIType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNPCAIType.Text == NPCAITypeDisplayNames["Random"])
            {
                lblNPCAIOddsToTargetSelfA.Visible = true;
                lblNPCAIOddsToTargetSelfB.Visible = true;
                nudNPCOddsToTargetSelf.Visible = true;
            }
            else
            {
                lblNPCAIOddsToTargetSelfA.Visible = false;
                lblNPCAIOddsToTargetSelfB.Visible = false;
                nudNPCOddsToTargetSelf.Visible = false;
                nudNPCOddsToTargetSelf.Value = 0;
            }
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

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
            chkNPCPursuesOutOfSightCharacters.Checked = npc.PursuesOutOfSightCharacters;
            chkNPCWandersIfWithoutTarget.Checked = npc.WandersIfWithoutTarget;
            chkNPCReappearsOnTheNextFloorIfAlliedToThePlayer.Checked = npc.ReappearsOnTheNextFloorIfAlliedToThePlayer;

            txtNPCExperiencePayout.Text = npc.ExperiencePayoutFormula;

            ssNPC.StatsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            ssNPC.StatData = ActiveDungeon.CharacterStats;
            ssNPC.Stats = npc.Stats;
            ssNPC.BaseSightRangeDisplayNames = BaseSightRangeDisplayNames;
            ssNPC.BaseSightRange = npc.BaseSightRange;
            ssNPC.CanGainExperience = npc.CanGainExperience;
            ssNPC.ExperienceToLevelUpFormula = npc.ExperienceToLevelUpFormula;
            ssNPC.MaxLevel = npc.MaxLevel;

            clbNPCAvailableSlots.Items.Clear();
            foreach (var slot in ActiveDungeon.ItemSlotInfos)
            {
                clbNPCAvailableSlots.Items.Add(slot.Id, npc.AvailableSlots.Contains(slot.Id));
            }
            clbNPCAvailableSlots.ItemCheck -= clbNPCAvailableSlots_ItemCheck;
            clbNPCAvailableSlots.ItemCheck += clbNPCAvailableSlots_ItemCheck;
            esNPC.Dungeon = ActiveDungeon;
            esNPC.AvailableSlots = npc.AvailableSlots;
            esNPC.Equipment = npc.InitialEquipment;
            chkNPCDropsEquipmentOnDeath.Checked = npc.DropsEquipmentOnDeath;

            nudNPCInventorySize.Value = npc.InventorySize;
            sisNPCStartingInventory.SelectableItems = ActiveDungeon.Items.ConvertAll(i => i.Id);
            sisNPCStartingInventory.InventorySize = npc.InventorySize;
            sisNPCStartingInventory.Inventory = npc.StartingInventory;
            sisNPCStartingInventory.InventoryContentsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            SetSingleActionEditorParams(saeNPCDefaultOnAttack, npc.Id, npc.DefaultOnAttack);
            SetSingleActionEditorParams(saeNPCOnTurnStart, npc.Id, npc.OnTurnStart);
            SetSingleActionEditorParams(saeNPCBeforeProcessAI, npc.Id, npc.BeforeProcessAI);
            SetSingleActionEditorParams(saeNPCOnSpawn, npc.Id, npc.OnSpawn);
            SetMultiActionEditorParams(maeNPCOnAttack, npc.Id, npc.OnAttack);
            SetSingleActionEditorParams(saeNPCOnAttacked, npc.Id, npc.OnAttacked);
            SetMultiActionEditorParams(maeNPCOnInteracted, npc.Id, npc.OnInteracted);
            SetSingleActionEditorParams(saeNPCOnDeath, npc.Id, npc.OnDeath);
            SetSingleActionEditorParams(saeNPCOnLevelUp, npc.Id, npc.OnLevelUp);

            cmbNPCAIType.Items.Clear();
            cmbNPCAIType.Text = "";
            foreach (var aiType in NPCAITypeDisplayNames)
            {
                cmbNPCAIType.Items.Add(aiType.Value);
                if (aiType.Key.Equals(npc.AIType))
                    cmbNPCAIType.Text = aiType.Value;
            }

            nudNPCOddsForModifier.Value = npc.OddsForModifier;
            chkNPCRandomizesForecolorIfWithModifiers.Checked = npc.RandomizesForecolorIfWithModifiers;
            nudNPCExperienceYieldMultiplierIfWithModifiers.Value = npc.ExperienceYieldMultiplierIfWithModifiers;
            nudNPCBaseHPMultiplierIfWithModifiers.Value = npc.BaseHPMultiplierIfWithModifiers;

            var regularLootTableId = npc.RegularLootTable?.LootTableId ?? "None";
            var regularLootTableDropPicks = npc.RegularLootTable?.DropPicks ?? 0;
            var modifierLootTableId = npc.LootTableWithModifiers?.LootTableId ?? "None";
            var modifierLootTableDropPicks = npc.LootTableWithModifiers?.DropPicks ?? 0;

            cmbNPCLootTable.Items.Clear();
            cmbNPCLootTable.Items.Add("None");
            cmbNPCLootTable.Text = "None";
            cmbNPCLootTableModifier.Items.Clear();
            cmbNPCLootTableModifier.Items.Add("None");
            cmbNPCLootTableModifier.Text = "None";
            foreach (var lootTable in dungeon.LootTableInfos.ConvertAll(lt => lt.Id))
            {
                cmbNPCLootTable.Items.Add(lootTable);
                cmbNPCLootTableModifier.Items.Add(lootTable);
                if (lootTable.Equals(regularLootTableId))
                    cmbNPCLootTable.Text = lootTable;
                if (lootTable.Equals(modifierLootTableId))
                    cmbNPCLootTableModifier.Text = lootTable;
            }
            nudNPCDropPicks.Value = cmbNPCLootTable.Text != "None" ? regularLootTableDropPicks : 0;
            nudNPCDropPicksModifier.Value = cmbNPCLootTableModifier.Text != "None" ? modifierLootTableDropPicks : 0;

            dgvNPCModifiers.Rows.Clear();
            foreach (var modifierData in npc.ModifierData ?? [])
            {
                dgvNPCModifiers.Rows.Add(modifierData.Level, modifierData.ModifierAmount);
            }
            dgvNPCModifiers.CellValueChanged += (sender, e) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void clbNPCAvailableSlots_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            var checkedItems = clbNPCAvailableSlots.CheckedItems.Cast<string>().ToList();
            string currentItem = clbNPCAvailableSlots.Items[e.Index].ToString();

            // For some reason, ItemCheck fires BEFORE updating CheckedItems, so we need to make a manual observation
            if (e.NewValue == CheckState.Checked)
            {
                if (!checkedItems.Contains(currentItem))
                    checkedItems.Add(currentItem);
            }
            else
            {
                if (checkedItems.Contains(currentItem))
                    checkedItems.Remove(currentItem);
            }

            esNPC.AvailableSlots = checkedItems;
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        public List<string> SaveData(string id)
        {
            dgvNPCModifiers.EndEdit();
            esNPC.EndEdit();
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNPCName.Text))
                validationErrors.Add("Enter an NPC Name first.");
            if (string.IsNullOrWhiteSpace(txtNPCDescription.Text))
                validationErrors.Add("Enter an NPC Description first.");
            if (crsNPC.Character == '\0')
                validationErrors.Add("This NPC does not have a Console Representation character.");
            foreach (var stat in ssNPC.Stats)
            {
                if (stat.Base < stat.Minimum)
                    validationErrors.Add($"The Stat '{stat.StatId}' has a Base value less than its Minimum value.");
                if (stat.Base > stat.Maximum)
                    validationErrors.Add($"The Stat '{stat.StatId}' has a Base value greater than its Maximum value.");
                if (stat.Minimum > stat.Maximum)
                    validationErrors.Add($"The Stat '{stat.StatId}' has a Minimum value greater than its Maximum value.");
            }
            if (string.IsNullOrWhiteSpace(ssNPC.BaseSightRange))
                validationErrors.Add("This NPC does not have a Sight Range set.");
            if (string.IsNullOrWhiteSpace(cmbNPCFaction.Text))
                validationErrors.Add("This NPC does not have a Faction.");
            if (string.IsNullOrWhiteSpace(txtNPCExperiencePayout.Text))
                validationErrors.Add("This NPC does not have an Experience Payout Formula.");
            if (ssNPC.CanGainExperience && string.IsNullOrWhiteSpace(ssNPC.ExperienceToLevelUpFormula))
                validationErrors.Add("This NPC can gain experience, but does not have a Level Up Formula.");
            if (ssNPC.CanGainExperience && ssNPC.MaxLevel == 1)
                validationErrors.Add("This NPC can gain experience, but cannot level up.");
            if (ssNPC.MaxLevel > 1 && string.IsNullOrWhiteSpace(ssNPC.ExperienceToLevelUpFormula))
                validationErrors.Add("This NPC has a maximum level above 1, but does not have a Level Up Formula.");
            if ((cmbNPCLootTable.Text == "None" || string.IsNullOrWhiteSpace(cmbNPCLootTable.Text)) && nudNPCDropPicks.Value > 0)
                validationErrors.Add("This NPC is set to drop items as loot, but has no Loot Table.");
            if (cmbNPCLootTable.Text != "None" && !string.IsNullOrWhiteSpace(cmbNPCLootTable.Text) && nudNPCDropPicks.Value <= 0)
                validationErrors.Add("This NPC has a Loot Table set, but is set to drop no items as loot.");
            if (clbNPCAvailableSlots.CheckedItems.Count == 0)
                validationErrors.Add("This NPC does not have any Available Equipment Slots.");
            if (esNPC.EquipmentList.Count == 0 && chkNPCDropsEquipmentOnDeath.Checked)
                validationErrors.Add("This NPC is set to drop Equipment on death, but is not set to have any Equipment.");

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
                LoadedNPC.PursuesOutOfSightCharacters = chkNPCPursuesOutOfSightCharacters.Checked;
                LoadedNPC.WandersIfWithoutTarget = chkNPCWandersIfWithoutTarget.Checked;
                LoadedNPC.ExperiencePayoutFormula = txtNPCExperiencePayout.Text;
                LoadedNPC.Stats = ssNPC.Stats;
                LoadedNPC.BaseSightRange = ssNPC.BaseSightRange;
                LoadedNPC.CanGainExperience = ssNPC.CanGainExperience;
                LoadedNPC.ExperienceToLevelUpFormula = ssNPC.ExperienceToLevelUpFormula;
                LoadedNPC.MaxLevel = ssNPC.MaxLevel;

                LoadedNPC.DropsEquipmentOnDeath = chkNPCDropsEquipmentOnDeath.Checked;

                LoadedNPC.AvailableSlots = clbNPCAvailableSlots.CheckedItems.Cast<string>().ToList();

                LoadedNPC.InitialEquipment = [];

                foreach (var equipment in esNPC.EquipmentList)
                {
                    if (LoadedNPC.AvailableSlots.Contains(equipment.Key))
                        LoadedNPC.InitialEquipment.Add(equipment.Value);
                }

                LoadedNPC.InventorySize = (int)nudNPCInventorySize.Value;
                LoadedNPC.StartingInventory = sisNPCStartingInventory.Inventory;

                LoadedNPC.DefaultOnAttack = saeNPCDefaultOnAttack.Action;
                if (LoadedNPC.DefaultOnAttack != null)
                    LoadedNPC.DefaultOnAttack.IsScript = false;
                LoadedNPC.OnSpawn = saeNPCOnSpawn.Action;
                if (LoadedNPC.OnSpawn != null)
                    LoadedNPC.OnSpawn.IsScript = false;
                LoadedNPC.OnTurnStart = saeNPCOnTurnStart.Action;
                if (LoadedNPC.OnTurnStart != null)
                    LoadedNPC.OnTurnStart.IsScript = false;
                LoadedNPC.BeforeProcessAI = saeNPCBeforeProcessAI.Action;
                if (LoadedNPC.BeforeProcessAI != null)
                    LoadedNPC.BeforeProcessAI.IsScript = false;
                LoadedNPC.OnAttack = maeNPCOnAttack.Actions;
                foreach (var action in LoadedNPC.OnAttack)
                {
                    action.IsScript = false;
                }
                LoadedNPC.OnAttacked = saeNPCOnAttacked.Action;
                if (LoadedNPC.OnAttacked != null)
                    LoadedNPC.OnAttacked.IsScript = false;
                LoadedNPC.OnInteracted = maeNPCOnInteracted.Actions;
                foreach (var action in LoadedNPC.OnInteracted)
                {
                    action.IsScript = false;
                }
                LoadedNPC.OnDeath = saeNPCOnDeath.Action;
                if (LoadedNPC.OnDeath != null)
                    LoadedNPC.OnDeath.IsScript = false;
                LoadedNPC.OnLevelUp = saeNPCOnLevelUp.Action;
                if (LoadedNPC.OnLevelUp != null)
                    LoadedNPC.OnLevelUp.IsScript = false;
                foreach (var aiType in NPCAITypeDisplayNames)
                {
                    if (aiType.Value.Equals(cmbNPCAIType.Text))
                    {
                        LoadedNPC.AIType = aiType.Key;
                    }
                }

                var regularLootTableId = cmbNPCLootTable.Text != "None" ? cmbNPCLootTable.Text : "";
                var modifierLootTableId = cmbNPCLootTableModifier.Text != "None" ? cmbNPCLootTableModifier.Text : "";

                LoadedNPC.RegularLootTable = new()
                {
                    LootTableId = regularLootTableId,
                    DropPicks = regularLootTableId != "" ? (int)nudNPCDropPicks.Value : 0
                };
                LoadedNPC.LootTableWithModifiers = new()
                {
                    LootTableId = modifierLootTableId,
                    DropPicks = modifierLootTableId != "" ? (int)nudNPCDropPicksModifier.Value : 0
                };

                LoadedNPC.ReappearsOnTheNextFloorIfAlliedToThePlayer = chkNPCReappearsOnTheNextFloorIfAlliedToThePlayer.Checked;
                LoadedNPC.OddsForModifier = (int)nudNPCOddsForModifier.Value;
                LoadedNPC.RandomizesForecolorIfWithModifiers = chkNPCRandomizesForecolorIfWithModifiers.Checked;
                LoadedNPC.ExperienceYieldMultiplierIfWithModifiers = nudNPCExperienceYieldMultiplierIfWithModifiers.Value;
                LoadedNPC.BaseHPMultiplierIfWithModifiers = nudNPCBaseHPMultiplierIfWithModifiers.Value;

                LoadedNPC.ModifierData = new();
                foreach (DataGridViewRow row in dgvNPCModifiers.Rows)
                {
                    if (row.IsNewRow) continue;
                    var level = int.Parse(row.Cells["Level"].Value.ToString());
                    var amount = int.Parse(row.Cells["ModifierAmount"].Value.ToString());
                    LoadedNPC.ModifierData.Add(new NPCModifierDataInfo()
                    {
                        Level = level,
                        ModifierAmount = amount
                    });
                }
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

        private void chkNPCPursuesOutOfSightCharacters_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkNPCWandersIfWithoutTarget_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCLootTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            nudNPCDropPicks.Enabled = !cmbNPCLootTable.Text.Equals("None");
            if (!nudNPCDropPicks.Enabled)
                nudNPCDropPicks.Value = 0;
        }

        private void nudNPCDropPicks_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            if (nudNPCDropPicks.Value == 0)
                cmbNPCLootTable.Text = "None";
        }

        private void cmbNPCLootTableModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            nudNPCDropPicksModifier.Enabled = !cmbNPCLootTableModifier.Text.Equals("None");
            if (!nudNPCDropPicksModifier.Enabled)
                nudNPCDropPicksModifier.Value = 0;
        }

        private void nudNPCDropPicksModifier_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            if (nudNPCDropPicksModifier.Value == 0)
                cmbNPCLootTableModifier.Text = "None";
        }

        private void nudNPCOddsForModifier_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkNPCRandomizesForecolorIfWithModifiers_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCExperienceYieldMultiplierIfWithModifiers_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void dgvNPCModifiers_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvNPCModifiers.Rows[e.RowIndex].IsNewRow) return;
            var cellValue = dgvNPCModifiers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            PreviousTextBoxValue = cellValue != null ? cellValue.ToString() : string.Empty;
        }

        private void dgvNPCModifiers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvNPCModifiers.Rows[e.RowIndex].IsNewRow) return;
            var cellValue = dgvNPCModifiers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;
            var minValue = (dgvNPCModifiers.Columns[e.ColumnIndex].Name == "Level") ? 1 : 0;

            if (!int.TryParse(cellValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int number) || number < minValue)
            {
                dgvNPCModifiers[e.ColumnIndex, e.RowIndex].Value = PreviousTextBoxValue;
            }
        }

        private void chkNPCReappearsOnTheNextFloorIfAlliedToThePlayer_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCBaseHPMultiplierIfWithModifiers_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

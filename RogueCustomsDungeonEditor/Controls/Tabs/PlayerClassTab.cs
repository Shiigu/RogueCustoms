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
    public partial class PlayerClassTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        private Dictionary<string, string> BaseSightRangeDisplayNames;
        public PlayerClassInfo LoadedPlayerClass { get; private set; }
        public event EventHandler TabInfoChanged;
        public PlayerClassTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, PlayerClassInfo playerClass, List<EffectTypeData> effectParamData, Dictionary<string, string> baseSightRangeDisplayNames)
        {
            ActiveDungeon = dungeon;
            LoadedPlayerClass = playerClass;
            BaseSightRangeDisplayNames = baseSightRangeDisplayNames;
            EffectParamData = effectParamData;
            txtPlayerClassName.Text = playerClass.Name;
            chkRequirePlayerPrompt.Checked = playerClass.RequiresNamePrompt;
            txtPlayerClassDescription.Text = playerClass.Description;
            try
            {
                crsPlayer.Character = playerClass.ConsoleRepresentation.Character;
                crsPlayer.BackgroundColor = playerClass.ConsoleRepresentation.BackgroundColor;
                crsPlayer.ForegroundColor = playerClass.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsPlayer.Character = '\0';
                crsPlayer.BackgroundColor = new GameColor(Color.Black);
                crsPlayer.ForegroundColor = new GameColor(Color.White);
            }
            cmbPlayerFaction.Items.Clear();
            cmbPlayerFaction.Text = "";
            foreach (var factionId in ActiveDungeon.FactionInfos.Select(fi => fi.Id))
            {
                cmbPlayerFaction.Items.Add(factionId);
                if (factionId.Equals(playerClass.Faction))
                    cmbPlayerFaction.Text = factionId;
            }
            chkPlayerStartsVisible.Checked = playerClass.StartsVisible;
            ssPlayer.StatsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            ssPlayer.BaseSightRangeDisplayNames = BaseSightRangeDisplayNames;
            ssPlayer.StatInfos = ActiveDungeon.CharacterStats;
            ssPlayer.Stats = playerClass.Stats;
            ssPlayer.BaseSightRange = playerClass.BaseSightRange;
            ssPlayer.CanGainExperience = playerClass.CanGainExperience;
            ssPlayer.ExperienceToLevelUpFormula = playerClass.ExperienceToLevelUpFormula;
            ssPlayer.MaxLevel = playerClass.MaxLevel;
            cmbPlayerStartingWeapon.Items.Clear();
            cmbPlayerStartingWeapon.Text = "";
            foreach (var weaponId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Weapon")).Select(i => i.Id))
            {
                cmbPlayerStartingWeapon.Items.Add(weaponId);
                if (weaponId.Equals(playerClass.StartingWeapon))
                    cmbPlayerStartingWeapon.Text = weaponId;
            }
            cmbPlayerStartingArmor.Items.Clear();
            cmbPlayerStartingArmor.Text = "";
            foreach (var armorId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Armor")).Select(i => i.Id))
            {
                cmbPlayerStartingArmor.Items.Add(armorId);
                if (armorId.Equals(playerClass.StartingArmor))
                    cmbPlayerStartingArmor.Text = armorId;
            }
            nudPlayerInventorySize.Value = playerClass.InventorySize;
            sisPlayerStartingInventory.SelectableItems = ActiveDungeon.Items.ConvertAll(i => i.Id);
            sisPlayerStartingInventory.InventorySize = playerClass.InventorySize;
            sisPlayerStartingInventory.Inventory = playerClass.StartingInventory;
            sisPlayerStartingInventory.InventoryContentsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            SetSingleActionEditorParams(saePlayerOnTurnStart, playerClass.Id, playerClass.OnTurnStart);
            SetMultiActionEditorParams(maePlayerOnAttack, playerClass.Id, playerClass.OnAttack);
            SetSingleActionEditorParams(saePlayerOnAttacked, playerClass.Id, playerClass.OnAttacked);
            SetSingleActionEditorParams(saePlayerOnDeath, playerClass.Id, playerClass.OnDeath);
            SetSingleActionEditorParams(saePlayerOnLevelUp, playerClass.Id, playerClass.OnLevelUp);
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtPlayerClassName.Text))
                validationErrors.Add("Enter a Player Class Name first.");
            if (string.IsNullOrWhiteSpace(txtPlayerClassDescription.Text))
                validationErrors.Add("Enter a Player Class Description first.");
            if (crsPlayer.ConsoleRepresentation.Character == '\0')
                validationErrors.Add("This Player Class does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(ssPlayer.BaseSightRange))
                validationErrors.Add("This Player Class does not have a Sight Range set.");
            if (string.IsNullOrWhiteSpace(cmbPlayerFaction.Text))
                validationErrors.Add("This Player Class does not have a Faction.");
            if (string.IsNullOrWhiteSpace(cmbPlayerStartingWeapon.Text))
                validationErrors.Add("This Player Class does not have an Emergency Weapon.");
            if (string.IsNullOrWhiteSpace(cmbPlayerStartingArmor.Text))
                validationErrors.Add("This Player Class does not have an Emergency Armor.");
            if (ssPlayer.CanGainExperience && string.IsNullOrWhiteSpace(ssPlayer.ExperienceToLevelUpFormula))
                validationErrors.Add("This Player Class can gain experience, but does not have a Level Up Formula.");
            if (ssPlayer.CanGainExperience && ssPlayer.MaxLevel == 1)
                validationErrors.Add("This Player Class can gain experience, but cannot level up.");

            if (!validationErrors.Any())
            {
                LoadedPlayerClass = new();
                LoadedPlayerClass.Id = id;
                LoadedPlayerClass.Name = txtPlayerClassName.Text;
                LoadedPlayerClass.RequiresNamePrompt = chkRequirePlayerPrompt.Checked;
                LoadedPlayerClass.Description = txtPlayerClassDescription.Text;
                LoadedPlayerClass.ConsoleRepresentation = crsPlayer.ConsoleRepresentation;
                LoadedPlayerClass.Faction = cmbPlayerFaction.Text;
                LoadedPlayerClass.StartsVisible = chkPlayerStartsVisible.Checked;
                LoadedPlayerClass.Stats = ssPlayer.Stats;
                LoadedPlayerClass.BaseSightRange = ssPlayer.BaseSightRange;
                LoadedPlayerClass.CanGainExperience = ssPlayer.CanGainExperience;
                LoadedPlayerClass.ExperienceToLevelUpFormula = ssPlayer.ExperienceToLevelUpFormula;
                LoadedPlayerClass.MaxLevel = ssPlayer.MaxLevel;

                LoadedPlayerClass.StartingWeapon = cmbPlayerStartingWeapon.Text;
                LoadedPlayerClass.StartingArmor = cmbPlayerStartingArmor.Text;

                LoadedPlayerClass.InventorySize = (int)nudPlayerInventorySize.Value;
                LoadedPlayerClass.StartingInventory = sisPlayerStartingInventory.Inventory;

                LoadedPlayerClass.OnTurnStart = saePlayerOnTurnStart.Action;
                if (LoadedPlayerClass.OnTurnStart != null)
                    LoadedPlayerClass.OnTurnStart.IsScript = false;
                LoadedPlayerClass.OnAttack = maePlayerOnAttack.Actions;
                foreach (var action in LoadedPlayerClass.OnAttack)
                {
                    action.IsScript = false;
                }
                LoadedPlayerClass.OnAttacked = saePlayerOnAttacked.Action;
                if (LoadedPlayerClass.OnAttacked != null)
                    LoadedPlayerClass.OnAttacked.IsScript = false;
                LoadedPlayerClass.OnDeath = saePlayerOnDeath.Action;
                if (LoadedPlayerClass.OnDeath != null)
                    LoadedPlayerClass.OnDeath.IsScript = false;
                LoadedPlayerClass.OnLevelUp = saePlayerOnLevelUp.Action;
                if (LoadedPlayerClass.OnLevelUp != null)
                    LoadedPlayerClass.OnLevelUp.IsScript = false;
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
        private void txtPlayerClassName_TextChanged(object sender, EventArgs e)
        {
            txtPlayerClassName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblPlayerClassNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkRequirePlayerPrompt_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtPlayerClassDescription_TextChanged(object sender, EventArgs e)
        {
            txtPlayerClassDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblPlayerClassDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbPlayerFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkPlayerStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbPlayerStartingWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbPlayerStartingArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudPlayerInventorySize_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            sisPlayerStartingInventory.InventorySize = (int)nudPlayerInventorySize.Value;
        }

        private void crsPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

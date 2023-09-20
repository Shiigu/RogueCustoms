using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    public partial class frmNPCGeneration : Form
    {
        private FloorInfo ActiveFloorGroup;
        public NPCGenerationParams NPCGenerationParams { get; private set; }
        public bool Saved { get; private set; }
        private List<string> ValidNPCClasses;

        public frmNPCGeneration(FloorInfo floorGroupToUse, NPCGenerationParams npcGenerationParams, DungeonInfo activeDungeon)
        {
            InitializeComponent();
            ActiveFloorGroup = floorGroupToUse;
            NPCGenerationParams = npcGenerationParams;
            ValidNPCClasses = activeDungeon.NPCs.ConvertAll(npc => npc.Id);
            var classIdColumn = (DataGridViewComboBoxColumn)dgvNPCTable.Columns["ClassId"];
            classIdColumn.DataSource = ValidNPCClasses;
            var npcTable = new BindingList<ClassInFloorInfo>()
            {
                AllowNew = true,
                AllowRemove = true
            };
            foreach (var npc in NPCGenerationParams.NPCList ?? floorGroupToUse.PossibleMonsters)
            {
                npcTable.Add(npc);
            }
            dgvNPCTable.DataSource = npcTable;
            nudMinNPCSpawnsAtStart.Value = floorGroupToUse.SimultaneousMinMonstersAtStart;
            nudSimultaneousMaxNPCs.Value = floorGroupToUse.SimultaneousMaxMonstersInFloor;
            nudTurnsPerNPCGeneration.Value = floorGroupToUse.TurnsPerMonsterGeneration;
            if (floorGroupToUse.MinFloorLevel != floorGroupToUse.MaxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {floorGroupToUse.MinFloorLevel} to {floorGroupToUse.MaxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {floorGroupToUse.MinFloorLevel}:";
        }

        private bool ValidateAndPrepareListForSave(out List<ClassInFloorInfo> npcList, out List<string> errorMessages)
        {
            npcList = new List<ClassInFloorInfo>();
            errorMessages = new List<string>();
            try
            {
                foreach (DataGridViewRow row in dgvNPCTable.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var npcRow = new ClassInFloorInfo();
                        npcRow.ClassId = row.Cells["ClassId"].Value?.ToString();
                        npcRow.MinLevel = int.Parse(row.Cells["MinLevel"].Value?.ToString());
                        npcRow.MaxLevel = int.Parse(row.Cells["MaxLevel"].Value?.ToString());
                        npcRow.SimultaneousMaxForKindInFloor = int.Parse(row.Cells["SimultaneousMaxForKindInFloor"].Value?.ToString());
                        npcRow.OverallMaxForKindInFloor = int.Parse(row.Cells["OverallMaxForKindInFloor"].Value?.ToString());
                        npcRow.ChanceToPick = int.Parse(row.Cells["ChanceToPick"].Value?.ToString());
                        npcRow.CanSpawnOnFirstTurn = bool.Parse(row.Cells["CanSpawnOnFirstTurn"].Value?.ToString());
                        npcRow.CanSpawnAfterFirstTurn = bool.Parse(row.Cells["CanSpawnAfterFirstTurn"].Value?.ToString());
                        npcList.Add(npcRow);
                    }
                }

                if (npcList.Count > 0)
                {

                    var groupedConditionNpcs = npcList.GroupBy(npc => new
                    {
                        npc.ClassId,
                        npc.MinLevel,
                        npc.MaxLevel,
                        npc.CanSpawnOnFirstTurn,
                        npc.CanSpawnAfterFirstTurn
                    }
                    );
                    foreach (var group in groupedConditionNpcs)
                    {
                        if (group.Count() > 1)
                            errorMessages.Add($"There is more than one entry for {group.First().ClassId} that applies\nunder the same condition. Either remove one or change their levels or spawn conditions.");

                        var npc = group.First();

                        if (string.IsNullOrWhiteSpace(npc.ClassId))
                        {
                            errorMessages.Add("At least one row is lacking an NPC Class.");
                        }
                        else if (!ValidNPCClasses.Contains(npc.ClassId))
                        {
                            errorMessages.Add($"{npc.ClassId} is not a recognized NPC Class.");
                        }
                        else
                        {
                            if (!npc.ChanceToPick.Between(1, 100))
                                errorMessages.Add($"{npc.ClassId}'s ChanceToPick must be an integer number between 1 and 100.");
                            if (npc.MinLevel <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Minimum Level must be an integer number higher than 0.");
                            if (npc.MaxLevel <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Maximum Level must be an integer number higher than 0.");
                            if (npc.MaxLevel < npc.MinLevel)
                                errorMessages.Add($"{npc.ClassId}'s Maximum Level cannot be lower than its Minimum Level.");
                            if (npc.OverallMaxForKindInFloor <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Overall Limit must be an integer number higher than 0.");
                            if (npc.SimultaneousMaxForKindInFloor <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Simultaneous Limit must be an integer number higher than 0.");
                            if (npc.OverallMaxForKindInFloor < npc.SimultaneousMaxForKindInFloor)
                                errorMessages.Add($"{npc.ClassId}'s Overall Limit cannot be lower than its Simultaneous Limit.");
                            if (!npc.CanSpawnOnFirstTurn && !npc.CanSpawnAfterFirstTurn)
                                errorMessages.Add($"{npc.ClassId}'s Can Spawn On First Turn and Can Spawn After First Turn are both disabled.");
                        }
                    }

                    var totalChanceToPick = npcList.Sum(npc => npc.ChanceToPick);
                    if (totalChanceToPick != 100)
                    {
                        errorMessages.Add("Total Chances to Pick don't add up to 100");
                    }
                }

                return !errorMessages.Any();
            }
            catch (Exception ex)
            {
                errorMessages.Add($"There has been an exception when parsing the table: {ex.Message}");
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAndPrepareListForSave(out List<ClassInFloorInfo> npcList, out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"The current NPC Generation data could not be saved due to the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Invalid NPC generation data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                if(NPCGenerationParams.NPCList != null)
                    NPCGenerationParams.NPCList.Clear();
                else
                    NPCGenerationParams.NPCList = new List<ClassInFloorInfo>();
                NPCGenerationParams.NPCList.AddRange(npcList);
                NPCGenerationParams.MinNPCSpawnsAtStart = (int)nudMinNPCSpawnsAtStart.Value;
                NPCGenerationParams.SimultaneousMaxNPCs = (int)nudSimultaneousMaxNPCs.Value;
                NPCGenerationParams.TurnsPerNPCGeneration = (int)nudTurnsPerNPCGeneration.Value;
                Saved = true;
                this.Close();
            }
        }

        private void nudSimultaneousMaxNPCs_ValueChanged(object sender, EventArgs e)
        {
            fklblNoNPCSpawnsWarning.Visible = dgvNPCTable.Rows.Count > 0 && nudSimultaneousMaxNPCs.Value == 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvNPCTable_Leave(object sender, EventArgs e)
        {
            dgvNPCTable.EndEdit();
        }
    }

    public class NPCGenerationParams
    {
        public List<ClassInFloorInfo> NPCList { get; set; }
        public int MinNPCSpawnsAtStart { get; set; }
        public int SimultaneousMaxNPCs { get; set; }
        public int TurnsPerNPCGeneration { get; set; }
    }
}

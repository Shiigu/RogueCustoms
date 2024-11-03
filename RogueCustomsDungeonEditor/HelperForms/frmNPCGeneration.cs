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
#pragma warning disable S101 // Types should be named in PascalCase
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class frmNPCGeneration : Form
    {
        private FloorInfo ActiveFloorGroup;
        public NPCGenerationParams NPCGenerationParams { get; private set; }
        public bool Saved { get; private set; }
        private List<string> ValidNPCClasses;

        public frmNPCGeneration(FloorInfo floorGroupToUse, int minFloorLevel, int maxFloorLevel, NPCGenerationParams npcGenerationParams, DungeonInfo activeDungeon)
        {
            InitializeComponent();
            ActiveFloorGroup = floorGroupToUse;
            NPCGenerationParams = new NPCGenerationParams
            {
                MinNPCSpawnsAtStart = npcGenerationParams.MinNPCSpawnsAtStart,
                SimultaneousMaxNPCs = npcGenerationParams.SimultaneousMaxNPCs,
                TurnsPerNPCGeneration = npcGenerationParams.TurnsPerNPCGeneration,
                NPCList = new()
            };
            foreach (var npc in npcGenerationParams.NPCList)
            {
                NPCGenerationParams.NPCList.Add(new ClassInFloorInfo
                {
                    ClassId = npc.ClassId,
                    MinLevel = npc.MinLevel,
                    MaxLevel = npc.MaxLevel,
                    MinimumInFirstTurn = npc.MinimumInFirstTurn,
                    SimultaneousMaxForKindInFloor = npc.SimultaneousMaxForKindInFloor,
                    OverallMaxForKindInFloor = npc.OverallMaxForKindInFloor,
                    ChanceToPick = npc.ChanceToPick,
                    CanSpawnOnFirstTurn = npc.CanSpawnOnFirstTurn,
                    CanSpawnAfterFirstTurn = npc.CanSpawnAfterFirstTurn,
                    SpawnCondition = npc.SpawnCondition,
                });
            }
            ValidNPCClasses = activeDungeon.NPCs.ConvertAll(npc => npc.Id);
            var classIdColumn = (DataGridViewComboBoxColumn)dgvNPCTable.Columns["ClassId"];
            classIdColumn.DataSource = ValidNPCClasses;
            var npcTable = new BindingList<ClassInFloorInfo>()
            {
                AllowNew = true,
                AllowRemove = true
            };
            foreach (var npc in NPCGenerationParams.NPCList)
            {
                npcTable.Add(npc);
            }
            dgvNPCTable.DataSource = npcTable;
            nudMinNPCSpawnsAtStart.Value = npcGenerationParams.MinNPCSpawnsAtStart;
            nudSimultaneousMaxNPCs.Value = npcGenerationParams.SimultaneousMaxNPCs;
            nudTurnsPerNPCGeneration.Value = npcGenerationParams.TurnsPerNPCGeneration;
            if (ActiveFloorGroup.MinFloorLevel != ActiveFloorGroup.MaxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";
        }

        private bool ValidateAndPrepareList(out List<ClassInFloorInfo> npcList, out List<string> errorMessages)
        {
            npcList = new List<ClassInFloorInfo>();
            errorMessages = new List<string>();
            try
            {
                foreach (DataGridViewRow row in dgvNPCTable.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var npcRow = new ClassInFloorInfo
                        {
                            ClassId = row.Cells["ClassId"].Value?.ToString(),
                            MinLevel = int.Parse(row.Cells["MinLevel"].Value?.ToString()),
                            MaxLevel = int.Parse(row.Cells["MaxLevel"].Value?.ToString()),
                            MinimumInFirstTurn = int.Parse(row.Cells["MinimumInFirstTurn"].Value?.ToString()),
                            SimultaneousMaxForKindInFloor = int.Parse(row.Cells["SimultaneousMaxForKindInFloor"].Value?.ToString()),
                            OverallMaxForKindInFloor = int.Parse(row.Cells["OverallMaxForKindInFloor"].Value?.ToString()),
                            ChanceToPick = int.Parse(row.Cells["ChanceToPick"].Value?.ToString()),
                            CanSpawnOnFirstTurn = bool.Parse(row.Cells["CanSpawnOnFirstTurn"].Value?.ToString()),
                            CanSpawnAfterFirstTurn = bool.Parse(row.Cells["CanSpawnAfterFirstTurn"].Value?.ToString()),
                            SpawnCondition = row.Cells["SpawnCondition"].Value?.ToString()
                        };
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
                        npc.CanSpawnAfterFirstTurn,
                        npc.SpawnCondition
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
                            if (npc.ChanceToPick <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Chance to Pick must be an integer number higher than 0.");
                            if (npc.MinLevel <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Minimum Level must be an integer number higher than 0.");
                            if (npc.MaxLevel <= 0)
                                errorMessages.Add($"{npc.ClassId}'s Maximum Level must be an integer number higher than 0.");
                            if (npc.MinimumInFirstTurn < 0)
                                errorMessages.Add($"{npc.ClassId}'s Minimum Spawns in the first turn must be a non-negative integer number.");
                            if (!npc.CanSpawnOnFirstTurn && npc.MinimumInFirstTurn > 0)
                                errorMessages.Add($"{npc.ClassId}'s Minimum Spawns in the first turn are higher than 0, but it's set to not spawn on the first turn.");
                            if (npc.MinimumInFirstTurn > npc.SimultaneousMaxForKindInFloor)
                                errorMessages.Add($"{npc.ClassId}'s Minimum Spawns in the first turn are higher than the maximum amount allowed.");
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
                            if (!string.IsNullOrWhiteSpace(npc.SpawnCondition) && !npc.SpawnCondition.IsBooleanExpression())
                                errorMessages.Add($"{npc.ClassId}'s Spawn Condition is not a valid boolean expression.");
                        }
                    }

                    var totalMinimumGuaranteedSpawns = npcList.Sum(npc => npc.MinimumInFirstTurn);

                    if(totalMinimumGuaranteedSpawns > (int)nudSimultaneousMaxNPCs.Value)
                        errorMessages.Add("There are more NPCs guaranteed to spawn in the first turn than the maximum simultaneous amount allowed.");
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
            if (!ValidateAndPrepareList(out List<ClassInFloorInfo> npcList, out List<string> errorMessages))
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
                if (NPCGenerationParams.NPCList != null)
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

        private void btnCheckGenerationOdds_Click(object sender, EventArgs e)
        {
            if (!ValidateAndPrepareList(out List<ClassInFloorInfo> npcList, out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"It's not possible to give NPC generation odds due to the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Invalid NPC generation data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                var oddsMessage = new StringBuilder();

                var npcsThatSpawnOnTheFirstTurn = npcList.Where(npc => npc.CanSpawnOnFirstTurn);
                var totalWeightFirstTurn = (double) npcsThatSpawnOnTheFirstTurn.Sum(npc => npc.ChanceToPick);

                var npcsThatSpawnAfterTheFirstTurn = npcList.Where(npc => npc.CanSpawnAfterFirstTurn);
                var totalWeightAfterFirstTurn = (double) npcsThatSpawnAfterTheFirstTurn.Sum(npc => npc.ChanceToPick);

                if (npcsThatSpawnOnTheFirstTurn.Any())
                {
                    oddsMessage.AppendLine("On the first turn, the odds for NPC spawns are the following*:");

                    foreach (var possibleNPC in npcsThatSpawnOnTheFirstTurn)
                    {
                        var odds = possibleNPC.ChanceToPick / totalWeightFirstTurn * 100;
                        if (possibleNPC.MinimumInFirstTurn == 0)
                            oddsMessage.AppendLine($"     - {possibleNPC.ClassId}: {odds:0.####}%");
                        else
                            oddsMessage.AppendLine($"     - {possibleNPC.ClassId}: {odds:0.####}% (Guaranteed {possibleNPC.MinimumInFirstTurn})");
                    }
                }
                else
                {
                    oddsMessage.AppendLine("No NPCs are set to spawn on the first turn.");
                }                

                if (npcsThatSpawnAfterTheFirstTurn.Any())
                {
                    oddsMessage.AppendLine("\nAfter the first turn, the odds for NPC spawns are the following*:");

                    foreach (var possibleNPC in npcsThatSpawnAfterTheFirstTurn)
                    {
                        var odds = possibleNPC.ChanceToPick / totalWeightAfterFirstTurn * 100;
                        oddsMessage.AppendLine($"     - {possibleNPC.ClassId}: {odds:0.####}%");
                    }
                }
                else
                {
                    oddsMessage.AppendLine("\nNo NPCs are set to spawn after the first turn.");
                }

                if (npcsThatSpawnOnTheFirstTurn.Any() || npcsThatSpawnAfterTheFirstTurn.Any())
                    oddsMessage.AppendLine("\n(Assuming the simultaneous or maximum allowed for any NPC hasn't been reached, and that the Spawn Condition is empty or fulfilled)");

                MessageBox.Show(oddsMessage.ToString(), "NPC Generation for Floor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    public class NPCGenerationParams
    {
        public List<ClassInFloorInfo> NPCList { get; set; }
        public int MinNPCSpawnsAtStart { get; set; }
        public int SimultaneousMaxNPCs { get; set; }
        public int TurnsPerNPCGeneration { get; set; }
    }
    #pragma warning restore S101 // Types should be named in PascalCase
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

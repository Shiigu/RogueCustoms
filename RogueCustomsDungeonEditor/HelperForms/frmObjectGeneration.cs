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
    #pragma warning disable IDE1006 // Estilos de nombres
    #pragma warning disable S2589 // Boolean expressions should not be gratuitous
    #pragma warning disable CS8601 // Posible asignación de referencia nula
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class frmObjectGeneration : Form
    {
        private readonly FloorInfo ActiveFloorGroup;
        private readonly EntityTypeForForm TypeToUse;
        public ObjectGenerationParams ObjectGenerationParams { get; private set; }
        public bool Saved { get; private set; }
        private readonly List<string> ValidObjectClasses;

        public frmObjectGeneration(FloorInfo floorGroupToUse, int minFloorLevel, int maxFloorLevel, ObjectGenerationParams objectGenerationParams, EntityTypeForForm typeToUse, DungeonInfo activeDungeon)
        {
            InitializeComponent();
            ActiveFloorGroup = floorGroupToUse;
            ObjectGenerationParams = new ObjectGenerationParams
            {
                MinInFloor = objectGenerationParams.MinInFloor,
                MaxInFloor = objectGenerationParams.MaxInFloor,
                ObjectList = new()
            };
            foreach (var @object in objectGenerationParams.ObjectList)
            {
                ObjectGenerationParams.ObjectList.Add(new ClassInFloorInfo
                {
                    ClassId = @object.ClassId,
                    SimultaneousMaxForKindInFloor = @object.SimultaneousMaxForKindInFloor,
                    ChanceToPick = @object.ChanceToPick,
                });
            }
            TypeToUse = typeToUse;
            string objectText;
            if (typeToUse == EntityTypeForForm.Item)
            {
                objectText = "Item";
            }
            else if (typeToUse == EntityTypeForForm.Trap)
            {
                objectText = "Trap";
            }
            else
            {
                throw new ArgumentException($"A Type of {typeToUse} is not valid for an Object Generation form");
            }

            if (minFloorLevel != maxFloorLevel)
                this.Text = $"{objectText} Generation for Floor Levels {ActiveFloorGroup.MinFloorLevel} to {ActiveFloorGroup.MaxFloorLevel}:";
            else
                this.Text = $"{objectText} Generation for Floor Level {ActiveFloorGroup.MinFloorLevel}:";
            btnSave.Text = $"Save {objectText} Generation Data";
            dgvObjectTable.Columns[0].HeaderText = $"{objectText} Class";
            if (typeToUse == EntityTypeForForm.Item)
            {
                ValidObjectClasses = activeDungeon.Items.ConvertAll(item => item.Id);
            }
            else if (typeToUse == EntityTypeForForm.Trap)
            {
                ValidObjectClasses = activeDungeon.Traps.ConvertAll(trap => trap.Id);
            }
            var classIdColumn = (DataGridViewComboBoxColumn)dgvObjectTable.Columns["ClassId"];
            classIdColumn.DataSource = ValidObjectClasses;
            var objectTable = new BindingList<ClassInFloorInfo>()
            {
                AllowNew = true,
                AllowRemove = true
            };
            foreach (var @object in objectGenerationParams.ObjectList)
            {
                objectTable.Add(@object);
            }
            dgvObjectTable.DataSource = objectTable;
            dgvObjectTable.Columns["MinLevel"].Visible = false;
            dgvObjectTable.Columns["MaxLevel"].Visible = false;
            dgvObjectTable.Columns["CanSpawnOnFirstTurn"].Visible = false;
            dgvObjectTable.Columns["CanSpawnAfterFirstTurn"].Visible = false;
            dgvObjectTable.Columns["OverallMaxForKindInFloor"].Visible = false;
            nudMinInFloor.Value = ObjectGenerationParams.MinInFloor;
            nudMaxInFloor.Value = ObjectGenerationParams.MaxInFloor;
            if (minFloorLevel != maxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";
        }

        private bool ValidateAndPrepareListForSave(out List<ClassInFloorInfo> objectList, out List<string> errorMessages)
        {
            objectList = new List<ClassInFloorInfo>();
            errorMessages = new List<string>();
            try
            {
                foreach (DataGridViewRow row in dgvObjectTable.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var objectRow = new ClassInFloorInfo
                        {
                            ClassId = row.Cells["ClassId"].Value?.ToString(),
                            SimultaneousMaxForKindInFloor = int.Parse(row.Cells["SimultaneousMaxForKindInFloor"].Value?.ToString()),
                            ChanceToPick = int.Parse(row.Cells["ChanceToPick"].Value?.ToString())
                        };
                        objectList.Add(objectRow);
                    }
                }

                if (objectList.Count > 0)
                {
                    var nameToDisplay = (TypeToUse == EntityTypeForForm.Item) ? "Item" : "Trap";
                    foreach (var @object in objectList)
                    {
                        if (objectList.Count(o => o.ClassId.Equals(@object.ClassId)) > 1)
                            errorMessages.Add($"There is more than one entry for {@object.ClassId}. Remove one of them.");

                        if (string.IsNullOrWhiteSpace(@object.ClassId))
                        {
                            errorMessages.Add($"At least one row is lacking a valid {nameToDisplay} Class.");
                        }
                        else if (!ValidObjectClasses.Contains(@object.ClassId))
                        {
                            errorMessages.Add($"{@object.ClassId} is not a recognized {nameToDisplay} Class.");
                        }
                        else
                        {
                            if (!@object.ChanceToPick.Between(1, 100))
                                errorMessages.Add($"{@object.ClassId}'s Chance To Pick must be an integer number between 1 and 100.");
                            if (@object.SimultaneousMaxForKindInFloor <= 0)
                                errorMessages.Add($"{@object.ClassId}'s Maximum must be an integer number higher than 0.");
                        }
                    }

                    var totalChanceToPick = objectList.Sum(npc => npc.ChanceToPick);
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
            var nameToDisplay = (TypeToUse == EntityTypeForForm.Item) ? "Item" : "Trap";
            if (!ValidateAndPrepareListForSave(out List<ClassInFloorInfo> objectList, out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"The current {nameToDisplay} Generation data could not be saved due to the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    $"Invalid {nameToDisplay} generation data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                if (ObjectGenerationParams.ObjectList != null)
                    ObjectGenerationParams.ObjectList.Clear();
                else
                    ObjectGenerationParams.ObjectList = new List<ClassInFloorInfo>();
                ObjectGenerationParams.ObjectList.AddRange(objectList);
                ObjectGenerationParams.MinInFloor = (int)nudMinInFloor.Value;
                ObjectGenerationParams.MaxInFloor = (int)nudMaxInFloor.Value;
                Saved = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvObjectTable_Leave(object sender, EventArgs e)
        {
            dgvObjectTable.EndEdit();
        }
    }

    public enum EntityTypeForForm
    {
        Item,
        Trap
    }

    public class ObjectGenerationParams
    {
        public List<ClassInFloorInfo> ObjectList { get; set; }
        public int MinInFloor { get; set; }
        public int MaxInFloor { get; set; }
    }
    #pragma warning restore IDE1006 // Estilos de nombres
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

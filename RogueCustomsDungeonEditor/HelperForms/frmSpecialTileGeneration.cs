using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
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
    public partial class frmSpecialTileGeneration : Form
    {
        private readonly FloorInfo ActiveFloorGroup;
        public List<SpecialTileInFloorInfo> SpecialTileGenerationInfoList { get; private set; }
        public bool Saved { get; private set; }
        private readonly List<string> ValidSpecialTileTypes;

        public frmSpecialTileGeneration(FloorInfo floorGroupToUse, int minFloorLevel, int maxFloorLevel, List<SpecialTileInFloorInfo> specialTileGenerationInfo, DungeonInfo activeDungeon)
        {
            InitializeComponent();
            ActiveFloorGroup = floorGroupToUse;
            SpecialTileGenerationInfoList = new(specialTileGenerationInfo);

            if (minFloorLevel != maxFloorLevel)
                this.Text = $"Special Tile Generation for Floor Levels {ActiveFloorGroup.MinFloorLevel} to {ActiveFloorGroup.MaxFloorLevel}:";
            else
                this.Text = $"Special Tile Generation for Floor Level {ActiveFloorGroup.MinFloorLevel}:";
            ValidSpecialTileTypes = activeDungeon.GetSpecialTileTypes(FormConstants.DefaultTileTypes).Select(stt => stt.Id).ToList();
            var tileTypeIdColumn = (DataGridViewComboBoxColumn)dgvObjectTable.Columns["TileTypeId"];
            tileTypeIdColumn.DataSource = ValidSpecialTileTypes;
            var generationTypeColumn = (DataGridViewComboBoxColumn)dgvObjectTable.Columns["GeneratorType"];
            generationTypeColumn.DataSource = Enum.GetNames(typeof(SpecialTileGenerationAlgorithm)).ToList();
            var specialTileGenerationParamsList = new BindingList<SpecialTileInFloorParams>()
            {
                AllowNew = true,
                AllowRemove = true
            };
            foreach (var specialTileParams in SpecialTileGenerationInfoList)
            {
                specialTileGenerationParamsList.Add(new()
                {
                    TileTypeId = specialTileParams.TileTypeId,
                    GeneratorType = specialTileParams.GeneratorType.ToString(),
                    MinSpecialTileGenerations = specialTileParams.MinSpecialTileGenerations,
                    MaxSpecialTileGenerations = specialTileParams.MaxSpecialTileGenerations
                });
            }
            dgvObjectTable.DataSource = specialTileGenerationParamsList;
            if (minFloorLevel != maxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";
        }

        private List<string> ValidateForSave(out List<SpecialTileInFloorInfo> specialTileList)
        {
            specialTileList = new();
            foreach (DataGridViewRow row in dgvObjectTable.Rows)
            {
                if (!row.IsNewRow)
                {
                    var specialTileRow = new SpecialTileInFloorInfo
                    {
                        TileTypeId = row.Cells["TileTypeId"].Value?.ToString(),
                        MinSpecialTileGenerations = int.Parse(row.Cells["MinSpecialTileGenerations"].Value?.ToString()),
                        MaxSpecialTileGenerations = int.Parse(row.Cells["MaxSpecialTileGenerations"].Value?.ToString()),
                        GeneratorType = Enum.Parse<SpecialTileGenerationAlgorithm>(row.Cells["GeneratorType"].Value?.ToString()),
                    };
                    specialTileList.Add(specialTileRow);
                }
            }
            var errorMessages = new List<string>();

            foreach (var specialTileGenerator in specialTileList)
            {
                if(string.IsNullOrWhiteSpace(specialTileGenerator.TileTypeId))
                    errorMessages.Add($"At least one Generator lacks a Tile Type");
                if (specialTileGenerator.MinSpecialTileGenerations < 0)
                    errorMessages.Add($"A Generator of {specialTileGenerator.TileTypeId} as {specialTileGenerator.GeneratorType} has a Minimum lower than 0.");
                if (specialTileGenerator.MaxSpecialTileGenerations < 0)
                    errorMessages.Add($"A Generator of {specialTileGenerator.TileTypeId} as {specialTileGenerator.GeneratorType} has a Maximum lower than 0.");
                if (specialTileGenerator.MinSpecialTileGenerations > specialTileGenerator.MaxSpecialTileGenerations)
                    errorMessages.Add($"A Generator of {specialTileGenerator.TileTypeId} as {specialTileGenerator.GeneratorType} has a Minimum higher than its Maximum.");
                if (specialTileGenerator.GeneratorType == null)
                    errorMessages.Add($"At least one Generator lacks a Generator Type");
                if (specialTileList.Any(stl => stl != specialTileGenerator && stl.TileTypeId.Equals(specialTileGenerator.TileTypeId) && stl.GeneratorType == specialTileGenerator.GeneratorType))
                    errorMessages.Add($"A Generator of {specialTileGenerator.TileTypeId} as {specialTileGenerator.GeneratorType} is present more than once");
            }

            return errorMessages;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var validationErrors = ValidateForSave(out List<SpecialTileInFloorInfo> specialTileList);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Special Tile Generation data could not be saved due to the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    $"Invalid Special Tile generation data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                Saved = true;
                SpecialTileGenerationInfoList = specialTileList;
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
    public class SpecialTileInFloorParams
    {
        public string TileTypeId { get; set; }
        public string GeneratorType { get; set; }
        public int MinSpecialTileGenerations { get; set; }
        public int MaxSpecialTileGenerations { get; set; }
    }

#pragma warning restore IDE1006 // Estilos de nombres
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

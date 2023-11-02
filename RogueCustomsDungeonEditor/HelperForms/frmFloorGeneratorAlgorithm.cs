using org.matheval.Node;
using RogueCustomsDungeonEditor.FloorInfos;
using RogueCustomsGameEngine.Utils.Enums;
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
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class frmFloorGeneratorAlgorithm : Form
    {
        public GeneratorAlgorithmInfo AlgorithmToSave { get; private set; }
        public bool Saved { get; private set; }
        private FloorInfo CurrentFloorGroup;
        private List<FloorTypeData> FloorTypeData;
        private int MaxColumns, MaxRows;
        private string CurrentAlgorithmName => lvFloorAlgorithms.SelectedItems.Count > 0 ? FloorTypeData[lvFloorAlgorithms.SelectedIndices[0]].InternalName : "";

        public frmFloorGeneratorAlgorithm(FloorInfo floorGroupToUse, int width, int height, int minFloorLevel, int maxFloorLevel, GeneratorAlgorithmInfo algorithmToSave, List<FloorTypeData> floorTypeData)
        {
            InitializeComponent();
            CurrentFloorGroup = floorGroupToUse;
            var algorithmIcons = new ImageList
            {
                ImageSize = new Size(64, 64),
                ColorDepth = ColorDepth.Depth32Bit
            };
            FloorTypeData = floorTypeData;
            lvFloorAlgorithms.Items.Clear();
            foreach (var algorithm in floorTypeData)
            {
                algorithmIcons.Images.Add(algorithm.InternalName, algorithm.PreviewImage);
                var algorithmItem = new ListViewItem($"{algorithm.DisplayName}", algorithm.InternalName)
                {
                    ToolTipText = algorithm.Description
                };
                lvFloorAlgorithms.Items.Add(algorithmItem);
            }
            lvFloorAlgorithms.LargeImageList = algorithmIcons;
            if (minFloorLevel != maxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";
            AlgorithmToSave = algorithmToSave;
            MaxColumns = width / 5;
            MaxRows = height / 5;
            if (AlgorithmToSave?.Rows > MaxRows)
                nudAlgorithmRows.Value = MaxRows;
            else
                nudAlgorithmRows.Value = AlgorithmToSave != null ? AlgorithmToSave.Rows : 1;
            nudAlgorithmRows.Maximum = MaxRows;
            if (AlgorithmToSave?.Columns > MaxColumns)
                nudAlgorithmColumns.Value = MaxColumns;
            else
                nudAlgorithmColumns.Value = AlgorithmToSave != null ? AlgorithmToSave.Columns : 1;
            nudAlgorithmColumns.Maximum = MaxColumns;
            if (AlgorithmToSave != null)
            {
                var algorithmIndex = floorTypeData.IndexOf(floorTypeData.Find(ftd => ftd.InternalName == AlgorithmToSave.Name));
                lvFloorAlgorithms.Items[algorithmIndex].Selected = true;
                lvFloorAlgorithms.Select();
            }
        }

        private void lvFloorAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = lvFloorAlgorithms.SelectedItems.Count > 0;
            if (lvFloorAlgorithms.SelectedItems.Count > 0 && lvFloorAlgorithms.SelectedItems[0].Text == "Single Room")
            {
                nudAlgorithmColumns.Value = 1;
                nudAlgorithmColumns.Enabled = false;
                nudAlgorithmRows.Value = 1;
                nudAlgorithmRows.Enabled = false;
            }
            else if (lvFloorAlgorithms.SelectedItems.Count > 0 && lvFloorAlgorithms.SelectedItems[0].Text != "Single Room")
            {
                nudAlgorithmColumns.Value = Math.Max(nudAlgorithmColumns.Value, nudAlgorithmColumns.Minimum);
                nudAlgorithmColumns.Enabled = true;
                nudAlgorithmRows.Value = Math.Max(nudAlgorithmRows.Value, nudAlgorithmRows.Minimum);
                nudAlgorithmRows.Enabled = true;
            }
            else
            {
                nudAlgorithmColumns.Enabled = false;
                nudAlgorithmRows.Enabled = false;
            }
            fklblRedundantAlgorithm.Visible = CurrentFloorGroup.PossibleGeneratorAlgorithms.Exists(pga => pga != AlgorithmToSave && pga.Name.Equals(CurrentAlgorithmName) && pga.Rows == (int)nudAlgorithmRows.Value && pga.Columns == (int)nudAlgorithmColumns.Value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(CurrentAlgorithmName != "OneBigRoom" && nudAlgorithmColumns.Value == 1 && nudAlgorithmRows.Value == 1)
            {
                var messageBoxResult = MessageBox.Show(
                    $"You have selected an Algorithm that isn't One Big Room, yet you have set it to have only one room. This is valid, but not recommended.\n\nDo you want to proceed?",
                    "Save Floor Algorithm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.No)
                    return;
            }
            if (AlgorithmToSave != null)
            {
                AlgorithmToSave.Name = CurrentAlgorithmName;
                AlgorithmToSave.Columns = (int)nudAlgorithmColumns.Value;
                AlgorithmToSave.Rows = (int)nudAlgorithmRows.Value;
            }
            else
            {
                AlgorithmToSave = new GeneratorAlgorithmInfo
                {
                    Name = CurrentAlgorithmName,
                    Columns = (int)nudAlgorithmColumns.Value,
                    Rows = (int)nudAlgorithmRows.Value
                };
            }
            Saved = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nudAlgorithmColumns_ValueChanged(object sender, EventArgs e)
        {
            fklblRedundantAlgorithm.Visible = CurrentFloorGroup.PossibleGeneratorAlgorithms.Exists(pga => pga != AlgorithmToSave && pga.Name.Equals(CurrentAlgorithmName) && pga.Rows == (int)nudAlgorithmRows.Value && pga.Columns == (int)nudAlgorithmColumns.Value);
        }

        private void nudAlgorithmRows_ValueChanged(object sender, EventArgs e)
        {
            fklblRedundantAlgorithm.Visible = CurrentFloorGroup.PossibleGeneratorAlgorithms.Exists(pga => pga != AlgorithmToSave && pga.Name.Equals(CurrentAlgorithmName) && pga.Rows == (int)nudAlgorithmRows.Value && pga.Columns == (int)nudAlgorithmColumns.Value);
        }
    }
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

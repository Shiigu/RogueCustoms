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
    public partial class frmFloorGeneratorAlgorithm : Form
    {
        public GeneratorAlgorithmInfo AlgorithmToSave { get; private set; }
        public bool Saved { get; private set; }
        private FloorInfo CurrentFloorGroup;
        private List<FloorTypeData> FloorTypeData;
        private int MaxColumns, MaxRows;
        private string CurrentAlgorithmName => lvFloorAlgorithms.SelectedItems.Count > 0 ? FloorTypeData[lvFloorAlgorithms.SelectedIndices[0]].InternalName : "";

        public frmFloorGeneratorAlgorithm(FloorInfo floorGroupToUse, GeneratorAlgorithmInfo algorithmToSave, List<FloorTypeData> floorTypeData)
        {
            InitializeComponent();
            CurrentFloorGroup = floorGroupToUse;
            var algorithmIcons = new ImageList();
            algorithmIcons.ImageSize = new Size(64, 64);
            algorithmIcons.ColorDepth = ColorDepth.Depth32Bit;
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
            if (floorGroupToUse.MinFloorLevel != floorGroupToUse.MaxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {floorGroupToUse.MinFloorLevel} to {floorGroupToUse.MaxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {floorGroupToUse.MinFloorLevel}:";
            AlgorithmToSave = algorithmToSave;
            MaxColumns = floorGroupToUse.Width / 5;
            MaxRows = floorGroupToUse.Height / 5;
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
                nudAlgorithmColumns.Minimum = 1;
                nudAlgorithmColumns.Value = 1;
                nudAlgorithmColumns.Enabled = false;
                nudAlgorithmRows.Minimum = 1;
                nudAlgorithmRows.Value = 1;
                nudAlgorithmRows.Enabled = false;
            }
            else if (lvFloorAlgorithms.SelectedItems.Count > 0 && lvFloorAlgorithms.SelectedItems[0].Text != "Single Room")
            {
                nudAlgorithmColumns.Minimum = 2;
                nudAlgorithmColumns.Value = Math.Max(nudAlgorithmColumns.Value, nudAlgorithmColumns.Minimum);
                nudAlgorithmColumns.Enabled = true;
                nudAlgorithmRows.Minimum = 2;
                nudAlgorithmRows.Value = Math.Max(nudAlgorithmRows.Value, nudAlgorithmRows.Minimum);
                nudAlgorithmRows.Enabled = true;
            }
            else
            {
                nudAlgorithmColumns.Enabled = false;
                nudAlgorithmRows.Enabled = false;
            }
            fklblRedundantAlgorithm.Visible = CurrentFloorGroup.PossibleGeneratorAlgorithms.Any(pga => pga != AlgorithmToSave && pga.Name.Equals(CurrentAlgorithmName) && pga.Rows == (int)nudAlgorithmRows.Value && pga.Columns == (int)nudAlgorithmColumns.Value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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
            fklblRedundantAlgorithm.Visible = CurrentFloorGroup.PossibleGeneratorAlgorithms.Any(pga => pga != AlgorithmToSave && pga.Name.Equals(CurrentAlgorithmName) && pga.Rows == (int)nudAlgorithmRows.Value && pga.Columns == (int)nudAlgorithmColumns.Value);
        }

        private void nudAlgorithmRows_ValueChanged(object sender, EventArgs e)
        {
            fklblRedundantAlgorithm.Visible = CurrentFloorGroup.PossibleGeneratorAlgorithms.Any(pga => pga != AlgorithmToSave && pga.Name.Equals(CurrentAlgorithmName) && pga.Rows == (int)nudAlgorithmRows.Value && pga.Columns == (int)nudAlgorithmColumns.Value);
        }
    }
}

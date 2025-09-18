using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
namespace RogueCustomsDungeonEditor.Controls
{
    public partial class QualityLevelSheet : UserControl
    {
        private string PreviousCellValue;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<QualityLevelOddsInfo> QualityLevels
        {
            get
            {
                var qualityLevels = new List<QualityLevelOddsInfo>();

                for (int i = 0; i < dgvQualityLevels.Rows.Count; i++)
                {
                    var qualityLevelId = dgvQualityLevels.Rows[i].Cells[0].Value.ToString();
                    var valueCell = dgvQualityLevels.Rows[i].Cells[1].Value.ToString();

                    var parsedValueCell = !string.IsNullOrWhiteSpace(valueCell) ? int.Parse(valueCell, NumberStyles.Integer, CultureInfo.InvariantCulture) : 0;

                    qualityLevels.Add(new()
                    {
                        Id = qualityLevelId,
                        ChanceToPick = parsedValueCell
                    });
                }
                return qualityLevels;
            }
            set
            {
                dgvQualityLevels.Rows.Clear();
                if (value == null || value.Count == 0) return;
                foreach (var level in value)
                {
                    dgvQualityLevels.Rows.Add(level.Id, level.ChanceToPick.ToString("0", CultureInfo.InvariantCulture));
                }
            }
        }

        public QualityLevelSheet()
        {
            InitializeComponent();
        }

        public void EndEdit()
        {
            dgvQualityLevels.EndEdit();
        }


        private void dgvQualityLevels_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var cellValue = dgvQualityLevels[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;
            PreviousCellValue = cellValue;
        }

        private void dgvQualityLevels_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            var cellValue = dgvQualityLevels[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;

            if (!int.TryParse(cellValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int _))
            {
                dgvQualityLevels[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
            }
        }

        private void dgvQualityLevels_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            var input = e.FormattedValue?.ToString() ?? string.Empty;

            if (!int.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out int result))
            {
                MessageBox.Show("Only numeric values are allowed.", "Item QualityLevels", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void btnCalculateOdds_Click(object sender, EventArgs e)
        {
            var oddsMessage = new StringBuilder();
            var currentQualityLevels = QualityLevels;

            if (currentQualityLevels.Count > 0)
            {
                var totalWeight = (double)currentQualityLevels.Sum(o => o.ChanceToPick);
                oddsMessage.AppendLine($"The odds for each quality level are the following:");

                foreach (var qualityLevel in currentQualityLevels)
                {
                    var odds = qualityLevel.ChanceToPick / totalWeight * 100;
                    oddsMessage.AppendLine($"     - {qualityLevel.Id}: {odds:0.####}%");
                }
            }

            MessageBox.Show(oddsMessage.ToString(), $"Quality Level Odds", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma

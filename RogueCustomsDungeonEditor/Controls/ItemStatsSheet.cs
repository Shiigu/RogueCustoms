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
    public partial class ItemStatsSheet : UserControl
    {
        private string PreviousCellValue;

        private List<(string Id, bool IsDecimal, bool IsPercentage)> StatTableData = new();


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TreatStatsAsAbsolute { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<StatInfo> StatData
        {
            set
            {
                StatTableData.Clear();
                foreach (var stat in value)
                {
                    var isDecimal = stat.StatType.Equals("Decimal", StringComparison.InvariantCultureIgnoreCase) || stat.StatType.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase);
                    var isPercentage = stat.StatType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase);
                    StatTableData.Add((stat.Id, isDecimal, isPercentage));
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<PassiveStatModifierInfo> Stats
        {
            get
            {
                var stats = new List<PassiveStatModifierInfo>();

                for (int i = 0; i < dgvStatsModifiers.Rows.Count; i++)
                {
                    var statData = StatTableData.FirstOrDefault(s => s.Id.Equals(dgvStatsModifiers.Rows[i].Cells[0].Value?.ToString() ?? string.Empty));
                    if (statData == default) continue;
                    var valueCell = dgvStatsModifiers.Rows[i].Cells[1].Value.ToString();
                    var parsedValueCell = !string.IsNullOrWhiteSpace(valueCell) ? decimal.Parse(valueCell.Replace("+", "").Replace("%", ""), NumberStyles.Float, CultureInfo.InvariantCulture) : 0;
                    if (parsedValueCell == 0 && !TreatStatsAsAbsolute) continue;
                    stats.Add(new()
                    {
                        Id = statData.Id,
                        Amount = parsedValueCell
                    });
                }
                return stats;
            }
            set
            {
                ResetStatsTable();
                for (int i = 0; i < StatTableData.Count; i++)
                {
                    var firstColumnValue = dgvStatsModifiers[0, i].Value?.ToString() ?? string.Empty;
                    var statData = StatTableData.FirstOrDefault(s => s.Id.Equals(firstColumnValue));
                    if (statData != default)
                    {
                        var correspondingModifier = value.FirstOrDefault(v => v.Id.Equals(statData.Id));
                        if (correspondingModifier == null || correspondingModifier.Amount == 0) continue;
                        if(!TreatStatsAsAbsolute)
                        {
                            if (!statData.IsPercentage)
                            {
                                if (!statData.IsDecimal)
                                {
                                    dgvStatsModifiers[1, i].Value = correspondingModifier.Amount.ToString("+0;-0", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    dgvStatsModifiers[1, i].Value = correspondingModifier.Amount.ToString("+0.00###;-0.00###", CultureInfo.InvariantCulture);
                                }
                            }
                            else
                            {
                                dgvStatsModifiers[1, i].Value = correspondingModifier.Amount.ToString("+0.###;-0.###", CultureInfo.InvariantCulture) + "%";
                            }
                        }
                        else
                        {
                            if (!statData.IsPercentage)
                            {
                                if (!statData.IsDecimal)
                                {
                                    if ((statData.Id.Equals("HP", StringComparison.CurrentCultureIgnoreCase) || statData.Id.Equals("MP", StringComparison.CurrentCultureIgnoreCase) || statData.Id.Equals("Hunger", StringComparison.CurrentCultureIgnoreCase)) && correspondingModifier.Amount < 2)
                                        correspondingModifier.Amount = 2;
                                    dgvStatsModifiers[1, i].Value = correspondingModifier.Amount.ToString("0", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    dgvStatsModifiers[1, i].Value = correspondingModifier.Amount.ToString("0.00###", CultureInfo.InvariantCulture);
                                }
                            }
                            else
                            {
                                dgvStatsModifiers[1, i].Value = correspondingModifier.Amount.ToString("0.###", CultureInfo.InvariantCulture) + "%";
                            }
                        }
                    }
                }
            }
        }

        public ItemStatsSheet()
        {
            InitializeComponent();
        }
        public void EndEdit()
        {
            dgvStatsModifiers.EndEdit();
        }

        private void ResetStatsTable()
        {
            dgvStatsModifiers.Rows.Clear();
            foreach (var stat in StatTableData)
            {
                if (!TreatStatsAsAbsolute)
                {
                    if (stat.IsPercentage)
                        dgvStatsModifiers.Rows.Add(stat.Id, "+0%");
                    else if (stat.IsDecimal)
                        dgvStatsModifiers.Rows.Add(stat.Id, "+0.00");
                    else
                        dgvStatsModifiers.Rows.Add(stat.Id, "+0");
                }
                else
                {
                    if (stat.IsPercentage)
                        dgvStatsModifiers.Rows.Add(stat.Id, "0%");
                    else if (stat.IsDecimal)
                        dgvStatsModifiers.Rows.Add(stat.Id, "0.00");
                    else if (stat.Id.Equals("HP", StringComparison.CurrentCultureIgnoreCase) || stat.Id.Equals("MP", StringComparison.CurrentCultureIgnoreCase) || stat.Id.Equals("Hunger", StringComparison.CurrentCultureIgnoreCase))
                        dgvStatsModifiers.Rows.Add(stat.Id, "2");
                    else
                        dgvStatsModifiers.Rows.Add(stat.Id, "0");
                }
            }
        }

        private void dgvStatsModifiers_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            var cellValue = dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "0";

            PreviousCellValue = cellValue;

            dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = cellValue.Replace("+", "").Replace("%", "");
        }

        private void dgvStatsModifiers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            var cellValue = dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;

            if (decimal.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
            {
                var firstColumnValue = dgvStatsModifiers[0, e.RowIndex].Value?.ToString() ?? string.Empty;
                var statData = StatTableData.FirstOrDefault(s => s.Id.Equals(firstColumnValue));
                if(statData != default)
                {
                    if (TreatStatsAsAbsolute)
                    {
                        if (!statData.IsPercentage)
                        {
                            if (!statData.IsDecimal)
                            {
                                result = (int)result;
                                if ((statData.Id.Equals("HP", StringComparison.CurrentCultureIgnoreCase) || statData.Id.Equals("MP", StringComparison.CurrentCultureIgnoreCase) || statData.Id.Equals("Hunger", StringComparison.CurrentCultureIgnoreCase)) && result < 2)
                                    result = 2;
                                dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = result.ToString("0", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = result.ToString("0.00###", CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            result = (int)result;
                            dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = result.ToString("0.###", CultureInfo.InvariantCulture) + "%";
                        }
                    }
                    else
                    {
                        if (!statData.IsPercentage)
                        {
                            if (!statData.IsDecimal)
                            {
                                result = (int)result;
                                dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = result.ToString("+0;-0", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = result.ToString("+0.00###;-0.00###", CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            result = (int)result;
                            dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = result.ToString("+0.###;-0.###", CultureInfo.InvariantCulture) + "%";
                        }
                    }
                }
            }
            else
            {
                dgvStatsModifiers[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
            }
        }

        private void dgvStatsModifiers_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 1) return;
            var input = e.FormattedValue?.ToString() ?? string.Empty;

            input = input.Replace("+", "").Replace("%", "");

            if (!decimal.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
            {
                MessageBox.Show("Only numeric values are allowed.", "Item Stat Modifiers", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
    }
}
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma

using MathNet.Numerics;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Control = System.Windows.Forms.Control;
using Label = System.Windows.Forms.Label;
using Parameter = RogueCustomsGameEngine.Utils.JsonImports.Parameter;

namespace RogueCustomsDungeonEditor.HelperForms
{
#pragma warning disable IDE1006 // Estilos de nombres
#pragma warning disable RCS1077 // Optimize LINQ method call.
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable S2259 // Null pointers should not be dereferenced
#pragma warning disable S6602 // "Find" method should be used instead of the "FirstOrDefault" extension
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
    public partial class frmActionParameters : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EffectInfo EffectToSave { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Saved { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ValidNPCs { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ValidItems { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ValidTraps { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ValidAlteredStatuses { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EffectTypeData EffectTypeData { get; private set; }

        private readonly DungeonInfo ActiveDungeon;
        private string PreviousTextBoxValue;

        private readonly List<ParameterType> TypesThatAlwaysHoldAValue = new()
        {
            ParameterType.Boolean,
            ParameterType.Odds,
            ParameterType.Number
        };

        public frmActionParameters(EffectInfo effectToSave, DungeonInfo activeDungeon, EffectTypeData paramsData, List<string> validNPCs, List<string> validItems, List<string> validTraps, List<string> validAlteredStatuses, bool isActionEdit)
        {
            InitializeComponent();
            ActiveDungeon = activeDungeon;
            if (paramsData.InternalName.Equals(effectToSave?.EffectName))
            {
                EffectToSave = effectToSave.Clone();
                if (EffectToSave.Params.Length != paramsData.Parameters.Count)
                {
                    var newParamsList = new Parameter[paramsData.Parameters.Count];
                    for (int i = 0; i < EffectToSave.Params.Length; i++)
                    {
                        newParamsList[i] = new Parameter
                        {
                            ParamName = paramsData.Parameters[i].InternalName
                        };
                        var existingParam = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(newParamsList[i].ParamName));
                        newParamsList[i].Value = existingParam != null ? existingParam.Value : paramsData.Parameters[i].Default;
                    }
                }
            }
            else
            {
                EffectToSave = new EffectInfo
                {
                    EffectName = paramsData.InternalName,
                    Params = new Parameter[paramsData.Parameters.Count]
                };
                for (int i = 0; i < EffectToSave.Params.Length; i++)
                {
                    EffectToSave.Params[i] = new Parameter
                    {
                        ParamName = paramsData.Parameters[i].InternalName,
                        Value = paramsData.Parameters[i].Default
                    };
                }
            }
            EffectTypeData = paramsData;
            ValidNPCs = validNPCs;
            ValidItems = validItems;
            ValidTraps = validTraps;
            ValidAlteredStatuses = validAlteredStatuses;

            var baseDisplayWidth = lblDisplayName.Width;
            var baseDescriptionWidth = lblDescription.Width;
            var baseDescriptionHeight = lblDescription.Height;

            lblDisplayName.Text = $"{paramsData.ComboBoxDisplayName}:";
            lblDescription.Text = paramsData.Description;

            if (lblDisplayName.PreferredWidth > baseDisplayWidth)
            {
                var widthChange = baseDisplayWidth - lblDisplayName.PreferredWidth - 5;
                lblDisplayName.Width = lblDisplayName.PreferredWidth;
                lblDescription.Width -= widthChange;
                lblRequired.Width -= widthChange;
                tlpParameters.Width -= widthChange;
                llblWikiAction.Width -= widthChange;
                btnSave.Width -= widthChange / 2;
                btnCancel.Location = new Point(btnCancel.Location.X - (widthChange / 2), btnCancel.Location.Y);
                btnCancel.Width -= widthChange / 2;
                this.Width -= widthChange;
            }

            if (lblDescription.PreferredWidth > baseDescriptionWidth)
            {
                var widthChange = baseDescriptionWidth - lblDescription.PreferredWidth - 5;
                lblDisplayName.Width -= widthChange;
                lblDescription.Width = lblDescription.PreferredWidth;
                lblRequired.Width -= widthChange;
                tlpParameters.Width -= widthChange;
                llblWikiAction.Width -= widthChange;
                btnSave.Width -= widthChange / 2;
                btnCancel.Location = new Point(btnCancel.Location.X - (widthChange / 2), btnCancel.Location.Y);
                btnCancel.Width -= widthChange / 2;
                this.Width -= widthChange;
            }

            if (lblDescription.PreferredHeight > baseDescriptionHeight)
            {
                var heightChange = baseDescriptionHeight - lblDescription.PreferredHeight - 5;
                lblDescription.Height = lblDescription.PreferredHeight;
                lblRequired.Location = new Point(lblRequired.Location.X, lblRequired.Location.Y - heightChange);
                tlpParameters.Location = new Point(tlpParameters.Location.X, tlpParameters.Location.Y - heightChange);
                llblWikiAction.Location = new Point(llblWikiAction.Location.X, llblWikiAction.Location.Y - heightChange);
                btnSave.Location = new Point(btnSave.Location.X, btnSave.Location.Y - heightChange);
                btnCancel.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y - heightChange);
                this.Height -= heightChange;
            }

            var rowNumber = 0;
            llblWikiAction.Focus();

            if (paramsData.Parameters.Count == 0)
                lblRequired.Text = "This Function has no parameters.";

            foreach (var parameter in paramsData.Parameters)
            {
                var nameLabel = new Label
                {
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = false,
                    Height = 30,
                    Text = parameter.Required && !TypesThatAlwaysHoldAValue.Contains(parameter.Type) && !parameter.OptionalIfFieldsHaveValue.Any()
                       ? $"{parameter.DisplayName}*"
                       : $"{parameter.DisplayName}"
                };

                string originalValue = isActionEdit ? effectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(parameter.InternalName, StringComparison.InvariantCultureIgnoreCase))?.Value : null;

                Control control = parameter.Type switch
                {
                    ParameterType.ComboBox => CreateComboBox(parameter, originalValue),
                    ParameterType.Formula => CreateFormulaTextBox(originalValue, parameter),
                    ParameterType.Odds => CreateNumericUpDown(originalValue, parameter),
                    ParameterType.Boolean => CreateCheckBox(originalValue, parameter),
                    ParameterType.Character => CreateCharacterControl(originalValue, parameter),
                    ParameterType.Color => CreateColorControl(originalValue, parameter),
                    ParameterType.Text => CreateTextBoxControl(originalValue, parameter),
                    ParameterType.Number => CreateNumberControl(originalValue, parameter),
                    ParameterType.NPC => CreateComboBox(parameter, originalValue),
                    ParameterType.Item => CreateComboBox(parameter, originalValue),
                    ParameterType.Trap => CreateComboBox(parameter, originalValue),
                    ParameterType.AlteredStatus => CreateComboBox(parameter, originalValue),
                    ParameterType.BooleanExpression => CreateFormulaTextBox(originalValue, parameter),
                    ParameterType.Key => new TextBox { Text = originalValue ?? parameter.Default },
                    ParameterType.Stat => CreateComboBox(parameter, originalValue),
                    ParameterType.Element => CreateComboBox(parameter, originalValue),
                    ParameterType.Script => CreateComboBox(parameter, originalValue),
                    _ => new TextBox { Text = originalValue ?? parameter.Default }
                };

                var toolTip = new ToolTip();
                if (nameLabel.PreferredSize.Width > nameLabel.Width)
                {
                    tlpParameters.Width += (nameLabel.PreferredSize.Width - nameLabel.Width);
                    this.Width += (nameLabel.PreferredSize.Width - nameLabel.Width);
                    btnSave.Left += (nameLabel.PreferredSize.Width - nameLabel.Width) / 2;
                    btnCancel.Left += (nameLabel.PreferredSize.Width - nameLabel.Width) / 2;
                }
                if (control is TableLayoutPanel tlp)
                    toolTip.SetToolTip(tlp.Controls[0], parameter.Description);
                else
                    toolTip.SetToolTip(control, parameter.Description);
                nameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                tlpParameters.Controls.Add(nameLabel, 0, rowNumber);
                tlpParameters.Controls.Add(control, 1, rowNumber);

                rowNumber++;
            }
            foreach (RowStyle rowStyle in tlpParameters.RowStyles)
            {
                rowStyle.SizeType = SizeType.Absolute;
                rowStyle.Height = 32;
            }
            tlpParameters.Height = 33 * rowNumber;
        }

        private IEnumerable<string> GetComboBoxItems(EffectParameter parameter)
        {
            return parameter.Type switch
            {
                ParameterType.ComboBox => parameter.ValidValues.Select(v => v.Value),
                ParameterType.NPC => ValidNPCs,
                ParameterType.Item => ValidItems,
                ParameterType.Trap => ValidTraps,
                ParameterType.AlteredStatus => ValidAlteredStatuses,
                ParameterType.Stat => ActiveDungeon.CharacterStats.Select(s => s.Id),
                ParameterType.Element => ActiveDungeon.ElementInfos.Select(e => e.Id),
                ParameterType.Script => ActiveDungeon.Scripts.Select(s => s.Id),
                _ => throw new ArgumentException($"{parameter.Type} is not valid for ComboBox parameter")
            };
        }

        private ComboBox CreateComboBox(EffectParameter parameter, string originalValue)
        {
            var comboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.Items.AddRange(GetComboBoxItems(parameter).ToArray());

            try
            {
                if (originalValue != null)
                {
                    var valueOfKey = comboBox.Items.Cast<string>().FirstOrDefault(vv => vv.Equals(originalValue, StringComparison.InvariantCultureIgnoreCase));
                    comboBox.Text = valueOfKey;
                }
                else
                {
                    comboBox.Text = parameter.Default;
                }
            }
            catch
            {
                comboBox.Text = parameter.Default;
            }

            return comboBox;
        }

        private TextBox CreateFormulaTextBox(string originalValue, EffectParameter parameter)
        {
            var textBox = new TextBox
            {
                Text = originalValue ?? parameter.Default
            };

            textBox.Enter += (sender, e) => PreviousTextBoxValue = textBox.Text;
            textBox.Leave += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text) && !textBox.Text.TestNumericExpression(true, out string errorMessage) && !textBox.Text.TestBooleanExpression(out errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid formula: {errorMessage}.",
                        "Invalid parameter data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    textBox.Text = PreviousTextBoxValue;
                }
            };

            return textBox;
        }

        private NumericUpDown CreateNumericUpDown(string originalValue, EffectParameter parameter)
        {
            var numericUpDown = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 100,
                Value = originalValue != null && int.TryParse(originalValue, out int parsedIntValue) ? parsedIntValue : int.Parse(parameter.Default)
            };

            return numericUpDown;
        }

        private CheckBox CreateCheckBox(string originalValue, EffectParameter parameter)
        {
            var checkBox = new CheckBox
            {
                Checked = originalValue != null && bool.TryParse(originalValue, out bool parsedBoolValue) ? parsedBoolValue : bool.Parse(parameter.Default)
            };

            return checkBox;
        }

        private TableLayoutPanel CreateTextBoxControl(string originalValue, EffectParameter parameter)
        {
            var textBoxPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            textBoxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            textBoxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            var textBox = new TextBox()
            {
                Dock = DockStyle.Fill
            };
            textBox.Text = originalValue ?? parameter.Default;
            var warningBox = new PictureBox()
            {
                ImageLocation = "./Icons/outline_info_black_24dp.png",
                SizeMode = PictureBoxSizeMode.StretchImage,
                Visible = false,
                Height = textBox.Height
            };
            var warningBoxTooltip = new ToolTip();
            warningBoxTooltip.SetToolTip(warningBox, "This value has been found as a Locale Entry key.\n\nIn-game, it will be replaced by the Locale Entry's value.");
            textBox.Tag = warningBox;
            textBox.ToggleEntryInLocaleWarning(ActiveDungeon, warningBox);
            textBox.TextChanged += (sender, e) =>
            {
                textBox.ToggleEntryInLocaleWarning(ActiveDungeon, textBox.Tag as Control);
            };

            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            textBoxPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            textBoxPanel.Controls.Add(textBox, 0, 0);
            textBoxPanel.Controls.Add(warningBox, 1, 0);

            return textBoxPanel;
        }

        private TextBox CreateNumberControl(string originalValue, EffectParameter parameter)
        {
            var textBox = new TextBox()
            {
                Dock = DockStyle.Fill
            };
            textBox.Text = originalValue ?? parameter.Default;
            textBox.Enter += (sender, e) => PreviousTextBoxValue = (sender as TextBox)?.Text;
            textBox.Leave += (sender, e) =>
            {
                var valueToValidate = textBox.Text;
                if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(false, out string errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid value: {errorMessage}.",
                        "Invalid parameter data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    textBox.Text = PreviousTextBoxValue;
                }
            };

            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            return textBox;
        }

        private TableLayoutPanel CreateCharacterControl(string originalValue, EffectParameter parameter)
        {
            var characterLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Text = originalValue ?? parameter.Default
            };

            var characterMapButton = new Button
            {
                Text = "Edit",
                Dock = DockStyle.Fill
            };
            characterMapButton.Click += (sender, e) =>
            {
                var characterMapResult = CharacterMapInputBox.ShowDialog(originalValue?[0]);
                if (characterMapResult.Saved)
                {
                    characterLabel.Text = characterMapResult.CharacterToSave.ToString();
                }
            };

            var characterPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            characterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            characterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            characterPanel.Controls.Add(characterLabel, 0, 0);
            characterPanel.Controls.Add(characterMapButton, 1, 0);

            return characterPanel;
        }

        private TableLayoutPanel CreateColorControl(string originalValue, EffectParameter parameter)
        {
            var colorTextBox = new TextBox
            {
                MaxLength = 15,
                Dock = DockStyle.Fill,
                Text = originalValue ?? parameter.Default
            };

            var colorButton = new Button
            {
                Dock = DockStyle.Fill,
                BackColor = !string.IsNullOrWhiteSpace(originalValue) ? originalValue.ToColor() : parameter.Default.ToColor()
            };

            colorButton.Click += (sender, e) =>
            {
                var colorDialog = new ColorDialog();
                try
                {
                    colorDialog.Color = colorTextBox.Text.ToColor();
                    colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
                }
                catch
                {
                    // Ignore invalid colors
                }

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    colorTextBox.Text = new GameColor(colorDialog.Color).ToString();
                    colorButton.BackColor = colorDialog.Color;
                }
            };

            colorTextBox.Enter += (sender, e) =>
            {
                PreviousTextBoxValue = colorTextBox.Text;
            };

            colorTextBox.Leave += (sender, e) =>
            {
                try
                {
                    var textColor = colorTextBox.Text.ToColor();
                    colorButton.BackColor = textColor;
                }
                catch
                {
                    MessageBox.Show(
                        $"You have entered an invalid color.",
                        "Invalid parameter data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    colorTextBox.Text = PreviousTextBoxValue;
                }
            };

            var colorPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Margin = new(0, 0, 0, 0),
                Padding = new(0, 0, 0, 0),
                AutoSize = true
            };

            colorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            colorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            colorPanel.Controls.Add(colorTextBox, 0, 0);
            colorPanel.Controls.Add(colorButton, 1, 0);

            return colorPanel;
        }

        private void tlpParameters_SizeChanged(object sender, EventArgs e)
        {
            var tableFinalY = tlpParameters.Location.Y + tlpParameters.Height;
            llblWikiParameters.Location = new Point(llblWikiParameters.Location.X, tableFinalY + 10);
            llblWikiAction.Location = new Point(llblWikiAction.Location.X, tableFinalY + 35);
            btnSave.Location = new Point(btnSave.Location.X, tableFinalY + 65);
            btnCancel.Location = new Point(btnCancel.Location.X, tableFinalY + 65);
            this.Height = tableFinalY + 145;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llblWikiParameters_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llblWikiParameters.LinkVisited = true;
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Shiigu/RogueCustoms/wiki/Effect-Parameters",
                UseShellExecute = true
            });
        }

        private void llblWikiAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llblWikiAction.LinkVisited = true;
            Process.Start(new ProcessStartInfo
            {
                FileName = $"https://github.com/Shiigu/RogueCustoms/wiki/{EffectTypeData.InternalName}",
                UseShellExecute = true
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var errorMessageStringBuilder = new StringBuilder();
            var paramsAndValues = new List<(string ParamName, string Value)>();
            for (int i = 0; i < EffectTypeData.Parameters.Count; i++)
            {
                var parameterData = EffectTypeData.Parameters[i];
                Control controlToValidate = null;
                var valueToValidate = string.Empty;
                controlToValidate = parameterData.Type == ParameterType.Character || parameterData.Type == ParameterType.Color || parameterData.Type == ParameterType.Text
                    ? tlpParameters.GetControlFromPosition(1, i).Controls[0]
                    : tlpParameters.GetControlFromPosition(1, i);

                switch (parameterData.Type)
                {
                    case ParameterType.ComboBox:
                        valueToValidate = (controlToValidate as ComboBox)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && (controlToValidate as ComboBox)?.Items.Contains(valueToValidate) != true)
                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" does not contain a valid value.");
                        else
                            valueToValidate = parameterData.ValidValues.Find(vv => vv.Value.Equals(valueToValidate)).Key;
                        break;
                    case ParameterType.NPC:
                    case ParameterType.Item:
                    case ParameterType.Trap:
                    case ParameterType.AlteredStatus:
                    case ParameterType.Stat:
                    case ParameterType.Element:
                    case ParameterType.Script:
                        valueToValidate = (controlToValidate as ComboBox)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && (controlToValidate as ComboBox)?.Items.Contains(valueToValidate) != true)
                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" does not contain a valid value.");
                        break;
                    case ParameterType.Formula:
                        valueToValidate = (controlToValidate as TextBox)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(true, out string errorMessage) && !valueToValidate.TestBooleanExpression(out errorMessage))
                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).Append("\" does not contain a valid formula: ").Append(errorMessage).AppendLine(".");
                        break;
                    case ParameterType.Character:
                        valueToValidate = (controlToValidate as Label)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.CanBeEncoded())
                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" does not contain a PxPlus character.");
                        break;
                    case ParameterType.Color:
                        valueToValidate = (controlToValidate as TextBox)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate))
                        {
                            try
                            {
                                _ = valueToValidate.ToColor();
                            }
                            catch
                            {
                                errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" does not contain a valid color.");
                            }
                        }
                        break;
                    case ParameterType.Text:
                    case ParameterType.Key:
                        valueToValidate = (controlToValidate as TextBox)?.Text;
                        break;
                    case ParameterType.Number:
                        valueToValidate = (controlToValidate as TextBox)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(false, out errorMessage) && !valueToValidate.TestBooleanExpression(out errorMessage))
                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).Append("\" does not contain a valid value: ").Append(errorMessage).AppendLine(".");
                        break;
                    case ParameterType.Odds:
                        valueToValidate = (controlToValidate as NumericUpDown)?.Value.ToString();
                        break;
                    case ParameterType.Boolean:
                        valueToValidate = (controlToValidate as CheckBox)?.Checked.ToString();
                        break;
                    case ParameterType.BooleanExpression:
                        valueToValidate = (controlToValidate as TextBox)?.Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestBooleanExpression(out errorMessage))
                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).Append("\" does not contain a valid expression: ").Append(errorMessage).AppendLine(".");
                        break;
                }

                paramsAndValues.Add((parameterData.InternalName, valueToValidate));

                if (parameterData.Required && !parameterData.OptionalIfFieldsHaveValue.Any() && string.IsNullOrWhiteSpace(valueToValidate))
                {
                    errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" is required.");
                }
                if (parameterData.Required && parameterData.OptionalIfFieldsHaveValue.Any() && string.IsNullOrWhiteSpace(valueToValidate))
                {
                    var fellowRequiredGroupHasValue = false;
                    var fellowRequiredCount = parameterData.OptionalIfFieldsHaveValue.Count;
                    var fellowRequiredEncountered = 0;
                    foreach (var paramName in parameterData.OptionalIfFieldsHaveValue)
                    {
                        if (!paramsAndValues.Exists(pav => pav.ParamName.Equals(paramName))) break;
                        fellowRequiredEncountered++;
                        var paramValue = paramsAndValues.Find(pav => pav.ParamName.Equals(paramName)).Value;
                        if (!string.IsNullOrWhiteSpace(paramValue))
                        {
                            fellowRequiredGroupHasValue = true;
                            break;
                        }
                    }
                    if (!fellowRequiredGroupHasValue && fellowRequiredEncountered == fellowRequiredCount)
                        errorMessageStringBuilder.Append("At least one of parameters (").Append(parameterData.DisplayName).Append(", ").AppendJoin(", ", parameterData.OptionalIfFieldsHaveValue).AppendLine(") is required.");
                }
            }
            var errorMessages = errorMessageStringBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(errorMessages))
            {
                MessageBox.Show(
                    $"These parameters could not be saved due to the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Invalid parameter data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                if (string.IsNullOrWhiteSpace(EffectToSave.EffectName))
                    EffectToSave.EffectName = EffectTypeData.InternalName;
                foreach (var (ParamName, Value) in paramsAndValues)
                {
                    var paramToSave = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(ParamName, StringComparison.InvariantCultureIgnoreCase));
                    if (paramToSave != null)                 // This should always be true
                    {
                        paramToSave.Value = Value;
                    }
                    else
                    {
                        EffectToSave.Params = EffectToSave.Params.Append(new Parameter
                        {
                            ParamName = ParamName,
                            Value = Value
                        }).ToArray();
                    }
                }
                Saved = true;
                this.Close();
            }
        }
    }
#pragma warning restore IDE1006 // Estilos de nombres
#pragma warning restore RCS1077 // Optimize LINQ method call.
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
#pragma warning restore S2259 // Null pointers should not be dereferenced
#pragma warning restore S6602 // "Find" method should be used instead of the "FirstOrDefault" extension
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
}

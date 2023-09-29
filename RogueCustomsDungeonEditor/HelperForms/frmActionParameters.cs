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
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using Label = System.Windows.Forms.Label;
using Parameter = RogueCustomsGameEngine.Utils.JsonImports.Parameter;
using Point = System.Drawing.Point;
using Timer = System.Windows.Forms.Timer;

namespace RogueCustomsDungeonEditor.HelperForms
{
    public partial class frmActionParameters : Form
    {
        public EffectInfo EffectToSave { get; private set; }
        public bool Saved { get; private set; }
        public List<string> ValidAlteredStatuses { get; private set; }
        public EffectTypeData EffectTypeData { get; private set; }

        private DungeonInfo ActiveDungeon;
        private string PreviousTextBoxValue;

        private readonly List<ParameterType> TypesThatAlwaysHoldAValue = new()
        {
            ParameterType.Boolean,
            ParameterType.Odds,
            ParameterType.Number
        };

        public frmActionParameters(EffectInfo effectToSave, DungeonInfo activeDungeon, EffectTypeData paramsData, List<string> validAlteredStatuses, bool isActionEdit)
        {
            InitializeComponent();
            ActiveDungeon = activeDungeon;
            if (paramsData.InternalName.Equals(effectToSave?.EffectName))
            {
                EffectToSave = effectToSave.Clone();
                if (EffectToSave.Params.Count() != paramsData.Parameters.Count)
                {
                    var newParamsList = new Parameter[paramsData.Parameters.Count];
                    for (int i = 0; i < EffectToSave.Params.Length; i++)
                    {
                        newParamsList[i] = new Parameter();
                        newParamsList[i].ParamName = paramsData.Parameters[i].InternalName;
                        var existingParam = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(newParamsList[i].ParamName));
                        newParamsList[i].Value = existingParam != null ? existingParam.Value : paramsData.Parameters[i].Default;
                    }
                }
            }
            else
            {
                EffectToSave = new EffectInfo();
                EffectToSave.EffectName = paramsData.InternalName;
                EffectToSave.Params = new Parameter[paramsData.Parameters.Count];
                for (int i = 0; i < EffectToSave.Params.Length; i++)
                {
                    EffectToSave.Params[i] = new Parameter();
                    EffectToSave.Params[i].ParamName = paramsData.Parameters[i].InternalName;
                    EffectToSave.Params[i].Value = paramsData.Parameters[i].Default;
                }
            }
            EffectTypeData = paramsData;
            ValidAlteredStatuses = validAlteredStatuses;
            lblDisplayName.Text = $"{paramsData.DisplayName}:";
            lblDescription.Text = paramsData.Description;

            if (lblDisplayName.PreferredWidth > lblDisplayName.Width)
            {
                var widthChange = lblDisplayName.Width - lblDisplayName.PreferredWidth;
                lblDisplayName.Width = lblDisplayName.PreferredWidth;
                lblDescription.Width -= widthChange;
                lblRequired.Width -= widthChange;
                tlpParameters.Width -= widthChange;
                llblWikiAction.Width -= widthChange;
                btnSave.Width -= widthChange / 2;
                btnCancel.Location = new Point(btnCancel.Location.X - widthChange / 2, btnCancel.Location.Y);
                btnCancel.Width -= widthChange / 2;
                this.Width -= widthChange;
            }

            var rowNumber = 0;
            llblWikiAction.Focus();

            foreach (var parameter in paramsData.Parameters)
            {
                var nameLabel = new Label
                {
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = false,
                    Height = 30
                };

                if (parameter.Required && !TypesThatAlwaysHoldAValue.Contains(parameter.Type) && !parameter.OptionalIfFieldsHaveValue.Any())
                {
                    nameLabel.Text = $"{parameter.DisplayName}*";
                }
                else
                {
                    nameLabel.Text = parameter.Required && !TypesThatAlwaysHoldAValue.Contains(parameter.Type) && parameter.OptionalIfFieldsHaveValue.Any()
                        ? $"{parameter.DisplayName}^"
                        : parameter.DisplayName;
                }

                Control control = null;

                var originalValue = (isActionEdit) ? effectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(parameter.InternalName, StringComparison.InvariantCultureIgnoreCase))?.Value : null;

                switch (parameter.Type)
                {
                    case ParameterType.ComboBox:
                        var comboBox = new ComboBox
                        {
                            DropDownStyle = ComboBoxStyle.DropDownList
                        };
                        comboBox.Items.AddRange(parameter.ValidValues.Select(v => v.Value).ToArray());

                        try
                        {
                            if (originalValue != null)
                            {
                                var valueOfKey = parameter.ValidValues.Find(vv => vv.Key.Equals(originalValue, StringComparison.InvariantCultureIgnoreCase)).Value;
                                comboBox.Text = valueOfKey;
                            }
                            else
                                comboBox.Text = parameter.Default;
                        }
                        catch (Exception ex)
                        {
                            comboBox.Text = parameter.Default;
                        }
                        control = comboBox;
                        break;
                    case ParameterType.Formula:
                        control = new TextBox();
                        ((TextBox)control).Text = originalValue ?? parameter.Default;
                        control.Enter += (sender, e) => PreviousTextBoxValue = (sender as TextBox)?.Text;
                        control.Leave += (sender, e) =>
                        {
                            var valueToValidate = (sender as TextBox)?.Text;
                            if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(true, out string errorMessage))
                            {
                                MessageBox.Show(
                                    $"You have entered an invalid formula: {errorMessage}.",
                                    "Invalid parameter data",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                                (sender as TextBox).Text = PreviousTextBoxValue;
                            }
                        };
                        break;
                    case ParameterType.Odds:
                        control = new NumericUpDown
                        {
                            Minimum = 1,
                            Maximum = 100
                        };
                        if (originalValue != null && int.TryParse(originalValue, out int parsedIntValue))
                            ((NumericUpDown)control).Value = parsedIntValue;
                        else if (originalValue == null)
                            ((NumericUpDown)control).Value = int.Parse(parameter.Default);
                        break;
                    case ParameterType.Boolean:
                        control = new CheckBox();
                        if (originalValue != null && bool.TryParse(originalValue, out bool parsedBoolValue))
                            ((CheckBox)control).Checked = parsedBoolValue;
                        else if (originalValue == null)
                            ((CheckBox)control).Checked = bool.Parse(parameter.Default);
                        break;
                    case ParameterType.Character:
                        control = new Label
                        {
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleCenter,
                            AutoSize = false
                        };
                        ((Label)control).Text = originalValue ?? parameter.Default;
                        var characterMapButton = new Button
                        {
                            Text = "Edit",
                            Dock = DockStyle.Fill,
                        };
                        characterMapButton.Click += (sender, e) =>
                        {
                            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), originalValue?[0]);
                            characterMapForm.ShowDialog();
                            if (characterMapForm.Saved)
                            {
                                ((Label)characterMapButton.Tag).Text = characterMapForm.CharacterToSave.ToString();
                            }
                        };
                        characterMapButton.Tag = control;
                        var characterMapTooltip = new ToolTip();
                        characterMapTooltip.SetToolTip(characterMapButton, "Change Character Representation");
                        var characterPanel = new TableLayoutPanel
                        {
                            ColumnCount = 2,
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                        };

                        characterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        characterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                        control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                        characterMapButton.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

                        characterPanel.Controls.Add(control, 0, 0);
                        characterPanel.Controls.Add(characterMapButton, 1, 0);
                        control = characterPanel;
                        break;
                    case ParameterType.Color:
                        control = new TextBox
                        {
                            MaxLength = 15,
                            Dock = DockStyle.Fill,
                        };
                        var colorButton = new Button
                        {
                            Dock = DockStyle.Fill,
                        };
                        if (!string.IsNullOrWhiteSpace(originalValue))
                        {
                            ((TextBox)control).Text = originalValue;
                            colorButton.BackColor = originalValue.ToColor();
                        }
                        else
                        {
                            try
                            {
                                colorButton.BackColor = parameter.Default.ToColor();
                                ((TextBox)control).Text = parameter.Default;
                            }
                            catch (Exception ex)
                            { }
                        }
                        colorButton.Tag = control;
                        colorButton.Click += (sender, e) =>
                        {
                            var textColor = (colorButton.Tag as TextBox).Text;
                            var colorDialog = new ColorDialog();
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(textColor))
                                {
                                    colorDialog.Color = textColor.ToColor();
                                    colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
                                }
                            }
                            catch { }
                            if (colorDialog.ShowDialog() == DialogResult.OK)
                            {
                                ((TextBox)colorButton.Tag).Text = new GameColor(colorDialog.Color).ToString();
                                colorButton.BackColor = colorDialog.Color;
                            }
                        };
                        control.Enter += (sender, e) => PreviousTextBoxValue = (sender as TextBox)?.Text;
                        control.Leave += (sender, e) =>
                        {
                            var valueToValidate = (sender as TextBox).Text;
                            if (!string.IsNullOrWhiteSpace(valueToValidate))
                            {
                                try
                                {
                                    var textBox = sender as TextBox;
                                    colorButton.BackColor = valueToValidate.ToColor();
                                }
                                catch
                                {
                                    MessageBox.Show(
                                        $"You have entered an invalid color.",
                                        "Invalid parameter data",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                    (sender as TextBox).Text = PreviousTextBoxValue;
                                }
                            }
                            colorButton.BackColor = SystemColors.ButtonFace;
                        };
                        var colorTooltip = new ToolTip();
                        colorTooltip.SetToolTip(colorButton, "Change Color");
                        var colorPanel = new TableLayoutPanel
                        {
                            ColumnCount = 2,
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                        };

                        colorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
                        colorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

                        control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                        colorButton.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

                        colorPanel.Controls.Add(control, 0, 0);
                        colorPanel.Controls.Add(colorButton, 1, 0);
                        control = colorPanel;
                        break;
                    case ParameterType.Text:
                        control = new TextBox()
                        {
                            Dock = DockStyle.Fill
                        };
                        ((TextBox)control).Text = originalValue ?? parameter.Default;
                        var warningBox = new PictureBox()
                        {
                            ImageLocation = "./Icons/outline_info_black_24dp.png",
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Visible = false,
                            Height = control.Height
                        };
                        var warningBoxTooltip = new ToolTip();
                        warningBoxTooltip.SetToolTip(warningBox, "This value has been found as a Locale Entry key.\n\nIn-game, it will be replaced by the Locale Entry's value.");
                        control.Tag = warningBox;
                        ((TextBox)control).ToggleEntryInLocaleWarning(ActiveDungeon, warningBox);
                        control.TextChanged += (sender, e) =>
                        {
                            var textBox = sender as TextBox;
                            textBox.ToggleEntryInLocaleWarning(ActiveDungeon, textBox.Tag as Control);
                        };

                        var textBoxPanel = new TableLayoutPanel
                        {
                            ColumnCount = 2,
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                        };
                        textBoxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
                        textBoxPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

                        control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                        textBoxPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

                        textBoxPanel.Controls.Add(control, 0, 0);
                        textBoxPanel.Controls.Add(warningBox, 1, 0);
                        control = textBoxPanel;
                        break;
                    case ParameterType.AlteredStatus:
                        var alteredStatusComboBox = new ComboBox();
                        alteredStatusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        foreach (var alteredStatus in ValidAlteredStatuses)
                        {
                            alteredStatusComboBox.Items.Add(alteredStatus);
                        }
                        try
                        {
                            if (originalValue != null)
                            {
                                var valueOfKey = ValidAlteredStatuses.Find(vals => vals.Equals(originalValue, StringComparison.InvariantCultureIgnoreCase));
                                alteredStatusComboBox.Text = valueOfKey;
                            }
                            else
                                alteredStatusComboBox.Text = parameter.Default;
                        }
                        catch (Exception ex)
                        {
                            alteredStatusComboBox.Text = parameter.Default;
                        }
                        control = alteredStatusComboBox;
                        break;
                    case ParameterType.Number:
                        control = new TextBox();
                        ((TextBox)control).Text = originalValue ?? parameter.Default;
                        control.Enter += (sender, e) => PreviousTextBoxValue = (sender as TextBox)?.Text;
                        control.Leave += (sender, e) =>
                        {
                            var valueToValidate = (sender as TextBox)?.Text;
                            if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(false, out string errorMessage))
                            {
                                MessageBox.Show(
                                    $"You have entered an invalid value: {errorMessage}.",
                                    "Invalid parameter data",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                                (sender as TextBox).Text = PreviousTextBoxValue;
                            }
                        };
                        break;
                    case ParameterType.BooleanExpression:
                        control = new TextBox();
                        ((TextBox)control).Text = originalValue ?? parameter.Default;
                        control.Enter += (sender, e) => PreviousTextBoxValue = (sender as TextBox)?.Text;
                        control.Leave += (sender, e) =>
                        {
                            var valueToValidate = (sender as TextBox)?.Text;
                            if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestBooleanExpression(out string errorMessage))
                            {
                                MessageBox.Show(
                                    $"You have entered an invalid expression: {errorMessage}.",
                                    "Invalid parameter data",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                                (sender as TextBox).Text = PreviousTextBoxValue;
                            }
                        };
                        break;
                    case ParameterType.Key:
                        control = new TextBox()
                        {
                            Dock = DockStyle.Fill
                        };
                        ((TextBox)control).Text = originalValue ?? parameter.Default;
                        control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                        break;
                }

                var toolTip = new ToolTip();
                if (control is TableLayoutPanel tlp)
                    toolTip.SetToolTip(tlp.Controls[0], parameter.Description);
                else
                    toolTip.SetToolTip(control, parameter.Description);
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

                tlpParameters.Controls.Add(nameLabel, 0, rowNumber);
                tlpParameters.Controls.Add(control, 1, rowNumber);

                rowNumber++;
            }
            tlpParameters.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tlpParameters.Height = 30 * rowNumber;
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
                FileName = $"https://github.com/Shiigu/RogueCustoms/wiki/Effect-Parameters",
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
                        valueToValidate = (controlToValidate as ComboBox).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !(controlToValidate as ComboBox).Items.Contains(valueToValidate))
                            errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a valid value.");
                        else
                            valueToValidate = parameterData.ValidValues.Find(vv => vv.Value.Equals(valueToValidate)).Key;
                        break;
                    case ParameterType.AlteredStatus:
                        valueToValidate = (controlToValidate as ComboBox).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !(controlToValidate as ComboBox).Items.Contains(valueToValidate))
                            errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a valid value.");
                        break;
                    case ParameterType.Formula:
                        valueToValidate = (controlToValidate as TextBox).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(true, out string errorMessage))
                            errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a valid formula: {errorMessage}.");
                        break;
                    case ParameterType.Character:
                        valueToValidate = (controlToValidate as Label).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.CanBeEncodedToIBM437())
                            errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a IBM437 character.");
                        break;
                    case ParameterType.Color:
                        valueToValidate = (controlToValidate as TextBox).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate))
                        {
                            try
                            {
                                _ = valueToValidate.ToColor();
                            }
                            catch
                            {
                                errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a valid color.");
                            }
                        }
                        break;
                    case ParameterType.Text:
                    case ParameterType.Key:
                        valueToValidate = (controlToValidate as TextBox).Text;
                        break;
                    case ParameterType.Number:
                        valueToValidate = (controlToValidate as TextBox).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestNumericExpression(false, out errorMessage))
                            errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a valid value: {errorMessage}.");
                        break;
                    case ParameterType.Odds:
                        valueToValidate = (controlToValidate as NumericUpDown).Value.ToString();
                        break;
                    case ParameterType.Boolean:
                        valueToValidate = (controlToValidate as CheckBox).Checked.ToString();
                        break;
                    case ParameterType.BooleanExpression:
                        valueToValidate = (controlToValidate as TextBox).Text;
                        if (!string.IsNullOrWhiteSpace(valueToValidate) && !valueToValidate.TestBooleanExpression(out errorMessage))
                            errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" does not contain a valid expression: {errorMessage}.");
                        break;
                }

                paramsAndValues.Add((parameterData.InternalName, valueToValidate));

                if (parameterData.Required && !parameterData.OptionalIfFieldsHaveValue.Any() && string.IsNullOrWhiteSpace(valueToValidate))
                {
                    errorMessageStringBuilder.AppendLine($"Parameter \"{parameterData.DisplayName}\" is required.");
                }
                if (parameterData.Required && parameterData.OptionalIfFieldsHaveValue.Any() && string.IsNullOrWhiteSpace(valueToValidate))
                {
                    var fellowRequiredGroupHasValue = false;
                    var fellowRequiredCount = parameterData.OptionalIfFieldsHaveValue.Count;
                    var fellowRequiredEncountered = 0;
                    foreach (var paramName in parameterData.OptionalIfFieldsHaveValue)
                    {
                        if (!paramsAndValues.Any(pav => pav.ParamName.Equals(paramName))) break;
                        fellowRequiredEncountered++;
                        var paramValue = paramsAndValues.Find(pav => pav.ParamName.Equals(paramName)).Value;
                        if (!string.IsNullOrWhiteSpace(paramValue))
                        {
                            fellowRequiredGroupHasValue = true;
                            break;
                        }
                    }
                    if (!fellowRequiredGroupHasValue && fellowRequiredEncountered == fellowRequiredCount)
                        errorMessageStringBuilder.AppendLine($"At least one of parameters ({parameterData.DisplayName}, {string.Join(", ", parameterData.OptionalIfFieldsHaveValue)}) is required.");
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
                foreach (var param in paramsAndValues)
                {
                    var paramToSave = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(param.ParamName, StringComparison.InvariantCultureIgnoreCase));
                    if (paramToSave != null)                 // This should always be true
                    {
                        paramToSave.Value = param.Value;
                    }
                }
                Saved = true;
                this.Close();
            }
        }
    }
}

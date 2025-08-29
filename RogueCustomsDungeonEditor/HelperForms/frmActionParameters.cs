using MathNet.Numerics;

using org.matheval;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public List<string> ValidTileTypes { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ValidAlteredStatuses { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ValidActionSchools { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EffectTypeData EffectTypeData { get; private set; }

        private readonly DungeonInfo ActiveDungeon;
        private string PreviousTextBoxValue;
        private readonly Font FontToUse;

        private readonly List<ParameterType> TypesThatAlwaysHoldAValue = new()
        {
            ParameterType.Boolean,
            ParameterType.Odds,
            ParameterType.Number
        };

        public frmActionParameters(EffectInfo effectToSave, DungeonInfo activeDungeon, EffectTypeData paramsData, List<string> validNPCs, List<string> validItems, List<string> validTraps, List<string> validTileTypes, List<string> validAlteredStatuses, bool isActionEdit)
        {
            InitializeComponent();
            ActiveDungeon = activeDungeon;
            if (paramsData.InternalName.Equals(effectToSave?.EffectName))
            {
                EffectToSave = effectToSave.Clone();
                if (EffectToSave.Params.Length != paramsData.Parameters.Count)
                {
                    var newParamsList = new List<Parameter>(paramsData.Parameters.Count);
                    for (int i = 0; i < EffectToSave.Params.Length; i++)
                    {
                        var paramData = paramsData.Parameters.FirstOrDefault(param => param.InternalName.Equals(EffectToSave.Params[i].ParamName, StringComparison.InvariantCultureIgnoreCase));
                        if (paramData == null) continue;
                        if (paramData.Type != ParameterType.Table)
                        {
                            newParamsList.Add(new Parameter
                            {
                                ParamName = paramData.InternalName
                            });
                            var existingParam = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(newParamsList[i].ParamName, StringComparison.InvariantCultureIgnoreCase));
                            newParamsList[i].Value = existingParam != null ? existingParam.Value : paramsData.Parameters[i].Default;
                        }
                        else
                        {
                            newParamsList.Add(new Parameter
                            {
                                ParamName = paramData.InternalName
                            });
                            var existingParam = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(newParamsList[i].ParamName, StringComparison.InvariantCultureIgnoreCase));
                            var existingId = existingParam.Value.Split('|').FirstOrDefault();
                            newParamsList[i].Value = existingParam != null ? existingParam.Value : null;
                        }
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
            ValidNPCs = validNPCs.Union(["<<CUSTOM>>"]).ToList();
            ValidItems = validItems.Union(["<<CUSTOM>>"]).ToList();
            ValidTraps = validTraps.Union(["<<CUSTOM>>"]).ToList();
            ValidTileTypes = validTileTypes.Union(["<<CUSTOM>>"]).ToList();
            ValidAlteredStatuses = validAlteredStatuses.Union(["<<CUSTOM>>"]).ToList();
            ValidActionSchools = activeDungeon.ActionSchoolInfos.Select(asi => asi.Id).Union(["All", "<<CUSTOM>>"]).ToList();

            var fontPath = Path.Combine(Application.StartupPath, "Resources\\PxPlus_Tandy1K-II_200L.ttf");
            var fontName = "PxPlus Tandy1K-II 200L";
            if (FontHelpers.LoadFont(fontPath))
            {
                var loadedFont = FontHelpers.GetFontByName(fontName);
                if (loadedFont != null)
                {
                    FontToUse = new Font(loadedFont, 8f, FontStyle.Regular);
                }
            }

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

            tlpParameters.Height = 0;
            foreach (var parameter in paramsData.Parameters)
            {
                var nameLabel = parameter.Type != ParameterType.Table ? new Label
                {
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = false,
                    Height = 30
                } : null;

                string originalValue = isActionEdit ? effectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(parameter.InternalName, StringComparison.InvariantCultureIgnoreCase))?.Value : null;
                var tableValues = new List<SelectionItem>();

                if(isActionEdit && parameter.Type == ParameterType.Table)
                {
                    foreach (var param in effectToSave.Params.Where(p => p.ParamName.Equals(parameter.InternalName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        if (param.Value != null)
                        {
                            var values = param.Value.Split('|');
                            tableValues.Add(new SelectionItem(values[0], values.Length > 1 ? values[1] : string.Empty, values.Length > 2 ? values[2] : string.Empty));
                        }
                    }
                }

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
                    ParameterType.TileType => CreateComboBox(parameter, originalValue),
                    ParameterType.Trap => CreateComboBox(parameter, originalValue),
                    ParameterType.AlteredStatus => CreateComboBox(parameter, originalValue),
                    ParameterType.ActionSchool => CreateComboBox(parameter, originalValue),
                    ParameterType.BooleanExpression => CreateFormulaTextBox(originalValue, parameter),
                    ParameterType.Key => new TextBox { Text = originalValue ?? parameter.Default },
                    ParameterType.Area => CreateComboBox(parameter, originalValue),
                    ParameterType.Stat => CreateComboBox(parameter, originalValue),
                    ParameterType.Element => CreateComboBox(parameter, originalValue),
                    ParameterType.Script => CreateComboBox(parameter, originalValue),
                    ParameterType.Table => CreateTableControl(tableValues, parameter, tlpParameters),
                    _ => new TextBox { Text = originalValue ?? parameter.Default }
                };

                var toolTip = new ToolTip();
                if (nameLabel != null && nameLabel.PreferredSize.Width > nameLabel.Width)
                {
                    tlpParameters.Width += (nameLabel.PreferredSize.Width - nameLabel.Width);
                    this.Width += (nameLabel.PreferredSize.Width - nameLabel.Width);
                    btnSave.Left += (nameLabel.PreferredSize.Width - nameLabel.Width) / 2;
                    btnCancel.Left += (nameLabel.PreferredSize.Width - nameLabel.Width) / 2;
                }
                else if (nameLabel == null && control.PreferredSize.Width > control.Width)
                {
                    tlpParameters.Width += (control.PreferredSize.Width - control.Width);
                    this.Width += (control.PreferredSize.Width - control.Width);
                    btnSave.Left += (control.PreferredSize.Width - control.Width) / 2;
                    btnCancel.Left += (control.PreferredSize.Width - control.Width) / 2;
                }
                if (control is TableLayoutPanel tlp)
                    toolTip.SetToolTip(tlp.Controls[0], parameter.Description);
                else
                    toolTip.SetToolTip(control, parameter.Description);

                if (nameLabel != null)
                {
                    nameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    tlpParameters.Controls.Add(nameLabel, 0, rowNumber);
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    tlpParameters.Controls.Add(control, 1, rowNumber);
                }
                else
                {
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    tlpParameters.Controls.Add(control, 0, rowNumber);
                    tlpParameters.SetColumnSpan(control, 2);
                }

                if (parameter.Type != ParameterType.Table)
                    tlpParameters.Height += 33;
                else
                    tlpParameters.Height += 200;
                rowNumber++;
            }
            RecalculateFields();
        }

        private void RecalculateFields()
        {
            for (int i = 0; i < EffectTypeData.Parameters.Count; i++)
            {
                var parameterData = EffectTypeData.Parameters[i];

                var required = GetRequired(parameterData);
                if (tlpParameters.GetControlFromPosition(0, i) is Label label)
                {
                    label.Text = required
                        ? $"{parameterData.DisplayName}*"
                        : $"{parameterData.DisplayName}";
                }

                var readOnly = GetReadOnly(parameterData);
                if (tlpParameters.GetControlFromPosition(1, i) is TableLayoutPanel tlp)
                {
                    foreach (Control innerControl in tlp.Controls)
                    {
                        innerControl.Enabled = !readOnly;
                        if (readOnly)
                        {
                            if (innerControl is NumericUpDown nud)
                            {
                                nud.Value = parameterData.Default != null && int.TryParse(parameterData.Default, out int parsedIntValue) ? parsedIntValue : 0;
                            }
                            else if (innerControl is CheckBox checkBox)
                            {
                                checkBox.Checked = parameterData.Default != null && bool.TryParse(parameterData.Default, out bool parsedBoolValue) ? parsedBoolValue : false;
                            }
                            else if (innerControl is TextBox textBox)
                            {
                                textBox.Text = parameterData.Default ?? string.Empty;
                            }
                            else if (innerControl is Label innerLabel)
                            {
                                innerLabel.Text = parameterData.Default ?? string.Empty;
                            }
                        }
                    }
                }
            }
        }

        public bool GetRequired(EffectParameter parameter)
        {
            var parsedExpression = parameter.Required;
            foreach (var field in EffectTypeData.Parameters)
            {
                parsedExpression = parsedExpression.Replace($"{{{field.InternalName}}}", $"\"{GetValue(field.InternalName)}\"");
            }
            return new Expression(parsedExpression).Eval<bool>();
        }

        public bool GetReadOnly(EffectParameter parameter)
        {
            var parsedExpression = parameter.ReadOnly;
            foreach (var field in EffectTypeData.Parameters)
            {
                parsedExpression = parsedExpression.Replace($"{{{field.InternalName}}}", $"\"{GetValue(field.InternalName)}\"");
            }
            return new Expression(parsedExpression).Eval<bool>();
        }

        private string GetValue(string fieldName)
        {
            var field = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
            var fieldIndex = EffectTypeData.Parameters.FindIndex(p => p.InternalName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
            var control = GetAppropriateControl(EffectTypeData.Parameters[fieldIndex], fieldIndex);
            return control switch
            {
                Label label => label.Text,
                TextBox textBox => textBox.Text,
                NumericUpDown numericUpDown => numericUpDown.Value.ToString(),
                CheckBox checkBox => checkBox.Checked.ToString(),
                ComboBox comboBox => comboBox.Text,
                TableLayoutPanel tableLayoutPanel => string.Join("|", tableLayoutPanel.Controls.OfType<TextBox>().Select(tb => tb.Text)),
                _ => string.Empty // Unsupported
            };
        }

        private IEnumerable<string> GetComboBoxItems(EffectParameter parameter)
        {
            return parameter.Type switch
            {
                ParameterType.ComboBox => parameter.ValidValues.Select(v => v.Value),
                ParameterType.NPC => ValidNPCs,
                ParameterType.Item => ValidItems,
                ParameterType.Trap => ValidTraps,
                ParameterType.TileType => ValidTileTypes,
                ParameterType.AlteredStatus => ValidAlteredStatuses,
                ParameterType.ActionSchool => ValidActionSchools,
                ParameterType.Area => GetAreas(),
                ParameterType.Stat => ActiveDungeon.CharacterStats.Select(s => s.Id),
                ParameterType.Element => ActiveDungeon.ElementInfos.Select(e => e.Id),
                ParameterType.Script => ActiveDungeon.Scripts.Select(s => s.Id),
                _ => throw new ArgumentException($"{parameter.Type} is not valid for ComboBox parameter")
            };
        }

        private static List<string> GetAreas()
        {
            var areaList = new List<string>();

            for (int i = 2; i < 10; i++)
            {
                areaList.Add($"Circle (Diametre {i})");
                areaList.Add($"Square ({i}x{i})");
            }

            areaList.Add("Whole Room");
            areaList.Add("Whole Map");

            return areaList;
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

            comboBox.Leave += (sender, e) => RecalculateFields();
            comboBox.SelectedValueChanged += (sender, e) => RecalculateFields();
            comboBox.TextChanged += (sender, e) => RecalculateFields();

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

            textBox.Leave += (sender, e) => RecalculateFields();

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

            numericUpDown.Leave += (sender, e) => RecalculateFields();

            return numericUpDown;
        }

        private CheckBox CreateCheckBox(string originalValue, EffectParameter parameter)
        {
            var checkBox = new CheckBox
            {
                Checked = originalValue != null && bool.TryParse(originalValue, out bool parsedBoolValue) ? parsedBoolValue : bool.Parse(parameter.Default)
            };

            checkBox.CheckedChanged += (sender, e) => RecalculateFields();

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

            textBox.Leave += (sender, e) => RecalculateFields();

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

            textBox.Leave += (sender, e) => RecalculateFields();

            return textBox;
        }

        private TableLayoutPanel CreateCharacterControl(string originalValue, EffectParameter parameter)
        {
            var characterLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Text = originalValue ?? parameter.Default,
                Font = FontToUse
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
                    RecalculateFields();
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
                    colorDialog.Color = (!string.IsNullOrWhiteSpace(colorTextBox.Text)) ? colorTextBox.Text.ToColor() : Color.White;
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
                    RecalculateFields();
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
                    if (!string.IsNullOrWhiteSpace(colorTextBox.Text))
                    {
                        var textColor = colorTextBox.Text.ToColor();
                        colorButton.BackColor = textColor;
                    }
                    else
                    {
                        colorButton.BackColor = Color.White;
                    }
                    RecalculateFields();
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

        private TableLayoutPanel CreateTableControl(List<SelectionItem> originalValues, EffectParameter parameter, TableLayoutPanel parent)
        {
            var nameLabel = new Label
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Height = 30
            };

            var table = new DataGridView
            {
                ColumnHeadersVisible = true,
                AllowUserToAddRows = true,
                AllowUserToDeleteRows = true,
                AllowUserToResizeColumns = true,
                ScrollBars = ScrollBars.Vertical
            };

            var columns = new List<DataGridViewTextBoxColumn>();

            foreach (var column in parameter.Columns)
            {
                var columnToAdd = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = column.Key,
                    Name = column.Key,
                    HeaderText = column.Header
                };
                columns.Add(columnToAdd);
            }
            table.Columns.AddRange([.. columns]);

            for (int i = 0; i < originalValues.Count; i++)
            {
                if (columns.Count > 2)
                    table.Rows.Add(originalValues[i].Id, originalValues[i].Name, originalValues[i].Description);
                else
                    table.Rows.Add(originalValues[i].Id, originalValues[i].Name);
            }

            table.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            var tablePanel = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Height = 200
            };

            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            tablePanel.Controls.Add(nameLabel, 0, 0);
            tablePanel.Controls.Add(table, 0, 1);

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];
                var columnData = parameter.Columns[i];

                if (columnData.Key == "Id")
                    column.Width = (int)(parent.Width * 0.3);
                else if (columnData.Key == "Name" && table.Columns.Count == 2)
                    column.Width = (int)(parent.Width * 0.7);
                else
                    column.Width = (int)(parent.Width * 0.35);
            }

            tablePanel.Leave += (sender, e) => RecalculateFields();

            return tablePanel;
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
            var paramsAndValues = new List<(string ParamName, string Value, ParameterType Type)>();
            for (int i = 0; i < EffectTypeData.Parameters.Count; i++)
            {
                var parameterData = EffectTypeData.Parameters[i];
                var controlToValidate = GetAppropriateControl(parameterData, i);
                var valueToValidate = string.Empty;
                var tableItems = new List<SelectionItem>();
                var required = GetRequired(parameterData);

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
                    case ParameterType.TileType:
                    case ParameterType.AlteredStatus:
                    case ParameterType.ActionSchool:
                    case ParameterType.Area:
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
                    case ParameterType.Table:
                        if (controlToValidate is DataGridView tableToValidate)
                        {
                            if (tableToValidate.Rows[0].IsNewRow && required)
                            {
                                errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" has no rows.");
                                break;
                            }
                            var foundUniques = new Dictionary<string, List<string>>();
                            foreach (DataGridViewRow row in tableToValidate.Rows)
                            {
                                if (row.IsNewRow) continue;

                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    var rowIndex = cell.OwningRow.Index;
                                    var headerText = cell.OwningColumn.HeaderText;
                                    var correspondingColumn = parameterData.Columns.FirstOrDefault(c => c.Header.Equals(headerText, StringComparison.InvariantCultureIgnoreCase));
                                    if ((correspondingColumn.Required || correspondingColumn.Unique) && string.IsNullOrWhiteSpace(cell.Value.ToString()))
                                    {
                                        errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).Append("\" has an empty value, which is not allowed, in row ").Append(rowIndex).Append(", column ").AppendLine(correspondingColumn.Header);
                                        break;
                                    }
                                    if(correspondingColumn.Unique && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                                    {
                                        var foundValues = foundUniques[correspondingColumn.Key];
                                        if(!foundValues.Contains(cell.Value.ToString()))
                                        {
                                            foundValues.Add(cell.Value.ToString());
                                            errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).Append("\" has a duplicate ").Append(correspondingColumn.Key).Append(", which is not allowed, in row ").Append(rowIndex).Append(", column ").AppendLine(correspondingColumn.Header);
                                        }
                                    }
                                    if (cell.Value.ToString().Contains('|'))
                                    {
                                        errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" contains a pipe (|), which is not allowed, in row ").Append(rowIndex).Append(", column ").AppendLine(correspondingColumn.Header);
                                        break;
                                    }
                                }
                            }
                            if(errorMessageStringBuilder.Length == 0)
                            {
                                foreach (DataGridViewRow row in tableToValidate.Rows)
                                {
                                    if (row.IsNewRow) continue;
                                    var valueStringBuilder = new StringBuilder();
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        if (string.IsNullOrWhiteSpace(cell.Value.ToString())) break;
                                        valueStringBuilder.Append(cell.Value.ToString()).Append('|');
                                    }
                                    paramsAndValues.Add((parameterData.InternalName, valueStringBuilder.ToString()[..^1], parameterData.Type));
                                }
                            }
                        }
                        break;
                }

                if (parameterData.Type != ParameterType.Table)
                {
                    if (required && string.IsNullOrWhiteSpace(valueToValidate))
                    {
                        errorMessageStringBuilder.Append("Parameter \"").Append(parameterData.DisplayName).AppendLine("\" is required.");
                    }
                    else
                    {
                        paramsAndValues.Add((parameterData.InternalName, valueToValidate, parameterData.Type));
                    }
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
                var paramsToClear = new List<Parameter>();
                foreach (var param in EffectToSave.Params)
                {
                    if(!paramsToClear.Contains(param) && paramsAndValues.Any(pav => pav.ParamName.Equals(param.ParamName) && pav.Type == ParameterType.Table))
                    {
                        paramsToClear.Add(param);
                    }
                }
                EffectToSave.Params = EffectToSave.Params.Except(paramsToClear).ToArray();
                foreach (var (ParamName, Value, Type) in paramsAndValues)
                {
                    var paramToSave = EffectToSave.Params.FirstOrDefault(p => p.ParamName.Equals(ParamName, StringComparison.InvariantCultureIgnoreCase));
                    if (Type != ParameterType.Table && paramToSave != null)      // This should always be true if not a list
                    {
                        paramToSave.Value = Value;
                    }
                    else
                    {
                        EffectToSave.Params =
                        [
                            .. EffectToSave.Params,
                            new Parameter
                            {
                                ParamName = ParamName,
                                Value = Value
                            },
                        ];
                    }
                }
                Saved = true;
                this.Close();
            }
        }

        private Control GetAppropriateControl(EffectParameter parameterData, int row)
        {
            if (parameterData.Type == ParameterType.Character || parameterData.Type == ParameterType.Color || parameterData.Type == ParameterType.Text)
                return tlpParameters.GetControlFromPosition(1, row).Controls[0];
            else if (parameterData.Type == ParameterType.Table)
                return tlpParameters.GetControlFromPosition(0, row).Controls[1];
            return tlpParameters.GetControlFromPosition(1, row);
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

using RogueCustomsDungeonValidator.Utils;
using RogueCustomsDungeonValidator.Validators;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RogueCustomsDungeonValidator
{
    public partial class frmMain : Form
    {
        private DungeonInfo parsedJson;
        private Dungeon dungeonForLocale;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtDungeonPath_TextChanged(object sender, EventArgs e)
        {
            btnValidateDungeon.Enabled = !string.IsNullOrWhiteSpace(txtDungeonPath.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofdDungeon.ShowDialog() == DialogResult.OK)
            {
                txtDungeonPath.Text = ofdDungeon.FileName;
            }
        }

        private void btnValidateDungeon_Click(object sender, EventArgs e)
        {
            try
            {
                var jsonString = File.ReadAllText(txtDungeonPath.Text);
                parsedJson = JsonSerializer.Deserialize<DungeonInfo>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                var dungeonValidator = new DungeonValidator(parsedJson);
                var isGoodDungeon = dungeonValidator.Validate();

                lblNoAnalysis.Visible = false;
                tvResults.Visible = true;
                tvResults.Nodes.Clear();
                tvResults.Font = new Font("Arial", 11, FontStyle.Regular);

                AddNodesForValidation("Name", dungeonValidator.NameValidationMessages);
                AddNodesForValidation("Author", dungeonValidator.AuthorValidationMessages);
                AddNodesForValidation("Welcome/Ending Message", dungeonValidator.MessageValidationMessages);
                AddNodesForValidation("Default Locale", dungeonValidator.DefaultLocaleValidationMessages);
                AddNodesForValidation("General Floor Plan", dungeonValidator.FloorPlanValidationMessages);

                AddNodesForValidation("Object Ids", dungeonValidator.IdValidationMessages);

                foreach (var (FloorMinimumLevel, FloorMaximumLevel, ValidationMessages) in dungeonValidator.FloorTypeValidationMessages)
                {
                    AddNodesForValidation($"Floor Type for Levels {FloorMinimumLevel}-{FloorMaximumLevel}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.FactionValidationMessages)
                {
                    AddNodesForValidation($"Faction {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.CharacterValidationMessages)
                {
                    AddNodesForValidation($"Character {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.ItemValidationMessages)
                {
                    AddNodesForValidation($"Item {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.TrapValidationMessages)
                {
                    AddNodesForValidation($"Trap {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.AlteredStatusValidationMessages)
                {
                    AddNodesForValidation($"Altered Status {Id}", ValidationMessages);
                }

                foreach (var (Language, ValidationMessages) in dungeonValidator.LocaleStringValidationMessages)
                {
                    AddNodesForValidation($"Locale {Language}", ValidationMessages);
                }

                if (isGoodDungeon)
                    MessageBox.Show($"{Path.GetFileName(txtDungeonPath.Text)} is a valid dungeon file. You can safely play with it.\nCheck validation results for more info.", "Rogue Customs Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"{Path.GetFileName(txtDungeonPath.Text)} is NOT a valid dungeon file. Please fix it.\nCheck validation results for more info.", "Rogue Customs Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Attempting to parse {Path.GetFileName(txtDungeonPath.Text)} threw an error:\n\n{ex.Message}\n\nPlease check if the file is valid.", "Rogue Customs Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblNoAnalysis.Visible = true;
                tvResults.Visible = false;
            }
        }

        private void AddNodesForValidation(string rootNodeName, DungeonValidationMessages validationMessages)
        {
            var nameNode = new TreeNode(rootNodeName)
            {
                NodeFont = new Font("Arial", 10, FontStyle.Bold)
            };
            if (validationMessages.HasErrors)
            {
                nameNode.ForeColor = Color.Red;
            }
            else if (validationMessages.HasWarnings)
            {
                nameNode.ForeColor = Color.DarkOrange;
            }
            else
            {
                nameNode.ForeColor = Color.Green;
            }

            foreach (var validationMessage in validationMessages.ValidationMessages)
            {
                var childNode = new TreeNode(validationMessage.Message)
                {
                    NodeFont = new Font("Arial", 10, FontStyle.Regular)
                };
                if (validationMessage.Type == DungeonValidationMessageType.Error)
                {
                    childNode.ForeColor = Color.Red;
                }
                else if (validationMessage.Type == DungeonValidationMessageType.Warning)
                {
                    childNode.ForeColor = Color.DarkOrange;
                }
                else if (validationMessage.Type == DungeonValidationMessageType.Success)
                {
                    childNode.ForeColor = Color.Green;
                }
                nameNode.Nodes.Add(childNode);
            }

            tvResults.Nodes.Add(nameNode);
        }
    }
}

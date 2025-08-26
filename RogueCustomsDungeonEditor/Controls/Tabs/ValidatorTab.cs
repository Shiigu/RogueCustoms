using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Validators;

using RogueCustomsGameEngine.Utils.JsonImports;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ValidatorTab : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PassedValidation { get; private set; }
        public event EventHandler OnError;
        public event EventHandler OnValidationComplete;

        public ValidatorTab()
        {
            InitializeComponent();
        }

        public async Task ValidateDungeon(DungeonInfo dungeonToValidate, List<string> MandatoryLocaleKeys)
        {
            try
            {
                PassedValidation = false;
                tvValidationResults.Nodes.Clear();
                var dungeonValidator = new DungeonValidator(dungeonToValidate);
                PassedValidation = await dungeonValidator.Validate(MandatoryLocaleKeys, tsslValidationProgress, tspbValidationProgress);

                tvValidationResults.Visible = true;
                tvValidationResults.Font = new Font("Arial", 11, FontStyle.Regular);

                AddValidationResultNode("Name", dungeonValidator.NameValidationMessages);
                AddValidationResultNode("Author", dungeonValidator.AuthorValidationMessages);
                AddValidationResultNode("Welcome/Ending Message", dungeonValidator.MessageValidationMessages);
                AddValidationResultNode("Default Locale", dungeonValidator.DefaultLocaleValidationMessages);
                AddValidationResultNode("General Floor Plan", dungeonValidator.FloorPlanValidationMessages);

                AddValidationResultNode("Object Ids", dungeonValidator.IdValidationMessages);

                foreach (var (Language, ValidationMessages) in dungeonValidator.LocaleStringValidationMessages)
                {
                    AddValidationResultNode($"Locale {Language}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.TileTypeValidationMessages)
                {
                    AddValidationResultNode($"Tile Type {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.TileSetValidationMessages)
                {
                    AddValidationResultNode($"Tileset {Id}", ValidationMessages);
                }

                foreach (var (FloorMinimumLevel, FloorMaximumLevel, ValidationMessages) in dungeonValidator.FloorGroupValidationMessages)
                {
                    if (FloorMinimumLevel != FloorMaximumLevel)
                        AddValidationResultNode($"Floor Group for Levels {FloorMinimumLevel}-{FloorMaximumLevel}", ValidationMessages);
                    else
                        AddValidationResultNode($"Floor Group for Level {FloorMinimumLevel}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.FactionValidationMessages)
                {
                    AddValidationResultNode($"Faction {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.StatValidationMessages)
                {
                    AddValidationResultNode($"Stat {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.ElementValidationMessages)
                {
                    AddValidationResultNode($"Element {Id}", ValidationMessages);
                }

                AddValidationResultNode("Action Schools", dungeonValidator.ActionSchoolValidationMessages);

                foreach (var (Id, ValidationMessages) in dungeonValidator.PlayerClassValidationMessages)
                {
                    AddValidationResultNode($"Player Class {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.NPCValidationMessages)
                {
                    AddValidationResultNode($"NPC {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.ItemValidationMessages)
                {
                    AddValidationResultNode($"Item {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.TrapValidationMessages)
                {
                    AddValidationResultNode($"Trap {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.AlteredStatusValidationMessages)
                {
                    AddValidationResultNode($"Altered Status {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.ScriptValidationMessages)
                {
                    AddValidationResultNode($"Script {Id}", ValidationMessages);
                }

                if (PassedValidation)
                    MessageBox.Show($"This Dungeon has passed Validation. You can save and play with it, assured no known game-breaking bugs will happen.\nCheck Validation Results for more info.", "Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"This Dungeon has failed Validation. Please fix it, as playing with it can cause known game-breaking bugs to happen.\nCheck Validation Results for more info.", "Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Error);

                OnValidationComplete?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Attempting to Validate this Dungeon threw an error:\n\n{ex.Message}\n\nPlease fix it.", "Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PassedValidation = true;
                OnError?.Invoke(null, EventArgs.Empty);
            }
        }

        private void AddValidationResultNode(string rootNodeName, DungeonValidationMessages validationMessages)
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

            tvValidationResults.Nodes.Add(nameNode);
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma

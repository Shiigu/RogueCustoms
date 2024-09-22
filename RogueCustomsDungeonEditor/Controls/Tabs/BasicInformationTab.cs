using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class BasicInformationTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        public event EventHandler TabInfoChanged;
        public BasicInformationTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeonInfoToLoad)
        {
            ActiveDungeon = dungeonInfoToLoad;
            txtDungeonName.Text = ActiveDungeon.Name;
            txtAuthor.Text = ActiveDungeon.Author;
            txtWelcomeMessage.Text = ActiveDungeon.WelcomeMessage;
            txtEndingMessage.Text = ActiveDungeon.EndingMessage;
            cmbDefaultLocale.Items.Clear();
            foreach (var locale in ActiveDungeon.Locales)
            {
                cmbDefaultLocale.Items.Add(locale.Language);
            }
            cmbDefaultLocale.Text = ActiveDungeon.DefaultLocale;
        }

        private void txtDungeonName_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            txtDungeonName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblDungeonNameLocale);
        }

        private void txtAuthor_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            txtAuthor.ToggleEntryInLocaleWarning(ActiveDungeon, fklblAuthorLocale);
        }

        private void txtWelcomeMessage_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            txtWelcomeMessage.ToggleEntryInLocaleWarning(ActiveDungeon, fklblWelcomeMessageLocale);
        }

        private void txtEndingMessage_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
            txtEndingMessage.ToggleEntryInLocaleWarning(ActiveDungeon, fklblEndingMessageLocale);
        }

        private void cmbDefaultLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            if (string.IsNullOrWhiteSpace(txtDungeonName.Text))
                validationErrors.Add("Dungeon Name is empty.");
            else                
                ActiveDungeon.Name = txtDungeonName.Text;
            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
                validationErrors.Add("Author Name is empty.");
            else
                ActiveDungeon.Author = txtAuthor.Text;
            if (string.IsNullOrWhiteSpace(txtWelcomeMessage.Text))
                validationErrors.Add("Welcome Message is empty.");
            else
                ActiveDungeon.WelcomeMessage = txtWelcomeMessage.Text;
            if (string.IsNullOrWhiteSpace(txtEndingMessage.Text))
                validationErrors.Add("Ending Message is empty.");
            else
                ActiveDungeon.EndingMessage = txtEndingMessage.Text;
            if (string.IsNullOrWhiteSpace(cmbDefaultLocale.Text))
                validationErrors.Add("Default Locale is empty.");
            else
                ActiveDungeon.DefaultLocale = cmbDefaultLocale.Text;
            return validationErrors;
        }
    }
}

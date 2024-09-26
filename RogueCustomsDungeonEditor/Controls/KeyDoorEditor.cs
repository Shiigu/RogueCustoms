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
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class KeyDoorEditor : UserControl
    {
        public DungeonInfo ActiveDungeon;
        public string KeyTypeName
        {
            get
            {
                return txtKeyTypeName.Text;
            }
            set
            {
                txtKeyTypeName.Text = value;
            }
        }

        public bool CanLockStairs
        {
            get
            {
                return chkCanLockStairs.Checked;
            }
            set
            {
                chkCanLockStairs.Checked = value;
            }
        }

        public bool CanLockItems
        {
            get
            {
                return chkCanLockItems.Checked;
            }
            set
            {
                chkCanLockItems.Checked = value;
            }
        }

        public ConsoleRepresentation KeyConsoleRepresentation
        {
            get
            {
                return crsKey.ConsoleRepresentation;
            }
            set
            {
                crsKey.SetConsoleRepresentation(value);
            }
        }

        public ConsoleRepresentation DoorConsoleRepresentation
        {
            get
            {
                return crsDoor.ConsoleRepresentation;
            }
            set
            {
                crsDoor.SetConsoleRepresentation(value);
            }
        }

        public KeyTypeInfo KeyDoorType
        { 
            get
            {
                return new()
                {
                    KeyTypeName = txtKeyTypeName.Text,
                    CanLockStairs = chkCanLockStairs.Checked,
                    CanLockItems = chkCanLockItems.Checked,
                    KeyConsoleRepresentation = crsKey.ConsoleRepresentation,
                    DoorConsoleRepresentation = crsDoor.ConsoleRepresentation
                };
            }
        }

        public bool IsSelected { get; private set; }

        public KeyDoorEditor()
        {
            InitializeComponent();
        }

        private void txtKeyTypeName_TextChanged(object sender, EventArgs e)
        {
            if (ActiveDungeon == null) return;
            txtKeyTypeName.ToggleConcatenatedEntryInLocaleWarning("KeyType", string.Empty,ActiveDungeon, fklblKeyDoorNameLocale);
            if(!fklblKeyDoorNameLocale.Visible)
                txtKeyTypeName.ToggleConcatenatedEntryInLocaleWarning("DoorType", string.Empty, ActiveDungeon, fklblKeyDoorNameLocale);
        }

        public void ToggleSelection()
        {
            IsSelected = !IsSelected;
            if (IsSelected)
                BackColor = SystemColors.Highlight;
            else
                BackColor = SystemColors.Control;
        }

    }
}

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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

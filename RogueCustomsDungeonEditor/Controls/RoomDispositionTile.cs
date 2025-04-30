using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;

using RogueCustomsDungeonEditor.FloorInfos;

using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class RoomDispositionTile : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<RoomDispositionData> RoomTypes { get; set; }
        private List<RoomDispositionData> PossibleRoomTypes;

        private RoomDispositionType _roomDispositionType;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RoomDispositionType RoomDispositionType
        {
            get
            {
                return _roomDispositionType;
            }
            set
            {
                var roomDispositionData = RoomTypes.FirstOrDefault(rdt => rdt.InternalName.Equals(value.ToString()));
                if (roomDispositionData != null)
                {
                    _roomDispositionType = value;
                    _selectedIndex = PossibleRoomTypes.IndexOf(roomDispositionData);
                    pcbTile.Image = roomDispositionData.TileImage;
                }
                else
                {
                    if (_x % 2 == 0 && _y % 2 == 0)
                        _roomDispositionType = RoomDispositionType.NoRoom;
                    else if ((_x % 2 != 0 && _y % 2 == 0) || (_x % 2 == 0 && _y % 2 != 0))
                        _roomDispositionType = RoomDispositionType.NoConnection;
                    else
                        _roomDispositionType = RoomDispositionType.ConnectionImpossible;
                    roomDispositionData = RoomTypes.FirstOrDefault(rdt => rdt.InternalName.Equals(_roomDispositionType.ToString()));
                    if (roomDispositionData != null)
                    {
                        pcbTile.Image = roomDispositionData.TileImage;
                        _selectedIndex = PossibleRoomTypes.IndexOf(roomDispositionData);
                    }
                    else
                    {
                        pcbTile.Image = null;
                        _selectedIndex = -1;
                    }
                }
                RoomTileTypeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string? RoomDispositionIndicatorChar
        {
            get
            {
                var roomDispositionData = RoomTypes.FirstOrDefault(rdt => rdt.InternalName.Equals(RoomDispositionType.ToString()));
                if (roomDispositionData != null)
                    return roomDispositionData.RoomDispositionIndicatorChar;
                return null;
            }
        }

        private int _selectedIndex;
        private int _x = -1;
        private int _y = -1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                if (_x > -1 && _y > -1)
                    UpdateRoomTypeList();
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                if (_x > -1 && _y > -1)
                    UpdateRoomTypeList();
            }
        }


        public event EventHandler RoomTileTypeChanged;

        public RoomDispositionTile()
        {
            InitializeComponent();
            pcbTile.ContextMenuStrip = cmsRoomTypes;
        }

        private void UpdateRoomTypeList()
        {
            if (_x % 2 == 0 && _y % 2 == 0)
                PossibleRoomTypes = RoomTypes.Where(rt => rt.DispositionItemType == RoomDispositionItemType.Room).ToList();
            else if ((_x % 2 != 0 && _y % 2 == 0) || (_x % 2 == 0 && _y % 2 != 0))
                PossibleRoomTypes = RoomTypes.Where(rt => rt.DispositionItemType == RoomDispositionItemType.Connection && rt.RoomDispositionIndicator != RoomDispositionType.ConnectionImpossible).ToList();
            else
                PossibleRoomTypes = RoomTypes.Where(rt => rt.RoomDispositionIndicator == RoomDispositionType.ConnectionImpossible).ToList();
            cmsRoomTypes.Items.Clear();
            foreach (var roomType in PossibleRoomTypes)
            {
                var roomButton = new ToolStripMenuItem(roomType.DisplayName)
                {
                    Enabled = roomType.RoomDispositionIndicator != RoomDispositionType.ConnectionImpossible,
                    CheckOnClick = false,
                    Image = roomType.TileImage
                };
                if (roomButton.Enabled)
                {
                    roomButton.Click += (sender, e) =>
                    {
                        RoomDispositionType = roomType.RoomDispositionIndicator;
                    };
                }
                else
                {
                    pcbTile.Image = roomType.TileImage;
                }
                cmsRoomTypes.Items.Add(roomButton);
            }
        }

        private void pcbTile_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs.Button == MouseButtons.Right)
            {
                cmsRoomTypes.Show(pcbTile, mouseEventArgs.Location);
                return;
            }
            _selectedIndex++;
            RoomDispositionType = PossibleRoomTypes[_selectedIndex % PossibleRoomTypes.Count].RoomDispositionIndicator;
        }

        private void pcbTile_MouseHover(object sender, EventArgs e)
        {
            var roomDispositionData = RoomTypes.FirstOrDefault(rdt => rdt.InternalName.Equals(RoomDispositionType.ToString()));
            if (roomDispositionData == null)
                return;
            ttTile.Show($"({X}, {Y})\n\n{roomDispositionData.DisplayName}", pcbTile);
        }
    }
}

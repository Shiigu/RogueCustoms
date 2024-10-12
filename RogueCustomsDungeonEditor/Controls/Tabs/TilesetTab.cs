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

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class TilesetTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        public TileSetInfo LoadedTileSet { get; private set; }
        public event EventHandler TabInfoChanged;
        public TilesetTab()
        {
            InitializeComponent();
        }

        public void LoadData(TileSetInfo tilesetInfoToLoad, DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedTileSet = tilesetInfoToLoad;
            flpTileTypeSets.Controls.Clear();
            foreach (var tileTypeSet in tilesetInfoToLoad.TileTypes)
            {
                var correspondingTileType = ActiveDungeon.TileTypeInfos.Find(tti => tti.Id.Equals(tileTypeSet.TileTypeId));
                if (correspondingTileType != null)
                {
                    AddTileTypeSetEditor(tileTypeSet, correspondingTileType);
                }
            }
        }

        private void AddTileTypeSetEditor(TileTypeSetInfo tileTypeSet, TileTypeInfo tileType)
        {
            var tileTypeSetEditor = new TileTypeSetEditor();

            tileTypeSetEditor.LoadData(tileTypeSet, tileType);
            tileTypeSetEditor.TabInfoChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);

            flpTileTypeSets.FlowDirection = FlowDirection.TopDown;
            flpTileTypeSets.WrapContents = false;  // Ensures controls don't wrap
            flpTileTypeSets.AutoScroll = true;
            flpTileTypeSets.Controls.Add(tileTypeSetEditor);
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            var tileTypeSetsToAdd = new List<TileTypeSetInfo>();

            foreach (TileTypeSetEditor tileTypeSetEditor in flpTileTypeSets.Controls)
            {
                validationErrors.AddRange(tileTypeSetEditor.ValidateTiles());
                tileTypeSetsToAdd.Add(tileTypeSetEditor.TileTypeSetInfo);
            }

            if (!validationErrors.Any())
            {
                LoadedTileSet = new TileSetInfo
                {
                    Id = id,
                    TileTypes = tileTypeSetsToAdd
                };
            }

            return validationErrors;
        }

        private void csrTopLeftWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrTopRightWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrVerticalWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrBottomLeftWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrBottomRightWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrHorizontalWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrConnectorWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrTopLeftHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrTopRightHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrVerticalHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrBottomLeftHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrBottomRightHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrHorizontalHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrHorizontalBottomHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrHorizontalTopHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrVerticalLeftHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrVerticalRightHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrCentralHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrFloor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrStairs_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void csrEmpty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

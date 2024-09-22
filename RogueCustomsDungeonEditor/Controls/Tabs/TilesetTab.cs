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

        public void LoadData(TileSetInfo tilesetInfoToLoad)
        {
            LoadedTileSet = tilesetInfoToLoad;
            csrTopLeftWall.SetConsoleRepresentation(tilesetInfoToLoad.TopLeftWall);
            csrTopRightWall.SetConsoleRepresentation(tilesetInfoToLoad.TopRightWall);
            csrBottomLeftWall.SetConsoleRepresentation(tilesetInfoToLoad.BottomLeftWall);
            csrBottomRightWall.SetConsoleRepresentation(tilesetInfoToLoad.BottomRightWall);
            csrHorizontalWall.SetConsoleRepresentation(tilesetInfoToLoad.HorizontalWall);
            csrConnectorWall.SetConsoleRepresentation(tilesetInfoToLoad.ConnectorWall);
            csrVerticalWall.SetConsoleRepresentation(tilesetInfoToLoad.VerticalWall);
            csrTopLeftHallway.SetConsoleRepresentation(tilesetInfoToLoad.TopLeftHallway);
            csrTopRightHallway.SetConsoleRepresentation(tilesetInfoToLoad.TopRightHallway);
            csrBottomLeftHallway.SetConsoleRepresentation(tilesetInfoToLoad.BottomLeftHallway);
            csrBottomRightHallway.SetConsoleRepresentation(tilesetInfoToLoad.BottomRightHallway);
            csrHorizontalHallway.SetConsoleRepresentation(tilesetInfoToLoad.HorizontalHallway);
            csrHorizontalBottomHallway.SetConsoleRepresentation(tilesetInfoToLoad.HorizontalBottomHallway);
            csrHorizontalTopHallway.SetConsoleRepresentation(tilesetInfoToLoad.HorizontalTopHallway);
            csrVerticalHallway.SetConsoleRepresentation(tilesetInfoToLoad.VerticalHallway);
            csrVerticalLeftHallway.SetConsoleRepresentation(tilesetInfoToLoad.VerticalLeftHallway);
            csrVerticalRightHallway.SetConsoleRepresentation(tilesetInfoToLoad.VerticalRightHallway);
            csrCentralHallway.SetConsoleRepresentation(tilesetInfoToLoad.CentralHallway);
            csrFloor.SetConsoleRepresentation(tilesetInfoToLoad.Floor);
            csrStairs.SetConsoleRepresentation(tilesetInfoToLoad.Stairs);
            csrEmpty.SetConsoleRepresentation(tilesetInfoToLoad.Empty);
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            validationErrors.AddRange(csrTopLeftWall.ConsoleRepresentation.Validate("Top Left Wall"));
            validationErrors.AddRange(csrTopRightWall.ConsoleRepresentation.Validate("Top Right Wall"));
            validationErrors.AddRange(csrBottomLeftWall.ConsoleRepresentation.Validate("Bottom Left Wall"));
            validationErrors.AddRange(csrBottomRightWall.ConsoleRepresentation.Validate("Bottom Right Wall"));
            validationErrors.AddRange(csrHorizontalWall.ConsoleRepresentation.Validate("Horizontal Wall"));
            validationErrors.AddRange(csrConnectorWall.ConsoleRepresentation.Validate("Connector Wall"));
            validationErrors.AddRange(csrVerticalWall.ConsoleRepresentation.Validate("Vertical Wall"));
            validationErrors.AddRange(csrTopLeftHallway.ConsoleRepresentation.Validate("Top Left Hallway"));
            validationErrors.AddRange(csrTopRightHallway.ConsoleRepresentation.Validate("Top Right Hallway"));
            validationErrors.AddRange(csrBottomLeftHallway.ConsoleRepresentation.Validate("Bottom Left Hallway"));
            validationErrors.AddRange(csrBottomRightHallway.ConsoleRepresentation.Validate("Bottom Right Hallway"));
            validationErrors.AddRange(csrHorizontalHallway.ConsoleRepresentation.Validate("Horizontal Hallway"));
            validationErrors.AddRange(csrHorizontalBottomHallway.ConsoleRepresentation.Validate("Horizontal Bottom Hallway"));
            validationErrors.AddRange(csrHorizontalTopHallway.ConsoleRepresentation.Validate("Horizontal Top Hallway"));
            validationErrors.AddRange(csrVerticalHallway.ConsoleRepresentation.Validate("Vertical Hallway"));
            validationErrors.AddRange(csrVerticalLeftHallway.ConsoleRepresentation.Validate("Vertical Left Hallway"));
            validationErrors.AddRange(csrVerticalRightHallway.ConsoleRepresentation.Validate("Vertical Right Hallway"));
            validationErrors.AddRange(csrCentralHallway.ConsoleRepresentation.Validate("Central Hallway"));
            validationErrors.AddRange(csrFloor.ConsoleRepresentation.Validate("Floor"));
            validationErrors.AddRange(csrStairs.ConsoleRepresentation.Validate("Stairs"));
            validationErrors.AddRange(csrEmpty.ConsoleRepresentation.Validate("Empty (inaccessible)"));

            if (!validationErrors.Any())
            {
                LoadedTileSet = new TileSetInfo
                {
                    Id = id,
                    TopLeftWall = csrTopLeftWall.ConsoleRepresentation,
                    TopRightWall = csrTopRightWall.ConsoleRepresentation,
                    BottomLeftWall = csrBottomLeftWall.ConsoleRepresentation,
                    BottomRightWall = csrBottomRightWall.ConsoleRepresentation,
                    HorizontalWall = csrHorizontalWall.ConsoleRepresentation,
                    ConnectorWall = csrConnectorWall.ConsoleRepresentation,
                    VerticalWall = csrVerticalWall.ConsoleRepresentation,
                    TopLeftHallway = csrTopLeftHallway.ConsoleRepresentation,
                    TopRightHallway = csrTopRightHallway.ConsoleRepresentation,
                    BottomLeftHallway = csrBottomLeftHallway.ConsoleRepresentation,
                    BottomRightHallway = csrBottomRightHallway.ConsoleRepresentation,
                    HorizontalHallway = csrHorizontalHallway.ConsoleRepresentation,
                    HorizontalBottomHallway = csrHorizontalBottomHallway.ConsoleRepresentation,
                    HorizontalTopHallway = csrHorizontalTopHallway.ConsoleRepresentation,
                    VerticalHallway = csrVerticalHallway.ConsoleRepresentation,
                    VerticalLeftHallway = csrVerticalLeftHallway.ConsoleRepresentation,
                    VerticalRightHallway = csrVerticalRightHallway.ConsoleRepresentation,
                    CentralHallway = csrCentralHallway.ConsoleRepresentation,
                    Floor = csrFloor.ConsoleRepresentation,
                    Stairs = csrStairs.ConsoleRepresentation,
                    Empty = csrEmpty.ConsoleRepresentation
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class TileTypeSetEditor : UserControl
    {
        public event EventHandler TabInfoChanged;
        private TileTypeInfo _tileType;

        public TileTypeSetInfo TileTypeSetInfo
        {
            get
            {
                return new()
                {
                    TileTypeId = _tileType.Id,
                    Central = csrCentral?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    TopLeft = csrTopLeft?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    TopRight = csrTopRight?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    BottomLeft = csrBottomLeft?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    BottomRight = csrBottomRight?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    Horizontal = csrHorizontal?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    Vertical = csrVertical?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    Connector = csrConnector?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    HorizontalBottom = csrHorizontalBottom?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    HorizontalTop = csrHorizontalTop?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    VerticalLeft = csrVerticalLeft?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                    VerticalRight = csrVerticalRight?.ConsoleRepresentation ?? new() { BackgroundColor = new GameColor(Color.Black), ForegroundColor = new GameColor(Color.Black), Character = ' ' },
                };
            }
        }

        public TileTypeSetEditor()
        {
            InitializeComponent();
        }

        public void LoadData(TileTypeSetInfo tileTypeSetInfoToLoad, TileTypeInfo tileType)
        {
            _tileType = tileType;
            lblName.Text = tileType.Id.ToUpperInvariant();
            csrCentral.SetConsoleRepresentation(tileTypeSetInfoToLoad.Central);
            if (tileType.CanVisiblyConnectWithOtherTiles)
            {
                csrTopLeft.SetConsoleRepresentation(tileTypeSetInfoToLoad.TopLeft);
                csrTopRight.SetConsoleRepresentation(tileTypeSetInfoToLoad.TopRight);
                csrBottomLeft.SetConsoleRepresentation(tileTypeSetInfoToLoad.BottomLeft);
                csrBottomRight.SetConsoleRepresentation(tileTypeSetInfoToLoad.BottomRight);
                csrHorizontal.SetConsoleRepresentation(tileTypeSetInfoToLoad.Horizontal);
                csrVertical.SetConsoleRepresentation(tileTypeSetInfoToLoad.Vertical);
            }
            else
            {
                csrTopLeft.Dispose();
                lblTopLeftHeader.Dispose();
                csrTopRight.Dispose();
                lblTopRightHeader.Dispose();
                csrBottomLeft.Dispose();
                lblBottomLeftHeader.Dispose();
                csrBottomRight.Dispose();
                lblBottomRightHeader.Dispose();
                csrHorizontal.Dispose();
                lblHorizontalHeader.Dispose();
                csrVertical.Dispose();
                lblVerticalHeader.Dispose();
            }
            if (tileType.Id.Equals("hallway", StringComparison.InvariantCultureIgnoreCase))
            {
                csrConnector.SetConsoleRepresentation(tileTypeSetInfoToLoad.Connector);
            }
            else
            {
                csrConnector.Dispose();
                lblConnectorHeader.Dispose();
                csrCentral.Left = 250;
                lblCentralHeader.Left = 250;
            }
            if (tileType.CanHaveMultilineConnections)
            {
                csrHorizontalBottom.SetConsoleRepresentation(tileTypeSetInfoToLoad.HorizontalBottom);
                csrHorizontalTop.SetConsoleRepresentation(tileTypeSetInfoToLoad.HorizontalTop);
                csrVerticalLeft.SetConsoleRepresentation(tileTypeSetInfoToLoad.VerticalLeft);
                csrVerticalRight.SetConsoleRepresentation(tileTypeSetInfoToLoad.VerticalRight);
            }
            else
            {
                csrHorizontalBottom.Dispose();
                lblHorizontalBottomHeader.Dispose();
                csrHorizontalTop.Dispose();
                lblHorizontalTopHeader.Dispose();
                csrVerticalLeft.Dispose();
                lblVerticalLeftHeader.Dispose();
                csrVerticalRight.Dispose();
                lblVerticalRightHeader.Dispose();
            }
        }

        public List<string> ValidateTiles()
        {
            var validationErrors = new List<string>();

            validationErrors.AddRange(csrCentral.ConsoleRepresentation.Validate($"{_tileType.Id}'s Central"));
            if (_tileType.CanVisiblyConnectWithOtherTiles)
            {
                validationErrors.AddRange(csrTopLeft.ConsoleRepresentation.Validate($"{_tileType.Id}'s Top Left"));
                validationErrors.AddRange(csrTopRight.ConsoleRepresentation.Validate($"{_tileType.Id}'s Top Right"));
                validationErrors.AddRange(csrBottomLeft.ConsoleRepresentation.Validate($"{_tileType.Id}'s Bottom Left"));
                validationErrors.AddRange(csrBottomRight.ConsoleRepresentation.Validate($"{_tileType.Id}'s Bottom Right"));
                validationErrors.AddRange(csrHorizontal.ConsoleRepresentation.Validate($"{_tileType.Id}'s Horizontal"));
                validationErrors.AddRange(csrVertical.ConsoleRepresentation.Validate($"{_tileType.Id}'s Vertical"));
            }
            if (_tileType.Id.Equals("hallway", StringComparison.InvariantCultureIgnoreCase))
            {
                validationErrors.AddRange(csrConnector.ConsoleRepresentation.Validate($"{_tileType.Id}'s Connector"));
            }
            if (_tileType.CanVisiblyConnectWithOtherTiles && _tileType.CanHaveMultilineConnections)
            {
                validationErrors.AddRange(csrHorizontalBottom.ConsoleRepresentation.Validate($"{_tileType.Id}'s Horizontal Bottom"));
                validationErrors.AddRange(csrHorizontalTop.ConsoleRepresentation.Validate($"{_tileType.Id}'s Horizontal Top"));
                validationErrors.AddRange(csrVerticalLeft.ConsoleRepresentation.Validate($"{_tileType.Id}'s Vertical Left"));
                validationErrors.AddRange(csrVerticalRight.ConsoleRepresentation.Validate($"{_tileType.Id}'s Vertical Right"));
            }

            return validationErrors;
        }

        private void csrCentral_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrConnector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrTopLeft_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrTopRight_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrVertical_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrBottomLeft_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrBottomRight_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrHorizontal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrHorizontalBottom_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrHorizontalTop_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrVerticalRight_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void csrVerticalLeft_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}

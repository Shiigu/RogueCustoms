using org.matheval.Node;

using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.FloorInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class frmFloorLayout : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FloorLayoutGenerationInfo GeneratorToSave { get; private set; }

        private readonly List<FloorLayoutGenerationInfo> CurrentGenerators;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Saved { get; private set; }
        private readonly List<RoomDispositionData> RoomTypeTiles;
        private readonly int FloorWidth, FloorHeight;
        private readonly int MaxColumns, MaxRows;
        private int Columns, Rows;
        private readonly List<string> RoomDispositionTemplates = new() { "Fully Random", "Outer Ring of Dummy Rooms", "Inner Ring of Dummy Rooms", "A Single Room" };

        public frmFloorLayout(List<FloorLayoutGenerationInfo> currentGenerators, int width, int height, int minFloorLevel, int maxFloorLevel, List<RoomDispositionData> roomTypeTiles, FloorLayoutGenerationInfo generatorToSave)
        {
            InitializeComponent();
            RoomTypeTiles = roomTypeTiles;
            if (minFloorLevel != maxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";
            FloorWidth = width;
            FloorHeight = height;
            MaxColumns = FloorWidth / 7;
            MaxRows = FloorHeight / 7;
            CurrentGenerators = currentGenerators;
            GeneratorToSave = generatorToSave ?? new FloorLayoutGenerationInfo
            {
                Columns = MaxColumns,
                Rows = MaxRows,
                RoomDisposition = string.Empty
            };
            nudMinWidth.Value = GeneratorToSave.MinRoomSize != null ? GeneratorToSave.MinRoomSize.Width : 5;
            nudMinHeight.Value = GeneratorToSave.MinRoomSize != null ? GeneratorToSave.MinRoomSize.Height : 5;
            nudMaxWidth.Value = GeneratorToSave.MaxRoomSize != null ? GeneratorToSave.MaxRoomSize.Width : width / MaxColumns;
            nudMaxHeight.Value = GeneratorToSave.MaxRoomSize != null ? GeneratorToSave.MaxRoomSize.Height : height / MaxRows;
            ConstructTable();
        }

        private void ConstructTable()
        {
            (int Width, int Height) initialGridDimensions = (tlpRoomDisposition.Width, tlpRoomDisposition.Height);
            tlpRoomDisposition.Controls.Clear();
            var rows = MaxRows + MaxRows - 1;
            var columns = MaxColumns + MaxColumns - 1;
            var generatorRows = GeneratorToSave.Rows + GeneratorToSave.Rows - 1;
            var generatorColumns = GeneratorToSave.Columns + GeneratorToSave.Columns - 1;
            tlpRoomDisposition.Width = 24 * columns;
            tlpRoomDisposition.Height = 24 * rows;
            tlpRoomDisposition.ColumnCount = columns;
            tlpRoomDisposition.RowCount = rows;
            foreach (ColumnStyle columnStyle in tlpRoomDisposition.ColumnStyles)
            {
                columnStyle.SizeType = SizeType.Absolute;
                columnStyle.Width = 24;
            }
            foreach (RowStyle rowStyle in tlpRoomDisposition.RowStyles)
            {
                rowStyle.SizeType = SizeType.Absolute;
                rowStyle.Height = 24;
            }
            var tileIndexToDraw = 0;
            for (int i = 0; i < rows * columns; i++)
            {
                var isUnusedTile = false;
                var tile = GeneratorToSave.RoomDisposition.ElementAtOrDefault(tileIndexToDraw);
                (int tableX, int tableY) = (i / columns, i % columns);
                (int generatorX, int generatorY) = (tileIndexToDraw / generatorColumns, tileIndexToDraw % generatorColumns);
                if (tableX > generatorX || tableY > generatorY)
                {
                    tile = '-';
                    isUnusedTile = true;
                }
                else
                {
                    tileIndexToDraw++;
                }
                (int X, int Y) = isUnusedTile ? (tableX, tableY) : (generatorX, generatorY);
                var roomDispositionTile = new RoomDispositionTile
                {
                    RoomTypes = RoomTypeTiles,
                    X = X,
                    Y = Y
                };
                var isHallwayTile = (X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0);
                if (X % 2 != 0 && Y % 2 != 0)
                    roomDispositionTile.RoomDispositionType = RoomDispositionType.ConnectionImpossible;
                else
                    roomDispositionTile.RoomDispositionType = tile.ToRoomDispositionIndicator(isHallwayTile);
                tlpRoomDisposition.Controls.Add(roomDispositionTile, Y, X);
                roomDispositionTile.RoomTileTypeChanged += RoomDispositionTile_RoomTileTypeChanged;
            }
            RoomDispositionTile_RoomTileTypeChanged(null, EventArgs.Empty);

            if (tlpRoomDisposition.Width > initialGridDimensions.Width)
            {
                this.Width += tlpRoomDisposition.Width - initialGridDimensions.Width;
                lblFloorGroupTitle.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label1.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label2.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label3.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label4.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                nudMinWidth.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                nudMinHeight.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label5.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label6.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label7.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label8.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                nudMaxWidth.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                nudMaxHeight.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label9.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                label10.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                lblFloorSize.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                cmbFloorTemplate.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                btnCancel.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                btnSave.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                btnSetToMinimumSize.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
                btnSetToMaximumSize.Left += (tlpRoomDisposition.Width - initialGridDimensions.Width) / 2;
            }
            else
            {
                tlpRoomDisposition.Left += (initialGridDimensions.Width - tlpRoomDisposition.Width) / 2;
            }
            if (tlpRoomDisposition.Height > initialGridDimensions.Height)
            {
                this.Height += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label1.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label2.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label3.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label4.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                nudMinWidth.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                nudMinHeight.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label5.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label6.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label7.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label8.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                nudMaxWidth.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                nudMaxHeight.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label9.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                label10.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                lblFloorSize.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                cmbFloorTemplate.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                btnCancel.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                btnSave.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                btnSetToMinimumSize.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
                btnSetToMaximumSize.Top += tlpRoomDisposition.Height - initialGridDimensions.Height;
            }
            else
            {
                tlpRoomDisposition.Top += (initialGridDimensions.Height - tlpRoomDisposition.Height) / 2;
            }

            foreach (var template in RoomDispositionTemplates)
            {
                cmbFloorTemplate.Items.Add(template);
            }
        }

        private void RoomDispositionTile_RoomTileTypeChanged(object? sender, EventArgs e)
        {
            var roomDispositionTiles = tlpRoomDisposition.Controls.Cast<RoomDispositionTile>().ToList();
            var roomTypesToNotCount = new List<RoomDispositionType> { RoomDispositionType.NoRoom, RoomDispositionType.NoConnection, RoomDispositionType.ConnectionImpossible };
            var roomTilesToCount = roomDispositionTiles.Where(rdt => !roomTypesToNotCount.Contains(rdt.RoomDispositionType));
            if (roomTilesToCount.Any())
            {
                var minX = roomTilesToCount.Select(rdt => rdt.X).Min();
                var minY = roomTilesToCount.Select(rdt => rdt.Y).Min();
                var maxX = roomTilesToCount.Select(rdt => rdt.X).Max();
                var maxY = roomTilesToCount.Select(rdt => rdt.Y).Max();
                var totalRows = Math.Max(1, maxY - minY + 1);
                var totalColumns = Math.Max(1, maxX - minX + 1);
                Columns = (totalRows + 1) / 2;
                Rows = (totalColumns + 1) / 2;
                nudMinWidth.Enabled = nudMaxWidth.Enabled = (Rows == 1 && Columns == 1) || Columns < MaxColumns;
                nudMinHeight.Enabled = nudMaxHeight.Enabled = (Rows == 1 && Columns == 1) || Rows < MaxRows;
                nudMinWidth.Minimum = nudMaxWidth.Minimum = 5;
                nudMinWidth.Maximum = nudMaxWidth.Maximum = FloorWidth / Columns;
                nudMinHeight.Minimum = nudMaxHeight.Minimum = 5;
                nudMinHeight.Maximum = nudMaxHeight.Maximum = FloorHeight / Rows;
                if (!nudMinWidth.Enabled && !nudMaxWidth.Enabled)
                {
                    nudMinWidth.Value = nudMinWidth.Minimum;
                    nudMaxWidth.Value = nudMaxWidth.Maximum;
                }
                if (!nudMinHeight.Enabled && !nudMaxHeight.Enabled)
                {
                    nudMinHeight.Value = nudMinHeight.Minimum;
                    nudMaxHeight.Value = nudMaxHeight.Maximum;
                }
                btnSave.Enabled = true;
                btnSetToMinimumSize.Enabled = nudMinWidth.Enabled || nudMinHeight.Enabled;
                btnSetToMaximumSize.Enabled = nudMaxWidth.Enabled || nudMaxHeight.Enabled;
            }
            else
            {
                Rows = Columns = 0;
                nudMinWidth.Enabled = nudMaxWidth.Enabled = nudMinHeight.Enabled = nudMaxHeight.Enabled = false;
                nudMinWidth.Minimum = nudMinWidth.Maximum = 0;
                nudMaxWidth.Minimum = nudMaxWidth.Maximum = 0;
                nudMinHeight.Minimum = nudMinHeight.Maximum = 0;
                nudMaxHeight.Minimum = nudMaxHeight.Maximum = 0;
                btnSave.Enabled = btnSetToMaximumSize.Enabled = btnSetToMinimumSize.Enabled = false;
            }
            lblFloorSize.Text = $"Floor Size (Rows x Columns): {Rows}x{Columns}";
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var rowsPlusConnections = Rows * 2 - 1;
            var columnsPlusConnections = Columns * 2 - 1;
            var roomDispositionTiles = tlpRoomDisposition.Controls.Cast<RoomDispositionTile>().ToList();
            var roomTypesToNotCount = new List<RoomDispositionType> { RoomDispositionType.NoRoom, RoomDispositionType.NoConnection, RoomDispositionType.ConnectionImpossible };
            var roomTilesToCount = roomDispositionTiles.Where(rdt => !roomTypesToNotCount.Contains(rdt.RoomDispositionType));
            var guaranteedFuseTiles = roomDispositionTiles.Where(rdt => rdt.RoomDispositionType == RoomDispositionType.GuaranteedFusion);
            var normalRoomTiles = roomDispositionTiles.Where(rdt => rdt.RoomDispositionType == RoomDispositionType.GuaranteedRoom || rdt.RoomDispositionType == RoomDispositionType.RandomRoom);
            var guaranteedRoomTiles = roomDispositionTiles.Where(rdt => rdt.RoomDispositionType == RoomDispositionType.GuaranteedRoom);

            var validationErrors = new List<string>();

            if (!guaranteedRoomTiles.Any() && normalRoomTiles.Count() == 1)
            {
                validationErrors.Add("When making a Single-Room layout, the room must be guaranteed.");
            }
            if ((normalRoomTiles.Count() - guaranteedFuseTiles.Count()) <= 1 && roomTilesToCount.Count() > 1)
            {
                validationErrors.Add("When making a layout that is not Single-Room layout, at least two normal, non-fused rooms must be possible.");
            }
            if (Rows == 0 || Columns == 0)
            {
                validationErrors.Add("You cannot save an empty Floor Layout.");
            }
            else
            {
                var roomDispositionAdyacencyMatrix = new RoomDispositionType[rowsPlusConnections, columnsPlusConnections];
                foreach (RoomDispositionTile tile in tlpRoomDisposition.Controls)
                {
                    if (tile.X >= rowsPlusConnections || tile.Y >= columnsPlusConnections) continue;
                    roomDispositionAdyacencyMatrix[tile.X, tile.Y] = tile.RoomDispositionType;
                }
                if (!roomDispositionAdyacencyMatrix.IsFullyConnectedAdjacencyMatrix(rdt => !roomTypesToNotCount.Contains(rdt)))
                    validationErrors.Add("The Floor Layout is not fully disconnected.");
            }

            if (!RoomsHaveNoMoreThanOneFusion())
                validationErrors.Add("At least one Room is guaranteed more than one Fusion, which is not allowed.");
            if (!ConnectionsHaveBothEndsCovered())
                validationErrors.Add("At least one possible Connection is missing one of its ends.");

            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"You cannot save this Floor Layout:\n\n- {string.Join("\n - ", validationErrors)}",
                    "Save Floor Layout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = string.Empty;
            var defaultName = !string.IsNullOrWhiteSpace(GeneratorToSave.Name) ? GeneratorToSave.Name : $"New Layout - {Columns}c x {Rows}r";
            do
            {
                inputBoxResult = InputBox.Show("Give this Floor Layout a Name", "Save Floor Layout", defaultName);
                if (inputBoxResult != null)
                {
                    if (CurrentGenerators.Any(pl => pl != GeneratorToSave && pl.Name.Equals(inputBoxResult)))
                        MessageBox.Show(
                            $"The name you picked, {inputBoxResult}, already belongs to another Layout in this floor.\n\nPlease pick another.",
                            "Save Floor Layout",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    else
                        GeneratorToSave.Name = inputBoxResult;
                }
                else
                    return;
            }
            while (CurrentGenerators.Any(pl => pl != GeneratorToSave && pl.Name.Equals(inputBoxResult)));
            var roomDispositionSequence = new StringBuilder();
            foreach (RoomDispositionTile tile in tlpRoomDisposition.Controls)
            {
                if (tile.X >= rowsPlusConnections || tile.Y >= columnsPlusConnections) continue;
                roomDispositionSequence.Append(tile.RoomDispositionIndicatorChar);
            }
            GeneratorToSave.Rows = Rows;
            GeneratorToSave.Columns = Columns;
            GeneratorToSave.MinRoomSize = new() { Width = (int)nudMinWidth.Value, Height = (int)nudMinHeight.Value };
            GeneratorToSave.MaxRoomSize = new() { Width = (int)nudMaxWidth.Value, Height = (int)nudMaxHeight.Value };
            GeneratorToSave.RoomDisposition = roomDispositionSequence.ToString();
            Saved = true;
            this.Close();
        }

        private bool RoomsHaveNoMoreThanOneFusion()
        {
            var validRoomTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedRoom, RoomDispositionType.GuaranteedDummyRoom, RoomDispositionType.RandomRoom };
            var roomDispositionTiles = tlpRoomDisposition.Controls.Cast<RoomDispositionTile>().ToList();
            foreach (var tile in roomDispositionTiles)
            {
                if (!validRoomTileTypes.Contains(tile.RoomDispositionType)) continue;
                var upHallway = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X - 1 && rdt.Y == tile.Y);
                var downHallway = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X + 1 && rdt.Y == tile.Y);
                var leftHallway = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X && rdt.Y == tile.Y - 1);
                var rightHallway = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X && rdt.Y == tile.Y + 1);
                var hallwayList = new List<RoomDispositionType?> { upHallway?.RoomDispositionType, downHallway?.RoomDispositionType, leftHallway?.RoomDispositionType, rightHallway?.RoomDispositionType };
                if (hallwayList.Count(rdt => rdt != null && rdt == RoomDispositionType.GuaranteedFusion) > 1)
                    return false;
            }
            return true;
        }

        private bool ConnectionsHaveBothEndsCovered()
        {
            var validHallwayTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedFusion, RoomDispositionType.GuaranteedHallway, RoomDispositionType.RandomConnection };
            var invalidEndTileTypes = new List<RoomDispositionType>() { RoomDispositionType.NoRoom, RoomDispositionType.ConnectionImpossible };
            var roomDispositionTiles = tlpRoomDisposition.Controls.Cast<RoomDispositionTile>().ToList();
            foreach (var tile in roomDispositionTiles)
            {
                if (!validHallwayTileTypes.Contains(tile.RoomDispositionType)) continue;
                var upRoom = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X - 1 && rdt.Y == tile.Y);
                var downRoom = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X + 1 && rdt.Y == tile.Y);
                var leftRoom = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X && rdt.Y == tile.Y - 1);
                var rightRoom = roomDispositionTiles.FirstOrDefault(rdt => rdt.X == tile.X && rdt.Y == tile.Y + 1);
                var isVerticalConnection = tile.X % 2 != 0 && tile.Y % 2 == 0;
                var isHorizontalConnection = tile.X % 2 == 0 && tile.Y % 2 != 0;
                if (isVerticalConnection && ((upRoom == null || downRoom == null) || (invalidEndTileTypes.Contains(upRoom.RoomDispositionType) || invalidEndTileTypes.Contains(downRoom.RoomDispositionType))))
                    return false;
                else if (isHorizontalConnection && ((leftRoom == null || rightRoom == null) || (invalidEndTileTypes.Contains(leftRoom.RoomDispositionType) || invalidEndTileTypes.Contains(rightRoom.RoomDispositionType))))
                    return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFloorTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show(
                $"Applying a Floor Layout Template will remove your current structure.\nWARNING: This CANNOT be reversed!\n\nDo you want to proceed?",
                "Use Floor Layout Template",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (messageBoxResult == DialogResult.No)
                return;

            switch (cmbFloorTemplate.Text)
            {
                case "Fully Random":
                    ConstructFullRandom();
                    break;
                case "Outer Ring of Dummy Rooms":
                    ConstructOuterDummyRing();
                    break;
                case "Inner Ring of Dummy Rooms":
                    ConstructInnerDummyRing();
                    break;
                case "A Single Room":
                    ConstructOneBigRoom();
                    break;
                default:
                    break;
            }
            cmbFloorTemplate.Text = "";
        }

        private void ConstructFullRandom()
        {
            var rows = Rows + Rows - 1;
            var columns = Columns + Columns - 1;
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var roomDispositionTile = tlpRoomDisposition.GetControlFromPosition(x, y) as RoomDispositionTile;
                    if (x % 2 == 0 && y % 2 == 0)
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.RandomRoom;
                    else if ((x % 2 != 0 && y % 2 == 0) || (x % 2 == 0 && y % 2 != 0))
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.RandomConnection;
                    else
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.ConnectionImpossible;
                }
            }
        }

        private void ConstructOuterDummyRing()
        {
            var rows = Rows + Rows - 1;
            var columns = Columns + Columns - 1;
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var roomDispositionTile = tlpRoomDisposition.GetControlFromPosition(x, y) as RoomDispositionTile;

                    // Calculate which ring we are in (0-based index)
                    int ringNumber = Math.Min(Math.Min(x, columns - 1 - x), Math.Min(y, rows - 1 - y)) / 2;

                    // Determine if this ring should have Dummy rooms or Normal rooms
                    bool isDummyRing = ringNumber % 2 == 0;

                    if (y % 2 == 0) // Rows with rooms or dummy rooms
                    {
                        if (x % 2 == 0)
                        {
                            roomDispositionTile.RoomDispositionType = isDummyRing
                                ? RoomDispositionType.GuaranteedDummyRoom
                                : RoomDispositionType.GuaranteedRoom;
                        }
                        else
                        {
                            roomDispositionTile.RoomDispositionType = RoomDispositionType.RandomConnection;
                        }
                    }
                    else // Rows with connection/impossible spaces
                    {
                        if (x % 2 == 0)
                        {
                            roomDispositionTile.RoomDispositionType = RoomDispositionType.RandomConnection;
                        }
                        else
                        {
                            roomDispositionTile.RoomDispositionType = RoomDispositionType.ConnectionImpossible;
                        }
                    }
                }
            }
        }

        private void ConstructInnerDummyRing()
        {
            var rows = Rows + Rows - 1;
            var columns = Columns + Columns - 1;
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var roomDispositionTile = tlpRoomDisposition.GetControlFromPosition(x, y) as RoomDispositionTile;

                    // Calculate which ring we are in (0-based index)
                    int ringNumber = Math.Min(Math.Min(x, columns - 1 - x), Math.Min(y, rows - 1 - y)) / 2;

                    // Determine if this ring should have Dummy rooms or Normal rooms
                    bool isDummyRing = ringNumber % 2 != 0;

                    if (y % 2 == 0) // Rows with rooms or dummy rooms
                    {
                        if (x % 2 == 0)
                        {
                            roomDispositionTile.RoomDispositionType = isDummyRing
                                ? RoomDispositionType.GuaranteedDummyRoom
                                : RoomDispositionType.GuaranteedRoom;
                        }
                        else
                        {
                            roomDispositionTile.RoomDispositionType = RoomDispositionType.RandomConnection;
                        }
                    }
                    else // Rows with connection/impossible spaces
                    {
                        if (x % 2 == 0)
                        {
                            roomDispositionTile.RoomDispositionType = RoomDispositionType.RandomConnection;
                        }
                        else
                        {
                            roomDispositionTile.RoomDispositionType = RoomDispositionType.ConnectionImpossible;
                        }
                    }
                }
            }
        }

        private void ConstructOneBigRoom()
        {
            var rows = Rows + Rows - 1;
            var columns = Columns + Columns - 1;
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var roomDispositionTile = tlpRoomDisposition.GetControlFromPosition(x, y) as RoomDispositionTile;
                    if (x == 0 && y == 0)
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.GuaranteedRoom;
                    else if (x % 2 == 0 && y % 2 == 0)
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.NoRoom;
                    else if ((x % 2 != 0 && y % 2 == 0) || (x % 2 == 0 && y % 2 != 0))
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.NoConnection;
                    else
                        roomDispositionTile.RoomDispositionType = RoomDispositionType.ConnectionImpossible;
                }
            }
        }

        private void nudMinWidth_Validating(object sender, CancelEventArgs e)
        {
            if (nudMinWidth.Value > nudMaxWidth.Value)
                e.Cancel = true;
        }

        private void nudMinHeight_Validating(object sender, CancelEventArgs e)
        {
            if (nudMinHeight.Value > nudMaxHeight.Value)
                e.Cancel = true;
        }

        private void nudMaxWidth_Validating(object sender, CancelEventArgs e)
        {
            if (nudMaxWidth.Value < nudMinWidth.Value)
                e.Cancel = true;
        }

        private void nudMaxHeight_Validating(object sender, CancelEventArgs e)
        {
            if (nudMaxHeight.Value < nudMinHeight.Value)
                e.Cancel = true;
        }

        private void btnSetToMinimumSize_Click(object sender, EventArgs e)
        {
            nudMinWidth.Value = nudMinWidth.Minimum;
            nudMinHeight.Value = nudMinHeight.Minimum;
        }

        private void btnSetToMaximumSize_Click(object sender, EventArgs e)
        {
            nudMaxWidth.Value = nudMaxWidth.Maximum;
            nudMaxHeight.Value = nudMaxHeight.Maximum;
        }
    }
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.HelperForms
{
    public partial class frmStaticFloorEditor : Form
    {
        private string PreviousCellValue;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FloorLayoutGenerationInfo GeneratorToSave { get; private set; }
        private readonly List<FloorLayoutGenerationInfo> CurrentGenerators;

        private Dictionary<string, string> RandomCharacterSpawnIds = new()
        {
            { EngineConstants.SPAWN_PLAYER_CHARACTER, "Player Character" },
            { EngineConstants.SPAWN_ANY_CHARACTER, "Anyone (including Player)" },
            { EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER, "Any Ally (including Player)" },
            { EngineConstants.SPAWN_ANY_ALLIED_CHARACTER, "Any Ally" },
            { EngineConstants.SPAWN_ANY_NEUTRAL_CHARACTER, "Any Neutral" },
            { EngineConstants.SPAWN_ANY_ENEMY_CHARACTER, "Any Enemy" }
        };
        private Dictionary<string, string> RandomItemSpawnIds = new()
        {
            { EngineConstants.SPAWN_ANY_ITEM, "Any Item" }
        };
        private Dictionary<string, string> RandomTrapSpawnIds = new()
        {
            { EngineConstants.SPAWN_ANY_TRAP, "Any Trap" }
        };

        private List<string> _qualityLevels;
        private int _selectedToolboxIndex = -1;
        private bool _isDrawing = false;
        private bool _highlightingIsActive = false;
        private DungeonInfo ActiveDungeon;
        private Font FontToUse;
        private FloorInfo CurrentFloor;
        private readonly ToolTip _toolTip = new();
        private readonly List<(ConsoleRepresentation Icon, string Description, RoomTileType Type, string TileTypeId)> ToolBoxList = new();
        private RoomTile[,] _tiles;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Saved { get; set; }

        public frmStaticFloorEditor(List<FloorLayoutGenerationInfo> currentGenerators, int width, int height, int minFloorLevel, int maxFloorLevel, FloorInfo currentFloor, DungeonInfo activeDungeon, FloorLayoutGenerationInfo generatorToSave)
        {
            InitializeComponent();
            ActiveDungeon = activeDungeon;
            CurrentFloor = currentFloor;
            CurrentGenerators = currentGenerators;
            if (minFloorLevel != maxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";

            _qualityLevels = activeDungeon.QualityLevelInfos.ConvertAll(q => q.Id);
            var qualityLevelColumn = (DataGridViewComboBoxColumn)dgvItems.Columns["ItemQualityLevel"];
            qualityLevelColumn.DataSource = _qualityLevels;

            try
            {
                var fontPath = Path.Combine(Application.StartupPath, "Resources\\PxPlus_Tandy1K-II_200L.ttf");
                var fontName = "PxPlus Tandy1K-II 200L";
                if (FontHelpers.LoadFont(fontPath))
                {
                    var loadedFont = FontHelpers.GetFontByName(fontName);
                    if (loadedFont != null)
                    {
                        FontToUse = new Font(loadedFont, 8f, FontStyle.Regular);
                    }
                }
            }
            catch
            {
                // Do nothing if the Font can't be found
            }
            GeneratorToSave = generatorToSave ?? new FloorLayoutGenerationInfo();

            var regularGridHeight = pnlGridSection.Height;
            pnlGrid.Width = 12 * width;
            pnlGrid.Height = 12 * height;
            if (pnlGrid.Width < pnlGridSection.Width)
                pnlGridSection.Width = pnlGrid.Width;
            if (pnlGrid.Height < pnlGridSection.Height)
                pnlGridSection.Height = pnlGrid.Height;

            pnlGrid.Paint += pnlGrid_Paint;
            pnlGrid.MouseDown += pnlGrid_MouseDown;
            pnlGrid.MouseMove += pnlGrid_MouseMove;
            pnlGrid.MouseUp += pnlGrid_MouseUp;
            pnlGrid.MouseLeave += (s, e) => { _isDrawing = false; lblCurrentlyOn.Text = ""; };

            tlpToolbox.Controls.Clear();

            ToolBoxList.AddRange(GetListForToolbox());

            if (generatorToSave == null || string.IsNullOrWhiteSpace(generatorToSave.StaticGenerator.FloorGeometry))
            {
                InitializeFloorGrid(width, height);
            }
            else
            {
                InitializeFloorGrid(generatorToSave.StaticGenerator, width, height);
            }

            tlpToolbox.RowCount = ToolBoxList.Count;
            tlpToolbox.ColumnCount = 2;
            tlpToolbox.ColumnStyles.Clear();
            tlpToolbox.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12f));
            tlpToolbox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            for (int i = 0; i < ToolBoxList.Count; i++)
            {
                var icon = ToolBoxList[i].Icon;
                var description = ToolBoxList[i].Description;
                var type = ToolBoxList[i].Type;

                var tileLabel = new Label
                {
                    Text = icon.Character.ToString(),
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = icon.BackgroundColor.ToColor(),
                    ForeColor = icon.ForegroundColor.ToColor(),
                    Font = FontToUse,
                    Margin = Padding.Empty,
                    Padding = Padding.Empty
                };

                var descriptionLabel = new Label
                {
                    Text = description,
                    AutoSize = false,
                    AutoEllipsis = true,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new(new FontFamily("Segoe UI"), 7f, FontStyle.Regular),
                    Height = 8
                };
                _toolTip.SetToolTip(descriptionLabel, description);

                int rowIndex = i;
                tileLabel.Click += (s, e) => SelectToolboxRow(rowIndex);
                descriptionLabel.Click += (s, e) => SelectToolboxRow(rowIndex);

                tlpToolbox.RowStyles.Add(new RowStyle(SizeType.Absolute, 12f));
                tlpToolbox.Controls.Add(tileLabel, 0, i);
                tlpToolbox.Controls.Add(descriptionLabel, 1, i);
            }

            pnlToolbox.Height = tcEntitySpawns.Height = pnlGridSection.Height - lblToolbox.Height;
            tlpCheckboxes.Dock = tlpButtons.Dock = DockStyle.None;
            this.Width = tlpEditor.Width = tlpButtons.Width = tlpCheckboxes.Width = flpEditor.Width;
            if (pnlGridSection.Height > regularGridHeight)
                tlpEditor.Height += (pnlGridSection.Height - regularGridHeight);
            else
                tlpEditor.Height -= (regularGridHeight - pnlGridSection.Height);
            tcEntitySpawns.Height = pnlGridSection.Height;
            dgvWaypoints.Height = tcEntitySpawns.Height - (int) (fklblWaypoints.Height * 1.5);
            this.Height = tlpEditor.Height;
        }

        public void InitializeFloorGrid(int width, int height)
        {
            _tiles = new RoomTile[width, height];
            dgvCharacters.Rows.Clear();
            dgvItems.Rows.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _tiles[x, y] = new RoomTile()
                    {
                        X = x + 1,
                        Y = y + 1,
                        TileType = RoomTileType.NormalTileType,
                        ConsoleRepresentation = new ConsoleRepresentation
                        {
                            Character = ' ',
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.White)
                        },
                        TileTypeId = "Empty"
                    };
                    _tiles[x, y].BaseConsoleRepresentation = _tiles[x, y].ConsoleRepresentation.Clone();
                }
            }

            pnlGrid.Invalidate();
        }

        public void InitializeFloorGrid(StaticGeneratorInfo generator, int width, int height)
        {
            _tiles = new RoomTile[width, height];
            dgvCharacters.Rows.Clear();
            dgvItems.Rows.Clear();
            dgvWaypoints.Rows.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _tiles[x, y] = new RoomTile()
                    {
                        X = x + 1,
                        Y = y + 1
                    };
                    var charIndex = x + (y * width);
                    if (charIndex >= generator.FloorGeometry.Length)
                    {
                        _tiles[x, y].TileType = RoomTileType.NormalTileType;
                        _tiles[x, y].ConsoleRepresentation = new ConsoleRepresentation
                        {
                            Character = ' ',
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.White)
                        };
                        _tiles[x, y].BaseConsoleRepresentation = _tiles[x, y].ConsoleRepresentation.Clone();
                        _tiles[x, y].TileTypeId = "Empty";
                        continue;
                    }
                    var geometryTile = generator.FloorGeometry[charIndex];
                    var toolboxItem = geometryTile switch
                    {
                        '-' => ToolBoxList.Find(t => t.TileTypeId == "Empty"),
                        '.' => ToolBoxList.Find(t => t.TileTypeId == "Floor"),
                        '#' => ToolBoxList.Find(t => t.TileTypeId == "Wall"),
                        '+' => ToolBoxList.Find(t => t.TileTypeId == "Hallway"),
                        _ => ToolBoxList.Find(t => t.TileTypeId == "Empty")
                    };
                    var specialSpawn = generator.SpecialSpawns?.Find(s => s.X == x + 1 && s.Y == y + 1);
                    if (specialSpawn != null)
                    {
                        var spawnIdToDescription = specialSpawn.SpawnId;
                        if (RandomCharacterSpawnIds.TryGetValue(specialSpawn.SpawnId, out var value))
                            spawnIdToDescription = value;
                        if (RandomItemSpawnIds.TryGetValue(specialSpawn.SpawnId, out var value2))
                            spawnIdToDescription = value2;
                        if (RandomTrapSpawnIds.TryGetValue(specialSpawn.SpawnId, out var value3))
                            spawnIdToDescription = value3;
                        if (spawnIdToDescription.StartsWith("Currency "))
                            spawnIdToDescription = spawnIdToDescription.Replace("Currency", ActiveDungeon.CurrencyInfo.Name);
                        toolboxItem = ToolBoxList.Find(t => t.Description == spawnIdToDescription);
                        if (toolboxItem.Type == RoomTileType.CharacterIncludingPlayer || toolboxItem.Type == RoomTileType.NPC)
                        {
                            _tiles[x, y].Level = specialSpawn.Level;
                            _tiles[x, y].TileType = toolboxItem.Type;
                        }
                        else if (toolboxItem.Type == RoomTileType.Item)
                        {
                            _tiles[x, y].Level = specialSpawn.Level;
                            _tiles[x, y].QualityLevel = specialSpawn.QualityLevel;
                            _tiles[x, y].TileType = toolboxItem.Type;
                        }
                        else if (toolboxItem.Type == RoomTileType.Waypoint)
                        {
                            _tiles[x, y].WaypointId = specialSpawn.WaypointId;
                            _tiles[x, y].TileType = toolboxItem.Type;
                        }
                    }
                    _selectedToolboxIndex = ToolBoxList.IndexOf(toolboxItem);
                    ApplyToolboxItemToTile(_tiles[x, y]);
                }
            }
            btnSave.Enabled = btnHighlightIslands.Enabled = btnHighlightRooms.Enabled = _tiles.Any(t => t.ConsoleRepresentation.Character != ' ');
            chkUseItemGenerator.Checked = generator.GenerateItemsOnFirstTurn;
            chkUseNPCGenerator.Checked = generator.GenerateNPCsAfterFirstTurn;
            chkUseTrapGenerator.Checked = generator.GenerateTrapsOnFirstTurn;

            pnlGrid.Invalidate();
        }

        private void pnlGrid_Paint(object sender, PaintEventArgs e)
        {
            if (_tiles == null) return;

            int tileSize = 12;

            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    var tile = _tiles[x, y];

                    // Draw background
                    using var brush = new SolidBrush(tile.ConsoleRepresentation.BackgroundColor.ToColor());
                    e.Graphics.FillRectangle(brush, x * tileSize, y * tileSize, tileSize, tileSize);

                    // Draw character
                    string text = tile.ConsoleRepresentation.Character.ToString();
                    Font font = FontToUse ?? this.Font;
                    SizeF textSize = e.Graphics.MeasureString(text, font);

                    float textX = x * tileSize + (tileSize - textSize.Width) / 2;
                    float textY = y * tileSize + (tileSize - textSize.Height) / 2;

                    using var fgBrush = new SolidBrush(tile.ConsoleRepresentation.ForegroundColor.ToColor());
                    e.Graphics.DrawString(text, font, fgBrush, textX, textY);
                }
            }
        }
        private void pnlGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (_selectedToolboxIndex < 0 || _highlightingIsActive) return;

            if (e.Button == MouseButtons.Left)
            {
                _isDrawing = true;
                PaintTileAtMouse(e.Location);
            }
        }

        private void pnlGrid_MouseMove(object sender, MouseEventArgs e)
        {
            int tileSize = 12;

            int x = e.Location.X / tileSize;
            int y = e.Location.Y / tileSize;


            if (x >= 0 && y >= 0 && x < _tiles.GetLength(0) && y < _tiles.GetLength(1))
            {
                var tile = _tiles[x, y];
                lblCurrentlyOn.Text = $"Currently on ({tile.X}, {tile.Y})";
            }

            if (_isDrawing && !_highlightingIsActive)
                PaintTileAtMouse(e.Location);
        }

        private void pnlGrid_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
        }

        private void PaintTileAtMouse(Point mouseLocation)
        {
            dgvCharacters.EndEdit();
            dgvItems.EndEdit();
            dgvWaypoints.EndEdit();
            int tileSize = 12;

            int x = mouseLocation.X / tileSize;
            int y = mouseLocation.Y / tileSize;

            if (x >= 0 && y >= 0 && x < _tiles.GetLength(0) && y < _tiles.GetLength(1))
            {
                var tile = _tiles[x, y];
                ApplyToolboxItemToTile(tile);
                pnlGrid.Invalidate(new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
            }
        }

        private void ApplyToolboxItemToTile(RoomTile tile)
        {
            var previousType = tile.TileType;
            var wasACharacterTableEntry = previousType == RoomTileType.CharacterIncludingPlayer || tile.TileType == RoomTileType.NPC;
            var wasAnItemTableEntry = previousType == RoomTileType.Item;
            var wasAWaypointTableEntry = previousType == RoomTileType.Waypoint;
            var toolboxItem = ToolBoxList[_selectedToolboxIndex];
            tile.TileType = toolboxItem.Type;
            tile.ConsoleRepresentation = toolboxItem.Icon;
            tile.BaseConsoleRepresentation = toolboxItem.Icon.Clone();
            tile.TileTypeId = toolboxItem.TileTypeId;
            tile.Description = toolboxItem.Description;
            btnSave.Enabled = btnHighlightIslands.Enabled = btnHighlightRooms.Enabled = _tiles.Any(t => t.ConsoleRepresentation.Character != ' ');
            var currentType = tile.TileType;
            var isACharacterTableEntry = currentType == RoomTileType.CharacterIncludingPlayer || tile.TileType == RoomTileType.NPC;
            var isAnItemTableEntry = currentType == RoomTileType.Item;
            var isAWaypointTableEntry = currentType == RoomTileType.Waypoint;

            if (!wasACharacterTableEntry && isACharacterTableEntry)
            {
                tile.Level = 1;
            }
            if (!wasAnItemTableEntry && isAnItemTableEntry)
            {
                tile.Level = 1;
                tile.QualityLevel = _qualityLevels[0];
            }
            if (!wasAWaypointTableEntry && isAWaypointTableEntry)
            {
                tile.WaypointId = "W1";
            }

            if (wasACharacterTableEntry || isACharacterTableEntry)
            {
                dgvCharacters.Rows.Clear();
                var tableEntries = _tiles.Where(t => t.TileType == RoomTileType.CharacterIncludingPlayer || t.TileType == RoomTileType.NPC);
                foreach (var entry in tableEntries)
                {
                    dgvCharacters.Rows.Add(entry.Description, entry.X, entry.Y, entry.Level);
                }
            }
            if (wasAnItemTableEntry || isAnItemTableEntry)
            {
                dgvItems.Rows.Clear();
                var tableEntries = _tiles.Where(t => t.TileType == RoomTileType.Item);
                foreach (var entry in tableEntries)
                {
                    var qualityLevel = entry.QualityLevel ?? _qualityLevels[0];
                    dgvItems.Rows.Add(entry.Description, entry.X, entry.Y, entry.Level, qualityLevel);
                }
            }
            if (wasAWaypointTableEntry || isAWaypointTableEntry)
            {
                dgvWaypoints.Rows.Clear();
                var tableEntries = _tiles.Where(t => t.TileType == RoomTileType.Waypoint);
                foreach (var entry in tableEntries)
                {
                    dgvWaypoints.Rows.Add(entry.X, entry.Y, entry.WaypointId);
                }
            }
        }

        private void SelectToolboxRow(int rowIndex)
        {
            if (_selectedToolboxIndex > -1)
                tlpToolbox.GetControlFromPosition(1, _selectedToolboxIndex).BackColor = SystemColors.Control;

            _selectedToolboxIndex = rowIndex;
            tlpToolbox.GetControlFromPosition(1, rowIndex).BackColor = Color.LightBlue;
        }

        private List<(ConsoleRepresentation Icon, string Description, RoomTileType Type, string TileTypeId)> GetListForToolbox()
        {
            var toolboxList = new List<(ConsoleRepresentation Icon, string Description, RoomTileType Type, string TileTypeId)>();
            var firstTileSet = ActiveDungeon.TileSetInfos[0];

            var emptyTileTypeSet = firstTileSet.TileTypes.Find(tt => tt.TileTypeId == "Empty");
            toolboxList.Add((emptyTileTypeSet.Central, "Empty Tile", RoomTileType.NormalTileType, "Empty"));

            var floorTileTypeSet = firstTileSet.TileTypes.Find(tt => tt.TileTypeId == "Floor");
            toolboxList.Add((floorTileTypeSet.Central, "Floor", RoomTileType.NormalTileType, "Floor"));

            var wallTileTypeSet = firstTileSet.TileTypes.Find(tt => tt.TileTypeId == "Wall");
            toolboxList.Add((wallTileTypeSet.Horizontal, "Wall", RoomTileType.NormalTileType, "Wall"));

            var hallwayTileTypeSet = firstTileSet.TileTypes.Find(tt => tt.TileTypeId == "Hallway");
            toolboxList.Add((hallwayTileTypeSet.Connector, "Hallway", RoomTileType.NormalTileType, "Hallway"));

            var stairsTileTypeSet = firstTileSet.TileTypes.Find(tt => tt.TileTypeId == "Stairs");
            toolboxList.Add((stairsTileTypeSet.Central, "Stairs", RoomTileType.Stairs, "Stairs"));

            var waypointIcon = new ConsoleRepresentation
            {
                Character = '↔',
                BackgroundColor = new GameColor(Color.White),
                ForegroundColor = new GameColor(Color.Red)
            };
            toolboxList.Add((waypointIcon, "Waypoint", RoomTileType.Waypoint, "Floor"));

            foreach (var tileTypeSet in firstTileSet.TileTypes.Where(tt => tt.TileTypeId != "Empty" && tt.TileTypeId != "Floor" && tt.TileTypeId != "Wall" && tt.TileTypeId != "Hallway" && tt.TileTypeId != "Stairs"))
            {
                toolboxList.Add((tileTypeSet.Central, tileTypeSet.TileTypeId, RoomTileType.SpecialTileType, "Floor"));
            }

            foreach (var keyType in CurrentFloor.PossibleKeys.KeyTypes)
            {
                toolboxList.Add((keyType.KeyConsoleRepresentation, $"{keyType.KeyTypeName} Key", RoomTileType.Key, "Floor"));
                toolboxList.Add((keyType.DoorConsoleRepresentation, $"{keyType.KeyTypeName} Door", RoomTileType.Door, "Hallway"));
            }

            foreach (var currencyPile in ActiveDungeon.CurrencyInfo.CurrencyPiles)
            {
                toolboxList.Add((ActiveDungeon.CurrencyInfo.ConsoleRepresentation, $"{ActiveDungeon.CurrencyInfo.Name} ({currencyPile.Id})", RoomTileType.Currency, "Floor"));
            }

            var playerCharacterIcon = new ConsoleRepresentation
            {
                Character = 'P',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.FromArgb(0, 255, 0))
            };
            toolboxList.Add((playerCharacterIcon, RandomCharacterSpawnIds[EngineConstants.SPAWN_PLAYER_CHARACTER], RoomTileType.CharacterIncludingPlayer, "Floor"));

            var anyCharacterIncludingPlayerIcon = new ConsoleRepresentation
            {
                Character = '?',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.Orange)
            };
            toolboxList.Add((anyCharacterIncludingPlayerIcon, RandomCharacterSpawnIds[EngineConstants.SPAWN_ANY_CHARACTER], RoomTileType.CharacterIncludingPlayer, "Floor"));

            var anyAlliedCharacterIncludingPlayerIcon = new ConsoleRepresentation
            {
                Character = '?',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.FromArgb(0, 255, 0))
            };
            toolboxList.Add((anyAlliedCharacterIncludingPlayerIcon, RandomCharacterSpawnIds[EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER], RoomTileType.CharacterIncludingPlayer, "Floor"));

            var anyAlliedCharacterIcon = new ConsoleRepresentation
            {
                Character = 'a',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.FromArgb(0, 255, 0))
            };
            toolboxList.Add((anyAlliedCharacterIcon, RandomCharacterSpawnIds[EngineConstants.SPAWN_ANY_ALLIED_CHARACTER], RoomTileType.NPC, "Floor"));

            var anyNeutralCharacterIcon = new ConsoleRepresentation
            {
                Character = 'n',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.Yellow)
            };
            toolboxList.Add((anyNeutralCharacterIcon, RandomCharacterSpawnIds[EngineConstants.SPAWN_ANY_NEUTRAL_CHARACTER], RoomTileType.NPC, "Floor"));

            var anyEnemyCharacterIcon = new ConsoleRepresentation
            {
                Character = 'e',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.Red)
            };
            toolboxList.Add((anyEnemyCharacterIcon, RandomCharacterSpawnIds[EngineConstants.SPAWN_ANY_ENEMY_CHARACTER], RoomTileType.NPC, "Floor"));

            foreach (var npc in ActiveDungeon.NPCs)
            {
                toolboxList.Add((npc.ConsoleRepresentation, npc.Id, RoomTileType.NPC, "Floor"));
            }

            var anyItemIcon = new ConsoleRepresentation
            {
                Character = '?',
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.Purple)
            };
            toolboxList.Add((anyItemIcon, RandomItemSpawnIds[EngineConstants.SPAWN_ANY_ITEM], RoomTileType.Item, "Floor"));

            foreach (var item in ActiveDungeon.Items)
            {
                toolboxList.Add((item.ConsoleRepresentation, item.Id, RoomTileType.Item, "Floor"));
            }

            var anyTrapIcon = new ConsoleRepresentation
            {
                Character = '?',
                BackgroundColor = new GameColor(Color.Red),
                ForegroundColor = new GameColor(Color.Yellow)
            };
            toolboxList.Add((anyTrapIcon, "Any Trap", RoomTileType.Trap, "Floor"));

            foreach (var trap in ActiveDungeon.Traps)
            {
                toolboxList.Add((trap.ConsoleRepresentation, trap.Id, RoomTileType.Trap, "Floor"));
            }

            return toolboxList;
        }

        private void dgvCharacters_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvCharacters.Columns[e.ColumnIndex].Name != "CharacterLevel") return;
            var cellValue = dgvCharacters.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            PreviousCellValue = cellValue != null ? cellValue.ToString() : string.Empty;
        }

        private void dgvCharacters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCharacters.Rows[e.RowIndex].IsNewRow) return;
            if (dgvCharacters.Columns[e.ColumnIndex].Name != "CharacterLevel") return;
            var cellValue = dgvCharacters.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;

            if (!int.TryParse(cellValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int number) || number < 1)
            {
                dgvCharacters[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
            }
            else
            {
                var rowX = (int)dgvCharacters.Rows[e.RowIndex].Cells["CharacterX"].Value;
                var rowY = (int)dgvCharacters.Rows[e.RowIndex].Cells["CharacterY"].Value;
                var correspondingTile = _tiles[rowX - 1, rowY - 1];
                correspondingTile.Level = int.Parse(cellValue, CultureInfo.InvariantCulture);
            }

        }

        private void dgvItems_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvItems.Columns[e.ColumnIndex].Name != "ItemLevel" && dgvItems.Columns[e.ColumnIndex].Name != "ItemQualityLevel") return;
            var cellValue = dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            PreviousCellValue = cellValue != null ? cellValue.ToString() : string.Empty;
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows[e.RowIndex].IsNewRow) return;
            if (dgvItems.Columns[e.ColumnIndex].Name != "ItemLevel" && dgvItems.Columns[e.ColumnIndex].Name != "ItemQualityLevel") return;

            var cellValue = dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;

            var rowX = (int)dgvItems.Rows[e.RowIndex].Cells["ItemX"].Value;
            var rowY = (int)dgvItems.Rows[e.RowIndex].Cells["ItemY"].Value;
            var correspondingTile = _tiles[rowX - 1, rowY - 1];

            if (dgvItems.Columns[e.ColumnIndex].Name == "ItemLevel")
            {
                if (!int.TryParse(cellValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int _))
                {
                    dgvItems[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
                }
                else
                {
                    correspondingTile.Level = int.Parse(cellValue, CultureInfo.InvariantCulture);
                }
            }
            if (dgvItems.Columns[e.ColumnIndex].Name == "ItemQualityLevel")
            {
                if (!_qualityLevels.Contains(cellValue))
                {
                    dgvItems[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
                }
                else
                {
                    correspondingTile.QualityLevel = cellValue;
                }
            }
        }

        private void dgvWaypoints_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvWaypoints.Columns[e.ColumnIndex].Name != "WaypointId") return;
            var cellValue = dgvWaypoints.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            PreviousCellValue = cellValue != null ? cellValue.ToString() : string.Empty;
        }

        private void dgvWaypoints_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvWaypoints.Rows[e.RowIndex].IsNewRow) return;
            if (dgvWaypoints.Columns[e.ColumnIndex].Name != "WaypointId") return;

            var cellValue = dgvWaypoints.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;

            var rowX = (int)dgvWaypoints.Rows[e.RowIndex].Cells["WaypointX"].Value;
            var rowY = (int)dgvWaypoints.Rows[e.RowIndex].Cells["WaypointY"].Value;
            var correspondingTile = _tiles[rowX - 1, rowY - 1];

            if (string.IsNullOrWhiteSpace(cellValue))
            {
                dgvItems[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
            }
            else
            {
                correspondingTile.WaypointId = cellValue;
            }
        }

        private void btnHighlightRooms_Click(object sender, EventArgs e)
        {
            if (_highlightingIsActive)
            {
                // Toggle back appearances to normal
                foreach (var row in _tiles)
                {
                    row.ConsoleRepresentation = row.BaseConsoleRepresentation;
                }
                _highlightingIsActive = false;
                btnHighlightIslands.Visible = true;
                btnHighlightRooms.Text = "Highlight Rooms";
                btnSave.Enabled = btnCancel.Enabled = true;
                pnlGrid.Invalidate();
            }
            else
            {
                HighlightRooms();
                btnHighlightIslands.Visible = false;
                btnHighlightRooms.Text = "Disable Highlighting";
                btnSave.Enabled = btnCancel.Enabled = false;
                _highlightingIsActive = true;
            }
        }

        private void btnHighlightIslands_Click(object sender, EventArgs e)
        {
            if (_highlightingIsActive)
            {
                // Toggle back appearances to normal
                foreach (var row in _tiles)
                {
                    row.ConsoleRepresentation = row.BaseConsoleRepresentation;
                }
                _highlightingIsActive = false;
                pnlGrid.Invalidate();
            }
            else
            {
                HighlightIslands();
                btnHighlightIslands.Visible = false;
                btnHighlightRooms.Text = "Disable Highlighting";
                btnSave.Enabled = btnCancel.Enabled = false;
                _highlightingIsActive = true;
            }
        }

        private void HighlightIslands()
        {
            var islands = _tiles.GetIslands(t => IsWalkableTile(t, true));

            Random rng = new();
            foreach (var island in islands)
            {
                Color highlight = Color.FromArgb(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256));

                foreach (var tile in island)
                {
                    tile.ConsoleRepresentation = new ConsoleRepresentation
                    {
                        Character = tile.ConsoleRepresentation.Character,
                        ForegroundColor = tile.TileTypeId != "Wall" ? tile.ConsoleRepresentation.ForegroundColor : new GameColor(highlight),
                        BackgroundColor = tile.TileTypeId != "Wall" ? new GameColor(highlight) : tile.ConsoleRepresentation.BackgroundColor
                    };
                }
            }

            pnlGrid.Invalidate();
        }

        private void HighlightRooms()
        {
            int rows = _tiles.GetLength(1);
            int cols = _tiles.GetLength(0);

            bool[,] visited = new bool[cols, rows];
            var _detectedRooms = GetRooms();

            Random rng = new();
            foreach (var room in _detectedRooms)
            {
                Color highlight = Color.FromArgb(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256));

                foreach (var tile in room)
                {
                    tile.ConsoleRepresentation = new ConsoleRepresentation
                    {
                        Character = tile.ConsoleRepresentation.Character,
                        ForegroundColor = tile.TileTypeId != "Wall" ? tile.ConsoleRepresentation.ForegroundColor : new GameColor(highlight),
                        BackgroundColor = tile.TileTypeId != "Wall" ? new GameColor(highlight) : tile.ConsoleRepresentation.BackgroundColor
                    };
                }
            }

            pnlGrid.Invalidate();
        }

        private List<List<RoomTile>> GetRooms()
        {
            int rows = _tiles.GetLength(1);
            int cols = _tiles.GetLength(0);
            bool[,] visited = new bool[cols, rows];
            var detectedRooms = new List<List<RoomTile>>();
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (!visited[x, y] && IsTileValidForRoom(_tiles[x, y]) && IsStrictFloor(_tiles[x, y]))
                    {
                        var room = FloodFillRoom(visited, x, y);
                        if (room.Count > 0)
                        {
                            var boundaries = AddAdjacentWalls(room);
                            if (boundaries.Count > 0)
                            {
                                room = boundaries.ToList();
                                detectedRooms.Add(room);
                            }
                        }
                    }
                }
            }
            return detectedRooms;
        }

        private bool IsTileValidForRoom(RoomTile tile)
        {
            if (tile == null) return false;
            if (tile.TileTypeId == "Empty") return false;
            if (tile.TileTypeId == "Wall") return false;
            if (tile.TileTypeId == "Hallway") return false;
            return true;
        }
        private bool IsStrictFloor(RoomTile tile)
        {
            if (tile == null) return false;
            if (tile.TileType == RoomTileType.SpecialTileType) return false;
            return tile.TileTypeId == "Floor" || tile.TileTypeId == "Stairs";
        }

        private bool IsWalkableTile(RoomTile tile, bool treatDoorsAsPassable)
        {
            var correspondingTileType = ActiveDungeon.TileTypeInfos.Find(tt => tt.Id == tile.TileTypeId);
            var isAWalkableTile = correspondingTileType != null && correspondingTileType.IsWalkable;
            return isAWalkableTile && (treatDoorsAsPassable || tile.TileType != RoomTileType.Door);
        }

        private List<RoomTile> FloodFillRoom(bool[,] visited, int startX, int startY)
        {
            int rows = _tiles.GetLength(1);
            int cols = _tiles.GetLength(0);

            List<RoomTile> roomTiles = new();
            Queue<(int x, int y)> queue = new();
            queue.Enqueue((startX, startY));
            var isFailedRoom = false;

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (x < 0 || y < 0 || x >= cols || y >= rows) continue;
                if (visited[x, y]) continue;

                var tile = _tiles[x, y];
                if (!IsTileValidForRoom(tile)) continue;

                visited[x, y] = true;
                roomTiles.Add(tile);

                var neighbors = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1), (1, 1), (1, -1), (-1, -1), (-1, 1) };
                var hasValidNeighbours = true;
                foreach (var (dx, dy) in neighbors)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < cols && ny < rows && !visited[nx, ny])
                    {
                        var neighbor = _tiles[nx, ny];
                        if (IsTileValidForRoom(neighbor))
                        {
                            queue.Enqueue((nx, ny));
                        }
                        else if (neighbor.TileTypeId == "Empty")
                        {
                            if (IsStrictFloor(tile))
                                isFailedRoom = true;
                            hasValidNeighbours = false;
                            break;
                        }
                    }
                }
                if (!hasValidNeighbours)
                    roomTiles.Remove(tile);
            }

            return isFailedRoom ? new() : roomTiles;
        }

        private List<RoomTile> AddAdjacentWalls(List<RoomTile> roomTiles)
        {
            HashSet<RoomTile> extended = new(roomTiles);

            foreach (var tile in roomTiles)
            {
                var neighbors = new (int dx, int dy)[]
                {
            (-1, 0), (1, 0), (0, -1), (0, 1),
            (-1,-1), (-1, 1), (1,-1), (1, 1)
                };
                var hasValidNeighbours = true;
                foreach (var (dx, dy) in neighbors)
                {
                    int nx = tile.X - 1 + dx;
                    int ny = tile.Y - 1 + dy;
                    if (nx >= 0 && ny >= 0 && nx < _tiles.GetLength(0) && ny < _tiles.GetLength(1))
                    {
                        var neighbor = _tiles[nx, ny];
                        if (neighbor.TileTypeId != "Empty")
                        {
                            extended.Add(neighbor);
                        }
                    }
                }
                if (!hasValidNeighbours && tile.TileType == RoomTileType.SpecialTileType)
                {
                    extended.Remove(tile);
                }
            }

            return extended.ToList();
        }

        private (bool SolvableWithoutKeys, bool SolvableWithKeys) IsFloorSolvable()
        {
            var solvableWithoutKeys = false;
            var solvableWithKeys = false;

            // A Floor is considered Solvable if...

            var rooms = GetRooms();

            // ... there is at least one room...
            if (rooms.Count == 0)
                return (false, false);

            var possiblePlayerPositions = _tiles.Where(_tiles => _tiles.TileType == RoomTileType.CharacterIncludingPlayer).ToList();
            var possibleStairsPositions = _tiles.Where(_tiles => _tiles.TileType == RoomTileType.Stairs).ToList();
            var islands = _tiles.GetIslands(t => IsWalkableTile(t, true));

            // ... there is at least one island...
            if (islands.Count == 0)
                return (false, false);

            // ... there are no preset Player positions or Stairs positions...
            if (possiblePlayerPositions.Count == 0 || possibleStairsPositions.Count == 0)
                return (true, true);

            // ... and, if there are preset Player positions and Stairs positions...
            foreach (var island in islands)
            {
                // ... there is at least one of both, or none at all, on each island.
                if (!island.Any(possiblePlayerPositions.Contains) && island.Any(possibleStairsPositions.Contains))
                    return (false, false);
                if (island.Any(possiblePlayerPositions.Contains) && !island.Any(possibleStairsPositions.Contains))
                    return (false, false);
            }

            return (true, IsFloorSolvableWithKeys());
        }

        private bool IsFloorSolvableWithKeys()
        {
            var keys = _tiles.Where(t => t.TileType == RoomTileType.Key).ToList();
            var doors = _tiles.Where(t => t.TileType == RoomTileType.Door).ToList();

            if (doors.Count == 0)
                return true;

            var possiblePlayerPositions = _tiles.Where(_tiles => _tiles.TileType == RoomTileType.CharacterIncludingPlayer).ToList();
            var possibleStairsPositions = _tiles.Where(_tiles => _tiles.TileType == RoomTileType.Stairs).ToList();
            var islands = _tiles.GetIslands(t => IsWalkableTile(t, true));

            var playerStairCombinations = new List<(RoomTile Player, RoomTile Stairs)>();
            foreach (var playerPos in possiblePlayerPositions)
            {
                var playerIsland = islands.Find(island => island.Contains(playerPos));
                foreach (var stairsPos in possibleStairsPositions)
                {
                    var stairsIsland = islands.Find(island => island.Contains(stairsPos));
                    if (playerIsland == stairsIsland)
                        playerStairCombinations.Add((playerPos, stairsPos));
                }
            }

            var allKeys = new List<string>();

            foreach (var (PlayerTile, StairsTile) in playerStairCombinations)
            {
                var islandsConsideringKeys = islands;
                var playerIsland = islandsConsideringKeys.Find(island => island.Contains(PlayerTile));
                var addedKeys = false;
                do
                {
                    addedKeys = false;
                    var newKeysInIsland = playerIsland.Where(t => t.TileType == RoomTileType.Key && !allKeys.Any(k => t.Description.Equals($"{k} Key", StringComparison.InvariantCultureIgnoreCase)));
                    if (newKeysInIsland.Any())
                    {
                        addedKeys = true;
                        foreach (var keyTile in newKeysInIsland)
                        {
                            allKeys.Add(keyTile.Description.Replace("Key", "").Trim());
                        }
                    }
                    islandsConsideringKeys = _tiles.GetIslands(t => IsWalkableTile(t, false) || allKeys.Any(k => !string.IsNullOrWhiteSpace(t.Description) && t.Description.Equals($"{k} Door", StringComparison.InvariantCultureIgnoreCase)));
                    playerIsland = islandsConsideringKeys.Find(island => island.Contains(PlayerTile));
                    if (playerIsland.Contains(StairsTile))
                        return true;
                }
                while (addedKeys);
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dgvCharacters.EndEdit();
            dgvItems.EndEdit();
            dgvWaypoints.EndEdit();
            (bool SolvableWithoutKeys, bool SolvableWithKeys) = IsFloorSolvable();

            if (!SolvableWithoutKeys)
            {
                MessageBox.Show("The current floor layout is not unsolvable with at least one Player/Stairs combination, even without considering keys and doors.\n\nPlease fix it and try to save again.", "Save Static Floor Geometry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!SolvableWithKeys)
            {
                MessageBox.Show("The current floor layout appears solvable, but at least one Player/Stairs combination is unsolvable considering keys and door.\n\nPlease fix it and try to save again.", "Save Static Floor Geometry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var inputBoxResult = string.Empty;
            var defaultName = !string.IsNullOrWhiteSpace(GeneratorToSave.Name) ? GeneratorToSave.Name : $"New Layout";
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

            var specialSpawns = new List<SpecialSpawnInfo>();
            var tileGeometryString = new StringBuilder();

            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                for (int x = 0; x < _tiles.GetLength(0); x++)
                {
                    var tile = _tiles[x, y];
                    var tileChar = '-';
                    if (tile.TileTypeId == "Empty")
                        tileChar = '-';
                    if (tile.TileTypeId == "Floor" || tile.TileTypeId == "Stairs")
                        tileChar = '.';
                    if (tile.TileTypeId == "Wall")
                        tileChar = '#';
                    if (tile.TileTypeId == "Hallway")
                        tileChar = '+';
                    tileGeometryString.Append(tileChar);
                    if (tile.TileType == RoomTileType.SpecialTileType)
                    {
                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = tile.Description
                        });
                    }
                    else if (tile.TileType == RoomTileType.Key || tile.TileType == RoomTileType.Door || tile.TileType == RoomTileType.Stairs)
                    {
                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = tile.Description
                        });
                    }
                    else if (tile.TileType == RoomTileType.Currency)
                    {
                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = tile.Description.Replace(ActiveDungeon.CurrencyInfo.Name, "Currency")
                        });
                    }
                    else if (tile.TileType == RoomTileType.Waypoint)
                    {
                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = EngineConstants.CREATE_WAYPOINT,
                            WaypointId = tile.WaypointId
                        });
                    }
                    else if (tile.TileType == RoomTileType.CharacterIncludingPlayer || tile.TileType == RoomTileType.NPC)
                    {
                        var spawnId = tile.Description;
                        if (RandomCharacterSpawnIds.Any(rcsi => rcsi.Value.Equals(spawnId)))
                        {
                            spawnId = RandomCharacterSpawnIds.First(kvp => kvp.Value == tile.Description).Key;
                        }

                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = spawnId,
                            Level = tile.Level
                        });
                    }
                    else if (tile.TileType == RoomTileType.Item)
                    {
                        var spawnId = tile.Description;
                        if (RandomItemSpawnIds.Any(rcsi => rcsi.Value.Equals(spawnId)))
                        {
                            spawnId = RandomItemSpawnIds.First(kvp => kvp.Value == tile.Description).Key;
                        }

                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = spawnId,
                            Level = tile.Level,
                            QualityLevel = tile.QualityLevel
                        });
                    }
                    else if (tile.TileType == RoomTileType.Trap)
                    {
                        var spawnId = tile.Description;
                        if (RandomTrapSpawnIds.Any(rcsi => rcsi.Value.Equals(spawnId)))
                        {
                            spawnId = RandomTrapSpawnIds.First(kvp => kvp.Value == tile.Description).Key;
                        }

                        specialSpawns.Add(new SpecialSpawnInfo
                        {
                            X = x + 1,
                            Y = y + 1,
                            SpawnId = spawnId,
                        });
                    }
                }
            }

            GeneratorToSave = new()
            {
                Name = inputBoxResult,
                StaticGenerator = new StaticGeneratorInfo
                {
                    FloorGeometry = tileGeometryString.ToString(),
                    SpecialSpawns = specialSpawns,
                    GenerateNPCsAfterFirstTurn = chkUseNPCGenerator.Checked,
                    GenerateItemsOnFirstTurn = chkUseItemGenerator.Checked,
                    GenerateTrapsOnFirstTurn = chkUseTrapGenerator.Checked
                }
            };

            Saved = true;
            this.Close();
        }
    }

    public class RoomTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string TileTypeId { get; set; }
        public RoomTileType TileType { get; set; }
        public ConsoleRepresentation BaseConsoleRepresentation { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public string QualityLevel { get; set; }
        public string WaypointId { get; set; }
    }

    public enum RoomTileType
    {
        NormalTileType,
        SpecialTileType,
        Key,
        Door,
        CharacterIncludingPlayer,
        NPC,
        Item,
        Trap,
        Currency,
        Stairs,
        Waypoint
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Clipboard;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.FloorInfos;
using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable CS8604 // Posible argumento de referencia nulo
namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class FloorGroupTab : UserControl
    {
        private List<EffectTypeData> EffectParamData;
        private List<RoomDispositionData> RoomDispositionData;
        private DungeonInfo ActiveDungeon;
        public FloorInfo OpenedFloorGroup { get; private set; }
        public FloorInfo LoadedFloorGroup { get; private set; }
        public event EventHandler TabInfoChanged;
        public FloorGroupTab()
        {
            InitializeComponent();
            ClipboardManager.ClipboardContentsChanged += ClipboardManager_ClipboardContentsChanged;
        }

        public void LoadData(DungeonInfo dungeon, FloorInfo floorGroup, List<EffectTypeData> effectParamData, List<RoomDispositionData> roomDispositionData)
        {
            ActiveDungeon = dungeon;
            OpenedFloorGroup = floorGroup;
            LoadedFloorGroup = floorGroup;
            EffectParamData = effectParamData;
            RoomDispositionData = roomDispositionData;
            nudMinFloorLevel.Minimum = 0;
            nudMinFloorLevel.Maximum = 99;
            nudMaxFloorLevel.Minimum = 0;
            nudMaxFloorLevel.Maximum = 99;
            nudMinFloorLevel.Value = LoadedFloorGroup.MinFloorLevel;
            nudMaxFloorLevel.Value = LoadedFloorGroup.MaxFloorLevel;
            nudWidth.Value = LoadedFloorGroup.Width;
            nudHeight.Value = LoadedFloorGroup.Height;
            lvFloorAlgorithms.Tag = null;
            btnNPCGenerator.Tag = new NPCGenerationParams
            {
                NPCList = LoadedFloorGroup.PossibleMonsters,
                MinNPCSpawnsAtStart = LoadedFloorGroup.SimultaneousMinMonstersAtStart,
                SimultaneousMaxNPCs = LoadedFloorGroup.SimultaneousMaxMonstersInFloor,
                TurnsPerNPCGeneration = LoadedFloorGroup.TurnsPerMonsterGeneration
            };
            btnItemGenerator.Tag = new ObjectGenerationParams
            {
                ObjectList = LoadedFloorGroup.PossibleItems,
                MinInFloor = LoadedFloorGroup.MinItemsInFloor,
                MaxInFloor = LoadedFloorGroup.MaxItemsInFloor
            };
            btnTrapGenerator.Tag = new ObjectGenerationParams
            {
                ObjectList = LoadedFloorGroup.PossibleTraps,
                MinInFloor = LoadedFloorGroup.MinTrapsInFloor,
                MaxInFloor = LoadedFloorGroup.MaxTrapsInFloor
            };
            cmbTilesets.Items.Clear();
            cmbTilesets.Items.AddRange(dungeon.TileSetInfos.Select(tileSet => tileSet.Id).ToArray());

            var selectedTilesetId = LoadedFloorGroup.TileSetId;
            if (dungeon.TileSetInfos.Exists(tileSet => tileSet.Id.Equals(selectedTilesetId)))
            {
                cmbTilesets.Text = selectedTilesetId;
            }
            SetSingleActionEditorParams(saeOnFloorStart, string.Empty, LoadedFloorGroup.OnFloorStart);

            chkGenerateStairsOnStart.Checked = LoadedFloorGroup.GenerateStairsOnStart;
            fklblStairsReminder.Visible = !chkGenerateStairsOnStart.Checked;
            RefreshGenerationAlgorithmList();
            nudMaxRoomConnections.Value = LoadedFloorGroup.MaxConnectionsBetweenRooms;
            nudExtraRoomConnectionOdds.Value = LoadedFloorGroup.OddsForExtraConnections;
            nudRoomFusionOdds.Value = LoadedFloorGroup.RoomFusionOdds;
            nudHungerLostPerTurn.Value = LoadedFloorGroup.HungerDegeneration;
        }

        public List<string> SaveData(bool saveAsNew)
        {
            var validationErrors = new List<string>();
            var floorLevelString = ((int)nudMinFloorLevel.Value != (int)nudMaxFloorLevel.Value)
                    ? $"{(int)nudMinFloorLevel.Value}-{(int)nudMaxFloorLevel.Value}"
                    : nudMinFloorLevel.Value.ToString();

            var intersectingFloorGroups = ActiveDungeon.FindIntersectingFloorGroups((int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value);
            var layoutList = new List<FloorLayoutGenerationInfo>((List<FloorLayoutGenerationInfo>)lvFloorAlgorithms.Tag);

            if (intersectingFloorGroups.Any(fi => saveAsNew || fi != OpenedFloorGroup))
                validationErrors.Add($"The Floor Level you chose, {floorLevelString}, intersects with at least another one.");

            if(string.IsNullOrWhiteSpace(cmbTilesets.Text))
                validationErrors.Add($"The Floor Group lacks a Tileset.");

            if(!layoutList.Any())
                validationErrors.Add($"The Floor Group lacks any possible Layouts.");

            if (!validationErrors.Any())
            {
                var npcGenerationParams = btnNPCGenerator.Tag as NPCGenerationParams;
                var itemGenerationParams = btnItemGenerator.Tag as ObjectGenerationParams;
                var trapGenerationParams = btnTrapGenerator.Tag as ObjectGenerationParams;
                LoadedFloorGroup = new()
                {
                    PossibleLayouts = layoutList,
                    MinFloorLevel = (int)nudMinFloorLevel.Value,
                    MaxFloorLevel = (int)nudMaxFloorLevel.Value,
                    TileSetId = cmbTilesets.Text,
                    Width = (int)nudWidth.Value,
                    Height = (int)nudHeight.Value,
                    GenerateStairsOnStart = chkGenerateStairsOnStart.Checked,
                    PossibleMonsters = npcGenerationParams.NPCList,
                    SimultaneousMinMonstersAtStart = npcGenerationParams.MinNPCSpawnsAtStart,
                    SimultaneousMaxMonstersInFloor = npcGenerationParams.SimultaneousMaxNPCs,
                    TurnsPerMonsterGeneration = npcGenerationParams.TurnsPerNPCGeneration,
                    PossibleItems = itemGenerationParams.ObjectList,
                    MinItemsInFloor = itemGenerationParams.MinInFloor,
                    MaxItemsInFloor = itemGenerationParams.MaxInFloor,
                    PossibleTraps = trapGenerationParams.ObjectList,
                    MinTrapsInFloor = trapGenerationParams.MinInFloor,
                    MaxTrapsInFloor = trapGenerationParams.MaxInFloor,
                    MaxConnectionsBetweenRooms = (int)nudMaxRoomConnections.Value,
                    OddsForExtraConnections = (int)nudExtraRoomConnectionOdds.Value,
                    RoomFusionOdds = (int)nudRoomFusionOdds.Value,
                    OnFloorStart = (!saeOnFloorStart.Action.IsNullOrEmpty()) ? saeOnFloorStart.Action : null,
                    HungerDegeneration = nudHungerLostPerTurn.Value
                };
            }

            return validationErrors;
        }

        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
        }

        private void RefreshGenerationAlgorithmList()
        {
            var layoutList = (List<FloorLayoutGenerationInfo>)lvFloorAlgorithms.Tag ?? null;
            if (layoutList == null)
            {
                layoutList = new List<FloorLayoutGenerationInfo>();
                foreach (var layout in LoadedFloorGroup.PossibleLayouts)
                {
                    layoutList.Add(new FloorLayoutGenerationInfo
                    {
                        Columns = layout.Columns,
                        Rows = layout.Rows,
                        MinRoomSize = new() { Width = layout.MinRoomSize.Width, Height = layout.MinRoomSize.Height },
                        MaxRoomSize = new() { Width = layout.MaxRoomSize.Width, Height = layout.MaxRoomSize.Height },
                        RoomDisposition = layout.RoomDisposition,
                        Name = layout.Name
                    });
                }
            }
            lvFloorAlgorithms.Tag = layoutList;
            lvFloorAlgorithms.Items.Clear();
            var algorithmIcons = new ImageList
            {
                ImageSize = new Size(64, 64),
                ColorDepth = ColorDepth.Depth32Bit
            };
            foreach (var layoutGenerator in layoutList)
            {
                var layoutThumbnail = ConstructLayoutThumbnail(layoutGenerator);
                algorithmIcons.Images.Add(layoutGenerator.Name, layoutThumbnail);
            }
            lvFloorAlgorithms.LargeImageList = algorithmIcons;
            foreach (var layoutGenerator in layoutList)
            {
                lvFloorAlgorithms.Items.Add(layoutGenerator.Name, layoutGenerator.Name);
            }
            nudMaxRoomConnections.Enabled = layoutList.Exists(pga => pga.Columns > 1 || pga.Rows > 1);
            if (nudMaxRoomConnections.Enabled)
            {
                nudMaxRoomConnections.Minimum = 1;
                nudMaxRoomConnections.Value = Math.Max(1, nudMaxRoomConnections.Value);
            }
            else
            {
                nudMaxRoomConnections.Minimum = 0;
                nudMaxRoomConnections.Value = 0;
            }
            nudExtraRoomConnectionOdds.Enabled = layoutList.Count > 1 && nudMaxRoomConnections.Value > 1;
            nudExtraRoomConnectionOdds.Value = !nudMaxRoomConnections.Enabled ? 0 : nudExtraRoomConnectionOdds.Value;
            nudRoomFusionOdds.Enabled = layoutList.Count > 1;
            nudRoomFusionOdds.Value = !nudRoomFusionOdds.Enabled ? 0 : nudRoomFusionOdds.Value;
            btnAddAlgorithm.Enabled = true;
            btnEditAlgorithm.Enabled = false;
            btnRemoveAlgorithm.Enabled = false;
        }

        private Image ConstructLayoutThumbnail(FloorLayoutGenerationInfo layout)
        {
            var rows = layout.Rows + layout.Rows - 1;
            var columns = layout.Columns + layout.Columns - 1;
            var width = 48 * columns;
            var height = 48 * rows;
            var thumbnail = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(thumbnail))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                // Fill it with Black to ensure it looks uniform
                graphics.Clear(Color.Black);
                for (int i = 0; i < layout.RoomDisposition.Length; i++)
                {
                    var tile = layout.RoomDisposition[i];
                    (int X, int Y) = ((int)Math.Floor((double)i / columns), i % columns);
                    var isHallwayTile = (X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0);
                    var roomDispositionData = RoomDispositionData.FirstOrDefault(rdd => rdd.RoomDispositionIndicator == tile.ToRoomDispositionIndicator(isHallwayTile));
                    graphics.DrawImage(roomDispositionData.TileImage, 48 * Y, 48 * X);
                }
            }

            return thumbnail;
        }

        private void nudMinFloorLevel_ValueChanged(object sender, EventArgs e)
        {
            nudMaxFloorLevel.Minimum = nudMinFloorLevel.Value;
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudMaxFloorLevel_ValueChanged(object sender, EventArgs e)
        {
            nudMinFloorLevel.Maximum = nudMaxFloorLevel.Value;
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudHungerLostPerTurn_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void ValidateOverlappingFloorGroupLevelsAndInformIfNeeded()
        {
            if (ActiveDungeon.HasOverlappingFloorInfosForLevels((int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, LoadedFloorGroup))
            {
                MessageBox.Show(
                    "Your current Floor Group's Level interval overlaps with another Floor Group's.\n\nPlease correct it.",
                    "Floor Group",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void nudMinFloorLevel_Leave(object sender, EventArgs e)
        {
            // Check if we are in design mode
            if (DesignMode || this.DesignMode || IsControlInDesignMode(this))
            {
                return; // Do nothing in design mode
            }
            ValidateOverlappingFloorGroupLevelsAndInformIfNeeded();
        }

        private void nudMaxFloorLevel_Leave(object sender, EventArgs e)
        {
            // Check if we are in design mode
            if (DesignMode || this.DesignMode || IsControlInDesignMode(this))
            {
                return; // Do nothing in design mode
            }
            ValidateOverlappingFloorGroupLevelsAndInformIfNeeded();
        }


        private bool IsControlInDesignMode(Control control)
        {
            if (control == null)
                return false;

            // Traverse up the control hierarchy to check if any parent is in design mode
            while (control != null)
            {
                if (control.Site != null && control.Site.DesignMode)
                {
                    return true;
                }
                control = control.Parent;
            }
            return false;
        }

        private void chkGenerateStairsOnStart_CheckedChanged(object sender, EventArgs e)
        {
            fklblStairsReminder.Visible = !chkGenerateStairsOnStart.Checked;
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnNPCGenerator_Click(object sender, EventArgs e)
        {
            var npcGenerationParams = btnNPCGenerator.Tag as NPCGenerationParams;
            var frmGeneratorWindow = new frmNPCGeneration(LoadedFloorGroup, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, npcGenerationParams, ActiveDungeon);
            frmGeneratorWindow.ShowDialog();
            if (frmGeneratorWindow.Saved)
            {
                btnNPCGenerator.Tag = frmGeneratorWindow.NPCGenerationParams;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void btnItemGenerator_Click(object sender, EventArgs e)
        {
            var itemGenerationParams = btnItemGenerator.Tag as ObjectGenerationParams;
            var frmGeneratorWindow = new frmObjectGeneration(LoadedFloorGroup, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, itemGenerationParams, EntityTypeForForm.Item, ActiveDungeon);
            frmGeneratorWindow.ShowDialog();
            if (frmGeneratorWindow.Saved)
            {
                btnItemGenerator.Tag = frmGeneratorWindow.ObjectGenerationParams;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void btnTrapGenerator_Click(object sender, EventArgs e)
        {
            var trapGenerationParams = btnTrapGenerator.Tag as ObjectGenerationParams;
            var frmGeneratorWindow = new frmObjectGeneration(LoadedFloorGroup, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, trapGenerationParams, EntityTypeForForm.Trap, ActiveDungeon);
            frmGeneratorWindow.ShowDialog();
            if (frmGeneratorWindow.Saved)
            {
                btnTrapGenerator.Tag = frmGeneratorWindow.ObjectGenerationParams;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void btnAddAlgorithm_Click(object sender, EventArgs e)
        {
            var frmFloorLayoutWindow = new frmFloorLayout(lvFloorAlgorithms.Tag as List<FloorLayoutGenerationInfo>, (int)nudWidth.Value, (int)nudHeight.Value, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, RoomDispositionData, null);
            frmFloorLayoutWindow.ShowDialog();
            if (frmFloorLayoutWindow.Saved)
            {
                if (lvFloorAlgorithms.Tag is not List<FloorLayoutGenerationInfo> layoutGenerators)
                {
                    layoutGenerators = new List<FloorLayoutGenerationInfo>();
                    layoutGenerators.AddRange(LoadedFloorGroup.PossibleLayouts);
                }
                layoutGenerators.Add(frmFloorLayoutWindow.GeneratorToSave);
                lvFloorAlgorithms.Tag = layoutGenerators;
                RefreshGenerationAlgorithmList();
                lvFloorAlgorithms.Items[lvFloorAlgorithms.Items.Count - 1].Selected = true;
                lvFloorAlgorithms.Select();
                lvFloorAlgorithms.EnsureVisible(lvFloorAlgorithms.Items.Count - 1);
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void btnEditAlgorithm_Click(object sender, EventArgs e)
        {
            var selectedIndex = lvFloorAlgorithms.SelectedIndices[0];
            if (lvFloorAlgorithms.Tag is not List<FloorLayoutGenerationInfo> layoutGenerators) return;
            var generatorToSave = layoutGenerators[lvFloorAlgorithms.SelectedIndices[0]];
            var frmFloorLayoutWindow = new frmFloorLayout(lvFloorAlgorithms.Tag as List<FloorLayoutGenerationInfo>, (int)nudWidth.Value, (int)nudHeight.Value, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, RoomDispositionData, generatorToSave);
            frmFloorLayoutWindow.ShowDialog();
            if (frmFloorLayoutWindow.Saved)
            {
                layoutGenerators = new List<FloorLayoutGenerationInfo>();
                layoutGenerators.AddRange(LoadedFloorGroup.PossibleLayouts);
                layoutGenerators[lvFloorAlgorithms.SelectedIndices[0]] = frmFloorLayoutWindow.GeneratorToSave;
                RefreshGenerationAlgorithmList();
                lvFloorAlgorithms.Items[selectedIndex].Selected = true;
                lvFloorAlgorithms.Select();
                lvFloorAlgorithms.EnsureVisible(selectedIndex);
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void btnRemoveAlgorithm_Click(object sender, EventArgs e)
        {
            if (lvFloorAlgorithms.Tag is not List<FloorLayoutGenerationInfo> floorLayout)
            {
                floorLayout = new List<FloorLayoutGenerationInfo>();
                floorLayout.AddRange(LoadedFloorGroup.PossibleLayouts);
            }
            var messageBoxResult = MessageBox.Show(
                $"Do you want to remove your currently-selected Floor Layout?",
                "Floor Layout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                floorLayout.RemoveAt(lvFloorAlgorithms.SelectedIndices[0]);
                lvFloorAlgorithms.Tag = floorLayout;
                RefreshGenerationAlgorithmList();
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void lvFloorAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddAlgorithm.Enabled = true;
            btnEditAlgorithm.Enabled = lvFloorAlgorithms.SelectedItems.Count > 0;
            btnCopyAlgorithm.Enabled = lvFloorAlgorithms.SelectedItems.Count > 0;
            btnRemoveAlgorithm.Enabled = lvFloorAlgorithms.SelectedItems.Count > 0;
        }

        private void btnCopyAlgorithm_Click(object sender, EventArgs e)
        {
            if (lvFloorAlgorithms.Tag is not List<FloorLayoutGenerationInfo> floorLayouts)
                return;
            ClipboardManager.Copy(FormConstants.LayoutClipboardKey, floorLayouts[lvFloorAlgorithms.SelectedIndices[0]]);
        }

        private void btnPasteAlgorithm_Click(object sender, EventArgs e)
        {
            if (lvFloorAlgorithms.Tag is not List<FloorLayoutGenerationInfo> floorLayouts)
                return;
            var copiedLayout = ClipboardManager.Paste<FloorLayoutGenerationInfo>(FormConstants.LayoutClipboardKey);
            if (floorLayouts.Any(fl => fl.Name.Equals(copiedLayout.Name)))
            {
                MessageBox.Show(
                    $"This Floor Group already has a Layout with the name {copiedLayout.Name}",
                    "Cannot Paste Floor Layout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            floorLayouts.Add(copiedLayout);
            RefreshGenerationAlgorithmList();
            ClipboardManager.RemoveData(FormConstants.LayoutClipboardKey);
            lvFloorAlgorithms.SelectedItems.Clear();
        }

        private void nudMaxRoomConnections_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            if (nudMaxRoomConnections.Value == 1)
            {
                nudExtraRoomConnectionOdds.Enabled = false;
                nudExtraRoomConnectionOdds.Value = 0;
            }
            else
            {
                nudExtraRoomConnectionOdds.Enabled = true;
            }
        }

        private void nudRoomConnectionOdds_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            if (nudExtraRoomConnectionOdds.Value == 0)
            {
                nudMaxRoomConnections.Enabled = false;
                nudMaxRoomConnections.Value = 1;
            }
            else
            {
                nudMaxRoomConnections.Enabled = true;
            }
        }

        private void nudRoomFusionOdds_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudExtraRoomConnectionOdds_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbTilesets_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void saeOnFloorStart_ActionContentsChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void ClipboardManager_ClipboardContentsChanged(object? sender, EventArgs e)
        {
            btnPasteAlgorithm.Enabled = ClipboardManager.ContainsData(FormConstants.LayoutClipboardKey);
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
#pragma warning restore CS8604 // Posible argumento de referencia nulo

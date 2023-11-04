using MathNet.Numerics.Distributions;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.FloorInfos;
using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion;
using RogueCustomsDungeonEditor.Validators;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsDungeonEditor.Controls;

namespace RogueCustomsDungeonEditor
{
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public partial class frmMain : Form
    {
        private readonly Dictionary<TabTypes, TabPage> TabsForNodeTypes = new();
        private readonly List<string> MandatoryLocaleKeys = new();
        private readonly List<string> BaseLocaleLanguages = new();
        private readonly LocaleInfo LocaleTemplate;
        private readonly List<FloorTypeData> BaseGeneratorAlgorithms = new();
        private readonly List<EffectTypeData> EffectParamData = new();
        private readonly Dictionary<string, string> BaseSightRangeDisplayNames = new Dictionary<string, string> {
            { "FullMap", "The whole map" },
            { "FullRoom", "The whole room" },
            { "FlatNumber", "A flat distance" }
        };

        private DungeonInfo ActiveDungeon;
        private NodeTag ActiveNodeTag;
        private TreeNode ActiveNode;

        private bool DirtyTab;
        private bool DirtyEntry;
        private bool DirtyDungeon;
        private bool AutomatedChange; // If true, does not set DirtyTab nor DirtyEntry to true
        private bool IsNewElement;
        private bool PassedValidation;
        private bool ReclickOnFailedSave;
        private bool ReselectingNode;

        private string PreviousTextBoxValue;
        private string PreviousItemType;
        private string DungeonPath;

        public frmMain()
        {
            InitializeComponent();
            TabsForNodeTypes[TabTypes.BasicInfo] = tpBasicInfo;
            TabsForNodeTypes[TabTypes.Locales] = tpLocales;
            TabsForNodeTypes[TabTypes.TileSetInfo] = tpTileSetInfos;
            TabsForNodeTypes[TabTypes.FloorInfo] = tpFloorInfos;
            TabsForNodeTypes[TabTypes.FactionInfo] = tpFactionInfos;
            TabsForNodeTypes[TabTypes.PlayerClass] = tpPlayerClass;
            TabsForNodeTypes[TabTypes.NPC] = tpNPC;
            TabsForNodeTypes[TabTypes.Item] = tpItem;
            TabsForNodeTypes[TabTypes.Trap] = tpTrap;
            TabsForNodeTypes[TabTypes.AlteredStatus] = tpAlteredStatus;
            TabsForNodeTypes[TabTypes.Validator] = tpValidation;
            tbTabs.TabPages.Clear();

            ofdDungeon.InitialDirectory = Application.StartupPath;
            sfdDungeon.InitialDirectory = Application.StartupPath;

            MandatoryLocaleKeys.AddRange(File.ReadAllLines("./Resources/MandatoryLocaleKeys.txt"));
            BaseLocaleLanguages.AddRange(File.ReadAllLines("./Resources/BaseLocaleLanguages.txt"));

            var jsonString = File.ReadAllText("./EffectInfos/EffectTypeData.json");
            EffectParamData = JsonSerializer.Deserialize<List<EffectTypeData>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            jsonString = File.ReadAllText("./FloorInfos/FloorTypeData.json");
            BaseGeneratorAlgorithms = JsonSerializer.Deserialize<List<FloorTypeData>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            jsonString = File.ReadAllText("./Resources/LocaleTemplate.json");
            LocaleTemplate = JsonSerializer.Deserialize<LocaleInfo>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var algorithmIcons = new ImageList
            {
                ImageSize = new Size(64, 64),
                ColorDepth = ColorDepth.Depth32Bit
            };
            foreach (var algorithm in BaseGeneratorAlgorithms)
            {
                algorithm.PreviewImage = Image.FromFile(algorithm.ImagePath);
                algorithmIcons.Images.Add(algorithm.InternalName, algorithm.PreviewImage);
            }
            lvFloorAlgorithms.LargeImageList = algorithmIcons;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DirtyDungeon)
            {
                var messageBoxResult = MessageBox.Show(
                    $"The current Dungeon has unsaved changes.\n\nDo you wish to save them before closing?",
                    "Exit",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.Yes)
                {
                    tsbSaveDungeon_Click(null, EventArgs.Empty);
                    if (!DirtyDungeon)  // If it's set back to false, it's that the Dungeon was saved.
                        Application.Exit();
                }
                else if (messageBoxResult == DialogResult.No)
                {
                    DirtyDungeon = false;
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #region Menu

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        #region TreeView

        private void tvDungeonInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (ReclickOnFailedSave)
            {
                ReclickOnFailedSave = false;
                return;
            }
            if (ActiveNode == e.Node) return;
            ReselectingNode = true;
            if (e.Node.Tag is NodeTag tag)
            {
                if (DirtyTab)
                {
                    var messageBoxResult = MessageBox.Show(
                        "The currently-opened Element has unsaved changes.\n\nDo you wish to save them?\n\n(Selecting \"No\" will make you lose all changes)",
                        "Unsaved Changes",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveElement();
                        if (DirtyTab)
                        {
                            if (ActiveNode != null)
                            {
                                ReclickOnFailedSave = true;
                                tvDungeonInfo.SelectedNode = ActiveNode;
                                tvDungeonInfo.SelectedNode.EnsureVisible();
                                tvDungeonInfo.Focus();
                                tvDungeonInfo.Refresh();
                            }
                            return;   // Don't switch nodes if saving failed.
                        }
                    }
                }
                foreach (TabPage tabPage in tbTabs.TabPages)
                {
                    if (tabPage != TabsForNodeTypes[TabTypes.Validator])
                        tbTabs.TabPages.Remove(tabPage);
                }
                tbTabs.TabPages.Insert(0, TabsForNodeTypes[tag.TabToOpen]);
                tbTabs.SelectedIndex = 0;
                var matchingNodes = tvDungeonInfo.Nodes.Find(e.Node.Text, true).Where(n => (n.Parent == null && e.Node.Parent == null) || n.Parent.Text.Equals(e.Node.Parent.Text)).ToList();
                if (matchingNodes.Any())
                {
                    matchingNodes[0].Parent?.Expand();
                    tvDungeonInfo.SelectedNode = matchingNodes[0];
                    tvDungeonInfo.SelectedNode.EnsureVisible();
                    tvDungeonInfo.Focus();
                }
                ActiveNode = e.Node;
                IsNewElement = false;
                LoadTabDataForTag(tag);
                tsbSaveElementAs.Visible = ActiveNodeTag.TabToOpen != TabTypes.BasicInfo;
                DirtyTab = false;
            }
            else
            {
                switch (e.Node.Text)
                {
                    case "Basic Information":
                        tssDungeonElement.Visible = false;
                        tsbAddElement.Visible = false;
                        tsbSaveElement.Visible = true;
                        tsbSaveElementAs.Visible = false;
                        tsbDeleteElement.Visible = false;
                        break;
                    case "Locales":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.Locales;
                        break;
                    case "Tilesets":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.TileSetInfo;
                        break;
                    case "Floor Groups":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.FloorInfo;
                        break;
                    case "Factions":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.FactionInfo;
                        break;
                    case "Player Classes":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.PlayerClass;
                        break;
                    case "NPCs":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.NPC;
                        break;
                    case "Items":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.Item;
                        break;
                    case "Traps":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.Trap;
                        break;
                    case "Altered Statuses":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == TabTypes.AlteredStatus;
                        break;
                }
            }
            ReselectingNode = false;
        }

        private void RefreshTreeNodes()
        {
            DirtyTab = false;
            tvDungeonInfo.Nodes.Clear();

            var basicInfoNode = new TreeNode("Basic Information")
            {
                Tag = new NodeTag { TabToOpen = TabTypes.BasicInfo, DungeonElement = null },
                Name = "Basic Information"
            };
            tvDungeonInfo.Nodes.Add(basicInfoNode);

            var localesRootNode = new TreeNode("Locales");
            foreach (var locale in ActiveDungeon.Locales)
            {
                var localeNode = new TreeNode(locale.Language)
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.Locales, DungeonElement = locale },
                    Name = locale.Language
                };
                localesRootNode.Nodes.Add(localeNode);
            }
            tvDungeonInfo.Nodes.Add(localesRootNode);

            var tileSetsRootNode = new TreeNode("Tilesets");
            foreach (var tileSetInfo in ActiveDungeon.TileSetInfos)
            {
                var tileSetInfoNode = new TreeNode(tileSetInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.TileSetInfo, DungeonElement = tileSetInfo },
                    Name = tileSetInfo.Id
                };
                tileSetsRootNode.Nodes.Add(tileSetInfoNode);
            }
            tvDungeonInfo.Nodes.Add(tileSetsRootNode);

            var floorInfosRootNode = new TreeNode("Floor Groups");
            foreach (var floorInfo in ActiveDungeon.FloorInfos.OrderBy(fi => fi.MinFloorLevel))
            {
                var floorInfoNode = new TreeNode()
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.FloorInfo, DungeonElement = floorInfo }
                };
                if (floorInfo.MinFloorLevel != floorInfo.MaxFloorLevel)
                    floorInfoNode.Text = $"Floors {floorInfo.MinFloorLevel}-{floorInfo.MaxFloorLevel}";
                else
                    floorInfoNode.Text = $"Floor {floorInfo.MinFloorLevel}";
                floorInfoNode.Name = floorInfoNode.Text;
                floorInfosRootNode.Nodes.Add(floorInfoNode);
            }
            tvDungeonInfo.Nodes.Add(floorInfosRootNode);

            var factionInfosRootNode = new TreeNode("Factions");
            foreach (var factionInfo in ActiveDungeon.FactionInfos)
            {
                var factionInfoNode = new TreeNode(factionInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.FactionInfo, DungeonElement = factionInfo },
                    Name = factionInfo.Id
                };
                factionInfosRootNode.Nodes.Add(factionInfoNode);
            }
            tvDungeonInfo.Nodes.Add(factionInfosRootNode);

            var playerClassRootNode = new TreeNode("Player Classes");
            foreach (var playerClass in ActiveDungeon.PlayerClasses)
            {
                var playerClassNode = new TreeNode($"{playerClass.ConsoleRepresentation.Character} - {playerClass.Id}")
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.PlayerClass, DungeonElement = playerClass }
                };
                playerClassNode.Name = playerClassNode.Text;
                playerClassRootNode.Nodes.Add(playerClassNode);
            }
            tvDungeonInfo.Nodes.Add(playerClassRootNode);

            var npcRootNode = new TreeNode("NPCs");
            foreach (var npc in ActiveDungeon.NPCs)
            {
                var npcNode = new TreeNode($"{npc.ConsoleRepresentation.Character} - {npc.Id}")
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.NPC, DungeonElement = npc }
                };
                npcNode.Name = npcNode.Text;
                npcRootNode.Nodes.Add(npcNode);
            }
            tvDungeonInfo.Nodes.Add(npcRootNode);

            var itemRootNode = new TreeNode("Items");
            foreach (var item in ActiveDungeon.Items)
            {
                var itemNode = new TreeNode($"{item.ConsoleRepresentation.Character} - {item.Id}")
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.Item, DungeonElement = item }
                };
                itemNode.Name = itemNode.Text;
                itemRootNode.Nodes.Add(itemNode);
            }
            tvDungeonInfo.Nodes.Add(itemRootNode);

            var trapRootNode = new TreeNode("Traps");
            foreach (var trap in ActiveDungeon.Traps)
            {
                var trapNode = new TreeNode($"{trap.ConsoleRepresentation.Character} - {trap.Id}")
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.Trap, DungeonElement = trap }
                };
                trapNode.Name = trapNode.Text;
                trapRootNode.Nodes.Add(trapNode);
            }
            tvDungeonInfo.Nodes.Add(trapRootNode);

            var alteredStatusRootNode = new TreeNode("Altered Statuses");
            foreach (var alteredStatus in ActiveDungeon.AlteredStatuses)
            {
                var alteredStatusNode = new TreeNode($"{alteredStatus.ConsoleRepresentation.Character} - {alteredStatus.Id}")
                {
                    Tag = new NodeTag { TabToOpen = TabTypes.AlteredStatus, DungeonElement = alteredStatus }
                };
                alteredStatusNode.Name = alteredStatusNode.Text;
                alteredStatusRootNode.Nodes.Add(alteredStatusNode);
            }
            tvDungeonInfo.Nodes.Add(alteredStatusRootNode);

            tvDungeonInfo.Focus();
        }

        private void SelectNodeIfExists(string nodeText, string parentNodeText)
        {
            if (ReselectingNode) return;
            var matchingNodes = tvDungeonInfo.Nodes.Find(nodeText, true).Where(n => (n.Parent == null && string.IsNullOrWhiteSpace(parentNodeText)) || n.Parent.Text.Equals(parentNodeText)).ToList();
            if (matchingNodes.Any())
            {
                matchingNodes[0].Parent?.Expand();
                tvDungeonInfo.SelectedNode = matchingNodes[0];
                tvDungeonInfo.SelectedNode.EnsureVisible();
                tvDungeonInfo.Focus();
            }
        }

        #endregion

        #region Toolbar

        private void tsbNewDungeon_Click(object sender, EventArgs e)
        {
            if (DirtyDungeon)
            {
                var messageBoxResult = MessageBox.Show(
                    $"The current Dungeon has unsaved changes.\n\nDo you wish to save them before closing?",
                    "New Dungeon",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.Cancel)
                {
                    return;
                }
                else if (messageBoxResult == DialogResult.Yes)
                {
                    tsbSaveDungeon_Click(null, EventArgs.Empty);
                    if (DirtyDungeon)  // If it's set back to false, it's that the Dungeon was saved.
                        return;
                }
            }
            ActiveDungeon = DungeonInfoHelpers.CreateEmptyDungeonTemplate(LocaleTemplate, BaseLocaleLanguages);
            ActiveDungeon.Version = Constants.CurrentDungeonJsonVersion;
            RefreshTreeNodes();
            tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
            tvDungeonInfo.Focus();
            tsbSaveDungeon.Visible = true;
            tsbSaveDungeonAs.Visible = true;
            tssElementValidate.Visible = true;
            tsbValidateDungeon.Visible = true;
            PassedValidation = false;
            this.Text = "Rogue Customs Dungeon Editor - [New Dungeon]";
            DirtyDungeon = false;
            DungeonPath = string.Empty;
            sfdDungeon.InitialDirectory = Application.StartupPath;
        }

        private void tsbOpenDungeon_Click(object sender, EventArgs e)
        {
            if (DirtyDungeon)
            {
                var messageBoxResult = MessageBox.Show(
                    $"The current Dungeon has unsaved changes.\n\nDo you wish to save them before closing?",
                    "Open Dungeon",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );
                if (messageBoxResult == DialogResult.Cancel)
                {
                    return;
                }
                else if (messageBoxResult == DialogResult.Yes)
                {
                    tsbSaveDungeon_Click(null, EventArgs.Empty);
                    if (DirtyDungeon)  // If it's set back to false, it's that the Dungeon was saved.
                        return;
                }
            }
            OpenDungeon();
        }

        private void OpenDungeon()
        {
            if (ofdDungeon.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var jsonString = File.ReadAllText(ofdDungeon.FileName);
                    ActiveDungeon = JsonSerializer.Deserialize<DungeonInfo>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                    var formerVersion = !string.IsNullOrWhiteSpace(ActiveDungeon.Version) ? new string(ActiveDungeon.Version) : "1.0";
                    ActiveDungeon = ActiveDungeon.ConvertDungeonInfoIfNeeded(jsonString, LocaleTemplate, MandatoryLocaleKeys);
                    ActiveDungeon.PruneNullActions();
                    tbTabs.TabPages.Clear();
                    DirtyEntry = false;
                    DirtyTab = false;
                    RefreshTreeNodes();
                    tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                    tsbSaveDungeon.Visible = true;
                    tsbSaveDungeonAs.Visible = true;
                    tssElementValidate.Visible = true;
                    tsbValidateDungeon.Visible = true;
                    PassedValidation = false;
                    DungeonPath = ofdDungeon.FileName;
                    sfdDungeon.InitialDirectory = Path.GetDirectoryName(ofdDungeon.FileName);
                    this.Text = $"Rogue Customs Dungeon Editor - [{Path.GetFileName(DungeonPath)}]";
                    if (!ActiveDungeon.Version.Equals(formerVersion))
                    {
                        MessageBox.Show($"Dungeon has been found to belong to outdated version {formerVersion}.\n\nDungeon has been updated to {ActiveDungeon.Version} with default values. Please check differences to ensure they work as intended, or adjust them if needed.\n\nRemember to save the Dungeon afterwards.", "Open Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DirtyDungeon = true;
                    }
                    else
                    {
                        DirtyDungeon = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Attempting to open this Dungeon threw an error:\n\n{ex.Message}\n\nAre you sure this is a valid Dungeon JSON file?", "Open Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsbSaveDungeon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DungeonPath))
                tsbSaveDungeonAs_Click(sender, e);
            else
                SaveDungeon(DungeonPath);
        }

        private void tsbSaveDungeonAs_Click(object sender, EventArgs e)
        {
            if (sfdDungeon.ShowDialog() == DialogResult.OK)
            {
                SaveDungeon(sfdDungeon.FileName);
            }
        }

        private void SaveDungeon(string filePath)
        {
            try
            {
                if (DirtyTab)
                {
                    var messageBoxResult = MessageBox.Show(
                        $"The currently-opened Element has unsaved changes.\n\nDo you wish to save them before saving the Dungeon?",
                        "Save Dungeon",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes && !SaveElement())
                    {
                        MessageBox.Show($"The currently-opened Element could not be saved due to errors. Please check it.\n\nThe Dungeon saving process will proceed nonetheless.", "Save Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                File.WriteAllText(filePath, string.Empty);
                using (var createStream = File.OpenWrite(filePath))
                {
                    JsonSerializer.Serialize(createStream, ActiveDungeon, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                }
                if (PassedValidation)
                    MessageBox.Show($"Dungeon has been successfully saved to {filePath}.", "Save Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"Dungeon has been successfully saved to {filePath}.\n\nHowever, it hasn't seemed to have passed Validation. Please Validate the Dungeon to check for potential game-breaking bugs.", "Save Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DungeonPath = filePath;
                sfdDungeon.InitialDirectory = Path.GetDirectoryName(filePath);
                this.Text = $"Rogue Customs Dungeon Editor - [{Path.GetFileName(DungeonPath)}]";
                DirtyDungeon = false;
                DirtyEntry = false;
                DirtyTab = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Attempting to save this Dungeon to {filePath} threw an error:\n\n{ex.Message}\n\nPlease check.", "Save Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbAddElement_Click(object sender, EventArgs e)
        {
            var tabToOpen = TabTypes.BasicInfo;
            if (tvDungeonInfo.SelectedNode?.Tag != null)
            {
                tabToOpen = ActiveNodeTag.TabToOpen;
            }
            else
            {
                switch (tvDungeonInfo.SelectedNode.Text)
                {
                    case "Locales":
                        tabToOpen = TabTypes.Locales;
                        break;
                    case "Tilesets":
                        tabToOpen = TabTypes.TileSetInfo;
                        break;
                    case "Floor Groups":
                        tabToOpen = TabTypes.FloorInfo;
                        break;
                    case "Factions":
                        tabToOpen = TabTypes.FactionInfo;
                        break;
                    case "Player Classes":
                        tabToOpen = TabTypes.PlayerClass;
                        break;
                    case "NPCs":
                        tabToOpen = TabTypes.NPC;
                        break;
                    case "Items":
                        tabToOpen = TabTypes.Item;
                        break;
                    case "Traps":
                        tabToOpen = TabTypes.Trap;
                        break;
                    case "Altered Statuses":
                        tabToOpen = TabTypes.AlteredStatus;
                        break;
                }
                tbTabs.TabPages.Clear();
                tbTabs.TabPages.Add(TabsForNodeTypes[tabToOpen]);
            }
            ActiveNodeTag = new NodeTag { TabToOpen = tabToOpen };
            switch (tabToOpen)
            {
                case TabTypes.Locales:
                    ActiveNodeTag.DungeonElement = LocaleTemplate.Clone(MandatoryLocaleKeys);
                    break;
                case TabTypes.TileSetInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateDefaultTileSet();
                    break;
                case TabTypes.FloorInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateFloorGroupTemplate();
                    break;
                case TabTypes.FactionInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateFactionTemplate();
                    break;
                case TabTypes.PlayerClass:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreatePlayerClassTemplate();
                    break;
                case TabTypes.NPC:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateNPCTemplate();
                    break;
                case TabTypes.Item:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateItemTemplate();
                    break;
                case TabTypes.Trap:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateTrapTemplate();
                    break;
                case TabTypes.AlteredStatus:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateAlteredStatusTemplate();
                    break;
                default:
                    break;
            }
            IsNewElement = true;
            ActiveNode = null;
            LoadTabDataForTag(ActiveNodeTag);
            DirtyTab = true;
        }

        private void tsbSaveElement_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
            SaveElement();
        }

        private bool SaveElement()
        {
            if (!IsNewElement)
            {
                switch (ActiveNodeTag.TabToOpen)
                {
                    case TabTypes.BasicInfo:
                        return SaveBasicInfo();
                    case TabTypes.Locales:
                        return SaveLocale();
                    case TabTypes.TileSetInfo:
                        return SaveTileSet(null);
                    case TabTypes.FloorInfo:
                        return SaveFloorGroup();
                    case TabTypes.FactionInfo:
                        return SaveFaction(null);
                    case TabTypes.PlayerClass:
                        return SavePlayerClass(null);
                    case TabTypes.NPC:
                        return SaveNPC(null);
                    case TabTypes.Item:
                        return SaveItem(null);
                    case TabTypes.Trap:
                        return SaveTrap(null);
                    case TabTypes.AlteredStatus:
                        return SaveAlteredStatus(null);
                    default:
                        return true;
                }
            }
            else
            {
                return SaveElementAs();
            }
        }

        private void tsbSaveElementAs_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
            SaveElementAs();
        }

        private bool SaveElementAs()
        {
            switch (ActiveNodeTag.TabToOpen)
            {
                case TabTypes.Locales:
                    return SaveLocaleAs();
                case TabTypes.TileSetInfo:
                    return SaveTileSetAs();
                case TabTypes.FloorInfo:
                    return SaveFloorGroupAs();
                case TabTypes.FactionInfo:
                    return SaveFactionAs();
                case TabTypes.PlayerClass:
                    return SavePlayerClassAs();
                case TabTypes.NPC:
                    return SaveNPCAs();
                case TabTypes.Item:
                    return SaveItemAs();
                case TabTypes.Trap:
                    return SaveTrapAs();
                case TabTypes.AlteredStatus:
                    return SaveAlteredStatusAs();
                default:
                    return true;
            }
        }

        private void tsbDeleteElement_Click(object sender, EventArgs e)
        {
            switch (ActiveNodeTag.TabToOpen)
            {
                case TabTypes.Locales:
                    DeleteLocale();
                    break;
                case TabTypes.TileSetInfo:
                    DeleteTileSet();
                    break;
                case TabTypes.FloorInfo:
                    DeleteFloorGroup();
                    break;
                case TabTypes.FactionInfo:
                    DeleteFaction();
                    break;
                case TabTypes.PlayerClass:
                    DeletePlayerClass();
                    break;
                case TabTypes.NPC:
                    DeleteNPC();
                    break;
                case TabTypes.Item:
                    DeleteItem();
                    break;
                case TabTypes.Trap:
                    DeleteTrap();
                    break;
                case TabTypes.AlteredStatus:
                    DeleteAlteredStatus();
                    break;
                default:
                    break;
            }
        }

        private void tsbValidateDungeon_Click(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.Validator]))
                tbTabs.TabPages.Add(TabsForNodeTypes[TabTypes.Validator]);
            tbTabs.SelectTab(TabsForNodeTypes[TabTypes.Validator]);
            ValidateDungeon();
        }

        #endregion

        #region Tabs

        private void LoadTabDataForTag(NodeTag tag)
        {
            ActiveNodeTag = tag;
            tsbAddElement.Visible = true;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = true;
            tsbDeleteElement.Visible = true;
            tssElementValidate.Visible = true;
            DirtyTab = false;
            switch (tag.TabToOpen)
            {
                case TabTypes.BasicInfo:
                    LoadBasicInfo();
                    break;
                case TabTypes.Locales:
                    var tagLocale = (LocaleInfo)tag.DungeonElement;
                    LoadLocaleInfoFor(tagLocale);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Locale - {tagLocale.Language}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Locale";
                    dgvLocales.Rows[0].Selected = true;
                    break;
                case TabTypes.TileSetInfo:
                    var tagTileSet = (TileSetInfo)tag.DungeonElement;
                    LoadTileSetInfoFor(tagTileSet);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Tileset - {tagTileSet.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Tileset";
                    break;
                case TabTypes.FloorInfo:
                    var tagFloorGroup = (FloorInfo)tag.DungeonElement;
                    LoadFloorInfoFor(tagFloorGroup);
                    if (!IsNewElement)
                    {
                        if (tagFloorGroup.MinFloorLevel != tagFloorGroup.MaxFloorLevel)
                            TabsForNodeTypes[tag.TabToOpen].Text = $"Floor Group - {tagFloorGroup.MinFloorLevel} to {tagFloorGroup.MaxFloorLevel}";
                        else
                            TabsForNodeTypes[tag.TabToOpen].Text = $"Floor Group - {tagFloorGroup.MinFloorLevel}";
                    }
                    else
                    {
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Floor Group";
                    }

                    break;
                case TabTypes.FactionInfo:
                    var tagFaction = (FactionInfo)tag.DungeonElement;
                    LoadFactionInfoFor(tagFaction);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Faction - {tagFaction.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Faction";
                    break;
                case TabTypes.PlayerClass:
                    var tagPlayerClass = (PlayerClassInfo)tag.DungeonElement;
                    LoadPlayerClassInfoFor(tagPlayerClass);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Player Class - {tagPlayerClass.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Player Class";
                    break;
                case TabTypes.NPC:
                    var tagNPC = (NPCInfo)tag.DungeonElement;
                    LoadNPCInfoFor(tagNPC);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"NPC - {tagNPC.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"NPC";
                    break;
                case TabTypes.Item:
                    var tagItem = (ItemInfo)tag.DungeonElement;
                    LoadItemInfoFor(tagItem);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Item - {tagItem.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Item";
                    break;
                case TabTypes.Trap:
                    var tagTrap = (TrapInfo)tag.DungeonElement;
                    LoadTrapInfoFor(tagTrap);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Trap - {tagTrap.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Trap";
                    break;
                case TabTypes.AlteredStatus:
                    var tagAlteredStatus = (AlteredStatusInfo)tag.DungeonElement;
                    LoadAlteredStatusInfoFor(tagAlteredStatus);
                    if (!IsNewElement)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Altered Status - {tagAlteredStatus.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Altered Status";
                    break;
                default:
                    break;
            }
            tbTabs_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void tbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbTabs.SelectedTab == null)
            {
                tssDungeonElement.Visible = false;
                tsbAddElement.Visible = false;
                tsbSaveElement.Visible = false;
                tsbSaveElementAs.Visible = false;
                tsbDeleteElement.Visible = false;
                tssElementValidate.Visible = false;
                tsbValidateDungeon.Visible = false;
            }
            else if (tbTabs.SelectedTab.Text.Equals("Basic Information"))
            {
                tssDungeonElement.Visible = true;
                tsbAddElement.Visible = false;
                tsbSaveElement.Visible = true;
                tsbSaveElementAs.Visible = false;
                tsbDeleteElement.Visible = false;
                tssElementValidate.Visible = true;
                tsbValidateDungeon.Visible = true;
            }
            else if (tbTabs.SelectedTab.Text.Equals("Validation Results"))
            {
                tssDungeonElement.Visible = false;
                tsbAddElement.Visible = false;
                tsbSaveElement.Visible = false;
                tsbSaveElementAs.Visible = false;
                tsbDeleteElement.Visible = false;
                tssElementValidate.Visible = true;
                tsbValidateDungeon.Visible = true;
            }
            else
            {
                tssDungeonElement.Visible = true;
                tsbAddElement.Visible = true;
                tsbSaveElement.Visible = true;
                tsbSaveElementAs.Visible = true;
                tsbDeleteElement.Visible = true;
                tssElementValidate.Visible = true;
                tsbValidateDungeon.Visible = true;
            }
        }

        #endregion

        #region Basic Information

        private void LoadBasicInfo()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
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
            if (!AutomatedChange) DirtyTab = true;
            txtDungeonName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblDungeonNameLocale);
        }

        private void txtAuthor_TextChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
            txtAuthor.ToggleEntryInLocaleWarning(ActiveDungeon, fklblAuthorLocale);
        }

        private void txtWelcomeMessage_TextChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
            txtWelcomeMessage.ToggleEntryInLocaleWarning(ActiveDungeon, fklblWelcomeMessageLocale);
        }

        private void txtEndingMessage_TextChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
            txtEndingMessage.ToggleEntryInLocaleWarning(ActiveDungeon, fklblEndingMessageLocale);
        }

        private void nudAmountOfFloors_ValueChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        private void cmbDefaultLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        private bool SaveBasicInfo()
        {
            ActiveDungeon.Name = txtDungeonName.Text;
            ActiveDungeon.Author = txtAuthor.Text;
            ActiveDungeon.WelcomeMessage = txtWelcomeMessage.Text;
            ActiveDungeon.EndingMessage = txtEndingMessage.Text;
            ActiveDungeon.DefaultLocale = cmbDefaultLocale.Text;
            MessageBox.Show(
                "Dungeon's Basic Information has been successfully updated!",
                "Save Basic Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        #endregion

        #region Shared Between Tabs

        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.AlteredStatuses = ActiveDungeon.AlteredStatuses.Where(als => !als.Id.Equals(classId)).Select(als => als.Id).ToList();
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
            sae.ActionContentsChanged += (_, _) => DirtyTab = true;
        }
        private void SetMultiActionEditorParams(MultiActionEditor mae, string classId, List<ActionWithEffectsInfo> actions)
        {
            mae.Actions = actions;
            mae.AlteredStatuses = ActiveDungeon.AlteredStatuses.Where(als => !als.Id.Equals(classId)).Select(als => als.Id).ToList();
            mae.Dungeon = ActiveDungeon;
            mae.EffectParamData = EffectParamData;
            mae.ActionContentsChanged += (_, _) => DirtyTab = true;
        }

        #endregion

        #region Locales

        private void LoadLocaleInfoFor(LocaleInfo localeInfo)
        {
            var localeClone = localeInfo.Clone(MandatoryLocaleKeys);
            if (localeClone.AddMissingMandatoryLocalesIfNeeded(LocaleTemplate, MandatoryLocaleKeys))
            {
                DirtyTab = true;
                MessageBox.Show(
                    "This Locale is missing some mandatory keys.\n\nThey have been added at the end of the table. Please check them.",
                    "Locale",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            if (AddMissingCustomLocalesIfNeeded(localeClone))
            {
                DirtyTab = true;
                MessageBox.Show(
                    "This Locale is missing some custom keys present in other Locales.\n\nThey have been added at the end of the table. Please check them.",
                    "Locale",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            dgvLocales.Tag = localeClone;
            dgvLocales.Rows.Clear();
            foreach (var entry in localeClone.LocaleStrings)
            {
                dgvLocales.Rows.Add(entry.Key, entry.Value);
            }
        }

        private bool AddMissingCustomLocalesIfNeeded(LocaleInfo localeInfo)
        {
            var customEntriesList = new List<LocaleInfoString>();
            foreach (var locale in ActiveDungeon.Locales.Where(l => l != localeInfo))
            {
                foreach (var localeString in locale.LocaleStrings)
                {
                    if (MandatoryLocaleKeys.Contains(localeString.Key)) continue;
                    customEntriesList.Add(localeString);
                }
            }
            customEntriesList = customEntriesList
                          .GroupBy(ck => ck.Key)
                          .Select(ck => ck.First())
                          .ToList();
            var missingCustomEntries = customEntriesList.Where(ck => !localeInfo.LocaleStrings.Exists(ls => ls.Key.Equals(ck.Key))).ToList();
            foreach (var missingEntry in missingCustomEntries)
            {
                var customLocaleEntry = customEntriesList.Find(ck => ck.Key.Equals(missingEntry.Key));
                localeInfo.LocaleStrings.Add(new LocaleInfoString
                {
                    Key = customLocaleEntry.Key,
                    Value = customLocaleEntry.Value
                });
            }
            return missingCustomEntries.Any();
        }

        private void dgvLocales_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLocales.SelectedRows.Count == 0) return;
            AutomatedChange = true;
            var key = dgvLocales.SelectedRows[0].Cells[0].Value.ToString();
            var locale = (LocaleInfo)dgvLocales.Tag;
            if (DirtyEntry)
            {
                var messageBoxResult = MessageBox.Show(
                    $"Do you want to save your current Locale Entry changes before moving to Entry {key}?",
                    "Locale",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (messageBoxResult == DialogResult.Yes)
                {
                    var localeStringToUpdate = locale.LocaleStrings.Find(ls => ls.Key.Equals(txtLocaleEntryValue.Text));
                    if (localeStringToUpdate != null)
                    {
                        localeStringToUpdate.Value = txtLocaleEntryValue.Text;
                        dgvLocales.Rows[locale.LocaleStrings.IndexOf(localeStringToUpdate)].SetValues(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
                    }
                    else
                    {
                        locale.LocaleStrings.Add(new LocaleInfoString
                        {
                            Key = txtLocaleEntryKey.Text,
                            Value = txtLocaleEntryValue.Text.Replace(Environment.NewLine, "\n")
                        });
                        dgvLocales.Rows.Add(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
                    }
                    DirtyTab = true;
                }
            }
            var localeString = locale.LocaleStrings.Find(ls => ls.Key.Equals(key));
            dgvLocales.CurrentCell = dgvLocales.SelectedRows[0].Cells[0];
            if (localeString != null)
            {
                SetLocaleStringForEdit(localeString);
            }
            AutomatedChange = false;
            DirtyEntry = false;
        }

        private void SetLocaleStringForEdit(LocaleInfoString localeString)
        {
            txtLocaleEntryKey.Text = localeString.Key;
            txtLocaleEntryKey.Enabled = true;
            txtLocaleEntryValue.Text = localeString.Value.Replace("\n", Environment.NewLine);
            txtLocaleEntryValue.Enabled = true;
        }

        private void txtLocaleEntryKey_TextChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyEntry = true;
            var locale = (LocaleInfo)dgvLocales.Tag;
            var keyExistsInLocale = locale.LocaleStrings.Exists(ls => ls.Key == txtLocaleEntryKey.Text);
            btnDeleteLocale.Enabled = !MandatoryLocaleKeys.Contains(txtLocaleEntryKey.Text) && keyExistsInLocale;
            btnAddLocale.Enabled = !keyExistsInLocale;
            btnUpdateLocale.Enabled = keyExistsInLocale;

            if (keyExistsInLocale)
            {
                var missingLanguages = new List<string>();
                foreach (var localeToCheck in ActiveDungeon.Locales)
                {
                    if (!localeToCheck.Language.Equals(locale.Language) && !localeToCheck.LocaleStrings.Exists(ls => ls.Key == txtLocaleEntryKey.Text))
                    {
                        missingLanguages.Add(localeToCheck.Language);
                    }
                }

                if (missingLanguages.Any())
                {
                    fklblMissingLocale.Visible = true;
                    fklblMissingLocale.Text = $"This locale is missing in the following languages:\n{string.Join(", ", missingLanguages)}";
                }
                else
                {
                    fklblMissingLocale.Visible = false;
                }
            }
        }

        private void txtLocaleEntryValue_TextChanged(object sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyEntry = true;
        }

        private void btnUpdateLocale_Click(object sender, EventArgs e)
        {
            var locale = (LocaleInfo)dgvLocales.Tag;
            var localeString = locale.LocaleStrings.Find(ls => ls.Key == txtLocaleEntryKey.Text);
            if (localeString != null)
            {
                localeString.Value = txtLocaleEntryValue.Text;
                dgvLocales.Rows[locale.LocaleStrings.IndexOf(localeString)].SetValues(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
                DirtyEntry = false;
                AutomatedChange = true;
                txtLocaleEntryKey_TextChanged(this, EventArgs.Empty);
                AutomatedChange = false;
                DirtyTab = true;
            }
            else
            {
                btnAddLocale_Click(null, EventArgs.Empty);
            }
        }

        private void btnAddLocale_Click(object sender, EventArgs e)
        {
            var locale = (LocaleInfo)dgvLocales.Tag;
            locale.LocaleStrings.Add(new LocaleInfoString
            {
                Key = txtLocaleEntryKey.Text,
                Value = txtLocaleEntryValue.Text.Replace(Environment.NewLine, "\n")
            });
            DirtyEntry = false;
            dgvLocales.Rows.Add(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
            dgvLocales.Rows[^1].Selected = true;
            AutomatedChange = true;
            txtLocaleEntryKey_TextChanged(this, EventArgs.Empty);
            AutomatedChange = false;
            DirtyTab = true;
        }

        private void btnDeleteLocale_Click(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show(
                $"Are you sure you want to delete the locale Entry {dgvLocales.SelectedRows[0].Cells[0].Value}?",
                "Delete Locale Entry",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                AutomatedChange = true;
                var locale = (LocaleInfo)dgvLocales.Tag;
                locale.LocaleStrings.RemoveAll(ls => ls.Key.Equals(dgvLocales.SelectedRows[0].Cells[0].Value));
                dgvLocales.Rows.RemoveAt(dgvLocales.SelectedRows[0].Index);
                dgvLocales.Rows[^1].Selected = true;
                AutomatedChange = false;
                DirtyTab = true;
                DirtyEntry = false;
            }
        }

        private bool PromptLocaleUpdate(LocaleInfo locale)
        {
            var messageBoxResult = MessageBox.Show(
                $"Are you sure you want to overwrite the existing values in Locale {locale.Language}?",
                "Update Locale",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                var preExistingLocaleIndex = ActiveDungeon.Locales.FindIndex(l => l.Language.Equals(locale.Language));
                ActiveDungeon.Locales[preExistingLocaleIndex] = ((LocaleInfo)dgvLocales.Tag).Clone(MandatoryLocaleKeys);
                MessageBox.Show(
                    $"Locale {locale.Language} has been successfully updated!",
                    "Update Locale",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                DirtyDungeon = true;
                DirtyTab = false;
                IsNewElement = false;
                PassedValidation = false;
                RefreshTreeNodes();
                SelectNodeIfExists(locale.Language, "Locales");
                return true;
            }
            return false;
        }

        private bool SaveLocale()
        {
            var locale = (LocaleInfo)dgvLocales.Tag;
            PromptLocaleUpdate(locale);
            return true;
        }

        private bool SaveLocaleAs()
        {
            string inputBoxResult;
            do
            {
                inputBoxResult = InputBox.Show("Indicate the Locale Identifier. It must be exactly two characters long.\n\n(For example, \"en\" or \"es\")", "Save Locale As");
                if (inputBoxResult == null) return false;
                if (inputBoxResult.Length > 2)
                {
                    MessageBox.Show(
                        $"{inputBoxResult} is too long of a name for a locale.\n\nIt must be exactly two characters long.",
                        "Create new Locale",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                    );
                }
            }
            while (inputBoxResult.Length > 2);
            if (!string.IsNullOrWhiteSpace(inputBoxResult))
            {
                IsNewElement = false;
                var preExistingLocale = ActiveDungeon.Locales.Find(l => l.Language.Equals(inputBoxResult));
                if (preExistingLocale == null)
                {
                    var newLocale = ((LocaleInfo)dgvLocales.Tag).Clone(MandatoryLocaleKeys);
                    newLocale.Language = inputBoxResult;
                    ActiveDungeon.Locales.Add(newLocale);
                    MessageBox.Show(
                        $"Locale {inputBoxResult} has been successfully created!",
                        "Create Locale",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    DirtyDungeon = true;
                    DirtyTab = false;
                    IsNewElement = false;
                    PassedValidation = false;
                    RefreshTreeNodes();
                    SelectNodeIfExists(inputBoxResult, "Locales");
                    return true;
                }
                else
                {
                    return PromptLocaleUpdate(preExistingLocale);
                }
            }
            return false;
        }

        private void DeleteLocale()
        {
            var activeLocale = (LocaleInfo)dgvLocales.Tag;
            var deleteLocalePrompt = IsNewElement
                ? "Do you want to remove this unsaved Locale?"
                : $"Do you want to PERMANENTLY delete Locale \"{activeLocale.Language}\"?";
            var messageBoxResult = MessageBox.Show(
                deleteLocalePrompt,
                "Locale",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    ActiveDungeon.Locales.RemoveAll(l => l.Language.Equals(activeLocale.Language));
                    MessageBox.Show(
                        $"Locale {activeLocale.Language} has been successfully deleted.",
                        "Delete Locale",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    if (ActiveDungeon.DefaultLocale.Equals(activeLocale.Language))
                    {
                        ActiveDungeon.DefaultLocale = string.Empty;
                        MessageBox.Show(
                            $"Locale {activeLocale.Language} was the Dungeon's Default Locale.\n\nSet up a new one to replace it.",
                            "Delete Locale",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }
                IsNewElement = false;
                DirtyEntry = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        #endregion

        #region Tileset

        private void LoadTileSetInfoFor(TileSetInfo tileSet)
        {
            csrTopLeftWall.SetConsoleRepresentation(tileSet.TopLeftWall);
            csrTopRightWall.SetConsoleRepresentation(tileSet.TopRightWall);
            csrBottomLeftWall.SetConsoleRepresentation(tileSet.BottomLeftWall);
            csrBottomRightWall.SetConsoleRepresentation(tileSet.BottomRightWall);
            csrHorizontalWall.SetConsoleRepresentation(tileSet.HorizontalWall);
            csrConnectorWall.SetConsoleRepresentation(tileSet.ConnectorWall);
            csrVerticalWall.SetConsoleRepresentation(tileSet.VerticalWall);
            csrTopLeftHallway.SetConsoleRepresentation(tileSet.TopLeftHallway);
            csrTopRightHallway.SetConsoleRepresentation(tileSet.TopRightHallway);
            csrBottomLeftHallway.SetConsoleRepresentation(tileSet.BottomLeftHallway);
            csrBottomRightHallway.SetConsoleRepresentation(tileSet.BottomRightHallway);
            csrHorizontalHallway.SetConsoleRepresentation(tileSet.HorizontalHallway);
            csrHorizontalBottomHallway.SetConsoleRepresentation(tileSet.HorizontalBottomHallway);
            csrHorizontalTopHallway.SetConsoleRepresentation(tileSet.HorizontalTopHallway);
            csrVerticalHallway.SetConsoleRepresentation(tileSet.VerticalHallway);
            csrVerticalLeftHallway.SetConsoleRepresentation(tileSet.VerticalLeftHallway);
            csrVerticalRightHallway.SetConsoleRepresentation(tileSet.VerticalRightHallway);
            csrCentralHallway.SetConsoleRepresentation(tileSet.CentralHallway);
            csrFloor.SetConsoleRepresentation(tileSet.Floor);
            csrStairs.SetConsoleRepresentation(tileSet.Stairs);
            csrEmpty.SetConsoleRepresentation(tileSet.Empty);
        }

        private bool SaveTileSet(string id)
        {
            if (!ValidateTileSetDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Tileset. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Tileset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var tileSet = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.TileSetInfos.Find(tsi => tsi.Id.Equals(id)) ?? new TileSetInfo() { Id = id }
                : (TileSetInfo)ActiveNodeTag.DungeonElement;
            tileSet.TopLeftWall = csrTopLeftWall.ConsoleRepresentation;
            tileSet.TopRightWall = csrTopRightWall.ConsoleRepresentation;
            tileSet.BottomLeftWall = csrBottomLeftWall.ConsoleRepresentation;
            tileSet.BottomRightWall = csrBottomRightWall.ConsoleRepresentation;
            tileSet.HorizontalWall = csrHorizontalWall.ConsoleRepresentation;
            tileSet.ConnectorWall = csrConnectorWall.ConsoleRepresentation;
            tileSet.VerticalWall = csrVerticalWall.ConsoleRepresentation;
            tileSet.TopLeftHallway = csrTopLeftHallway.ConsoleRepresentation;
            tileSet.TopRightHallway = csrTopRightHallway.ConsoleRepresentation;
            tileSet.BottomLeftHallway = csrBottomLeftHallway.ConsoleRepresentation;
            tileSet.BottomRightHallway = csrBottomRightHallway.ConsoleRepresentation;
            tileSet.HorizontalHallway = csrHorizontalHallway.ConsoleRepresentation;
            tileSet.HorizontalBottomHallway = csrHorizontalBottomHallway.ConsoleRepresentation;
            tileSet.HorizontalTopHallway = csrHorizontalTopHallway.ConsoleRepresentation;
            tileSet.VerticalHallway = csrVerticalHallway.ConsoleRepresentation;
            tileSet.VerticalLeftHallway = csrVerticalLeftHallway.ConsoleRepresentation;
            tileSet.VerticalRightHallway = csrVerticalRightHallway.ConsoleRepresentation;
            tileSet.CentralHallway = csrCentralHallway.ConsoleRepresentation;
            tileSet.Floor = csrFloor.ConsoleRepresentation;
            tileSet.Stairs = csrStairs.ConsoleRepresentation;
            tileSet.Empty = csrEmpty.ConsoleRepresentation;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.TileSetInfos.Exists(tsi => tsi.Id.Equals(id)))
            {
                ActiveDungeon.TileSetInfos.Add(tileSet);
                MessageBox.Show(
                    $"Tileset {id} has been successfully created!",
                    "Create Tileset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"Tileset {tileSet.Id} has been successfully updated!",
                    "Update Tileset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            SelectNodeIfExists(tileSet.Id, "Tilesets");
            PassedValidation = false;
            return true;
        }

        private bool SaveTileSetAs()
        {
            if (!ValidateTileSetDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Tileset. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Tileset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the Tileset Identifier", "Save Tileset As");
            if (inputBoxResult != null)
            {
                var saveResult = false;
                if (ActiveDungeon.TileSetInfos.Exists(tsi => tsi.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Tileset with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Tileset Status",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        saveResult = SaveTileSet(inputBoxResult);
                    }
                }
                else
                {
                    saveResult = SaveTileSet(inputBoxResult);
                }
                var savedTileSet = ActiveDungeon.AlteredStatuses.Find(tsi => tsi.Id.Equals(inputBoxResult));
                if (savedTileSet != null)
                {
                    SelectNodeIfExists(savedTileSet.Id, "Tilesets");
                }
                return saveResult;
            }
            return false;
        }

        private bool ValidateTileSetDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            errorMessages.AddRange(csrTopLeftWall.ConsoleRepresentation.Validate("Top Left Wall"));
            errorMessages.AddRange(csrTopRightWall.ConsoleRepresentation.Validate("Top Right Wall"));
            errorMessages.AddRange(csrBottomLeftWall.ConsoleRepresentation.Validate("Bottom Left Wall"));
            errorMessages.AddRange(csrBottomRightWall.ConsoleRepresentation.Validate("Bottom Right Wall"));
            errorMessages.AddRange(csrHorizontalWall.ConsoleRepresentation.Validate("Horizontal Wall"));
            errorMessages.AddRange(csrConnectorWall.ConsoleRepresentation.Validate("Connector Wall"));
            errorMessages.AddRange(csrVerticalWall.ConsoleRepresentation.Validate("Vertical Wall"));
            errorMessages.AddRange(csrTopLeftHallway.ConsoleRepresentation.Validate("Top Left Hallway"));
            errorMessages.AddRange(csrTopRightHallway.ConsoleRepresentation.Validate("Top Right Hallway"));
            errorMessages.AddRange(csrBottomLeftHallway.ConsoleRepresentation.Validate("Bottom Left Hallway"));
            errorMessages.AddRange(csrBottomRightHallway.ConsoleRepresentation.Validate("Bottom Right Hallway"));
            errorMessages.AddRange(csrHorizontalHallway.ConsoleRepresentation.Validate("Horizontal Hallway"));
            errorMessages.AddRange(csrHorizontalBottomHallway.ConsoleRepresentation.Validate("Horizontal Bottom Hallway"));
            errorMessages.AddRange(csrHorizontalTopHallway.ConsoleRepresentation.Validate("Horizontal Top Hallway"));
            errorMessages.AddRange(csrVerticalHallway.ConsoleRepresentation.Validate("Vertical Hallway"));
            errorMessages.AddRange(csrVerticalLeftHallway.ConsoleRepresentation.Validate("Vertical Left Hallway"));
            errorMessages.AddRange(csrVerticalRightHallway.ConsoleRepresentation.Validate("Vertical Right Hallway"));
            errorMessages.AddRange(csrCentralHallway.ConsoleRepresentation.Validate("Central Hallway"));
            errorMessages.AddRange(csrFloor.ConsoleRepresentation.Validate("Floor"));
            errorMessages.AddRange(csrStairs.ConsoleRepresentation.Validate("Stairs"));
            errorMessages.AddRange(csrEmpty.ConsoleRepresentation.Validate("Empty (inaccessible)"));

            return !errorMessages.Any();
        }

        public void DeleteTileSet()
        {
            var activeTileSet = (TileSetInfo)ActiveNodeTag.DungeonElement;
            var deleteTileSetPrompt = IsNewElement
                ? "Do you want to remove this unsaved Tileset?"
                : $"Do you want to PERMANENTLY delete Tileset {activeTileSet.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteTileSetPrompt,
                "Tileset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activeTileSet.Id);
                    ActiveDungeon.TileSetInfos.RemoveAll(tsi => tsi.Id.Equals(activeTileSet.Id));
                    MessageBox.Show(
                        $"Tileset {removedId} has been successfully deleted.",
                        "Delete Tileset",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void csrTopLeftWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrTopRightWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrVerticalWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrBottomLeftWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrBottomRightWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrHorizontalWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrVerticalConnectorWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrConnectorWall_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrTopLeftHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrTopRightHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrVerticalHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrBottomLeftHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrBottomRightHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrHorizontalHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrHorizontalBottomHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrHorizontalTopHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrVerticalLeftHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrVerticalRightHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrCentralHallway_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrFloor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrStairs_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void csrEmpty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        #endregion

        #region Floor Group

        private void LoadFloorInfoFor(FloorInfo floorGroup)
        {
            nudMinFloorLevel.Minimum = 0;
            nudMinFloorLevel.Maximum = 99;
            nudMaxFloorLevel.Minimum = 0;
            nudMaxFloorLevel.Maximum = 99;
            nudMinFloorLevel.Value = floorGroup.MinFloorLevel;
            nudMaxFloorLevel.Value = floorGroup.MaxFloorLevel;
            nudWidth.Value = floorGroup.Width;
            nudHeight.Value = floorGroup.Height;
            lvFloorAlgorithms.Tag = null;
            btnNPCGenerator.Tag = new NPCGenerationParams
            {
                NPCList = floorGroup.PossibleMonsters,
                MinNPCSpawnsAtStart = floorGroup.SimultaneousMinMonstersAtStart,
                SimultaneousMaxNPCs = floorGroup.SimultaneousMaxMonstersInFloor,
                TurnsPerNPCGeneration = floorGroup.TurnsPerMonsterGeneration
            };
            btnItemGenerator.Tag = new ObjectGenerationParams
            {
                ObjectList = floorGroup.PossibleItems,
                MinInFloor = floorGroup.MinItemsInFloor,
                MaxInFloor = floorGroup.MaxItemsInFloor
            };
            btnTrapGenerator.Tag = new ObjectGenerationParams
            {
                ObjectList = floorGroup.PossibleTraps,
                MinInFloor = floorGroup.MinTrapsInFloor,
                MaxInFloor = floorGroup.MaxTrapsInFloor
            };
            cmbTilesets.Items.Clear();
            cmbTilesets.Items.AddRange(ActiveDungeon.TileSetInfos.Select(tileSet => tileSet.Id).ToArray());

            var selectedTilesetId = floorGroup.TileSetId;
            if (ActiveDungeon.TileSetInfos.Any(tileSet => tileSet.Id.Equals(selectedTilesetId)))
            {
                cmbTilesets.Text = selectedTilesetId;
            }
            SetSingleActionEditorParams(saeOnFloorStart, string.Empty, floorGroup.OnFloorStart);

            chkGenerateStairsOnStart.Checked = floorGroup.GenerateStairsOnStart;
            fklblStairsReminder.Visible = !chkGenerateStairsOnStart.Checked;
            RefreshGenerationAlgorithmList();
            nudMaxRoomConnections.Value = floorGroup.MaxConnectionsBetweenRooms;
            nudExtraRoomConnectionOdds.Value = floorGroup.OddsForExtraConnections;
            nudRoomFusionOdds.Value = floorGroup.RoomFusionOdds;
        }

        private bool ValidateFloorGroupDataForSave(out List<string> errorMessages)
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            errorMessages = new List<string>();

            if ((lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>)?.Any() != true)
                errorMessages.Add("This Floor Group has been given no Floor Generation Algorithms.");
            if (string.IsNullOrEmpty(cmbTilesets.Text))
                errorMessages.Add("This Floor Group lacks a Tileset.");
            if (IsOverlappingWithOtherFloorInfos((int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, activeFloorGroup))
                errorMessages.Add("This Floor Group overlaps with another in at least one Floor Level.");

            return !errorMessages.Any();
        }

        private bool SaveFloorGroup()
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            if (!ValidateFloorGroupDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Floor Group. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Floor Group",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            return PromptFloorInfoUpdate(activeFloorGroup);
        }

        private bool SaveFloorGroupAs()
        {
            if (!ValidateFloorGroupDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Floor Group. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Floor Group",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            return PromptFloorInfoUpdate(null);
        }

        private bool IsOverlappingWithOtherFloorInfos(int minFloor, int maxFloor, FloorInfo floorGroup)
        {
            foreach (var floorGroupToCheck in ActiveDungeon.FloorInfos)
            {
                if (floorGroup == floorGroupToCheck) continue;
                if (IntHelpers.DoIntervalsIntersect(minFloor, maxFloor, floorGroupToCheck.MinFloorLevel, floorGroupToCheck.MaxFloorLevel))
                    return true;
            }
            return false;
        }

        private bool PromptFloorInfoUpdate(FloorInfo floorGroup)
        {
            string floorLevelString = string.Empty;
            var messageBoxResult = DialogResult.Yes;
            var saveAsNew = floorGroup == null;

            if (floorGroup != null)
            {
                floorLevelString = (floorGroup.MinFloorLevel != floorGroup.MaxFloorLevel)
                    ? $"{floorGroup.MinFloorLevel}-{floorGroup.MaxFloorLevel}"
                    : floorGroup.MinFloorLevel.ToString();
                messageBoxResult = MessageBox.Show(
                    $"Are you sure you want to overwrite the existing values in Floor Group {floorLevelString}?",
                    "Update Locale",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
            }

            if (messageBoxResult == DialogResult.Yes)
            {
                var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
                if (floorGroup == null)
                {
                    floorGroup = activeFloorGroup.Clone();
                }
                floorGroup.MinFloorLevel = (int)nudMinFloorLevel.Value;
                floorGroup.MaxFloorLevel = (int)nudMaxFloorLevel.Value;
                floorGroup.TileSetId = cmbTilesets.Text;
                floorGroup.Width = (int)nudWidth.Value;
                floorGroup.Height = (int)nudHeight.Value;
                floorGroup.GenerateStairsOnStart = chkGenerateStairsOnStart.Checked;
                var npcGenerationParams = btnNPCGenerator.Tag as NPCGenerationParams;
                floorGroup.PossibleMonsters = npcGenerationParams.NPCList;
                floorGroup.SimultaneousMinMonstersAtStart = npcGenerationParams.MinNPCSpawnsAtStart;
                floorGroup.SimultaneousMaxMonstersInFloor = npcGenerationParams.SimultaneousMaxNPCs;
                floorGroup.TurnsPerMonsterGeneration = npcGenerationParams.TurnsPerNPCGeneration;
                var itemGenerationParams = btnItemGenerator.Tag as ObjectGenerationParams;
                floorGroup.PossibleItems = itemGenerationParams.ObjectList;
                floorGroup.MinItemsInFloor = itemGenerationParams.MinInFloor;
                floorGroup.MaxItemsInFloor = itemGenerationParams.MaxInFloor;
                var trapGenerationParams = btnTrapGenerator.Tag as ObjectGenerationParams;
                floorGroup.PossibleTraps = trapGenerationParams.ObjectList;
                floorGroup.MinTrapsInFloor = trapGenerationParams.MinInFloor;
                floorGroup.MaxTrapsInFloor = trapGenerationParams.MaxInFloor;
                floorGroup.PossibleGeneratorAlgorithms = lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>;
                floorGroup.MaxConnectionsBetweenRooms = (int)nudMaxRoomConnections.Value;
                floorGroup.OddsForExtraConnections = (int)nudExtraRoomConnectionOdds.Value;
                floorGroup.RoomFusionOdds = (int)nudRoomFusionOdds.Value;
                floorGroup.OnFloorStart = (!saeOnFloorStart.Action.IsNullOrEmpty()) ? saeOnFloorStart.Action : null;

                if (saveAsNew)
                {
                    ActiveDungeon.FloorInfos.Add(floorGroup);
                    ActiveDungeon.FloorInfos = ActiveDungeon.FloorInfos.OrderBy(fi => fi.MinFloorLevel).ToList();
                }

                ActiveDungeon.AmountOfFloors = ActiveDungeon.FloorInfos.Max(fi => fi.MaxFloorLevel);

                floorLevelString = (floorGroup.MinFloorLevel != floorGroup.MaxFloorLevel)
                    ? $"{floorGroup.MinFloorLevel}-{floorGroup.MaxFloorLevel}"
                    : floorGroup.MinFloorLevel.ToString();

                MessageBox.Show(
                    $"Floor Group {floorLevelString} has been successfully saved!",
                    "Save Floor Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                var floorInfoNodeText = string.Empty;
                if (floorGroup.MinFloorLevel != floorGroup.MaxFloorLevel)
                    floorInfoNodeText = $"Floors {floorGroup.MinFloorLevel}-{floorGroup.MaxFloorLevel}";
                else
                    floorInfoNodeText = $"Floor {floorGroup.MinFloorLevel}";

                DirtyDungeon = true;
                DirtyTab = false;
                IsNewElement = false;
                RefreshTreeNodes();
                SelectNodeIfExists(floorInfoNodeText, "Floor Groups");
                return true;
            }
            return false;
        }

        private void DeleteFloorGroup()
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var floorLevelString = (activeFloorGroup.MinFloorLevel != activeFloorGroup.MaxFloorLevel)
                    ? $"{activeFloorGroup.MinFloorLevel}-{activeFloorGroup.MaxFloorLevel}"
                    : activeFloorGroup.MinFloorLevel.ToString();
            var deleteFloorGroupPrompt = IsNewElement
                ? "Do you want to remove this unsaved Floor Group?"
                : $"Do you want to PERMANENTLY delete Floor Group {floorLevelString}?";
            var messageBoxResult = MessageBox.Show(
                deleteFloorGroupPrompt,
                "Floor Group",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    ActiveDungeon.FloorInfos.RemoveAll(fi => fi.MinFloorLevel == activeFloorGroup.MinFloorLevel && fi.MaxFloorLevel == activeFloorGroup.MaxFloorLevel);
                    MessageBox.Show(
                        $"Floor Group {floorLevelString} has been successfully deleted.",
                        "Delete Floor Group",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void RefreshGenerationAlgorithmList()
        {
            var algorithmList = (List<GeneratorAlgorithmInfo>)lvFloorAlgorithms.Tag ?? ((FloorInfo)ActiveNodeTag.DungeonElement).PossibleGeneratorAlgorithms;
            lvFloorAlgorithms.Tag = algorithmList;
            lvFloorAlgorithms.Items.Clear();
            foreach (var generationAlgorithm in algorithmList)
            {
                lvFloorAlgorithms.Items.Add($"{generationAlgorithm.Name}\n{generationAlgorithm.Columns}c x {generationAlgorithm.Rows}r", generationAlgorithm.Name);
            }
            nudMaxRoomConnections.Enabled = algorithmList.Exists(pga => pga.Columns > 1 || pga.Rows > 1);
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
            nudExtraRoomConnectionOdds.Enabled = algorithmList.Count > 1 && nudMaxRoomConnections.Value > 1;
            nudExtraRoomConnectionOdds.Value = !nudMaxRoomConnections.Enabled ? 0 : nudExtraRoomConnectionOdds.Value;
            nudRoomFusionOdds.Enabled = algorithmList.Count > 1;
            nudRoomFusionOdds.Value = !nudRoomFusionOdds.Enabled ? 0 : nudRoomFusionOdds.Value;
            btnAddAlgorithm.Enabled = true;
            btnEditAlgorithm.Enabled = false;
            btnRemoveAlgorithm.Enabled = false;
        }

        private void nudMinFloorLevel_ValueChanged(object sender, EventArgs e)
        {
            nudMaxFloorLevel.Minimum = nudMinFloorLevel.Value;
            DirtyTab = true;
        }

        private void nudMaxFloorLevel_ValueChanged(object sender, EventArgs e)
        {
            nudMinFloorLevel.Maximum = nudMaxFloorLevel.Value;
            DirtyTab = true;
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void ValidateOverlappingFloorGroupLevelsAndInformIfNeeded()
        {
            if (ActiveDungeon.HasOverlappingFloorInfosForLevels((int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, (FloorInfo)ActiveNodeTag.DungeonElement))
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
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.FloorInfo])) return;
            ValidateOverlappingFloorGroupLevelsAndInformIfNeeded();
        }

        private void nudMaxFloorLevel_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.FloorInfo])) return;
            ValidateOverlappingFloorGroupLevelsAndInformIfNeeded();
        }

        private void chkGenerateStairsOnStart_CheckedChanged(object sender, EventArgs e)
        {
            fklblStairsReminder.Visible = !chkGenerateStairsOnStart.Checked;
            DirtyTab = true;
        }

        private void btnNPCGenerator_Click(object sender, EventArgs e)
        {
            var floorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var npcGenerationParams = btnNPCGenerator.Tag as NPCGenerationParams;
            var frmGeneratorWindow = new frmNPCGeneration(floorGroup, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, npcGenerationParams, ActiveDungeon);
            frmGeneratorWindow.ShowDialog();
            if (frmGeneratorWindow.Saved)
            {
                btnNPCGenerator.Tag = frmGeneratorWindow.NPCGenerationParams;
                DirtyTab = true;
            }
        }

        private void btnItemGenerator_Click(object sender, EventArgs e)
        {
            var floorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var itemGenerationParams = btnItemGenerator.Tag as ObjectGenerationParams;
            var frmGeneratorWindow = new frmObjectGeneration(floorGroup, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, itemGenerationParams, EntityTypeForForm.Item, ActiveDungeon);
            frmGeneratorWindow.ShowDialog();
            if (frmGeneratorWindow.Saved)
            {
                btnItemGenerator.Tag = frmGeneratorWindow.ObjectGenerationParams;
                DirtyTab = true;
            }
        }

        private void btnTrapGenerator_Click(object sender, EventArgs e)
        {
            var floorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var trapGenerationParams = btnTrapGenerator.Tag as ObjectGenerationParams;
            var frmGeneratorWindow = new frmObjectGeneration(floorGroup, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, trapGenerationParams, EntityTypeForForm.Trap, ActiveDungeon);
            frmGeneratorWindow.ShowDialog();
            if (frmGeneratorWindow.Saved)
            {
                btnTrapGenerator.Tag = frmGeneratorWindow.ObjectGenerationParams;
                DirtyTab = true;
            }
        }

        private void btnAddAlgorithm_Click(object sender, EventArgs e)
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var frmAlgorithmWindow = new frmFloorGeneratorAlgorithm(activeFloorGroup, (int)nudWidth.Value, (int)nudHeight.Value, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, null, BaseGeneratorAlgorithms);
            frmAlgorithmWindow.ShowDialog();
            if (frmAlgorithmWindow.Saved)
            {
                if (lvFloorAlgorithms.Tag is not List<GeneratorAlgorithmInfo> generatorAlgorithms)
                {
                    generatorAlgorithms = new List<GeneratorAlgorithmInfo>();
                    generatorAlgorithms.AddRange(activeFloorGroup.PossibleGeneratorAlgorithms);
                }
                generatorAlgorithms.Add(frmAlgorithmWindow.AlgorithmToSave);
                lvFloorAlgorithms.Tag = generatorAlgorithms;
                RefreshGenerationAlgorithmList();
                lvFloorAlgorithms.Items[lvFloorAlgorithms.Items.Count - 1].Selected = true;
                lvFloorAlgorithms.Select();
                lvFloorAlgorithms.EnsureVisible(lvFloorAlgorithms.Items.Count - 1);
                DirtyTab = true;
            }
        }

        private void btnEditAlgorithm_Click(object sender, EventArgs e)
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var selectedIndex = lvFloorAlgorithms.SelectedIndices[0];
            var generatorAlgorithms = lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>;
            var algorithmToSave = generatorAlgorithms[lvFloorAlgorithms.SelectedIndices[0]];
            var frmAlgorithmWindow = new frmFloorGeneratorAlgorithm(activeFloorGroup, (int)nudWidth.Value, (int)nudHeight.Value, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, algorithmToSave, BaseGeneratorAlgorithms);
            frmAlgorithmWindow.ShowDialog();
            if (frmAlgorithmWindow.Saved)
            {
                if (generatorAlgorithms == null)
                {
                    generatorAlgorithms = new List<GeneratorAlgorithmInfo>();
                    generatorAlgorithms.AddRange(activeFloorGroup.PossibleGeneratorAlgorithms);
                }
                generatorAlgorithms[lvFloorAlgorithms.SelectedIndices[0]] = frmAlgorithmWindow.AlgorithmToSave;
                RefreshGenerationAlgorithmList();
                lvFloorAlgorithms.Items[selectedIndex].Selected = true;
                lvFloorAlgorithms.Select();
                lvFloorAlgorithms.EnsureVisible(selectedIndex);
                DirtyTab = true;
            }
        }

        private void btnRemoveAlgorithm_Click(object sender, EventArgs e)
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            if (lvFloorAlgorithms.Tag is not List<GeneratorAlgorithmInfo> generatorAlgorithms)
            {
                generatorAlgorithms = new List<GeneratorAlgorithmInfo>();
                generatorAlgorithms.AddRange(activeFloorGroup.PossibleGeneratorAlgorithms);
            }
            var messageBoxResult = MessageBox.Show(
                $"Do you want to remove your currently-selected Floor Algorithm?",
                "Floor Algorithm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                generatorAlgorithms.RemoveAt(lvFloorAlgorithms.SelectedIndices[0]);
                lvFloorAlgorithms.Tag = generatorAlgorithms;
                RefreshGenerationAlgorithmList();
                DirtyTab = true;
            }
        }

        private void lvFloorAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddAlgorithm.Enabled = true;
            btnEditAlgorithm.Enabled = lvFloorAlgorithms.SelectedItems.Count > 0;
            btnRemoveAlgorithm.Enabled = lvFloorAlgorithms.SelectedItems.Count > 0;
        }

        private void nudMaxRoomConnections_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
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
            DirtyTab = true;
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
            DirtyTab = true;
        }

        private void cmbTilesets_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        #endregion

        #region Faction

        public void LoadFactionInfoFor(FactionInfo faction)
        {
            txtFactionName.Text = faction.Name;
            txtFactionName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionNameLocale);
            txtFactionDescription.Text = faction.Description;
            txtFactionDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionDescriptionLocale);
            lbAllies.Items.Clear();
            lbEnemies.Items.Clear();
            lbNeutrals.Items.Clear();
            foreach (var otherFaction in ActiveDungeon.FactionInfos)
            {
                if (faction.Id.Equals(otherFaction.Id)) continue;
                if (faction.AlliedWith.Contains(otherFaction.Id))
                    lbAllies.Items.Add(otherFaction.Id);
                else if (faction.EnemiesWith.Contains(otherFaction.Id))
                    lbEnemies.Items.Add(otherFaction.Id);
                else
                    lbNeutrals.Items.Add(otherFaction.Id);
            }
            btnAlliesToNeutrals.Enabled = lbAllies.Items.Count > 0;
            btnNeutralsToAllies.Enabled = lbNeutrals.Items.Count > 0;
            btnNeutralsToEnemies.Enabled = lbNeutrals.Items.Count > 0;
            btnEnemiesToNeutrals.Enabled = lbEnemies.Items.Count > 0;
        }

        public bool ValidateFactionDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtFactionName.Text))
                errorMessages.Add("Enter a Faction Name first.");
            if (string.IsNullOrWhiteSpace(txtFactionDescription.Text))
                errorMessages.Add("Enter a Faction Description first.");
            return !errorMessages.Any();
        }

        public bool SaveFaction(string id)
        {
            if (!ValidateFactionDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Faction. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var faction = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.FactionInfos.Find(fi => fi.Id.Equals(id)) ?? new FactionInfo() { Id = id }
                : (FactionInfo)ActiveNodeTag.DungeonElement;
            faction.Name = txtFactionName.Text;
            faction.Description = txtFactionDescription.Text;
            faction.AlliedWith = new List<string>();
            foreach (string allyId in lbAllies.Items)
            {
                faction.AlliedWith.Add(allyId);
                var alliedFaction = ActiveDungeon.FactionInfos.Find(fi => fi.Id.Equals(allyId));
                if (!alliedFaction.AlliedWith.Contains(faction.Id))
                    alliedFaction.AlliedWith.Add(faction.Id);
                if (alliedFaction.NeutralWith.Contains(faction.Id))
                    alliedFaction.NeutralWith.Remove(faction.Id);
                if (alliedFaction.EnemiesWith.Contains(faction.Id))
                    alliedFaction.EnemiesWith.Remove(faction.Id);
            }
            faction.NeutralWith = new List<string>();
            foreach (string neutralId in lbNeutrals.Items)
            {
                faction.NeutralWith.Add(neutralId);
                var neutralFaction = ActiveDungeon.FactionInfos.Find(fi => fi.Id.Equals(neutralId));
                if (neutralFaction.AlliedWith.Contains(faction.Id))
                    neutralFaction.AlliedWith.Remove(faction.Id);
                if (!neutralFaction.NeutralWith.Contains(faction.Id))
                    neutralFaction.NeutralWith.Add(faction.Id);
                if (neutralFaction.EnemiesWith.Contains(faction.Id))
                    neutralFaction.EnemiesWith.Remove(faction.Id);
            }
            faction.EnemiesWith = new List<string>();
            foreach (string enemyId in lbEnemies.Items)
            {
                faction.EnemiesWith.Add(enemyId);
                var enemyFaction = ActiveDungeon.FactionInfos.Find(fi => fi.Id.Equals(enemyId));
                if (enemyFaction.AlliedWith.Contains(faction.Id))
                    enemyFaction.AlliedWith.Remove(faction.Id);
                if (enemyFaction.NeutralWith.Contains(faction.Id))
                    enemyFaction.NeutralWith.Remove(faction.Id);
                if (!enemyFaction.EnemiesWith.Contains(faction.Id))
                    enemyFaction.EnemiesWith.Add(faction.Id);
            }

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.FactionInfos.Exists(fi => fi.Id.Equals(id)))
            {
                ActiveDungeon.FactionInfos.Add(faction);
                MessageBox.Show(
                    $"Faction {id} has been successfully created!\n\n(Keep in mind that Factions whose allegiances you've changed have also changed)",
                    "Create Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"Faction {faction.Id} has been successfully updated!\n\n(Keep in mind that Factions whose allegiances you've changed have also changed)",
                    "Update Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            SelectNodeIfExists(id ?? faction.Id, "Factions");
            PassedValidation = false;
            return true;
        }

        public bool SaveFactionAs()
        {
            if (!ValidateFactionDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Faction. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the Faction Identifier", "Save Faction As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.FactionInfos.Exists(fi => fi.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A faction with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Faction",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveFaction(inputBoxResult);
                        SelectNodeIfExists(inputBoxResult, "Factions");
                    }
                }
                else
                {
                    SaveFaction(inputBoxResult);
                    if (ActiveDungeon.FactionInfos.Exists(fi => fi.Id.Equals(inputBoxResult)))
                        SelectNodeIfExists(inputBoxResult, "Factions");
                }
                return true;
            }
            return false;
        }

        public void DeleteFaction()
        {
            var activeFaction = (FactionInfo)ActiveNodeTag.DungeonElement;
            var deleteFactionPrompt = IsNewElement
                ? "Do you want to remove this unsaved Faction?"
                : $"Do you want to PERMANENTLY delete Faction {activeFaction.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteFactionPrompt,
                "Faction",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activeFaction.Id);
                    ActiveDungeon.FactionInfos.RemoveAll(fi => fi.Id.Equals(activeFaction.Id));
                    foreach (var faction in ActiveDungeon.FactionInfos)
                    {
                        faction.AlliedWith.RemoveAll(a => a.Equals(removedId));
                        faction.NeutralWith.RemoveAll(n => n.Equals(removedId));
                        faction.EnemiesWith.RemoveAll(e => e.Equals(removedId));
                    }
                    MessageBox.Show(
                        $"Faction {removedId} has been successfully deleted.",
                        "Delete Faction",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void txtFactionName_TextChanged(object sender, EventArgs e)
        {
            txtFactionName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionNameLocale);
            DirtyTab = true;
        }

        private void txtFactionDescription_TextChanged(object sender, EventArgs e)
        {
            txtFactionDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionDescriptionLocale);
            DirtyTab = true;
        }

        private void lbAllies_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAllyToNeutral.Enabled = lbAllies.SelectedItem != null;
        }

        private void lbNeutrals_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNeutralToAlly.Enabled = lbNeutrals.SelectedItem != null;
            btnNeutralToEnemy.Enabled = lbNeutrals.SelectedItem != null;
        }

        private void lbEnemies_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEnemyToNeutral.Enabled = lbEnemies.SelectedItem != null;
        }

        private void btnNeutralToAlly_Click(object sender, EventArgs e)
        {
            lbAllies.Items.Add(lbNeutrals.SelectedItem);
            lbNeutrals.Items.Remove(lbNeutrals.SelectedItem);
            DirtyTab = true;
            btnNeutralToAlly.Enabled = false;
        }

        private void btnAllyToNeutral_Click(object sender, EventArgs e)
        {
            lbNeutrals.Items.Add(lbAllies.SelectedItem);
            lbAllies.Items.Remove(lbAllies.SelectedItem);
            DirtyTab = true;
            btnAllyToNeutral.Enabled = false;
        }

        private void btnEnemyToNeutral_Click(object sender, EventArgs e)
        {
            lbNeutrals.Items.Add(lbEnemies.SelectedItem);
            lbEnemies.Items.Remove(lbEnemies.SelectedItem);
            DirtyTab = true;
            btnEnemyToNeutral.Enabled = false;
        }

        private void btnNeutralToEnemy_Click(object sender, EventArgs e)
        {
            lbEnemies.Items.Add(lbNeutrals.SelectedItem);
            lbNeutrals.Items.Remove(lbNeutrals.SelectedItem);
            DirtyTab = true;
            btnNeutralToEnemy.Enabled = false;
        }

        private void btnNeutralsToAllies_Click(object sender, EventArgs e)
        {
            var neutralsToRemove = new List<string>();
            foreach (string neutralId in lbNeutrals.Items)
            {
                lbAllies.Items.Add(neutralId);
                neutralsToRemove.Add(neutralId);
            }
            foreach (var neutralToRemove in neutralsToRemove)
            {
                lbNeutrals.Items.Remove(neutralToRemove);
            }
            DirtyTab = true;
            btnNeutralsToAllies.Enabled = false;
            btnNeutralToAlly.Enabled = false;
        }

        private void btnAlliesToNeutrals_Click(object sender, EventArgs e)
        {
            var alliesToRemove = new List<string>();
            foreach (string allyId in lbAllies.Items)
            {
                lbNeutrals.Items.Add(allyId);
                alliesToRemove.Add(allyId);
            }
            foreach (var allyToRemove in alliesToRemove)
            {
                lbAllies.Items.Remove(allyToRemove);
            }
            DirtyTab = true;
            btnAllyToNeutral.Enabled = false;
            btnAlliesToNeutrals.Enabled = false;
        }

        private void btnEnemiesToNeutrals_Click(object sender, EventArgs e)
        {
            var enemiesToRemove = new List<string>();
            foreach (string enemyId in lbEnemies.Items)
            {
                lbNeutrals.Items.Add(enemyId);
                enemiesToRemove.Add(enemyId);
            }
            foreach (var enemyToRemove in enemiesToRemove)
            {
                lbEnemies.Items.Remove(enemyToRemove);
            }
            DirtyTab = true;
            btnEnemiesToNeutrals.Enabled = false;
            btnEnemyToNeutral.Enabled = false;
        }

        private void btnNeutralsToEnemies_Click(object sender, EventArgs e)
        {
            var neutralsToRemove = new List<string>();
            foreach (string neutralId in lbNeutrals.Items)
            {
                lbEnemies.Items.Add(neutralId);
                neutralsToRemove.Add(neutralId);
            }
            foreach (var neutralToRemove in neutralsToRemove)
            {
                lbNeutrals.Items.Remove(neutralToRemove);
            }
            DirtyTab = true;
            btnNeutralsToEnemies.Enabled = false;
            btnNeutralToEnemy.Enabled = false;
        }

        #endregion

        #region Player Class

        private void LoadPlayerClassInfoFor(PlayerClassInfo playerClass)
        {
            txtPlayerClassName.Text = playerClass.Name;
            chkRequirePlayerPrompt.Checked = playerClass.RequiresNamePrompt;
            txtPlayerClassDescription.Text = playerClass.Description;
            try
            {
                crsPlayer.Character = playerClass.ConsoleRepresentation.Character;
                crsPlayer.BackgroundColor = playerClass.ConsoleRepresentation.BackgroundColor;
                crsPlayer.ForegroundColor = playerClass.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsPlayer.Character = '\0';
                crsPlayer.BackgroundColor = new GameColor(Color.Black);
                crsPlayer.ForegroundColor = new GameColor(Color.White);
            }
            cmbPlayerFaction.Items.Clear();
            cmbPlayerFaction.Text = "";
            foreach (var factionId in ActiveDungeon.FactionInfos.Select(fi => fi.Id))
            {
                cmbPlayerFaction.Items.Add(factionId);
                if (factionId.Equals(playerClass.Faction))
                    cmbPlayerFaction.Text = factionId;
            }
            chkPlayerStartsVisible.Checked = playerClass.StartsVisible;
            nudPlayerBaseHP.Value = playerClass.BaseHP;
            nudPlayerHPPerLevelUp.Value = playerClass.MaxHPIncreasePerLevel;
            nudPlayerBaseMP.Value = playerClass.BaseMP;
            nudPlayerMPPerLevelUp.Value = playerClass.MaxMPIncreasePerLevel;
            nudPlayerBaseAttack.Value = playerClass.BaseAttack;
            nudPlayerAttackPerLevelUp.Value = playerClass.AttackIncreasePerLevel;
            nudPlayerBaseDefense.Value = playerClass.BaseDefense;
            nudPlayerDefensePerLevelUp.Value = playerClass.DefenseIncreasePerLevel;
            nudPlayerBaseMovement.Value = playerClass.BaseMovement;
            nudPlayerMovementPerLevelUp.Value = playerClass.MovementIncreasePerLevel;
            nudPlayerBaseHPRegeneration.Value = playerClass.BaseHPRegeneration;
            nudPlayerHPRegenerationPerLevelUp.Value = playerClass.HPRegenerationIncreasePerLevel;
            nudPlayerBaseMPRegeneration.Value = playerClass.BaseHPRegeneration;
            nudPlayerMPRegenerationPerLevelUp.Value = playerClass.HPRegenerationIncreasePerLevel;
            cmbPlayerSightRange.Items.Clear();
            cmbPlayerSightRange.Text = "";
            chkPlayerUsesMP.Checked = playerClass.UsesMP;
            TogglePlayerMPControls();
            foreach (var sightRange in BaseSightRangeDisplayNames)
            {
                cmbPlayerSightRange.Items.Add(sightRange.Value);
                if (sightRange.Key.Equals(playerClass.BaseSightRange))
                    cmbPlayerSightRange.Text = sightRange.Value;
            }
            if (string.IsNullOrWhiteSpace(cmbPlayerSightRange.Text))
            {
                cmbPlayerSightRange.Text = BaseSightRangeDisplayNames["FlatNumber"];
                lblPlayerSightRangeText.Visible = true;
                nudPlayerFlatSightRange.Visible = true;
                nudPlayerFlatSightRange.Enabled = true;
                try
                {
                    nudPlayerFlatSightRange.Value = int.Parse(playerClass.BaseSightRange);
                }
                catch
                {
                    nudPlayerFlatSightRange.Value = 1;
                }
            }
            else
            {
                lblPlayerSightRangeText.Visible = false;
                nudPlayerFlatSightRange.Visible = false;
                nudPlayerFlatSightRange.Enabled = false;
                nudPlayerFlatSightRange.Value = 1;
            }
            chkPlayerCanGainExperience.Checked = playerClass.CanGainExperience;
            nudPlayerMaxLevel.Value = playerClass.MaxLevel;
            txtPlayerLevelUpFormula.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMPPerLevelUp.Enabled = (chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1) && chkPlayerUsesMP.Checked;
            nudPlayerAttackPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerDefensePerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMovementPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMPRegenerationPerLevelUp.Enabled = (chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1) && chkPlayerUsesMP.Checked;
            if (playerClass.CanGainExperience || playerClass.MaxLevel > 1)
            {
                txtPlayerLevelUpFormula.Text = playerClass.ExperienceToLevelUpFormula;
            }
            else
            {
                txtPlayerLevelUpFormula.Text = "";
            }
            cmbPlayerStartingWeapon.Items.Clear();
            cmbPlayerStartingWeapon.Text = "";
            foreach (var weaponId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Weapon")).Select(i => i.Id))
            {
                cmbPlayerStartingWeapon.Items.Add(weaponId);
                if (weaponId.Equals(playerClass.StartingWeapon))
                    cmbPlayerStartingWeapon.Text = weaponId;
            }
            cmbPlayerStartingArmor.Items.Clear();
            cmbPlayerStartingArmor.Text = "";
            foreach (var armorId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Armor")).Select(i => i.Id))
            {
                cmbPlayerStartingArmor.Items.Add(armorId);
                if (armorId.Equals(playerClass.StartingArmor))
                    cmbPlayerStartingArmor.Text = armorId;
            }
            nudPlayerInventorySize.Value = playerClass.InventorySize;
            sisPlayerStartingInventory.SelectableItems = ActiveDungeon.Items.ConvertAll(i => i.Id);
            sisPlayerStartingInventory.InventorySize = playerClass.InventorySize;
            sisPlayerStartingInventory.Inventory = playerClass.StartingInventory;
            sisPlayerStartingInventory.InventoryContentsChanged += (_, _) => DirtyTab = true;
            SetSingleActionEditorParams(saePlayerOnTurnStart, playerClass.Id, playerClass.OnTurnStart);
            SetMultiActionEditorParams(maePlayerOnAttack, playerClass.Id, playerClass.OnAttack);
            SetSingleActionEditorParams(saePlayerOnAttacked, playerClass.Id, playerClass.OnAttacked);
            SetSingleActionEditorParams(saePlayerOnDeath, playerClass.Id, playerClass.OnDeath);
        }

        private bool SavePlayerClass(string id)
        {
            if (!ValidatePlayerClassDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Player Class. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var playerClass = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.PlayerClasses.Find(p => p.Id.Equals(id)) ?? new PlayerClassInfo() { Id = id }
                : (PlayerClassInfo)ActiveNodeTag.DungeonElement;
            playerClass.Name = txtPlayerClassName.Text;
            playerClass.RequiresNamePrompt = chkRequirePlayerPrompt.Checked;
            playerClass.Description = txtPlayerClassDescription.Text;
            playerClass.ConsoleRepresentation = crsPlayer.ConsoleRepresentation;
            playerClass.Faction = cmbPlayerFaction.Text;
            playerClass.StartsVisible = chkPlayerStartsVisible.Checked;
            playerClass.UsesMP = chkPlayerUsesMP.Checked;
            playerClass.BaseHP = (int)nudPlayerBaseHP.Value;
            playerClass.BaseMP = (int)nudPlayerBaseMP.Value;
            playerClass.BaseAttack = (int)nudPlayerBaseAttack.Value;
            playerClass.BaseDefense = (int)nudPlayerBaseDefense.Value;
            playerClass.BaseMovement = (int)nudPlayerBaseMovement.Value;
            playerClass.BaseHPRegeneration = nudPlayerBaseHPRegeneration.Value;
            playerClass.BaseMPRegeneration = nudPlayerBaseMPRegeneration.Value;

            if (cmbPlayerSightRange.Text.Equals(BaseSightRangeDisplayNames["FlatNumber"]))
            {
                playerClass.BaseSightRange = ((int)nudPlayerFlatSightRange.Value).ToString();
            }
            else
            {
                playerClass.BaseSightRange = BaseSightRangeDisplayNames.FirstOrDefault(bsrdn => bsrdn.Value.Equals(cmbPlayerSightRange.Text)).Key;
            }

            playerClass.CanGainExperience = chkPlayerCanGainExperience.Checked;
            playerClass.MaxLevel = (int)nudPlayerMaxLevel.Value;
            playerClass.ExperienceToLevelUpFormula = txtPlayerLevelUpFormula.Text;
            playerClass.MaxHPIncreasePerLevel = nudPlayerHPPerLevelUp.Value;
            playerClass.MaxMPIncreasePerLevel = nudPlayerMPPerLevelUp.Value;
            playerClass.AttackIncreasePerLevel = nudPlayerAttackPerLevelUp.Value;
            playerClass.DefenseIncreasePerLevel = nudPlayerDefensePerLevelUp.Value;
            playerClass.MovementIncreasePerLevel = nudPlayerMovementPerLevelUp.Value;
            playerClass.HPRegenerationIncreasePerLevel = nudPlayerHPRegenerationPerLevelUp.Value;
            playerClass.MPRegenerationIncreasePerLevel = nudPlayerMPRegenerationPerLevelUp.Value;

            playerClass.StartingWeapon = cmbPlayerStartingWeapon.Text;
            playerClass.StartingArmor = cmbPlayerStartingArmor.Text;

            playerClass.InventorySize = (int)nudPlayerInventorySize.Value;
            playerClass.StartingInventory = sisPlayerStartingInventory.Inventory;

            playerClass.OnTurnStart = saePlayerOnTurnStart.Action;
            playerClass.OnAttack = maePlayerOnAttack.Actions;
            playerClass.OnAttacked = saePlayerOnAttacked.Action;
            playerClass.OnDeath = saePlayerOnDeath.Action;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.PlayerClasses.Exists(p => p.Id.Equals(id)))
            {
                ActiveDungeon.PlayerClasses.Add(playerClass);
                MessageBox.Show(
                    $"Player Class {id} has been successfully created!",
                    "Create Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"Player Class {playerClass.Id} has been successfully updated!",
                    "Update Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{playerClass.ConsoleRepresentation.Character} - {playerClass.Id}";
            SelectNodeIfExists(nodeText, "Player Classes");
            PassedValidation = false;
            return true;
        }

        private bool SavePlayerClassAs()
        {
            if (!ValidatePlayerClassDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Player Class. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the Player Class Identifier", "Save Player Class As");
            if (inputBoxResult != null)
            {
                var saveResult = false;
                if (ActiveDungeon.PlayerClasses.Exists(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Player Class with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Player Class",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        saveResult = SavePlayerClass(inputBoxResult);
                    }
                }
                else
                {
                    saveResult = SavePlayerClass(inputBoxResult);
                }
                var savedClass = ActiveDungeon.PlayerClasses.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Player Classes");
                }
                return saveResult;
            }
            return false;
        }

        private bool ValidatePlayerClassDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtPlayerClassName.Text))
                errorMessages.Add("Enter a Player Class Name first.");
            if (string.IsNullOrWhiteSpace(txtPlayerClassDescription.Text))
                errorMessages.Add("Enter a Player Class Description first.");
            if (crsPlayer.ConsoleRepresentation.Character == '\0')
                errorMessages.Add("This Player Class does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(cmbPlayerSightRange.Text))
                errorMessages.Add("This Player Class does not have a Sight Range set.");
            if (string.IsNullOrWhiteSpace(cmbPlayerFaction.Text))
                errorMessages.Add("This Player Class does not have a Faction.");
            if (string.IsNullOrWhiteSpace(cmbPlayerStartingWeapon.Text))
                errorMessages.Add("This Player Class does not have an Emergency Weapon.");
            if (string.IsNullOrWhiteSpace(cmbPlayerStartingArmor.Text))
                errorMessages.Add("This Player Class does not have an Emergency Armor.");
            if (chkPlayerCanGainExperience.Checked && string.IsNullOrWhiteSpace(txtPlayerLevelUpFormula.Text))
                errorMessages.Add("This Player Class can gain experience, but does not have a Level Up Formula.");
            if (chkPlayerCanGainExperience.Checked && (int)nudPlayerMaxLevel.Value == 1)
                errorMessages.Add("This Player Class can gain experience, but cannot level up.");

            return !errorMessages.Any();
        }

        public void DeletePlayerClass()
        {
            var activePlayerClass = (PlayerClassInfo)ActiveNodeTag.DungeonElement;
            var deletePlayerClassPrompt = IsNewElement
                ? "Do you want to remove this unsaved Player Class?"
                : $"Do you want to PERMANENTLY delete Player Class {activePlayerClass.Id}?";
            var messageBoxResult = MessageBox.Show(
                deletePlayerClassPrompt,
                "Player Class",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activePlayerClass.Id);
                    ActiveDungeon.PlayerClasses.RemoveAll(pc => pc.Id.Equals(activePlayerClass.Id));
                    MessageBox.Show(
                        $"Player Class {removedId} has been successfully deleted.",
                        "Delete Player Class",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void txtPlayerClassName_TextChanged(object sender, EventArgs e)
        {
            txtPlayerClassName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblPlayerClassNameLocale);
            DirtyTab = true;
        }

        private void chkRequirePlayerPrompt_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void txtPlayerClassDescription_TextChanged(object sender, EventArgs e)
        {
            txtPlayerClassDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblPlayerClassDescriptionLocale);
            DirtyTab = true;
        }

        private void cmbPlayerFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void chkPlayerStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void cmbPlayerStartingWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void cmbPlayerStartingArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerInventorySize_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            sisPlayerStartingInventory.InventorySize = (int)nudPlayerInventorySize.Value;
        }

        private void crsPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerBaseHP_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerBaseAttack_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerBaseDefense_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerBaseMovement_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerBaseHPRegeneration_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerFlatSightRange_ValueChanged(object sender, EventArgs e)
        {
            if (nudPlayerFlatSightRange.Visible)
                DirtyTab = true;
        }

        private void cmbPlayerSightRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            if (cmbPlayerSightRange.Text.Equals(BaseSightRangeDisplayNames["FlatNumber"]))
            {
                lblPlayerSightRangeText.Visible = true;
                nudPlayerFlatSightRange.Visible = true;
                nudPlayerFlatSightRange.Enabled = true;
            }
            else
            {
                lblPlayerSightRangeText.Visible = false;
                nudPlayerFlatSightRange.Visible = false;
                nudPlayerFlatSightRange.Enabled = false;
            }
        }

        private void chkPlayerCanGainExperience_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            txtPlayerLevelUpFormula.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMPPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerAttackPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerDefensePerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMovementPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
        }

        private void txtPlayerLevelUpFormula_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtPlayerLevelUpFormula.Text;
        }

        private void txtPlayerLevelUpFormula_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.PlayerClass])) return;

            if (!PreviousTextBoxValue.Equals(txtPlayerLevelUpFormula.Text))
            {
                var parsedLevelUpFormula = Regex.Replace(txtPlayerLevelUpFormula.Text, @"\blevel\b", "1", RegexOptions.IgnoreCase);

                if (!string.IsNullOrWhiteSpace(parsedLevelUpFormula) && !parsedLevelUpFormula.TestNumericExpression(false, out string errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid Experience Formula: {errorMessage}",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtPlayerLevelUpFormula.Text = PreviousTextBoxValue;
                }
                else
                {
                    DirtyTab = true;
                }
            }

            PreviousTextBoxValue = "";
        }

        private void nudPlayerMaxLevel_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            txtPlayerLevelUpFormula.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMPPerLevelUp.Enabled = (chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1) && chkPlayerUsesMP.Checked;
            nudPlayerAttackPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerDefensePerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMovementPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMPRegenerationPerLevelUp.Enabled = (chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1) && chkPlayerUsesMP.Checked;
        }

        private void nudPlayerHPPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerAttackPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerDefensePerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerMovementPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerHPRegenerationPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }
        private void nudPlayerBaseMP_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void chkPlayerUsesMP_CheckedChanged(object sender, EventArgs e)
        {
            TogglePlayerMPControls();
            DirtyTab = true;
        }

        private void nudPlayerBaseMPRegeneration_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerMPPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudPlayerMPRegenerationPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void TogglePlayerMPControls()
        {
            nudPlayerBaseMP.Enabled = chkPlayerUsesMP.Checked;
            if (!chkPlayerUsesMP.Checked)
                nudPlayerBaseMP.Value = 0;
            nudPlayerBaseMPRegeneration.Enabled = chkPlayerUsesMP.Checked;
            if (!chkPlayerUsesMP.Checked)
                nudPlayerBaseMPRegeneration.Value = 0;
            nudPlayerMPPerLevelUp.Enabled = (chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1) && chkPlayerUsesMP.Checked;
            if (!chkPlayerUsesMP.Checked)
                nudPlayerMPPerLevelUp.Value = 0;
            nudPlayerMPRegenerationPerLevelUp.Enabled = (chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1) && chkPlayerUsesMP.Checked;
            if (!chkPlayerUsesMP.Checked)
                nudPlayerMPRegenerationPerLevelUp.Value = 0;
        }

        #endregion

        #region NPC
        private void LoadNPCInfoFor(NPCInfo npc)
        {
            txtNPCName.Text = npc.Name;
            txtNPCDescription.Text = npc.Description;
            try
            {
                crsNPC.Character = npc.ConsoleRepresentation.Character;
                crsNPC.BackgroundColor = npc.ConsoleRepresentation.BackgroundColor;
                crsNPC.ForegroundColor = npc.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsNPC.Character = '\0';
                crsNPC.BackgroundColor = new GameColor(Color.Black);
                crsNPC.ForegroundColor = new GameColor(Color.White);
            }
            cmbNPCFaction.Items.Clear();
            cmbNPCFaction.Text = "";
            foreach (var factionId in ActiveDungeon.FactionInfos.Select(fi => fi.Id))
            {
                cmbNPCFaction.Items.Add(factionId);
                if (factionId.Equals(npc.Faction))
                    cmbNPCFaction.Text = factionId;
            }
            chkNPCStartsVisible.Checked = npc.StartsVisible;
            chkNPCKnowsAllCharacterPositions.Checked = npc.KnowsAllCharacterPositions;
            txtNPCExperiencePayout.Text = npc.ExperiencePayoutFormula;
            nudNPCBaseHP.Value = npc.BaseHP;
            nudNPCHPPerLevelUp.Value = npc.MaxHPIncreasePerLevel;
            nudNPCBaseMP.Value = npc.BaseMP;
            nudNPCMPPerLevelUp.Value = npc.MaxMPIncreasePerLevel;
            nudNPCBaseAttack.Value = npc.BaseAttack;
            nudNPCAttackPerLevelUp.Value = npc.AttackIncreasePerLevel;
            nudNPCBaseDefense.Value = npc.BaseDefense;
            nudNPCDefensePerLevelUp.Value = npc.DefenseIncreasePerLevel;
            nudNPCBaseMovement.Value = npc.BaseMovement;
            nudNPCMovementPerLevelUp.Value = npc.MovementIncreasePerLevel;
            nudNPCBaseHPRegeneration.Value = npc.BaseHPRegeneration;
            nudNPCHPRegenerationPerLevelUp.Value = npc.HPRegenerationIncreasePerLevel;
            nudNPCBaseMPRegeneration.Value = npc.BaseMPRegeneration;
            nudNPCMPRegenerationPerLevelUp.Value = npc.MPRegenerationIncreasePerLevel;
            chkNPCUsesMP.Checked = npc.UsesMP;
            ToggleNPCMPControls();
            txtNPCLevelUpFormula.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMPPerLevelUp.Enabled = (chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1) && chkNPCUsesMP.Checked;
            nudNPCAttackPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCDefensePerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMovementPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPRegenerationPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMPRegenerationPerLevelUp.Enabled = (chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1) && chkNPCUsesMP.Checked;
            cmbNPCSightRange.Items.Clear();
            cmbNPCSightRange.Text = "";
            foreach (var sightRange in BaseSightRangeDisplayNames)
            {
                cmbNPCSightRange.Items.Add(sightRange.Value);
                if (sightRange.Key.Equals(npc.BaseSightRange))
                    cmbNPCSightRange.Text = sightRange.Value;
            }
            if (string.IsNullOrWhiteSpace(cmbNPCSightRange.Text))
            {
                cmbNPCSightRange.Text = BaseSightRangeDisplayNames["FlatNumber"];
                lblNPCSightRangeText.Visible = true;
                nudNPCFlatSightRange.Visible = true;
                nudNPCFlatSightRange.Enabled = true;
                try
                {
                    nudNPCFlatSightRange.Value = int.Parse(npc.BaseSightRange);
                }
                catch
                {
                    nudNPCFlatSightRange.Value = 1;
                }
            }
            else
            {
                lblNPCSightRangeText.Visible = false;
                nudNPCFlatSightRange.Visible = false;
                nudNPCFlatSightRange.Enabled = false;
                nudNPCFlatSightRange.Value = 1;
            }
            chkNPCCanGainExperience.Checked = npc.CanGainExperience;
            nudNPCMaxLevel.Value = npc.MaxLevel;
            if (npc.CanGainExperience || npc.MaxLevel > 1)
            {
                txtNPCLevelUpFormula.Text = npc.ExperienceToLevelUpFormula;
            }
            else
            {
                txtNPCLevelUpFormula.Text = "";
            }
            cmbNPCStartingWeapon.Items.Clear();
            cmbNPCStartingWeapon.Text = "";
            foreach (var weaponId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Weapon")).Select(i => i.Id))
            {
                cmbNPCStartingWeapon.Items.Add(weaponId);
                if (weaponId.Equals(npc.StartingWeapon))
                    cmbNPCStartingWeapon.Text = weaponId;
            }
            cmbNPCStartingArmor.Items.Clear();
            cmbNPCStartingArmor.Text = "";
            foreach (var armorId in ActiveDungeon.Items.Where(i => i.EntityType.Equals("Armor")).Select(i => i.Id))
            {
                cmbNPCStartingArmor.Items.Add(armorId);
                if (armorId.Equals(npc.StartingArmor))
                    cmbNPCStartingArmor.Text = armorId;
            }
            nudNPCInventorySize.Value = npc.InventorySize;
            sisNPCStartingInventory.SelectableItems = ActiveDungeon.Items.ConvertAll(i => i.Id);
            sisNPCStartingInventory.InventorySize = npc.InventorySize;
            sisNPCStartingInventory.Inventory = npc.StartingInventory;
            sisNPCStartingInventory.InventoryContentsChanged += (_, _) => DirtyTab = true;
            SetSingleActionEditorParams(saeNPCOnTurnStart, npc.Id, npc.OnTurnStart);
            SetMultiActionEditorParams(maeNPCOnAttack, npc.Id, npc.OnAttack);
            SetSingleActionEditorParams(saeNPCOnAttacked, npc.Id, npc.OnAttacked);
            SetSingleActionEditorParams(saeNPCOnDeath, npc.Id, npc.OnDeath);
            nudNPCOddsToTargetSelf.Value = npc.AIOddsToUseActionsOnSelf;
        }

        private bool SaveNPC(string id)
        {
            if (!ValidateNPCDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save NPC. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var npc = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.NPCs.Find(n => n.Id.Equals(id)) ?? new NPCInfo() { Id = id }
                : (NPCInfo)ActiveNodeTag.DungeonElement;
            npc.Name = txtNPCName.Text;
            npc.Description = txtNPCDescription.Text;
            npc.ConsoleRepresentation = crsNPC.ConsoleRepresentation;
            npc.Faction = cmbNPCFaction.Text;
            npc.StartsVisible = chkNPCStartsVisible.Checked;
            npc.KnowsAllCharacterPositions = chkNPCKnowsAllCharacterPositions.Checked;
            npc.ExperienceToLevelUpFormula = txtNPCLevelUpFormula.Text;
            npc.ExperiencePayoutFormula = txtNPCExperiencePayout.Text;
            npc.UsesMP = chkNPCUsesMP.Checked;
            npc.BaseHP = (int)nudNPCBaseHP.Value;
            npc.BaseMP = (int)nudNPCBaseMP.Value;
            npc.BaseAttack = (int)nudNPCBaseAttack.Value;
            npc.BaseDefense = (int)nudNPCBaseDefense.Value;
            npc.BaseMovement = (int)nudNPCBaseMovement.Value;
            npc.BaseHPRegeneration = nudNPCBaseHPRegeneration.Value;
            npc.BaseMPRegeneration = nudNPCBaseMPRegeneration.Value;

            if (cmbNPCSightRange.Text.Equals(BaseSightRangeDisplayNames["FlatNumber"]))
            {
                npc.BaseSightRange = ((int)nudNPCFlatSightRange.Value).ToString();
            }
            else
            {
                npc.BaseSightRange = BaseSightRangeDisplayNames.FirstOrDefault(bsrdn => bsrdn.Value.Equals(cmbNPCSightRange.Text)).Key;
            }

            npc.CanGainExperience = chkNPCCanGainExperience.Checked;
            npc.MaxHPIncreasePerLevel = nudNPCHPPerLevelUp.Value;
            npc.MaxMPIncreasePerLevel = nudNPCMPPerLevelUp.Value;
            npc.AttackIncreasePerLevel = nudNPCAttackPerLevelUp.Value;
            npc.DefenseIncreasePerLevel = nudNPCDefensePerLevelUp.Value;
            npc.MovementIncreasePerLevel = nudNPCMovementPerLevelUp.Value;
            npc.HPRegenerationIncreasePerLevel = nudNPCHPRegenerationPerLevelUp.Value;
            npc.MPRegenerationIncreasePerLevel = nudNPCMPRegenerationPerLevelUp.Value;

            npc.StartingWeapon = cmbNPCStartingWeapon.Text;
            npc.StartingArmor = cmbNPCStartingArmor.Text;

            npc.InventorySize = (int)nudNPCInventorySize.Value;
            npc.StartingInventory = sisNPCStartingInventory.Inventory;

            npc.OnTurnStart = saeNPCOnTurnStart.Action;
            npc.OnAttack = maeNPCOnAttack.Actions;
            npc.OnAttacked = saeNPCOnAttacked.Action;
            npc.OnDeath = saeNPCOnDeath.Action;
            npc.AIOddsToUseActionsOnSelf = (int)nudNPCOddsToTargetSelf.Value;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.NPCs.Exists(n => n.Id.Equals(id)))
            {
                ActiveDungeon.NPCs.Add(npc);
                MessageBox.Show(
                    $"NPC {id} has been successfully created!",
                    "Create NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"NPC {npc.Id} has been successfully updated!",
                    "Update NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            RefreshTreeNodes();
            var nodeText = $"{npc.ConsoleRepresentation.Character} - {npc.Id}";
            SelectNodeIfExists(nodeText, "NPCs");
            return true;
        }

        private bool SaveNPCAs()
        {
            if (!ValidateNPCDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save NPC. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the NPC Identifier", "Save NPC As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.NPCs.Exists(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An NPC with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "NPC",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveNPC(inputBoxResult);
                    }
                }
                else
                {
                    return SaveNPC(inputBoxResult);
                }
            }
            return false;
        }

        private bool ValidateNPCDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNPCName.Text))
                errorMessages.Add("Enter an NPC Name first.");
            if (string.IsNullOrWhiteSpace(txtNPCDescription.Text))
                errorMessages.Add("Enter an NPC Description first.");
            if (crsNPC.Character == '\0')
                errorMessages.Add("This NPC does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(cmbNPCSightRange.Text))
                errorMessages.Add("This NPC does not have a Sight Range set.");
            if (string.IsNullOrWhiteSpace(cmbNPCFaction.Text))
                errorMessages.Add("This NPC does not have a Faction.");
            if (string.IsNullOrWhiteSpace(cmbNPCStartingWeapon.Text))
                errorMessages.Add("This NPC does not have an Emergency Weapon.");
            if (string.IsNullOrWhiteSpace(cmbNPCStartingArmor.Text))
                errorMessages.Add("This NPC does not have an Emergency Armor.");
            if (string.IsNullOrWhiteSpace(txtNPCExperiencePayout.Text))
                errorMessages.Add("This NPC does not have an Experience Payout Formula.");
            if (chkNPCCanGainExperience.Checked && string.IsNullOrWhiteSpace(txtNPCLevelUpFormula.Text))
                errorMessages.Add("This NPC can gain experience, but does not have a Level Up Formula.");
            if (chkNPCCanGainExperience.Checked && (int)nudNPCMaxLevel.Value == 1)
                errorMessages.Add("This NPC can gain experience, but cannot level up.");
            if ((int)nudNPCMaxLevel.Value > 1 && string.IsNullOrWhiteSpace(txtNPCLevelUpFormula.Text))
                errorMessages.Add("This NPC has a maximum level above 1, but does not have a Level Up Formula.");

            return !errorMessages.Any();
        }

        public void DeleteNPC()
        {
            var activeNPC = (NPCInfo)ActiveNodeTag.DungeonElement;
            var deleteNPCPrompt = IsNewElement
                ? "Do you want to remove this unsaved NPC?"
                : $"Do you want to PERMANENTLY delete NPC {activeNPC.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteNPCPrompt,
                "NPC",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activeNPC.Id);
                    ActiveDungeon.NPCs.RemoveAll(npc => npc.Id.Equals(activeNPC.Id));
                    MessageBox.Show(
                        $"NPC {removedId} has been successfully deleted.",
                        "Delete NPC",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }
        private void txtNPCExperiencePayout_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtNPCExperiencePayout.Text;
        }

        private void txtNPCExperiencePayout_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.NPC])) return;

            if (!PreviousTextBoxValue.Equals(txtNPCExperiencePayout.Text))
            {
                var parsedPayoutFormula = Regex.Replace(txtNPCExperiencePayout.Text, @"\blevel\b", "1", RegexOptions.IgnoreCase);

                if (!string.IsNullOrWhiteSpace(parsedPayoutFormula) && !parsedPayoutFormula.TestNumericExpression(false, out string errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid Experience Formula: {errorMessage}",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtNPCExperiencePayout.Text = PreviousTextBoxValue;
                }
                else
                {
                    DirtyTab = true;
                }
            }

            PreviousTextBoxValue = "";
        }

        private void chkNPCKnowsAllCharacterPositions_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCOddsToTargetSelf_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void txtNPCName_TextChanged(object sender, EventArgs e)
        {
            txtNPCName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblNPCNameLocale);
            DirtyTab = true;
        }

        private void txtNPCDescription_TextChanged(object sender, EventArgs e)
        {
            txtNPCDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblNPCDescriptionLocale);
            DirtyTab = true;
        }

        private void cmbNPCFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void chkNPCStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void cmbNPCStartingWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void cmbNPCStartingArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCInventorySize_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            sisNPCStartingInventory.InventorySize = (int)nudNPCInventorySize.Value;
        }

        private void crsNPC_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCBaseHP_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCBaseAttack_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCBaseDefense_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCBaseMovement_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCBaseHPRegeneration_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCFlatSightRange_ValueChanged(object sender, EventArgs e)
        {
            if (nudNPCFlatSightRange.Visible)
                DirtyTab = true;
        }

        private void cmbNPCSightRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            if (cmbNPCSightRange.Text.Equals(BaseSightRangeDisplayNames["FlatNumber"]))
            {
                lblNPCSightRangeText.Visible = true;
                nudNPCFlatSightRange.Visible = true;
                nudNPCFlatSightRange.Enabled = true;
            }
            else
            {
                lblNPCSightRangeText.Visible = false;
                nudNPCFlatSightRange.Visible = false;
                nudNPCFlatSightRange.Enabled = false;
            }
        }

        private void chkNPCCanGainExperience_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            txtNPCLevelUpFormula.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMPPerLevelUp.Enabled = (chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1) && chkNPCUsesMP.Checked;
            nudNPCAttackPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCDefensePerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMovementPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPRegenerationPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMPRegenerationPerLevelUp.Enabled = (chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1) && chkNPCUsesMP.Checked;
        }

        private void txtNPCLevelUpFormula_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtNPCLevelUpFormula.Text;
        }

        private void txtNPCLevelUpFormula_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.NPC])) return;

            if (!PreviousTextBoxValue.Equals(txtNPCLevelUpFormula.Text))
            {
                var parsedLevelUpFormula = Regex.Replace(txtNPCLevelUpFormula.Text, @"\blevel\b", "1", RegexOptions.IgnoreCase);

                if (!string.IsNullOrWhiteSpace(parsedLevelUpFormula) && !parsedLevelUpFormula.TestNumericExpression(false, out string errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid Experience Formula: {errorMessage}",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtNPCLevelUpFormula.Text = PreviousTextBoxValue;
                }
                else
                {
                    DirtyTab = true;
                }
            }

            PreviousTextBoxValue = "";
        }

        private void nudNPCMaxLevel_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
            txtNPCLevelUpFormula.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMPPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCAttackPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCDefensePerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMovementPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPRegenerationPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMPRegenerationPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
        }

        private void nudNPCHPPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCAttackPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCDefensePerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCMovementPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCHPRegenerationPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCBaseMP_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void chkNPCUsesMP_CheckedChanged(object sender, EventArgs e)
        {
            ToggleNPCMPControls();
            DirtyTab = true;
        }

        private void nudNPCBaseMPRegeneration_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCMPPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void nudNPCMPRegenerationPerLevelUp_ValueChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void ToggleNPCMPControls()
        {
            nudNPCBaseMP.Enabled = chkNPCUsesMP.Checked;
            if (!chkNPCUsesMP.Checked)
                nudNPCBaseMP.Value = 0;
            nudNPCBaseMPRegeneration.Enabled = chkNPCUsesMP.Checked;
            if (!chkNPCUsesMP.Checked)
                nudNPCBaseMPRegeneration.Value = 0;
            nudNPCMPPerLevelUp.Enabled = (chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1) && chkNPCUsesMP.Checked;
            if (!chkNPCUsesMP.Checked)
                nudNPCMPPerLevelUp.Value = 0;
            nudNPCMPRegenerationPerLevelUp.Enabled = (chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1) && chkNPCUsesMP.Checked;
            if (!chkNPCUsesMP.Checked)
                nudNPCMPRegenerationPerLevelUp.Value = 0;
        }
        #endregion

        #region Item
        private void LoadItemInfoFor(ItemInfo item)
        {
            txtItemName.Text = item.Name;
            txtItemDescription.Text = item.Description;
            try
            {
                crsItem.Character = item.ConsoleRepresentation.Character;
                crsItem.BackgroundColor = item.ConsoleRepresentation.BackgroundColor;
                crsItem.ForegroundColor = item.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsItem.Character = '\0';
                crsItem.BackgroundColor = new GameColor(Color.Black);
                crsItem.ForegroundColor = new GameColor(Color.White);
            }
            cmbItemType.Text = "";
            cmbItemType.Items.Clear();
            cmbItemType.Items.AddRange(new string[] { "Weapon", "Armor", "Consumable" });
            PreviousItemType = "";
            foreach (string itemType in cmbItemType.Items)
            {
                if (itemType.Equals(item.EntityType))
                {
                    PreviousItemType = itemType;
                    cmbItemType.Text = itemType;
                    break;
                }
            }
            ToggleItemTypeControlsVisibility();
            txtItemPower.Text = item.Power;
            chkItemStartsVisible.Checked = item.StartsVisible;
            chkItemCanBePickedUp.Checked = item.CanBePickedUp;
            SetSingleActionEditorParams(saeItemOnTurnStart, item.Id, item.OnTurnStart);
            SetMultiActionEditorParams(maeItemOnAttack, item.Id, item.OnAttack);
            SetSingleActionEditorParams(saeItemOnAttacked, item.Id, item.OnAttacked);
            SetSingleActionEditorParams(saeItemOnUse, item.Id, item.OnUse);
            SetSingleActionEditorParams(saeItemOnStepped, item.Id, item.OnStepped);
        }

        private bool SaveItem(string id)
        {
            if (!ValidateItemDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Item. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var item = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.Items.Find(i => i.Id.Equals(id)) ?? new ItemInfo() { Id = id }
                : (ItemInfo)ActiveNodeTag.DungeonElement;
            item.Name = txtItemName.Text;
            item.Description = txtItemDescription.Text;
            item.ConsoleRepresentation = crsItem.ConsoleRepresentation;
            item.StartsVisible = chkItemStartsVisible.Checked;
            item.CanBePickedUp = chkItemCanBePickedUp.Checked;
            item.EntityType = cmbItemType.Text;
            item.Power = txtItemPower.Text;

            item.OnTurnStart = null;
            item.OnAttacked = null;

            if (item.EntityType == "Weapon" || item.EntityType == "Armor")
            {
                item.OnTurnStart = saeItemOnTurnStart.Action;
                item.OnAttacked = saeItemOnTurnStart.Action;
                item.OnUse = DungeonInfoHelpers.CreateEquipAction();
            }
            else if (item.EntityType == "Consumable")
            {
                item.OnUse = saeItemOnUse.Action;
            }

            item.OnAttack = maeItemOnAttack.Actions;
            item.OnStepped = saeItemOnStepped.Action;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.Items.Exists(i => i.Id.Equals(id)))
            {
                ActiveDungeon.Items.Add(item);
                MessageBox.Show(
                    $"Item {id} has been successfully created!",
                    "Create Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"Item {item.Id} has been successfully updated!",
                    "Update Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{item.ConsoleRepresentation.Character} - {item.Id}";
            SelectNodeIfExists(nodeText, "Items");
            PassedValidation = false;
            return true;
        }

        private bool SaveItemAs()
        {
            if (!ValidateItemDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Item. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the Item Identifier", "Save Item As");
            if (inputBoxResult != null)
            {
                var saveResult = false;
                if (ActiveDungeon.Items.Exists(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Item with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Item",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        saveResult = SaveItem(inputBoxResult);
                    }
                }
                else
                {
                    saveResult = SaveItem(inputBoxResult);
                }
                var savedClass = ActiveDungeon.Items.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Items");
                }
                return saveResult;
            }
            return false;
        }

        private bool ValidateItemDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtItemName.Text))
                errorMessages.Add("Enter an Item Name first.");
            if (string.IsNullOrWhiteSpace(txtItemDescription.Text))
                errorMessages.Add("Enter an Item Description first.");
            if (crsItem.Character == '\0')
                errorMessages.Add("This Item does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(cmbItemType.Text))
                errorMessages.Add("This Item does not have an Item Type.");
            if (string.IsNullOrWhiteSpace(txtItemPower.Text))
                errorMessages.Add("This Item does not have a Power.");

            return !errorMessages.Any();
        }

        public void DeleteItem()
        {
            var activeItem = (ItemInfo)ActiveNodeTag.DungeonElement;
            var deleteItemPrompt = IsNewElement
                ? "Do you want to remove this unsaved Item?"
                : $"Do you want to PERMANENTLY delete Item {activeItem.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteItemPrompt,
                "Item",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activeItem.Id);
                    ActiveDungeon.Items.RemoveAll(i => i.Id.Equals(activeItem.Id));
                    MessageBox.Show(
                        $"Item {removedId} has been successfully deleted.",
                        "Delete Item",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void txtItemName_TextChanged(object sender, EventArgs e)
        {
            txtItemName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblItemNameLocale);
            DirtyTab = true;
        }

        private void txtItemDescription_TextChanged(object sender, EventArgs e)
        {
            txtItemDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblItemDescriptionLocale);
            DirtyTab = true;
        }

        private void ToggleItemTypeControlsVisibility()
        {
            maeItemOnAttack.RequiresDescription = (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor");
            if (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor")
            {
                saeItemOnUse.Visible = false;
                saeItemOnUse.Action = null;
                saeItemOnTurnStart.Visible = true;
                maeItemOnAttack.Visible = true;
                maeItemOnAttack.ActionDescription = "The Item's Owner can\ninteract with someone else\nwith the following:";
                saeItemOnAttacked.Visible = true;
            }
            else if (cmbItemType.Text == "Consumable")
            {
                saeItemOnUse.Visible = true;
                saeItemOnTurnStart.Visible = false;
                saeItemOnTurnStart.Action = null;
                maeItemOnAttack.Visible = true;
                maeItemOnAttack.ActionDescription = "Someone can use it to\ninteract with someone else\nwith the following:";
                saeItemOnAttacked.Visible = false;
                saeItemOnAttacked.Action = null;
            }
            else
            {
                saeItemOnUse.Visible = false;
                saeItemOnUse.Action = null;
                saeItemOnTurnStart.Visible = false;
                saeItemOnTurnStart.Action = null;
                maeItemOnAttack.Visible = false;
                maeItemOnAttack.ActionDescription = "You shouldn't see this.";
                saeItemOnAttacked.Visible = false;
                saeItemOnAttacked.Action = null;
                saeItemOnStepped.Visible = false;
                saeItemOnStepped.Action = null;
            }
        }

        private void cmbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((PreviousItemType.Equals("Consumable") && (cmbItemType.Text.Equals("Weapon") || cmbItemType.Text.Equals("Armor")))
                || ((PreviousItemType.Equals("Weapon") || PreviousItemType.Equals("Armor")) && cmbItemType.Text.Equals("Consumable")))
            {
                var changeItemTypePrompt = (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor")
                    ? "Changing an Item Type from Consumable to Equippable will delete some saved Actions.\n\nNOTE: This is NOT reversible."
                    : "Changing an Item Type from Equippable to Consumable will delete some saved Actions.\n\nNOTE: This is NOT reversible.";
                var messageBoxResult = MessageBox.Show(
                    $"{changeItemTypePrompt}\n\nDo you wish to continue?",
                    "Change Item Type",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (messageBoxResult == DialogResult.No)
                {
                    cmbItemType.Text = PreviousItemType;
                    return;
                }
            }

            ToggleItemTypeControlsVisibility();

            DirtyTab = true;
            PreviousItemType = cmbItemType.Text;
        }

        private void txtItemPower_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtItemPower.Text;
        }

        private void txtItemPower_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.Item])) return;

            if (!PreviousTextBoxValue.Equals(txtItemPower.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtItemPower.Text) && !txtItemPower.Text.IsDiceNotation() && !int.TryParse(txtItemPower.Text, out _))
                {
                    MessageBox.Show(
                        $"Item Power must be either a flat integer or a Dice Notation expression",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtItemPower.Text = PreviousTextBoxValue;
                }
                else
                {
                    DirtyTab = true;
                }
            }

            PreviousTextBoxValue = "";
        }

        private void chkItemStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void chkItemCanBePickedUp_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void crsItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }
        #endregion

        #region Trap

        private void LoadTrapInfoFor(TrapInfo trap)
        {
            txtTrapName.Text = trap.Name;
            txtTrapDescription.Text = trap.Description;
            try
            {
                crsTrap.Character = trap.ConsoleRepresentation.Character;
                crsTrap.BackgroundColor = trap.ConsoleRepresentation.BackgroundColor;
                crsTrap.ForegroundColor = trap.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsTrap.Character = '\0';
                crsTrap.BackgroundColor = new GameColor(Color.Black);
                crsTrap.ForegroundColor = new GameColor(Color.White);
            }
            txtTrapPower.Text = trap.Power;
            chkTrapStartsVisible.Checked = trap.StartsVisible;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(chkTrapStartsVisible, "The 'spirit' of a Trap is that it spawns invisible.\n\nHowever, it can be enabled for debugging purposes.");
            SetSingleActionEditorParams(saeTrapOnStepped, trap.Id, trap.OnStepped);
        }

        private bool SaveTrap(string id)
        {
            if (!ValidateTrapDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Trap. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var trap = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.Traps.Find(t => t.Id.Equals(id)) ?? new TrapInfo() { Id = id }
                : (TrapInfo)ActiveNodeTag.DungeonElement;
            trap.Name = txtTrapName.Text;
            trap.Description = txtTrapDescription.Text;
            trap.ConsoleRepresentation = crsTrap.ConsoleRepresentation;
            trap.StartsVisible = chkTrapStartsVisible.Checked;
            trap.Power = txtTrapPower.Text;

            trap.OnStepped = saeTrapOnStepped.Action;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.Traps.Exists(t => t.Id.Equals(id)))
            {
                ActiveDungeon.Traps.Add(trap);
                MessageBox.Show(
                    $"Trap {id} has been successfully created!",
                    "Create Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"Trap {trap.Id} has been successfully updated!",
                    "Update Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{trap.ConsoleRepresentation.Character} - {trap.Id}";
            SelectNodeIfExists(nodeText, "Traps");
            PassedValidation = false;
            return true;
        }

        private bool SaveTrapAs()
        {
            if (!ValidateTrapDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Trap. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the Trap Identifier", "Save Trap As");
            if (inputBoxResult != null)
            {
                var saveResult = false;
                if (ActiveDungeon.Traps.Exists(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Trap with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Trap",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        saveResult = SaveTrap(inputBoxResult);
                    }
                }
                else
                {
                    saveResult = SaveTrap(inputBoxResult);
                }
                var savedClass = ActiveDungeon.Traps.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Traps");
                }
                return saveResult;
            }
            return false;
        }

        private bool ValidateTrapDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtTrapName.Text))
                errorMessages.Add("Enter an Trap Name first.");
            if (string.IsNullOrWhiteSpace(txtTrapDescription.Text))
                errorMessages.Add("Enter an Trap Description first.");
            if (crsTrap.Character == '\0')
                errorMessages.Add("This Trap does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(txtTrapPower.Text))
                errorMessages.Add("This Trap does not have a Power.");

            return !errorMessages.Any();
        }

        public void DeleteTrap()
        {
            var activeTrap = (TrapInfo)ActiveNodeTag.DungeonElement;
            var deleteTrapPrompt = IsNewElement
                ? "Do you want to remove this unsaved Trap?"
                : $"Do you want to PERMANENTLY delete Trap {activeTrap.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteTrapPrompt,
                "Trap",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activeTrap.Id);
                    ActiveDungeon.Traps.RemoveAll(t => t.Id.Equals(activeTrap.Id));
                    MessageBox.Show(
                        $"Trap {removedId} has been successfully deleted.",
                        "Delete Trap",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void txtTrapName_TextChanged(object sender, EventArgs e)
        {
            txtTrapName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblTrapNameLocale);
            DirtyTab = true;
        }

        private void txtTrapDescription_TextChanged(object sender, EventArgs e)
        {
            txtTrapDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblTrapDescriptionLocale);
            DirtyTab = true;
        }

        private void txtTrapPower_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtTrapPower.Text;
        }

        private void txtTrapPower_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.Trap])) return;

            if (!PreviousTextBoxValue.Equals(txtTrapPower.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtTrapPower.Text) && !txtTrapPower.Text.IsDiceNotation() && !int.TryParse(txtTrapPower.Text, out _))
                {
                    MessageBox.Show(
                        $"Trap Power must be either a flat integer or a Dice Notation expression",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtTrapPower.Text = PreviousTextBoxValue;
                }
                else
                {
                    DirtyTab = true;
                }
            }

            PreviousTextBoxValue = "";
        }

        private void chkTrapStartsVisible_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void crsTrap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        #endregion

        #region Altered Status

        private void LoadAlteredStatusInfoFor(AlteredStatusInfo alteredStatus)
        {
            txtAlteredStatusName.Text = alteredStatus.Name;
            txtAlteredStatusDescription.Text = alteredStatus.Description;
            try
            {
                crsAlteredStatus.Character = alteredStatus.ConsoleRepresentation.Character;
                crsAlteredStatus.BackgroundColor = alteredStatus.ConsoleRepresentation.BackgroundColor;
                crsAlteredStatus.ForegroundColor = alteredStatus.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsAlteredStatus.Character = '\0';
                crsAlteredStatus.BackgroundColor = new GameColor(Color.Black);
                crsAlteredStatus.ForegroundColor = new GameColor(Color.White);
            }
            chkAlteredStatusCanStack.Checked = alteredStatus.CanStack;
            chkAlteredStatusCanOverwrite.Checked = alteredStatus.CanOverwrite;
            chkAlteredStatusCleanseOnFloorChange.Checked = alteredStatus.CleanseOnFloorChange;
            chkAlteredStatusCleansedOnCleanseActions.Checked = alteredStatus.CleansedByCleanseActions;
            SetSingleActionEditorParams(saeAlteredStatusOnApply, alteredStatus.Id, alteredStatus.OnApply);
            SetSingleActionEditorParams(saeAlteredStatusOnTurnStart, alteredStatus.Id, alteredStatus.OnTurnStart);
        }

        private bool SaveAlteredStatus(string id)
        {
            if (!ValidateAlteredStatusDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Altered Status. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Altered Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var alteredStatus = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.AlteredStatuses.Find(als => als.Id.Equals(id)) ?? new AlteredStatusInfo() { Id = id }
                : (AlteredStatusInfo)ActiveNodeTag.DungeonElement;
            alteredStatus.Name = txtAlteredStatusName.Text;
            alteredStatus.Description = txtAlteredStatusDescription.Text;
            alteredStatus.ConsoleRepresentation = crsAlteredStatus.ConsoleRepresentation;
            alteredStatus.CanStack = chkAlteredStatusCanStack.Checked;
            alteredStatus.CanOverwrite = chkAlteredStatusCanOverwrite.Checked;
            alteredStatus.CleanseOnFloorChange = chkAlteredStatusCleanseOnFloorChange.Checked;
            alteredStatus.CleansedByCleanseActions = chkAlteredStatusCleansedOnCleanseActions.Checked;

            alteredStatus.OnApply = saeAlteredStatusOnApply.Action;
            alteredStatus.OnTurnStart = saeAlteredStatusOnTurnStart.Action;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.AlteredStatuses.Exists(als => als.Id.Equals(id)))
            {
                ActiveDungeon.AlteredStatuses.Add(alteredStatus);
                MessageBox.Show(
                    $"Altered Status {id} has been successfully created!",
                    "Create Altered Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    $"Altered Status {alteredStatus.Id} has been successfully updated!",
                    "Update Altered Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            IsNewElement = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{alteredStatus.ConsoleRepresentation.Character} - {alteredStatus.Id}";
            SelectNodeIfExists(nodeText, "Altered Statuses");
            PassedValidation = false;
            return true;
        }

        private bool SaveAlteredStatusAs()
        {
            if (!ValidateAlteredStatusDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Altered Status. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Altered Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var inputBoxResult = InputBox.Show("Indicate the Altered Status Identifier", "Save Altered Status As");
            if (inputBoxResult != null)
            {
                var saveResult = false;
                if (ActiveDungeon.AlteredStatuses.Exists(als => als.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Altered Status with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Altered Status",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        saveResult = SaveAlteredStatus(inputBoxResult);
                    }
                }
                else
                {
                    saveResult = SaveAlteredStatus(inputBoxResult);
                }
                var savedClass = ActiveDungeon.AlteredStatuses.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Altered Statuses");
                }
                return saveResult;
            }
            return false;
        }

        private bool ValidateAlteredStatusDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtAlteredStatusName.Text))
                errorMessages.Add("Enter an Altered Status Name first.");
            if (string.IsNullOrWhiteSpace(txtAlteredStatusDescription.Text))
                errorMessages.Add("Enter an Altered Status Description first.");
            if (crsAlteredStatus.Character == '\0')
                errorMessages.Add("This Altered Status does not have a Console Representation character.");

            return !errorMessages.Any();
        }

        public void DeleteAlteredStatus()
        {
            var activeAlteredStatus = (AlteredStatusInfo)ActiveNodeTag.DungeonElement;
            var deleteAlteredStatusPrompt = IsNewElement
                ? "Do you want to remove this unsaved Altered Status?"
                : $"Do you want to PERMANENTLY delete Altered Status {activeAlteredStatus.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteAlteredStatusPrompt,
                "Altered Status",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewElement)
                {
                    var removedId = new string(activeAlteredStatus.Id);
                    ActiveDungeon.AlteredStatuses.RemoveAll(als => als.Id.Equals(activeAlteredStatus.Id));
                    MessageBox.Show(
                        $"Altered Status {removedId} has been successfully deleted.",
                        "Delete Altered Status",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewElement = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = TabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void txtAlteredStatusName_TextChanged(object sender, EventArgs e)
        {
            txtAlteredStatusName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblAlteredStatusNameLocale);
            DirtyTab = true;
        }

        private void txtAlteredStatusDescription_TextChanged(object sender, EventArgs e)
        {
            txtAlteredStatusDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblAlteredStatusDescriptionLocale);
            DirtyTab = true;
        }

        private void crsAlteredStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DirtyTab = true;
        }

        private void chkAlteredStatusCanStack_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAlteredStatusCanStack.Checked)
                chkAlteredStatusCanOverwrite.Checked = false;
            DirtyTab = true;
        }

        private void chkAlteredStatusCanOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAlteredStatusCanOverwrite.Checked)
                chkAlteredStatusCanStack.Checked = false;
            DirtyTab = true;
        }

        private void chkAlteredStatusCleanseOnFloorChange_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        private void chkAlteredStatusCleansedOnCleanseActions_CheckedChanged(object sender, EventArgs e)
        {
            DirtyTab = true;
        }

        #endregion

        #region Validator

        private void ValidateDungeon()
        {
            try
            {
                PassedValidation = false;
                tvValidationResults.Nodes.Clear();
                var dungeonValidator = new DungeonValidator(ActiveDungeon);
                PassedValidation = dungeonValidator.Validate(MandatoryLocaleKeys);

                tvValidationResults.Visible = true;
                tvValidationResults.Font = new Font("Arial", 11, FontStyle.Regular);

                AddValidationResultNode("Name", dungeonValidator.NameValidationMessages);
                AddValidationResultNode("Author", dungeonValidator.AuthorValidationMessages);
                AddValidationResultNode("Welcome/Ending Message", dungeonValidator.MessageValidationMessages);
                AddValidationResultNode("Default Locale", dungeonValidator.DefaultLocaleValidationMessages);
                AddValidationResultNode("General Floor Plan", dungeonValidator.FloorPlanValidationMessages);

                AddValidationResultNode("Object Ids", dungeonValidator.IdValidationMessages);

                foreach (var (Id, ValidationMessages) in dungeonValidator.TileSetValidationMessages)
                {
                    AddValidationResultNode($"Tileset {Id}", ValidationMessages);
                }

                foreach (var (FloorMinimumLevel, FloorMaximumLevel, ValidationMessages) in dungeonValidator.FloorGroupValidationMessages)
                {
                    if (FloorMinimumLevel != FloorMaximumLevel)
                        AddValidationResultNode($"Floor Group for Levels {FloorMinimumLevel}-{FloorMaximumLevel}", ValidationMessages);
                    else
                        AddValidationResultNode($"Floor Group for Level {FloorMinimumLevel}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.FactionValidationMessages)
                {
                    AddValidationResultNode($"Faction {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.PlayerClassValidationMessages)
                {
                    AddValidationResultNode($"Player Class {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.NPCValidationMessages)
                {
                    AddValidationResultNode($"NPC {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.ItemValidationMessages)
                {
                    AddValidationResultNode($"Item {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.TrapValidationMessages)
                {
                    AddValidationResultNode($"Trap {Id}", ValidationMessages);
                }

                foreach (var (Id, ValidationMessages) in dungeonValidator.AlteredStatusValidationMessages)
                {
                    AddValidationResultNode($"Altered Status {Id}", ValidationMessages);
                }

                foreach (var (Language, ValidationMessages) in dungeonValidator.LocaleStringValidationMessages)
                {
                    AddValidationResultNode($"Locale {Language}", ValidationMessages);
                }

                if (PassedValidation)
                    MessageBox.Show($"This Dungeon has passed Validation. You can save and play with it, assured no known game-breaking bugs will happen.\nCheck Validation Results for more info.", "Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"This Dungeon has failed Validation. Please fix it, as playing with it can cause known game-breaking bugs to happen.\nCheck Validation Results for more info.", "Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Attempting to Validate this Dungeon threw an error:\n\n{ex.Message}\n\nPlease fix it.", "Dungeon Validator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PassedValidation = true;
                tvValidationResults.Visible = false;
                if (tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.Validator]))
                    tbTabs.TabPages.Remove(TabsForNodeTypes[TabTypes.Validator]);
            }
        }

        private void AddValidationResultNode(string rootNodeName, DungeonValidationMessages validationMessages)
        {
            var nameNode = new TreeNode(rootNodeName)
            {
                NodeFont = new Font("Arial", 10, FontStyle.Bold)
            };
            if (validationMessages.HasErrors)
            {
                nameNode.ForeColor = Color.Red;
            }
            else if (validationMessages.HasWarnings)
            {
                nameNode.ForeColor = Color.DarkOrange;
            }
            else
            {
                nameNode.ForeColor = Color.Green;
            }

            foreach (var validationMessage in validationMessages.ValidationMessages)
            {
                var childNode = new TreeNode(validationMessage.Message)
                {
                    NodeFont = new Font("Arial", 10, FontStyle.Regular)
                };
                if (validationMessage.Type == DungeonValidationMessageType.Error)
                {
                    childNode.ForeColor = Color.Red;
                }
                else if (validationMessage.Type == DungeonValidationMessageType.Warning)
                {
                    childNode.ForeColor = Color.DarkOrange;
                }
                else if (validationMessage.Type == DungeonValidationMessageType.Success)
                {
                    childNode.ForeColor = Color.Green;
                }
                nameNode.Nodes.Add(childNode);
            }

            tvValidationResults.Nodes.Add(nameNode);
        }

        #endregion
    }

    public class NodeTag
    {
        public TabTypes TabToOpen { get; set; }
        public object? DungeonElement { get; set; }
    }

    public enum TabTypes
    {
        BasicInfo,
        Locales,
        TileSetInfo,
        FloorInfo,
        FactionInfo,
        PlayerClass,
        NPC,
        Item,
        Trap,
        AlteredStatus,
        Validator
    }
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
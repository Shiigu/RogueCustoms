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
using System.IO;
using System.Reflection.Metadata;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace RogueCustomsDungeonEditor
{
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
                PropertyNameCaseInsensitive = true
            });

            jsonString = File.ReadAllText("./FloorInfos/FloorTypeData.json");
            BaseGeneratorAlgorithms = JsonSerializer.Deserialize<List<FloorTypeData>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            jsonString = File.ReadAllText("./Resources/LocaleTemplate.json");
            LocaleTemplate = JsonSerializer.Deserialize<LocaleInfo>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var algorithmIcons = new ImageList();
            algorithmIcons.ImageSize = new Size(64, 64);
            algorithmIcons.ColorDepth = ColorDepth.Depth32Bit;
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
                    Application.Exit();
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
                var matchingNodes = tvDungeonInfo.Nodes.Find(e.Node.Text, true).Where(n => (n.Parent == null && e.Node.Parent == null) || (n.Parent.Text.Equals(e.Node.Parent.Text))).ToList();
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
            var matchingNodes = tvDungeonInfo.Nodes.Find(nodeText, true).Where(n => (n.Parent == null && string.IsNullOrWhiteSpace(parentNodeText)) || (n.Parent.Text.Equals(parentNodeText))).ToList();
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
                    return;
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
                    return;
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
                        PropertyNameCaseInsensitive = true
                    });
                    var formerVersion = !string.IsNullOrWhiteSpace(ActiveDungeon.Version) ? new string(ActiveDungeon.Version) : "1.0";
                    ActiveDungeon.ConvertDungeonInfoIfNeeded();
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
                        DirtyDungeon = false;
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
                File.WriteAllText(filePath, String.Empty);
                using FileStream createStream = File.OpenWrite(filePath);
                JsonSerializer.Serialize(createStream, ActiveDungeon, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                createStream.Dispose();
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
                tabToOpen = ActiveNodeTag.TabToOpen;
            else
            {
                switch (tvDungeonInfo.SelectedNode.Text)
                {
                    case "Locales":
                        tabToOpen = TabTypes.Locales;
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

        private void SaveElement()
        {
            if (!IsNewElement)
            {
                switch (ActiveNodeTag.TabToOpen)
                {
                    case TabTypes.BasicInfo:
                        SaveBasicInfo();
                        break;
                    case TabTypes.Locales:
                        SaveLocale();
                        break;
                    case TabTypes.FloorInfo:
                        SaveFloorGroup();
                        break;
                    case TabTypes.FactionInfo:
                        SaveFaction(null);
                        break;
                    case TabTypes.PlayerClass:
                        SavePlayerClass(null);
                        break;
                    case TabTypes.NPC:
                        SaveNPC(null);
                        break;
                    case TabTypes.Item:
                        SaveItem(null);
                        break;
                    case TabTypes.Trap:
                        SaveTrap(null);
                        break;
                    case TabTypes.AlteredStatus:
                        SaveAlteredStatus(null);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SaveElementAs();
            }
        }

        private void tsbSaveElementAs_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
            SaveElementAs();
        }

        private void SaveElementAs()
        {
            switch (ActiveNodeTag.TabToOpen)
            {
                case TabTypes.Locales:
                    SaveLocaleAs();
                    break;
                case TabTypes.FloorInfo:
                    SaveFloorGroupAs();
                    break;
                case TabTypes.FactionInfo:
                    SaveFactionAs();
                    break;
                case TabTypes.PlayerClass:
                    SavePlayerClassAs();
                    break;
                case TabTypes.NPC:
                    SaveNPCAs();
                    break;
                case TabTypes.Item:
                    SaveItemAs();
                    break;
                case TabTypes.Trap:
                    SaveTrapAs();
                    break;
                case TabTypes.AlteredStatus:
                    SaveAlteredStatusAs();
                    break;
                default:
                    break;
            }
        }

        private void tsbDeleteElement_Click(object sender, EventArgs e)
        {
            switch (ActiveNodeTag.TabToOpen)
            {
                case TabTypes.Locales:
                    DeleteLocale();
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
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Floor Group";
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

        #region Shared between tabs

        private void OpenActionEditScreenForListBox(ListBox actionListBox, string actionTypeText, bool isNewAction, bool requiresCondition, bool requiresDescription, bool requiresActionName, string placeholderActionNameIfNeeded, UsageCriteria usageCriteria)
        {
            var action = (isNewAction)
                            ? new ActionWithEffectsInfo()
                            : (actionListBox.SelectedItem as ListBoxItem).Tag as ActionWithEffectsInfo;
            var classId = ((ClassInfo)ActiveNodeTag.DungeonElement).Id;
            var frmActionEdit = new frmActionEdit(action, ActiveDungeon, classId, actionTypeText, requiresCondition, requiresDescription, requiresActionName, placeholderActionNameIfNeeded, usageCriteria, ActiveDungeon.AlteredStatuses.Where(als => !als.Id.Equals(classId)).Select(als => als.Id).ToList(), EffectParamData);
            frmActionEdit.ShowDialog();
            if (frmActionEdit.Saved)
            {
                if (frmActionEdit.IsNewAction && !string.IsNullOrWhiteSpace(frmActionEdit.ActionToSave?.Effect?.EffectName))
                {
                    actionListBox.Items.Add(new ListBoxItem
                    {
                        Text = frmActionEdit.ActionToSave.Name,
                        Tag = frmActionEdit.ActionToSave
                    });
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(frmActionEdit.ActionToSave?.Effect?.EffectName))
                        (actionListBox.SelectedItem as ListBoxItem).Tag = frmActionEdit.ActionToSave;
                    else
                        actionListBox.Items.Remove(actionListBox.SelectedItem);
                }
                DirtyTab = true;
            }
        }

        private void OpenActionEditScreenForButton(Button actionButton, string actionTypeText, string classId, bool requiresCondition, bool requiresDescription, bool requiresActionName, string placeholderActionNameIfNeeded, UsageCriteria usageCriteria)
        {
            var action = actionButton.Tag as ActionWithEffectsInfo;
            var frmActionEdit = new frmActionEdit(action, ActiveDungeon, classId, actionTypeText, requiresCondition, requiresDescription, requiresActionName, placeholderActionNameIfNeeded, usageCriteria, ActiveDungeon.AlteredStatuses.Where(als => !als.Id.Equals(classId)).Select(als => als.Id).ToList(), EffectParamData);
            frmActionEdit.ShowDialog();
            if (frmActionEdit.Saved)
            {
                actionButton.Tag = frmActionEdit.ActionToSave;
                DirtyTab = true;
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

        private void SaveBasicInfo()
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
        }

        #endregion

        #region Locales

        private void LoadLocaleInfoFor(LocaleInfo localeInfo)
        {
            var localeClone = localeInfo.Clone(MandatoryLocaleKeys);
            AddMissingMandatoryLocalesIfNeeded(localeClone);
            dgvLocales.Tag = localeClone;
            dgvLocales.Rows.Clear();
            foreach (var entry in localeClone.LocaleStrings)
            {
                dgvLocales.Rows.Add(entry.Key, entry.Value);
            }
        }

        private void AddMissingMandatoryLocalesIfNeeded(LocaleInfo localeInfo)
        {
            var localeKeys = localeInfo.LocaleStrings.Select(x => x.Key).ToList();
            var missingMandatoryKeys = MandatoryLocaleKeys.Except(localeKeys);
            foreach (var missingKey in missingMandatoryKeys)
            {
                var templateLocaleEntry = LocaleTemplate.LocaleStrings.Find(ls => ls.Key.Equals(missingKey));
                localeInfo.LocaleStrings.Add(new LocaleInfoString
                {
                    Key = templateLocaleEntry.Key,
                    Value = templateLocaleEntry.Value
                });
            }
            if(missingMandatoryKeys.Any())
            {
                DirtyTab = true;
                MessageBox.Show(
                    "This Locale is missing some mandatory keys.\n\nThey have been added at the end of the table. Please check them.",
                    "Locale",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
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
            var keyExistsInLocale = locale.LocaleStrings.Any(ls => ls.Key == txtLocaleEntryKey.Text);
            btnDeleteLocale.Enabled = !MandatoryLocaleKeys.Contains(txtLocaleEntryKey.Text) && keyExistsInLocale;
            btnAddLocale.Enabled = !keyExistsInLocale;
            btnUpdateLocale.Enabled = keyExistsInLocale;

            if (keyExistsInLocale)
            {
                var missingLanguages = new List<string>();
                foreach (var localeToCheck in ActiveDungeon.Locales)
                {
                    if (!localeToCheck.Language.Equals(locale.Language) && !localeToCheck.LocaleStrings.Any(ls => ls.Key == txtLocaleEntryKey.Text))
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

        private void PromptLocaleUpdate(LocaleInfo locale)
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
            }
        }

        private void SaveLocale()
        {
            var locale = (LocaleInfo)dgvLocales.Tag;
            PromptLocaleUpdate(locale);
        }

        private void SaveLocaleAs()
        {
            string inputBoxResult;
            do
            {
                inputBoxResult = InputBox.Show("Indicate the Locale Identifier. It must be exactly two characters long.\n\n(For example, \"en\" or \"es\")", "Save Locale As");
                if (inputBoxResult == null) return;
                if (inputBoxResult.Length > 2)
                    MessageBox.Show(
                        $"{inputBoxResult} is too long of a name for a locale.\n\nIt must be exactly two characters long.",
                        "Create new Locale",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                    );
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
                }
                else
                {
                    PromptLocaleUpdate(preExistingLocale);
                }
            }
        }

        private void DeleteLocale()
        {
            var activeLocale = (LocaleInfo)dgvLocales.Tag;
            var deleteLocalePrompt = (IsNewElement)
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
            btnOnFloorStartAction.Tag = floorGroup.OnFloorStartActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            chkGenerateStairsOnStart.Checked = floorGroup.GenerateStairsOnStart;
            fklblStairsReminder.Visible = !chkGenerateStairsOnStart.Checked;
            RefreshGenerationAlgorithmList();
            nudMaxRoomConnections.Value = floorGroup.MaxConnectionsBetweenRooms;
            nudExtraRoomConnectionOdds.Value = floorGroup.OddsForExtraConnections;
            nudRoomFusionOdds.Value = floorGroup.RoomFusionOdds;
        }

        private void SaveFloorGroup()
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            if ((lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>)?.Any() != true)
            {
                MessageBox.Show(
                    "You cannot save this Floor Group because it has no set Generator Algorithms.\n\nPlease correct it.",
                    "Invalid Floor Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            if (IsOverlappingWithOtherFloorInfos((int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, activeFloorGroup))
            {
                MessageBox.Show(
                    "You cannot save this Floor Group because it would overlap with the level of another Floor Group.\n\nPlease correct it.",
                    "Invalid Floor Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            PromptFloorInfoUpdate(activeFloorGroup);
        }

        private void SaveFloorGroupAs()
        {
            if ((lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>)?.Any() != true)
            {
                MessageBox.Show(
                    "You cannot save this Floor Group because it has no set Generator Algorithms.\n\nPlease correct it.",
                    "Invalid Floor Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            if (IsOverlappingWithOtherFloorInfos((int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, null))
            {
                MessageBox.Show(
                    "You cannot save this Floor Group because it would overlap with the level of another Floor Group.\n\nPlease correct it.",
                    "Invalid Floor Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            PromptFloorInfoUpdate(null);
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

        private void PromptFloorInfoUpdate(FloorInfo floorGroup)
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
                floorGroup.OnFloorStartActions = new();
                var onFloorStartAction = btnOnFloorStartAction.Tag as ActionWithEffectsInfo;
                if (!string.IsNullOrWhiteSpace(onFloorStartAction?.Effect?.EffectName))
                    floorGroup.OnFloorStartActions.Add(onFloorStartAction);

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
            }
        }


        private void DeleteFloorGroup()
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var floorLevelString = (activeFloorGroup.MinFloorLevel != activeFloorGroup.MaxFloorLevel)
                    ? $"{activeFloorGroup.MinFloorLevel}-{activeFloorGroup.MaxFloorLevel}"
                    : activeFloorGroup.MinFloorLevel.ToString(); ;
            var deleteFloorGroupPrompt = (IsNewElement)
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
            nudMaxRoomConnections.Enabled = algorithmList.Any(pga => pga.Columns > 1 || pga.Rows > 1);
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

        private void btnOnFloorStartAction_Click(object sender, EventArgs e)
        {
            var floorGroup = ((FloorInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnOnFloorStartAction, "Turn Start", string.Empty, false, false, false, "FloorTurnStart", UsageCriteria.AnyTargetAnyTime);
        }

        private void btnAddAlgorithm_Click(object sender, EventArgs e)
        {
            var activeFloorGroup = (FloorInfo)ActiveNodeTag.DungeonElement;
            var frmAlgorithmWindow = new frmFloorGeneratorAlgorithm(activeFloorGroup, (int)nudWidth.Value, (int)nudHeight.Value, (int)nudMinFloorLevel.Value, (int)nudMaxFloorLevel.Value, null, BaseGeneratorAlgorithms);
            frmAlgorithmWindow.ShowDialog();
            if (frmAlgorithmWindow.Saved)
            {
                var generatorAlgorithms = lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>;
                if (generatorAlgorithms == null)
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
            var generatorAlgorithms = lvFloorAlgorithms.Tag as List<GeneratorAlgorithmInfo>;
            if (generatorAlgorithms == null)
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
                nudExtraRoomConnectionOdds.Enabled = true;
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
                nudMaxRoomConnections.Enabled = true;
        }

        private void nudRoomFusionOdds_ValueChanged(object sender, EventArgs e)
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

        public void SaveFaction(string id)
        {
            if (!ValidateFactionDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Faction. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
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

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.FactionInfos.Any(fi => fi.Id.Equals(id)))
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
        }

        public void SaveFactionAs()
        {
            if (!ValidateFactionDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Faction. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = InputBox.Show("Indicate the Faction Identifier", "Save Faction As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.FactionInfos.Any(fi => fi.Id.Equals(inputBoxResult)))
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
                    if (ActiveDungeon.FactionInfos.Any(fi => fi.Id.Equals(inputBoxResult)))
                        SelectNodeIfExists(inputBoxResult, "Factions");
                }
            }
        }

        public void DeleteFaction()
        {
            var activeFaction = (FactionInfo)ActiveNodeTag.DungeonElement;
            var deleteFactionPrompt = (IsNewElement)
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
                lblPlayerConsoleRepresentation.Text = playerClass.ConsoleRepresentation.Character.ToString();
                lblPlayerConsoleRepresentation.BackColor = playerClass.ConsoleRepresentation.BackgroundColor.ToColor();
                lblPlayerConsoleRepresentation.ForeColor = playerClass.ConsoleRepresentation.ForegroundColor.ToColor();
            }
            catch (Exception ex)
            {
                lblPlayerConsoleRepresentation.BackColor = Color.Transparent;
                lblPlayerConsoleRepresentation.ForeColor = Color.Black;
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
            nudPlayerBaseAttack.Value = playerClass.BaseAttack;
            nudPlayerAttackPerLevelUp.Value = playerClass.AttackIncreasePerLevel;
            nudPlayerBaseDefense.Value = playerClass.BaseDefense;
            nudPlayerDefensePerLevelUp.Value = playerClass.DefenseIncreasePerLevel;
            nudPlayerBaseMovement.Value = playerClass.BaseMovement;
            nudPlayerMovementPerLevelUp.Value = playerClass.MovementIncreasePerLevel;
            nudPlayerBaseHPRegeneration.Value = playerClass.BaseHPRegeneration;
            nudPlayerHPRegenerationPerLevelUp.Value = playerClass.HPRegenerationIncreasePerLevel;
            cmbPlayerSightRange.Items.Clear();
            cmbPlayerSightRange.Text = "";
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
                catch (Exception ex)
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
            nudPlayerAttackPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerDefensePerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMovementPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
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
            cmbPlayerInventoryItemChoices.Items.Clear();
            cmbPlayerInventoryItemChoices.Text = "";
            foreach (var itemId in ActiveDungeon.Items.Select(i => i.Id))
            {
                cmbPlayerInventoryItemChoices.Items.Add(itemId);
            }
            lbPlayerStartingInventory.Items.Clear();
            foreach (var itemId in playerClass.StartingInventory)
            {
                lbPlayerStartingInventory.Items.Add(itemId);
            }
            btnPlayerAddItem.Enabled = false;
            btnPlayerRemoveItem.Enabled = false;
            btnPlayerOnTurnStartAction.Tag = playerClass.OnTurnStartActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            lbPlayerOnAttackActions.Items.Clear();
            lbPlayerOnAttackActions.DisplayMember = "Text";
            foreach (var action in playerClass.OnAttackActions)
            {
                var actionItem = new ListBoxItem
                {
                    Text = action.Name,
                    Tag = action
                };
                lbPlayerOnAttackActions.Items.Add(actionItem);
            }
            btnEditPlayerOnAttackAction.Enabled = false;
            btnRemovePlayerOnAttackAction.Enabled = false;
            btnPlayerOnAttackedAction.Tag = playerClass.OnAttackedActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            btnPlayerOnDeathAction.Tag = playerClass.OnDeathActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
        }

        private void SavePlayerClass(string id)
        {
            if (!ValidatePlayerClassDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Player Class. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var playerClass = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.PlayerClasses.Find(p => p.Id.Equals(id)) ?? new PlayerClassInfo() { Id = id }
                : (PlayerClassInfo)ActiveNodeTag.DungeonElement;
            playerClass.Name = txtPlayerClassName.Text;
            playerClass.RequiresNamePrompt = chkRequirePlayerPrompt.Checked;
            playerClass.Description = txtPlayerClassDescription.Text;
            playerClass.ConsoleRepresentation = new ConsoleRepresentation
            {
                Character = lblPlayerConsoleRepresentation.Text[0],
                BackgroundColor = new GameColor(lblPlayerConsoleRepresentation.BackColor),
                ForegroundColor = new GameColor(lblPlayerConsoleRepresentation.ForeColor)
            };
            playerClass.Faction = cmbPlayerFaction.Text;
            playerClass.StartsVisible = chkPlayerStartsVisible.Checked;
            playerClass.BaseHP = (int)nudPlayerBaseHP.Value;
            playerClass.BaseAttack = (int)nudPlayerBaseAttack.Value;
            playerClass.BaseDefense = (int)nudPlayerBaseDefense.Value;
            playerClass.BaseMovement = (int)nudPlayerBaseMovement.Value;
            playerClass.BaseHPRegeneration = nudPlayerBaseHPRegeneration.Value;

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
            playerClass.AttackIncreasePerLevel = nudPlayerAttackPerLevelUp.Value;
            playerClass.DefenseIncreasePerLevel = nudPlayerDefensePerLevelUp.Value;
            playerClass.MovementIncreasePerLevel = nudPlayerMovementPerLevelUp.Value;
            playerClass.HPRegenerationIncreasePerLevel = nudPlayerHPRegenerationPerLevelUp.Value;

            playerClass.StartingWeapon = cmbPlayerStartingWeapon.Text;
            playerClass.StartingArmor = cmbPlayerStartingArmor.Text;

            playerClass.InventorySize = (int)nudPlayerInventorySize.Value;
            playerClass.StartingInventory = new();
            foreach (string inventoryItemId in lbPlayerStartingInventory.Items)
            {
                playerClass.StartingInventory.Add(inventoryItemId);
            }

            var onTurnStartAction = btnPlayerOnTurnStartAction.Tag as ActionWithEffectsInfo;
            playerClass.OnTurnStartActions = new();
            if (!string.IsNullOrWhiteSpace(onTurnStartAction?.Effect?.EffectName))
                playerClass.OnTurnStartActions.Add(onTurnStartAction);

            playerClass.OnAttackActions = new();
            foreach (ListBoxItem onAttackActionListItem in lbPlayerOnAttackActions.Items)
            {
                playerClass.OnAttackActions.Add(onAttackActionListItem.Tag as ActionWithEffectsInfo);
            }

            var onAttackedAction = btnPlayerOnAttackedAction.Tag as ActionWithEffectsInfo;
            playerClass.OnAttackedActions = new();
            if (!string.IsNullOrWhiteSpace(onAttackedAction?.Effect?.EffectName))
                playerClass.OnAttackedActions.Add(onAttackedAction);

            var onDeathAction = btnPlayerOnDeathAction.Tag as ActionWithEffectsInfo;
            playerClass.OnDeathActions = new();
            if (!string.IsNullOrWhiteSpace(onDeathAction?.Effect?.EffectName))
                playerClass.OnDeathActions.Add(onDeathAction);

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.PlayerClasses.Any(p => p.Id.Equals(id)))
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
        }

        private void SavePlayerClassAs()
        {
            if (!ValidatePlayerClassDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Player Class. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = InputBox.Show("Indicate the Player Class Identifier", "Save Player Class As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.PlayerClasses.Any(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Player Class with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Player Class",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SavePlayerClass(inputBoxResult);
                    }
                }
                else
                {
                    SavePlayerClass(inputBoxResult);
                }
                var savedClass = ActiveDungeon.PlayerClasses.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Player Classes");
                }
            }
        }

        private bool ValidatePlayerClassDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtPlayerClassName.Text))
                errorMessages.Add("Enter a Player Class Name first.");
            if (string.IsNullOrWhiteSpace(txtPlayerClassDescription.Text))
                errorMessages.Add("Enter a Player Class Description first.");
            if (string.IsNullOrWhiteSpace(lblPlayerConsoleRepresentation.Text))
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
            var activePlayerClass = (ClassInfo)ActiveNodeTag.DungeonElement;
            var deletePlayerClassPrompt = (IsNewElement)
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
            btnPlayerAddItem.Enabled = !string.IsNullOrWhiteSpace(cmbPlayerInventoryItemChoices.Text) && lbPlayerStartingInventory.Items.Count < nudPlayerInventorySize.Value;
        }

        private void cmbPlayerInventoryItemChoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnPlayerAddItem.Enabled = !string.IsNullOrWhiteSpace(cmbPlayerInventoryItemChoices.Text) && lbPlayerStartingInventory.Items.Count < nudPlayerInventorySize.Value;
        }

        private void btnPlayerAddItem_Click(object sender, EventArgs e)
        {
            lbPlayerStartingInventory.Items.Add(cmbPlayerInventoryItemChoices.Text);
            cmbPlayerInventoryItemChoices.SelectedItem = null;
            DirtyTab = true;
        }

        private void btnPlayerRemoveItem_Click(object sender, EventArgs e)
        {
            lbPlayerStartingInventory.Items.Remove(lbPlayerStartingInventory.SelectedItem);
            btnPlayerAddItem.Enabled = !string.IsNullOrWhiteSpace(cmbPlayerInventoryItemChoices.Text) && lbPlayerStartingInventory.Items.Count < nudPlayerInventorySize.Value;
            DirtyTab = true;
        }

        private void lbPlayerStartingInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnPlayerRemoveItem.Enabled = lbPlayerStartingInventory.SelectedItem != null;
        }

        private void btnChangePlayerConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), (!string.IsNullOrWhiteSpace(lblPlayerConsoleRepresentation.Text)) ? lblPlayerConsoleRepresentation.Text[0] : '\0');
            characterMapForm.ShowDialog();
            if (characterMapForm.Saved)
            {
                lblPlayerConsoleRepresentation.Text = characterMapForm.CharacterToSave.ToString();
                DirtyTab = true;
            }
        }

        private void btnChangePlayerConsoleCharacterForeColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblPlayerConsoleRepresentation.ForeColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblPlayerConsoleRepresentation.ForeColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnChangePlayerConsoleCharacterBackColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblPlayerConsoleRepresentation.BackColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblPlayerConsoleRepresentation.BackColor = colorDialog.Color;
                DirtyTab = true;
            }
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
            nudPlayerAttackPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerDefensePerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMovementPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
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
            nudPlayerAttackPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerDefensePerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerMovementPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
            nudPlayerHPRegenerationPerLevelUp.Enabled = chkPlayerCanGainExperience.Checked || nudPlayerMaxLevel.Value > 1;
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

        private void btnPlayerOnTurnStartAction_Click(object sender, EventArgs e)
        {
            var playerClass = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnPlayerOnTurnStartAction, "Turn Start", playerClass.Id, false, false, false, "PlayerClassTurnStart", UsageCriteria.AnyTargetAnyTime);
        }

        private void lbPlayerOnAttackActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditPlayerOnAttackAction.Enabled = lbPlayerOnAttackActions.SelectedItem != null;
            btnRemovePlayerOnAttackAction.Enabled = lbPlayerOnAttackActions.SelectedItem != null;
        }

        private void btnAddPlayerOnAttackAction_Click(object sender, EventArgs e)
        {
            OpenActionEditScreenForListBox(lbPlayerOnAttackActions, "Interact", true, true, true, true, string.Empty, UsageCriteria.FullConditions);

        }

        private void btnEditPlayerOnAttackAction_Click(object sender, EventArgs e)
        {
            OpenActionEditScreenForListBox(lbPlayerOnAttackActions, "Interact", false, true, true, true, string.Empty, UsageCriteria.FullConditions);
        }

        private void btnRemovePlayerOnAttackAction_Click(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show(
                "Do you want to delete the currently-selected Interaction Action?\n\nNote: This is NOT reversible.",
                "Delete Action",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                lbPlayerOnAttackActions.Items.Remove(lbPlayerOnAttackActions.SelectedItem);
                lbPlayerOnAttackActions.SelectedItem = null;
            }
        }

        private void btnPlayerOnAttackedAction_Click(object sender, EventArgs e)
        {
            var playerClass = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnPlayerOnAttackedAction, "Interacted", playerClass.Id, false, false, false, "PlayerClassAttacked", UsageCriteria.AnyTargetAnyTime);
        }

        private void btnPlayerOnDeathAction_Click(object sender, EventArgs e)
        {
            var playerClass = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnPlayerOnDeathAction, "Death", playerClass.Id, false, false, false, "PlayerClassDeath", UsageCriteria.AnyTargetAnyTime);
        }

        #endregion

        #region NPC
        private void LoadNPCInfoFor(NPCInfo npc)
        {
            txtNPCName.Text = npc.Name;
            txtNPCDescription.Text = npc.Description;
            try
            {
                lblNPCConsoleRepresentation.Text = npc.ConsoleRepresentation.Character.ToString();
                lblNPCConsoleRepresentation.BackColor = npc.ConsoleRepresentation.BackgroundColor.ToColor();
                lblNPCConsoleRepresentation.ForeColor = npc.ConsoleRepresentation.ForegroundColor.ToColor();
            }
            catch (Exception ex)
            {
                lblNPCConsoleRepresentation.BackColor = Color.Transparent;
                lblNPCConsoleRepresentation.ForeColor = Color.Black;
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
            nudNPCBaseAttack.Value = npc.BaseAttack;
            nudNPCAttackPerLevelUp.Value = npc.AttackIncreasePerLevel;
            nudNPCBaseDefense.Value = npc.BaseDefense;
            nudNPCDefensePerLevelUp.Value = npc.DefenseIncreasePerLevel;
            nudNPCBaseMovement.Value = npc.BaseMovement;
            nudNPCMovementPerLevelUp.Value = npc.MovementIncreasePerLevel;
            nudNPCBaseHPRegeneration.Value = npc.BaseHPRegeneration;
            nudNPCHPRegenerationPerLevelUp.Value = npc.HPRegenerationIncreasePerLevel;
            txtNPCLevelUpFormula.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCAttackPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCDefensePerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMovementPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPRegenerationPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
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
                catch (Exception ex)
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
            cmbNPCInventoryItemChoices.Items.Clear();
            cmbNPCInventoryItemChoices.Text = "";
            foreach (var itemId in ActiveDungeon.Items.Select(i => i.Id))
            {
                cmbNPCInventoryItemChoices.Items.Add(itemId);
            }
            lbNPCStartingInventory.Items.Clear();
            foreach (var itemId in npc.StartingInventory)
            {
                lbNPCStartingInventory.Items.Add(itemId);
            }
            btnNPCAddItem.Enabled = false;
            btnNPCRemoveItem.Enabled = false;
            btnNPCOnTurnStartAction.Tag = npc.OnTurnStartActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            lbNPCOnAttackActions.Items.Clear();
            lbNPCOnAttackActions.DisplayMember = "Text";
            foreach (var action in npc.OnAttackActions)
            {
                var actionItem = new ListBoxItem
                {
                    Text = action.Name,
                    Tag = action
                };
                lbNPCOnAttackActions.Items.Add(actionItem);
            }
            btnEditNPCOnAttackAction.Enabled = false;
            btnRemoveNPCOnAttackAction.Enabled = false;
            btnNPCOnAttackedAction.Tag = npc.OnAttackedActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            btnNPCOnDeathAction.Tag = npc.OnDeathActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            nudNPCOddsToTargetSelf.Value = npc.AIOddsToUseActionsOnSelf;
        }

        private void SaveNPC(string id)
        {
            if (!ValidateNPCDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save NPC. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var npc = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.NPCs.Find(n => n.Id.Equals(id)) ?? new NPCInfo() { Id = id }
                : (NPCInfo)ActiveNodeTag.DungeonElement;
            npc.Name = txtNPCName.Text;
            npc.Description = txtNPCDescription.Text;
            npc.ConsoleRepresentation = new ConsoleRepresentation
            {
                Character = lblNPCConsoleRepresentation.Text[0],
                BackgroundColor = new GameColor(lblNPCConsoleRepresentation.BackColor),
                ForegroundColor = new GameColor(lblNPCConsoleRepresentation.ForeColor)
            };
            npc.Faction = cmbNPCFaction.Text;
            npc.StartsVisible = chkNPCStartsVisible.Checked;
            npc.KnowsAllCharacterPositions = chkNPCKnowsAllCharacterPositions.Checked;
            npc.ExperienceToLevelUpFormula = txtNPCLevelUpFormula.Text;
            npc.ExperiencePayoutFormula = txtNPCExperiencePayout.Text;
            npc.BaseHP = (int)nudNPCBaseHP.Value;
            npc.BaseAttack = (int)nudNPCBaseAttack.Value;
            npc.BaseDefense = (int)nudNPCBaseDefense.Value;
            npc.BaseMovement = (int)nudNPCBaseMovement.Value;
            npc.BaseHPRegeneration = nudNPCBaseHPRegeneration.Value;

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
            npc.AttackIncreasePerLevel = nudNPCAttackPerLevelUp.Value;
            npc.DefenseIncreasePerLevel = nudNPCDefensePerLevelUp.Value;
            npc.MovementIncreasePerLevel = nudNPCMovementPerLevelUp.Value;
            npc.HPRegenerationIncreasePerLevel = nudNPCHPRegenerationPerLevelUp.Value;

            npc.StartingWeapon = cmbNPCStartingWeapon.Text;
            npc.StartingArmor = cmbNPCStartingArmor.Text;

            npc.InventorySize = (int)nudNPCInventorySize.Value;
            npc.StartingInventory = new();
            foreach (string inventoryItemId in lbNPCStartingInventory.Items)
            {
                npc.StartingInventory.Add(inventoryItemId);
            }

            var onTurnStartAction = btnNPCOnTurnStartAction.Tag as ActionWithEffectsInfo;
            npc.OnTurnStartActions = new();
            if (!string.IsNullOrWhiteSpace(onTurnStartAction?.Effect?.EffectName))
                npc.OnTurnStartActions.Add(onTurnStartAction);

            npc.OnAttackActions = new();
            foreach (ListBoxItem onAttackActionListItem in lbNPCOnAttackActions.Items)
            {
                npc.OnAttackActions.Add(onAttackActionListItem.Tag as ActionWithEffectsInfo);
            }

            var onAttackedAction = btnNPCOnAttackedAction.Tag as ActionWithEffectsInfo;
            npc.OnAttackedActions = new();
            if (!string.IsNullOrWhiteSpace(onAttackedAction?.Effect?.EffectName))
                npc.OnAttackedActions.Add(onAttackedAction);

            var onDeathAction = btnNPCOnDeathAction.Tag as ActionWithEffectsInfo;
            npc.OnDeathActions = new();
            if (!string.IsNullOrWhiteSpace(onDeathAction?.Effect?.EffectName))
                npc.OnDeathActions.Add(onDeathAction);
            npc.AIOddsToUseActionsOnSelf = (int)nudNPCOddsToTargetSelf.Value;

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.NPCs.Any(n => n.Id.Equals(id)))
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
        }

        private void SaveNPCAs()
        {
            if (!ValidateNPCDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save NPC. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = InputBox.Show("Indicate the NPC Identifier", "Save NPC As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.NPCs.Any(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An NPC with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "NPC",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveNPC(inputBoxResult);
                    }
                }
                else
                {
                    SaveNPC(inputBoxResult);
                }
            }
        }

        private bool ValidateNPCDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNPCName.Text))
                errorMessages.Add("Enter an NPC Name first.");
            if (string.IsNullOrWhiteSpace(txtNPCDescription.Text))
                errorMessages.Add("Enter an NPC Description first.");
            if (string.IsNullOrWhiteSpace(lblNPCConsoleRepresentation.Text))
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
            var activeNPC = (ClassInfo)ActiveNodeTag.DungeonElement;
            var deleteNPCPrompt = (IsNewElement)
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
            btnNPCAddItem.Enabled = !string.IsNullOrWhiteSpace(cmbNPCInventoryItemChoices.Text) && lbNPCStartingInventory.Items.Count < nudNPCInventorySize.Value;
        }

        private void cmbNPCInventoryItemChoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNPCAddItem.Enabled = !string.IsNullOrWhiteSpace(cmbNPCInventoryItemChoices.Text) && lbNPCStartingInventory.Items.Count < nudNPCInventorySize.Value;
        }

        private void btnNPCAddItem_Click(object sender, EventArgs e)
        {
            lbNPCStartingInventory.Items.Add(cmbNPCInventoryItemChoices.Text);
            cmbNPCInventoryItemChoices.SelectedItem = null;
            DirtyTab = true;
        }

        private void btnNPCRemoveItem_Click(object sender, EventArgs e)
        {
            lbNPCStartingInventory.Items.Remove(lbNPCStartingInventory.SelectedItem);
            btnNPCAddItem.Enabled = !string.IsNullOrWhiteSpace(cmbNPCInventoryItemChoices.Text) && lbNPCStartingInventory.Items.Count < nudNPCInventorySize.Value;
            DirtyTab = true;
        }

        private void lbNPCStartingInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNPCRemoveItem.Enabled = lbNPCStartingInventory.SelectedItem != null;
        }

        private void btnChangeNPCConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), (!string.IsNullOrWhiteSpace(lblNPCConsoleRepresentation.Text)) ? lblNPCConsoleRepresentation.Text[0] : '\0');
            characterMapForm.ShowDialog();
            if (characterMapForm.Saved)
            {
                lblNPCConsoleRepresentation.Text = characterMapForm.CharacterToSave.ToString();
                DirtyTab = true;
            }
        }

        private void btnChangeNPCConsoleCharacterForeColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblNPCConsoleRepresentation.ForeColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblNPCConsoleRepresentation.ForeColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnChangeNPCConsoleCharacterBackColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblNPCConsoleRepresentation.BackColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblNPCConsoleRepresentation.BackColor = colorDialog.Color;
                DirtyTab = true;
            }
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
            nudNPCAttackPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCDefensePerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCMovementPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
            nudNPCHPRegenerationPerLevelUp.Enabled = chkNPCCanGainExperience.Checked || nudNPCMaxLevel.Value > 1;
        }

        private void txtNPCLevelUpFormula_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtNPCLevelUpFormula.Text;
        }

        private void txtNPCLevelUpFormula_Leave(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[TabTypes.NPC])) return;

            if(!PreviousTextBoxValue.Equals(txtNPCLevelUpFormula.Text))
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
            nudNPCHPPerLevelUp.Enabled = nudNPCMaxLevel.Value > 1 || nudNPCMaxLevel.Value > 1;
            nudNPCAttackPerLevelUp.Enabled = nudNPCMaxLevel.Value > 1 || nudNPCMaxLevel.Value > 1;
            nudNPCDefensePerLevelUp.Enabled = nudNPCMaxLevel.Value > 1 || nudNPCMaxLevel.Value > 1;
            nudNPCMovementPerLevelUp.Enabled = nudNPCMaxLevel.Value > 1 || nudNPCMaxLevel.Value > 1;
            nudNPCHPRegenerationPerLevelUp.Enabled = nudNPCMaxLevel.Value > 1 || nudNPCMaxLevel.Value > 1;
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

        private void btnNPCOnTurnStartAction_Click(object sender, EventArgs e)
        {
            var NPC = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnNPCOnTurnStartAction, "Turn Start", NPC.Id, false, false, false, "NPCTurnStart", UsageCriteria.AnyTargetAnyTime);
        }

        private void lbNPCOnAttackActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditNPCOnAttackAction.Enabled = lbNPCOnAttackActions.SelectedItem != null;
            btnRemoveNPCOnAttackAction.Enabled = lbNPCOnAttackActions.SelectedItem != null;
        }

        private void btnAddNPCOnAttackAction_Click(object sender, EventArgs e)
        {
            OpenActionEditScreenForListBox(lbNPCOnAttackActions, "Interact", true, true, false, true, string.Empty, UsageCriteria.FullConditions);
        }

        private void btnEditNPCOnAttackAction_Click(object sender, EventArgs e)
        {
            OpenActionEditScreenForListBox(lbNPCOnAttackActions, "Interact", false, true, false, true, string.Empty, UsageCriteria.FullConditions);
        }

        private void btnRemoveNPCOnAttackAction_Click(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show(
                "Do you want to delete the currently-selected Interaction Action?\n\nNote: This is NOT reversible.",
                "Delete Action",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                lbNPCOnAttackActions.Items.Remove(lbNPCOnAttackActions.SelectedItem);
                lbNPCOnAttackActions.SelectedItem = null;
            }
        }

        private void btnNPCOnAttackedAction_Click(object sender, EventArgs e)
        {
            var NPC = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnNPCOnAttackedAction, "Interacted", NPC.Id, false, false, false, "NPCAttacked", UsageCriteria.AnyTargetAnyTime);
        }

        private void btnNPCOnDeathAction_Click(object sender, EventArgs e)
        {
            var NPC = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnNPCOnDeathAction, "Death", NPC.Id, false, false, false, "NPCDeath", UsageCriteria.AnyTargetAnyTime);
        }
        #endregion

        #region Item
        private void LoadItemInfoFor(ItemInfo item)
        {
            txtItemName.Text = item.Name;
            txtItemDescription.Text = item.Description;
            try
            {
                lblItemConsoleRepresentation.Text = item.ConsoleRepresentation.Character.ToString();
                lblItemConsoleRepresentation.BackColor = item.ConsoleRepresentation.BackgroundColor.ToColor();
                lblItemConsoleRepresentation.ForeColor = item.ConsoleRepresentation.ForegroundColor.ToColor();
            }
            catch (Exception ex)
            {
                lblItemConsoleRepresentation.BackColor = Color.Transparent;
                lblItemConsoleRepresentation.ForeColor = Color.Black;
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
            btnItemOnSteppedAction.Tag = item.OnItemSteppedActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            btnItemOnUseAction.Tag = item.OnItemUseActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            btnItemOnTurnStartAction.Tag = item.OnTurnStartActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            btnItemOnAttackedAction.Tag = item.OnAttackedActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            lbItemOnAttackActions.Items.Clear();
            lbItemOnAttackActions.DisplayMember = "Text";
            foreach (var action in item.OnAttackActions)
            {
                var actionItem = new ListBoxItem
                {
                    Text = action.Name,
                    Tag = action
                };
                lbItemOnAttackActions.Items.Add(actionItem);
            }
            btnEditItemOnAttackAction.Enabled = false;
            btnRemoveItemOnAttackAction.Enabled = false;
        }

        private void SaveItem(string id)
        {
            if (!ValidateItemDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Item. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var item = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.Items.Find(i => i.Id.Equals(id)) ?? new ItemInfo() { Id = id }
                : (ItemInfo)ActiveNodeTag.DungeonElement;
            item.Name = txtItemName.Text;
            item.Description = txtItemDescription.Text;
            item.ConsoleRepresentation = new ConsoleRepresentation
            {
                Character = lblItemConsoleRepresentation.Text[0],
                BackgroundColor = new GameColor(lblItemConsoleRepresentation.BackColor),
                ForegroundColor = new GameColor(lblItemConsoleRepresentation.ForeColor)
            };
            item.StartsVisible = chkItemStartsVisible.Checked;
            item.CanBePickedUp = chkItemCanBePickedUp.Checked;
            item.EntityType = cmbItemType.Text;
            item.Power = txtItemPower.Text;

            item.OnTurnStartActions = new();
            item.OnAttackActions = new();
            item.OnAttackedActions = new();
            item.OnItemSteppedActions = new();
            item.OnItemUseActions = new();

            if (item.EntityType == "Weapon" || item.EntityType == "Armor")
            {
                var onTurnStartAction = btnItemOnTurnStartAction.Tag as ActionWithEffectsInfo;
                if (!string.IsNullOrWhiteSpace(onTurnStartAction?.Effect?.EffectName))
                    item.OnTurnStartActions.Add(onTurnStartAction);
                var onAttackedAction = btnItemOnAttackedAction.Tag as ActionWithEffectsInfo;
                if (!string.IsNullOrWhiteSpace(onAttackedAction?.Effect?.EffectName))
                    item.OnAttackedActions.Add(onAttackedAction);
                item.OnItemUseActions.Add(DungeonInfoHelpers.CreateEquipAction());
            }
            else if (item.EntityType == "Consumable")
            {
                var onUseAction = btnItemOnUseAction.Tag as ActionWithEffectsInfo;
                if (!string.IsNullOrWhiteSpace(onUseAction?.Effect?.EffectName))
                    item.OnItemUseActions.Add(onUseAction);
            }

            foreach (ListBoxItem onAttackActionListItem in lbItemOnAttackActions.Items)
            {
                item.OnAttackActions.Add(onAttackActionListItem.Tag as ActionWithEffectsInfo);
            }

            var onSteppedAction = btnItemOnSteppedAction.Tag as ActionWithEffectsInfo;
            if (!string.IsNullOrWhiteSpace(onSteppedAction?.Effect?.EffectName))
                item.OnItemSteppedActions.Add(onSteppedAction);

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.Items.Any(i => i.Id.Equals(id)))
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
        }

        private void SaveItemAs()
        {
            if (!ValidateItemDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Item. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = InputBox.Show("Indicate the Item Identifier", "Save Item As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.Items.Any(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Item with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Item",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveItem(inputBoxResult);
                    }
                }
                else
                {
                    SaveItem(inputBoxResult);
                }
                var savedClass = ActiveDungeon.Items.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Items");
                }
            }
        }

        private bool ValidateItemDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtItemName.Text))
                errorMessages.Add("Enter an Item Name first.");
            if (string.IsNullOrWhiteSpace(txtItemDescription.Text))
                errorMessages.Add("Enter an Item Description first.");
            if (string.IsNullOrWhiteSpace(lblItemConsoleRepresentation.Text))
                errorMessages.Add("This Item does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(cmbItemType.Text))
                errorMessages.Add("This Item does not have an Item Type.");
            if (string.IsNullOrWhiteSpace(txtItemPower.Text))
                errorMessages.Add("This Item does not have a Power.");

            return !errorMessages.Any();
        }

        public void DeleteItem()
        {
            var activeItem = (ClassInfo)ActiveNodeTag.DungeonElement;
            var deleteItemPrompt = (IsNewElement)
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
            if (cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor")
            {
                lblItemOnUseAction.Visible = false;
                btnItemOnUseAction.Visible = false;
                btnItemOnUseAction.Tag = null;
                lblItemOnTurnStartAction.Visible = true;
                btnItemOnTurnStartAction.Visible = true;
                lblItemOnAttackActions.Visible = true;
                lblItemOnAttackActions.Text = "Someone equipping it can\ninteract with someone else\nwith the following:";
                lbItemOnAttackActions.Visible = true;
                btnAddItemOnAttackAction.Visible = true;
                btnEditItemOnAttackAction.Visible = true;
                btnRemoveItemOnAttackAction.Visible = true;
                lblItemOnAttackedAction.Visible = true;
                btnItemOnAttackedAction.Visible = true;
            }
            else if (cmbItemType.Text == "Consumable")
            {
                lblItemOnUseAction.Visible = true;
                btnItemOnUseAction.Visible = true;
                lblItemOnTurnStartAction.Visible = false;
                btnItemOnTurnStartAction.Visible = false;
                btnItemOnTurnStartAction.Tag = null;
                lblItemOnAttackActions.Visible = true;
                lblItemOnAttackActions.Text = "Someone can use it to\ninteract with someone else\nwith the following:";
                lbItemOnAttackActions.Visible = true;
                btnAddItemOnAttackAction.Visible = true;
                btnEditItemOnAttackAction.Visible = true;
                btnRemoveItemOnAttackAction.Visible = true;
                lblItemOnAttackedAction.Visible = false;
                btnItemOnAttackedAction.Visible = false;
                btnItemOnAttackedAction.Tag = null;
            }
            else
            {
                lblItemOnUseAction.Visible = false;
                btnItemOnUseAction.Visible = false;
                btnItemOnUseAction.Tag = null;
                lblItemOnTurnStartAction.Visible = false;
                btnItemOnTurnStartAction.Visible = false;
                btnItemOnTurnStartAction.Tag = null;
                lblItemOnAttackActions.Visible = false;
                lbItemOnAttackActions.Visible = false;
                btnAddItemOnAttackAction.Visible = false;
                btnEditItemOnAttackAction.Visible = false;
                btnRemoveItemOnAttackAction.Visible = false;
                lblItemOnAttackedAction.Visible = false;
                btnItemOnAttackedAction.Visible = false;
                btnItemOnAttackedAction.Tag = null;
            }
        }

        private void cmbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((PreviousItemType.Equals("Consumable") && (cmbItemType.Text.Equals("Weapon") || cmbItemType.Text.Equals("Armor")))
                || (PreviousItemType.Equals("Weapon") || PreviousItemType.Equals("Armor")) && (cmbItemType.Text.Equals("Consumable")))
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

        private void btnChangeItemConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), (!string.IsNullOrWhiteSpace(lblItemConsoleRepresentation.Text)) ? lblItemConsoleRepresentation.Text[0] : '\0');
            characterMapForm.ShowDialog();
            if (characterMapForm.Saved)
            {
                lblItemConsoleRepresentation.Text = characterMapForm.CharacterToSave.ToString();
                DirtyTab = true;
            }
        }

        private void btnChangeItemConsoleCharacterForeColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblItemConsoleRepresentation.ForeColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblItemConsoleRepresentation.ForeColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnChangeItemConsoleCharacterBackColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblItemConsoleRepresentation.BackColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblItemConsoleRepresentation.BackColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnItemOnSteppedAction_Click(object sender, EventArgs e)
        {
            var item = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnItemOnSteppedAction, "Stepped On", item.Id, false, false, false, "ItemStepped", UsageCriteria.AnyTargetAnyTime);
        }

        private void btnItemOnUseAction_Click(object sender, EventArgs e)
        {
            var item = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnItemOnUseAction, "Used", item.Id, true, cmbItemType.Text == "Weapon" || cmbItemType.Text == "Armor", false, "ItemUse", UsageCriteria.AnyTargetAnyTime);
        }

        private void lbItemOnAttackActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditItemOnAttackAction.Enabled = lbItemOnAttackActions.SelectedItem != null;
            btnRemoveItemOnAttackAction.Enabled = lbItemOnAttackActions.SelectedItem != null;
        }

        private void btnAddItemOnAttackAction_Click(object sender, EventArgs e)
        {
            OpenActionEditScreenForListBox(lbItemOnAttackActions, "Owner Interact", true, true, true, true, string.Empty, UsageCriteria.FullConditions);
        }

        private void btnEditItemOnAttackAction_Click(object sender, EventArgs e)
        {
            OpenActionEditScreenForListBox(lbItemOnAttackActions, "Owner Interact", false, true, true, true, string.Empty, UsageCriteria.FullConditions);
        }

        private void btnRemoveItemOnAttackAction_Click(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show(
                "Do you want to delete the currently-selected Interaction Action?\n\nNote: This is NOT reversible.",
                "Delete Action",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                lbItemOnAttackActions.Items.Remove(lbItemOnAttackActions.SelectedItem);
                lbItemOnAttackActions.SelectedItem = null;
            }
        }

        private void btnItemOnTurnStartAction_Click(object sender, EventArgs e)
        {
            var item = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnItemOnTurnStartAction, "Owner Turn Start", item.Id, false, false, false, "ItemTurnStart", UsageCriteria.AnyTargetAnyTime);
        }

        private void btnItemOnAttackedAction_Click(object sender, EventArgs e)
        {
            var item = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnItemOnAttackedAction, "Owner Interacted", item.Id, false, false, false, "ItemTurnStart", UsageCriteria.AnyTargetAnyTime);
        }
        #endregion

        #region Trap

        private void LoadTrapInfoFor(TrapInfo trap)
        {
            txtTrapName.Text = trap.Name;
            txtTrapDescription.Text = trap.Description;
            try
            {
                lblTrapConsoleRepresentation.Text = trap.ConsoleRepresentation.Character.ToString();
                lblTrapConsoleRepresentation.BackColor = trap.ConsoleRepresentation.BackgroundColor.ToColor();
                lblTrapConsoleRepresentation.ForeColor = trap.ConsoleRepresentation.ForegroundColor.ToColor();
            }
            catch (Exception ex)
            {
                lblTrapConsoleRepresentation.BackColor = Color.Transparent;
                lblTrapConsoleRepresentation.ForeColor = Color.Black;
            }
            txtTrapPower.Text = trap.Power;
            chkTrapStartsVisible.Checked = trap.StartsVisible;
            var toolTip = new ToolTip();
            toolTip.SetToolTip(chkTrapStartsVisible, "The 'spirit' of a Trap is that it spawns invisible.\n\nHowever, it can be enabled for debugging purposes.");
            btnTrapOnSteppedAction.Tag = trap.OnItemSteppedActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
        }

        private void SaveTrap(string id)
        {
            if (!ValidateTrapDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Trap. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var trap = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.Traps.Find(t => t.Id.Equals(id)) ?? new TrapInfo() { Id = id }
                : (TrapInfo)ActiveNodeTag.DungeonElement;
            trap.Name = txtTrapName.Text;
            trap.Description = txtTrapDescription.Text;
            trap.ConsoleRepresentation = new ConsoleRepresentation
            {
                Character = lblTrapConsoleRepresentation.Text[0],
                BackgroundColor = new GameColor(lblTrapConsoleRepresentation.BackColor),
                ForegroundColor = new GameColor(lblTrapConsoleRepresentation.ForeColor)
            };
            trap.StartsVisible = chkTrapStartsVisible.Checked;
            trap.Power = txtTrapPower.Text;

            trap.OnItemSteppedActions = new();

            var onSteppedAction = btnTrapOnSteppedAction.Tag as ActionWithEffectsInfo;
            if (!string.IsNullOrWhiteSpace(onSteppedAction?.Effect?.EffectName))
                trap.OnItemSteppedActions.Add(onSteppedAction);

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.Traps.Any(t => t.Id.Equals(id)))
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
        }

        private void SaveTrapAs()
        {
            if (!ValidateTrapDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Trap. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = InputBox.Show("Indicate the Trap Identifier", "Save Trap As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.Traps.Any(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Trap with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Trap",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveTrap(inputBoxResult);
                    }
                }
                else
                {
                    SaveTrap(inputBoxResult);
                }
                var savedClass = ActiveDungeon.Traps.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Traps");
                }
            }
        }

        private bool ValidateTrapDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtTrapName.Text))
                errorMessages.Add("Enter an Trap Name first.");
            if (string.IsNullOrWhiteSpace(txtTrapDescription.Text))
                errorMessages.Add("Enter an Trap Description first.");
            if (string.IsNullOrWhiteSpace(lblTrapConsoleRepresentation.Text))
                errorMessages.Add("This Trap does not have a Console Representation character.");
            if (string.IsNullOrWhiteSpace(txtTrapPower.Text))
                errorMessages.Add("This Trap does not have a Power.");

            return !errorMessages.Any();
        }

        public void DeleteTrap()
        {
            var activeTrap = (ClassInfo)ActiveNodeTag.DungeonElement;
            var deleteTrapPrompt = (IsNewElement)
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

        private void btnChangeTrapConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), (!string.IsNullOrWhiteSpace(lblTrapConsoleRepresentation.Text)) ? lblTrapConsoleRepresentation.Text[0] : '\0');
            characterMapForm.ShowDialog();
            if (characterMapForm.Saved)
            {
                lblTrapConsoleRepresentation.Text = characterMapForm.CharacterToSave.ToString();
                DirtyTab = true;
            }
        }

        private void btnChangeTrapConsoleCharacterForeColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblTrapConsoleRepresentation.ForeColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblTrapConsoleRepresentation.ForeColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnChangeTrapConsoleCharacterBackColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblTrapConsoleRepresentation.BackColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblTrapConsoleRepresentation.BackColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnTrapOnSteppedAction_Click(object sender, EventArgs e)
        {
            var trap = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnTrapOnSteppedAction, "Stepped On", trap.Id, false, false, false, "TrapStepped", UsageCriteria.AnyTargetAnyTime);
        }

        #endregion

        #region Altered Status

        private void LoadAlteredStatusInfoFor(AlteredStatusInfo alteredStatus)
        {
            txtAlteredStatusName.Text = alteredStatus.Name;
            txtAlteredStatusDescription.Text = alteredStatus.Description;
            try
            {
                lblAlteredStatusConsoleRepresentation.Text = alteredStatus.ConsoleRepresentation.Character.ToString();
                lblAlteredStatusConsoleRepresentation.BackColor = alteredStatus.ConsoleRepresentation.BackgroundColor.ToColor();
                lblAlteredStatusConsoleRepresentation.ForeColor = alteredStatus.ConsoleRepresentation.ForegroundColor.ToColor();
            }
            catch (Exception ex)
            {
                lblAlteredStatusConsoleRepresentation.BackColor = Color.Transparent;
                lblAlteredStatusConsoleRepresentation.ForeColor = Color.Black;
            }
            chkAlteredStatusCanStack.Checked = alteredStatus.CanStack;
            chkAlteredStatusCanOverwrite.Checked = alteredStatus.CanOverwrite;
            chkAlteredStatusCleanseOnFloorChange.Checked = alteredStatus.CleanseOnFloorChange;
            chkAlteredStatusCleansedOnCleanseActions.Checked = alteredStatus.CleansedByCleanseActions;
            btnAlteredStatusOnApplyAction.Tag = alteredStatus.OnStatusApplyActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
            btnAlteredStatusOnTurnStartAction.Tag = alteredStatus.OnTurnStartActions.ElementAtOrDefault(0) ?? new ActionWithEffectsInfo();
        }

        private void SaveAlteredStatus(string id)
        {
            if (!ValidateAlteredStatusDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save Altered Status. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Altered Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var alteredStatus = !string.IsNullOrWhiteSpace(id)
                ? ActiveDungeon.AlteredStatuses.Find(als => als.Id.Equals(id)) ?? new AlteredStatusInfo() { Id = id }
                : (AlteredStatusInfo)ActiveNodeTag.DungeonElement;
            alteredStatus.Name = txtAlteredStatusName.Text;
            alteredStatus.Description = txtAlteredStatusDescription.Text;
            alteredStatus.ConsoleRepresentation = new ConsoleRepresentation
            {
                Character = lblAlteredStatusConsoleRepresentation.Text[0],
                BackgroundColor = new GameColor(lblAlteredStatusConsoleRepresentation.BackColor),
                ForegroundColor = new GameColor(lblAlteredStatusConsoleRepresentation.ForeColor)
            };
            alteredStatus.CanStack = chkAlteredStatusCanStack.Checked;
            alteredStatus.CanOverwrite = chkAlteredStatusCanOverwrite.Checked;
            alteredStatus.CleanseOnFloorChange = chkAlteredStatusCleanseOnFloorChange.Checked;
            alteredStatus.CleansedByCleanseActions = chkAlteredStatusCleansedOnCleanseActions.Checked;

            alteredStatus.OnTurnStartActions = new();
            alteredStatus.OnStatusApplyActions = new();

            var onStatusApplyAction = btnAlteredStatusOnApplyAction.Tag as ActionWithEffectsInfo;
            if (!string.IsNullOrWhiteSpace(onStatusApplyAction?.Effect?.EffectName))
                alteredStatus.OnStatusApplyActions.Add(onStatusApplyAction);

            var onTurnStartAction = btnAlteredStatusOnTurnStartAction.Tag as ActionWithEffectsInfo;
            if (!string.IsNullOrWhiteSpace(onTurnStartAction?.Effect?.EffectName))
                alteredStatus.OnTurnStartActions.Add(onTurnStartAction);

            if (!string.IsNullOrWhiteSpace(id) && !ActiveDungeon.AlteredStatuses.Any(als => als.Id.Equals(id)))
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
        }

        private void SaveAlteredStatusAs()
        {
            if (!ValidateAlteredStatusDataForSave(out List<string> errorMessages))
            {
                MessageBox.Show(
                    $"Cannot save AlteredStatus. Please correct the following errors:\n- {string.Join("\n- ", errorMessages)}",
                    "Save Altered Status",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var inputBoxResult = InputBox.Show("Indicate the Altered Status Identifier", "Save Altered Status As");
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.AlteredStatuses.Any(als => als.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Altered Status with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Altered Status",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveAlteredStatus(inputBoxResult);
                    }
                }
                else
                {
                    SaveAlteredStatus(inputBoxResult);
                }
                var savedClass = ActiveDungeon.AlteredStatuses.Find(pc => pc.Id.Equals(inputBoxResult));
                if (savedClass != null)
                {
                    var nodeText = $"{savedClass.ConsoleRepresentation.Character} - {savedClass.Id}";
                    SelectNodeIfExists(nodeText, "Altered Statuses");
                }
            }
        }

        private bool ValidateAlteredStatusDataForSave(out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtAlteredStatusName.Text))
                errorMessages.Add("Enter an Altered Status Name first.");
            if (string.IsNullOrWhiteSpace(txtAlteredStatusDescription.Text))
                errorMessages.Add("Enter an Altered Status Description first.");
            if (string.IsNullOrWhiteSpace(lblAlteredStatusConsoleRepresentation.Text))
                errorMessages.Add("This Altered Status does not have a Console Representation character.");

            return !errorMessages.Any();
        }

        public void DeleteAlteredStatus()
        {
            var activeAlteredStatus = (ClassInfo)ActiveNodeTag.DungeonElement;
            var deleteAlteredStatusPrompt = (IsNewElement)
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

        private void btnChangeAlteredStatusConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), (!string.IsNullOrWhiteSpace(lblAlteredStatusConsoleRepresentation.Text)) ? lblAlteredStatusConsoleRepresentation.Text[0] : '\0');
            characterMapForm.ShowDialog();
            if (characterMapForm.Saved)
            {
                lblAlteredStatusConsoleRepresentation.Text = characterMapForm.CharacterToSave.ToString();
                DirtyTab = true;
            }
        }

        private void btnChangeAlteredStatusConsoleCharacterForeColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblAlteredStatusConsoleRepresentation.ForeColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblAlteredStatusConsoleRepresentation.ForeColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnChangeAlteredStatusConsoleCharacterBackColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = lblAlteredStatusConsoleRepresentation.BackColor;
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblAlteredStatusConsoleRepresentation.BackColor = colorDialog.Color;
                DirtyTab = true;
            }
        }

        private void btnAlteredStatusOnApplyAction_Click(object sender, EventArgs e)
        {
            var alteredStatus = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnAlteredStatusOnApplyAction, "Apply Status", alteredStatus.Id, false, false, false, "StatusApply", UsageCriteria.AnyTargetAnyTime);
        }

        private void btnAlteredStatusOnTurnStartAction_Click(object sender, EventArgs e)
        {
            var alteredStatus = ((ClassInfo)ActiveNodeTag.DungeonElement);
            OpenActionEditScreenForButton(btnAlteredStatusOnTurnStartAction, "Inflicted Turn Start", alteredStatus.Id, false, false, false, "StatusTurnStart", UsageCriteria.AnyTargetAnyTime);
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

    public class ListBoxItem
    {
        public string Text { get; set; }
        public object Tag { get; set; }

        public override string ToString() => Text;
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
        FloorInfo,
        FactionInfo,
        PlayerClass,
        NPC,
        Item,
        Trap,
        AlteredStatus,
        Validator
    }
}
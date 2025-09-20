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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using RogueCustomsDungeonEditor.Controls;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsDungeonEditor.Clipboard;
using RogueCustomsDungeonEditor.Controls.Tabs;

namespace RogueCustomsDungeonEditor
{
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable S4144 // Methods should not have identical implementations
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public partial class frmMain : Form
    {
        private readonly Dictionary<RogueTabTypes, TabPage> TabsForNodeTypes = new();
        private readonly List<string> MandatoryLocaleKeys = new();
        private readonly List<string> BaseLocaleLanguages = new();
        private readonly LocaleInfo LocaleTemplate;
        private readonly List<RoomDispositionData> RoomDispositionData = new();
        private readonly List<EffectTypeData> EffectParamData = new();
        private readonly Dictionary<string, string> BaseSightRangeDisplayNames = new Dictionary<string, string> {
            { "FullMap", "The whole map" },
            { "FullRoom", "The whole room" },
            { "FlatNumber", "A flat distance" }
        };

        private readonly Dictionary<string, string> NPCAITypeDisplayNames = new Dictionary<string, string> {
            { "Default", "Default strategy" },
            { "Random", "Heavily Randomized" },
            { "CostEfficient", "Cost-efficiently" },
            { "AllOut", "All-out" },
            { "Null", "None (NPC does nothing)" }
        };

        private DungeonInfo ActiveDungeon;
        private NodeTag ActiveNodeTag;
        private TreeNode ActiveNode;

        private bool DirtyTab;
        private bool DirtyDungeon;
        private bool AutomatedChange; // If true, does not set DirtyTab to true
        private bool IsNewObject;
        private bool PassedValidation;
        private bool ReclickOnFailedSave;
        private bool ReselectingNode;
        private bool ClickedSave;

        private string PreviousTextBoxValue;
        private string PreviousItemType;
        private string DungeonPath;

        public frmMain()
        {
            InitializeComponent();
            TabsForNodeTypes[RogueTabTypes.BasicInfo] = tpBasicInfo;
            TabsForNodeTypes[RogueTabTypes.Locales] = tpLocales;
            TabsForNodeTypes[RogueTabTypes.TileTypeInfo] = tpTileTypeInfo;
            TabsForNodeTypes[RogueTabTypes.TileSetInfo] = tpTileSetInfos;
            TabsForNodeTypes[RogueTabTypes.FloorInfo] = tpFloorInfos;
            TabsForNodeTypes[RogueTabTypes.FactionInfo] = tpFactionInfos;
            TabsForNodeTypes[RogueTabTypes.StatInfo] = tbStatInfos;
            TabsForNodeTypes[RogueTabTypes.ElementInfo] = tbElementInfos;
            TabsForNodeTypes[RogueTabTypes.ActionSchoolsInfo] = tbActionSchoolInfos;
            TabsForNodeTypes[RogueTabTypes.LootTableInfo] = tpLootTableInfos;
            TabsForNodeTypes[RogueTabTypes.CurrencyInfo] = tpCurrencyInfo;
            TabsForNodeTypes[RogueTabTypes.AffixInfo] = tpAffixes;
            TabsForNodeTypes[RogueTabTypes.NPCModifierInfo] = tpNPCModifiers;
            TabsForNodeTypes[RogueTabTypes.QualityLevelInfo] = tpQualityLevels;
            TabsForNodeTypes[RogueTabTypes.ItemSlotInfo] = tpItemSlotInfos;
            TabsForNodeTypes[RogueTabTypes.ItemTypeInfo] = tpItemTypeInfos;
            TabsForNodeTypes[RogueTabTypes.PlayerClass] = tpPlayerClass;
            TabsForNodeTypes[RogueTabTypes.NPC] = tpNPC;
            TabsForNodeTypes[RogueTabTypes.Item] = tpItem;
            TabsForNodeTypes[RogueTabTypes.Trap] = tpTrap;
            TabsForNodeTypes[RogueTabTypes.AlteredStatus] = tpAlteredStatus;
            TabsForNodeTypes[RogueTabTypes.Scripts] = tpScripts;
            TabsForNodeTypes[RogueTabTypes.Validator] = tpValidation;
            tbTabs.TabPages.Clear();

            ofdDungeon.InitialDirectory = Application.StartupPath;
            sfdDungeon.InitialDirectory = Application.StartupPath;

            MandatoryLocaleKeys.AddRange(File.ReadAllLines("./Resources/MandatoryLocaleKeys.txt"));
            BaseLocaleLanguages.AddRange(File.ReadAllLines("./Resources/BaseLocaleLanguages.txt"));

            CharacterMapInputBox.ConstructCharacterMap();

            var jsonString = File.ReadAllText("./EffectInfos/EffectTypeData.json");
            EffectParamData = JsonSerializer.Deserialize<List<EffectTypeData>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            jsonString = File.ReadAllText("./FloorInfos/RoomDispositionData.json");
            RoomDispositionData = JsonSerializer.Deserialize<List<RoomDispositionData>>(jsonString, new JsonSerializerOptions
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

            foreach (var roomTile in RoomDispositionData)
            {
                roomTile.TileImage = Image.FromFile(roomTile.ImagePath);
                roomTile.RoomDispositionIndicator = Enum.Parse<RoomDispositionType>(roomTile.InternalName);
            }
            BasicInformationTab.TabInfoChanged += BasicInformationTab_TabInfoChanged;
            LocaleEntriesTab.TabInfoChanged += LocaleEntriesTab_TabInfoChanged;
            TileTypeTab.TabInfoChanged += TileTypeTab_TabInfoChanged;
            TilesetTab.TabInfoChanged += TilesetTab_TabInfoChanged;
            FloorGroupTab.TabInfoChanged += FloorGroupTab_TabInfoChanged;
            FactionTab.TabInfoChanged += FactionTab_TabInfoChanged;
            StatTab.TabInfoChanged += StatTab_TabInfoChanged;
            ElementTab.TabInfoChanged += ElementTab_TabInfoChanged;
            PlayerClassTab.TabInfoChanged += PlayerClassTab_TabInfoChanged;
            NPCTab.TabInfoChanged += NPCTab_TabInfoChanged;
            ItemTab.TabInfoChanged += ItemTab_TabInfoChanged;
            TrapTab.TabInfoChanged += TrapTab_TabInfoChanged;
            AlteredStatusTab.TabInfoChanged += AlteredStatusTab_TabInfoChanged;
            ScriptsTab.TabInfoChanged += ScriptsTab_TabInfoChanged;
            ActionSchoolsTab.TabInfoChanged += ActionSchoolsTab_TabInfoChanged;
            AffixTab.TabInfoChanged += AffixTab_TabInfoChanged;
            NPCModifiersTab.TabInfoChanged += NPCModifiersTab_TabInfoChanged;
            QualityLevelsTab.TabInfoChanged += QualityLevelsTab_TabInfoChanged;
            LootTableTab.TabInfoChanged += LootTableTab_TabInfoChanged;
            ItemTypesTab.TabInfoChanged += ItemTypesTab_TabInfoChanged;
            ItemSlotsTab.TabInfoChanged += ItemSlotsTab_TabInfoChanged;
            ValidatorTab.OnValidationComplete += ValidatorTab_OnValidationComplete;
            ValidatorTab.OnError += ValidatorTab_OnError;
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
            if (ClickedSave) return;
            if (ActiveNode == e.Node) return;
            ReselectingNode = true;
            if (e.Node.Tag is NodeTag tag)
            {
                if (DirtyTab)
                {
                    var messageBoxResult = MessageBox.Show(
                        "The currently-opened object has unsaved changes.\n\nDo you wish to save them?\n\n(Selecting \"No\" will make you lose all changes)",
                        "Unsaved Changes",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (messageBoxResult == DialogResult.Yes)
                    {
                        SaveObject();
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
                    if (tabPage != TabsForNodeTypes[RogueTabTypes.Validator])
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
                IsNewObject = false;
                LoadTabDataForTag(tag);
                tsbSaveElementAs.Visible = ActiveNodeTag.TabToOpen != RogueTabTypes.BasicInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.AffixInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.NPCModifierInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.QualityLevelInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.ActionSchoolsInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.CurrencyInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.ItemTypeInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.ItemSlotInfo
                    && ActiveNodeTag.TabToOpen != RogueTabTypes.Scripts;
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
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.Locales;
                        break;
                    case "Special Tiles":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.TileTypeInfo;
                        break;
                    case "Tilesets":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.TileSetInfo;
                        break;
                    case "Floor Groups":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.FloorInfo;
                        break;
                    case "Factions":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.FactionInfo;
                        break;
                    case "Custom Stats":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.StatInfo;
                        break;
                    case "Action Schools":
                        tssDungeonElement.Visible = false;
                        tsbAddElement.Visible = false;
                        tsbSaveElement.Visible = true;
                        tsbSaveElementAs.Visible = false;
                        tsbDeleteElement.Visible = false;
                        break;
                    case "Affixes":
                        tssDungeonElement.Visible = false;
                        tsbAddElement.Visible = false;
                        tsbSaveElement.Visible = true;
                        tsbSaveElementAs.Visible = false;
                        tsbDeleteElement.Visible = false;
                        break;
                    case "NPC Modifiers":
                        tssDungeonElement.Visible = false;
                        tsbAddElement.Visible = false;
                        tsbSaveElement.Visible = true;
                        tsbSaveElementAs.Visible = false;
                        tsbDeleteElement.Visible = false;
                        break;
                    case "Quality Levels":
                        tssDungeonElement.Visible = false;
                        tsbAddElement.Visible = false;
                        tsbSaveElement.Visible = true;
                        tsbSaveElementAs.Visible = false;
                        tsbDeleteElement.Visible = false;
                        break;
                    case "Attack Elements":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.ElementInfo;
                        break;
                    case "Loot Tables":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.StatInfo;
                        break;
                    case "Player Classes":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.PlayerClass;
                        break;
                    case "NPCs":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.NPC;
                        break;
                    case "Items":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.Item;
                        break;
                    case "Traps":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.Trap;
                        break;
                    case "Altered Statuses":
                        tssDungeonElement.Visible = true;
                        tsbAddElement.Visible = true;
                        tsbSaveElement.Visible = tsbSaveElementAs.Visible = tsbDeleteElement.Visible = e.Node.Nodes.Count > 0 && ActiveNodeTag.TabToOpen == RogueTabTypes.AlteredStatus;
                        break;
                    case "Scripts":
                        tssDungeonElement.Visible = false;
                        tsbAddElement.Visible = false;
                        tsbSaveElement.Visible = true;
                        tsbSaveElementAs.Visible = false;
                        tsbDeleteElement.Visible = false;
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
                Tag = new NodeTag { TabToOpen = RogueTabTypes.BasicInfo, DungeonElement = null },
                Name = "Basic Information"
            };
            tvDungeonInfo.Nodes.Add(basicInfoNode);

            var localesRootNode = new TreeNode("Locales");
            foreach (var locale in ActiveDungeon.Locales)
            {
                var localeNode = new TreeNode(locale.Language)
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.Locales, DungeonElement = locale },
                    Name = locale.Language
                };
                localesRootNode.Nodes.Add(localeNode);
            }
            tvDungeonInfo.Nodes.Add(localesRootNode);

            var tileTypesRootNode = new TreeNode("Special Tiles");
            foreach (var tileTypeInfo in ActiveDungeon.TileTypeInfos.Where(tt => !FormConstants.DefaultTileTypes.Any(dtt => dtt.Equals(tt.Id, StringComparison.InvariantCultureIgnoreCase))))
            {
                var tileTypeInfoNode = new TreeNode(tileTypeInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.TileTypeInfo, DungeonElement = tileTypeInfo },
                    Name = tileTypeInfo.Id
                };
                tileTypesRootNode.Nodes.Add(tileTypeInfoNode);
            }
            tvDungeonInfo.Nodes.Add(tileTypesRootNode);

            var tileSetsRootNode = new TreeNode("Tilesets");
            foreach (var tileSetInfo in ActiveDungeon.TileSetInfos)
            {
                var tileSetInfoNode = new TreeNode(tileSetInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.TileSetInfo, DungeonElement = tileSetInfo },
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
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.FloorInfo, DungeonElement = floorInfo }
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
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.FactionInfo, DungeonElement = factionInfo },
                    Name = factionInfo.Id
                };
                factionInfosRootNode.Nodes.Add(factionInfoNode);
            }
            tvDungeonInfo.Nodes.Add(factionInfosRootNode);

            var statInfosRootNode = new TreeNode("Custom Stats");
            foreach (var statInfo in ActiveDungeon.CharacterStats.Where(cs => !FormConstants.DefaultStats.Any(ms => ms.Equals(cs.Id, StringComparison.InvariantCultureIgnoreCase))))
            {
                var statInfoNode = new TreeNode(statInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.StatInfo, DungeonElement = statInfo },
                    Name = statInfo.Id
                };
                statInfosRootNode.Nodes.Add(statInfoNode);
            }
            tvDungeonInfo.Nodes.Add(statInfosRootNode);

            var elementInfosRootNode = new TreeNode("Attack Elements");
            foreach (var elementInfo in ActiveDungeon.ElementInfos)
            {
                var elementInfoNode = new TreeNode(elementInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.ElementInfo, DungeonElement = elementInfo },
                    Name = elementInfo.Id
                };
                elementInfosRootNode.Nodes.Add(elementInfoNode);
            }
            tvDungeonInfo.Nodes.Add(elementInfosRootNode);

            var actionSchoolsInfoNode = new TreeNode("Action Schools")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.ActionSchoolsInfo, DungeonElement = null },
                Name = "Action Schools"
            };
            tvDungeonInfo.Nodes.Add(actionSchoolsInfoNode);

            var lootTableInfosRootNode = new TreeNode("Loot Tables");
            foreach (var lootTableInfo in ActiveDungeon.LootTableInfos)
            {
                var lootTableInfoNode = new TreeNode(lootTableInfo.Id)
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.LootTableInfo, DungeonElement = lootTableInfo },
                    Name = lootTableInfo.Id
                };
                lootTableInfosRootNode.Nodes.Add(lootTableInfoNode);
            }
            tvDungeonInfo.Nodes.Add(lootTableInfosRootNode);

            var currencyInfoNode = new TreeNode("Currency")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.CurrencyInfo, DungeonElement = null },
                Name = "Currency"
            };
            tvDungeonInfo.Nodes.Add(currencyInfoNode);

            var npcModifierInfoNode = new TreeNode("NPC Modifiers")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.NPCModifierInfo, DungeonElement = null },
                Name = "NPC Modifiers"
            };
            tvDungeonInfo.Nodes.Add(npcModifierInfoNode);

            var affixInfoNode = new TreeNode("Affixes")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.AffixInfo, DungeonElement = null },
                Name = "Affixes"
            };
            tvDungeonInfo.Nodes.Add(affixInfoNode);

            var qualityLevelInfoNode = new TreeNode("Quality Levels")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.QualityLevelInfo, DungeonElement = null },
                Name = "Quality Levels"
            };
            tvDungeonInfo.Nodes.Add(qualityLevelInfoNode);

            var itemSlotsInfoNode = new TreeNode("Item Slots")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.ItemSlotInfo, DungeonElement = null },
                Name = "Item Slots"
            };
            tvDungeonInfo.Nodes.Add(itemSlotsInfoNode);

            var itemTypeInfoNode = new TreeNode("Item Types")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.ItemTypeInfo, DungeonElement = null },
                Name = "Item Types"
            };
            tvDungeonInfo.Nodes.Add(itemTypeInfoNode);

            var playerClassRootNode = new TreeNode("Player Classes");
            foreach (var playerClass in ActiveDungeon.PlayerClasses)
            {
                var playerClassNode = new TreeNode($"{playerClass.ConsoleRepresentation.Character} - {playerClass.Id}")
                {
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.PlayerClass, DungeonElement = playerClass }
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
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.NPC, DungeonElement = npc }
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
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.Item, DungeonElement = item }
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
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.Trap, DungeonElement = trap }
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
                    Tag = new NodeTag { TabToOpen = RogueTabTypes.AlteredStatus, DungeonElement = alteredStatus }
                };
                alteredStatusNode.Name = alteredStatusNode.Text;
                alteredStatusRootNode.Nodes.Add(alteredStatusNode);
            }
            tvDungeonInfo.Nodes.Add(alteredStatusRootNode);

            var scriptsInfoNode = new TreeNode("Scripts")
            {
                Tag = new NodeTag { TabToOpen = RogueTabTypes.Scripts, DungeonElement = null },
                Name = "Scripts"
            };
            tvDungeonInfo.Nodes.Add(scriptsInfoNode);

            tvDungeonInfo.Focus();
        }

        private void SelectNodeIfExists(string nodeText, string parentNodeText)
        {
            if (ReselectingNode) return;
            var matchingNodes = tvDungeonInfo.Nodes.Find(nodeText, true).Where(n => (n.Parent == null && n.Tag != null && string.IsNullOrWhiteSpace(parentNodeText)) || n.Parent.Text.Equals(parentNodeText)).ToList();
            if (matchingNodes.Count != 0)
            {
                matchingNodes[0].Parent?.Expand();
                tvDungeonInfo.SelectedNode = matchingNodes[0];
                tvDungeonInfo.SelectedNode.EnsureVisible();
                tvDungeonInfo.Focus();
                LoadTabDataForTag(matchingNodes[0].Tag as NodeTag);
                DirtyTab = false;
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
            ActiveDungeon.Version = EngineConstants.CurrentDungeonJsonVersion;
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
                    ActiveDungeon = jsonString.ConvertDungeonInfoIfNeeded(LocaleTemplate, MandatoryLocaleKeys);
                    ActiveDungeon.PruneNullActions();
                    tbTabs.TabPages.Clear();
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
                        $"The currently-opened object has unsaved changes.\n\nDo you wish to save them before saving the Dungeon?",
                        "Save Dungeon",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes && !SaveObject())
                    {
                        MessageBox.Show($"The currently-opened object could not be saved due to errors. Please check it.\n\nThe Dungeon saving process will proceed nonetheless.", "Save Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DirtyTab = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Attempting to save this Dungeon to {filePath} threw an error:\n\n{ex.Message}\n\nPlease check.", "Save Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbAddElement_Click(object sender, EventArgs e)
        {
            var tabToOpen = RogueTabTypes.BasicInfo;
            if (tvDungeonInfo.SelectedNode?.Tag != null)
            {
                tabToOpen = ActiveNodeTag.TabToOpen;
            }
            else
            {
                switch (tvDungeonInfo.SelectedNode.Text)
                {
                    case "Locales":
                        tabToOpen = RogueTabTypes.Locales;
                        break;
                    case "Special Tiles":
                        tabToOpen = RogueTabTypes.TileTypeInfo;
                        break;
                    case "Tilesets":
                        tabToOpen = RogueTabTypes.TileSetInfo;
                        break;
                    case "Floor Groups":
                        tabToOpen = RogueTabTypes.FloorInfo;
                        break;
                    case "Factions":
                        tabToOpen = RogueTabTypes.FactionInfo;
                        break;
                    case "Custom Stats":
                        tabToOpen = RogueTabTypes.StatInfo;
                        break;
                    case "Attack Elements":
                        tabToOpen = RogueTabTypes.ElementInfo;
                        break;
                    case "Loot Tables":
                        tabToOpen = RogueTabTypes.LootTableInfo;
                        break;
                    case "Player Classes":
                        tabToOpen = RogueTabTypes.PlayerClass;
                        break;
                    case "NPCs":
                        tabToOpen = RogueTabTypes.NPC;
                        break;
                    case "Items":
                        tabToOpen = RogueTabTypes.Item;
                        break;
                    case "Traps":
                        tabToOpen = RogueTabTypes.Trap;
                        break;
                    case "Altered Statuses":
                        tabToOpen = RogueTabTypes.AlteredStatus;
                        break;
                }
                tbTabs.TabPages.Clear();
                tbTabs.TabPages.Add(TabsForNodeTypes[tabToOpen]);
            }
            ActiveNodeTag = new NodeTag { TabToOpen = tabToOpen };
            switch (tabToOpen)
            {
                case RogueTabTypes.Locales:
                    ActiveNodeTag.DungeonElement = LocaleTemplate.Clone(MandatoryLocaleKeys);
                    break;
                case RogueTabTypes.TileTypeInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateTileTypeTemplate();
                    break;
                case RogueTabTypes.TileSetInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateDefaultTileSet();
                    break;
                case RogueTabTypes.FloorInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateFloorGroupTemplate();
                    break;
                case RogueTabTypes.FactionInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateFactionTemplate();
                    break;
                case RogueTabTypes.StatInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateStatTemplate();
                    break;
                case RogueTabTypes.ElementInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateElementTemplate();
                    break;
                case RogueTabTypes.LootTableInfo:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateLootTableTemplate();
                    break;
                case RogueTabTypes.PlayerClass:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreatePlayerClassTemplate(ActiveDungeon.CharacterStats);
                    break;
                case RogueTabTypes.NPC:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateNPCTemplate(ActiveDungeon.CharacterStats);
                    break;
                case RogueTabTypes.Item:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateItemTemplate();
                    break;
                case RogueTabTypes.Trap:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateTrapTemplate();
                    break;
                case RogueTabTypes.AlteredStatus:
                    ActiveNodeTag.DungeonElement = DungeonInfoHelpers.CreateAlteredStatusTemplate();
                    break;
                default:
                    break;
            }
            IsNewObject = true;
            ActiveNode = null;
            LoadTabDataForTag(ActiveNodeTag);
            DirtyTab = true;
        }

        private void tsbSaveElement_Click(object sender, EventArgs e)
        {
            ClickedSave = true;
            ActiveControl = null;
            SaveObject();
            ClickedSave = false;
        }

        private bool SaveObject()
        {
            if (!IsNewObject)
            {
                return ActiveNodeTag.TabToOpen switch
                {
                    RogueTabTypes.BasicInfo => SaveBasicInfo(),
                    RogueTabTypes.Locales => SaveLocale(),
                    RogueTabTypes.TileTypeInfo => SaveTileType(),
                    RogueTabTypes.TileSetInfo => SaveTileSet(),
                    RogueTabTypes.FloorInfo => SaveFloorGroup(false),
                    RogueTabTypes.FactionInfo => SaveFaction(),
                    RogueTabTypes.StatInfo => SaveStat(),
                    RogueTabTypes.ElementInfo => SaveElement(),
                    RogueTabTypes.ActionSchoolsInfo => SaveActionSchools(),
                    RogueTabTypes.LootTableInfo => SaveLootTable(),
                    RogueTabTypes.CurrencyInfo => SaveCurrency(),
                    RogueTabTypes.NPCModifierInfo => SaveNPCModifiers(),
                    RogueTabTypes.AffixInfo => SaveAffixes(),
                    RogueTabTypes.QualityLevelInfo => SaveQualityLevels(),
                    RogueTabTypes.ItemSlotInfo => SaveItemSlots(),
                    RogueTabTypes.ItemTypeInfo => SaveItemTypes(),
                    RogueTabTypes.PlayerClass => SavePlayerClass(),
                    RogueTabTypes.NPC => SaveNPC(),
                    RogueTabTypes.Item => SaveItem(),
                    RogueTabTypes.Trap => SaveTrap(),
                    RogueTabTypes.AlteredStatus => SaveAlteredStatus(),
                    RogueTabTypes.Scripts => SaveScripts(),
                    _ => true,
                };
            }
            else
            {
                return SaveObjectAs();
            }
        }

        private void tsbSaveElementAs_Click(object sender, EventArgs e)
        {
            ClickedSave = true;
            ActiveControl = null;
            SaveObjectAs();
            ClickedSave = false;
        }

        private bool SaveObjectAs()
        {
            switch (ActiveNodeTag.TabToOpen)
            {
                case RogueTabTypes.Locales:
                    return SaveLocaleAs();
                case RogueTabTypes.TileTypeInfo:
                    return SaveTileTypeAs();
                case RogueTabTypes.TileSetInfo:
                    return SaveTileSetAs();
                case RogueTabTypes.FloorInfo:
                    return SaveFloorGroup(true);
                case RogueTabTypes.FactionInfo:
                    return SaveFactionAs();
                case RogueTabTypes.StatInfo:
                    return SaveStatAs();
                case RogueTabTypes.ElementInfo:
                    return SaveElementAs();
                case RogueTabTypes.LootTableInfo:
                    return SaveLootTableAs();
                case RogueTabTypes.PlayerClass:
                    return SavePlayerClassAs();
                case RogueTabTypes.NPC:
                    return SaveNPCAs();
                case RogueTabTypes.Item:
                    return SaveItemAs();
                case RogueTabTypes.Trap:
                    return SaveTrapAs();
                case RogueTabTypes.AlteredStatus:
                    return SaveAlteredStatusAs();
                default:
                    return true;
            }
        }

        private void tsbDeleteElement_Click(object sender, EventArgs e)
        {
            switch (ActiveNodeTag.TabToOpen)
            {
                case RogueTabTypes.Locales:
                    DeleteLocale();
                    break;
                case RogueTabTypes.TileTypeInfo:
                    DeleteTileType();
                    break;
                case RogueTabTypes.TileSetInfo:
                    DeleteTileSet();
                    break;
                case RogueTabTypes.FloorInfo:
                    DeleteFloorGroup();
                    break;
                case RogueTabTypes.FactionInfo:
                    DeleteFaction();
                    break;
                case RogueTabTypes.StatInfo:
                    DeleteStat();
                    break;
                case RogueTabTypes.ElementInfo:
                    DeleteElement();
                    break;
                case RogueTabTypes.LootTableInfo:
                    DeleteLootTable();
                    break;
                case RogueTabTypes.PlayerClass:
                    DeletePlayerClass();
                    break;
                case RogueTabTypes.NPC:
                    DeleteNPC();
                    break;
                case RogueTabTypes.Item:
                    DeleteItem();
                    break;
                case RogueTabTypes.Trap:
                    DeleteTrap();
                    break;
                case RogueTabTypes.AlteredStatus:
                    DeleteAlteredStatus();
                    break;
                default:
                    break;
            }
        }

        private async void tsbValidateDungeon_Click(object sender, EventArgs e)
        {
            if (!tbTabs.TabPages.Contains(TabsForNodeTypes[RogueTabTypes.Validator]))
                tbTabs.TabPages.Add(TabsForNodeTypes[RogueTabTypes.Validator]);
            tbTabs.SelectTab(TabsForNodeTypes[RogueTabTypes.Validator]);
            await ValidatorTab.ValidateDungeon(ActiveDungeon, MandatoryLocaleKeys);
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
                case RogueTabTypes.BasicInfo:
                    LoadBasicInfo();
                    break;
                case RogueTabTypes.Locales:
                    var tagLocale = (LocaleInfo)tag.DungeonElement;
                    LoadLocaleInfoFor(tagLocale);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Locale - {tagLocale.Language}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Locale";
                    break;
                case RogueTabTypes.TileTypeInfo:
                    var tagTileType = (TileTypeInfo)tag.DungeonElement;
                    LoadTileTypeInfoFor(tagTileType);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Tile Type - {tagTileType.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Tile Type";
                    break;
                case RogueTabTypes.TileSetInfo:
                    var tagTileSet = (TileSetInfo)tag.DungeonElement;
                    LoadTileSetInfoFor(tagTileSet);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Tileset - {tagTileSet.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Tileset";
                    break;
                case RogueTabTypes.FloorInfo:
                    var tagFloorGroup = (FloorInfo)tag.DungeonElement;
                    LoadFloorInfoFor(tagFloorGroup);
                    if (!IsNewObject)
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
                case RogueTabTypes.FactionInfo:
                    var tagFaction = (FactionInfo)tag.DungeonElement;
                    LoadFactionInfoFor(tagFaction);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Faction - {tagFaction.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Faction";
                    break;
                case RogueTabTypes.StatInfo:
                    var tagStat = (StatInfo)tag.DungeonElement;
                    LoadStatInfoFor(tagStat);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Stat - {tagStat.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Stat";
                    break;
                case RogueTabTypes.ElementInfo:
                    var tagElement = (ElementInfo)tag.DungeonElement;
                    LoadElementInfoFor(tagElement);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Attack Element - {tagElement.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Attack Element";
                    break;
                case RogueTabTypes.ActionSchoolsInfo:
                    LoadActionSchools();
                    break;
                case RogueTabTypes.CurrencyInfo:
                    LoadCurrencyInfo();
                    break;
                case RogueTabTypes.AffixInfo:
                    LoadAffixes();
                    break;
                case RogueTabTypes.NPCModifierInfo:
                    LoadNPCModifiers();
                    break;
                case RogueTabTypes.QualityLevelInfo:
                    LoadQualityLevels();
                    break;
                case RogueTabTypes.ItemSlotInfo:
                    LoadItemSlots();
                    break;
                case RogueTabTypes.ItemTypeInfo:
                    LoadItemTypes();
                    break;
                case RogueTabTypes.LootTableInfo:
                    var tagLootTable = (LootTableInfo)tag.DungeonElement;
                    LoadLootTableInfoFor(tagLootTable);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Loot Table - {tagLootTable.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Loot Table";
                    break;
                case RogueTabTypes.PlayerClass:
                    var tagPlayerClass = (PlayerClassInfo)tag.DungeonElement;
                    LoadPlayerClassInfoFor(tagPlayerClass);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Player Class - {tagPlayerClass.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Player Class";
                    break;
                case RogueTabTypes.NPC:
                    var tagNPC = (NPCInfo)tag.DungeonElement;
                    LoadNPCInfoFor(tagNPC);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"NPC - {tagNPC.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"NPC";
                    break;
                case RogueTabTypes.Item:
                    var tagItem = (ItemInfo)tag.DungeonElement;
                    LoadItemInfoFor(tagItem);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Item - {tagItem.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Item";
                    break;
                case RogueTabTypes.Trap:
                    var tagTrap = (TrapInfo)tag.DungeonElement;
                    LoadTrapInfoFor(tagTrap);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Trap - {tagTrap.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Trap";
                    break;
                case RogueTabTypes.AlteredStatus:
                    var tagAlteredStatus = (AlteredStatusInfo)tag.DungeonElement;
                    LoadAlteredStatusInfoFor(tagAlteredStatus);
                    if (!IsNewObject)
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Altered Status - {tagAlteredStatus.Id}";
                    else
                        TabsForNodeTypes[tag.TabToOpen].Text = $"Altered Status";
                    break;
                case RogueTabTypes.Scripts:
                    LoadScripts();
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
            else if (tbTabs.SelectedTab.Text.Equals("Scripts"))
            {
                tssDungeonElement.Visible = true;
                tsbAddElement.Visible = false;
                tsbSaveElement.Visible = true;
                tsbSaveElementAs.Visible = false;
                tsbDeleteElement.Visible = false;
                tssElementValidate.Visible = true;
                tsbValidateDungeon.Visible = true;
            }
            else if (tbTabs.SelectedTab.Text.Equals("Action Schools"))
            {
                tssDungeonElement.Visible = true;
                tsbAddElement.Visible = false;
                tsbSaveElement.Visible = true;
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

        #region Shared Between Tabs

        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
            sae.ActionContentsChanged += (_, _) => DirtyTab = true;
        }
        private void SetMultiActionEditorParams(MultiActionEditor mae, string classId, List<ActionWithEffectsInfo> actions)
        {
            mae.Actions = actions;
            mae.ClassId = classId;
            mae.Dungeon = ActiveDungeon;
            mae.EffectParamData = EffectParamData;
            mae.ActionContentsChanged += (_, _) => DirtyTab = true;
        }

        #endregion

        #region Basic Info

        private void LoadBasicInfo()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            BasicInformationTab.LoadData(ActiveDungeon);
        }

        private bool SaveBasicInfo()
        {
            var validationErrors = BasicInformationTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Basic Information cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                "Save Basic Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's Basic Information has been successfully saved!",
                "Save Basic Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }
        private void BasicInformationTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Locales

        private void LoadLocaleInfoFor(LocaleInfo localeInfo)
        {
            var localeClone = localeInfo.Clone(MandatoryLocaleKeys);
            if (localeClone.AddMissingMandatoryLocalesIfNeeded(LocaleTemplate, MandatoryLocaleKeys))
            {
                DirtyTab = true;
                if(!IsNewObject)
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
                if (!IsNewObject)
                    MessageBox.Show(
                        "This Locale is missing some custom keys present in other Locales.\n\nThey have been added at the end of the table. Please check them.",
                        "Locale",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
            }
            if (AddMissingKeyDoorLocalesIfNeeded(localeClone))
            {
                DirtyTab = true;
                if (!IsNewObject)
                    MessageBox.Show(
                        "This Locale is missing some custom keys related to Keys and Doors.\n\nThey have been added at the end of the table. Please check them.",
                        "Locale",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
            }
            LocaleEntriesTab.LoadData(ActiveDungeon, localeClone, MandatoryLocaleKeys);
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

        private bool AddMissingKeyDoorLocalesIfNeeded(LocaleInfo localeInfo)
        {
            var customEntriesList = new List<LocaleInfoString>();
            foreach (var floorInfo in ActiveDungeon.FloorInfos)
            {
                if (floorInfo.PossibleKeys == null || !floorInfo.PossibleKeys.KeyTypes.Any()) continue;
                foreach (var keyType in floorInfo.PossibleKeys.KeyTypes)
                {
                    if (!MandatoryLocaleKeys.Contains($"KeyType{keyType.KeyTypeName}"))
                    {
                        customEntriesList.Add(new()
                        {
                            Key = $"KeyType{keyType.KeyTypeName}",
                            Value = $"{keyType.KeyTypeName} Key"
                        });
                    }
                    if (MandatoryLocaleKeys.Contains($"DoorType{keyType.KeyTypeName}")) continue;
                    customEntriesList.Add(new()
                    {
                        Key = $"DoorType{keyType.KeyTypeName}",
                        Value = $"{keyType.KeyTypeName} Door"
                    });
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

        private bool SaveLocale()
        {
            if (string.IsNullOrWhiteSpace(LocaleEntriesTab.LoadedLocale.Language))
                return SaveLocaleAs();
            return SaveLocaleToDungeon(LocaleEntriesTab.LoadedLocale.Language);
        }

        private bool SaveLocaleAs()
        {
            string inputBoxResult;
            do
            {
                inputBoxResult = InputBox.Show("Indicate the Locale Identifier. It must be exactly two characters long.\n\n(For example, \"en\" or \"es\")", "Save Locale As", string.Empty, false, ActiveDungeon.Locales.ConvertAll(l => l.Language));
                if (inputBoxResult == null) return false;
                if (ActiveDungeon.Locales.Exists(l => l.Language.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Locale named {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Locale As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveLocaleToDungeon(inputBoxResult);
                    }
                }
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
                SaveLocaleToDungeon(inputBoxResult);
            }
            return false;
        }

        private bool SaveLocaleToDungeon(string language)
        {
            var validationErrors = LocaleEntriesTab.SaveData(language);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Locale. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Locale",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var localeToSave = LocaleEntriesTab.LoadedLocale;
            var preExistingLocale = ActiveDungeon.Locales.FirstOrDefault(l => l.Language.Equals(language));
            var isNewLocale = preExistingLocale == null;
            if (isNewLocale)
            {
                ActiveDungeon.Locales.Add(localeToSave);
            }
            else
            {
                ActiveDungeon.Locales[ActiveDungeon.Locales.IndexOf(preExistingLocale)] = localeToSave.Clone(MandatoryLocaleKeys);
            }
            var verb = isNewLocale ? "Save" : "Update";
            MessageBox.Show(
                $"Locale {language} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Locale",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(localeToSave.Language, "Locales");
            return true;
        }

        private void DeleteLocale()
        {
            var activeLocale = LocaleEntriesTab.LoadedLocale;
            var deleteLocalePrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }
        private void LocaleEntriesTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Tile Type

        private void LoadTileTypeInfoFor(TileTypeInfo tileType)
        {
            TileTypeTab.LoadData(tileType, ActiveDungeon, EffectParamData);
        }

        private bool SaveTileType()
        {
            if (string.IsNullOrWhiteSpace(TileTypeTab.LoadedTileType.Id))
                return SaveTileTypeAs();
            return SaveTileTypeToDungeon(TileTypeTab.LoadedTileType.Id);
        }

        private bool SaveTileTypeAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Tile Type Identifier", "Save Special Tile As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(TileTypeInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.TileTypeInfos.Exists(tti => tti.Id.Equals(inputBoxResult)))
                {
                    if(FormConstants.DefaultTileTypes.Any(dtt => dtt.Equals(inputBoxResult, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        MessageBox.Show(
                            $"{inputBoxResult} is a default Tile Type.\n\nPlease pick another.",
                            "Save Special Tile As",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return false;
                    }
                    else
                    {
                        var messageBoxResult = MessageBox.Show(
                            $"A Special Tile with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                            "Save Special Tile As",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );
                        if (messageBoxResult == DialogResult.Yes)
                        {
                            return SaveTileTypeToDungeon(inputBoxResult);
                        }
                    }
                }
                else
                {
                    return SaveTileTypeToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveTileTypeToDungeon(string id)
        {
            var validationErrors = TileTypeTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save the Special Tile Type. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Special Tile",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var tileTypeToSave = TileTypeTab.LoadedTileType;
            var preExistingTileType = ActiveDungeon.TileTypeInfos.FirstOrDefault(tti => tti.Id.Equals(id));
            var isNewTileType = preExistingTileType == null;
            if (isNewTileType)
            {
                ActiveDungeon.TileTypeInfos.Add(tileTypeToSave);
            }
            else
            {
                ActiveDungeon.TileTypeInfos[ActiveDungeon.TileTypeInfos.IndexOf(preExistingTileType)] = tileTypeToSave;
            }
            var verb = isNewTileType ? "Save" : "Update";
            MessageBox.Show(
                $"Special Tile {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Special Tile",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(tileTypeToSave.Id, "Special Tiles");
            return true;
        }

        public void DeleteTileType()
        {
            var activeTileType = TileTypeTab.LoadedTileType;
            var deleteTileTypePrompt = IsNewObject
                ? "Do you want to remove this unsaved Special Tile?"
                : $"Do you want to PERMANENTLY delete Special Tile {activeTileType.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteTileTypePrompt,
                "Special Tile",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewObject)
                {
                    var removedId = new string(activeTileType.Id);
                    ActiveDungeon.TileTypeInfos.RemoveAll(tti => tti.Id.Equals(activeTileType.Id));
                    ActiveDungeon.FloorInfos.ForEach(fi => fi.PossibleSpecialTiles = fi.PossibleSpecialTiles.Where(pst => !pst.TileTypeId.Equals(activeTileType.Id, StringComparison.InvariantCultureIgnoreCase)).ToList());
                    MessageBox.Show(
                        $"Special Tile {removedId} has been successfully deleted.",
                        "Delete Special Tile",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void TileTypeTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Tileset

        private void LoadTileSetInfoFor(TileSetInfo tileSet)
        {
            TilesetTab.LoadData(tileSet, ActiveDungeon);
        }

        private bool SaveTileSet()
        {
            if (string.IsNullOrWhiteSpace(TilesetTab.LoadedTileSet.Id))
                return SaveTileSetAs();
            return SaveTileSetToDungeon(TilesetTab.LoadedTileSet.Id);
        }

        private bool SaveTileSetAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Tileset Identifier", "Save Tileset As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(TileSetInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.TileSetInfos.Exists(tsi => tsi.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Tileset with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Tileset As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveTileSetToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveTileSetToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveTileSetToDungeon(string id)
        {
            var validationErrors = TilesetTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Tileset. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Tileset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var tilesetToSave = TilesetTab.LoadedTileSet;
            var preExistingTileset = ActiveDungeon.TileSetInfos.FirstOrDefault(ti => ti.Id.Equals(id));
            var isNewTileSet = preExistingTileset == null;
            if (isNewTileSet)
            {
                ActiveDungeon.TileSetInfos.Add(tilesetToSave);
            }
            else
            {
                ActiveDungeon.TileSetInfos[ActiveDungeon.TileSetInfos.IndexOf(preExistingTileset)] = tilesetToSave;
            }
            var verb = isNewTileSet ? "Save" : "Update";
            MessageBox.Show(
                $"Tileset {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Tileset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(tilesetToSave.Id, "Tilesets");
            return true;
        }

        public void DeleteTileSet()
        {
            var activeTileSet = TilesetTab.LoadedTileSet;
            var deleteTileSetPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void TilesetTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Floor Group

        private void LoadFloorInfoFor(FloorInfo floorGroup)
        {
            tsbSaveElementAs.Visible = false;
            FloorGroupTab.LoadData(ActiveDungeon, floorGroup, EffectParamData, RoomDispositionData);
        }

        private bool SaveFloorGroup(bool saveAsNew)
        {
            var validationErrors = FloorGroupTab.SaveData(saveAsNew);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Floor Group. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Floor Group",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var floorGroupToSave = FloorGroupTab.LoadedFloorGroup;
            var preExistingFloorGroup = ActiveDungeon.FloorInfos.FirstOrDefault(fg => fg == FloorGroupTab.OpenedFloorGroup);
            var isNewFloorGroup = saveAsNew || preExistingFloorGroup == null;
            if (isNewFloorGroup)
            {
                ActiveDungeon.FloorInfos.Add(floorGroupToSave);
            }
            else
            {
                ActiveDungeon.FloorInfos[ActiveDungeon.FloorInfos.IndexOf(preExistingFloorGroup)] = floorGroupToSave;
            }
            var floorLevelString = (floorGroupToSave.MinFloorLevel != floorGroupToSave.MaxFloorLevel)
                    ? $"{floorGroupToSave.MinFloorLevel}-{floorGroupToSave.MaxFloorLevel}"
                    : floorGroupToSave.MinFloorLevel.ToString();
            var verb = isNewFloorGroup ? "Save" : "Update";
            MessageBox.Show(
                $"Floor Group {floorLevelString} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Floor Group",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            var floorInfoNodeText = string.Empty;
            if (floorGroupToSave.MinFloorLevel != floorGroupToSave.MaxFloorLevel)
                floorInfoNodeText = $"Floors {floorLevelString}";
            else
                floorInfoNodeText = $"Floor {floorLevelString}";
            ActiveDungeon.AmountOfFloors = ActiveDungeon.FloorInfos.Select(fg => fg.MaxFloorLevel).Max();
            DirtyDungeon = true;
            DirtyTab = false;
            IsNewObject = false;
            RefreshTreeNodes();
            SelectNodeIfExists(floorInfoNodeText, "Floor Groups");
            FloorGroupTab.LoadedFloorGroup = FloorGroupTab.OpenedFloorGroup = floorGroupToSave;
            return true;
        }


        private void DeleteFloorGroup()
        {
            var activeFloorGroup = FloorGroupTab.LoadedFloorGroup;
            var floorLevelString = (activeFloorGroup.MinFloorLevel != activeFloorGroup.MaxFloorLevel)
                    ? $"{activeFloorGroup.MinFloorLevel}-{activeFloorGroup.MaxFloorLevel}"
                    : activeFloorGroup.MinFloorLevel.ToString();
            var deleteFloorGroupPrompt = IsNewObject
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
                if (!IsNewObject)
                {
                    ActiveDungeon.FloorInfos.RemoveAll(fi => fi.MinFloorLevel == activeFloorGroup.MinFloorLevel && fi.MaxFloorLevel == activeFloorGroup.MaxFloorLevel);
                    MessageBox.Show(
                        $"Floor Group {floorLevelString} has been successfully deleted.",
                        "Delete Floor Group",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void FloorGroupTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Faction
        
        public void LoadFactionInfoFor(FactionInfo faction)
        {
            FactionTab.LoadData(ActiveDungeon, faction);
        }

        private bool SaveFaction()
        {
            if (string.IsNullOrWhiteSpace(FactionTab.LoadedFaction.Id))
                return SaveFactionAs();
            return SaveFactionToDungeon(FactionTab.LoadedFaction.Id);
        }

        private bool SaveFactionAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Faction Identifier", "Save Faction As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(FactionInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.FactionInfos.Exists(fi => fi.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Faction with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Faction As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveFactionToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveFactionToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveFactionToDungeon(string id)
        {
            var validationErrors = FactionTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Faction. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Faction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var factionToSave = FactionTab.LoadedFaction;
            var preExistingFaction = ActiveDungeon.FactionInfos.FirstOrDefault(fi => fi.Id.Equals(id));
            var isNewFaction = preExistingFaction == null;
            if (isNewFaction)
            {
                ActiveDungeon.FactionInfos.Add(factionToSave);
            }
            else
            {
                ActiveDungeon.FactionInfos[ActiveDungeon.FactionInfos.IndexOf(preExistingFaction)] = factionToSave;
            }
            foreach (var alliedFaction in ActiveDungeon.FactionInfos.Where(fi => factionToSave.AlliedWith.Contains(fi.Id)))
            {
                alliedFaction.AlliedWith.Add(factionToSave.Id);
                alliedFaction.NeutralWith.Remove(factionToSave.Id);
                alliedFaction.EnemiesWith.Remove(factionToSave.Id);
            }
            foreach (var neutralFaction in ActiveDungeon.FactionInfos.Where(fi => factionToSave.NeutralWith.Contains(fi.Id)))
            {
                neutralFaction.AlliedWith.Remove(factionToSave.Id);
                neutralFaction.NeutralWith.Add(factionToSave.Id);
                neutralFaction.EnemiesWith.Remove(factionToSave.Id);
            }
            foreach (var enemyFaction in ActiveDungeon.FactionInfos.Where(fi => factionToSave.EnemiesWith.Contains(fi.Id)))
            {
                enemyFaction.AlliedWith.Remove(factionToSave.Id);
                enemyFaction.NeutralWith.Remove(factionToSave.Id);
                enemyFaction.EnemiesWith.Add(factionToSave.Id);
            }
            var verb = isNewFaction ? "Save" : "Update";
            MessageBox.Show(
                $"Faction {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Faction",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(factionToSave.Id, "Factions");
            return true;
        }

        public void DeleteFaction()
        {
            var activeFaction = FactionTab.LoadedFaction;
            var deleteFactionPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void FactionTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Stat

        public void LoadStatInfoFor(StatInfo stat)
        {
            StatTab.LoadData(stat, ActiveDungeon);
        }

        private bool SaveStat()
        {
            if (string.IsNullOrWhiteSpace(StatTab.LoadedStat.Id))
                return SaveStatAs();
            return SaveStatToDungeon(StatTab.LoadedStat.Id);
        }

        private bool SaveStatAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Stat Identifier", "Save Stat As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(CharacterStatInfo)));
            if (inputBoxResult != null)
            {
                if(FormConstants.DefaultStats.Exists(s => s.Equals(inputBoxResult, StringComparison.InvariantCultureIgnoreCase)))
                {
                    MessageBox.Show(
                        $"{inputBoxResult} is a default Stat.\n\nPlease pick another.",
                        "Save Stat As",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return false;
                }
                else if (ActiveDungeon.CharacterStats.Exists(s => s.Id.Equals(inputBoxResult, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Stat with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Stat As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveStatToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveStatToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveStatToDungeon(string id)
        {
            var validationErrors = StatTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Stat. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Stat",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var statToSave = StatTab.LoadedStat;
            var preExistingStat = ActiveDungeon.CharacterStats.FirstOrDefault(s => s.Id.Equals(id));
            var isNewStat = preExistingStat == null;
            if (isNewStat)
            {
                ActiveDungeon.CharacterStats.Add(statToSave);
            }
            else
            {
                ActiveDungeon.CharacterStats[ActiveDungeon.CharacterStats.IndexOf(preExistingStat)] = statToSave;
            }
            var verb = isNewStat ? "Save" : "Update";
            MessageBox.Show(
                $"Stat {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Stat",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(statToSave.Id, "Custom Stats");
            return true;
        }

        public void DeleteStat()
        {
            var activeStat = StatTab.LoadedStat;
            var deleteStatPrompt = IsNewObject
                ? "Do you want to remove this unsaved Stat?"
                : $"Do you want to PERMANENTLY delete Stat {activeStat.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteStatPrompt,
                "Stat",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewObject)
                {
                    var removedId = new string(activeStat.Id);
                    ActiveDungeon.CharacterStats.RemoveAll(s => s.Id.Equals(activeStat.Id));
                    foreach (var stat in ActiveDungeon.CharacterStats)
                    {
                        if(stat.RegeneratesStatId != null && stat.RegeneratesStatId.Equals(removedId))
                        {
                            stat.StatType = "Decimal";
                            stat.RegeneratesStatId = string.Empty;
                        }
                    }
                    MessageBox.Show(
                        $"Stat {removedId} has been successfully deleted.",
                        "Delete Stat",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }
        private void StatTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Element

        public void LoadElementInfoFor(ElementInfo element)
        {
            ElementTab.LoadData(element, ActiveDungeon, EffectParamData);
        }

        private bool SaveElement()
        {
            if (string.IsNullOrWhiteSpace(ElementTab.LoadedElement.Id))
                return SaveElementAs();
            return SaveElementToDungeon(ElementTab.LoadedElement.Id);
        }

        private bool SaveElementAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Attack Element Identifier", "Save Attack Element As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(ElementInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.ElementInfos.Exists(s => s.Id.Equals(inputBoxResult, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Attack Element with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Attack Element As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveElementToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveElementToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveElementToDungeon(string id)
        {
            var validationErrors = ElementTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Attack Element. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Attack Element",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var elementToSave = ElementTab.LoadedElement;
            var preExistingElement = ActiveDungeon.ElementInfos.FirstOrDefault(s => s.Id.Equals(id));
            var isNewElement = preExistingElement == null;
            if (isNewElement)
            {
                ActiveDungeon.ElementInfos.Add(elementToSave);
            }
            else
            {
                ActiveDungeon.ElementInfos[ActiveDungeon.ElementInfos.IndexOf(preExistingElement)] = elementToSave;
            }
            var verb = isNewElement ? "Save" : "Update";
            MessageBox.Show(
                $"Attack Element {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Attack Element",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(elementToSave.Id, "Attack Elements");
            return true;
        }

        public void DeleteElement()
        {
            var activeAttackElement = ElementTab.LoadedElement;
            var deleteAttackElementPrompt = IsNewObject
                ? "Do you want to remove this unsaved Attack Element?"
                : $"Do you want to PERMANENTLY delete Attack Element {activeAttackElement.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteAttackElementPrompt,
                "Attack Element",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewObject)
                {
                    var removedId = new string(activeAttackElement.Id);
                    ActiveDungeon.ElementInfos.RemoveAll(s => s.Id.Equals(activeAttackElement.Id));
                    MessageBox.Show(
                        $"Attack Element {removedId} has been successfully deleted.",
                        "Delete Attack Element",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void ElementTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Loot Table

        public void LoadLootTableInfoFor(LootTableInfo lootTable)
        {
            LootTableTab.LoadData(ActiveDungeon, lootTable);
        }

        private bool SaveLootTable()
        {
            if (string.IsNullOrWhiteSpace(LootTableTab.LoadedLootTable.Id))
                return SaveLootTableAs();
            return SaveLootTableToDungeon(LootTableTab.LoadedLootTable.Id);
        }

        private bool SaveLootTableAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Loot Table Identifier", "Save Loot Table As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(LootTableInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.LootTableInfos.Exists(s => s.Id.Equals(inputBoxResult, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Loot Table with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Loot Table As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveLootTableToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveLootTableToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveLootTableToDungeon(string id)
        {
            var validationErrors = LootTableTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Loot Table. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Loot Table",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var lootTableToSave = LootTableTab.LoadedLootTable;
            var preExistingLootTable = ActiveDungeon.LootTableInfos.FirstOrDefault(s => s.Id.Equals(id));
            var isNewLootTable = preExistingLootTable == null;
            if (isNewLootTable)
            {
                ActiveDungeon.LootTableInfos.Add(lootTableToSave);
            }
            else
            {
                ActiveDungeon.LootTableInfos[ActiveDungeon.LootTableInfos.IndexOf(preExistingLootTable)] = lootTableToSave;
            }
            var verb = isNewLootTable ? "Save" : "Update";
            MessageBox.Show(
                $"Loot Table {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Loot Table",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            IsNewObject = false;
            PassedValidation = false;
            RefreshTreeNodes();
            SelectNodeIfExists(lootTableToSave.Id, "Loot Tables");
            return true;
        }

        public void DeleteLootTable()
        {
            var activeLootTable = LootTableTab.LoadedLootTable;
            var deleteLootTablePrompt = IsNewObject
                ? "Do you want to remove this unsaved Loot Table?"
                : $"Do you want to PERMANENTLY delete Loot Table {activeLootTable.Id}?";
            var messageBoxResult = MessageBox.Show(
                deleteLootTablePrompt,
                "Loot Table",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                if (!IsNewObject)
                {
                    var removedId = new string(activeLootTable.Id);
                    ActiveDungeon.LootTableInfos.RemoveAll(s => s.Id.Equals(activeLootTable.Id));
                    MessageBox.Show(
                        $"Loot Table {removedId} has been successfully deleted.",
                        "Delete Loot Table",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void LootTableTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Currency

        public void LoadCurrencyInfo()
        {
            CurrencyTab.LoadData(ActiveDungeon);
        }

        private bool SaveCurrency()
        {
            var validationErrors = CurrencyTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Currency Data. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Currency Data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            ActiveDungeon.CurrencyInfo = CurrencyTab.LoadedCurrency;
            MessageBox.Show(
                $"Currency Data has been successfully saved!",
                $"Save Currency Data",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyTab = false;
            DirtyDungeon = true;
            PassedValidation = false;
            RefreshTreeNodes();
            return true;
        }


        private void CurrencyTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Player Class

        private void LoadPlayerClassInfoFor(PlayerClassInfo playerClass)
        {
            PlayerClassTab.LoadData(ActiveDungeon, playerClass, EffectParamData, BaseSightRangeDisplayNames);
        }

        private bool SavePlayerClass()
        {
            if (string.IsNullOrWhiteSpace(PlayerClassTab.LoadedPlayerClass.Id))
                return SavePlayerClassAs();
            return SavePlayerClassToDungeon(PlayerClassTab.LoadedPlayerClass.Id);
        }

        private bool SavePlayerClassAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Player Class Identifier", "Save Player Class As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(PlayerClassInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.PlayerClasses.Exists(pc => pc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"A Player Class with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Player Class As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SavePlayerClassToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SavePlayerClassToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SavePlayerClassToDungeon(string id)
        {
            var validationErrors = PlayerClassTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Player Class. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Player Class",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var playerClassToSave = PlayerClassTab.LoadedPlayerClass;
            var preExistingPlayerClass = ActiveDungeon.PlayerClasses.FirstOrDefault(pc => pc.Id.Equals(id));
            var isNewPlayerClass = preExistingPlayerClass == null;
            if (isNewPlayerClass)
            {
                ActiveDungeon.PlayerClasses.Add(playerClassToSave);
            }
            else
            {
                ActiveDungeon.PlayerClasses[ActiveDungeon.PlayerClasses.IndexOf(preExistingPlayerClass)] = playerClassToSave;
            }
            var verb = isNewPlayerClass ? "Save" : "Update";
            MessageBox.Show(
                $"Player Class {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Player Class",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            IsNewObject = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{playerClassToSave.ConsoleRepresentation.Character} - {playerClassToSave.Id}";
            SelectNodeIfExists(nodeText, "Player Classes");
            PassedValidation = false;
            return true;
        }

        public void DeletePlayerClass()
        {
            var activePlayerClass = PlayerClassTab.LoadedPlayerClass;
            var deletePlayerClassPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }
        
        private void PlayerClassTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region NPC
        private void LoadNPCInfoFor(NPCInfo npc)
        {
            NPCTab.LoadData(ActiveDungeon, npc, EffectParamData, BaseSightRangeDisplayNames, NPCAITypeDisplayNames);
        }

        private bool SaveNPC()
        {
            if (string.IsNullOrWhiteSpace(NPCTab.LoadedNPC.Id))
                return SaveNPCAs();
            return SaveNPCToDungeon(NPCTab.LoadedNPC.Id);
        }
        
        private bool SaveNPCAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the NPC Identifier", "Save NPC As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(NPCInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.NPCs.Exists(npc => npc.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An NPC with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save NPC As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveNPCToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveNPCToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveNPCToDungeon(string id)
        {
            var validationErrors = NPCTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save NPC. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save NPC",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var npcToSave = NPCTab.LoadedNPC;
            var preExistingNPC = ActiveDungeon.NPCs.FirstOrDefault(npc => npc.Id.Equals(id));
            var isNewNPC = preExistingNPC == null;
            if (isNewNPC)
            {
                ActiveDungeon.NPCs.Add(npcToSave);
            }
            else
            {
                ActiveDungeon.NPCs[ActiveDungeon.NPCs.IndexOf(preExistingNPC)] = npcToSave;
            }
            var verb = isNewNPC ? "Save" : "Update";
            MessageBox.Show(
                $"NPC {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} NPC",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            IsNewObject = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{npcToSave.ConsoleRepresentation.Character} - {npcToSave.Id}";
            SelectNodeIfExists(nodeText, "NPCs");
            PassedValidation = false;
            return true;
        }

        public void DeleteNPC()
        {
            var activeNPC = NPCTab.LoadedNPC;
            var deleteNPCPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void NPCTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Item
        private void LoadItemInfoFor(ItemInfo item)
        {
            ItemTab.LoadData(ActiveDungeon, item, EffectParamData);
        }

        private bool SaveItem()
        {
            if (string.IsNullOrWhiteSpace(ItemTab.LoadedItem.Id))
                return SaveItemAs();
            return SaveItemToDungeon(ItemTab.LoadedItem.Id);
        }

        private bool SaveItemAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Item Identifier", "Save Item As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(ItemInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.Items.Exists(i => i.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Item with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Item As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveItemToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveItemToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveItemToDungeon(string id)
        {
            var validationErrors = ItemTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Item. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var itemToSave = ItemTab.LoadedItem;
            var preExistingItem = ActiveDungeon.Items.FirstOrDefault(i => i.Id.Equals(id));
            var isNewItem = preExistingItem == null;
            if (isNewItem)
            {
                ActiveDungeon.Items.Add(itemToSave);
            }
            else
            {
                ActiveDungeon.Items[ActiveDungeon.Items.IndexOf(preExistingItem)] = itemToSave;
            }
            var verb = isNewItem ? "Save" : "Update";
            MessageBox.Show(
                $"Item {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Item",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            IsNewObject = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{itemToSave.ConsoleRepresentation.Character} - {itemToSave.Id}";
            SelectNodeIfExists(nodeText, "Items");
            PassedValidation = false;
            return true;
        }

        public void DeleteItem()
        {
            var activeItem = (ItemInfo)ActiveNodeTag.DungeonElement;
            var deleteItemPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void ItemTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Trap

        private void LoadTrapInfoFor(TrapInfo trap)
        {
            TrapTab.LoadData(ActiveDungeon, trap, EffectParamData);
        }

        private bool SaveTrap()
        {
            if (string.IsNullOrWhiteSpace(TrapTab.LoadedTrap.Id))
                return SaveTrapAs();
            return SaveTrapToDungeon(TrapTab.LoadedTrap.Id);
        }

        private bool SaveTrapAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Trap Identifier", "Save Trap As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(TrapInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.Traps.Exists(i => i.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Trap with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Trap As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveTrapToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveTrapToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveTrapToDungeon(string id)
        {
            var validationErrors = TrapTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save Trap. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save Trap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var TrapToSave = TrapTab.LoadedTrap;
            var preExistingTrap = ActiveDungeon.Traps.FirstOrDefault(t => t.Id.Equals(id));
            var isNewTrap = preExistingTrap == null;
            if (isNewTrap)
            {
                ActiveDungeon.Traps.Add(TrapToSave);
            }
            else
            {
                ActiveDungeon.Traps[ActiveDungeon.Traps.IndexOf(preExistingTrap)] = TrapToSave;
            }
            var verb = isNewTrap ? "Save" : "Update";
            MessageBox.Show(
                $"Trap {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Trap",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            IsNewObject = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{TrapToSave.ConsoleRepresentation.Character} - {TrapToSave.Id}";
            SelectNodeIfExists(nodeText, "Traps");
            PassedValidation = false;
            return true;
        }

        public void DeleteTrap()
        {
            var activeTrap = (TrapInfo)ActiveNodeTag.DungeonElement;
            var deleteTrapPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void TrapTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Altered Status

        private void LoadAlteredStatusInfoFor(AlteredStatusInfo alteredStatus)
        {
            AlteredStatusTab.LoadData(ActiveDungeon, alteredStatus, EffectParamData);
        }

        private bool SaveAlteredStatus()
        {
            if (string.IsNullOrWhiteSpace(AlteredStatusTab.LoadedAlteredStatus.Id))
                return SaveAlteredStatusAs();
            return SaveAlteredStatusToDungeon(AlteredStatusTab.LoadedAlteredStatus.Id);
        }

        private bool SaveAlteredStatusAs()
        {
            var inputBoxResult = InputBox.Show("Indicate the Altered Status Identifier", "Save Altered Status As", string.Empty, true, ActiveDungeon.GetAllIds(typeof(AlteredStatusInfo)));
            if (inputBoxResult != null)
            {
                if (ActiveDungeon.AlteredStatuses.Exists(i => i.Id.Equals(inputBoxResult)))
                {
                    var messageBoxResult = MessageBox.Show(
                        $"An Altered Status with Id {inputBoxResult} already exists.\n\nDo you wish to overwrite it?",
                        "Save Altered Status As",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (messageBoxResult == DialogResult.Yes)
                    {
                        return SaveAlteredStatusToDungeon(inputBoxResult);
                    }
                }
                else
                {
                    return SaveAlteredStatusToDungeon(inputBoxResult);
                }
            }
            return false;
        }

        private bool SaveAlteredStatusToDungeon(string id)
        {
            var validationErrors = AlteredStatusTab.SaveData(id);
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"Cannot save AlteredStatus. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Save AlteredStatus",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            var AlteredStatusToSave = AlteredStatusTab.LoadedAlteredStatus;
            var preExistingAlteredStatus = ActiveDungeon.AlteredStatuses.FirstOrDefault(t => t.Id.Equals(id));
            var isNewAlteredStatus = preExistingAlteredStatus == null;
            if (isNewAlteredStatus)
            {
                ActiveDungeon.AlteredStatuses.Add(AlteredStatusToSave);
            }
            else
            {
                ActiveDungeon.AlteredStatuses[ActiveDungeon.AlteredStatuses.IndexOf(preExistingAlteredStatus)] = AlteredStatusToSave;
            }
            var verb = isNewAlteredStatus ? "Save" : "Update";
            MessageBox.Show(
                $"Altered Status {id} has been successfully {verb.ToLowerInvariant()}d!",
                $"{verb} Altered Status",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            IsNewObject = false;
            DirtyDungeon = true;
            DirtyTab = false;
            RefreshTreeNodes();
            var nodeText = $"{AlteredStatusToSave.ConsoleRepresentation.Character} - {AlteredStatusToSave.Id}";
            SelectNodeIfExists(nodeText, "Altered Statuses");
            PassedValidation = false;
            return true;
        }

        public void DeleteAlteredStatus()
        {
            var activeAlteredStatus = (AlteredStatusInfo)ActiveNodeTag.DungeonElement;
            var deleteAlteredStatusPrompt = IsNewObject
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
                if (!IsNewObject)
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
                IsNewObject = false;
                DirtyDungeon = true;
                DirtyTab = false;
                ActiveNodeTag.DungeonElement = null;
                ActiveNodeTag.TabToOpen = RogueTabTypes.BasicInfo;
                RefreshTreeNodes();
                tvDungeonInfo.SelectedNode = tvDungeonInfo.TopNode;
                tvDungeonInfo.Focus();
            }
        }

        private void AlteredStatusTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }

        #endregion

        #region Scripts

        private void LoadScripts()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            ScriptsTab.LoadData(ActiveDungeon, ActiveDungeon.Scripts, EffectParamData);
        }

        private bool SaveScripts()
        {
            var validationErrors = ScriptsTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Scripts cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Scripts Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            ActiveDungeon.Scripts = ScriptsTab.LoadedScripts;
            MessageBox.Show(
                "Dungeon's Scripts has been successfully saved!",
                "Save Scripts",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void ScriptsTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region Action Schools

        private void LoadActionSchools()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            ActionSchoolsTab.LoadData(ActiveDungeon);
        }

        private bool SaveActionSchools()
        {
            var validationErrors = ActionSchoolsTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Action Schools cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Action Schools",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's Action Schools have been successfully saved!",
                "Save Action Schools",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void ActionSchoolsTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region Affixes

        private void LoadAffixes()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            AffixTab.LoadData(ActiveDungeon, EffectParamData);
        }

        private bool SaveAffixes()
        {
            var validationErrors = AffixTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Affixes cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Affixes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's Affixes have been successfully saved!",
                "Save Affixes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            ActiveDungeon.AffixInfos = AffixTab.LoadedAffixes;
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void AffixTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region NPC Modifiers
        private void LoadNPCModifiers()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            NPCModifiersTab.LoadData(ActiveDungeon, EffectParamData);
        }

        private bool SaveNPCModifiers()
        {
            var validationErrors = NPCModifiersTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's NPC Modifiers cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Affixes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's NPC Modifiers have been successfully saved!",
                "Save NPC Modifiers",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            ActiveDungeon.NPCModifierInfos = NPCModifiersTab.LoadedNPCModifiers;
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void NPCModifiersTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region Quality Levels

        private void LoadQualityLevels()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            QualityLevelsTab.LoadData(ActiveDungeon);
        }

        private bool SaveQualityLevels()
        {
            var validationErrors = QualityLevelsTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Quality Levels cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Quality Levels",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's Quality Levels have been successfully saved!",
                "Save Quality Levels",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            ActiveDungeon.QualityLevelInfos = QualityLevelsTab.LoadedQualityInfos;
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void QualityLevelsTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region Item Slots

        private void LoadItemSlots()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            ItemSlotsTab.LoadData(ActiveDungeon);
        }

        private bool SaveItemSlots()
        {
            var validationErrors = ItemSlotsTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Item Slots cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Item Slots",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's Item Slots have been successfully saved!",
                "Save Item Slots",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            ActiveDungeon.ItemSlotInfos = ItemSlotsTab.LoadedItemSlotInfos;
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void ItemSlotsTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region Item Types

        private void LoadItemTypes()
        {
            tsbAddElement.Visible = false;
            tsbSaveElement.Visible = true;
            tsbSaveElementAs.Visible = false;
            tsbDeleteElement.Visible = false;
            tssElementValidate.Visible = true;
            ItemTypesTab.LoadData(ActiveDungeon);
        }

        private bool SaveItemTypes()
        {
            var validationErrors = ItemTypesTab.SaveData();
            if (validationErrors.Any())
            {
                MessageBox.Show(
                    $"The Dungeon's Item Types cannot be saved.\n\nPlease check the following errors:\n- {string.Join("\n - ", validationErrors)}",
                    "Save Item Types",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            MessageBox.Show(
                "Dungeon's Item Types have been successfully saved!",
                "Save Item Types",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            ActiveDungeon.ItemTypeInfos = ItemTypesTab.LoadedItemTypeInfos;
            DirtyDungeon = true;
            DirtyTab = false;
            PassedValidation = false;
            return true;
        }

        private void ItemTypesTab_TabInfoChanged(object? sender, EventArgs e)
        {
            if (!AutomatedChange) DirtyTab = true;
        }
        #endregion

        #region Validator

        private void ValidatorTab_OnValidationComplete(object? sender, EventArgs e)
        {
            PassedValidation = ValidatorTab.PassedValidation;
        }

        private void ValidatorTab_OnError(object? sender, EventArgs e)
        {
            PassedValidation = false;
            if (tbTabs.TabPages.Contains(TabsForNodeTypes[RogueTabTypes.Validator]))
                tbTabs.TabPages.Remove(TabsForNodeTypes[RogueTabTypes.Validator]);
        }

        #endregion
    }

    public class NodeTag
    {
        public RogueTabTypes TabToOpen { get; set; }
        public object? DungeonElement { get; set; }
    }

    public enum RogueTabTypes
    {
        BasicInfo,
        Locales,
        TileTypeInfo,
        TileSetInfo,
        FloorInfo,
        FactionInfo,
        StatInfo,
        ElementInfo,
        ActionSchoolsInfo,
        LootTableInfo,
        CurrencyInfo,
        AffixInfo,
        NPCModifierInfo,
        QualityLevelInfo,
        ItemSlotInfo,
        ItemTypeInfo,
        PlayerClass,
        NPC,
        Item,
        Trap,
        AlteredStatus,
        Scripts,
        Validator
    }
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
#pragma warning restore S4144 // Methods should not have identical implementations
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.Interaction;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
namespace RogueCustomsDungeonEditor.HelperForms
{
    public partial class frmActionTest : Form
    {
        private DungeonInfo ActiveDungeon;
        private Dungeon TestDungeon;
        private Entity Source, ItemOwner;
        private ITargetable Target;
        private ElementType SourceObjectType, TargetObjectType;
        private List<PassiveStatModifierInfo> SourceStats, TargetStats;
        private ActionWithEffects TestAction;
        private UsageCriteria UsageCriteria;
        private string ClassId;
        private string ActionTypeText;
        private ActionWithEffectsInfo actionToTest;
        private string LastLocaleUsed;

        public frmActionTest(ActionWithEffectsInfo actionToTest, DungeonInfo activeDungeon, string classId, string actionTypeText, UsageCriteria usageCriteria)
        {
            InitializeComponent();
            ActionTypeText = actionTypeText;
            lblTitle.Text = $"Test {actionTypeText} for {classId}";

            ActiveDungeon = activeDungeon;
            btnTestAction.Enabled = activeDungeon.Locales.Count > 0;
            UsageCriteria = usageCriteria;
            ClassId = classId;

            // Check if we are in design mode
            if (DesignMode || this.DesignMode)
            {
                return; // Do nothing in design mode
            }

            try
            {
                var fontPath = Path.Combine(Application.StartupPath, "Resources\\PxPlus_Tandy1K-II_200L.ttf");
                var fontName = "PxPlus Tandy1K-II 200L";
                if (FontHelpers.LoadFont(fontPath))
                {
                    var loadedFont = FontHelpers.GetFontByName(fontName);
                    if (loadedFont != null)
                    {
                        txtMessageLog.Font = new Font(loadedFont, 8f, FontStyle.Regular);
                    }
                }
            }
            catch
            {
                // Do nothing if the Font can't be found
            }

            cmbTestLocale.Items.Clear();
            foreach (var locale in activeDungeon.Locales)
            {
                cmbTestLocale.Items.Add(locale.Language);
            }

            var localeToUse = ActiveDungeon.DefaultLocale ?? ActiveDungeon.Locales[0].Language;
            LastLocaleUsed = localeToUse;

            cmbTestLocale.Text = LastLocaleUsed;

            if (TestDungeon == null)
            {
                TestDungeon = new Dungeon(ActiveDungeon, localeToUse, false);
                TestDungeon.PromptInvoker = new DummyPromptInvoker();
                TestDungeon.PlayerClass = TestDungeon.Classes.First(c => c.EntityType == EntityType.Player);
            }
            TestAction = ActionWithEffects.Create(actionToTest, TestDungeon.ActionSchools);

            crsSource.Visible = true;
            crsSource.Character = 'U';
            crsSource.BackgroundColor = new GameColor(Color.Black);
            crsSource.ForegroundColor = new GameColor(Color.FromArgb(0, 255, 0));
            crsTarget.Visible = true;
            crsTarget.Character = 'T';
            crsTarget.BackgroundColor = new GameColor(Color.Black);
            crsTarget.ForegroundColor = new GameColor(Color.FromArgb(255, 0, 0));
            issSource.TreatStatsAsAbsolute = true;
            issTarget.TreatStatsAsAbsolute = true;

            if (activeDungeon.PlayerClasses.Any(pc => pc.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase)) || activeDungeon.NPCs.Any(npc => npc.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase)))
                SourceObjectType = ElementType.Character;
            else if (activeDungeon.Items.Any(i => i.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase)))
            {
                var correspondingItem = activeDungeon.Items.FirstOrDefault(i => i.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase));
                var correspondingItemType = activeDungeon.ItemTypeInfos.FirstOrDefault(iti => iti.Id.Equals(correspondingItem.ItemType, StringComparison.InvariantCultureIgnoreCase));
                SourceObjectType = correspondingItemType.Usability == ItemUsability.Equip ? ElementType.Equippable : ElementType.Consumable;
            }
            else if (activeDungeon.Traps.Any(t => t.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase)))
                SourceObjectType = ElementType.Trap;
            else if (activeDungeon.AlteredStatuses.Any(als => als.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase)))
                SourceObjectType = ElementType.AlteredStatus;

            issSource.Visible = true;
            lblSourceStatuses.Visible = true;
            clbSourceStatuses.Visible = true;
            crsSource.Visible = true;
            lblSourceTitle.Visible = true;

            if (SourceObjectType == ElementType.Tile || SourceObjectType == ElementType.Trap || SourceObjectType == ElementType.AlteredStatus || SourceObjectType == ElementType.Tile)
            {
                issSource.Visible = false;
                lblSourceStatuses.Visible = false;
                clbSourceStatuses.Visible = false;
                crsSource.Visible = false;
                lblSourceTitle.Visible = false;
            }

            issTarget.Visible = true;
            lblTargetStatuses.Visible = true;
            clbTargetStatuses.Visible = true;
            crsTarget.Visible = true;
            lblTargetTitle.Visible = true;

            if (TestAction.TargetTypes.Contains(TargetType.Tile))
            {
                TargetObjectType = ElementType.Tile;
                issTarget.Visible = false;
                lblTargetStatuses.Visible = false;
                clbTargetStatuses.Visible = false;
                crsTarget.Visible = false;
                lblTargetTitle.Visible = false;
            }
            else if ((!TestAction.TargetTypes.Contains(TargetType.Self) && !TestAction.TargetTypes.Contains(TargetType.Tile)) || TestAction.TargetTypes.Count > 1)
            {
                if (TestAction.TargetTypes.Contains(TargetType.Enemy))
                {
                    crsTarget.ForegroundColor = new GameColor(Color.Red);
                }
                else if (TestAction.TargetTypes.Contains(TargetType.Ally))
                {
                    crsTarget.ForegroundColor = new GameColor(Color.FromArgb(0, 255, 0));
                }
                else if (TestAction.TargetTypes.Contains(TargetType.Neutral))
                {
                    crsTarget.ForegroundColor = new GameColor(Color.DarkOrange);
                }
                TargetObjectType = ElementType.Character;
            }

            if (SourceObjectType != ElementType.Trap || TargetObjectType == ElementType.Character)
            {
                SourceStats = new();
                TargetStats = new();
                foreach (var stat in activeDungeon.CharacterStats)
                {
                    var amount = stat.Id switch
                    {
                        "HP" => 40M,
                        "MP" => 100M,
                        "Hunger" => 100M,
                        "Attack" => 3M,
                        "Accuracy" => 90M,
                        _ when stat.StatType.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase) => 0.25M,
                        _ => 0M
                    };
                    if (SourceObjectType != ElementType.Trap)
                    {
                        SourceStats.Add(new()
                        {
                            Id = stat.Id,
                            Amount = amount
                        });
                    }
                    if (TargetObjectType == ElementType.Character)
                    {
                        TargetStats.Add(new()
                        {
                            Id = stat.Id,
                            Amount = amount
                        });
                    }
                }
                issSource.StatData = ActiveDungeon.CharacterStats;
                issSource.Stats = SourceStats;
                issTarget.StatData = ActiveDungeon.CharacterStats;
                issTarget.Stats = TargetStats;
            }
            else
            {
                lblStatsInfo.Visible = false;
            }

            clbSourceStatuses.Items.Clear();
            clbTargetStatuses.Items.Clear();
            var statusList = new List<string>();

            foreach (var status in activeDungeon.AlteredStatuses)
            {
                clbSourceStatuses.Items.Add(status.Id);
                clbTargetStatuses.Items.Add(status.Id);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtMessageLog.Clear();
        }

        private void cmbTestLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTestAction.Enabled = cmbTestLocale.Text.Length > 0;
        }

        private void cmbTestLocale_TextChanged(object sender, EventArgs e)
        {
            btnTestAction.Enabled = cmbTestLocale.Text.Length > 0;
        }

        private async void btnTestAction_Click(object sender, EventArgs e)
        {
            if (TestDungeon == null || !cmbTestLocale.Text.Equals(LastLocaleUsed, StringComparison.InvariantCultureIgnoreCase))
            {
                var localeToUse = !string.IsNullOrWhiteSpace(cmbTestLocale.Text) ? cmbTestLocale.Text : ActiveDungeon.DefaultLocale ?? ActiveDungeon.Locales[0].Language;
                TestDungeon = new Dungeon(ActiveDungeon, localeToUse, false);
                TestDungeon.PromptInvoker = new DummyPromptInvoker();
                TestDungeon.PlayerClass = TestDungeon.Classes.First(c => c.EntityType == EntityType.Player);
            }

            await TestDungeon.GenerateDebugMap();

            TestDungeon.CurrentEntityId = 1;
            TestDungeon.CurrentFloor.AICharacters.Clear();
            TestDungeon.CurrentFloor.Items.Clear();
            TestDungeon.CurrentFloor.Traps.Clear();
            TestDungeon.CurrentFloor.Keys.Clear();

            foreach (var tile in TestDungeon.CurrentFloor.Tiles.Where(t => t.Type != t.BaseType))
            {
                tile.ResetType();
            }

            var learnsetId = ActiveDungeon.LearnsetInfos.First().Id;

            var equippableClassInfo = new ItemInfo()
            {
                Id = "Equippable",
                Name = SourceObjectType == ElementType.Equippable ? ClassId : "Equippable",
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation,
                ItemType = ActiveDungeon.ItemTypeInfos[0].Id,
                MinimumQualityLevel = ActiveDungeon.QualityLevelInfos[0].Id,
                MaximumQualityLevel = ActiveDungeon.QualityLevelInfos[0].Id
            };

            var equippableClass = new EntityClass(equippableClassInfo, TestDungeon, ActiveDungeon.CharacterStats);

            var inventoryClassInfo = new ItemInfo()
            {
                Id = "HeldItem",
                Name = SourceObjectType == ElementType.Consumable ? ClassId : "HeldItem",
                Power = "0",
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation
            };
            var heldItemClass = new EntityClass(inventoryClassInfo, TestDungeon, ActiveDungeon.CharacterStats);

            if (SourceObjectType == ElementType.Character || SourceObjectType == ElementType.Tile || SourceObjectType == ElementType.Equippable)
            {
                Source = CreateNPC("Source", crsSource, issSource, clbSourceStatuses, equippableClass, heldItemClass, learnsetId);
                TestAction.User = Source;
            }
            else if (SourceObjectType == ElementType.Consumable)
            {
                Source = CreateConsumable(ClassId);
                TestAction.User = Source;
                if (!ActionTypeText.Equals("Item Use", StringComparison.InvariantCultureIgnoreCase))
                {
                    ItemOwner = CreateNPC("ItemOwner", crsSource, issSource, clbSourceStatuses, equippableClass, heldItemClass, learnsetId);
                    Source = ItemOwner;
                }
            }
            else if (SourceObjectType == ElementType.Trap)
            {
                Source = CreateTrap(ClassId);
                TestAction.User = Source;
            }
            else if (SourceObjectType == ElementType.AlteredStatus)
            {
                Source = CreateAlteredStatus();
                TestAction.User = Source;
            }

            if (TargetObjectType == ElementType.Character)
            {
                if (!TestAction.TargetTypes.Contains(TargetType.Self))
                {
                    Target = CreateNPC("Target", crsTarget, issTarget, clbTargetStatuses, equippableClass, heldItemClass, learnsetId);
                }
                else
                {
                    Target = Source;
                }
            }
            else if (TargetObjectType == ElementType.Tile)
            {
                var candidateTiles = TestDungeon.CurrentFloor.Tiles.Where(t => t.IsWalkable && !t.IsOccupied);
                Target = candidateTiles[new Random().Next(candidateTiles.Count)];
            }

            for (int i = 0; i < new Random().Next(1, 6); i++)
            {
                if (SourceObjectType == ElementType.Character || SourceObjectType == ElementType.Tile || SourceObjectType == ElementType.Equippable)
                    CreateNPC($"Dummy{i}", crsSource, issSource, clbSourceStatuses, equippableClass, heldItemClass, learnsetId);
                else if (!TestAction.TargetTypes.Contains(TargetType.Self))
                    CreateNPC($"Dummy{i}", crsTarget, issTarget, clbTargetStatuses, equippableClass, heldItemClass, learnsetId);
            }

            try
            {
                btnTestAction.Enabled = false;
                btnClearLog.Enabled = false;
                btnClose.Enabled = false;

                txtMessageLog.Clear();

                TestDungeon.CurrentFloor.Snapshot = new(TestDungeon, TestDungeon.CurrentFloor);
                TestAction.Map = TestDungeon.CurrentFloor;
                await TestAction.Do(Source, Target, false);

                var dungeonStatus = await TestDungeon.GetStatus();
                
                while(dungeonStatus.DisplayEvents.Count > 0)
                {
                    var displayEventList = dungeonStatus.DisplayEvents.Dequeue();
                    foreach (var displayEvent in displayEventList.Events)
                    {
                        switch (displayEvent.DisplayEventType)
                        {
                            case DisplayEventType.AddLogMessage:
                                var message = displayEvent.Params[0] as MessageDto;
                                txtMessageLog.AppendText($"{message.Message}\n", message.ForegroundColor.ToColor());
                                break;
                            case DisplayEventType.AddMessageBox:
                                var messageBox = displayEvent.Params[0] as MessageBoxDto;
                                txtMessageLog.AppendText($"MESSAGE BOX: {messageBox.Title} => {messageBox.Message}\n", messageBox.WindowColor.ToColor());
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtMessageLog.AppendText("Oh no! The Action could not be executed!\n", Color.Red);
                txtMessageLog.AppendText(ex.Message, Color.White);
                txtMessageLog.AppendText("\n---------------------\n", Color.Red);
                txtMessageLog.AppendText(ex.StackTrace, Color.Yellow);
            }
            finally
            {
                btnTestAction.Enabled = true;
                btnClearLog.Enabled = true;
                btnClose.Enabled = true;
            }
        }

        private NonPlayableCharacter CreateNPC(string name, ConsoleRepresentationSelector crs, ItemStatsSheet iss, CheckedListBox clb, EntityClass equippableClass, EntityClass inventoryClass, string learnsetId)
        {
            var npcClassInfo = new NPCInfo()
            {
                Id = name,
                Stats = new(),
                Name = name,
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                OnInteracted = new(),
                OnLevelUp = new(),
                OnSpawn = new(),
                OnTurnStart = new(),
                StartingInventory = new(),
                InventorySize = 25,
                BaseSightRange = "full map",
                AIType = "Default",
                ExperiencePayoutFormula = "1",
                ExperienceToLevelUpFormula = "9999",
                CanGainExperience = true,
                ConsoleRepresentation = crs.ConsoleRepresentation,
                ModifierData = new (),
                RegularLootTable = new NPCLootTableDataInfo { LootTableId = "None", DropPicks = 0 },
                InitialEquipment = new(),
                AvailableSlots = new(),
                Learnset = learnsetId
            };
            iss.TreatStatsAsAbsolute = true;
            iss.EndEdit();
            foreach (var stat in iss.Stats)
            {
                npcClassInfo.Stats.Add(new()
                {
                    StatId = stat.Id,
                    Base = stat.Amount,
                    IncreasePerLevel = 0,
                    Maximum = stat.Amount * 2,
                    Minimum = stat.Amount / 2,
                });
            }
            var npcClass = new EntityClass(npcClassInfo, TestDungeon, ActiveDungeon.CharacterStats);
            var npc = new NonPlayableCharacter(npcClass, 1, TestDungeon.CurrentFloor);
            foreach (var item in clb.CheckedItems)
            {
                var status = TestDungeon.CurrentFloor.PossibleStatuses.Find(s => s.ClassId.Equals(item.ToString()));
                status?.ApplyTo(null, npc, 0, 1);
            }
            npc.Visible = true;
            var possibleTiles = TestDungeon.CurrentFloor.Tiles.Where(t => t.IsWalkable && !t.IsOccupied);
            npc.Position = possibleTiles[new Random().Next(possibleTiles.Count)].Position;
            npc.HP.Current -= npc.HP.Current / 5;
            npc.MP.Current -= npc.MP.Current / 5;
            npc.Hunger.Current -= npc.Hunger.Current / 5;
            var weapon = new Item(equippableClass, 1, TestDungeon.CurrentFloor);
            weapon.Power = "1d3";
            weapon.ItemType = TestDungeon.ItemTypes[0];
            weapon.MinimumQualityLevel = TestDungeon.QualityLevels[0];
            weapon.MaximumQualityLevel = TestDungeon.QualityLevels[0];
            weapon.SetQualityLevel(TestDungeon.QualityLevels[0]);
            weapon.GotSpecificallyIdentified = true;
            var armor = new Item(equippableClass, 1, TestDungeon.CurrentFloor);
            armor.Power = "0";
            armor.ItemType = TestDungeon.ItemTypes[0];
            armor.MinimumQualityLevel = TestDungeon.QualityLevels[0];
            armor.MaximumQualityLevel = TestDungeon.QualityLevels[0];
            armor.SetQualityLevel(TestDungeon.QualityLevels[0]);
            armor.GotSpecificallyIdentified = true;
            npc.Equipment = [weapon, armor];
            var firstItem = new Item(inventoryClass, 1, TestDungeon.CurrentFloor);
            firstItem.Id = new Random().Next(100000, 999999);
            npc.Inventory.Add(firstItem);
            firstItem.ItemType = TestDungeon.ItemTypes[0];
            firstItem.MinimumQualityLevel = TestDungeon.QualityLevels[0];
            firstItem.MaximumQualityLevel = TestDungeon.QualityLevels[0];
            firstItem.SetQualityLevel(TestDungeon.QualityLevels[0]);
            firstItem.GotSpecificallyIdentified = true;
            for (int i = 0; i < new Random().Next(1, 5); i++)
            {
                var newItem = new Item(TestDungeon.ItemClasses[new Random().Next(TestDungeon.ItemClasses.Count)], 1, TestDungeon.CurrentFloor);
                newItem.Id = new Random().Next(100000, 999999);
                npc.Inventory.Add(newItem);
                newItem.ItemType = TestDungeon.ItemTypes[0];
                newItem.MinimumQualityLevel = TestDungeon.QualityLevels[0];
                newItem.MaximumQualityLevel = TestDungeon.QualityLevels[0];
                newItem.SetQualityLevel(TestDungeon.QualityLevels[0]);
                newItem.GotSpecificallyIdentified = true;
            }
            npc.Faction = TestDungeon.Factions[new Random().Next(TestDungeon.Factions.Count)];

            var mostExpensiveItemInTheDungeon = TestDungeon.ItemClasses.OrderByDescending(i => i.BaseValue).FirstOrDefault();
            npc.CurrencyCarried = new Random().Next(mostExpensiveItemInTheDungeon.BaseValue * 2);
            TestDungeon.CurrentFloor.AICharacters.Add(npc);

            return npc;
        }

        private Item CreateConsumable(string name)
        {
            var consumableClassInfo = new ItemInfo()
            {
                Id = "Consumable",
                Name = name,
                Power = "1d3",
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation,
                ItemType = ActiveDungeon.ItemTypeInfos[0].Id,
                MinimumQualityLevel = ActiveDungeon.QualityLevelInfos[0].Id,
                MaximumQualityLevel = ActiveDungeon.QualityLevelInfos[0].Id
            };
            var consumableClass = new EntityClass(consumableClassInfo, TestDungeon, ActiveDungeon.CharacterStats);
            return new Item(consumableClass, 1, TestDungeon.CurrentFloor);
        }

        private Trap CreateTrap(string name)
        {
            var trapClassInfo = new TrapInfo()
            {
                Id = "Trap",
                Name = name,
                Power = "1d3",
                OnStepped = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation
            };
            var trapClass = new EntityClass(trapClassInfo, TestDungeon, ActiveDungeon.CharacterStats);
            var trap = new Trap(trapClass, TestDungeon.CurrentFloor);
            trap.Position = TestDungeon.CurrentFloor.Tiles.Where(t => t.IsWalkable)[0].Position;
            return trap;
        }

        private AlteredStatus CreateAlteredStatus()
        {
            var alteredStatusClassInfo = new AlteredStatusInfo()
            {
                Id = "Status",
                Name = "Status",
                OnApply = new(),
                OnAttacked = new(),
                OnRemove = new(),
                OnTurnStart = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation
            };
            var alteredStatusClass = new EntityClass(alteredStatusClassInfo, TestDungeon, ActiveDungeon.CharacterStats);
            var alteredStatus = new AlteredStatus(alteredStatusClass, TestDungeon.CurrentFloor);
            do
            {
                alteredStatus.Power = new Random().Next(-6, 6);
            }
            while (alteredStatus.Power == 0);
            return alteredStatus;
        }

        private enum ElementType
        {
            Character,
            Equippable,
            Consumable,
            Trap,
            AlteredStatus,
            Tile
        }

        private class DummyPromptInvoker : IPromptInvoker
        {
            public Task<bool> OpenYesNoPrompt(string title, string message, string yesButtonText, string noButtonText, GameColor color) => Task.FromResult(new Random().NextDouble() >= 0.5);
            public Task<string> OpenSelectOption(string title, string message, SelectionItem[] choices, bool showCancelButton, GameColor borderColor) => Task.FromResult(choices[new Random().Next(choices.Length)].Id);
            public Task<ItemInput> OpenSelectItem(string title, InventoryDto choices, bool showCancelButton)
            {
                var choice = choices.InventoryItems[new Random().Next(choices.InventoryItems.Count)];
                return Task.FromResult(new ItemInput
                {
                    Id = choice.ItemId,
                    ClassId = choice.ClassId
                });
            }
            public Task<string> OpenSelectAction(string title, ActionListDto choices, bool showCancelButton) => Task.FromResult(choices.Actions[new Random().Next(choices.Actions.Count)].SelectionId);
            public Task<int?> OpenBuyPrompt(string title, InventoryDto choices, bool showCancelButton) => Task.FromResult((int?)choices.InventoryItems[new Random().Next(choices.InventoryItems.Count)].ItemId);
            public Task<int?> OpenSellPrompt(string title, InventoryDto choices, bool showCancelButton) => Task.FromResult((int?)choices.InventoryItems[new Random().Next(choices.InventoryItems.Count)].ItemId);
            public Task<string> OpenTextPrompt(string title, string message, string defaultText, GameColor borderColor) => Task.FromResult(defaultText);
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma

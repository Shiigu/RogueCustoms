using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
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

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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

        public frmActionTest(ActionWithEffectsInfo actionToTest, DungeonInfo activeDungeon, string classId, string actionTypeText, UsageCriteria usageCriteria)
        {
            InitializeComponent();
            ActionTypeText = actionTypeText;
            lblTitle.Text = $"Test {actionTypeText} for {classId}";

            ActiveDungeon = activeDungeon;
            btnTestAction.Enabled = activeDungeon.Locales.Count > 0;
            TestAction = ActionWithEffects.Create(actionToTest);
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
            else if (activeDungeon.Items.Any(i => i.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase) && !i.EntityType.Equals("Consumable", StringComparison.InvariantCultureIgnoreCase)))
                SourceObjectType = ElementType.Equippable;
            else if (activeDungeon.Items.Any(i => i.Id.Equals(classId, StringComparison.InvariantCultureIgnoreCase) && i.EntityType.Equals("Consumable", StringComparison.InvariantCultureIgnoreCase)))
                SourceObjectType = ElementType.Consumable;
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

        private async void btnTestAction_Click(object sender, EventArgs e)
        {
            if (TestDungeon == null)
            {
                TestDungeon = new Dungeon(ActiveDungeon, ActiveDungeon.Locales[0].Language, false);
                TestDungeon.PromptInvoker = new DummyPromptInvoker();
                TestDungeon.PlayerClass = TestDungeon.Classes.First(c => c.EntityType == EntityType.Player);
                await TestDungeon.GenerateDebugMap();
            }

            TestDungeon.CurrentEntityId = 1;
            TestDungeon.CurrentFloor.AICharacters.Clear();
            TestDungeon.CurrentFloor.Items.Clear();
            TestDungeon.CurrentFloor.Traps.Clear();
            TestDungeon.CurrentFloor.Keys.Clear();

            var equippableClassInfo = new ItemInfo()
            {
                Id = "Equippable",
                Name = ClassId,
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation
            };

            var equippableClass = new EntityClass(equippableClassInfo, TestDungeon.LocaleToUse, EntityType.Weapon, ActiveDungeon.CharacterStats);

            var inventoryClassInfo = new ItemInfo()
            {
                Id = "HeldItem",
                Name = "HeldItem",
                Power = "0",
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                OnTurnStart = new(),
                OnUse = new(),
                ConsoleRepresentation = crsSource.ConsoleRepresentation
            };
            var heldItemClass = new EntityClass(inventoryClassInfo, TestDungeon.LocaleToUse, EntityType.Consumable, ActiveDungeon.CharacterStats);

            if (SourceObjectType == ElementType.Character || SourceObjectType == ElementType.Tile || SourceObjectType == ElementType.Equippable)
            {
                Source = CreateNPC("Source", crsSource, issSource, clbSourceStatuses, equippableClass, heldItemClass);
                TestAction.User = Source;
            }
            else if (SourceObjectType == ElementType.Consumable)
            {
                Source = CreateConsumable(ClassId);
                TestAction.User = Source;
                if (!ActionTypeText.Equals("Item Use", StringComparison.InvariantCultureIgnoreCase))
                {
                    ItemOwner = CreateNPC("ItemOwner", crsSource, issSource, clbSourceStatuses, equippableClass, heldItemClass);
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
                    Target = CreateNPC("Target", crsTarget, issTarget, clbTargetStatuses, equippableClass, heldItemClass);
                }
                else
                {
                    Target = Source;
                }
            }
            else if (TargetObjectType == ElementType.Tile)
            {
                Target = TestDungeon.CurrentFloor.Tiles.ToList().First(t => t.IsWalkable && !t.IsOccupied);
            }

            try
            {
                TestDungeon.CurrentFloor.Snapshot = new(TestDungeon, TestDungeon.CurrentFloor);

                await TestAction.Do(Source, Target, false);

                txtMessageLog.Clear();

                var dungeonStatus = await TestDungeon.GetStatus();

                foreach (var displayEventList in dungeonStatus.DisplayEvents)
                {
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

                dungeonStatus.DisplayEvents.Clear();
            }
            catch (Exception ex)
            {
                txtMessageLog.AppendText("Oh no! The Action could not be executed!\n", Color.Red);
                txtMessageLog.AppendText(ex.Message, Color.White);
                txtMessageLog.AppendText("\n---------------------\n", Color.Red);
                txtMessageLog.AppendText(ex.StackTrace, Color.Yellow);
            }
        }

        private NonPlayableCharacter CreateNPC(string name, ConsoleRepresentationSelector crs, ItemStatsSheet iss, CheckedListBox clb, EntityClass equippableClass, EntityClass inventoryClass)
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
                InventorySize = 5,
                BaseSightRange = "full map",
                AIType = "Default",
                ExperiencePayoutFormula = "1",
                ExperienceToLevelUpFormula = "9999",
                CanGainExperience = true,
                ConsoleRepresentation = crs.ConsoleRepresentation
            };
            issSource.TreatStatsAsAbsolute = true;
            foreach (var stat in iss.Stats)
            {
                npcClassInfo.Stats.Add(new()
                {
                    StatId = stat.Id,
                    Base = stat.Amount,
                    IncreasePerLevel = 0
                });
            }
            var npcClass = new EntityClass(npcClassInfo, TestDungeon.LocaleToUse, EntityType.NPC, ActiveDungeon.CharacterStats);
            var npc = new NonPlayableCharacter(npcClass, 1, TestDungeon.CurrentFloor);
            foreach (var item in clb.CheckedItems)
            {
                var status = TestDungeon.CurrentFloor.PossibleStatuses.Find(s => s.ClassId.Equals(item.ToString()));
                status?.ApplyTo(npc, 0, 1);
            }
            npc.Visible = true;
            npc.Position = TestDungeon.CurrentFloor.Tiles.Where(t => t.IsWalkable && !t.IsOccupied)[0].Position;
            npc.HP.Current -= npc.HP.Current / 5;
            npc.MP.Current -= npc.MP.Current / 5;
            npc.Hunger.Current -= npc.Hunger.Current / 5;
            npc.StartingWeapon = new Item(equippableClass, TestDungeon.CurrentFloor);
            npc.StartingWeapon.Power = "1d3";
            npc.StartingArmor = new Item(equippableClass, TestDungeon.CurrentFloor);
            npc.StartingArmor.Power = "0";
            npc.Inventory.Add(new Item(inventoryClass, TestDungeon.CurrentFloor));

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
                ConsoleRepresentation = crsSource.ConsoleRepresentation
            };
            var consumableClass = new EntityClass(consumableClassInfo, TestDungeon.LocaleToUse, EntityType.Consumable, ActiveDungeon.CharacterStats);
            return new Item(consumableClass, TestDungeon.CurrentFloor);
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
            var trapClass = new EntityClass(trapClassInfo, TestDungeon.LocaleToUse, EntityType.Trap, ActiveDungeon.CharacterStats);
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
            var alteredStatusClass = new EntityClass(alteredStatusClassInfo, TestDungeon.LocaleToUse, EntityType.AlteredStatus, ActiveDungeon.CharacterStats);
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
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma

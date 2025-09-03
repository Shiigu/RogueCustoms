using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    #pragma warning disable S2589 // Boolean expressions should not be gratuitous
    #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public static class DungeonInfoHelpers
    {
        public static DungeonInfo CreateEmptyDungeonTemplate(LocaleInfo localeTemplate, List<string> baseLocaleLanguages)
        {
            var templateDungeon = new DungeonInfo
            {
                Locales = new(),
                TileSetInfos = new(),
                TileTypeInfos = new(),
                FloorInfos = new(),
                FactionInfos = new(),
                CharacterStats = new(),
                ElementInfos = new(),
                ActionSchoolInfos = new(),
                PlayerClasses = new(),
                NPCs = new(),
                Items = new(),
                Traps = new(),
                AlteredStatuses = new()
            };

            foreach (var localeLanguage in baseLocaleLanguages)
            {
                var newLocale = localeTemplate.Clone(localeTemplate.LocaleStrings.ConvertAll(ls => ls.Key));
                newLocale.Language = localeLanguage;

                templateDungeon.Locales.Add(newLocale);
            }

            templateDungeon.Name = "DungeonName";
            templateDungeon.Author = "Author";
            templateDungeon.WelcomeMessage = "WelcomeMessage";
            templateDungeon.EndingMessage = "EndingMessage";

            templateDungeon.TileTypeInfos = CreateDefaultTileTypes();

            templateDungeon.TileSetInfos.Add(CreateDefaultTileSet());
            templateDungeon.TileSetInfos.Add(CreateRetroTileSet());

            templateDungeon.FloorInfos.Add(CreateFloorGroupTemplate());

            templateDungeon.FactionInfos.Add(CreateFactionTemplate());

            templateDungeon.CharacterStats = CreateStatsTemplate();

            templateDungeon.ElementInfos = new() { CreateElementTemplate() };

            templateDungeon.ActionSchoolInfos = new() { CreateActionSchoolTemplate() };

            templateDungeon.PlayerClasses.Add(CreatePlayerClassTemplate(templateDungeon.CharacterStats));

            templateDungeon.NPCs.Add(CreateNPCTemplate(templateDungeon.CharacterStats));

            templateDungeon.Items.Add(CreateItemTemplate());

            templateDungeon.Traps.Add(CreateTrapTemplate());

            templateDungeon.AlteredStatuses.Add(CreateAlteredStatusTemplate());

            templateDungeon.Scripts = new();

            templateDungeon.DefaultLocale = baseLocaleLanguages[0];

            return templateDungeon;
        }

        public static List<TileTypeInfo> CreateDefaultTileTypes()
        {
            var empty = new TileTypeInfo()
            {
                Id = "Empty",
                Name = "TileTypeEmpty",
                IsVisible = true,
                IsWalkable = false,
                IsSolid = false,
                CanBeTransformed = false,
                CanVisiblyConnectWithOtherTiles = false,
                CanHaveMultilineConnections = false,
                AcceptsItems = false
            };
            var floor = new TileTypeInfo()
            {
                Id = "Floor",
                Name = "TileTypeFloor",
                IsVisible = true,
                IsWalkable = true,
                IsSolid = false,
                CanBeTransformed = true,
                CanVisiblyConnectWithOtherTiles = false,
                CanHaveMultilineConnections = false,
                AcceptsItems = true
            };
            var wall = new TileTypeInfo()
            {
                Id = "Wall",
                Name = "TileTypeWall",
                IsVisible = false,
                IsWalkable = false,
                IsSolid = true,
                CanBeTransformed = true,
                CanVisiblyConnectWithOtherTiles = true,
                CanHaveMultilineConnections = false,
                AcceptsItems = false
            };
            var hallway = new TileTypeInfo()
            {
                Id = "Hallway",
                Name = "TileTypeHallway",
                IsVisible = true,
                IsWalkable = true,
                IsSolid = false,
                CanBeTransformed = true,
                CanVisiblyConnectWithOtherTiles = true,
                CanHaveMultilineConnections = true,
                AcceptsItems = true
            };
            var stairs = new TileTypeInfo()
            {
                Id = "Stairs",
                Name = "TileTypeStairs",
                IsVisible = true,
                IsWalkable = true,
                IsSolid = false,
                CanBeTransformed = false,
                CanVisiblyConnectWithOtherTiles = false,
                CanHaveMultilineConnections = false,
                AcceptsItems = false
            };

            return new() { floor, empty, wall, hallway, stairs };
        }

        public static TileTypeInfo CreateTileTypeTemplate()
        {
            return new TileTypeInfo()
            {
                Name = "TileTypeCustom",
                Description = "TileTypeCustomDescription",
                IsVisible = true,
                IsWalkable = true,
                IsSolid = false,
                CanBeTransformed = false,
                CanVisiblyConnectWithOtherTiles = false,
                CanHaveMultilineConnections = false,
                AcceptsItems = false,
                OnStood = null
            };
        }

        public static TileSetInfo CreateDefaultTileSet()
        {
            return new TileSetInfo
            {
                Id = "Default",
                TileTypes = new List<TileTypeSetInfo>
                {
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Wall",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        Connector = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        TopLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '█'
                        },
                        TopRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '█'
                        },
                        BottomLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '█'
                        },
                        BottomRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '█'
                        },
                        Horizontal = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '█'
                        },
                        Vertical = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '█'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Hallway",
                        Connector = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        TopLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        TopRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        BottomLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        BottomRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        Horizontal = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        HorizontalTop = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        HorizontalBottom = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        Vertical = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        VerticalLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        VerticalRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        },
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Blue),
                            Character = '▒'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Floor",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.DarkGray),
                            Character = '.'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Stairs",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Yellow),
                            ForegroundColor = new GameColor(Color.DarkGreen),
                            Character = '>'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Empty",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Black),
                            Character = ' '
                        }
                    }
                }
            };
        }

        public static TileSetInfo CreateRetroTileSet()
        {
            return new TileSetInfo
            {
                Id = "Retro",
                TileTypes = new List<TileTypeSetInfo>
                {
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Wall",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '╬'
                        },
                        Connector = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '╬'
                        },
                        TopLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '╔'
                        },
                        TopRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '╗'
                        },
                        BottomLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '╚'
                        },
                        BottomRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '╝'
                        },
                        Horizontal = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '═'
                        },
                        Vertical = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '║'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Hallway",
                        Connector = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        TopLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        TopRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        BottomLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        BottomRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        Horizontal = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        HorizontalTop = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        HorizontalBottom = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        Vertical = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        VerticalLeft = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        VerticalRight = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        },
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                            Character = '▒'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Floor",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 84, 84, 84)),
                            Character = '·'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Stairs",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                            Character = '>'
                        }
                    },
                    new TileTypeSetInfo
                    {
                        TileTypeId = "Empty",
                        Central = new ConsoleRepresentation
                        {
                            BackgroundColor = new GameColor(Color.Black),
                            ForegroundColor = new GameColor(Color.Black),
                            Character = ' '
                        }
                    }
                }
            };
        }

        public static TileTypeSetInfo CreateDefaultTileTypeSet(string tileTypeId)
        {
            var defaultConsoleRepresentation = new ConsoleRepresentation
            {
                BackgroundColor = new GameColor(Color.Black),
                ForegroundColor = new GameColor(Color.Black),
                Character = ' '
            };
            return new TileTypeSetInfo
            {
                TileTypeId = "tileTypeId",
                Connector = defaultConsoleRepresentation,
                TopLeft = defaultConsoleRepresentation,
                TopRight = defaultConsoleRepresentation,
                BottomLeft = defaultConsoleRepresentation,
                BottomRight = defaultConsoleRepresentation,
                Horizontal = defaultConsoleRepresentation,
                HorizontalTop = defaultConsoleRepresentation,
                HorizontalBottom = defaultConsoleRepresentation,
                Vertical = defaultConsoleRepresentation,
                VerticalLeft = defaultConsoleRepresentation,
                VerticalRight = defaultConsoleRepresentation,
                Central = defaultConsoleRepresentation
            };
        }
        public static FloorInfo CreateFloorGroupTemplate()
        {
            return new FloorInfo()
            {
                GenerateStairsOnStart = true,
                HungerDegeneration = 0,
                MinFloorLevel = 1,
                MaxFloorLevel = 1,
                MaxConnectionsBetweenRooms = 1,
                OddsForExtraConnections = 0,
                RoomFusionOdds = 0,
                MonsterHouseOdds = 0,
                TurnsPerMonsterGeneration = 35,
                MinItemsInFloor = 0,
                MaxItemsInFloor = 0,
                MinTrapsInFloor = 0,
                MaxTrapsInFloor = 0,
                SimultaneousMaxMonstersInFloor = 0,
                SimultaneousMinMonstersAtStart = 0,
                Width = 64,
                Height = 32,
                PossibleKeys = new(),
                PossibleMonsters = new(),
                PossibleItems = new(),
                PossibleTraps = new(),
                PossibleLayouts = new() { CreateFloorLayoutGenerationInfoTemplate() },
                OnFloorStart = new(),
            };
        }

        public static FloorLayoutGenerationInfo CreateFloorLayoutGenerationInfoTemplate()
        {
            return new FloorLayoutGenerationInfo
            {
                Name = "Default",
                Rows = 4,
                Columns = 4,
                MinRoomSize = new() { Width = 5, Height = 5 },
                MaxRoomSize = new() { Width = 5, Height = 5 },
                RoomDisposition = "???????? ? ? ????????? ? ? ????????? ? ? ????????"
            };
        }

        public static KeyTypeInfo CreateKeyTypeInfoTemplate()
        {
            return new KeyTypeInfo
            {
                KeyTypeName = "Standard",
                CanLockStairs = true,
                CanLockItems = true,
                KeyConsoleRepresentation = new()
                {
                    Character = '¥',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.White),
                },
                DoorConsoleRepresentation = new()
                {
                    Character = 'Θ',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.White),
                }
            };
        }

        public static FactionInfo CreateFactionTemplate()
        {
            return new FactionInfo()
            {
                Id = "DummyFaction",
                Name = "Faction",
                Description = "Description",
                AlliedWith = new(),
                NeutralWith = new(),
                EnemiesWith = new()
            };
        }

        public static StatInfo CreateStatTemplate()
        {
            return new StatInfo()
            {
                StatType = "Integer",
                Name = "CharacterCustomStat",
                HasMax = false,
                MinCap = 0,
                MaxCap = EngineConstants.NORMAL_STAT_CAP,
                RegeneratesStatId = ""
            };
        }

        public static List<StatInfo> CreateStatsTemplate()
        {
            var hp = new StatInfo()
            {
                Id = "HP",
                StatType = "HP",
                Name = "CharacterHPStat",
                HasMax = true,
                MinCap = 0,
                MaxCap = EngineConstants.RESOURCE_STAT_CAP
            };
            var hpRegeneration = new StatInfo()
            {
                Id = "HPRegeneration",
                StatType = "Regeneration",
                Name = "CharacterHPRegenerationStat",
                RegeneratesStatId = "HP",
                HasMax = false,
                MinCap = 0,
                MaxCap = EngineConstants.REGEN_STAT_CAP
            };

            var mp = new StatInfo()
            {
                Id = "MP",
                StatType = "MP",
                Name = "CharacterMPStat",
                HasMax = true,
                MinCap = 0,
                MaxCap = EngineConstants.RESOURCE_STAT_CAP
            };
            var mpRegeneration = new StatInfo()
            {
                Id = "MPRegeneration",
                StatType = "Regeneration",
                Name = "CharacterMPRegenerationStat",
                RegeneratesStatId = "MP",
                HasMax = false,
                MinCap = 0,
                MaxCap = EngineConstants.REGEN_STAT_CAP
            };

            var hunger = new StatInfo()
            {
                Id = "Hunger",
                StatType = "Hunger",
                Name = "CharacterHungerStat",
                HasMax = true,
                MinCap = 0,
                MaxCap = EngineConstants.RESOURCE_STAT_CAP
            };

            var attack = new StatInfo()
            {
                Id = "Attack",
                StatType = "Integer",
                Name = "CharacterAttackStat",
                HasMax = false,
                MinCap = 0,
                MaxCap = EngineConstants.NORMAL_STAT_CAP
            };

            var defense = new StatInfo()
            {
                Id = "Defense",
                StatType = "Integer",
                Name = "CharacterDefenseStat",
                HasMax = false,
                MinCap = 0,
                MaxCap = EngineConstants.NORMAL_STAT_CAP
            };

            var movement = new StatInfo()
            {
                Id = "Movement",
                StatType = "Integer",
                Name = "CharacterMovementStat",
                HasMax = false,
                MinCap = 0,
                MaxCap = EngineConstants.MOVEMENT_STAT_CAP
            };

            var accuracy = new StatInfo()
            {
                Id = "Accuracy",
                StatType = "Percentage",
                Name = "CharacterAccuracyStat",
                HasMax = false,
                MinCap = EngineConstants.MIN_ACCURACY_CAP,
                MaxCap = EngineConstants.MAX_ACCURACY_CAP
            };

            var evasion = new StatInfo()
            {
                Id = "Evasion",
                StatType = "Percentage",
                Name = "CharacterEvasionStat",
                HasMax = false,
                MinCap = EngineConstants.MIN_EVASION_CAP,
                MaxCap = EngineConstants.MAX_EVASION_CAP
            };

            return new() { hp, hpRegeneration, mp, mpRegeneration, hunger, attack, defense, movement, accuracy, evasion };
        }

        public static ElementInfo CreateElementTemplate()
        {
            return new ElementInfo()
            {
                Id = "Element",
                Name = "ElementName",
                Color = new GameColor(Color.White),
                ResistanceStatId = "",
                ExcessResistanceCausesHealDamage = false,
                OnAfterAttack = null
            };
        }

        public static ActionSchoolInfo CreateActionSchoolTemplate()
        {
            return new ActionSchoolInfo()
            {
                Id = "School",
                Name = "ActionSchoolName",
            };
        }

        public static LootTableInfo CreateLootTableTemplate()
        {
            return new LootTableInfo()
            {
                Id = "Okay",
                Entries = []
            };
        }

        public static PlayerClassInfo CreatePlayerClassTemplate(List<StatInfo> stats)
        {
            var playerClassTemplate = new PlayerClassInfo
            {
                Id = "DummyPlayer",
                Name = "Player",
                Description = "Description",
                ConsoleRepresentation = new ConsoleRepresentation
                {
                    Character = 'P',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.SpringGreen)
                },
                RequiresNamePrompt = false,
                Faction = "",
                StartsVisible = true,
                Stats = new(),
                BaseSightRange = "FullRoom",
                StartingWeapon = "",
                StartingArmor = "",
                InventorySize = 0,
                StartingInventory = new(),
                CanGainExperience = true,
                ExperienceToLevelUpFormula = "",
                MaxLevel = 2,
                OnTurnStart = new(),
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new()
            };

            foreach (var stat in stats)
            {
                playerClassTemplate.Stats.Add(new CharacterStatInfo()
                {
                    StatId = stat.Id,
                    Base = stat.MinCap,
                    IncreasePerLevel = 0
                });
            }

            return playerClassTemplate;
        }
        public static NPCInfo CreateNPCTemplate(List<StatInfo> stats)
        {
            var npcTemplate = new NPCInfo
            {
                Id = "DummyNPC",
                Name = "NPC",
                Description = "Description",
                ConsoleRepresentation = new ConsoleRepresentation
                {
                    Character = 'N',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Red)
                },
                Faction = "",
                StartsVisible = true,
                KnowsAllCharacterPositions = true,
                PursuesOutOfSightCharacters = true,
                WandersIfWithoutTarget = true,
                ExperiencePayoutFormula = "level",
                Stats = new(),
                BaseSightRange = "FullRoom",
                StartingWeapon = "",
                StartingArmor = "",
                InventorySize = 0,
                StartingInventory = new(),
                CanGainExperience = true,
                ExperienceToLevelUpFormula = "",
                MaxLevel = 2,
                OnTurnStart = new(),
                OnAttack = new(),
                OnAttacked = new(),
                OnDeath = new(),
                AIType = "Default"
            };

            foreach (var stat in stats)
            {
                npcTemplate.Stats.Add(new CharacterStatInfo()
                {
                    StatId = stat.Id,
                    Base = stat.MinCap,
                    IncreasePerLevel = 0
                });
            }

            return npcTemplate;
        }
        public static ItemInfo CreateItemTemplate()
        {
            return new ItemInfo
            {
                Id = "DummyItem",
                Name = "Item",
                Description = "Description",
                ConsoleRepresentation = new ConsoleRepresentation
                {
                    Character = '?',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.MediumPurple)
                },
                StartsVisible = true,
                Power = "0",
                EntityType = "",
                OnTurnStart = new(),
                OnAttack = new(),
                OnAttacked = new(),
                OnUse = new(),
            };
        }

        public static TrapInfo CreateTrapTemplate()
        {
            return new TrapInfo
            {
                Id = "DummyTrap",
                Name = "Trap",
                Description = "Description",
                ConsoleRepresentation = new ConsoleRepresentation
                {
                    Character = '!',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.DarkRed)
                },
                StartsVisible = false,
                Power = "0",
                OnStepped = new(),
            };
        }

        public static AlteredStatusInfo CreateAlteredStatusTemplate()
        {
            return new AlteredStatusInfo
            {
                Id = "DummyStatus",
                Name = "AlteredStatus",
                Description = "Description",
                ConsoleRepresentation = new ConsoleRepresentation
                {
                    Character = 'X',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.DarkMagenta)
                },
                CanStack = false,
                CanOverwrite = false,
                CleansedByCleanseActions = true,
                CleanseOnFloorChange = true,
                OnApply = new(),
                OnTurnStart = new(),
            };
        }

        public static bool HasOverlappingFloorInfosForLevels(this DungeonInfo dungeon, int minFloorLevel, int maxFloorLevel, FloorInfo floorBeingEdited)
        {
            for (int i = minFloorLevel; i <= maxFloorLevel; i++)
            {
                var typesForFloorLevel = dungeon.FloorInfos.Where(fi => (floorBeingEdited == null || floorBeingEdited != fi) && i.Between(fi.MinFloorLevel, fi.MaxFloorLevel));
                if (typesForFloorLevel.Count() > 0)
                    return true;
            }
            return false;
        }

        public static LocaleInfo Clone(this LocaleInfo localeInfo, List<string> mandatoryLocaleKeys)
        {
            var newLocale = new LocaleInfo
            {
                Language = localeInfo.Language,
                LocaleStrings = new()
            };
            foreach (var mandatoryKey in mandatoryLocaleKeys)
            {
                var localeEntry = localeInfo.LocaleStrings.Find(ls => mandatoryKey.Equals(ls.Key));
                if (localeEntry != null)
                {
                    newLocale.LocaleStrings.Add(new LocaleInfoString
                    {
                        Key = localeEntry.Key,
                        Value = localeEntry.Value
                    });
                }
            }
            foreach (var localeEntry in localeInfo.LocaleStrings.Where(ls => !mandatoryLocaleKeys.Contains(ls.Key)))
            {
                newLocale.LocaleStrings.Add(new LocaleInfoString
                {
                    Key = localeEntry.Key,
                    Value = localeEntry.Value
                });
            }
            return newLocale;
        }

        public static FloorInfo Clone(this FloorInfo info)
        {
            if (info == null) return null;

            var clonedFloor = new FloorInfo
            {
                MinFloorLevel = info.MinFloorLevel,
                MaxFloorLevel = info.MaxFloorLevel,
                TileSetId = info.TileSetId,
                Width = info.Width,
                Height = info.Height,
                GenerateStairsOnStart = info.GenerateStairsOnStart,
                PossibleMonsters = new List<ClassInFloorInfo>(),
                PossibleItems = new List<ClassInFloorInfo>(),
                PossibleTraps = new List<ClassInFloorInfo>(),
                PossibleLayouts = new List<FloorLayoutGenerationInfo>(),
                PossibleSpecialTiles = new List<SpecialTileInFloorInfo>(),
                MaxConnectionsBetweenRooms = info.MaxConnectionsBetweenRooms,
                OddsForExtraConnections = info.OddsForExtraConnections,
                RoomFusionOdds = info.RoomFusionOdds,
                OnFloorStart = info.OnFloorStart.Clone(),
                SimultaneousMinMonstersAtStart = info.SimultaneousMinMonstersAtStart,
                SimultaneousMaxMonstersInFloor = info.SimultaneousMaxMonstersInFloor,
                TurnsPerMonsterGeneration = info.TurnsPerMonsterGeneration,
                HungerDegeneration = info.HungerDegeneration,
                MinItemsInFloor = info.MinItemsInFloor,
                MaxItemsInFloor = info.MaxItemsInFloor,
                MinTrapsInFloor = info.MinTrapsInFloor,
                MaxTrapsInFloor = info.MaxTrapsInFloor,
                PossibleKeys = info.PossibleKeys.Clone(),
                MonsterHouseOdds = info.MonsterHouseOdds
            };

            info.PossibleMonsters.ForEach(pm => clonedFloor.PossibleMonsters.Add(pm.Clone()));
            info.PossibleItems.ForEach(pi => clonedFloor.PossibleItems.Add(pi.Clone()));
            info.PossibleTraps.ForEach(pt => clonedFloor.PossibleTraps.Add(pt.Clone()));
            info.PossibleLayouts.ForEach(pl => clonedFloor.PossibleLayouts.Add(pl.Clone()));
            info.PossibleSpecialTiles.ForEach(pst => clonedFloor.PossibleSpecialTiles.Add(pst.Clone()));

            return clonedFloor;
        }

        public static SpecialTileInFloorInfo Clone(this SpecialTileInFloorInfo info)
        {
            if (info == null) return null;

            var clonedSpecialTile = new SpecialTileInFloorInfo
            {
                TileTypeId = info.TileTypeId,
                GeneratorType = info.GeneratorType,
                MinSpecialTileGenerations = info.MinSpecialTileGenerations,
                MaxSpecialTileGenerations = info.MaxSpecialTileGenerations
            };

            return clonedSpecialTile;
        }

        public static FloorLayoutGenerationInfo Clone(this FloorLayoutGenerationInfo info)
        {
            if (info == null) return null;

            var clonedLayout = new FloorLayoutGenerationInfo
            {
                Name = info.Name,
                MinRoomSize = new() { Width = info.MinRoomSize.Width, Height = info.MinRoomSize.Height },
                MaxRoomSize = new() { Width = info.MaxRoomSize.Width, Height = info.MaxRoomSize.Height },
                Columns = info.Columns,
                Rows = info.Rows,
                RoomDisposition = new string(info.RoomDisposition)
            };

            return clonedLayout;
        }

        public static ClassInFloorInfo Clone(this ClassInFloorInfo info)
        {
            if (info == null) return null;

            var clonedClassInFloor = new ClassInFloorInfo
            {
                ClassId = info.ClassId,
                MinLevel = info.MinLevel,
                MaxLevel = info.MaxLevel,
                OverallMaxForKindInFloor = info.OverallMaxForKindInFloor,
                CanSpawnOnFirstTurn = info.CanSpawnOnFirstTurn,
                CanSpawnAfterFirstTurn = info.CanSpawnAfterFirstTurn,
                MinimumInFirstTurn = info.MinimumInFirstTurn,
                SimultaneousMaxForKindInFloor = info.SimultaneousMaxForKindInFloor,
                ChanceToPick = info.ChanceToPick,
                SpawnCondition = info.SpawnCondition
            };

            return clonedClassInFloor;
        }

        public static KeyGenerationInfo Clone(this KeyGenerationInfo info)
        {
            if (info == null) return null;

            var clonedKeyGenerationInfo = new KeyGenerationInfo
            {
                LockedRoomOdds = info.LockedRoomOdds,
                KeySpawnInEnemyInventoryOdds = info.KeySpawnInEnemyInventoryOdds,
                MaxPercentageOfLockedCandidateRooms = info.MaxPercentageOfLockedCandidateRooms,
                KeyTypes = new()
            };
            info.KeyTypes.ForEach(pk => clonedKeyGenerationInfo.KeyTypes.Add(pk.Clone()));

            return clonedKeyGenerationInfo;
        }

        public static KeyTypeInfo Clone(this KeyTypeInfo info)
        {
            if (info == null) return null;

            var clonedKeyType = new KeyTypeInfo
            {
                KeyTypeName = info.KeyTypeName,
                CanLockItems = info.CanLockItems,
                CanLockStairs = info.CanLockStairs,
                DoorConsoleRepresentation = info.DoorConsoleRepresentation.Clone(),
                KeyConsoleRepresentation = info.KeyConsoleRepresentation.Clone()
            };

            return clonedKeyType;
        }

        public static ActionWithEffectsInfo Clone(this ActionWithEffectsInfo info)
        {
            if (info == null) return null;

            var clonedAction = new ActionWithEffectsInfo
            {
                Id = info.Id,
                Name = info.Name,
                Description = info.Description,
                CooldownBetweenUses = info.CooldownBetweenUses,
                StartingCooldown = info.StartingCooldown,
                MinimumRange = info.MinimumRange,
                MaximumRange = info.MaximumRange,
                MaximumUses = info.MaximumUses,
                MPCost = info.MPCost,
                TargetTypes = new List<string>(info.TargetTypes ?? new List<string>()),
                Effect = info.Effect.Clone(),
                UseCondition = info.UseCondition,
                AIUseCondition = info.AIUseCondition,
                FinishesTurnWhenUsed = info.FinishesTurnWhenUsed,
                IsScript = info.IsScript
            };

            return clonedAction;
        }

        public static EffectInfo Clone(this EffectInfo info)
        {
            if (info == null) return null;

            var clonedEffect = new EffectInfo
            {
                EffectName = info.EffectName,
                Params = new Parameter[info.Params.Length]
            };

            for (int i = 0; i < clonedEffect.Params.Length; i++)
            {
                clonedEffect.Params[i] = new Parameter
                {
                    ParamName = info.Params[i].ParamName,
                    Value = info.Params[i].Value
                };
            }

            if (!string.IsNullOrWhiteSpace(info?.Then?.EffectName))
                clonedEffect.Then = info.Then.Clone();
            if (!string.IsNullOrWhiteSpace(info?.OnSuccess?.EffectName))
                clonedEffect.OnSuccess = info.OnSuccess.Clone();
            if (!string.IsNullOrWhiteSpace(info?.OnFailure?.EffectName))
                clonedEffect.OnFailure = info.OnFailure.Clone();

            return clonedEffect;
        }

        public static void PruneNullActions(this DungeonInfo dungeonInfo)
        {
            foreach (var tileType in dungeonInfo.TileTypeInfos.Where(tti => tti.OnStood.IsNullOrEmpty()))
            {
                tileType.OnStood = null;
            }

            foreach (var floorInfo in dungeonInfo.FloorInfos.Where(floorInfo => floorInfo.OnFloorStart.IsNullOrEmpty()))
            {
                floorInfo.OnFloorStart = null;
            }

            foreach (var element in dungeonInfo.ElementInfos.Where(e => e.OnAfterAttack.IsNullOrEmpty()))
            {
                element.OnAfterAttack = null;
            }

            foreach (var playerClass in dungeonInfo.PlayerClasses)
            {
                if (playerClass == null) continue;
                if (playerClass.OnTurnStart.IsNullOrEmpty())
                    playerClass.OnTurnStart = null;
                playerClass.OnAttack ??= [];
                playerClass.OnAttack.RemoveAll(oa => oa.IsNullOrEmpty());
                if (playerClass.OnAttacked.IsNullOrEmpty())
                    playerClass.OnAttacked = null;
                if (playerClass.OnDeath.IsNullOrEmpty())
                    playerClass.OnDeath = null;
                if (playerClass.OnLevelUp.IsNullOrEmpty())
                    playerClass.OnLevelUp = null;
            }

            foreach (var npc in dungeonInfo.NPCs)
            {
                if (npc == null) continue;
                if (npc.OnSpawn.IsNullOrEmpty())
                    npc.OnSpawn = null;
                if (npc.OnTurnStart.IsNullOrEmpty())
                    npc.OnTurnStart = null;
                npc.OnAttack ??= [];
                npc.OnAttack.RemoveAll(oa => oa.IsNullOrEmpty());
                npc.OnInteracted ??= [];
                npc.OnInteracted.RemoveAll(oa => oa.IsNullOrEmpty());
                if (npc.OnAttacked.IsNullOrEmpty())
                    npc.OnAttacked = null;
                if (npc.OnDeath.IsNullOrEmpty())
                    npc.OnDeath = null;
                if (npc.OnLevelUp.IsNullOrEmpty())
                    npc.OnLevelUp = null;
            }

            foreach (var item in dungeonInfo.Items)
            {
                if (item == null) continue;
                if (item.OnTurnStart.IsNullOrEmpty())
                    item.OnTurnStart = null;
                item.OnAttack ??= [];
                item.OnAttack.RemoveAll(oa => oa.IsNullOrEmpty());
                if (item.OnAttacked.IsNullOrEmpty())
                    item.OnAttacked = null;
                if (item.OnUse.IsNullOrEmpty())
                    item.OnUse = null;
                if (item.OnDeath.IsNullOrEmpty())
                    item.OnDeath = null;
            }

            foreach (var trap in dungeonInfo.Traps)
            {
                if (trap == null) continue;
                if (trap.OnStepped.IsNullOrEmpty())
                    trap.OnStepped = null;
            }

            foreach (var alteredStatus in dungeonInfo.AlteredStatuses)
            {
                if (alteredStatus == null) continue;
                if (alteredStatus.OnTurnStart.IsNullOrEmpty())
                    alteredStatus.OnTurnStart = null;
                if (alteredStatus.OnApply.IsNullOrEmpty())
                    alteredStatus.OnApply = null;
                if (alteredStatus.BeforeAttack.IsNullOrEmpty())
                    alteredStatus.BeforeAttack = null;
                if (alteredStatus.OnAttacked.IsNullOrEmpty())
                    alteredStatus.OnAttacked = null;
                if (alteredStatus.OnRemove.IsNullOrEmpty())
                    alteredStatus.OnRemove = null;
            }
        }

        public static bool AddMissingMandatoryLocalesIfNeeded(this LocaleInfo localeInfo, LocaleInfo localeTemplate, List<string> mandatoryLocaleKeys)
        {
            var localeKeys = localeInfo.LocaleStrings.Select(x => x.Key).ToList();
            var missingMandatoryKeys = mandatoryLocaleKeys.Except(localeKeys);
            foreach (var missingKey in missingMandatoryKeys)
            {
                var templateLocaleEntry = localeTemplate.LocaleStrings.Find(ls => ls.Key.Equals(missingKey));
                localeInfo.LocaleStrings.Add(new LocaleInfoString
                {
                    Key = templateLocaleEntry.Key,
                    Value = templateLocaleEntry.Value
                });
            }
            return missingMandatoryKeys.Any();
        }

        public static bool IsNullOrEmpty(this ActionWithEffectsInfo? action)
        {
            return action == null || action.Effect.IsNullOrEmpty();
        }

        public static bool IsNullOrEmpty(this EffectInfo? effect)
        {
            return effect == null || string.IsNullOrWhiteSpace(effect.EffectName);
        }

        public static List<FloorInfo> FindIntersectingFloorGroups(this DungeonInfo dungeonInfo, int minFloorLevel, int maxFloorLevel)
        {
            if (dungeonInfo == null || dungeonInfo.FloorInfos == null) return new();
            return dungeonInfo.FloorInfos.Where(fi => IntHelpers.DoIntervalsIntersect(minFloorLevel, maxFloorLevel, fi.MinFloorLevel, fi.MaxFloorLevel)).ToList();
        }

        public static List<TileTypeInfo> GetSpecialTileTypes(this DungeonInfo dungeonInfo, List<string> defaultTileTypeIds)
        {
            return dungeonInfo.TileTypeInfos.Where(tti => !defaultTileTypeIds.Contains(tti.Id, StringComparer.OrdinalIgnoreCase)).ToList();
        }

        public static List<string> GetAllIds(this DungeonInfo dungeonInfo, Type typeToExclude)
        {
            var ids = new List<string>();
            if (typeToExclude != typeof(TileTypeInfo))
                ids.AddRange(dungeonInfo.TileTypeInfos.Select(t => t.Id));
            if (typeToExclude != typeof(TileSetInfo))
                ids.AddRange(dungeonInfo.TileSetInfos.Select(t => t.Id));
            if (typeToExclude != typeof(FactionInfo))
                ids.AddRange(dungeonInfo.FactionInfos.Select(f => f.Id));
            if (typeToExclude != typeof(CharacterStatInfo))
                ids.AddRange(dungeonInfo.CharacterStats.Select(s => s.Id));
            if (typeToExclude != typeof(ElementInfo))
                ids.AddRange(dungeonInfo.ElementInfos.Select(e => e.Id));
            if (typeToExclude != typeof(ActionSchoolInfo))
                ids.AddRange(dungeonInfo.ActionSchoolInfos.Select(a => a.Id));
            if (typeToExclude != typeof(LootTableInfo))
                ids.AddRange(dungeonInfo.LootTableInfos.Select(a => a.Id));
            if (typeToExclude != typeof(PlayerClassInfo))
                ids.AddRange(dungeonInfo.PlayerClasses.Select(p => p.Id));
            if (typeToExclude != typeof(NPCInfo))
                ids.AddRange(dungeonInfo.NPCs.Select(n => n.Id));
            if (typeToExclude != typeof(ItemInfo))
                ids.AddRange(dungeonInfo.Items.Select(i => i.Id));
            if (typeToExclude != typeof(TrapInfo))
                ids.AddRange(dungeonInfo.Traps.Select(t => t.Id));
            if (typeToExclude != typeof(AlteredStatusInfo))
                ids.AddRange(dungeonInfo.AlteredStatuses.Select(a => a.Id));
            return ids;
        }
    }
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

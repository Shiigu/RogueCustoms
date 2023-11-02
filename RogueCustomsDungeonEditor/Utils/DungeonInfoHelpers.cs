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
                FloorInfos = new(),
                FactionInfos = new(),
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

            templateDungeon.TileSetInfos.Add(CreateDefaultTileSet());
            templateDungeon.TileSetInfos.Add(CreateRetroTileSet());

            templateDungeon.FloorInfos.Add(CreateFloorGroupTemplate());

            templateDungeon.FactionInfos.Add(CreateFactionTemplate());

            templateDungeon.PlayerClasses.Add(CreatePlayerClassTemplate());

            templateDungeon.NPCs.Add(CreateNPCTemplate());

            templateDungeon.Items.Add(CreateItemTemplate());

            templateDungeon.Traps.Add(CreateTrapTemplate());

            templateDungeon.AlteredStatuses.Add(CreateAlteredStatusTemplate());

            templateDungeon.DefaultLocale = baseLocaleLanguages[0];

            return templateDungeon;
        }

        public static TileSetInfo CreateDefaultTileSet()
        {
            return new TileSetInfo
            {
                Id = "Default",
                TopLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                TopRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                BottomLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                BottomRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                HorizontalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                VerticalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                ConnectorWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                TopLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                TopRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                BottomLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                BottomRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                HorizontalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                VerticalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                HorizontalTopHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                HorizontalBottomHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                VerticalRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                VerticalLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                CentralHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                Floor = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.DarkGray),
                    Character = '.'
                },
                Stairs = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Yellow),
                    ForegroundColor = new GameColor(Color.DarkGreen),
                    Character = '>'
                },
                Empty = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Black),
                    Character = ' '
                }
            };
        }

        public static TileSetInfo CreateRetroTileSet()
        {
            return new TileSetInfo
            {
                Id = "Retro",
                TopLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╔'
                },
                TopRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╗'
                },
                BottomLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╚'
                },
                BottomRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╝'
                },
                HorizontalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '═'
                },
                VerticalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '║'
                },
                ConnectorWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╬'
                },
                TopLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                TopRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                BottomLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                BottomRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                HorizontalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                VerticalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                HorizontalTopHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                HorizontalBottomHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                VerticalRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                VerticalLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                CentralHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                Floor = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 84, 252, 84)),
                    Character = '.'
                },
                Stairs = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 84, 252, 84)),
                    Character = '╫'
                },
                Empty = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Black),
                    Character = ' '
                }
            };
        }

        public static FloorInfo CreateFloorGroupTemplate()
        {
            return new FloorInfo()
            {
                MinFloorLevel = 1,
                MaxFloorLevel = 1,
                Width = 5,
                Height = 5,
                PossibleMonsters = new(),
                PossibleItems = new(),
                PossibleTraps = new(),
                PossibleGeneratorAlgorithms = new()
                {
                    new GeneratorAlgorithmInfo
                    {
                        Name = "OneBigRoom",
                        Rows = 1,
                        Columns = 1
                    }
                },
                OnFloorStart = new()
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

        public static PlayerClassInfo CreatePlayerClassTemplate()
        {
            return new PlayerClassInfo
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
                UsesMP = false,
                BaseHP = 1,
                MaxHPIncreasePerLevel = 0,
                BaseMP = 0,
                MaxMPIncreasePerLevel = 0,
                BaseAttack = 0,
                AttackIncreasePerLevel = 0,
                BaseDefense = 0,
                DefenseIncreasePerLevel = 0,
                BaseMovement = 1,
                MovementIncreasePerLevel = 0,
                BaseHPRegeneration = 0,
                HPRegenerationIncreasePerLevel = 0,
                BaseMPRegeneration = 0,
                MPRegenerationIncreasePerLevel = 0,
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
        }
        public static NPCInfo CreateNPCTemplate()
        {
            return new NPCInfo
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
                ExperiencePayoutFormula = "level",
                UsesMP = false,
                BaseHP = 1,
                MaxHPIncreasePerLevel = 0,
                BaseMP = 0,
                MaxMPIncreasePerLevel = 0,
                BaseAttack = 0,
                AttackIncreasePerLevel = 0,
                BaseDefense = 0,
                DefenseIncreasePerLevel = 0,
                BaseMovement = 1,
                MovementIncreasePerLevel = 0,
                BaseHPRegeneration = 0,
                HPRegenerationIncreasePerLevel = 0,
                BaseMPRegeneration = 0,
                MPRegenerationIncreasePerLevel = 0,
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
                AIOddsToUseActionsOnSelf = 50
            };
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
                CanBePickedUp = true,
                StartsVisible = true,
                Power = "0",
                EntityType = "",
                OnStepped = new(),
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

        public static ActionWithEffectsInfo CreateEquipAction()
        {
            return new ActionWithEffectsInfo
            {
                Name = "Equip",
                Effect = new EffectInfo
                {
                    EffectName = "PrintText",
                    Params = new Parameter[]
                    {
                        new Parameter
                        {
                            ParamName = "text",
                            Value = "ObjectEquippedText"
                        }
                    },
                    Then = new EffectInfo
                    {
                        EffectName = "Equip",
                        Params = Array.Empty<Parameter>()
                    }
                }
            };
        }

        public static bool HasOverlappingFloorInfosForLevels(this DungeonInfo dungeon, int minFloorLevel, int maxFloorLevel, FloorInfo floorBeingEdited)
        {
            for (int i = minFloorLevel; i <= maxFloorLevel; i++)
            {
                var typesForFloorLevel = dungeon.FloorInfos.Where(fi => (floorBeingEdited == null || floorBeingEdited != fi) && fi.MinFloorLevel <= fi.MaxFloorLevel && i.Between(fi.MinFloorLevel, fi.MaxFloorLevel));
                if (typesForFloorLevel.Count() > 1)
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
                Width = info.Width,
                Height = info.Height,
                GenerateStairsOnStart = info.GenerateStairsOnStart,
                PossibleMonsters = new List<ClassInFloorInfo>(info.PossibleMonsters),
                PossibleItems = new List<ClassInFloorInfo>(info.PossibleItems),
                PossibleTraps = new List<ClassInFloorInfo>(info.PossibleTraps),
                PossibleGeneratorAlgorithms = new List<GeneratorAlgorithmInfo>(info.PossibleGeneratorAlgorithms),
                MaxConnectionsBetweenRooms = info.MaxConnectionsBetweenRooms,
                OddsForExtraConnections = info.OddsForExtraConnections,
                RoomFusionOdds = info.RoomFusionOdds,
                OnFloorStart = info.OnFloorStart.Clone()
            };

            return clonedFloor;
        }

        public static ActionWithEffectsInfo Clone(this ActionWithEffectsInfo info)
        {
            if (info == null) return null;

            var clonedAction = new ActionWithEffectsInfo
            {
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
                UseCondition = info.UseCondition
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
            foreach (var floorInfo in dungeonInfo.FloorInfos.Where(floorInfo => string.IsNullOrWhiteSpace(floorInfo.OnFloorStart?.Name)))
            {
                floorInfo.OnFloorStart = null;
            }

            foreach (var playerClass in dungeonInfo.PlayerClasses)
            {
                if (playerClass == null) continue;
                if (string.IsNullOrWhiteSpace(playerClass?.OnTurnStart?.Name))
                    playerClass.OnTurnStart = null;
                playerClass.OnAttack.RemoveAll(oa => oa == null);
                if (string.IsNullOrWhiteSpace(playerClass?.OnAttacked?.Name))
                    playerClass.OnAttacked = null;
                if (string.IsNullOrWhiteSpace(playerClass?.OnDeath?.Name))
                    playerClass.OnDeath = null;
            }

            foreach (var npc in dungeonInfo.NPCs)
            {
                if (npc == null) continue;
                if (string.IsNullOrWhiteSpace(npc?.OnTurnStart?.Name))
                    npc.OnTurnStart = null;
                npc.OnAttack.RemoveAll(oa => oa == null);
                if (string.IsNullOrWhiteSpace(npc?.OnAttacked?.Name))
                    npc.OnAttacked = null;
                if (string.IsNullOrWhiteSpace(npc?.OnDeath?.Name))
                    npc.OnDeath = null;
            }

            foreach (var item in dungeonInfo.Items)
            {
                if (item == null) continue;
                if (string.IsNullOrWhiteSpace(item?.OnTurnStart?.Name))
                    item.OnTurnStart = null;
                item.OnAttack.RemoveAll(oa => oa == null);
                if (string.IsNullOrWhiteSpace(item?.OnAttacked?.Name))
                    item.OnAttacked = null;
                if (string.IsNullOrWhiteSpace(item?.OnUse?.Name))
                    item.OnUse = null;
                if (string.IsNullOrWhiteSpace(item?.OnStepped?.Name))
                    item.OnStepped = null;
            }

            foreach (var trap in dungeonInfo.Traps)
            {
                if (trap == null) continue;
                if (string.IsNullOrWhiteSpace(trap?.OnStepped?.Name))
                    trap.OnStepped = null;
            }

            foreach (var alteredStatus in dungeonInfo.AlteredStatuses)
            {
                if (alteredStatus == null) continue;
                if (string.IsNullOrWhiteSpace(alteredStatus?.OnTurnStart?.Name))
                    alteredStatus.OnTurnStart = null;
                if (string.IsNullOrWhiteSpace(alteredStatus?.OnApply?.Name))
                    alteredStatus.OnApply = null;
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
    }
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

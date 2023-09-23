using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class DungeonInfoHelpers
    {
        public static DungeonInfo CreateEmptyDungeonTemplate(LocaleInfo localeTemplate, List<string> baseLocaleLanguages)
        {
            var templateDungeon = new DungeonInfo
            {
                Locales = new(),
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
                var newLocale = localeTemplate.Clone();
                newLocale.Language = localeLanguage;

                templateDungeon.Locales.Add(newLocale);
            }

            templateDungeon.Name = "DungeonName";
            templateDungeon.Author = "Author";
            templateDungeon.WelcomeMessage = "WelcomeMessage";
            templateDungeon.EndingMessage = "EndingMessage";

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

        public static LocaleInfo Clone(this LocaleInfo localeInfo)
        {
            var newLocale = new LocaleInfo
            {
                Language = localeInfo.Language,
                LocaleStrings = new()
            };
            foreach (var localeEntry in localeInfo.LocaleStrings)
            {
                newLocale.LocaleStrings.Add(new LocaleInfoString
                {
                    Key = localeEntry.Key,
                    Value = localeEntry.Value
                });
            }
            return newLocale;
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
                OnFloorStartActions = new()
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

        public static ClassInfo CreatePlayerClassTemplate()
        {
            return new ClassInfo
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
                BaseHP = 1,
                MaxHPIncreasePerLevel = 0,
                BaseAttack = 0,
                AttackIncreasePerLevel = 0,
                BaseDefense = 0,
                DefenseIncreasePerLevel = 0,
                BaseMovement = 1,
                MovementIncreasePerLevel = 0,
                BaseHPRegeneration = 0,
                HPRegenerationIncreasePerLevel = 0,
                BaseSightRange = "FullRoom",
                StartingWeapon = "",
                StartingArmor = "",
                InventorySize = 0,
                StartingInventory = new(),
                CanGainExperience = true,
                ExperienceToLevelUpFormula = "",
                MaxLevel = 2,
                OnTurnStartActions = new(),
                OnAttackActions = new(),
                OnAttackedActions = new(),
                OnDeathActions = new()
            };
        }
        public static ClassInfo CreateNPCTemplate()
        {
            return new ClassInfo
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
                BaseHP = 1,
                MaxHPIncreasePerLevel = 0,
                BaseAttack = 0,
                AttackIncreasePerLevel = 0,
                BaseDefense = 0,
                DefenseIncreasePerLevel = 0,
                BaseMovement = 1,
                MovementIncreasePerLevel = 0,
                BaseHPRegeneration = 0,
                HPRegenerationIncreasePerLevel = 0,
                BaseSightRange = "FullRoom",
                StartingWeapon = "",
                StartingArmor = "",
                InventorySize = 0,
                StartingInventory = new(),
                CanGainExperience = true,
                ExperienceToLevelUpFormula = "",
                MaxLevel = 2,
                OnTurnStartActions = new(),
                OnAttackActions = new(),
                OnAttackedActions = new(),
                OnDeathActions = new(),
                AIOddsToUseActionsOnSelf = 50
            };
        }
        public static ClassInfo CreateItemTemplate()
        {
            return new ClassInfo
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
                OnItemSteppedActions = new(),
                OnTurnStartActions = new(),
                OnAttackActions = new(),
                OnAttackedActions = new(),
                OnItemUseActions = new(),
            };
        }

        public static ClassInfo CreateTrapTemplate()
        {
            return new ClassInfo
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
                OnItemSteppedActions = new(),
            };
        }

        public static ClassInfo CreateAlteredStatusTemplate()
        {
            return new ClassInfo
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
                OnStatusApplyActions = new(),
                OnTurnStartActions = new(),
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

        public static FloorInfo Clone(this FloorInfo info)
        {
            if (info == null) return null;

            var clonedFloor = new FloorInfo();

            clonedFloor.MinFloorLevel = info.MinFloorLevel;
            clonedFloor.MaxFloorLevel = info.MaxFloorLevel;
            clonedFloor.Width = info.Width;
            clonedFloor.Height = info.Height;
            clonedFloor.GenerateStairsOnStart = info.GenerateStairsOnStart;
            clonedFloor.PossibleMonsters = new List<ClassInFloorInfo>(info.PossibleMonsters);
            clonedFloor.PossibleItems = new List<ClassInFloorInfo>(info.PossibleItems);
            clonedFloor.PossibleTraps = new List<ClassInFloorInfo>(info.PossibleTraps);
            clonedFloor.PossibleGeneratorAlgorithms = new List<GeneratorAlgorithmInfo>(info.PossibleGeneratorAlgorithms);
            clonedFloor.MaxConnectionsBetweenRooms = info.MaxConnectionsBetweenRooms;
            clonedFloor.OddsForExtraConnections = info.OddsForExtraConnections;
            clonedFloor.RoomFusionOdds = info.RoomFusionOdds;
            clonedFloor.OnFloorStartActions = new List<ActionWithEffectsInfo>();
            foreach (var action in info.OnFloorStartActions)
            {
                clonedFloor.OnFloorStartActions.Add(action.Clone());
            }

            return clonedFloor;
        }

        public static ActionWithEffectsInfo Clone(this ActionWithEffectsInfo info)
        {
            if (info == null) return null;

            var clonedAction = new ActionWithEffectsInfo();

            clonedAction.Name = info.Name;
            clonedAction.Description = info.Description;
            clonedAction.CooldownBetweenUses = info.CooldownBetweenUses;
            clonedAction.StartingCooldown = info.StartingCooldown;
            clonedAction.MinimumRange = info.MinimumRange;
            clonedAction.MaximumRange = info.MaximumRange;
            clonedAction.MaximumUses = info.MaximumUses;
            clonedAction.TargetTypes = new List<string>(info.TargetTypes ?? new List<string>());
            clonedAction.Effect = info.Effect.Clone();

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
    }
}

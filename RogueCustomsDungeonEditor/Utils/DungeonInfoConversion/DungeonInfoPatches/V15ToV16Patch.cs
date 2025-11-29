using System;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V15ToV16Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            UpdateLocaleInfos(root);
            UpdateTileTypeInfos(root);
            UpdateFloorInfos(root);
            UpdateElementInfos(root);
            UpdateActionSchools(root);
            UpdateLootTables(root);
            UpdateCurrencyInfo(root);
            UpdateNPCModifierInfos(root);
            UpdateAffixInfos(root);
            UpdateQualityLevelInfos(root);
            UpdateItemSlotInfos(root);
            UpdateItemTypeInfos(root);
            UpdateItems(root);
            UpdatePlayerClasses(root);
            UpdateNPCs(root);
            UpdateTraps(root);
            UpdateAlteredStatuses(root);
            UpdateScripts(root);

            root["Version"] = "1.6";
        }

        private static void UpdateLocaleInfos(JsonObject root)
        {
            if (root["Locales"] is not JsonArray locales) return;
            foreach (var locale in locales.OfType<JsonObject>())
            {
                UpdateLocaleStrings(locale);
            }
        }

        private static void UpdateLocaleStrings(JsonObject root)
        {
            if (root["LocaleStrings"] is not JsonArray localeStrings) return;

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("SchoolNameNormal", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "SchoolNameNormal", ["Value"] = "Normal" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("CurrencyName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "CurrencyName", ["Value"] = "Gold" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("CurrencyDescription", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "CurrencyDescription", ["Value"] = "Sparkling coins used as trade money" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("ItemSlotWeaponName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "ItemSlotWeaponName", ["Value"] = "Weapon" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("ItemSlotArmorName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "ItemSlotArmorName", ["Value"] = "Armor" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("ItemTypeWeaponName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "ItemTypeWeaponName", ["Value"] = "Weapon" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("ItemTypeArmorName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "ItemTypeArmorName", ["Value"] = "Armor" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("ItemTypeConsumableName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "ItemTypeConsumableName", ["Value"] = "Consumable" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("ItemTypeCharmName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "ItemTypeCharmName", ["Value"] = "Charm" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("MeleeWeaponAttackName", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "MeleeWeaponAttackName", ["Value"] = "Melee Attack" });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("DefaultAttackDescription", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "DefaultAttackDescription", ["Value"] = "Deals damage with no secondary effect." });
            }

            if (!localeStrings.OfType<JsonObject>().Any(p => p["Key"]?.ToString().Equals("DefaultMeleeAttackText", StringComparison.OrdinalIgnoreCase) == true))
            {
                localeStrings.Add(new JsonObject { ["Key"] = "DefaultMeleeAttackText", ["Value"] = "{source} tries to hit {target}." });
            }
        }

        private static void UpdateTileTypeInfos(JsonObject root)
        {
            if (root["TileTypeInfos"] is not JsonArray tileTypeInfos) return;
            foreach (var tileType in tileTypeInfos.OfType<JsonObject>())
            {
                tileType["CausesPartialInvisibility"] = false;
                if (tileType["OnStood"] is JsonObject onStood)
                {
                    UpdateAction(onStood);
                }
            }
        }

        private static void UpdateFloorInfos(JsonObject root)
        {
            if (root["FloorInfos"] is not JsonArray floorInfos) return;
            foreach (var floorGroup in floorInfos.OfType<JsonObject>())
            {
                foreach (var possibleLayout in floorGroup["PossibleLayouts"]?.AsArray() ?? new JsonArray())
                {
                    if (possibleLayout is JsonObject layoutObj && possibleLayout["ProceduralGenerator"] is null && possibleLayout["StaticGenerator"] is null)
                    {
                        layoutObj["ProceduralGenerator"] = new JsonObject();
                        layoutObj["ProceduralGenerator"]["Rows"] = layoutObj["Rows"]?.DeepClone();
                        layoutObj["ProceduralGenerator"]["Columns"] = layoutObj["Columns"]?.DeepClone();
                        layoutObj["ProceduralGenerator"]["RoomDisposition"] = layoutObj["RoomDisposition"]?.DeepClone();
                        layoutObj["ProceduralGenerator"]["MinRoomSize"] = new JsonObject();
                        layoutObj["ProceduralGenerator"]["MinRoomSize"]["Width"] = layoutObj["MinRoomSize"]?["Width"]?.DeepClone();
                        layoutObj["ProceduralGenerator"]["MinRoomSize"]["Height"] = layoutObj["MinRoomSize"]?["Height"]?.DeepClone();
                        layoutObj["ProceduralGenerator"]["MaxRoomSize"] = new JsonObject();
                        layoutObj["ProceduralGenerator"]["MaxRoomSize"]["Width"] = layoutObj["MaxRoomSize"]?["Width"]?.DeepClone();
                        layoutObj["ProceduralGenerator"]["MaxRoomSize"]["Height"] = layoutObj["MaxRoomSize"]?["Height"]?.DeepClone();
                        layoutObj.Remove("Rows");
                        layoutObj.Remove("Columns");
                        layoutObj.Remove("RoomDisposition");
                        layoutObj.Remove("MinRoomSize");
                        layoutObj.Remove("MaxRoomSize");
                    }
                }
                if (floorGroup["OnFloorStart"] is JsonObject onFloorStart)
                {
                    UpdateAction(onFloorStart);
                }
                foreach (var npc in floorGroup["PossibleMonsters"]?.AsArray() ?? new JsonArray())
                {
                    if (npc is JsonObject npcObj)
                    {
                        npcObj["MinLevel"] = 1;
                        npcObj["MaxLevel"] = 1;
                    }
                }
                foreach (var item in floorGroup["PossibleItems"]?.AsArray() ?? new JsonArray())
                {
                    if (item is JsonObject itemObj)
                    {
                        itemObj["MinLevel"] = 1;
                        itemObj["MaxLevel"] = 1;
                    }
                }
                foreach (var trap in floorGroup["PossibleTraps"]?.AsArray() ?? new JsonArray())
                {
                    if (trap is JsonObject trapObj)
                    {
                        trapObj["MinLevel"] = 1;
                        trapObj["MaxLevel"] = 1;
                    }
                }
            }
        }

        private static void UpdateElementInfos(JsonObject root)
        {
            if (root["ElementInfos"] is not JsonArray elementInfos) return;
            foreach (var element in elementInfos.OfType<JsonObject>())
            {
                if (element["OnAfterAttack"] is JsonObject onAfterAttack)
                {
                    UpdateAction(onAfterAttack);
                }
            }
        }

        private static void UpdateActionSchools(JsonObject root)
        {
            if (root["ActionSchoolInfos"] is JsonArray) return;
            var actionSchoolInfos = new JsonArray
            {
                new JsonObject
                {
                    ["Id"] = "Default",
                    ["Name"] = "SchoolNameDefault"
                }
            };
            root["ActionSchoolInfos"] = actionSchoolInfos;
        }

        private static void UpdateLootTables(JsonObject root)
        {
            if (root["LootTableInfos"] is JsonArray) return;
            var lootTableInfos = new JsonArray
            {
                new JsonObject
                {
                    ["Id"] = "Default",
                    ["Entries"] = new JsonArray
                    {
                        new JsonObject
                        {
                            ["PickId"] = "No Drop",
                            ["Weight"] = 100
                        }
                    },
                    ["OverridesQualityLevelOddsOfItems"] = false,
                    ["QualityLevelOdds"] = new JsonArray()
                    {
                        new JsonObject
                        {
                            ["Id"] = "Normal",
                            ["ChanceToPick"] = 100
                        }
                    }
                }
            };
            root["LootTableInfos"] = lootTableInfos;
        }

        private static void UpdateCurrencyInfo(JsonObject root)
        {
            if (root["CurrencyInfo"] is JsonObject) return;
            var currencyInfo = new JsonObject
            {
                ["Name"] = "CurrencyName",
                ["Description"] = "CurrencyDescription",
                ["ConsoleRepresentation"] = JsonSerializer.SerializeToNode(new ConsoleRepresentation()
                {
                    Character = '$',
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Yellow)
                }),
                ["CurrencyPiles"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["Id"] = "Normal",
                        ["Minimum"] = 1,
                        ["Maximum"] = 1
                    }
                }
            };
            root["CurrencyInfo"] = currencyInfo;
        }

        private static void UpdateNPCModifierInfos(JsonObject root)
        {
            if (root["NPCModifierInfos"] is JsonArray) return;
            root["NPCModifierInfos"] = new JsonArray();
        }
        private static void UpdateAffixInfos(JsonObject root)
        {
            if (root["AffixInfos"] is JsonArray) return;
            root["AffixInfos"] = new JsonArray();
        }
        private static void UpdateQualityLevelInfos(JsonObject root)
        {
            if (root["QualityLevelInfos"] is JsonArray) return;
            var qualityLevelInfos = new JsonArray
            {
                new JsonObject
                {
                    ["Id"] = "Normal",
                    ["Name"] = "QualityLevelNormal",
                    ["MinimumAffixes"] = 0,
                    ["MaximumAffixes"] = 0,
                    ["AttachesWhatToItemName"] = "None",
                    ["ItemNameColor"] = JsonSerializer.SerializeToNode(new GameColor(Color.White))
                }
            };
            root["QualityLevelInfos"] = qualityLevelInfos;
        }

        private static void UpdatePlayerClasses(JsonObject root)
        {
            if (root["PlayerClasses"] is not JsonArray playerClasses) return;
            foreach (var playerClass in playerClasses.OfType<JsonObject>())
            {
                foreach (var action in playerClass["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                if (playerClass["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (playerClass["OnDeath"] is JsonObject onDeath)
                {
                    RemoveGiveExperienceFromOnDeath(onDeath);
                    UpdateAction(onDeath);
                }
                if (playerClass["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (playerClass["OnLevelUp"] is JsonObject onLevelUp)
                {
                    UpdateAction(onLevelUp);
                }
                if (playerClass["DefaultOnAttack"] is null)
                {
                    playerClass["DefaultOnAttack"] = JsonSerializer.SerializeToNode(DungeonInfoHelpers.CreateDefaultAttackTemplate());
                }
                if (playerClass["AvailableSlots"] is null)
                {
                    playerClass["AvailableSlots"] = new JsonArray("Weapon", "Armor");
                }
                if (playerClass["InitialEquipment"] is null)
                {
                    playerClass["InitialEquipment"] = new JsonArray();
                    if (playerClass["InitialEquippedWeapon"] != null)
                        playerClass["InitialEquipment"].AsArray().Add(playerClass["InitialEquippedWeapon"].DeepClone());
                    if (playerClass["InitialEquippedArmor"] != null)
                        playerClass["InitialEquipment"].AsArray().Add(playerClass["InitialEquippedArmor"].DeepClone());
                }
                playerClass["StartingWeapon"] = null;
                playerClass["StartingArmor"] = null;
                playerClass["InitialEquippedWeapon"] = null;
                playerClass["InitialEquippedArmor"] = null;
                if (playerClass["SaleValuePercentage"] is null)
                    playerClass["SaleValuePercentage"] = 50;
                if (playerClass["NeedsToIdentifyItems"] is null)
                    playerClass["NeedsToIdentifyItems"] = false;
            }
        }

        private static void UpdateNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;
            foreach (var npc in npcs.OfType<JsonObject>())
            {
                if (npc["OnSpawn"] is JsonObject onSpawn)
                {
                    npc["OnSpawn"]["Id"] = "Spawn";
                    npc["OnSpawn"]["Name"] = "Spawn";
                    UpdateAction(onSpawn);
                }
                foreach (var action in npc["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                foreach (var action in npc["OnInteracted"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                if (npc["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (npc["OnDeath"] is JsonObject onDeath)
                {
                    RemoveGiveExperienceFromOnDeath(onDeath);
                    UpdateAction(onDeath);
                }
                if (npc["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (npc["OnLevelUp"] is JsonObject onLevelUp)
                {
                    UpdateAction(onLevelUp);
                }
                if (npc["DefaultOnAttack"] is null)
                {
                    npc["DefaultOnAttack"] = JsonSerializer.SerializeToNode(DungeonInfoHelpers.CreateDefaultAttackTemplate());
                }
                if (npc["AvailableSlots"] is null)
                {
                    npc["AvailableSlots"] = new JsonArray("Weapon", "Armor");
                }
                if (npc["InitialEquipment"] is null)
                {
                    npc["InitialEquipment"] = new JsonArray(npc["StartingWeapon"].DeepClone(), npc["StartingArmor"].DeepClone());
                }
                npc["StartingWeapon"] = null;
                npc["StartingArmor"] = null;
                if (npc["DropsEquipmentOnDeath"] is null)
                    npc["DropsEquipmentOnDeath"] = false;
                if (npc["ReappearsOnTheNextFloorIfAlliedToThePlayer"] is null)
                    npc["ReappearsOnTheNextFloorIfAlliedToThePlayer"] = false;
                if (npc["RegularLootTable"] is null)
                {
                    npc["RegularLootTable"] = new JsonObject
                    {
                        ["LootTableId"] = npc["LootTableId"]?.DeepClone() ?? "None",
                        ["DropPicks"] = npc["DropPicks"]?.DeepClone() ?? 0
                    };
                }
                if (npc["LootTableWithModifiers"] is null)
                {
                    npc["LootTableWithModifiers"] = new JsonObject
                    {
                        ["LootTableId"] = npc["LootTableId"]?.DeepClone() ?? "None",
                        ["DropPicks"] = npc["DropPicks"]?.DeepClone() ?? 0
                    };
                }
                npc.Remove("LootTableId");
                npc.Remove("DropPicks");

                if (npc["ModifierData"] is null)
                {
                    npc["ModifierData"] = new JsonArray {
                        new JsonObject
                        {
                            ["Level"] = 1,
                            ["ModifierAmount"] = 0
                        }
                    };
                }
                if (npc["OddsForModifier"] is null)
                    npc["OddsForModifier"] = 0;
                if (npc["RandomizesForecolorIfWithModifiers"] is null)
                    npc["RandomizesForecolorIfWithModifiers"] = false;
                if (npc["ExperienceYieldMultiplierIfWithModifiers"] is null)
                    npc["ExperienceYieldMultiplierIfWithModifiers"] = 1;
                if (npc["BaseHPMultiplierIfWithModifiers"] is null)
                    npc["BaseHPMultiplierIfWithModifiers"] = 1;
            }
        }

        private static void UpdateItemTypeInfos(JsonObject root)
        {
            if (root["ItemTypeInfos"] is JsonArray) return;
            var itemTypeInfos = new JsonArray
            {
                new JsonObject
                {
                    ["Id"] = "Weapon",
                    ["Name"] = "ItemTypeWeaponName",
                    ["Usability"] = (int) ItemUsability.Equip,
                    ["PowerType"] = (int) ItemPowerType.Damage,
                    ["Slot1"] = "Weapon",
                    ["Slot2"] = "",
                    ["MinimumQualityLevelForUnidentified"] = "",
                    ["UnidentifiedItemName"] = "???",
                    ["UnidentifiedItemDescription"] = "???",
                    ["UnidentifiedItemActionName"] = "???",
                    ["UnidentifiedItemActionDescription"] = "???"
                },
                new JsonObject
                {
                    ["Id"] = "Armor",
                    ["Name"] = "ItemTypeArmorName",
                    ["Usability"] = (int) ItemUsability.Equip,
                    ["PowerType"] = (int) ItemPowerType.Mitigation,
                    ["Slot1"] = "Armor",
                    ["Slot2"] = "",
                    ["MinimumQualityLevelForUnidentified"] = "",
                    ["UnidentifiedItemName"] = "???",
                    ["UnidentifiedItemDescription"] = "???",
                    ["UnidentifiedItemActionName"] = "???",
                    ["UnidentifiedItemActionDescription"] = "???"
                },
                new JsonObject
                {
                    ["Id"] = "Consumable",
                    ["Name"] = "ItemTypeConsumableName",
                    ["Usability"] = (int) ItemUsability.Use,
                    ["PowerType"] = (int) ItemPowerType.UsePower,
                    ["Slot1"] = "",
                    ["Slot2"] = "",
                    ["MinimumQualityLevelForUnidentified"] = "",
                    ["UnidentifiedItemName"] = "???",
                    ["UnidentifiedItemDescription"] = "???",
                    ["UnidentifiedItemActionName"] = "???",
                    ["UnidentifiedItemActionDescription"] = "???"
                },
                new JsonObject
                {
                    ["Id"] = "Charm",
                    ["Name"] = "ItemTypeCharmName",
                    ["Usability"] = (int) ItemUsability.Nothing,
                    ["PowerType"] = (int) ItemPowerType.UsePower,
                    ["Slot1"] = "",
                    ["Slot2"] = "",
                    ["MinimumQualityLevelForUnidentified"] = "",
                    ["UnidentifiedItemName"] = "???",
                    ["UnidentifiedItemDescription"] = "???",
                    ["UnidentifiedItemActionName"] = "???",
                    ["UnidentifiedItemActionDescription"] = "???"
                }
            };
            root["ItemTypeInfos"] = itemTypeInfos;
        }

        private static void UpdateItemSlotInfos(JsonObject root)
        {
            if (root["ItemSlotInfos"] is JsonArray) return;
            var itemSlotInfos = new JsonArray
            {
                new JsonObject
                {
                    ["Id"] = "Weapon",
                    ["Name"] = "ItemSlotWeaponName"
                },
                new JsonObject
                {
                    ["Id"] = "Armor",
                    ["Name"] = "ItemSlotArmorName"
                }
            };
            root["ItemSlotInfos"] = itemSlotInfos;
        }

        private static void UpdateItems(JsonObject root)
        {
            if (root["Items"] is not JsonArray items) return;
            foreach (var item in items.OfType<JsonObject>())
            {
                if (item["ItemType"] is null)
                    item["ItemType"] = item["EntityType"]?.GetValue<string>();
                item.Remove("EntityType");
                foreach (var action in item["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                if (item["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (item["OnDeath"] is JsonObject onDeath)
                {
                    UpdateAction(onDeath);
                }
                if (item["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (item["OnUse"] is JsonObject onUse)
                {
                    UpdateAction(onUse);
                }
                if (item["BaseValue"] is null)
                    item["BaseValue"] = 0;
                if (item["MinimumQualityLevel"] is null)
                    item["MinimumQualityLevel"] = "Normal";
                if (item["MaximumQualityLevel"] is null)
                    item["MaximumQualityLevel"] = "Normal";
                if (item["QualityLevelOdds"] is null)
                {
                    item["QualityLevelOdds"] = new JsonArray()
                    {
                        new JsonObject
                        {
                            ["Id"] = "Normal",
                            ["ChanceToPick"] = 100
                        }
                    };
                }
            }
        }

        private static void UpdateTraps(JsonObject root)
        {
            if (root["Traps"] is not JsonArray traps) return;
            foreach (var trap in traps.OfType<JsonObject>())
            {
                if (trap["OnStepped"] is JsonObject onStepped)
                {
                    UpdateAction(onStepped);
                }
            }
        }

        private static void UpdateAlteredStatuses(JsonObject root)
        {
            if (root["AlteredStatuses"] is not JsonArray alteredStatuses) return;
            foreach (var alteredStatus in alteredStatuses.OfType<JsonObject>())
            {
                if (alteredStatus["BeforeAttack"] is JsonObject beforeAttack)
                {
                    UpdateAction(beforeAttack);
                }

                if (alteredStatus["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }

                if (alteredStatus["OnApply"] is JsonObject onApply)
                {
                    UpdateAction(onApply);
                }

                if (alteredStatus["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }

                if (alteredStatus["OnRemove"] is JsonObject onRemove)
                {
                    UpdateAction(onRemove);
                }
            }
        }

        private static void UpdateScripts(JsonObject root)
        {
            if (root["Scripts"] is not JsonArray scripts) return;
            foreach (var script in scripts.OfType<JsonObject>())
            {
                UpdateAction(script);
            }
        }

        public static void UpdateAction(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Name"].GetValue<string>()?.Length == 0)
                actionWithEffects["Name"] = actionWithEffects["Id"].ToString();
            actionWithEffects["School"] = "Default";
            UpdateActionSteps(actionWithEffects);
        }

        public static void UpdateActionSteps(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                UpdateApplyAlteredStatusStepsToV16(effect);
                UpdateApplyStatAlterationStepsToV16(effect);
                UpdateTransformTileStepsToV16(effect);
                UpdateSpawnNPCStepsToV16(effect);
                UpdateCleanseStatAlterationStepsToV16(effect);
                UpdateCleanseStatAlterationsStepsToV16(effect);
                UpdateGiveItemStepsToV16(effect);
                UpdateTeleportStepsToV16(effect);
            }
        }

        private static void RemoveGiveExperienceFromOnDeath(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                RemoveGiveExperience(null, effect, actionWithEffects, "Effect");
            }
        }

        private static void RemoveGiveExperience(JsonObject? parentEffect, JsonObject effect, JsonObject parentContainer, string effectKey)
        {
            if (effect["EffectName"]?.ToString() == "GiveExperience")
            {
                if (effect[effectKey] is JsonObject childEffect)
                {
                    parentContainer[effectKey] = childEffect.DeepClone();
                    RemoveGiveExperience(parentEffect, childEffect, parentContainer, effectKey);
                }
                else
                {
                    parentContainer[effectKey] = null;
                }
                return;
            }

            if (effect["Then"] is JsonObject thenObj)
                RemoveGiveExperience(effect, thenObj, effect, "Then");
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                RemoveGiveExperience(effect, onSuccessObj, effect, "OnSuccess");
            if (effect["OnFailure"] is JsonObject onFailureObj)
                RemoveGiveExperience(effect, onFailureObj, effect, "OnFailure");
        }

        private static void UpdateApplyAlteredStatusStepsToV16(JsonObject effect)
        {
            UpdateApplyAlteredStatusParametersToV16(effect);
        }

        private static void UpdateApplyAlteredStatusParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "ApplyAlteredStatus")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("AnnounceStatusRefresh", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "AnnounceStatusRefresh", ["Value"] = "True" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateApplyAlteredStatusParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateApplyAlteredStatusParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateApplyAlteredStatusParametersToV16(onFailureObj);
        }

        private static void UpdateApplyStatAlterationStepsToV16(JsonObject effect)
        {
            UpdateApplyStatAlterationParametersToV16(effect);
        }

        private static void UpdateApplyStatAlterationParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "ApplyStatAlteration")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("CanBeOverwritten", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "CanBeOverwritten", ["Value"] = "False" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateApplyStatAlterationParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateApplyStatAlterationParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateApplyStatAlterationParametersToV16(onFailureObj);
        }
        private static void UpdateTransformTileStepsToV16(JsonObject effect)
        {
            UpdateTransformTileParametersToV16(effect);
        }

        private static void UpdateTransformTileParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "TransformTile")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("TurnLength", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "TurnLength", ["Value"] = "-1" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateTransformTileParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateTransformTileParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateTransformTileParametersToV16(onFailureObj);
        }

        private static void UpdateSpawnNPCStepsToV16(JsonObject effect)
        {
            UpdateSpawnNPCParametersToV16(effect);
        }

        private static void UpdateSpawnNPCParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "SpawnNPC")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("InheritsSpawnerColour", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "InheritsSpawnerColour", ["Value"] = "false" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateSpawnNPCParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateSpawnNPCParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateSpawnNPCParametersToV16(onFailureObj);
        }

        private static void UpdateCleanseStatAlterationStepsToV16(JsonObject effect)
        {
            UpdateCleanseStatAlterationParametersToV16(effect);
        }

        private static void UpdateCleanseStatAlterationParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "CleanseStatAlteration")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("ClearsBuffs", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "ClearsBuffs", ["Value"] = "True" });
                }
                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("ClearsDebuffs", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "ClearsDebuffs", ["Value"] = "True" });
                }
                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("ClearsFromStatuses", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "ClearsFromStatuses", ["Value"] = "True" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateCleanseStatAlterationParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateCleanseStatAlterationParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateCleanseStatAlterationParametersToV16(onFailureObj);
        }

        private static void UpdateCleanseStatAlterationsStepsToV16(JsonObject effect)
        {
            UpdateCleanseStatAlterationsParametersToV16(effect);
        }

        private static void UpdateCleanseStatAlterationsParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "CleanseStatAlterations")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("ClearsBuffs", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "ClearsBuffs", ["Value"] = "True" });
                }
                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("ClearsDebuffs", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "ClearsDebuffs", ["Value"] = "True" });
                }
                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("ClearsFromStatuses", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "ClearsFromStatuses", ["Value"] = "True" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateCleanseStatAlterationsParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateCleanseStatAlterationsParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateCleanseStatAlterationsParametersToV16(onFailureObj);
        }

        private static void UpdateGiveItemStepsToV16(JsonObject effect)
        {
            UpdateGiveItemParametersToV16(effect);
        }

        private static void UpdateGiveItemParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "GiveItem")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("InformOfSource", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "InformOfSource", ["Value"] = "True" });
                }

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("InformThePlayer", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "InformThePlayer", ["Value"] = "True" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateGiveItemParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateGiveItemParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateGiveItemParametersToV16(onFailureObj);
        }

        private static void UpdateTeleportStepsToV16(JsonObject effect)
        {
            UpdateTeleportParametersToV16(effect);
        }

        private static void UpdateTeleportParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "Teleport")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("TargetTile", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "TargetTile", ["Value"] = "AnyTile" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateTeleportParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateTeleportParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateTeleportParametersToV16(onFailureObj);
        }
    }
}

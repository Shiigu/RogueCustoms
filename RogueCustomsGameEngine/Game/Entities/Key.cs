using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.Entities
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class Key : Entity, IHasActions, IPickable
    {
        public Character Owner { get; set; }

        public Key(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            Owner = null;
            MapClassActions(entityClass.OnAttack, OwnOnAttack);
        }

        public void RefreshCooldownsAndUpdateTurnLength()
        {
            // Keys don't have any cooldowns
        }

        public void PerformOnTurnStart()
        {
            // Keys don't have an OnTurnStart
        }

        public static ActionWithEffectsInfo GetOpenDoorActionForKey(string keyName)
        {
            return new ActionWithEffectsInfo()
            {
                CooldownBetweenUses = 0,
                Description = "UnlockDoorDescription",
                FinishesTurnWhenUsed = false,
                MinimumRange = 1,
                MaximumRange = 1,
                MaximumUses = 0,
                MPCost = 0,
                Id = $"KeyType{keyName}",
                Name = $"KeyType{keyName}",
                StartingCooldown = 0,
                TargetTypes = new() { "Tile" },
                UseCondition = $"{{target.Type}} == \"Door\" && {{target.DoorId}} == \"{keyName}\"",
                Effect = new()
                {
                    EffectName = "PrintText",
                    Params = new[]
                    {
                        new Parameter
                        {
                            ParamName = "Text",
                            Value = "ObjectUsedText"
                        },
                        new Parameter
                        {
                            ParamName = "Color",
                            Value = "255,255,255,255"
                        },
                        new Parameter
                        {
                            ParamName = "BypassesVisibilityCheck",
                            Value = "False"
                        }
                    },
                    Then = new()
                    {
                        EffectName = "UnlockDoor",
                        Params = new[]
                        {
                            new Parameter
                            {
                                ParamName = "Target",
                                Value = "target"
                            },
                            new Parameter
                            {
                                ParamName = "DoorId",
                                Value = keyName
                            },
                            new Parameter
                            {
                                ParamName = "Accuracy",
                                Value = "100"
                            },
                            new Parameter
                            {
                                ParamName = "BypassesAccuracyCheck",
                                Value = "True"
                            }
                        },
                        OnSuccess = new()
                        {
                            EffectName = "CheckCondition",
                            Params = new[]
                            {
                                new Parameter
                                {
                                    ParamName = "Condition",
                                    Value = $"[Doors_{keyName}] < 1"
                                }
                            },
                            OnSuccess = new()
                            {
                                EffectName = "Remove",
                                Params = new[]
                                {
                                    new Parameter
                                    {
                                        ParamName = "Target",
                                        Value = "this"
                                    },
                                    new Parameter
                                    {
                                        ParamName = "Accuracy",
                                        Value = "100"
                                    },
                                    new Parameter
                                    {
                                        ParamName = "BypassesAccuracyCheck",
                                        Value = "True"
                                    }
                                },
                                OnSuccess = new()
                                {
                                    EffectName = "PrintText",
                                    Params = new[]
                                    {
                                        new Parameter
                                        {
                                            ParamName = "Text",
                                            Value = "ObjectDisappearedText"
                                        },
                                        new Parameter
                                        {
                                            ParamName = "Color",
                                            Value = "255,255,255,255"
                                        },
                                        new Parameter
                                        {
                                            ParamName = "BypassesVisibilityCheck",
                                            Value = "False"
                                        }
                                    }
                                }
                            }
                        },
                        OnFailure = new()
                        {
                            EffectName = "PrintText",
                            Params = new[]
                            {
                                new Parameter
                                {
                                    ParamName = "Text",
                                    Value = "FailedAttackText"
                                },
                                new Parameter
                                {
                                    ParamName = "Color",
                                    Value = "255,255,255,255"
                                },
                                new Parameter
                                {
                                    ParamName = "BypassesVisibilityCheck",
                                    Value = "False"
                                }
                            }
                        }
                    }
                }
            };
        }

        public override void SetActionIds()
        {
            for (int i = 0; i < OwnOnAttack.Count; i++)
            {
                OwnOnAttack[i].SelectionId = $"{Id}_{ClassId}_CA{i}_{OwnOnAttack[i].Id}";
                if (OwnOnAttack[i].IsScript)
                    OwnOnAttack[i].SelectionId += "_S";
            }
        }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

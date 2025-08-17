using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
namespace RogueCustomsGameEngine.Game.Entities
{
    [Serializable]
    public class Trap : Entity, IHasActions
    {
        public Faction Faction { get; set; }                // Exclusively used to allow Traps to be visible only to certain characters.
        public ActionWithEffects OnStepped { get; set; }
        public string Power { get; set; }

        public Trap(EntityClass entityClass, Map map) : base(entityClass, map)
        {
            Power = entityClass.Power;
            OnStepped = MapClassAction(entityClass.OnStepped);
        }

        public async Task Stepped(Character stomper)
        {
            if (OnStepped == null) return;
            if (stomper == Map.Player || Map.Player.CanSee(stomper))
            {
                Map.DisplayEvents.Add(($"{stomper.Name} stepped on trap", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.TrapActivate }
                    }
                }
                ));
            }
            var successfulEffects = await OnStepped.Do(this, stomper, true);
            if (successfulEffects != null)
            {
                stomper.Visible = true;
                if(!Map.IsDebugMode)
                {
                    Map.DisplayEvents.Add(($"{stomper.Name} stepped on trap", new()
                    {
                        new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { stomper.Position, Map.GetConsoleRepresentationForCoordinates(stomper.Position.X, stomper.Position.Y) }
                        }
                    }));
                }
            }
            if (successfulEffects != null && EngineConstants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                await stomper.AttackedBy(null);
        }

        public Task RefreshCooldownsAndUpdateTurnLength()
        {
            if (OnStepped?.CooldownBetweenUses > 0 && OnStepped?.CurrentCooldown > 0)
                OnStepped.CurrentCooldown--;
            return Task.CompletedTask;
        }

        public Task PerformOnTurnStart()
        {
            // Traps don't have any OnTurnStart
            return Task.CompletedTask;
        }

        public bool CanBeSeenBy(Character c)
        {
            if (c == null) return false;
            if (c.CanSee(this)) return true;
            if (!Visible && Faction != null && c.FOVTiles.Any(t => t == ContainingTile))
            {
                if (Faction == c.Faction) return true;
                if (Faction.IsAlliedWith(c.Faction)) return true;
            }
            return false;
        }

        public override void SetActionIds()
        {
            // No need to set Ids
        }

    }
}
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;
using System.Security.Principal;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Drawing;
using System.Numerics;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable S2259 // Null Pointers should not be dereferenced
    #pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning disable CS8601 // Posible asignación de referencia nula
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
    #pragma warning disable CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class NonPlayableCharacter : Character, IAIControlled
    {
        private List<(Character Character, TargetType TargetType)> KnownCharacters { get; } = new List<(Character Character, TargetType TargetType)>();
        public AIType AIType { get; set; }

        private ITargetable CurrentTarget;
        private GamePoint LatestTargetPosition;

        private readonly bool KnowsAllCharacterPositions;
        private readonly bool PursuesOutOfSightCharacters;
        private readonly bool WandersIfWithoutTarget;

        private List<Room> VisitedRooms = new();

        private (GamePoint Destination, List<Tile> Route) PathToUse;
        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }
        public bool SpawnedViaMonsterHouse { get; set; }

        public NonPlayableCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
            KnownCharacters.Add((this, TargetType.Self));
            PathToUse = (null, null);
            CurrentTarget = null;
            KnowsAllCharacterPositions = entityClass.KnowsAllCharacterPositions;
            PursuesOutOfSightCharacters = entityClass.PursuesOutOfSightCharacters;
            WandersIfWithoutTarget = entityClass.WandersIfWithoutTarget;
            AIType = entityClass.AIType;

            OnSpawn = MapClassAction(entityClass.OnSpawn);
            OnInteracted = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnInteracted, OnInteracted);
        }

        public void ProcessAI()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive)
            {
                RemainingMovement = 0;
                TookAction = true;
                return;     // Dead entities don't move. Set all the possible flags to ensure they aren't seen as movable.
            }
            UpdateKnownCharacterList();
            ConsiderSwappingTargets();
            if (!ConsiderWalking())
            {
                if (ConsiderUsingActionOnSelf())
                    return;
                if (ConsiderUsingActionOnTarget())
                    return;
                if (ConsiderUsingActionOnTile())
                    return;
            }
            TryToMoveToTarget();
        }

        public void ConsiderSwappingTargets()
        {
            if (ContainingRoom != null && !VisitedRooms.Contains(ContainingRoom))
                VisitedRooms.Add(ContainingRoom);

            var formerTarget = CurrentTarget;
            var distanceToCurrentTarget = (CurrentTarget is Character c) ? GamePoint.Distance(c.Position, Position) : int.MaxValue;
            var targetsWithinSight = KnownCharacters.Where(kc => kc.Character.ExistenceStatus == EntityExistenceStatus.Alive && CanSee(kc.Character));

            // If I don't have a target or someone's closer than my current target, swap to them
            var closerCharacters = targetsWithinSight.Where(t => GamePoint.Distance(t.Character.Position, Position) < distanceToCurrentTarget && OnAttack.Any(oa => oa.TargetTypes.Contains(t.TargetType)));

            if(closerCharacters.Any())
            {
                var targetingPreferences = new List<(Character Character, int Weight)>();
                foreach (var character in closerCharacters)
                {
                    // I prefer targeting enemies
                    switch(character.TargetType)
                    {
                        case TargetType.Enemy:
                            targetingPreferences.Add((character.Character, 4));
                            break;
                        case TargetType.Ally:
                            targetingPreferences.Add((character.Character, 2));
                            break;
                        case TargetType.Neutral:
                            targetingPreferences.Add((character.Character, 1));
                            break;
                    }
                }
                CurrentTarget = targetingPreferences.TakeRandomElementWithWeights(c => c.Weight, Rng).Character;
            }

            // If I reached the target but never swapped it, forget about it and start to wander around
            if (PathToUse.Route != null && !PathToUse.Route.Any() && CurrentTarget != null && PathToUse.Destination == CurrentTarget.Position)
                CurrentTarget = null;

            // If I somehow got thrown away from the path towards the target (e.g. Teleport), recalculate it
            if (PathToUse.Route != null && PathToUse.Route.Any() && CurrentTarget != null && !Map.GetAdjacentTiles(Position, true).Contains(PathToUse.Route[0]))
                PathToUse = (Destination: CurrentTarget.Position, Route: Map.GetPathBetweenTiles(Position, CurrentTarget.Position).Skip(1).ToList());

            // If I somehow lost path towards the target but never swapped it, recalculate it
            if (PathToUse.Route != null && !PathToUse.Route.Any() && CurrentTarget != null && PathToUse.Destination != CurrentTarget.Position)
                PathToUse = (Destination: CurrentTarget.Position, Route: Map.GetPathBetweenTiles(Position, CurrentTarget.Position).Skip(1).ToList());

            // Recalculate path if the target changed, or if they changed positions
            if (CurrentTarget != null && (CurrentTarget != formerTarget || (CurrentTarget is Character ct && ct.ExistenceStatus != EntityExistenceStatus.Alive) || CurrentTarget.Position != LatestTargetPosition))
                PathToUse = (Destination: CurrentTarget.Position, Route: Map.GetPathBetweenTiles(Position, CurrentTarget.Position).Skip(1).ToList());

            LatestTargetPosition = CurrentTarget?.Position;
        }

        public bool ConsiderWalking()
        {
            if (CurrentTarget != null && CurrentTarget is Character tc)
            {
                // If I can pursue even if out of sight, I keep going as usual
                if (PursuesOutOfSightCharacters)
                    return false;

                // If I can see the target, I keep going as usual
                if (CanSee(tc))
                    return false;

                // If I can't see the target, I check if I can see their last four positions
                for (int i = tc.LatestPositions.Count - 1; i >= 0; i--)
                {
                    var positionTile = Map.GetTileFromCoordinates(tc.LatestPositions[i]);
                    // If I can see one of these last positions, I go there as they may still be in sight
                    if (FOVTiles.Contains(positionTile))
                    {
                        CurrentTarget = positionTile;
                        return true;
                    }
                }
            }
            else if (CurrentTarget is Tile tt)
            {
                // If I haven't reached the target Tile, I keep going as usual
                if (tt != ContainingTile && PathToUse.Route != null)
                    return true;
            }

            if (!WandersIfWithoutTarget)
                return false;

            // IF EITHER
            //    a)  I can't see my target nor their last four positions (they got out of sight)
            //    b)  I've reached my target Tile and I haven't seen any target
            // THEN
            //      I change my target to a random Tile in a random Room that isn't mine (preferably one I haven't seen yet)
            //      And I take a step there

            if(Movement.Current > 0)
            {
                var pickableRooms = new List<Room>();

                if (Map.Rooms.All(VisitedRooms.Contains))
                    pickableRooms = Map.Rooms.Where(r => r != ContainingRoom).ToList();
                else
                    pickableRooms = Map.Rooms.Except(VisitedRooms).Where(r => r != ContainingRoom).ToList();

                var anotherRoom = (pickableRooms.Count > 0) ? pickableRooms.TakeRandomElement(Rng) : ContainingRoom;
                var aTileInThatRoom = anotherRoom.GetTiles().Where(t => t.IsWalkable && !t.IsHarmfulFor(this)).TakeRandomElement(Rng);

                CurrentTarget = aTileInThatRoom;
                PathToUse = (Destination: CurrentTarget.Position, Route: Map.GetPathBetweenTiles(Position, CurrentTarget.Position).Skip(1).ToList());
            }
            else
            {
                CurrentTarget = ContainingTile;
                PathToUse = (Destination: CurrentTarget.Position, Route: Map.GetPathBetweenTiles(Position, CurrentTarget.Position).ToList());
            }

            return true;
        }

        public bool ConsiderUsingActionOnSelf()
        {
            // Get all the OnAttack that target Self, and all the OnItemUse in the Inventory
            var onAttackOnSelf = OnAttack.Where(oa => oa.TargetTypes.Contains(TargetType.Self)).ToList();
            var onItemUses = Inventory.Select(i => i.OnUse).Where(a => a != null).ToList();
            var possibleActionsOnSelf = onAttackOnSelf.Union(onItemUses);
            var actionsAndWeights = new List<(ActionWithEffects Action, int Weight)>();
            foreach (var action in possibleActionsOnSelf)
            {
                var weight = action.GetActionWeightFor(CurrentTarget, this);
                if(weight > 0)
                    actionsAndWeights.Add((action, weight));
            }

            // There's some chance I won't use an item at all
            actionsAndWeights.Add((null, 55));
            var pickedAction = actionsAndWeights.TakeRandomElementWithWeights(aaw => aaw.Weight, Rng);

            if (pickedAction.Action == null)
                return false;

            if (!pickedAction.Action.ChecksCondition(this, this) || !pickedAction.Action.ChecksAICondition(this, this))
                return false;

            pickedAction.Action.Do(this, this, true);
            if (pickedAction.Action.FinishesTurnWhenUsed)
                TookAction = true;

            return true;
        }

        public bool ConsiderUsingActionOnTile()
        {
            if (CurrentTarget is not Character c || !CanSee(c)) 
                return false;   // I won't do anything to a Tile if I don't have my Target on sight
            
            var possibleActionsOnTiles = OnAttack.Where(oa => oa.TargetTypes.Contains(TargetType.Tile));

            if (!possibleActionsOnTiles.Any())
                return false;

            var validDistances = new List<int>();
            foreach (var action in possibleActionsOnTiles)
            {
                for (int i = action.MinimumRange; i <= action.MaximumRange; i++)
                {
                    validDistances.Add(i);
                }
            }
            validDistances = validDistances.Distinct().ToList();

            // Get all the Tiles where I can use an Action on
            var tilesWithinMaxDistance = Map.GetFOVTilesWithinDistance(Position, validDistances.Max());

            var actionsAndWeights = new List<(ActionWithEffects Action, Tile Tile, int Weight)>();

            foreach (var tile in tilesWithinMaxDistance)
            {
                var hasActionsForTile = false;
                foreach (var action in possibleActionsOnTiles)
                {
                    if (!action.CanBeUsedOnTile(tile, this)) continue;
                    var weight = action.GetActionWeightFor(tile, this);
                    if (weight > 0)
                    {
                        actionsAndWeights.Add((action, tile, weight));
                        hasActionsForTile = true;
                    }
                }
                // Add a chance where I won't do anything on a Tile
                if(hasActionsForTile)
                    actionsAndWeights.Add((null, null, 25));
            }
            var pickedAction = actionsAndWeights.TakeRandomElementWithWeights(aaw => aaw.Weight, Rng);

            if (pickedAction.Action == null)
                return false;

            if (!pickedAction.Action.ChecksCondition(this, pickedAction.Tile) || !pickedAction.Action.ChecksAICondition(this, pickedAction.Tile))
                return false;

            pickedAction.Action.Do(this, pickedAction.Tile, true);
            if (pickedAction.Action.FinishesTurnWhenUsed)
                TookAction = true;

            return true;
        }

        public bool ConsiderUsingActionOnTarget()
        {
            var currentTargetInfo = KnownCharacters.Find(kc => kc.Character == CurrentTarget);
            if (currentTargetInfo == default)
                return false; // This ideally shouldn't happen, but in case it does...
            var distanceToTarget = GamePoint.Distance(Position, CurrentTarget.Position);
            var possibleActionsOnTarget = OnAttack.Where(oa => oa.TargetTypes.Contains(currentTargetInfo.TargetType)).ToList();
            var actionsAndWeights = new List<(ActionWithEffects Action, int Weight)>();
            foreach (var action in possibleActionsOnTarget)
            {
                if (!action.ChecksCondition(this, CurrentTarget) || !action.ChecksAICondition(this, CurrentTarget)) continue;
                var weight = action.GetActionWeightFor(CurrentTarget, this);
                if (weight > 0)
                    actionsAndWeights.Add((action, weight));
            }

            if (!actionsAndWeights.Any())
                return false;

            var pickedAction = actionsAndWeights.TakeRandomElementWithWeights(aaw => aaw.Weight, Rng);

            if (pickedAction.Action == null)
                return false;

            if (distanceToTarget > pickedAction.Action.MaximumRange)
                return false;   // I must get closer
            else if (distanceToTarget < pickedAction.Action.MinimumRange)
            {
                // I must move further
                var tilesAtMinimumRangeFromTarget = Map.GetTilesWithinDistance(CurrentTarget.Position, pickedAction.Action.MinimumRange).Where(t => t.IsWalkable && !t.IsOccupied);
                var visibleTilesAtMinimumRangeFromTarget = FOVTiles.Intersect(tilesAtMinimumRangeFromTarget);
                if (!visibleTilesAtMinimumRangeFromTarget.Any())
                    return false; // No valid tile, don't do anything
                var tilesAndWeights = new List<(Tile Tile, int Weight)>();
                foreach (var tile in visibleTilesAtMinimumRangeFromTarget)
                {
                    tilesAndWeights.Add((tile, (int) (1 / (GamePoint.Distance(Position, tile.Position) / 10)) * 100));
                }
                var pickedTile = tilesAndWeights.TakeRandomElementWithWeights(taw => taw.Weight, Rng);
                CurrentTarget = pickedTile.Tile;
                return false;
            }

            pickedAction.Action.Do(this, CurrentTarget, true);
            if (pickedAction.Action.FinishesTurnWhenUsed)
                TookAction = true;

            return true;
        }

        public void TryToMoveToTarget()
        {
            // Check if I the path I was going to use could be used
            RecalculatePathIfNeeded();
            if (PathToUse.Route == null || !PathToUse.Route.Any())
            {
                // Finish turn if I'm not going to move
                RemainingMovement = 0;
                TookAction = true;
                return;
            }
            var nextTile = PathToUse.Route[0];
            if (!nextTile.IsWalkable || nextTile.IsOccupied)
            {
                // If I can't move to the next Tile, even after recalculating, don't move
                RemainingMovement = 0;
                return;
            }
            if(ContainingTile == nextTile || RemainingMovement <= 0)
            {
                // If I'm on my target or cannot walk to it, end turn
                TookAction = true;
                return;
            }
            if (Map.TryMoveCharacter(this, nextTile))
            {
                PathToUse.Route = PathToUse.Route.Skip(1).ToList();
                if (RemainingMovement <= 0)
                    TookAction = true;
            }
        }
        
        private void RecalculatePathIfNeeded()
        {
            if (PathToUse.Route != null && PathToUse.Route.Any())
            {
                var nextTile = PathToUse.Route[0];
                // If my next Tile is impossible to step on, find another viable path from adjacent Tiles
                if (!nextTile.IsWalkable || nextTile.IsOccupied)
                {
                    var adjacentTiles = Map.GetAdjacentWalkableTiles(Position, true);
                    var pathsFromAdjacentTiles = new List<List<Tile>>();
                    foreach (var tile in adjacentTiles)
                    {
                        if (tile.IsOccupied) continue;
                        pathsFromAdjacentTiles.Add(Map.GetPathBetweenTiles(tile.Position, CurrentTarget.Position));
                    }
                    if (pathsFromAdjacentTiles.Any())
                        PathToUse.Route = pathsFromAdjacentTiles.TakeRandomElement(Rng);
                    else
                        PathToUse = default; // Forget about this route if it's impossible to reach
                }
            }
        }

        public void UpdateKnownCharacterList()
        {
            KnownCharacters.RemoveAll(kc => kc.Character.ExistenceStatus != EntityExistenceStatus.Alive);     // Don't target the dead (yet?)
            if(KnowsAllCharacterPositions)
            {
                foreach (var characterInMap in Map.GetCharacters().Where(c => c.ExistenceStatus == EntityExistenceStatus.Alive))
                {
                    if(!KnownCharacters.Select(kc => kc.Character).Contains(characterInMap))
                        KnownCharacters.Add((characterInMap, CalculateTargetTypeFor(characterInMap)));
                }
            }
            else
            {
                FOVTiles.ForEach(t =>
                {
                    if (Map.GetEntitiesFromCoordinates(t.Position).Find(e => !e.Passable && e.ExistenceStatus == EntityExistenceStatus.Alive) is Character characterInTile
                        && CanSee(characterInTile)
                        && !KnownCharacters.Select(kc => kc.Character).Contains(characterInTile))
                    {
                        KnownCharacters.Add((characterInTile, CalculateTargetTypeFor(characterInTile)));
                    }
                });
            }
        }

        public void ClearKnownCharacters()
        {
            KnownCharacters.Clear();
        }

        public void UpdateKnownCharacterRelationships()
        {
            for (var i = 0; i < KnownCharacters.Count; i++)
            {
                var character = KnownCharacters[i];
                character.TargetType = CalculateTargetTypeFor(character.Character);
            }
        }

        public override void AttackedBy(Character source)
        {
            base.AttackedBy(source);

            if (source == null) return;

            // If this character has a neutral relation with another's faction, they get flagged as an enemy if they get attacked by them

            if (CalculateTargetTypeFor(source) != TargetType.Neutral) return;
            var knownCharacter = KnownCharacters.Find(kc => kc.Character.Equals(source));
            if (knownCharacter != default)
                knownCharacter.TargetType = TargetType.Enemy;
            else
                KnownCharacters.Add((source, TargetType.Enemy));
        }

        public override void Die(Entity? attacker = null)
        {
            var events = new List<DisplayEventDto>();
            base.Die(attacker);
            if (ExistenceStatus == EntityExistenceStatus.Dead)
            {
                Inventory?.ForEach(i => DropItem(i));
                Inventory?.Clear();
            }
            if (attacker == Map.Player || Map.Player.CanSee(this))
            {
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { Position, Map.GetConsoleRepresentationForCoordinates(Position.X, Position.Y) }
                    });
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.NPCDeath }
                });
            }
            Map.DisplayEvents.Add(($"NPC {Name} dies", events));
        }

        public override void PickItem(Item item, bool informToPlayer)
        {
            Inventory.Add(item);
            item.Owner = this;
            item.Position = null;
            item.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                Map.AppendMessage(Map.Locale["NPCPickItem"].Format(new { CharacterName = Name, ItemName = item.Name }));
                Map.DisplayEvents.Add(($"NPC {Name} picks item", new()
                    {
                        new() {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.NPCItemGet }
                        }
                    }
                ));
            }
        }

        public override void DropItem(Item item)
        {
            var events = new List<DisplayEventDto>();
            Tile pickedEmptyTile = null;
            if (!ContainingTile.GetPickableObjects().Exists(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (ContainingTile.Trap == null || ContainingTile.Trap.ExistenceStatus != EntityExistenceStatus.Alive))
                pickedEmptyTile = ContainingTile;
            if(pickedEmptyTile == null)
            {
                var closeEmptyTiles = Map.Tiles.GetElementsWithinDistanceWhere(Position.Y, Position.X, 5, true, t => t.IsWalkable && !t.IsOccupied && !t.GetPickableObjects().Exists(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (t.Trap == null || t.Trap.ExistenceStatus != EntityExistenceStatus.Alive) && (t.Key == null || t.Key.ExistenceStatus != EntityExistenceStatus.Alive)).ToList();
                var closestDistance = closeEmptyTiles.Any() ? closeEmptyTiles.Min(t => GamePoint.Distance(t.Position, Position)) : -1;
                var closestEmptyTiles = closeEmptyTiles.Where(t => GamePoint.Distance(t.Position, Position) <= closestDistance);
                if (closestEmptyTiles.Any())
                {
                    pickedEmptyTile = closestEmptyTiles.TakeRandomElement(Rng);
                }
                else
                {
                    item.Position = null;
                    item.Owner = null!;
                    item.ExistenceStatus = EntityExistenceStatus.Gone;
                    Map.AppendMessage(Map.Locale["NPCItemCannotBePutOnFloor"].Format(new { ItemName = item.Name }));
                    Map.Items.Remove(item);
                }
            }
            if (pickedEmptyTile != null)
            {
                item.Position = pickedEmptyTile.Position;
                item.Owner = null!;
                item.ExistenceStatus = EntityExistenceStatus.Alive;
                Map.AppendMessage(Map.Locale["NPCPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = item.Name }));
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { item.Position, Map.GetConsoleRepresentationForCoordinates(item.Position.X, item.Position.Y) }
                    }
                    );
                }
            }
            Map.DisplayEvents.Add(($"NPC {Name} drops item", events));
        }

        public override void PickKey(Key key, bool informToPlayer)
        {
            // Nothing. NPCs are not meant to pick up Keys.
        }

        public override void SetActionIds()
        {
            base.SetActionIds();
            for (int i = 0; i < OnInteracted.Count; i++)
            {
                OnInteracted[i].SelectionId = $"{Id}_{ClassId}_CA{i}_{OnInteracted[i].Id}";
                if (OnInteracted[i].IsScript)
                    OnInteracted[i].SelectionId += "_S";
            }
        }
    }
    #pragma warning restore S2259 // Null Pointers should not be dereferenced
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
    #pragma warning restore CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

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

        // This is used to prevent continuous paying attention to themselves rather than on others
        private ActionWithEffects LastUsedActionOnSelf;
        // This is used to prevent continuous shuffling between tiles if it does not find an open path
        public GamePoint LastPosition { get; set; }
        public AIType AIType { get; set; }

        private ITargetable CurrentTarget;
        private ActionWithEffects CurrentAction;
        private readonly bool KnowsAllCharacterPositions;
        public readonly int AIOddsToUseActionsOnSelf;
        private (GamePoint Destination, List<Tile> Route) PathToUse;
        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }

        public NonPlayableCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
            KnownCharacters.Add((this, TargetType.Self));
            PathToUse = (null, null);
            CurrentTarget = null;
            LastUsedActionOnSelf = null;
            KnowsAllCharacterPositions = entityClass.KnowsAllCharacterPositions;
            AIType = entityClass.AIType;
            AIOddsToUseActionsOnSelf = entityClass.AIOddsToUseActionsOnSelf;

            OnSpawn = MapClassAction(entityClass.OnSpawn);
            OnInteracted = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnInteracted, OnInteracted);

            for (int i = 0; i < OnInteracted.Count; i++)
            {
                OnInteracted[i].ActionId = i;
            }
        }

        public void PickTargetAndPath()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;                                       // Dead entities don't move
            if (!OnAttack.Any()) return;
            UpdateKnownCharacterList();
            CurrentAction = null;
            CurrentTarget = null;
            var attackActionsWithValidTargets = LookForAttackActionsWithValidTargets().Where(a => a.PossibleTargets.Any());
            if (attackActionsWithValidTargets.Any())
            {
                PickADirectTargetIfPossible(attackActionsWithValidTargets);
            }
            if (CurrentTarget == null)
            {
                PickADistantTargetIfPossible();
            }
            // If the picked next tile is inaccessible, invisible, or contains a visible trap, and it does not contain the target, find another tile.
            if(PathToUse.Route?.Count > 1 && !TileCanBeApproached(PathToUse.Route?[1]) || PathToUse.Route?[1].Trap?.CanBeSeenBy(this) == true)
            {
                var adjacentTiles = Map.GetAdjacentWalkableTiles(Position, true);
                var visibleAdjacentTiles = FOVTiles.Intersect(adjacentTiles);
                var visibleAdjacentTilesWithoutKnownTraps = visibleAdjacentTiles.Where(t => t.Trap?.CanBeSeenBy(this) != true);
                var visibleAdjacentTilesWithKnownTraps = visibleAdjacentTiles.Except(visibleAdjacentTilesWithoutKnownTraps);

                if (GetAnApproachablePath(visibleAdjacentTilesWithoutKnownTraps, out List<Tile> pickedPath))
                    PathToUse.Route = pickedPath;
                else if (GetAnApproachablePath(visibleAdjacentTilesWithKnownTraps, out pickedPath))
                    PathToUse.Route = pickedPath;
            }

            // But if they still can't find anywhere to move, skip the turn.
            // Notice the lack of the "There's a known trap" condition, as it's meant to discourage walking into traps, but allow doing so if there's no option left.
            if (PathToUse.Route?.Count > 1 && !TileCanBeApproached(PathToUse.Route?[1]))
                RemainingMovement = 0;
        }

        private IEnumerable<(ActionWithEffects Action, List<(ITargetable Target, int Distance)> PossibleTargets)> LookForAttackActionsWithValidTargets()
        {
            foreach (var action in OnAttack)
            {
                if (action.MayBeUsed)
                {
                    if (!action.TargetTypes.Contains(TargetType.Tile))
                    {
                        var possibleTargets = KnownCharacters.Where(kc => action.TargetTypes.Contains(kc.TargetType))
                                        .Select(kc => ((ITargetable)kc.Character, Distance: (int)GamePoint.Distance(kc.Character.Position, Position)));
                        if (possibleTargets.Any())
                            yield return (action, possibleTargets.Where(kc => kc.Distance.Between(action.MinimumRange, action.MaximumRange)).ToList());
                    }
                    else
                    {
                        var possibleTiles = Map.GetFOVTilesWithinDistance(Position, action.MaximumRange)
                                        .Select(t => ((ITargetable)t, Distance: (int)GamePoint.Distance(t.Position, Position))).ToList();
                        if (!possibleTiles.Exists(t => t.Item1 == ContainingTile))
                            possibleTiles.Add((ContainingTile, 0));
                        if (possibleTiles.Any())
                            yield return (action, possibleTiles.Where(t => t.Distance.Between(action.MinimumRange, action.MaximumRange)).ToList());
                    }
                }
            }
            foreach (var itemOnUse in Inventory.Select(i => i.OnUse))
            {
                if (itemOnUse == null) continue;
                yield return (itemOnUse, new List<(ITargetable Target, int Distance)> { (this, 0) });
            }
        }

        private void PickADirectTargetIfPossible(IEnumerable<(ActionWithEffects Action, List<(ITargetable Target, int Distance)> PossibleTargets)> attackActionsWithValidTargets)
        {
            List<(ActionWithEffects Action, ITargetable Target, int Weight)> weightedActions = new();
            foreach (var action in attackActionsWithValidTargets)
            {
                foreach (var target in action.PossibleTargets.Select(t => t.Target))
                {
                    weightedActions.Add((action.Action, target, action.Action.GetActionWeightFor(target, this)));
                }
            }
            var maxWeight = weightedActions.Max(a => a.Weight);
            var actionsWithMaxWeight = weightedActions.Where(a => a.Weight == maxWeight);
            var actionToUse = actionsWithMaxWeight.TakeRandomElement(Rng);

            CurrentAction = actionToUse.Action;
            CurrentTarget = actionToUse.Target;

            if (CurrentTarget != this && CurrentTarget != ContainingTile)
                PathToUse = (Destination: CurrentTarget.Position, Route: Map.GetPathBetweenTiles(Position, CurrentTarget.Position));
            else
                PathToUse = (null, null);
        }

        private void PickADistantTargetIfPossible()
        {
            var minimumMaximumRange = OnAttack.Where(oaa => oaa.MayBeUsed).Min(oaa => oaa.MaximumRange);
            var attackActionsWithMinimumMaximumRange = OnAttack.Where(oaa => oaa.MayBeUsed && oaa.MaximumRange == minimumMaximumRange);
            var closestTargets = GetClosestTargets(attackActionsWithMinimumMaximumRange.TakeRandomElement(Rng));

            if (!closestTargets.Any()) return;

            var pickedTarget = closestTargets.TakeRandomElement(Rng);
            var destination = pickedTarget.Position;
            var distance = (int)GamePoint.Distance(pickedTarget.Position, Position);
            var minimumMinimumRange = attackActionsWithMinimumMaximumRange.Min(aawmmr => aawmmr.MinimumRange);

            if (distance < minimumMinimumRange)
            {
                if (PathToUse.Destination != null && GamePoint.Distance(PathToUse.Destination, pickedTarget.Position).Between(minimumMinimumRange, minimumMaximumRange))
                {
                    destination = PathToUse.Destination;
                }
                else
                {
                    var possibleDestinations = Map.Tiles.GetElementsWithinDistance(Position.Y, Position.X, minimumMaximumRange, true)
                                    .Where(t => t.IsWalkable && !t.IsOccupied && GamePoint.Distance(t.Position, pickedTarget.Position).Between(minimumMinimumRange, minimumMaximumRange));
                    var paths = possibleDestinations.Select(pd => Map.GetPathBetweenTiles(Position, pd.Position)).Where(p => p.Any()).ToList();
                    var minLength = paths.Min(p => p.Count);
                    var pathWithMinLength = paths.First(p => p.Count == minLength);
                    destination = pathWithMinLength[pathWithMinLength.Count - 1].Position;
                }
            }

            CurrentTarget = pickedTarget;
            PathToUse = (Destination: destination, Route: Map.GetPathBetweenTiles(Position, destination));
        }

        public bool TileCanBeApproached(Tile t)
        {
            return t != null && t.IsWalkable && t != ContainingTile && (!t.IsOccupied || (CurrentTarget != null && t.LivingCharacter == CurrentTarget)) && FOVTiles.Contains(t);
        }

        private bool GetAnApproachablePath(IEnumerable<Tile> adjacentTiles, out List<Tile> pickedPath)
        {
            foreach (var adjacentTile in adjacentTiles)
            {
                // Exclude the previous tile from the path to avoid walking in circles
                if (!TileCanBeApproached(adjacentTile) || adjacentTile.Position.Equals(LastPosition)) continue;
                var pathToDestination = Map.GetPathBetweenTiles(adjacentTile.Position, PathToUse.Destination);
                if (pathToDestination == null) continue;

                // Exclude the current tile from the path to avoid walking in circles
                if (pathToDestination.Any() && !pathToDestination.Contains(ContainingTile))
                {
                    pathToDestination.Insert(0, ContainingTile);
                    pickedPath = pathToDestination;
                    return true;
                }
            }
            pickedPath = null;
            return false;
        }

        public void AttackOrMove()
        {
            if(ExistenceStatus != EntityExistenceStatus.Alive
                || CurrentTarget == null)
            {
                RemainingMovement = 0;
                TookAction = true;
                return;
            }

            if(CurrentTarget != this)
            {
                if (CurrentAction != null && CurrentAction.CanBeUsedOn(CurrentTarget))
                {
                    if(!CurrentAction.TargetTypes.Contains(TargetType.Tile))
                        AttackCharacter(CurrentTarget as Character, CurrentAction);
                    else
                        InteractWithTile(CurrentTarget as Tile, CurrentAction);
                }
                else
                {
                    var validActions = OnAttack.Where(oaa => oaa.CanBeUsedOn(CurrentTarget));
                    if (validActions.Any())
                    {
                        List<(ActionWithEffects Action, ITargetable Target, int Weight)> weightedActions = new();
                        foreach (var action in validActions)
                        {
                            weightedActions.Add((action, CurrentTarget, action.GetActionWeightFor(CurrentTarget, this)));
                        }
                        var maxWeight = weightedActions.Max(a => a.Weight);
                        var actionsWithMaxWeight = weightedActions.Where(a => a.Weight == maxWeight);
                        if (actionsWithMaxWeight.Any())
                        {
                            var pickedAction = actionsWithMaxWeight.TakeRandomElement(Rng).Action;
                            if(!pickedAction.TargetTypes.Contains(TargetType.Tile))
                                AttackCharacter(CurrentTarget as Character, pickedAction);
                            else
                                InteractWithTile(CurrentTarget as Tile, pickedAction);
                        }
                        else
                            MoveTo(PathToUse.Destination);
                    }
                    else
                        MoveTo(PathToUse.Destination);
                }
            }
            else
            {
                if (CurrentAction.CanBeUsedOn(this))
                    AttackCharacter(this, CurrentAction);
                else
                {
                    var possibleActionsOnSelf = new List<(ActionWithEffects action, Item item)>();
                    foreach (var onAttackAction in OnAttack.Where(oaa => oaa != LastUsedActionOnSelf && oaa.CanBeUsedOn(this)))
                    {
                        possibleActionsOnSelf.Add((onAttackAction, null));
                    }
                    foreach (var item in Inventory.Where(i => i.EntityType == EntityType.Consumable))
                    {
                        if (item.OnUse != LastUsedActionOnSelf && item.OnUse.MayBeUsed)
                            possibleActionsOnSelf.Add((item.OnUse, item));
                    }
                    if (possibleActionsOnSelf.Any())
                    {
                        var (action, item) = possibleActionsOnSelf.TakeRandomElement(Rng);
                        if (item == null)
                            AttackCharacter(this, action);
                        else
                            action?.Do(item, this, true);
                        LastUsedActionOnSelf = action;
                        if (action?.FinishesTurnWhenUsed == true)
                            TookAction = true;
                    }
                }
            }
            if((RemainingMovement > 0 && Movement == 0) || TookAction)
                LastUsedActionOnSelf = null;
        }

        public void MoveTo(GamePoint p)
        {
            if (p == null)
            {
                RemainingMovement = 0;
            }

            if (RemainingMovement == 0)
            {
                if(Movement == 0)
                    TookAction = true;
                return;
            }

            // We store the latest used path to avoid unnecessary recalculations.
            var path = p.Equals(PathToUse.Destination) ? PathToUse : (Destination: p, Route: Map.GetPathBetweenTiles(Position, p));

            if (path.Route?.Any() == true)
            {
                if (path.Route[0].Position.Equals(Position))
                    path.Route = path.Route.Skip(1).ToList();

                if (path.Route.Any() && path.Route[0] != ContainingTile && Map.TryMoveCharacter(this, path.Route[0]))
                    PathToUse = path;
                else
                    PathToUse.Destination = null;
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

        public List<Character> GetClosestTargets(ActionWithEffects action)
        {
            List<(Character target, int distance)> targetsAndDistances = new();
            KnownCharacters.Where(kc => action.TargetTypes.Contains(kc.TargetType))
                .ForEach(t => targetsAndDistances.Add((t.Character, (int)Math.Ceiling(GamePoint.Distance(Position, t.Character.Position)))));
            if (!targetsAndDistances.Any()) return new List<Character>();
            var minimumDistance = targetsAndDistances.Min(tad => tad.distance);
            return targetsAndDistances.Where(tad => tad.distance == minimumDistance).Select(tad => tad.target).ToList();
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
            base.Die(attacker);
            if (ExistenceStatus == EntityExistenceStatus.Dead)
            {
                Inventory?.ForEach(i => DropItem(i));
                Inventory?.Clear();
            }
        }

        public override void PickItem(Item item)
        {
            Inventory.Add(item);
            item.Owner = this;
            item.Position = null;
            item.ExistenceStatus = EntityExistenceStatus.Gone;
            Map.AppendMessage(Map.Locale["NPCPickItem"].Format(new { CharacterName = Name, ItemName = item.Name }));
        }

        public override void DropItem(Item item)
        {
            Tile pickedEmptyTile = null;
            if (!ContainingTile.GetItems().Exists(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (ContainingTile.Trap == null || ContainingTile.Trap.ExistenceStatus != EntityExistenceStatus.Alive))
                pickedEmptyTile = ContainingTile;
            if(pickedEmptyTile == null)
            {
                var closeEmptyTiles = Map.Tiles.GetElementsWithinDistanceWhere(Position.Y, Position.X, 5, true, t => t.IsWalkable && !t.IsOccupied && !t.GetItems().Exists(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (t.Trap == null || t.Trap.ExistenceStatus != EntityExistenceStatus.Alive)).ToList();
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

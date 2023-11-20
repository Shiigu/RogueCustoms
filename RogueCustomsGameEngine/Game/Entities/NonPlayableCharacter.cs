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
    #pragma warning disable S2259 // Null pointers should not be dereferenced
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
        public Point LastPosition { get; set; }

        private Character CurrentTarget;
        private readonly bool KnowsAllCharacterPositions;
        private readonly int AIOddsToUseActionsOnSelf;
        private (Point Destination, List<Tile> Route) PathToUse;
        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }

        public NonPlayableCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
            KnownCharacters.Add((this, TargetType.Self));
            PathToUse = (null, null);
            CurrentTarget = null;
            LastUsedActionOnSelf = null;
            KnowsAllCharacterPositions = entityClass.KnowsAllCharacterPositions;
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
            var attackActionsWithValidTargets = LookForAttackActionsWithValidTargets().Where(a => a.PossibleTargets.Any());
            if (attackActionsWithValidTargets.Any())
            {
                CurrentTarget = null;
                if (Rng.NextInclusive(1, 100) <= AIOddsToUseActionsOnSelf)
                {
                    var hasUsableAttacksOnSelf = OnAttack.Exists(oaa => oaa != LastUsedActionOnSelf && oaa.CanBeUsedOn(this));
                    var hasUsableItemsOnSelf = Inventory?.Exists(i => i.EntityType == EntityType.Consumable && i.OnUse != LastUsedActionOnSelf && i.OnUse.MayBeUsed) == true;
                    if(hasUsableAttacksOnSelf || hasUsableItemsOnSelf)
                    {
                        CurrentTarget = this;
                        PathToUse = (null, null);
                    }
                }
                if (CurrentTarget == null)
                {
                    var actionToUse = attackActionsWithValidTargets.TakeRandomElement(Rng);
                    var targetToUse = actionToUse.PossibleTargets.TakeRandomElement(Rng).Character;

                    CurrentTarget = targetToUse;
                    PathToUse = (Destination: targetToUse.Position, Route: Map.GetPathBetweenTiles(Position, targetToUse.Position));
                }
            }
            else
            {
                var minimumMaximumRange = OnAttack.Where(oaa => oaa.MayBeUsed).Min(oaa => oaa.MaximumRange);
                var attackActionsWithMinimumMaximumRange = OnAttack.Where(oaa => oaa.MayBeUsed && oaa.MaximumRange == minimumMaximumRange);
                var closestTargets = GetClosestTargets(attackActionsWithMinimumMaximumRange.TakeRandomElement(Rng));

                if (!closestTargets.Any()) return;

                var pickedTarget = closestTargets.TakeRandomElement(Rng);
                var destination = pickedTarget.Position;
                var distance = (int)Point.Distance(pickedTarget.Position, Position);
                var minimumMinimumRange = attackActionsWithMinimumMaximumRange.Min(aawmmr => aawmmr.MinimumRange);

                if (distance < minimumMinimumRange)
                {
                    if (PathToUse.Destination != null && Point.Distance(PathToUse.Destination, pickedTarget.Position).Between(minimumMinimumRange, minimumMaximumRange))
                    {
                        destination = PathToUse.Destination;
                    }
                    else
                    {
                        var possibleDestinations = Map.Tiles.GetElementsWithinDistance(Position.Y, Position.X, minimumMaximumRange, true)
                                        .Where(t => t.IsWalkable && !t.IsOccupied && Point.Distance(t.Position, pickedTarget.Position).Between(minimumMinimumRange, minimumMaximumRange));
                        var paths = possibleDestinations.Select(pd => Map.GetPathBetweenTiles(Position, pd.Position)).Where(p => p.Any()).ToList();
                        var minLength = paths.Min(p => p.Count);
                        var pathWithMinLength = paths.First(p => p.Count == minLength);
                        destination = pathWithMinLength[pathWithMinLength.Count - 1].Position;
                    }
                }

                CurrentTarget = pickedTarget;
                PathToUse = (Destination: destination, Route: Map.GetPathBetweenTiles(Position, destination));
            }
            // If the picked next tile is inaccessible or not visible and it does not contain the target, find another tile.
            if(PathToUse.Route?.Count > 1 && ((PathToUse.Route?[1].IsOccupied == true && CurrentTarget != null && PathToUse.Route?[1].Character != CurrentTarget) || !FOVTiles.Contains(PathToUse.Route?[1])))
            {
                var adjacentTiles = Map.GetAdjacentWalkableTiles(Position);
                var visibleAdjacentTiles = FOVTiles.Intersect(adjacentTiles);
                var orderedAdjacentTiles = visibleAdjacentTiles.OrderBy(t => ArrayHelpers.GetManhattanDistanceBetweenCells(t.Position.X, t.Position.Y, PathToUse.Destination.X, PathToUse.Destination.Y));
                foreach (var adjacentTile in orderedAdjacentTiles)
                {
                    if (!adjacentTile.IsWalkable || adjacentTile.IsOccupied || adjacentTile == ContainingTile || adjacentTile.Position.Equals(LastPosition)) continue;
                    var pathToDestination = Map.GetPathBetweenTiles(adjacentTile.Position, PathToUse.Destination);
                    if (pathToDestination?.Any() == true && pathToDestination?.Contains(ContainingTile) != true)
                    {
                        pathToDestination.Insert(0, ContainingTile);
                        PathToUse.Route = pathToDestination;
                        break;
                    }
                }
            }
            // But if they still can't find anywhere to move, skip the turn.
            if (PathToUse.Route?.Count > 1 && ((PathToUse.Route?[1].IsOccupied == true && CurrentTarget != null && PathToUse.Route?[1].Character != CurrentTarget) || !FOVTiles.Contains(PathToUse.Route?[1])))
                RemainingMovement = 0;
        }

        public void AttackOrMove()
        {
            if(ExistenceStatus != EntityExistenceStatus.Alive
                || (CurrentTarget != null && CurrentTarget.ExistenceStatus != EntityExistenceStatus.Alive)
                || (CurrentTarget != null && !CurrentTarget.Faction.AlliedWith.Contains(Faction) && !CurrentTarget.Visible)
                || CurrentTarget == null)
            {
                RemainingMovement = 0;
                TookAction = true;
                return;
            }

            if(CurrentTarget != this)
            {
                var validActions = OnAttack.Where(oaa => oaa.CanBeUsedOn(CurrentTarget));
                if (validActions.Any())
                    AttackCharacter(CurrentTarget, validActions.TakeRandomElement(Rng));
                else
                    MoveTo(PathToUse.Destination);
            }
            else
            {
                var possibleActionsOnSelf = new List<(ActionWithEffects action, Item item)>();
                foreach (var onAttackAction in OnAttack.Where(oaa => oaa != LastUsedActionOnSelf && oaa.CanBeUsedOn(this)))
                {
                    possibleActionsOnSelf.Add((onAttackAction, null));
                }
                foreach (var item in Inventory.Where(i => i.EntityType == EntityType.Consumable))
                {
                    if(item.OnUse != LastUsedActionOnSelf && item.OnUse.MayBeUsed)
                        possibleActionsOnSelf.Add((item.OnUse, item));
                }
                if(possibleActionsOnSelf.Any())
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
            if((RemainingMovement > 0 && Movement == 0) || TookAction)
                LastUsedActionOnSelf = null;
        }

        public void MoveTo(Point p)
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

        private IEnumerable<(ActionWithEffects Action, List<(Character Character, int Distance)> PossibleTargets)> LookForAttackActionsWithValidTargets()
        {
            foreach (var action in OnAttack)
            {
                if (action.MayBeUsed)
                {
                    var possibleTargets = KnownCharacters.Where(kc => kc.TargetType != TargetType.Self && action.TargetTypes.Contains(kc.TargetType))
                        .Select(kc => (kc.Character, Distance: (int)Point.Distance(kc.Character.Position, Position)));
                    if (possibleTargets.Any())
                        yield return (action, possibleTargets.Where(kc => kc.Distance.Between(action.MinimumRange, action.MaximumRange)).ToList());
                }
            }
        }

        public List<Character> GetClosestTargets(ActionWithEffects action)
        {
            List<(Character target, int distance)> targetsAndDistances = new();
            KnownCharacters.Where(kc => action.TargetTypes.Contains(kc.TargetType))
                .ForEach(t => targetsAndDistances.Add((t.Character, (int)Math.Ceiling(Point.Distance(Position, t.Character.Position)))));
            if (!targetsAndDistances.Any()) return new List<Character>();
            var minimumDistance = targetsAndDistances.Min(tad => tad.distance);
            return targetsAndDistances.Where(tad => tad.distance == minimumDistance).Select(tad => tad.target).ToList();
        }

        public override void AttackedBy(Character source)
        {
            base.AttackedBy(source);

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
            ExistenceStatus = EntityExistenceStatus.Dead;
            Passable = true;
            if (attacker == null || attacker is Character)
                OnDeath?.Where(oda => attacker == null || oda?.ChecksCondition(this, attacker as Character) == true).ForEach(oda => oda?.Do(this, attacker, true));
            Inventory?.ForEach(i => DropItem(i));
            Inventory?.Clear();
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
                var closestDistance = closeEmptyTiles.Any() ? closeEmptyTiles.Min(t => Point.Distance(t.Position, Position)) : -1;
                var closestEmptyTiles = closeEmptyTiles.Where(t => Point.Distance(t.Position, Position) <= closestDistance);
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
    #pragma warning restore S2259 // Null pointers should not be dereferenced
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
    #pragma warning restore CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}

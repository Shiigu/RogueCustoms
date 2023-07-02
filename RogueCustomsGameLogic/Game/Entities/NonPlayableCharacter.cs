using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RogueCustomsGameEngine.Game.Entities
{
    public class NonPlayableCharacter : Character, IAIControlled
    {
        private List<(Character Character, TargetType TargetType)> KnownCharacters { get; } = new List<(Character Character, TargetType TargetType)>();
        
        // This is used to prevent continuous paying attention to themselves rather than on others
        private ActionWithEffects LastUsedActionOnSelf;

        private Character CurrentTarget;
        private bool KnowsAllCharacterPositions;
        private int AIOddsToUseActionsOnSelf;
        private (Point Destination, List<Tile> Route) PathToUse;

        public NonPlayableCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
            KnownCharacters.Add((this, TargetType.Self));
            PathToUse = (null, null);
            CurrentTarget = null;
            LastUsedActionOnSelf = null;
            KnowsAllCharacterPositions = entityClass.KnowsAllCharacterPositions;
            AIOddsToUseActionsOnSelf = entityClass.AIOddsToUseActionsOnSelf;
        }

        public void PickTargetAndPath()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive) return;                                       // Dead entities don't move
            UpdateKnownCharacterList();
            var attackActionsWithValidTargets = LookForAttackActionsWithValidTargets().Where(a => a.PossibleTargets.Any());
            if (attackActionsWithValidTargets.Any())
            {
                CurrentTarget = null;
                if (Rng.NextInclusive(1, 100) <= AIOddsToUseActionsOnSelf)
                {
                    var hasUsableAttacksOnSelf = OnAttackActions.Any(oaa => oaa != LastUsedActionOnSelf && oaa.CanBeUsedOn(this, Map));
                    var hasUsableItemsOnSelf = Inventory?.Any(i => i.EntityType == EntityType.Consumable && i.OnItemUseActions.Any(oiua => oiua != LastUsedActionOnSelf && oiua.CanBeUsed)) == true;
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
                var minimumMaximumRange = OnAttackActions.Where(oaa => oaa.CanBeUsed).Min(oaa => oaa.MaximumRange);
                var attackActionsWithMinimumMaximumRange = OnAttackActions.Where(oaa => oaa.CanBeUsed && oaa.MaximumRange == minimumMaximumRange);
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
                        destination = paths.First(p => p.Count == minLength).Last().Position;
                    }
                }

                CurrentTarget = pickedTarget;
                PathToUse = (Destination: destination, Route: Map.GetPathBetweenTiles(Position, destination));
            }
        }

        public void AttackOrMove()
        {
            if(CurrentTarget != this)
            {
                while (RemainingMovement > 0 && ExistenceStatus == EntityExistenceStatus.Alive && CurrentTarget != null && CurrentTarget.ExistenceStatus == EntityExistenceStatus.Alive)
                {
                    var validActions = OnAttackActions.Where(oaa => oaa.CanBeUsedOn(CurrentTarget, Map));
                    if (validActions.Any())
                        AttackCharacter(CurrentTarget, validActions.TakeRandomElement(Rng));
                    else
                        MoveTo(PathToUse.Destination);
                }
            }
            else
            {
                var possibleActionsOnSelf = new List<(ActionWithEffects action, Item item)>();
                ActionWithEffects possibleAttackAction = null;
                foreach (var onAttackAction in OnAttackActions.Where(oaa => oaa != LastUsedActionOnSelf && oaa.CanBeUsedOn(this, Map)))
                {
                    possibleActionsOnSelf.Add((onAttackAction, null));
                }
                foreach (var item in Inventory.Where(i => i.EntityType == EntityType.Consumable))
                {
                    foreach (var action in item.OnItemUseActions.Where(oiua => oiua != LastUsedActionOnSelf && oiua.CanBeUsed))
                    {
                        possibleActionsOnSelf.Add((action, item));
                    }
                }
                if(possibleActionsOnSelf.Any())
                {
                    var (action, item) = possibleActionsOnSelf.TakeRandomElement(Rng);
                    if (item == null)
                        AttackCharacter(this, action);
                    else
                        action.Do(item, this);
                    LastUsedActionOnSelf = action;
                }
            }
            LastUsedActionOnSelf = null;
        }

        public void MoveTo(Point p)
        {
            if (p == null)
            {
                RemainingMovement = 0;
                return;
            }
            // We store the latest used path to avoid unnecessary recalculations.
            var path = p.Equals(PathToUse.Destination) ? PathToUse : (Destination: p, Route: Map.GetPathBetweenTiles(Position, p));

            if (path.Route?.Any() == true)
            {
                if (path.Route.Count > 1)
                    path.Route = path.Route.Skip(1).ToList();

                if (Map.TryMoveCharacter(this, path.Route[0]))
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
                foreach (var characterInMap in Map.Characters.Where(c => c.ExistenceStatus == EntityExistenceStatus.Alive))
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
            foreach (var action in OnAttackActions)
            {
                if (action.CanBeUsed)
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
            if (attacker is Character c)
                OnDeathActions?.ForEach(oda => oda.Do(this, c));
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
            if (!ContainingTile.Items.Any(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (ContainingTile.Trap == null || ContainingTile.Trap.ExistenceStatus != EntityExistenceStatus.Alive))
                pickedEmptyTile = ContainingTile;
            if(pickedEmptyTile == null)
            {
                var closeEmptyTiles = Map.Tiles.GetElementsWithinDistanceWhere(Position.Y, Position.X, 5, true, t => t.IsWalkable && !t.IsOccupied && !t.Items.Any(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (t.Trap == null || t.Trap.ExistenceStatus != EntityExistenceStatus.Alive)).ToList();
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
}

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
using System.Threading.Tasks;
using RogueCustomsGameEngine.Utils;

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
        public GamePoint LastPositionBeforeRemove { get; set; }

        private readonly bool KnowsAllCharacterPositions;
        private readonly bool PursuesOutOfSightCharacters;
        private readonly bool WandersIfWithoutTarget;

        private List<Room> VisitedRooms = new();

        private (GamePoint Destination, List<Tile> Route) PathToUse;
        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }
        public bool SpawnedViaMonsterHouse { get; set; }
        public bool CanSeeTraps { get; set; }
        public LootTable LootTable { get; set; }
        public List<(EntityClass Class, int Amount)> Drops { get; set; }

        public NonPlayableCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
            KnownCharacters.Add((this, TargetType.Self));
            PathToUse = (null, null);
            LastPositionBeforeRemove = null;
            CurrentTarget = null;
            KnowsAllCharacterPositions = entityClass.KnowsAllCharacterPositions;
            PursuesOutOfSightCharacters = entityClass.PursuesOutOfSightCharacters;
            WandersIfWithoutTarget = entityClass.WandersIfWithoutTarget;
            CanSeeTraps = false;
            AIType = entityClass.AIType;

            OnSpawn = MapClassAction(entityClass.OnSpawn);
            OnInteracted = new List<ActionWithEffects>();
            MapClassActions(entityClass.OnInteracted, OnInteracted);

            LootTable = entityClass.LootTable;
            CreateDrops(entityClass.LootTable, entityClass.DropPicks);
        }

        private void CreateDrops(LootTable lootTable, int dropPicks)
        {
            Drops = [];

            if (lootTable == null || lootTable.Entries.Count == 0 || dropPicks == 0) return;

            var validItemClasses = Map.PossibleItemClasses.Except(Map.UndroppableItemClasses).ToList();
            object pickedObject = null;

            for (int i = 0; i < dropPicks; i++)
            {
                var currentLootTable = lootTable;
                var foundAPick = false;
                do
                {
                    pickedObject = currentLootTable.Entries.TakeRandomElementWithWeights(e => e.Weight, Rng).Pick;
                    if (pickedObject is LootTable lt)
                    {
                        currentLootTable = lt;
                    }
                    else if (pickedObject is EntityClass ec)
                    {
                        Drops.Add((ec, 0));
                        foundAPick = true;
                    }
                    else if (pickedObject is CurrencyPile cp)
                    {
                        Drops.Add((Map.CurrencyClass, Map.Rng.NextInclusive(cp.Minimum, cp.Maximum)));
                        foundAPick = true;
                    }
                    else if (pickedObject is string s && EngineConstants.SPECIAL_LOOT_ENTRIES.Contains(s))
                    {
                        if (s == EngineConstants.LOOT_NO_DROP)
                        {
                            // Do nothing
                            foundAPick = true;
                        }
                        else if (s == EngineConstants.LOOT_WEAPON)
                        {
                            var chosenWeapon = validItemClasses.Where(ic => ic.EntityType == EntityType.Weapon).ToList().TakeRandomElement(Rng);
                            Drops.Add((chosenWeapon, 0));
                            foundAPick = true;
                        }
                        else if (s == EngineConstants.LOOT_ARMOR)
                        {
                            var chosenArmor = validItemClasses.Where(ic => ic.EntityType == EntityType.Armor).ToList().TakeRandomElement(Rng);
                            Drops.Add((chosenArmor, 0));
                            foundAPick = true;
                        }
                        else if (s == EngineConstants.LOOT_EQUIPPABLE)
                        {
                            var chosenEquippable = validItemClasses.Where(ic => ic.EntityType == EntityType.Weapon || ic.EntityType == EntityType.Armor).ToList().TakeRandomElement(Rng);
                            Drops.Add((chosenEquippable, 0));
                            foundAPick = true;
                        }
                        else if (s == EngineConstants.LOOT_CONSUMABLE)
                        {
                            var chosenConsumable = validItemClasses.Where(ic => ic.EntityType == EntityType.Consumable).ToList().TakeRandomElement(Rng);
                            Drops.Add((chosenConsumable, 0));
                            foundAPick = true;
                        }
                    }
                }
                while (!foundAPick);
            }
        }

        public async Task ProcessAI()
        {
            if (ExistenceStatus != EntityExistenceStatus.Alive || Map.Player.ExistenceStatus != EntityExistenceStatus.Alive || AIType == AIType.Null)
            {
                RemainingMovement = 0;
                TookAction = true;
                return;     // Dead entities don't move. Set all the possible flags to ensure they aren't seen as movable.
                            // Also, disable movement if the player is dead because there's nothing else to do here.
                            // Also, disable AI if type is Null
            }
            UpdateKnownCharacterList();
            ConsiderSwappingTargets();
            if (!ConsiderWalking())
            {
                if (await ConsiderUsingActionOnSelf())
                    return;
                if (await ConsiderUsingActionOnTarget())
                    return;
                if (await ConsiderUsingActionOnTile())
                    return;
            }
            await TryToMoveToTarget();
        }

        public void ConsiderSwappingTargets()
        {
            if (ContainingRoom != null && !VisitedRooms.Contains(ContainingRoom))
                VisitedRooms.Add(ContainingRoom);

            var formerTarget = CurrentTarget;
            var distanceToCurrentTarget = (CurrentTarget is Character c && c.ExistenceStatus == EntityExistenceStatus.Alive) ? (int) GamePoint.Distance(c.Position, Position) : int.MaxValue;
            var targetsWithinSight = KnownCharacters.Where(kc => kc.Character != this && kc.Character.ExistenceStatus == EntityExistenceStatus.Alive && CanSee(kc.Character));

            // If I don't have a target or someone valid is closer than my current target, consider swapping to them
            var closerCharacters = targetsWithinSight.Where(t => GamePoint.Distance(t.Character.Position, Position) < distanceToCurrentTarget && OnAttack.Any(oa => oa.CanBeUsedOn(t.Character)));

            // If no one close enough is valid, consider swapping to anyone within sight that I can attack
            if (!closerCharacters.Any())
                closerCharacters = targetsWithinSight.Where(t => OnAttack.Any(oa => oa.CanBeUsedOn(t.Character)));

            // If no one within sight is valid, consider swapping to anyone within sight that I can potentially attack if I'm at range
            if (!closerCharacters.Any())
                closerCharacters = targetsWithinSight.Where(t => OnAttack.Any(oa => oa.MaximumRange > 0 && oa.TargetTypes.Contains(t.TargetType)));

            if (closerCharacters.Any())
            {
                var targetingPreferences = new List<(Character Character, int Weight)>();
                foreach (var character in closerCharacters)
                {
                    var distanceToTarget = Math.Max(1, (int)GamePoint.Distance(character.Character.Position, Position));

                    // I prefer targeting enemies
                    var factionWeight = character.TargetType switch
                    {
                        TargetType.Enemy => 40,
                        TargetType.Ally => 20,
                        TargetType.Neutral => 10,
                        _ => 0
                    };

                    // I prefer closer targets
                    var finalWeight = factionWeight - (distanceToTarget * 2);

                    targetingPreferences.Add((character.Character, Math.Max(1, finalWeight)));
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
                    if (FOVTiles.Any(t => t == positionTile))
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
            {
                // If I'm on a Hallway or connector tile, I wander to prevent blocking other Characters
                if (ContainingTile.Type != TileType.Hallway && !Map.GetAdjacentTiles(Position, true).Any(t => t.Type == TileType.Hallway))
                    return false;
            }

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

        public async Task<bool> ConsiderUsingActionOnSelf()
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

            await pickedAction.Action.Do(this, this, true);
            if (pickedAction.Action.FinishesTurnWhenUsed)
                TookAction = true;

            return true;
        }

        public async Task<bool> ConsiderUsingActionOnTile()
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

            await pickedAction.Action.Do(this, pickedAction.Tile, true);
            if (pickedAction.Action.FinishesTurnWhenUsed)
                TookAction = true;

            return true;
        }

        public async Task<bool> ConsiderUsingActionOnTarget()
        {
            var currentTargetInfo = KnownCharacters.Find(kc => kc.Character == CurrentTarget);
            if (currentTargetInfo == default)
                return false; // This ideally shouldn't happen, but in case it does...
            var distanceToTarget = (int) GamePoint.Distance(Position, CurrentTarget.Position);
            var possibleActionsOnTarget = OnAttack.Where(oa => oa.TargetTypes.Contains(currentTargetInfo.TargetType)).ToList();
            var actionsAndWeights = new List<(ActionWithEffects Action, int Weight)>();
            foreach (var action in possibleActionsOnTarget)
            {
                if (!action.ChecksCondition(this, CurrentTarget) || !action.ChecksAICondition(this, CurrentTarget) || !action.CanBeUsedOn(CurrentTarget)) continue;
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
                var tilesAtMinimumRangeFromTarget = Map.GetTilesWithinDistance(CurrentTarget.Position, pickedAction.Action.MinimumRange, false).Where(t => t.IsWalkable && !t.IsOccupied);
                var visibleTilesAtMinimumRangeFromTarget = FOVTiles.Intersect(tilesAtMinimumRangeFromTarget);
                if (!visibleTilesAtMinimumRangeFromTarget.Any())
                    return false; // No valid tile, don't do anything
                var tilesAndWeights = new List<(Tile Tile, int Weight)>();
                foreach (var tile in visibleTilesAtMinimumRangeFromTarget)
                {
                    tilesAndWeights.Add((tile, (int) (1 / ((int) GamePoint.Distance(Position, tile.Position) / 10F)) * 100));
                }
                var pickedTile = tilesAndWeights.TakeRandomElementWithWeights(taw => taw.Weight, Rng);
                CurrentTarget = pickedTile.Tile;
                return false;
            }

            await pickedAction.Action.Do(this, CurrentTarget, true);
            if (pickedAction.Action.FinishesTurnWhenUsed)
                TookAction = true;

            return true;
        }

        public async Task TryToMoveToTarget()
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
            if (await Map.TryMoveCharacter(this, nextTile))
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
                        pathsFromAdjacentTiles.Add(Map.GetPathBetweenTiles(tile.Position, PathToUse.Route.Last().Position));
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

        public override async Task AttackedBy(Character source)
        {
            await base.AttackedBy(source);

            if (source == null) return;

            // If this character has a neutral relation with another's faction, they get flagged as an enemy if they get attacked by them

            if (CalculateTargetTypeFor(source) != TargetType.Neutral) return;
            var knownCharacter = KnownCharacters.Find(kc => kc.Character.Equals(source));
            if (knownCharacter != default)
                knownCharacter.TargetType = TargetType.Enemy;
            else
                KnownCharacters.Add((source, TargetType.Enemy));
        }

        public override async Task Die(Entity? attacker = null)
        {
            var events = new List<DisplayEventDto>();
            foreach (var oda in OnDeath?.Where(oda => oda != null && (attacker == null || attacker is not Character || oda.ChecksCondition(this, attacker as Character) == true)))
            {
                await oda.Do(this, attacker, true);
            }
            if (HP.Current <= 0)
            {
                ExistenceStatus = EntityExistenceStatus.Dead;
                Passable = true;
                Inventory?.ForEach(i => DropItem(i));
                Inventory?.Clear();
                var droppedCurrency = false;
                foreach (var dropEntry in Drops)
                {
                    var entryClass = dropEntry.Class;
                    if (entryClass != Map.CurrencyClass)
                    {
                        var itemForDrop = await Map.AddEntity(entryClass, 1, null, false) as Item;
                        if (LootTable.OverridesQualityLevelOddsOfItems)
                        {
                            itemForDrop.SetQualityLevel(LootTable.QualityLevelOdds.TakeRandomElementWithWeights(qlo => qlo.ChanceToPick, Rng).QualityLevel);
                        }
                        DropItem(itemForDrop);
                    }
                    else
                    {
                        CurrencyCarried += dropEntry.Amount;
                    }
                }
                if (CurrencyCarried > 0)
                {
                    droppedCurrency = true;
                    var currencyForDrop = Map.CreateCurrency(CurrencyCarried, null, false);
                    DropItem(currencyForDrop);
                }
                if (attacker == Map.Player || Map.Player.CanSee(this))
                {
                    if (!Map.IsDebugMode && Position != null)
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
                    if (droppedCurrency)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.Currency }
                        }
                        );
                    }
                }
                Map.DisplayEvents.Add(($"NPC {Name} dies", events));
                if (attacker is Character c && ExperiencePayout > 0)
                    await GiveExperienceTo(c);
            }
        }

        public override void PickItem(IPickable pickable, bool informToPlayer)
        {
            var pickableAsEntity = pickable as Entity;
            var isCurrency = false;
            pickable.Owner = this;
            if (pickable is Item i)
            {
                Inventory.Add(i);
            }
            else if (pickable is Currency c)
            {
                isCurrency = true;
                CurrencyCarried += c.Amount;
            }
            pickableAsEntity.Position = null;
            pickableAsEntity.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                if (isCurrency)
                {
                    Map.AppendMessage(Map.Locale["CharacterPicksCurrency"].Format(new { CharacterName = Name, CurrencyName = pickableAsEntity.Name }));
                    Map.DisplayEvents.Add(($"NPC {Name} picked currency", new()
                    {
                        new() {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.NPCItemGet }
                        }
                    }
                    ));
                }
                else
                {
                    Map.AppendMessage(Map.Locale["NPCPickItem"].Format(new { CharacterName = Name, ItemName = pickableAsEntity.Name }));
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
        }

        public override void DropItem(IPickable pickable)
        {
            var pickableAsEntity = pickable as Entity;
            var events = new List<DisplayEventDto>();
            var centralPosition = Position ?? LastPositionBeforeRemove;
            var centralTile = Map.GetTileFromCoordinates(centralPosition);
            Tile pickedEmptyTile = null;
            if (!centralTile.GetPickableObjects().Exists(i => i.ExistenceStatus == EntityExistenceStatus.Alive) && (centralTile.Trap == null || centralTile.Trap.ExistenceStatus != EntityExistenceStatus.Alive))
                pickedEmptyTile = centralTile;
            if(pickedEmptyTile == null)
            {
                var closeEmptyTiles = Map.Tiles.GetElementsWithinDistanceWhere(centralPosition.Y, centralPosition.X, 5, true, t => t.AllowsDrops).ToList();
                if(centralTile?.AllowsDrops == true)
                    closeEmptyTiles.Add(centralTile);
                closeEmptyTiles = closeEmptyTiles.Where(t => t.LivingCharacter == null || t.LivingCharacter.ExistenceStatus != EntityExistenceStatus.Alive || t.LivingCharacter == this).ToList();
                var closestDistance = closeEmptyTiles.Any() ? closeEmptyTiles.Min(t => (int) GamePoint.Distance(t.Position, centralPosition)) : -1;
                var closestEmptyTiles = closeEmptyTiles.Where(t => (int) GamePoint.Distance(t.Position, centralPosition) <= closestDistance);
                if (closestEmptyTiles.Any())
                {
                    pickedEmptyTile = closestEmptyTiles.TakeRandomElement(Rng);
                }
                else
                {
                    pickableAsEntity.Position = null;
                    pickable.Owner = null!;
                    pickableAsEntity.ExistenceStatus = EntityExistenceStatus.Gone;
                    Map.AppendMessage(Map.Locale["NPCItemCannotBePutOnFloor"].Format(new { ItemName = pickableAsEntity.Name }));
                    if(pickableAsEntity is Item i)
                        Map.Items.Remove(i);
                }
            }
            if (pickedEmptyTile != null)
            {
                pickableAsEntity.Position = pickedEmptyTile.Position;
                pickable.Owner = null!;
                pickableAsEntity.ExistenceStatus = EntityExistenceStatus.Alive;
                Map.AppendMessage(Map.Locale["NPCPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = pickableAsEntity.Name }));
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { pickableAsEntity.Position, Map.GetConsoleRepresentationForCoordinates(pickableAsEntity.Position.X, pickableAsEntity.Position.Y) }
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

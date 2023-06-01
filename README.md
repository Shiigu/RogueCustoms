# Rogue Customs

Welcome to Rogue Customs, the roguelike where YOU make the dungeon!

The gameplay itself might be extremely simplistic, but the appeal is that you don't have to beat a certain kind of dungeon with the same enemies and items. Instead, you are the one to define it. Do you want to go past a temple full of mystics? Sure. Do you want to get past an underground mafia facility? Go ahead! Your imagination's the limit (as well as the engine's limitations)!

# How to play

Head to the Releases section and, at the very least, download the Client file.

Unzip the contents in a new folder, and open the `RoguelikeConsoleClient.exe` file.

You will be presented with two options:
- **Pick a Dungeon**: The game will retrieve all the dungeons you have saved in the `JSON` folder (if you are playing locally), or the ones stored in the server's own `JSON` folder (if playing against a server). Pick one, select "Play Selected Dungeon" and play! Follow the instructions given by the game to continue.
- **Options**: At the time of writing, this currently only has two settings: whether the game will have to look for dungeons locally or server-side (and in case it's server-side, you have to give the server address).

# But how do I make a dungeon?

In this game, Dungeons take the form of `.json` files following a certain template. In this repository's root folder, you will find a `DungeonBasicTemplate.json` file that has all the basic elements to start building.

For more details... from here on you will see an explanation for each element in order.

## The root

Present at the root of the JSON file, this contains all the information pertaining the entire dungeon.

Its fields are:
- `Name`: The dungeon name, which will be used in the **Pick a Dungeon** screen and all Message Boxes created by it in-game.
- `Author`: The dungeon's author, which will be used in the **Pick a Dungeon** screen.
- `WelcomeMessage`: Before the dungeon begins, a **BRIEFING** screen will show up with this field's contents.
- `EndingMessage`: After getting through the stairs of the dungeon's last floor, a **THE END** screen will show up with this field's contents.
- `AmountOfFloors`: The amount of floors in a dungeon. In a more practical way, the amount of stairs the player will have to cross before meeting the **THE END** screen.

## FloorInfos

This contains all the information used to procedurally generate a floor. This field is an array, so you can have as many Floor Configurations as you want, but make sure no two Floor Configurations overlap in any level.

Its fields are:
- `MinFloorLevel`: The lowest Floor level in which this configuration applies.
- `MaxFloorLevel`: The highest Floor level in which this configuration applies. Both `MinFloorLevel` and `MaxFloorLevel` can be the same (meaning the Configuration only lasts for a single Floor), but `MaxFloorLevel` cannot be lower than `MinFloorLevel`.
- `Width`: The Floor's Width in tiles. The packaged console application can show up to 64x32 tiles at the same time, but it supports more.
- `Height`: The Floor's Height in tiles. The packaged console application can show up to 64x32 tiles at the same time, but it supports more.
- `GenerateStairsOnStart`: If `true`, the stairs will be present on the Floor as soon as it's generated. Otherwise, you must set something or someone to `GenerateStairs` in some Action for the Stairs to spawn (otherwise you softlock).
- `PossibleMonsters`:

___

### PossibleMonsters

This contains the list of monsters (allied, neutral or enemy) that can be generated in the current Floor Configuration.

Its fields are:
- `ClassId`: The Id of a monster.
- `MinLevel`: The lowest level the monster can have in this configuration.
- `MaxLevel`: The highest level the monster can have in this configuration. Both `MinLevel` and `MaxLevel` can be the same (meaning the monster can only be at one level), but `MaxLevel` cannot be lower than `MinLevel`.
- `SimultaneousMaxForKindInFloor`: Indicates how many monsters of this Id can be on the Floor at the same time.
- `OverallMaxForKindInFloor`: Indicates how many monsters of this Id can be generated in the same Floor (even if they aren't all present at the same time).
- `ChanceToPick`: Indicates the odds (1 to 100) to pick a monster from the entire pool. **The sum of all `ChanceToPick` from PossibleMonsters must equal 100!**
- `CanSpawnOnFirstTurn`: If `true`, the monster can be generated when the Floor is generated.
- `CanSpawnAfterFirstTurn`: If `true`, the monster can be generated every `TurnsPerMonsterGeneration` turns.

___

- `SimultaneousMinMonstersAtStart`: Indicates the minimum amount of monsters that the Floor will generate when it's created.
- `SimultaneousMaxMonstersAtStart`: Indicates the maximum amount of monsters that the Floor will generate when it's created. Both `SimultaneousMinMonstersAtStart` and `SimultaneousMaxMonstersAtStart` can be the same (meaning the Floor will always have the same amount of monsters at start), but `SimultaneousMaxMonstersAtStart` cannot be lower than `SimultaneousMinMonstersAtStart`.
- `TurnsPerMonsterGeneration`: Indicates how many turns will pass between each attempt from the Floor to generate a monster. Say, if `TurnsPerMonsterGeneration` is 25, the Floor will attempt to generate a monster at turns 25, 50, 75, 100...
- `PossibleItems`:

___

### PossibleItems

This contains the list of items that can be generated in the current Floor Configuration. Items can only be generated when the Floor is created.

Its fields are:
- `ClassId`: The Id of an item.
- `SimultaneousMaxForKindInFloor`: Indicates how many items of this Id can be on the Floor at the same time.
- `ChanceToPick`: Indicates the odds (1 to 100) to pick an item from the entire pool. **The sum of all `ChanceToPick` from PossibleItems must equal 100!**

___

- `MinItemsInFloor`: Indicates the minimum amount of items that the Floor will generate when it's created.
- `MaxItemsInFloor`: Indicates the maximum amount of items that the Floor will generate when it's created. Both `MinItemsInFloor` and `MaxItemsInFloor` can be the same (meaning the Floor will always have the same amount of items at start), but `MaxItemsInFloor` cannot be lower than `MinItemsInFloor`.
- `PossibleTraps`:

___

### PossibleTraps

This contains the list of traps that can be generated in the current Floor Configuration. Traps can only be generated when the Floor is created.

Its fields are:
- `ClassId`: The Id of a trap.
- `SimultaneousMaxForKindInFloor`: Indicates how many traps of this Id can be on the Floor at the same time.
- `ChanceToPick`: Indicates the odds (1 to 100) to pick a trap from the entire pool. **The sum of all `ChanceToPick` from PossibleTraps must equal 100!**

___

- `MinTrapsInFloor`: Indicates the minimum amount of traps that the Floor will generate when it's created.
- `MaxTrapsInFloor`: Indicates the maximum amount of traps that the Floor will generate when it's created. Both `MinTrapsInFloor` and `MaxTrapsInFloor` can be the same (meaning the Floor will always have the same amount of traps at start), but `MaxTrapsInFloor` cannot be lower than `MinTrapsInFloor`.
- `MaxConnectionsBetweenRooms`: Indicates the maximum amount of hallways the Floor can have between the two same rooms. **Unless you are using the `OneBigRoom` generation algorithm, this must be 1 or higher.**
- `OddsForExtraConnections`: Indicates the odds (1 to 100) in which the game will try to generate an extra hallway between two rooms. Does nothing if `MaxConnectionsBetweenRooms` is lower than 2.
- `RoomFusionOdds`: Indicates the odds (1 to 100) in which the game will try to fuse two rooms. Does nothing if the generation algorithm prevents generating more than one non-Dummy room.[^1]
- `PossibleGeneratorAlgorithms`:

___

### PossibleGeneratorAlgorithms

This contains the list of algorithms that can be used to generate a Floor in the current Floor Configuration.

The Generator Algorithms work by splitting the map into `Rows`x`Columns` cells of identical (if possible), size. Every Room, before fusing, cannot get past their respective cell's boundaries.

Its fields are:
- `Name`: The Algorithm's name. They are as follows:
    1. **Standard**: It has no particular bias towards room generation. Any possible room can be Dummy[^1] or not, but at least one will not be Dummy.
    2. **OuterDummyRing**: All the rooms at the borders of the map will be Dummies[^1]. All other rooms will be non-Dummies.
    3. **InnerDummyRing**: Opposite to the previous one. All the rooms at the borders of the map will be non-Dummies[^1]. All other rooms will be Dummies.
    4. **OneBigRoom**: Generates a single room of a variable size.
- `Rows`: Indicates the amount of Rows the Floor will be split into.
- `Columns`: Indicates the amount of Columns the Floor will be split into.

It is possible to have multiple Algorithms with the same name.

**NOTE**: A Generator Algorithm will be valid if it allows the game to generate a Room with a size of at least 5x5 in every row and column.

___

- `OnFloorStartActions`: Indicates a set of actions that will be performed when the Floor is generated. Actions will be explained in a further section.

## FactionInfos

This indicates all the factions that are fighting in the dungeon. Every Character, has to belong to one faction.

Factions matter most when it comes to AI behaviour and action targeting.

Factions can interact with each other in the following way:
- **Allied**: Allied AIs will not attack each other. Attacking an Allied AI will not cause them to retaliate.
- **Neutral**: Neutral AIs will not attack anyone unless they are attacked, which will cause them to retaliate against their attacker.
- **Enemy**: Enemy AIs will chase after any enemy they can see.

Its fields are:
- `Id`: The faction's Id.
- `Name`: The faction's name. Currently unused in the Console Client.
- `Description`: Currently unused in the Console Client.
- `AlliedWith`: An array of Ids of the Factions this is allied with.
- `NeutralWith`: An array of Ids of the Factions this is neutral with.
- `EnemyWith`: An array of Ids of the Factions this is enemies with.

## Characters

This contains all the characters in the Dungeon - in more technical descriptions, anyone that can die.

Its fields are:
- `Id`: The Character's Id.
- `Name`: The Character's name.
- `Description`: Currently unused in the Console Client.
- `ConsoleRepresentation`: Indicates how it will be shown in the Console Client. Console Representation will be explained in a further section.
- `EntityType`: For a Character, it will either be `Player` or `NPC`. **There can only be one Character with the `Player` EntityType per Dungeon!**
- `Faction`: The Id belonging to the Player's faction.
- `StartsVisible`: If `false`, the Character will be invisible until an action involving them is performed.
- `OnTurnStartActions`: Indicates a set of actions that can be autonomously performed at the start of a turn. Actions will be explained in a further section.
- `BaseHP`: The initial HP stat. Must be an integer.
- `BaseAttack`: The initial Attack stat. Must be an integer.
- `BaseDefense`: The initial Defense stat. Must be an integer.
- `BaseMovement`: The initial Movement stat, indicating how many tiles the Character can move per turn. Must be an integer.
- `BaseHPRegeneration`: The initial HP Regeneration stat.
- `BaseSightRange`: Indicates the Character's FOV. Can be `FullMap` (indicating they know the entire map from the start), `FullRoom` (indicating they can see the whole room they are in, or only the nearby tiles if in a Hallway) or an integer indicating how far they can see (this is lower in a Hallway). For Players, it also indicates how much of a map is shown in screen.
- `InventorySize`: Indicates how many items the Character can carry (not counting equipped ones).
- `StartingWeapon`: Indicates the Id of the `Weapon` the Character starts with. Said Weapon must be defined in the Items section. **This MUST be set!**
- `StartingArmor`: Indicates the Id of the `Armor` the Character starts with. Said Armor must be defined in the Items section. **This MUST be set!**
- `CanGainExperience`: Indicates if the Character is a valid recipient of the `GainExperience` action.
- `OnAttackActions`: Indicates a set of actions that can be performed when targeting, regardless of the owned items. Actions will be explained in a further section.
- `OnAttackedActions`: Indicates a set of actions that can be autonomously performed when attacked, regardless of the owned items. Actions will be explained in a further section.
- `OnDeathActions`: Indicates a set of actions that will be autonomously performed once the Character dies. Actions will be explained in a further section.
- `MaxLevel`: Indicates the level in which the Character can't gain any more experience.
- `ExperiencePayoutFormula`: Indicates the mathematical formula to calculate how much experience this Character gives when it uses the `GainExperience` action. `Level` can be used as a variable in the formula.
- `ExperienceToLevelUpFormula`: Indicates the mathematical formula to calculate how much experience this Character needs to level up. `Level` can be used as a variable in the formula.
- `MaxHPIncreasePerLevel`: How much Max HP the Character gains with every level past 1. **NOTE: Current HP is always maxed out upon leveling up.**
- `AttackIncreasePerLevel`: How much Attack the Character gains with every level past 1.
- `DefenseIncreasePerLevel`: How much Defense the Character gains with every level past 1.
- `MovementIncreasePerLevel`: How much Movement the Character gains with every level past 1.
- `HPRegenerationIncreasePerLevel`: How much HP Regeneration the Character gains with every level past 1.

There are also two dynamically calculated stats in a Character:
- `Damage`: Indicates the overall damage-dealing capabilities. It`s a combination of the Character's Attack, the current Weapon's Power, and any Attack boosts and nerfs that currently apply.
- `Mitigation`: Indicates the overall damage-taking reduction capabilities. It`s a combination of the Character's Defemse, the current Armor's Power, and any Defense boosts and nerfs that currently apply.

## Items

This contains all the items in the Dungeon - in more technical descriptions, anything that can be put in the inventory. Even enemy-only weapons and armor have to be defined here.

Its fields are:
- `Id`: The Item's Id.
- `Name`: The Item's name.
- `Description`: Indicates the Item's description in the Console Client's Inventory Screen. Can be used to explain what it does.
- `ConsoleRepresentation`: Indicates how it will be shown in the Console Client. Console Representation will be explained in a further section.
- `EntityType`: For an Item, it can be `Weapon`, `Armor`, or `Consumable`.
- `StartsVisible`: If `false`, the Item will be invisible until an action involving them is performed.
- `Power`: Indicates the Item's "strength". Can be used as a parameter in an action, and can be written using Dungeons & Dragon's Dice Notation. In a `Weapon`, it adds to a Character's Damage. In an `Armor`, it adds to a Character`s Mitigation.
- `OnTurnStartActions`: Indicates a set of actions that the Item can autonomously perform, if equipped by a Character, at the start of a turn. Actions will be explained in a further section.
- `OnAttackActions`: Indicates a set of actions that can be performed, if owned by a Character, when targeting. Actions will be explained in a further section.
- `OnAttackedActions`: Indicates a set of actions that can be autonomously performed, if a Character equipping it is attacked. Actions will be explained in a further section.
- `OnItemSteppedActions`: Indicates a set of actions that can be autonomously performed if an Item, when in the Floor, is stepped on by a Character. Actions will be explained in a further section.
- `OnItemUseActions`: Indicates a set of actions that can be performed by a Character when using the Item. Actions will be explained in a further section.

## Traps

This contains all the traps in the Dungeon - in more technical descriptions, anything that can't die or be put in the inventory.

Its fields are:
- `Id`: The Trap's Id.
- `Name`: The Trap's name.
- `Description`: Indicates the Trap's description in the Console Client's Inventory Screen. Can be used to explain what it does.
- `ConsoleRepresentation`: Indicates how it will be shown in the Console Client. Console Representation will be explained in a further section.
- `EntityType`: For a Trap, it is always `Trap`.
- `StartsVisible`: If `false`, the Trap will be invisible until an action involving them is performed.
- `Power`: Indicates the Trap's "strength". Can be used as a parameter in an action, and can be written using Dungeons & Dragon's Dice Notation.
- `OnItemSteppedActions`: Indicates a set of actions that can be autonomously performed if a Trap is stepped on by a Character. Actions will be explained in a further section.

## AlteredStatuses

This contains all the Altered Statuses in the Dungeon - in more technical descriptions, anything that can affect a Character for a period of time.

Its fields are:
- `Id`: The Status's Id. Two instances of an Altered Status, for considerations of `CanStack` and `CanOverwrite`, will be considered to be the same if the have the same Id.
- `Name`: The Status's name.
- `Description`: Indicates the Status's description in the Console Client's Player Info Screen. Can be used to explain what it does.
- `ConsoleRepresentation`: Indicates how it will be shown in the Console Client. Console Representation will be explained in a further section.
- `EntityType`: For an Altered Status, it is always `AlteredStatus`.
- `CanStack`: If `true`, it will be possible to have multiple copies of the same Altered Status on the same Character.
- `CanOverwrite`: If `true` and `CanStack` is `false`, attempting to apply a new instance of the Altered Status will remove the old one before applying the new one.
- `CleanseOnFloorChange`: If `true`, all instances of this Altered Status will be removed from the Player when they use the stairs.
- `CleansedByCleanseActions`: If `true`, an action with a `Cleanse` effect will remove the status.
- `OnTurnStartActions`: Indicates a set of actions that the Altered Status can autonomously perform, if applied on a Character, at the start of a turn. Actions will be explained in a further section.
- `OnStatusApplyActions`: Indicates a set of actions that the Altered Status will autonomously perform the moment it's applied to a Character. Actions will be explained in a further section.

### Console Representation

Indicates how the Console Client will display a Character, Item, Trap or Altered status.

Its fields are:
- `Character`: Indicates the display Character.
- `BackgroundColor`: Indicates the Character's Background Color. It has four fields: **R**ed, **G**reen, **B**lue, and **A**lpha.
- `ForegroundColor`: Indicates the Character's actual Color. It has four fields: **R**ed, **G**reen, **B**lue, and **A**lpha.

### Actions and Effects

Most of the Dungeon's programmability comes from an object called an Action. Every Floor, Character, Item, Trap or Altered Status has a set of Actions that can be performed on certain circumnstances.

When an Action is performed, it always has the following parameters:
- `This`: Indicates the object the action is programmed to.
- `Source`: Indicates the object that called the action.
- `Target`: Indicates the object the action is directed to.

For example, if `Player` uses an item called `Scroll of Destruction` on an `Enemy`, the `OnAttackAction` will have the following parameters:
- `This`: Scroll of Destruction
- `Source`: Player
- `Target`: Enemy

Regardless of the name of the Action field, it has the following fields:
- `Name`: If it's part of an `OnAttackActions`, it is used to display the Action's name in the Console Client's Action screen. Used internally otherwise.
- `TargetTypes`: Indicates who can be targeted by the action. Currently supports `self` (the Character that calls the action), `Ally`, `Neutral` or `Enemy` (self-explanatory). It's an array, so it can target multiple types of Characters.
- `MinimumRange`: Indicates the minimum distance between the Action's `Source` and the `Target`. As a reference, Melee Range indicates a Range of 1.
- `MaximumRange`: Indicates the maximum distance between the Action's `Source` and the `Target`. As a reference, Melee Range indicates a Range of 1. Both `MinimumRange` and `MaximumRange` can be the same (indicating the Action can only be performed at only one distance), but `MaximumRange` cannot be lower than `MinimumRange`.
- `StartingCooldown`: When the Character/Item/Trap/Status is Generated, the Action will have an initial cooldown and cannot be used as soon as possible.
- `CooldownBetweenUses`: Minimum amount of turns between each time the Action is performed. If 0 or 1, the Action effectively has no cooldown whatsoever.
- `MaximumUses`: The maximum amount of uses until the Action becomes permanently unusable. If 0, the Action can be used as many times as possible.
- `Effect`:

#### Effect

The Effect represents the function itself. Once the Character or the NPC picks an Action, the list of Effects in the `Effect` field will be called.

Its fields are:
- `EffectName`: This is the name of the function that will be executed when the Effect is called. The current list of functions will be shown later.
- `Params`: A set of arrays of `ParamName` and `Value` that the function needs to execute. `Value` can use fields from the `This`, `Source` and `Target` objects from the Action. It's case-insensitive. Mathematical formulas can be used in `Value`.
- `Then`: The Effect that will be called regardless of the outcome of the current Effect.
- `OnSuccess`: The Effect that will be called if the current Effect reports a success by returning `true` (e.g. if an attack hits).
- `OnFailure`: The Effect that will be called if the current Effect reports a failure by returning `false` (e.g. if an attack misses or deals no damage).

You can chain as many Effects as you want thanks to the `Then`, `OnSuccess` and `OnFailure` fields. If you want the chain to end in either of them, set the value to `null`.

**NOTE:** `OnSuccess` and `OnFailure` must be together. You can't have one without the other.
**NOTE:** If there's both a `Then` and an `OnSuccess`/`OnFailure` in the same Effect, the latter will be ignored, and `Then` will be called instead.

The dynamic parameters that can be used for an Effect are as follows:
- `{this}`, `{source}` and `{target}` return the object's Name.
- `{this.[FIELD]}`, `{source.[FIELD]}` and `{target.[FIELD]}` can return fields from any of the objects the Action is tied to:
    - For Characters:
        - `{[CHARACTER].Weapon}`: Returns the Character's Weapon.
        - `{[CHARACTER].Armor}`: Returns the Character's Armor.
        - `{[CHARACTER].HP}`: Returns the Character's HP.
        - `{[CHARACTER].MaxHP}`: Returns the Character's Max HP.
        - `{[CHARACTER].Attack}`: Returns the Character's Atack.
        - `{[CHARACTER].Damage}`: Returns the Character's Damage.
        - `{[CHARACTER].Defense}`: Returns the Character's Defense.
        - `{[CHARACTER].Mitigation}`: Returns the Character's Mitigation.
        - `{[CHARACTER].Movement}`: Returns the Character's Movement.
        - `{[CHARACTER].HPRegeneration}`: Returns the Character's HPRegeneration.
        - `{[CHARACTER].ExperiencePayout}`: Returns the Character's Experience Payout (after formula calculation).
    - For Items and Traps:
        - `{[ITEM].Owner}`: Returns the Name of the Item's current owner. Can be used to obtain the name of who's currently using an Item they have in the inventory, for example.
        - `{[ITEM].Power}`: Returns the Item's Power.
    - For Altered Statuses:
        - `{[ITEM].TurnLength}`: Returns how many turns the Altered Status lasts.
        - `{[ITEM].Power}`: Returns the Altered Status's Power.

These parameters can be used in any `Value` for any Effect, as long as it's of a compatible type.

### List of currently-available functions with effects

#### DealDamage

**Description**:

`Source` deals damage to someone else using `This`.

**Required Parameters**:
- `Damage`: A formula of the Attack Power used to deal damage. It's an integer (non-integer values will be truncated).
- `Mitigation`: A formula for the Power used to reduce the damage dealt. It's an integer (non-integer values will be truncated).
- `Target`: Who is going to take damage. Must be a Character. Generally will be the same object as in `Target`.
- `Accuracy`: The odds (1 to 100) for the attack to hit. If 100 or higher, it will always hit.

**Optional Parameters**: None

**Result**:

`Target` will take damage equal to `Damage - Mitigation` if the attack hits. If this becomes 0 or lower, `Target` does not take damage.

Returns `true` (Success) when the attack hits and `Damage - Mitigation > 0`. Returns `false` (Failure) if the attack misses, or if it hits but `Damage - Mitigation <= 0`.

#### ReplaceConsoleRepresentation

**Description**:

`Source` changes its Console Representation Character, Color, or both.

**Required Parameters**: None, but at least one of the Optional Parameters is required.

**Optional Parameters**:
- `Character`: The displayCharacter in which the Console Representation will turn into.
- `Color`: The ForegroundColor in which the Console Representation will turn into. It's written in this order: `{Red,Green,Blue,Alpha}`.

**Result**:

`Source` will change its Console Representation's `Character` and `ForegroundColor` to the Effect's `Character` and `Color`, if present.

Always returns `true` (Success).

#### PrintText

**Description**:

Will print some text into the Message Log.

**Required Parameters**:
- `Text`: The text to print into the Message Log.

**Optional Parameters**: None

**Result**:

If the Player can see either `Source` or `Target`, `Text` will be printed into the Message Log.

Always returns `true` (Success).

#### MessageBox

**Description**:

Produce a Message Box for the game Client.

**Required Parameters**:
- `Title`: The Message Box's title.
- `Text`: The Message Box's inner text:
- `Color`: The Message Box's border and title box color. It's written in this order: `{Red,Green,Blue,Alpha}`.

**Optional Parameters**: None

**Result**:

Before the player's next turn, a Message Box will pop up in their Client.

Always returns `true` (Success).

#### GiveExperience

**Description**:

`Source` gives `Target` some experience points, if applicable.

**Required Parameters**:
- `Target`: Who is going to recieve experience points. Normally the same object as in `Target`.
- `Amount`: How many experience points `Target` will receive. Must be higher than 0.

**Optional Parameters**: None

**Result**:

`Target` will receive `Amount` experience points, unless it cannot gain any experience (either because it can never get them, or it's already at Max Level).

Returns `true` (Success) if `Target` got any experience points. Returns `false` (Failure) otherwise.

##### ApplyAlteredStatus

**Description**:

`Source` attempts to cause an altered status to `Target`.

**Required Parameters**:
- `Id`: The Altered Status's `ClassId`.
- `Target`: Who is going to recieve the Altered Status. Normally the same object as in `Target`.
- `Chance`: The odds (1 to 100) for the Altered Status to be applied. If 100 or higher, it will always be applied.
- `Power`: Sets the Altered Status's `Power` value.
- `TurnLength`: The amount of turns the Altered Status will last. If lower than 0, it will last forever until Cleansed.

**Optional Parameters**: None

**Result**:

`Target` will be inflicted the Altered Status, if it succeeds. It can fail if the Altered Status's `CanStack` and `CanOverwrite` are both at `false` and `Target` is already inflicted by this status.

Returns `true` (Success) if the Altered Status is applied. Returns `false` (Failure) otherwise.

#### CleanseAlteredStatus

**Description**:

`Source` attempts to remove one of `Target`'s Altered Statuses.

**Required Parameters**:
- `Id`: The Altered Status's `ClassId`.
- `Target`: Who is going to lose the Altered Status. Normally the same object as in `Target`.
- `Chance`: The odds (1 to 100) for the Altered Status to be removed. If 100 or higher, it will always be removed.

**Optional Parameters**: None

**Result**:

`Target` will lose the Altered Status, if it succeeds. If there's a Stat Alteration that is tied to the Altered Status, the Stat Alteration will be removed as well.

Returns `true` (Success) if the Altered Status is removed. Returns `false` (Failure) otherwise.

#### CleanseAllAlteredStatuses

**Description**:

`Source` attempts to remove all of `Target`'s Altered Statuses.

**Required Parameters**:
- `Target`: Who is going to lose the Altered Status. Normally the same object as in `Target`.
- `Chance`: The odds (1 to 100) for the Altered Status to be removed. If 100 or higher, it will always be removed.

**Optional Parameters**: None

**Result**:

`Target` will lose all Altered Statuses, if it succeeds. If there's a Stat Alteration that is tied to any of the Altered Statuses, the Stat Alteration will be removed as well.

Returns `true` (Success) if all Altered Statuses are removed. Returns `false` (Failure) otherwise.

#### ApplyStatAlteration

**Description**:

`Source` attempts to modify one of `Target`'s Stats.

**Required Parameters**:
- `Id`: The Stat Alteration's identifier. It's used to inform of the alteration's presence in the Player Info screen, or to notify in the Message Log when it expires.
- `Target`: Who is going to recieve the Stat Alteration. Normally the same object as in `Target`.
- `Stat`: The name of the Stat that is going to be altered (Max HP, Attack, Defense, Movement or HP Regeneration). The name is just as with the dynamic parameters (case-insensitive and without spaces).
- `Chance`: The odds (1 to 100) for the Stat Alteration to be applied. If 100 or higher, it will always be applied.
- `Amount`: Sets how much of the Stat will be modified.
- `TurnLength`: The amount of turns the Stat Alteration will last. If lower than 0, it will last forever until Cleansed.
- `CanBeStacked`: If `true`, multiple copies of the same Stat Alteration can be present.

**Optional Parameters**: None

**Result**:

`Target` will be inflicted the Stat Alteration, if it succeeds. It can fail if `Target` already has a Stat Alteration with the same `Id` and `CanBeStacked` is `false`.

Returns `true` (Success) if the Stat Alteration is applied. Returns `false` (Failure) otherwise.

#### CleanseStatAlteration

**Description**:

`Source` attempts to remove all Stat Alterations on one of `Target`'s Stats.

**Required Parameters**:
- `Stat`: The name of the Stat whose alterations will try to be removed.
- `Chance`: The odds (1 to 100) for the Stat's Alterations to be removed. If 100 or higher, they will always be removed.

**Optional Parameters**: None

**Result**:

`Target` will lose all the alterations on their `Stat`, if it succeeds. If any of the stat alterations are tied to an Altered Status, the Altered Status will be removed as well.

Returns `true` (Success) if `Stat`'s Alterations are removed. Returns `false` (Failure) otherwise.

#### CleanseStatAlterations

**Description**:

`Source` attempts to remove all Stat Alterations on all of `Target`'s Stats.

**Required Parameters**:
- `Chance`: The odds (1 to 100) for the Stat Alterations to be removed. If 100 or higher, they will always be removed.

**Optional Parameters**: None

**Result**:

`Target` will lose all the alterations on their stats, if it succeeds. If any of the stat alterations are tied to an Altered Status, the Altered Status will be removed as well.

Returns `true` (Success) if all Stat Alterations are removed. Returns `false` (Failure) otherwise.

#### GenerateStairs

**Description**:

`Source` attempts to generate the Floor's Stairs.

**Required Parameters**: None.

**Optional Parameters**: None

**Result**:

If the Floor's Stairs weren't present, they will be generated on a valid location

Returns `true` (Success) if there weren't any Stairs before calling this function. Returns `false` (Failure) otherwise.

#### HealDamage

**Description**:

`Source` will use `This` to heal some of `Target`'s lost HP.

**Required Parameters**:
- `Source`: Who is going to heal. Generally will be the same object as in `Target`.
- `Target`: Who is going to get healed. Must be a Character. Generally will be the same object as in `Target`.
- `Power`: The amount of damage that is trying to be healed. Can be written in Dice Notation.

**Optional Parameters**: None

**Result**:

`Target` will take heal equal to `Power`. If the healing would take them over `MaxHP`, it will only heal up to `MaxHP` instead.

Returns `true` (Success) when `Target`'s `HP` was lower than their `MaxHP` upon trying to heal. Returns `false` (Failure) otherwise.

#### Equip

**Description**:

`Target` will try to equip `This`.

**Required Parameters**: None

**Optional Parameters**: None

**Result**:

`Target` will now have `This` equipped. If there was another Item in their equipped slot (that was neither the `StartingWeapon` nor the `StartingArmor`), the previous Item will be placed where `This` used to be (either the floor or the inventory).

**Throws an error if `This` is not a `Weapon` or `Armor`, or if `Target` is not a Character.**

Always returns `true` (Success).

#### Remove

**Description**:

`This` will try to remove `Target`

**Required Parameters**:
- `Target`: What is going to get removed. Must be an Item. Generally will be the same object as in `Target`.
- `Chance`: The odds (1 to 100) for `Target` to be removed. If 100 or higher, they will always be removed.

**Optional Parameters**: None

**Result**:

`Target` will disappear from the game, if it succeeds.

**Throws an error if `Target` is not an Item.**

Returns `true` (Success) if `Target` is removed. Returns `false` (Failure) otherwise.

[^1] A "Dummy Room" is a room that only contains one tile. They look identical to Hallways.

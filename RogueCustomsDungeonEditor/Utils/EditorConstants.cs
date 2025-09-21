using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class EditorConstants
    {
        public static readonly List<string> ReservedWords = ["<<CUSTOM>>", "NO DROP", "EQUIPPABLE", "NONE", EngineConstants.SPAWN_ANY_ALLIED_CHARACTER, EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER, EngineConstants.SPAWN_ANY_CHARACTER, EngineConstants.SPAWN_ANY_NEUTRAL_CHARACTER, EngineConstants.SPAWN_ANY_TRAP, EngineConstants.SPAWN_ANY_ENEMY_CHARACTER, EngineConstants.SPAWN_ANY_ITEM, EngineConstants.CREATE_WAYPOINT, "Use", "Equip", "Nothing", "Damage", "Mitigation", "UsePower", "HP", "MP", "MaxHP", "MaxMP", "Hunger", "MaxHunger", "Accuracy", "Evasion", "Movement", "Attack", "Defense", "HPRegeneration", "MPRegeneration"];
        public static readonly List<string> PartialReservedWords = ["CURRENCY"];
    }
}

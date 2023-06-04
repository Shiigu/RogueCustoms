using RoguelikeGameEngine.Utils.JsonImports;
using System.Collections.Generic;

namespace RoguelikeGameEngine.Game.DungeonStructure
{
    public class Faction
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;

        public readonly List<string> AlliedWithIds;
        public readonly List<string> NeutralWithIds;
        public readonly List<string> EnemiesWithIds;

        public readonly List<Faction> AlliedWith;
        public readonly List<Faction> NeutralWith;
        public readonly List<Faction> EnemiesWith;

        public Faction(FactionInfo factionInfo, Locale Locale)
        {
            Id = factionInfo.Id;
            Name = Locale[factionInfo.Name];
            Description = Locale[factionInfo.Description];
            AlliedWithIds = factionInfo.AlliedWith;
            NeutralWithIds = factionInfo.NeutralWith;
            EnemiesWithIds = factionInfo.EnemiesWith;
            AlliedWith = new List<Faction>();
            NeutralWith = new List<Faction>();
            EnemiesWith = new List<Faction>();
        }
    }
}

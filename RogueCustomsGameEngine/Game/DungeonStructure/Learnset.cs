using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class Learnset
    {
        public string Id { get; set; }
        public List<LearnsetEntry> Entries { get; set; }

        public Learnset(LearnsetInfo info)
        {
            Entries = new List<LearnsetEntry>();
            Id = info.Id;
            foreach (var entryInfo in info.Entries)
            {
                var entry = new LearnsetEntry()
                {
                    Level = entryInfo.Level,
                    ScriptToLearnId = entryInfo.LearnedScriptId,
                    ScriptToForget = entryInfo.ForgotScriptId
                };
                Entries.Add(entry);
            }
        }

        public void MapScriptsToLearn(List<ActionWithEffects> scripts)
        {
            foreach (var entry in Entries)
            {
                if (!string.IsNullOrEmpty(entry.ScriptToLearnId))
                {
                    entry.ScriptToLearn = scripts.FirstOrDefault(s => s.Id == entry.ScriptToLearnId)
                        ?? throw new FormatException($"No Script with id {entry.ScriptToLearnId} was found.");
                }
            }
        }
    }

    [Serializable]
    public class LearnsetEntry
    {
        public int Level { get; set; }
        public string ScriptToLearnId { get; set; }
        public ActionWithEffects ScriptToLearn { get; set; }
        public string ScriptToForget { get; set; }
    }
}

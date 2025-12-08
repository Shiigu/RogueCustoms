using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonLearnsetValidator
    {
        public static async Task<DungeonValidationMessages> Validate(LearnsetInfo learnsetJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();
            var validScriptIds = dungeonJson.Scripts.ConvertAll(s => s.Id.ToLowerInvariant());
            
            if (!string.IsNullOrWhiteSpace(learnsetJson.Id))
            {
                if(!dungeonJson.PlayerClasses.Any(pc => pc.Learnset.Equals(learnsetJson.Id)) && !dungeonJson.NPCs.Any(npc => npc.Learnset.Equals(learnsetJson.Id)))
                    messages.AddError($"Learnset is not being used by any Character at present.");

                var learnsetScripts = new List<string>();
                
                foreach(var entry in learnsetJson.Entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.LearnedScriptId))
                    {
                        messages.AddError($"Learnset is set not learn a Script at level {entry.Level}.");
                    }
                    else if (!dungeonJson.Scripts.Any(s => s.Id.Equals(entry.LearnedScriptId)))
                    {
                        messages.AddError($"Learnset is set to learn {entry.LearnedScriptId}, a non-existing Script, at level {entry.Level}.");
                    }

                    if (!string.IsNullOrWhiteSpace(entry.ForgotScriptId) && !dungeonJson.Scripts.Any(s => s.Id.Equals(entry.ForgotScriptId)))
                    {
                        messages.AddError($"Learnset is set to forget {entry.ForgotScriptId}, a non-existing Script, at level {entry.Level}.");
                    }

                    if (learnsetScripts.Contains(entry.LearnedScriptId))
                    {
                        messages.AddError($"Learnset is set to learn {entry.LearnedScriptId} at level {entry.Level}, which was already learned.");
                    }
                    else
                    {
                        learnsetScripts.Add(entry.LearnedScriptId);
                    }

                    if (!string.IsNullOrWhiteSpace(entry.ForgotScriptId) && !learnsetScripts.Contains(entry.ForgotScriptId))
                    {
                        messages.AddError($"Learnset is set to forget {entry.ForgotScriptId} at level {entry.Level}, which hasn't been learned yet.");
                    }
                    else if (!string.IsNullOrWhiteSpace(entry.ForgotScriptId))
                    {
                        learnsetScripts.Remove(entry.ForgotScriptId);
                    }
                }
            }
            else
            {
                messages.AddError($"Learnset #{dungeonJson.LearnsetInfos.IndexOf(learnsetJson)} lacks an Id.");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}

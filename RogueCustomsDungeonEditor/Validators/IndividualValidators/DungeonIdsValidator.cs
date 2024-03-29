﻿using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonIdsValidator
    {
        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            var idDictionary = new Dictionary<string, List<string>>();

            foreach (var faction in dungeonJson.FactionInfos)
            {
                if(!idDictionary.ContainsKey(faction.Id))
                {
                    idDictionary[faction.Id] = new List<string> { faction.Name };
                }
                else
                {
                    idDictionary[faction.Id].Add(faction.Name);
                }
            }

            UpdateIdDictionaryWith(idDictionary, dungeonJson.NPCs.ConvertAll(npc => (npc.Id, npc.Name)));
            UpdateIdDictionaryWith(idDictionary, dungeonJson.Items.ConvertAll(i => (i.Id, i.Name)));
            UpdateIdDictionaryWith(idDictionary, dungeonJson.Traps.ConvertAll(t => (t.Id, t.Name)));
            UpdateIdDictionaryWith(idDictionary, dungeonJson.AlteredStatuses.ConvertAll(als => (als.Id, als.Name)));

            foreach (var key in idDictionary.Keys)
            {
                if (idDictionary[key].Count > 1)
                {
                    messages.Add($"{key} is a duplicate Id for {idDictionary[key].JoinAnd()}", DungeonValidationMessageType.Error);
                }
            }

            if (!dungeonJson.PlayerClasses.Any())
                messages.AddError("Dungeon does not have any Player classes.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }

        private static void UpdateIdDictionaryWith(Dictionary<string, List<string>> idDictionary, List<(string Id, string Name)> classes)
        {
            foreach (var @class in classes)
            {
                if (!idDictionary.ContainsKey(@class.Id))
                {
                    idDictionary[@class.Id] = new List<string> { @class.Name };
                }
                else
                {
                    idDictionary[@class.Id].Add(@class.Name);
                }
            }
        }
    }
}

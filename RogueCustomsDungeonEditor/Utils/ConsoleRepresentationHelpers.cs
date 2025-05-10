
using RogueCustomsDungeonEditor.Validators;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ConsoleRepresentationHelpers
    {
        public static DungeonValidationMessages Validate(this ConsoleRepresentation representation, string ownerClassId, bool ownerIsPlayerClass, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            if (representation.Character is '\0' or ' ')
                messages.AddWarning("Console Representation lacks a visible Character. If the object is supposed to be printed to console, consider changing it.");
            if (representation.ForegroundColor == representation.BackgroundColor)
                messages.AddWarning("Console Representation's background and foreground are of the same color. Its Character won't be visible to the player.");
            if (!representation.Character.ToString().CanBeEncoded())
                messages.AddWarning("Console Representation cannot be properly encoded to PxPlus. Console clients may display it incorrectly.");

            if(!ownerIsPlayerClass)
                messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.PlayerClasses.ConvertAll(pc => (pc.Id, pc.ConsoleRepresentation))));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.NPCs.ConvertAll(npc => (npc.Id, npc.ConsoleRepresentation))));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.Items.ConvertAll(i => (i.Id, i.ConsoleRepresentation))));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.Traps.ConvertAll(t => (t.Id, t.ConsoleRepresentation))));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.AlteredStatuses.ConvertAll(als => (als.Id, als.ConsoleRepresentation))));

            return messages;
        }
        public static DungeonValidationMessages ValidateStandalone(this ConsoleRepresentation representation, string name, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            if (representation.Character is '\0' or ' ')
                messages.AddWarning($"{name}'s Console Representation lacks a visible Character. If the object is supposed to be printed to console, consider changing it.");
            if (representation.ForegroundColor == representation.BackgroundColor)
                messages.AddWarning($"{name}'s Console Representation's background and foreground are of the same color. Its Character won't be visible to the player.");
            if (!representation.Character.ToString().CanBeEncoded())
                messages.AddWarning($"{name}'s Console Representation cannot be properly encoded to PxPlus. Console clients may display it incorrectly.");
            

            return messages;
        }

        private static DungeonValidationMessages CheckIdenticalRepresentations(this ConsoleRepresentation representation, string ownerClassId, List<(string Id, ConsoleRepresentation ConsoleRepresentation)> classes)
        {
            var messages = new DungeonValidationMessages();

            foreach (var classInfo in classes)
            {
                if(classInfo.Id != ownerClassId && classInfo.ConsoleRepresentation?.Equals(representation) == true)
                    messages.AddWarning($"Console Representation is identical to that of {classInfo.Id}. This might cause confusion to players.");
            }

            return messages;
        }
        public static List<string> Validate(this ConsoleRepresentation consoleRepresentation, string objectName)
        {
            var errorMessages = new List<string>();

            if (consoleRepresentation.Character == '\0')
                errorMessages.Add($"{objectName} does not have a Console Representation character.");

            if (consoleRepresentation.ForegroundColor == null)
                errorMessages.Add($"{objectName} does not have a Console Representation Foreground Color.");

            if (consoleRepresentation.BackgroundColor == null)
                errorMessages.Add($"{objectName} does not have a Console Representation Foreground Color.");

            return errorMessages;
        }
    }
}

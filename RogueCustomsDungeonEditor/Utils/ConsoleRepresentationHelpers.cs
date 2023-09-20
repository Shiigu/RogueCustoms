
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
            if (!representation.Character.ToString().CanBeEncodedToIBM437())
                messages.AddWarning("Console Representation cannot be properly encoded to IBM437. Console clients may display it incorrectly.");

            if(!ownerIsPlayerClass)
                messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.PlayerClasses));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.NPCs));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.Items));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.Traps));
            messages.AddRange(representation.CheckIdenticalRepresentations(ownerClassId, dungeonJson.AlteredStatuses));

            return messages;
        }

        private static DungeonValidationMessages CheckIdenticalRepresentations(this ConsoleRepresentation representation, string ownerClassId, List<ClassInfo> classes)
        {
            var messages = new DungeonValidationMessages();

            foreach (var classInfo in classes)
            {
                if(classInfo.Id != ownerClassId && classInfo.ConsoleRepresentation?.Equals(representation) == true)
                    messages.AddWarning($"Console Representation is identical to that of {classInfo.Id}. This might cause confusion to players.");
            }

            return messages;
        }
    }
}

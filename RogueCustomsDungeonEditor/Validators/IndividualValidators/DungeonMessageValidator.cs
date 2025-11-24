using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonMessageValidator
    {
        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();
            var welcomeMessage = dungeonJson.WelcomeMessage;
            var endingMessage = dungeonJson.EndingMessage;

            messages.AddRange(dungeonJson.ValidateString(welcomeMessage, "Dungeon", "Welcome Message", true));
            messages.AddRange(dungeonJson.ValidateString(endingMessage, "Dungeon", "Ending Message", true));

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}

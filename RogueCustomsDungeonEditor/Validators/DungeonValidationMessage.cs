using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsDungeonEditor.Validators
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class DungeonValidationMessages
    {
        public List<DungeonValidationMessage> ValidationMessages { get; set; } = new List<DungeonValidationMessage>();
        public void Add(string message, DungeonValidationMessageType type)
        {
            ValidationMessages.Add(new DungeonValidationMessage
            {
                Message = message,
                Type = type
            });
        }
        public void AddSuccess(string message)
        {
            ValidationMessages.Add(new DungeonValidationMessage
            {
                Message = message,
                Type = DungeonValidationMessageType.Success
            });
        }
        public void AddError(string message)
        {
            ValidationMessages.Add(new DungeonValidationMessage
            {
                Message = message,
                Type = DungeonValidationMessageType.Error
            });
        }
        public void AddWarning(string message)
        {
            ValidationMessages.Add(new DungeonValidationMessage
            {
                Message = message,
                Type = DungeonValidationMessageType.Warning
            });
        }

        public void AddRange(DungeonValidationMessages messages)
        {
            ValidationMessages.AddRange(messages.ValidationMessages);
        }
        public void AddRange(List<DungeonValidationMessage> messages)
        {
            ValidationMessages.AddRange(messages);
        }

        public bool HasWarnings => ValidationMessages.Exists(vm => vm.Type == DungeonValidationMessageType.Warning);
        public bool HasErrors => ValidationMessages.Exists(vm => vm.Type == DungeonValidationMessageType.Error);

        public bool Any()
        {
            return ValidationMessages.Any();
        }
    }

    public class DungeonValidationMessage
    {
        public string Message { get; set; }
        public DungeonValidationMessageType Type { get; set; }
    }

    public enum DungeonValidationMessageType
    {
        Info,
        Success,
        Error,
        Warning
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

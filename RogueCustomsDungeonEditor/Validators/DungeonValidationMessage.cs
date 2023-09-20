using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators
{
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

        public bool HasWarnings => ValidationMessages.Any(vm => vm.Type == DungeonValidationMessageType.Warning);
        public bool HasErrors => ValidationMessages.Any(vm => vm.Type == DungeonValidationMessageType.Error);

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
}

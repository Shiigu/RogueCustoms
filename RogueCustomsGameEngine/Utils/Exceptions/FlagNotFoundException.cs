using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Exceptions
{
    [Serializable]
    public class FlagNotFoundException : Exception
    {
        public string FlagName { get; set; }
        public FlagNotFoundException()
        { }

        public FlagNotFoundException(string message)
            : base(message)
        { }

        public FlagNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public FlagNotFoundException(string message, string flagName)
            : base(message)
        { 
            FlagName = flagName;
        }
    }
}

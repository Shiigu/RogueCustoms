using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V14
{
    [Serializable]
    public class PlayerClassInfoV14 : CharacterInfoV14
    {
        public bool RequiresNamePrompt { get; set; }
    }
}

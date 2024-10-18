using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V14
{
    [Serializable]
    public class NPCInfoV14 : CharacterInfoV14
    {
        public bool KnowsAllCharacterPositions { get; set; }
        public string AIType { get; set; }
        public int AIOddsToUseActionsOnSelf { get; set; }
        public ActionWithEffectsInfo OnSpawn { get; set; } = new ActionWithEffectsInfo();
        public List<ActionWithEffectsInfo> OnInteracted { get; set; } = new List<ActionWithEffectsInfo>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    [Serializable]
    public class DisplayEventDto
    {
        public DisplayEventType DisplayEventType { get; set; }
        public List<object> Params { get; set; }
    }
    
    [Serializable]
    public enum DisplayEventType
    {
        PlaySpecialEffect,
        AddLogMessage,
        ClearLogMessages,
        AddMessageBox,
        UpdateTileRepresentation,
        SetDungeonStatus,
        SetOnStairs,
        SetCanMove,
        SetCanAct,
        UpdatePlayerData,
        UpdatePlayerPosition,
        UpdateExperienceBar,
        TriggerPrompt
    }

    [Serializable]
    public enum UpdatePlayerDataType
    {
        UpdateConsoleRepresentation,
        ModifyStat,
        ModifyMaxStat,
        ModifyEquippedItem,
        UpdateAlteredStatuses,
        UpdateInventory
    }
}

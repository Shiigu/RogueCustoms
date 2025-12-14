using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGodotClient.Utils
{
    public enum MessageScreenType
    {
        Briefing,
        Ending,
        Error
    }

    public enum ControlMode
    {
        NormalMove,
        NormalOnStairs,
        Immobilized,
        ImmobilizedOnStairs,
        MustSkipTurn,
        Targeting,
        Waiting,
        PreMoveHighlight,
        None
    }

    public enum SelectionMode
    {
        Interact,
        Inventory,
        Quest,
        SelectAction,
        SelectItem,
        Buy,
        Sell
    }

    public enum SortActionMode
    {
        Default,
        UsableActionsFirst,
        CursorOnFirstUsableAction
    }

    public enum FlashEffectMode
    {
        FullScreen,
        MapSection,
        Hide
    }

    public enum InactiveControlShowMode
    {
        Hide,
        Dim
    }
}

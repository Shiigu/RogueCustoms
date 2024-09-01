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
        None
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace RogueCustomsGodotClient.Screens.GameSubScreens
{
    public abstract partial class GamePanel : Panel
    {
        public GameScreen Parent => GetParent<GameScreen>();
        public abstract void Update();
    }
}

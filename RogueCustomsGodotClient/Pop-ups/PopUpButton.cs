using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace RogueCustomsGodotClient.Popups
{
    public class PopUpButton
    {
        public string Text { get; set; }
        public Action Callback { get; set; }
        public string ActionPress { get; set; }
        public Button AssociatedButton { get; private set; }

        public void AssociateButton(Button button) => AssociatedButton = button;
    }
}

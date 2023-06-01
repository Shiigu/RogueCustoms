using SadConsole;
using SadRogue.Primitives;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.UI.Consoles.Utils;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole
{
    public class MessageLogConsole : GameSubConsole
    {
        private readonly ScrollableMessageSubConsole ScrollableMessageLogSubConsole;
        public MessageLogConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.MessageLogCellWidth, GameConsoleConstants.MessageLogCellHeight)
        {
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = true;
            ScrollableMessageLogSubConsole = new ScrollableMessageSubConsole(Width - 3, Height - 2, 200)
            {
                Position = new Point(1, 1)
            };
            Children.Add(ScrollableMessageLogSubConsole);
        }

        public override void Render(TimeSpan delta)
        {
            this.Clear();

            var dungeonStatus = ParentContainer.LatestDungeonStatus;

            var square = new Rectangle(0, 0, Width, Height);
            var title = " MESSAGES ";

            this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 178)));
            this.Print((square.Width - title.Length) / 2, 0, title);

            if (ParentContainer.RequiresRefreshingDungeonState)
            {
                ScrollableMessageLogSubConsole.PrintList(dungeonStatus.LogMessages);
            }

            base.Render(delta);
        }
    }
}

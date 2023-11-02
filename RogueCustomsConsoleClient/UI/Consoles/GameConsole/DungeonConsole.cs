using SadConsole;
using SadConsole.Effects;
using RogueCustomsConsoleClient.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using SadRogue.Primitives;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.Resources.Localization;
using System;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public class DungeonConsole : GameSubConsole
    {
        private readonly BicolorBlink SelectionBlink;
        public (int X, int Y) CursorLocation { get; set; }
        public bool WithCursor { get; set; }
        private bool UpdateCursorDisplay { get; set; }

        private (int X, int Y) LatestCursorLocation;
        public DungeonConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.MapCellWidth, GameConsoleConstants.MapCellHeight)
        {
            SelectionBlink = new BicolorBlink() { BlinkSpeed = TimeSpan.FromSeconds(0.075d), BlinkOutBackgroundColor = Color.White, BlinkOutForegroundColor = Color.White };
            Build();
        }

        public new void Build()
        {
            base.Build();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font");
            RefreshOnlyOnStatusUpdate = true;
            CursorLocation = default;
            LatestCursorLocation = default;
            WithCursor = false;
            UpdateCursorDisplay = false;
        }

        public void AddCursor()
        {
            var playerEntity = ParentContainer.LatestDungeonStatus.PlayerEntity;
            RefreshOnlyOnStatusUpdate = false;
            if (playerEntity != null)
            {
                WithCursor = true;
                CursorLocation = (playerEntity.X, playerEntity.Y);
                this.SetEffect(LatestCursorLocation.X, LatestCursorLocation.Y, SelectionBlink);
            }
        }

        public void MoveCursor(CoordinateInput input)
        {
            var dungeonStatus = ParentContainer.LatestDungeonStatus;
            (int X, int Y) newCoordinates = (CursorLocation.X + input.X, CursorLocation.Y + input.Y);

            if (newCoordinates.X < 0 || newCoordinates.X >= dungeonStatus.Width) return;
            if (newCoordinates.Y < 0 || newCoordinates.Y >= dungeonStatus.Height) return;

            var tile = dungeonStatus.GetTileFromCoordinates(newCoordinates.X, newCoordinates.Y);

            if (!tile.Targetable) return;

            LatestCursorLocation = CursorLocation;
            CursorLocation = newCoordinates;
        }

        public void RemoveCursor()
        {
            this.SetEffect(CursorLocation.X, CursorLocation.Y, null);
            this.SetEffect(LatestCursorLocation.X, LatestCursorLocation.Y, null);
            LatestCursorLocation = CursorLocation = default;
            RefreshOnlyOnStatusUpdate = true;
            WithCursor = false;
            UpdateCursorDisplay = false;
            Update(TimeSpan.Zero);
        }

        public override void Update(TimeSpan delta)
        {
            var dungeonStatus = ParentContainer.LatestDungeonStatus;
            if (dungeonStatus == null) return;

            UpdateCursorDisplay = WithCursor && !CursorLocation.Equals(LatestCursorLocation);

            if (WithCursor && !CursorLocation.Equals(LatestCursorLocation))
            {
                this.SetEffect(LatestCursorLocation.X, LatestCursorLocation.Y, null);
                LatestCursorLocation = default;
            }
            else if (WithCursor)
            {
                base.Update(delta);
                return;
            }

            if (dungeonStatus.DungeonStatus == DungeonStatus.Completed)
            {
                var message = BackendHandler.Instance.GetDungeonEndingMessage();

                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("TheEndMessageHeader"), message);

                ParentContainer.LatestDungeonStatus = null;
                base.Update(delta);
                return;
            }

            this.Clear();

            var square = new Rectangle(0, 0, Width, Height);

            this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 178)));
            var title = $" {dungeonStatus.FloorName} ";
            this.Print((square.Width - title.Length) / 2, 0, title);

            var playerEntity = dungeonStatus.PlayerEntity;
            if (playerEntity != null)
            {
                int minRetrieveX = 0, minRetrieveY = 0, minDisplayX = 1, minDisplayY = 1, widthToDisplay = 0, heightToDisplay = 0;

                int cursorX = CursorLocation != default ? CursorLocation.X : playerEntity.X;
                int cursorY = CursorLocation != default ? CursorLocation.Y : playerEntity.Y;

                if (dungeonStatus.Width == GameConsoleConstants.MapDisplayWidth)
                {
                    minRetrieveX = 0;
                    minDisplayX = 1;
                    widthToDisplay = GameConsoleConstants.MapDisplayWidth;
                }
                else if (dungeonStatus.Width > GameConsoleConstants.MapDisplayWidth)
                {
                    minRetrieveX = Math.Max(0, cursorX - (GameConsoleConstants.MapDisplayWidth / 2));
                    if (minRetrieveX + GameConsoleConstants.MapDisplayWidth > dungeonStatus.Width)
                        minRetrieveX = dungeonStatus.Width - GameConsoleConstants.MapDisplayWidth;
                    minDisplayX = 1;
                    widthToDisplay = GameConsoleConstants.MapDisplayWidth;
                }
                else if (dungeonStatus.Width < GameConsoleConstants.MapDisplayWidth)
                {
                    minRetrieveX = 0;
                    minDisplayX = ((GameConsoleConstants.MapDisplayWidth - dungeonStatus.Width) / 2) + 1;
                    widthToDisplay = dungeonStatus.Width;
                }

                if (dungeonStatus.Height == GameConsoleConstants.MapDisplayHeight)
                {
                    minRetrieveY = 0;
                    minDisplayY = 1;
                    heightToDisplay = GameConsoleConstants.MapDisplayHeight;
                }
                else if (dungeonStatus.Height > GameConsoleConstants.MapDisplayHeight)
                {
                    minRetrieveY = Math.Max(0, cursorY - (GameConsoleConstants.MapDisplayHeight / 2));
                    if (minRetrieveY + GameConsoleConstants.MapDisplayHeight > dungeonStatus.Height)
                        minRetrieveY = dungeonStatus.Height - GameConsoleConstants.MapDisplayHeight;
                    minDisplayY = 1;
                    heightToDisplay = GameConsoleConstants.MapDisplayHeight;
                }
                else if (dungeonStatus.Height < GameConsoleConstants.MapDisplayHeight)
                {
                    minRetrieveY = 0;
                    minDisplayY = ((GameConsoleConstants.MapDisplayHeight - dungeonStatus.Height) / 2) + 1;
                    heightToDisplay = dungeonStatus.Height;
                }

                for (var y = 0; y < heightToDisplay; y++)
                {
                    for (var x = 0; x < widthToDisplay; x++)
                    {
                        var tileInCoordinates = dungeonStatus.GetTileConsoleRepresentationFromCoordinates(minRetrieveX + x, minRetrieveY + y);
                        var xScreenCoord = x + minDisplayX;
                        var yScreenCoord = y + minDisplayY;
                        var displayCell = new ColoredGlyph(tileInCoordinates.ForegroundColor.ToSadRogueColor(),
                                                           tileInCoordinates.BackgroundColor.ToSadRogueColor(),
                                                           tileInCoordinates.Character.ToGlyph());
                        this.SetGlyph(xScreenCoord, yScreenCoord, displayCell);
                        if (UpdateCursorDisplay && (x, y).Equals(CursorLocation))
                        {
                            LatestCursorLocation = CursorLocation;
                            this.SetEffect(LatestCursorLocation.X, LatestCursorLocation.Y, null);
                            this.SetEffect(xScreenCoord, yScreenCoord, SelectionBlink);
                        }
                    }
                }
            }

            base.Update(delta);
        }
    }
}

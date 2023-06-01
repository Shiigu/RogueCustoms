using SadConsole;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.Helpers;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.UI.Consoles.GameConsole.GameWindows;
using SadRogue.Primitives;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole
{
    public class PlayerInfoConsole : GameSubConsole
    {
        private const string DetailsButtonText = " DETAILS ";
        private readonly Button DetailsButton;

        public PlayerInfoConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.PlayerInfoCellWidth, GameConsoleConstants.PlayerInfoCellHeight)
        {
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = true;
            DetailsButton = new Button(DetailsButtonText.Length, 1)
            {
                Position = new Point((Width - DetailsButtonText.Length) / 2, 38),
                Text = DetailsButtonText
            };
            DetailsButton.Click += DetailsButton_Click;

            Controls.Add(DetailsButton);
        }

        private void DetailsButton_Click(object? sender, EventArgs e)
        {
            try
            {
                var playerInfo = BackendHandler.Instance.GetPlayerDetailInfo();
                ParentContainer.ActiveWindow = PlayerCharacterDetailWindow.Show(ParentContainer, playerInfo);
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, "ERROR", "OH NO!\nAn error has occured!\nGet ready to return to the main menu...");
            }
        }

        public override void Render(TimeSpan delta)
        {
            var dungeonStatus = ParentContainer.LatestDungeonStatus;

            var playerEntity = dungeonStatus.Entities.Find(e => e.IsPlayer);

            if (playerEntity != null)
            {
                this.Clear();

                var square = new Rectangle(0, 0, Width, Height);
                var title = " YOU ";

                this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 178)));
                this.Print((square.Width - title.Length) / 2, 0, title);

                this.Print((square.Width - playerEntity.Name.Length) / 2, 2, playerEntity.Name);

                var levelString = $"LEVEL {playerEntity.Level}";

                this.Print((square.Width - levelString.Length) / 2, 4, levelString);
                this.SetGlyph((square.Width - 1) / 2, 6, new ColoredGlyph(playerEntity.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), playerEntity.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), playerEntity.ConsoleRepresentation.Character.ToGlyph()));
                this.Print(2, 10, $"HP: {playerEntity.HP}/{playerEntity.MaxHP}");
                this.Print(2, 13, "WEAPON");
                this.Print(2, 14, $"{playerEntity.Weapon}");
                this.Print(2, 16, "Damage:");
                this.Print(2, 17, $"{playerEntity.Damage}");
                this.Print(2, 20, "ARMOR");
                this.Print(2, 21, $"{playerEntity.Armor}");
                this.Print(2, 23, "Mitigation:");
                this.Print(2, 24, $"{playerEntity.Mitigation}");
                this.Print(2, 26, $"Movement: {playerEntity.Movement}");
                this.Print(2, 28, "Statuses:");
                if(dungeonStatus.AlteredStatuses.Any())
                {
                    var baseColumnIndex = 2;
                    var baseRowIndex = 30;
                    var statusesPerIndex = 9;
                    foreach (var als in dungeonStatus.AlteredStatuses)
                    {
                        var index = dungeonStatus.AlteredStatuses.IndexOf(als);
                        if (baseRowIndex + (int)(index / statusesPerIndex) == DetailsButton.Position.Y - 2)
                        {
                            this.Print(2, baseRowIndex + (int)(index / statusesPerIndex), "AND MORE!");
                            break;
                        }
                        this.SetGlyph(baseColumnIndex + index % statusesPerIndex, baseRowIndex + (int)(index / statusesPerIndex), als.ConsoleRepresentation.Character.ToGlyph(), als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor());
                    }
                }
                else
                {
                    this.Print(4, 29, "NONE!");
                }
            }
            base.Render(delta);
        }
    }
}

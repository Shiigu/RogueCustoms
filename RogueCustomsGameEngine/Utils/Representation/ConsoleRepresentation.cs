using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace RogueCustomsGameEngine.Utils.Representation
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public sealed class ConsoleRepresentation : IEquatable<ConsoleRepresentation?>
    {
        [JsonConstructor]
        public ConsoleRepresentation() { }

        public char Character { get; set; }
        public GameColor BackgroundColor { get; set; } = new GameColor(Color.Black);
        public GameColor ForegroundColor { get; set; } = new GameColor(Color.White);

        public static ConsoleRepresentation EmptyTile { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is ConsoleRepresentation cr)
            {
                return Character == cr.Character
                    && BackgroundColor.Equals(cr.BackgroundColor)
                    && ForegroundColor.Equals(cr.ForegroundColor);
            }
            return false;
        }

        public bool Equals(ConsoleRepresentation? other)
        {
            return other is not null &&
                   Character == other.Character &&
                   EqualityComparer<GameColor>.Default.Equals(BackgroundColor, other.BackgroundColor) &&
                   EqualityComparer<GameColor>.Default.Equals(ForegroundColor, other.ForegroundColor);
        }

        public ConsoleRepresentation Clone()
        {
            return new ConsoleRepresentation
            {
                Character = Character,
                BackgroundColor = BackgroundColor,
                ForegroundColor = ForegroundColor
            };
        }

        public ConsoleRepresentation AsDarkened()
        {
            return new ConsoleRepresentation
            {
                Character = Character,
                BackgroundColor = BackgroundColor.AsDarkened(),
                ForegroundColor = ForegroundColor.AsDarkened()
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Character, BackgroundColor, ForegroundColor);
        }

        public static bool operator ==(ConsoleRepresentation? left, ConsoleRepresentation? right)
        {
            return EqualityComparer<ConsoleRepresentation>.Default.Equals(left, right);
        }

        public static bool operator !=(ConsoleRepresentation? left, ConsoleRepresentation? right)
        {
            return !(left == right);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

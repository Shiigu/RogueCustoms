using System.Runtime.CompilerServices;
using System;
using System.Runtime.Serialization;
using SadRogue.Primitives;

namespace SadConsole.Effects
{
    /// <summary>
    /// Blinks the foreground and background colors of a cell with the specified colors.
    /// </summary>
    [DataContract]
    public class BicolorBlink : CellEffectBase
    {
        private int _blinkCounter = 0;

        private bool _isOn;

        private TimeSpan _duration = TimeSpan.Zero;

        //
        // Resumen:
        //     How long it takes to transition from blinking in and out.
        [DataMember]
        public TimeSpan BlinkSpeed { get; set; }

        //
        // Resumen:
        //     The color the foreground blinks to.
        [DataMember]
        public Color BlinkOutForegroundColor { get; set; }

        //
        // Resumen:
        //     The color the background blinks to.
        [DataMember]
        public Color BlinkOutBackgroundColor { get; set; }

        //
        // Resumen:
        //     When true, ignores the SadConsole.Effects.Blinker.BlinkOutBackgroundColor and
        //     SadConsole.Effects.Blinker.BlinkOutForegroundColor colors and instead swaps the
        //     glyph's foreground and background colors.
        [DataMember]
        public bool SwapColorsFromCell { get; set; }

        //
        // Resumen:
        //     How many times to blink. The value of -1 represents forever.
        [DataMember]
        public int BlinkCount { get; set; }

        //
        // Resumen:
        //     The total duraction this effect will run for, before being flagged as finished.
        //     System.TimeSpan.MaxValue represents forever.
        [DataMember]
        public TimeSpan Duration { get; set; }

        //
        // Resumen:
        //     Creates a new instance of the blink effect.
        public BicolorBlink()
        {
            Duration = TimeSpan.MaxValue;
            BlinkCount = -1;
            BlinkSpeed = TimeSpan.FromSeconds(1.0);
            BlinkOutBackgroundColor = Color.Transparent;
            BlinkOutForegroundColor = Color.Transparent;
            _isOn = true;
            _blinkCounter = 0;
        }

        public override bool ApplyToCell(ColoredGlyphBase cell, ColoredGlyphBase originalState)
        {
            Color oldForeColor = cell.Foreground;
            Color oldBackColor = cell.Background;

            if (!_isOn)
            {
                if (SwapColorsFromCell)
                {
                    cell.Foreground = originalState.Background;
                    cell.Background = originalState.Foreground;
                }
                else
                {
                    cell.Foreground = BlinkOutForegroundColor;
                    cell.Background = BlinkOutBackgroundColor;
                }
            }
            else
            {
                cell.Foreground = originalState.Foreground;
                cell.Background = originalState.Background;
            }
            return cell.Foreground != oldForeColor || cell.Background != oldBackColor;
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            if (!_delayFinished || base.IsFinished)
            {
                return;
            }

            if (Duration != TimeSpan.MaxValue)
            {
                _duration += delta;
                if (_duration >= Duration)
                {
                    base.IsFinished = true;
                    return;
                }
            }

            if (_timeElapsed < BlinkSpeed)
            {
                return;
            }

            _isOn = !_isOn;
            _timeElapsed = TimeSpan.Zero;
            if (BlinkCount != -1)
            {
                _blinkCounter++;
                if (BlinkCount != -1 && _blinkCounter > BlinkCount * 2 + 1)
                {
                    base.IsFinished = true;
                }
            }
        }

        //
        // Resumen:
        //     Restarts the cell effect but does not reset it.
        public override void Restart()
        {
            _isOn = true;
            _blinkCounter = 0;
            _duration = TimeSpan.Zero;
            base.Restart();
        }

        public override ICellEffect Clone()
        {
            return new BicolorBlink
            {
                BlinkOutBackgroundColor = BlinkOutBackgroundColor,
                BlinkOutForegroundColor = BlinkOutForegroundColor,
                BlinkSpeed = BlinkSpeed,
                _isOn = _isOn,
                SwapColorsFromCell = SwapColorsFromCell,
                BlinkCount = BlinkCount,
                IsFinished = base.IsFinished,
                StartDelay = base.StartDelay,
                CloneOnAdd = base.CloneOnAdd,
                RemoveOnFinished = base.RemoveOnFinished,
                RestoreCellOnRemoved = base.RestoreCellOnRemoved,
                RunEffectOnApply = base.RunEffectOnApply,
                _timeElapsed = _timeElapsed
            };
        }

        public override string ToString()
        {
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 5);
            defaultInterpolatedStringHandler.AppendLiteral("BLINKER-");
            defaultInterpolatedStringHandler.AppendFormatted(BlinkOutBackgroundColor.PackedValue);
            defaultInterpolatedStringHandler.AppendLiteral("-");
            defaultInterpolatedStringHandler.AppendFormatted(BlinkOutForegroundColor.PackedValue);
            defaultInterpolatedStringHandler.AppendLiteral("-");
            defaultInterpolatedStringHandler.AppendFormatted(BlinkSpeed);
            defaultInterpolatedStringHandler.AppendLiteral("-");
            defaultInterpolatedStringHandler.AppendFormatted(SwapColorsFromCell);
            defaultInterpolatedStringHandler.AppendLiteral("-");
            defaultInterpolatedStringHandler.AppendFormatted(base.StartDelay);
            return defaultInterpolatedStringHandler.ToStringAndClear();
        }
    }
}

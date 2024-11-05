using System;

using Godot;

public partial class ScalableLabel : Label
{
    public int MinFontSize { get; set; }
    public int DefaultFontSize { get; set; }

    public override void _Ready()
    {
    }

    public new void SetText(string text)
    {
        Text = text;
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        AddThemeFontSizeOverride("normal_font_size", DefaultFontSize);

        var currentFontSize = 16;
        var maxWidth = Size.X - 16;
        var textWidth = currentFontSize * Text.Length;

        // Loop until the text fits or reaches the minimum font size
        while (textWidth > maxWidth && currentFontSize > MinFontSize)
        {
            currentFontSize -= 1;
            textWidth = currentFontSize * Text.Length;
        }

        Size = new(Math.Min(maxWidth, textWidth), 16);
        AddThemeFontSizeOverride("normal_font_size", currentFontSize);
    }
}
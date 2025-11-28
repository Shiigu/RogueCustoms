using System;
using System.Linq;

using Godot;

public partial class ScalableLabel : Label
{
    public int MinFontSize { get; set; }
    public int DefaultFontSize { get; set; }

    public override void _Ready()
    {
    }

    public override Vector2 _GetMinimumSize()
    {
        var font = GetThemeFont("font");
        var fontSize = GetThemeFontSize("normal_font_size");

        if (font == null || string.IsNullOrEmpty(Text))
            return new Vector2(0, 0);

        var textWidth = font.GetStringSize(Text, HorizontalAlignment.Left, -1, fontSize).X;
        var textHeight = font.GetHeight(fontSize);

        return new Vector2(textWidth, textHeight);
    }

    public new void SetText(string text)
    {
        Text = text;
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        var font = GetThemeFont("font");
        if (font == null)
            return;

        var textLines = Text.Split(new string[] { "[p]", "[/p]" }, StringSplitOptions.RemoveEmptyEntries);

        var longestLine = textLines.MaxBy(t => t.Length);

        var currentFontSize = DefaultFontSize;
        var maxWidth = Size.X - DefaultFontSize;
        var textWidth = font.GetStringSize(longestLine, HorizontalAlignment.Left, -1, currentFontSize).X;

        // Loop until the text fits or reaches the minimum font size
        while (textWidth > maxWidth && currentFontSize > MinFontSize)
        {
            currentFontSize--;
            textWidth = font.GetStringSize(longestLine, HorizontalAlignment.Left, -1, currentFontSize).X;
        }

        AddThemeFontSizeOverride("font_size", currentFontSize);
    }
}
using System;
using System.Linq;

using Godot;

using RogueCustomsGodotClient.Helpers;

public partial class ScalableRichTextLabel : RichTextLabel
{
    public int MinFontSize { get; set; }
    public int DefaultFontSize { get; set; }

    public override void _Ready()
    {
    }

    public override Vector2 _GetMinimumSize()
    {
        return new Vector2(Size.X, Size.Y);
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

        var textLines = Text.ToStringWithoutBbcode().Split(new string[] { "[p]", "[/p]" }, StringSplitOptions.RemoveEmptyEntries);

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

        AddThemeFontSizeOverride("normal_font_size", currentFontSize);
    }
}
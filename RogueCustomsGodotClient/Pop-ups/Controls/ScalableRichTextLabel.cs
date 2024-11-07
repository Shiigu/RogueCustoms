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

    public new void SetText(string text)
    {
        Text = text;
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        AddThemeFontSizeOverride("normal_font_size", DefaultFontSize);

        var textLines = Text.ToStringWithoutBbcode().Split(new string[] { "[p]", "[/p]" }, StringSplitOptions.RemoveEmptyEntries);

        var longestLine = textLines.MaxBy(t => t.Length);

        var currentFontSize = DefaultFontSize;
        var maxWidth = Size.X - DefaultFontSize;
        var textWidth = currentFontSize * longestLine.Length;

        // Loop until the text fits or reaches the minimum font size
        while (textWidth > maxWidth && currentFontSize > MinFontSize)
        {
            currentFontSize -= 1;
            textWidth = currentFontSize * longestLine.Length;
        }

        Size = new(Math.Min(maxWidth, textWidth), DefaultFontSize);
        AddThemeFontSizeOverride("normal_font_size", currentFontSize);
    }
}
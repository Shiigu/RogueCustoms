using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

public partial class LocalizableOptionButton : OptionButton
{
    [Export]
    public Godot.Collections.Array<string> TranslationKeys { get; set; }
        = [];

    public override void _Ready()
    {
        RefreshItems();

        TranslationServer.Singleton.Connect(
            "translation_changed",
            new Callable(this, nameof(OnLocaleChanged))
        );
    }
    private void OnLocaleChanged()
    {
        RefreshItems();
    }

    public void RefreshItems()
    {
        // Since OptionButtons don't refresh on localization, we use this method to do it manually.

        var previous = Selected;

        Clear();

        foreach (string key in TranslationKeys)
        {
            var translated = TranslationServer.Translate(key);
            AddItem(translated);
        }

        if (previous >= 0 && previous < ItemCount)
            Select(previous);
    }

    public void SetTranslationKeys(IEnumerable<string> keys)
    {
        TranslationKeys.Clear();
        foreach (var k in keys)
            TranslationKeys.Add(k);

        RefreshItems();
    }
}

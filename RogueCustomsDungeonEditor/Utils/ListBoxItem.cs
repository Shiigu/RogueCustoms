namespace RogueCustomsDungeonEditor.Utils
{
    public class ListBoxItem
    {
        public string Text { get; set; }
        public object Tag { get; set; }

        public override string ToString() => Text;
    }
}

using RoguelikeGameEngine.Utils.JsonImports;

namespace RoguelikeGameEngine.Utils.InputsAndOutputs
{
    public class DungeonListDto
    {
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        public override string ToString() => $"{Name} - by {Author}";

        public DungeonListDto() { }

        public DungeonListDto(string internalName, DungeonInfo info) {
            InternalName = internalName;
            Name = info.Name;
            Author = info.Author;
        }
    }
}

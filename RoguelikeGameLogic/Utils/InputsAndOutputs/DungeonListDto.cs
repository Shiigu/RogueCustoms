using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.JsonImports;

namespace RoguelikeGameEngine.Utils.InputsAndOutputs
{
    public class DungeonListDto
    {
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        public DungeonListDto() { }

        public DungeonListDto(string internalName, DungeonInfo info, string locale) {
            InternalName = internalName;
            Name = info.GetLocalizedName(locale);
            Author = info.Author;
        }
    }
}

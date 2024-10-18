using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using System.Collections.Generic;
using System.Xml.Linq;
using System;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    [Serializable]
    public class DungeonListDto
    {
        public string CurrentVersion { get; set; } = string.Empty;
        public List<DungeonPickDto> Dungeons { get; set; } = new List<DungeonPickDto>();

        public DungeonListDto() { }

        public DungeonListDto(string currentVersion)
        {
            CurrentVersion = currentVersion;
        }

        public void AddDungeonToList(string internalName, DungeonInfo info, string locale)
        {
            Dungeons.Add(new DungeonPickDto(internalName, info.GetLocalizedName(locale), info.Author, info.Version, !string.IsNullOrWhiteSpace(info.Version) && info.Version.Equals(EngineConstants.CurrentDungeonJsonVersion)));
        }
    }

    [Serializable]
    public class DungeonPickDto
    {
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public bool IsAtCurrentVersion { get; set; }

        public DungeonPickDto(string internalName, string name, string author, string version, bool isAtCurrentVersion)
        {
            InternalName = internalName;
            Name = name;
            Author = author;
            Version = (!string.IsNullOrWhiteSpace(version)) ? version : "0.9";
            IsAtCurrentVersion = isAtCurrentVersion;
        }
    }
}

namespace RoguelikeGameEngine.Utils.JsonImports
{
    [Serializable]
    public class DungeonInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string WelcomeMessage { get; set; }
        public string EndingMessage { get; set; }
        public int AmountOfFloors { get; set; }
        public List<FloorInfo> FloorInfos { get; set; }
        public List<FactionInfo> FactionInfos { get; set; }
        public List<ClassInfo> Characters { get; set; }
        public List<ClassInfo> Items { get; set; }
        public List<ClassInfo> Traps { get; set; }
        public List<ClassInfo> AlteredStatuses { get; set; }
    }
}

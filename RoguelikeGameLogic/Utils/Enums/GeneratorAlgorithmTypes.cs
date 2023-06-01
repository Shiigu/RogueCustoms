namespace RoguelikeGameEngine.Utils.Enums
{
    public class GeneratorAlgorithm
    {
        public GeneratorAlgorithmTypes Type { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }

    public enum GeneratorAlgorithmTypes
    {
        Standard = 1,
        OuterDummyRing = 2,
        InnerDummyRing = 3,
        OneBigRoom = 4,
        Dummy = 5
    }
}

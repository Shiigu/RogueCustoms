using System;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ElementInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GameColor Color { get; set; }
        public string ResistanceStatId { get; set; }
        public bool ExcessResistanceCausesHealDamage { get; set; }
        public ActionWithEffectsInfo OnAfterAttack { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

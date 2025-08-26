using System;
using System.Collections.Generic;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class Element
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GameColor Color { get; set; }
        public string ResistanceStatId { get; set; }
        public bool ExcessResistanceCausesHealDamage { get; set; }
        public ActionWithEffects OnAfterAttack { get; set; }

        public Element(ElementInfo element, Locale localeToUse, List<ActionSchool> actionSchools)
        {
            Id = element.Id;
            Name = localeToUse[element.Name];
            Color = element.Color;
            ResistanceStatId = element.ResistanceStatId;
            ExcessResistanceCausesHealDamage = element.ExcessResistanceCausesHealDamage;
            OnAfterAttack = ActionWithEffects.Create(element.OnAfterAttack, actionSchools);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

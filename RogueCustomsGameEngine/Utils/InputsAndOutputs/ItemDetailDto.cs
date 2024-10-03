using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ItemDetailDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PowerName { get; set; }
        public string Power { get; set; }
        public List<StatModificationDto> StatModifications { get; set; }
        public List<string> OnAttackActions { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public ItemDetailDto() { }

        public ItemDetailDto(Item i)
        {
            Name = i.Name;
            Description = i.Description;
            ConsoleRepresentation = i.ConsoleRepresentation;
            PowerName = i.EntityType switch
            {
                EntityType.Weapon => i.Map.Locale["CharacterDamageStat"],
                EntityType.Armor => i.Map.Locale["CharacterMitigationStat"],
                _ => string.Empty
            };
            Power = i.Power ?? string.Empty;
            StatModifications = new();
            i.StatModifiers.ForEach(m => {
                var correspondingStat = i.Owner.Stats.FirstOrDefault(s => s.Id.Equals(m.Id));
                if (correspondingStat != null)
                    StatModifications.Add(new StatModificationDto(m, correspondingStat, i.Map));
            });
            OnAttackActions = new();
            i.OwnOnAttack.ForEach(ooa => OnAttackActions.Add(ooa.Name));
        }
        public ItemDetailDto(EntityClass itemClass, Dungeon dungeon)
        {
            Name = dungeon.LocaleToUse[itemClass.Name];
            Description = dungeon.LocaleToUse[itemClass.Description];
            ConsoleRepresentation = itemClass.ConsoleRepresentation;
            PowerName = itemClass.EntityType switch
            {
                EntityType.Weapon => dungeon.LocaleToUse["CharacterDamageStat"],
                EntityType.Armor => dungeon.LocaleToUse["CharacterMitigationStat"],
                _ => string.Empty
            };
            Power = itemClass.Power ?? string.Empty;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

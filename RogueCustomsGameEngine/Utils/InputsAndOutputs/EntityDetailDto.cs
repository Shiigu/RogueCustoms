using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
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
    public class EntityDetailDto
    {
        public bool ShowEntityDescription { get; set; }
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        public ConsoleRepresentation EntityConsoleRepresentation { get; set; }
        public bool ShowTileDescription { get; set; }
        public string TileName { get; set; }
        public string TileDescription { get; set; }
        public ConsoleRepresentation TileConsoleRepresentation { get; set; }

        public EntityDetailDto() { }

        public EntityDetailDto(Entity? entity, Tile tile, bool showTileDescription)
        {
            ShowEntityDescription = (entity != null);
            if (entity != null)
            {
                EntityName = entity.Name;
                EntityDescription = entity.Description;
                EntityConsoleRepresentation = entity.ConsoleRepresentation;
            }

            if (!showTileDescription) return;
            ShowTileDescription = showTileDescription;
            TileName = tile.TypeName;
            TileDescription = tile.TypeDescription;
            TileConsoleRepresentation = tile.ConsoleRepresentation;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

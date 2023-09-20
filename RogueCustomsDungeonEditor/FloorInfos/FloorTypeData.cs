using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.FloorInfos
{
    public class FloorTypeData
    {
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public Image PreviewImage { get; set; }
    }
}

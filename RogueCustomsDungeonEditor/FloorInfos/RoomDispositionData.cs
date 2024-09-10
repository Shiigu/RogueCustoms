using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsDungeonEditor.FloorInfos
{
    public class RoomDispositionData
    {
        public string? InternalName { get; set; }
        public string? DisplayName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoomDispositionItemType DispositionItemType { get; set; }
        public RoomDispositionType RoomDispositionIndicator { get; set; }
        public string? RoomDispositionIndicatorChar => RoomDispositionIndicator.ToChar().ToString();
        public string? ImagePath { get; set; }

        public Image? TileImage { get; set; }
    }

    public enum RoomDispositionItemType
    {
        Room,
        Connection
    }
}

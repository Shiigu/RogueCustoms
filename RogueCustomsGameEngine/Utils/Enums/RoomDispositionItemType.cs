namespace RogueCustomsGameEngine.Utils.Enums
{
    public enum RoomDispositionType
    {
        GuaranteedRoom,
        GuaranteedDummyRoom,
        NoRoom,
        RandomRoom,
        GuaranteedHallway,
        GuaranteedFusion,
        NoConnection,
        RandomConnection,
        ConnectionImpossible
    }

    public static class RoomDispositionIndicatorHelpers
    {
        public static RoomDispositionType ToRoomDispositionIndicator(this string s, bool isForHallway) => s[0].ToRoomDispositionIndicator(isForHallway);
        public static RoomDispositionType ToRoomDispositionIndicator(this char c, bool isForHallway)
        {
            return c switch
            {
                'R' => RoomDispositionType.GuaranteedRoom,
                'D' => RoomDispositionType.GuaranteedDummyRoom,
                '?' => (isForHallway) ? RoomDispositionType.RandomConnection : RoomDispositionType.RandomRoom,
                '-' => (isForHallway) ? RoomDispositionType.NoConnection : RoomDispositionType.NoRoom,
                '+' => RoomDispositionType.GuaranteedHallway,
                '=' => RoomDispositionType.GuaranteedFusion,
                ' ' => RoomDispositionType.ConnectionImpossible,
                _ => RoomDispositionType.ConnectionImpossible
            };
        }

        public static char ToChar(this RoomDispositionType indicator)
        {
            return indicator switch
            {
                RoomDispositionType.GuaranteedRoom => 'R',
                RoomDispositionType.GuaranteedDummyRoom => 'D',
                RoomDispositionType.NoRoom => '-',
                RoomDispositionType.RandomRoom => '?',
                RoomDispositionType.GuaranteedHallway => '+',
                RoomDispositionType.GuaranteedFusion => '=',
                RoomDispositionType.NoConnection => '-',
                RoomDispositionType.RandomConnection => '?',
                RoomDispositionType.ConnectionImpossible => ' ',
                _ => ' '
            };
        }
    }
}

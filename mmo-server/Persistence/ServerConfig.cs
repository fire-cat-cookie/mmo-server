using mmo_shared;
using System.Collections.Generic;

namespace mmo_server.Persistence {
    public class ServerConfig
    {
        public partial class Characters
        {
            public static Vector2 StartingPosition { get; } = new Vector2(126, 103);
            public static Vector2 RespawnPosition { get; } = new Vector2(214, 130);
        }

        public partial class Game {
            public const int targetTickrate = 50;
        }

        public partial class Zones
        {
            public const uint startingZone = 1;
            public static Dictionary<uint, ushort> numberOfMobs = new Dictionary<uint, ushort>() { { startingZone, 100 } };
        }

        public partial class DebugMessages {
            public const bool login = true;
        }
    }
}

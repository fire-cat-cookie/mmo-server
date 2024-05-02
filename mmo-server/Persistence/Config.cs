
using mmo_shared;

namespace mmo_server.Persistence {
    public class Config {
        public Game game { get; } = new Game();
        public Characters characters { get; } = new Characters();
        public Accounts accounts { get; } = new Accounts();
        public DebugMessages debugMessages { get; } = new DebugMessages();

        public class Game {
            public int TargetTickrate { get; } = 50;
        }

        public class Characters {
            public float baseMovementPerSecond { get; } = 8f;
            public uint StartingZone { get; } = 1;
            public Vector2 StartingPosition { get; } = new Vector2(126, 103);
            public Vector2 RespawnPosition { get; } = new Vector2(214, 130);
            public float baseAttackWindup { get; } = 0.45f;
            public float baseAttackCooldown { get; } = 0.8f;
            public float baseAttackDamage { get; } = 85;
            public float baseAttackRange { get; } = 7;
            public float baseRadius { get; } = 0.7f;
        }

        public class Accounts {
            public int MinCharactersForUsername { get; } = 3;
            public int MaxCharactersForUsername { get; } = 20;
            public int MinCharactersForPassword { get; } = 3;
            public int MaxCharactersForPassword { get; } = 20;
        }
        public class DebugMessages {
            public bool Login { get; } = true;
        }

        public Config() {
        }
    }
}

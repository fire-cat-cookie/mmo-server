
using mmo_shared;
using System.Collections.Generic;

namespace mmo_shared {
    public class SharedConfig {

        public class Characters {
            public const float baseMovementPerSecond = 8f;
            public const float baseAttackWindup = 0.45f;
            public const float baseAttackCooldown  = 0.8f;
            public const float baseAttackDamage = 35;
            public const float baseAttackRange = 7;
            public const float baseRadius = 0.7f;
        }

        public class Accounts {
            public const int minCharactersForUsername = 3;
            public const int maxCharactersForUsername = 20;
            public const int minCharactersForPassword = 3;
            public const int maxCharactersForPassword = 20;
        }
    }
}

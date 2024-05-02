using mmo_server.Gamestate;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmo_shared.Messages;
using mmo_server.Persistence;

namespace mmo_server
{
    class Converter {
        public static CharInfo CreateCharInfo(ActiveCharacter c) {
            return new CharInfo(c.Entity.AccountId, c.Entity.Name, c.Entity.Class, c.Entity.Level,
                c.Position, c.Destination, c.Velocity, c.MaxHealth, c.CurrentHealth,
                c.AttackRange, c.AttackCooldown, c.Alive);
        }

        public static ActiveCharacter CreateDefaultCharacter(Character entity, Config gameConfig)
        {
            return new ActiveCharacter(entity)
            {
                MaxHealth = 1000,
                CurrentHealth = 1000,
                AttackCooldown = gameConfig.characters.baseAttackCooldown,
                AttackRange = gameConfig.characters.baseAttackRange,
                Alive = true,
                Velocity = 0f
            };
        }
    }
}

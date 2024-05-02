using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages
{
    public class MobInfo : ActorInfo
    {

        public MobInfo() { }

        public MobInfo(uint actorId, Vector2 position, Vector2 destination, float movespeed, float maxHealth, float currentHealth, float attackRange, float attackCooldown, string name, bool alive)
        {
            ActorId = actorId;
            Position = position;
            Destination = destination;
            Movespeed = movespeed;
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            AttackRange = attackRange;
            AttackCooldown = attackCooldown;
            Name = name;
            Alive = alive;
        }
    }
}

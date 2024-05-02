using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class CharInfo : Message {
        [Order(0)] public uint PlayerId { get; set; }
        [Order(1)] public string Name { get; set; }
        [Order(2)] public byte ClassId { get; set; }
        [Order(3)] public ushort Level { get; set; }
        [Order(4)] public Vector2 Position { get; set; }
        [Order(5)] public Vector2 Destination { get; set; }
        [Order(6)] public float Velocity { get; set; }
        [Order(7)] public float MaxHealth { get; set; }
        [Order(8)] public float CurrentHealth { get; set; }
        [Order(9)] public float AttackRange { get; set; }
        [Order(10)] public float AttackCooldown { get; set; }
        [Order(11)] public bool Alive { get; set; }

        public CharInfo() { }

        public CharInfo(uint playerId, string name, byte classId, ushort level,
            Vector2 position, Vector2 destination, float velocity, float maxHealth, float currentHealth,
            float attackRange, float attackCooldown, bool alive) {
            PlayerId = playerId;
            Name = name;
            ClassId = classId;
            Level = level;
            Position = position;
            Destination = destination;
            Velocity = velocity;
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            AttackRange = attackRange;
            AttackCooldown = attackCooldown;
            Alive = alive;
        }
    }
}

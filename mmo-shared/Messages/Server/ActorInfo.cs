using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages
{
    public abstract class ActorInfo : Message
    {
        [Order(0)] public uint ActorId { get; set; }
        [Order(1)] public Vector2 Position { get; set; }
        [Order(2)] public Vector2 Destination { get; set; }
        [Order(3)] public float Movespeed { get; set; }
        [Order(4)] public float MaxHealth { get; set; }
        [Order(5)] public float CurrentHealth { get; set; }
        [Order(6)] public float AttackRange { get; set; }
        [Order(7)] public float AttackCooldown { get; set; }
        [Order(8)] public string Name { get; set; }
        [Order(9)] public bool Alive { get; set; }

    }
}

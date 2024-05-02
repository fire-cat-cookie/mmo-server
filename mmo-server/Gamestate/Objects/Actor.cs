using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate
{
    internal interface Actor : Positionable
    {
        uint ActorId { get; set; }
        Vector2 Destination { get; set; }
        float MaxHealth { get; set; }
        float CurrentHealth { get; set; }
        float AttackRange { get; set; }
        float AttackCooldown { get; set; }
        float AttackDamage { get; set; }
        float MoveSpeed { get; set; }
        bool Alive { get; set; }
        uint ZoneId { get; set; }
        string Name { get; set; }
    }
}

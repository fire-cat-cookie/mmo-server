using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmo_shared;

namespace mmo_server.Gamestate;

internal class Mob : Actor
{
    public uint ActorId { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Destination { get; set; }
    public float Velocity { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float AttackRange { get; set; }
    public float AttackCooldown { get; set; }
    public float AttackDamage { get; set; }
    public float MoveSpeed { get; set; }
    public bool Alive { get; set; } = true;
    public uint ZoneId { get; set; }
    public string Name { get; set; }
}

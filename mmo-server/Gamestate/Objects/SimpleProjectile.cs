using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    public class SimpleProjectile : Positionable {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Velocity { get; set; }
        public float RemainingLifespan { get; set; }

        public SimpleProjectile(Vector2 startPosition, Vector2 direction, float velocity, float lifespan) {
            Position = startPosition;
            Direction = direction;
            Velocity = velocity;
            RemainingLifespan = lifespan;
        }
    }
}

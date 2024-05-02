using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    public class CircleCollider {
        public float Radius { get; protected set; }
        public Positionable Parent { get; set; }

        public CircleCollider(float radius, Positionable parent) {
            Radius = radius;
            Parent = parent;
        }
    }
}

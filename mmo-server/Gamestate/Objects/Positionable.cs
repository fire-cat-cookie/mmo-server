using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    public interface Positionable {
        Vector2 Position { get; set; }
    }
}

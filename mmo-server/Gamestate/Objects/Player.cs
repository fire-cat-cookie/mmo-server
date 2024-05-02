using System;
using System.Collections.Generic;
using System.Net;

namespace mmo_server.Gamestate {
    class Player {
        public IPEndPoint Ip {get; private set;}
        public uint AccountId { get; private set; }
        public ActiveCharacter CurrentCharacter { get; set; }

        public Player(IPEndPoint ip, uint accountId) {
            Ip = ip;
            AccountId = accountId;
        }
    }
}
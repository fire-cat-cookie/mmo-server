using mmo_server.Gamestate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    /// <summary>
    /// Holds references to players and relevant information about them.
    /// </summary>
    class PlayerService {
        private Dictionary<IPEndPoint, Player> _connectedPlayers = new Dictionary<IPEndPoint, Player>();
        public IReadOnlyDictionary<IPEndPoint, Player> ByIP {
            get { return new ReadOnlyDictionary<IPEndPoint, Player>(_connectedPlayers); }
        }

        private Dictionary<UInt32, Player> _accounts = new Dictionary<UInt32, Player>();
        public ReadOnlyDictionary<UInt32, Player> ByAccountId {
            get { return new ReadOnlyDictionary<UInt32, Player>(_accounts); }
        }

        public void AddPlayer(Player p) {
            _connectedPlayers.Add(p.Ip, p);
            _accounts.Add(p.AccountId, p);
        }

        public void RemovePlayer(Player p) {
            _connectedPlayers.Remove(p.Ip);
            _accounts.Remove(p.AccountId);
        }

        public Player FindPlayer(IPEndPoint ip) {
            if (_connectedPlayers.ContainsKey(ip)) {
                return _connectedPlayers[ip];
            }
            return null;
        }

        public Player FindPlayer(uint accountId) {
            if (_accounts.ContainsKey(accountId)) {
                return _accounts[accountId];
            }
            return null;
        }

        ///<returns>The owner of the given character, if the player is connected. Else return null</returns>
        public Player FindPlayer(ActiveCharacter c) {
            if (_accounts.ContainsKey(c.Entity.AccountId)) {
                Player p = _accounts[c.Entity.AccountId];
                return p;
            }
            return null;
        }
    }
}

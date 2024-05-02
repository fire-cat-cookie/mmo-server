using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_server.Persistence;
using mmo_shared;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    class CharacterLoginService {
        public delegate void HandleLogin(ActiveCharacter c);
        public event HandleLogin CharacterLoggedIn = delegate { };
        public event HandleLogin CharacterLoggedOut = delegate { };

        private Database db;
        private PlayerService players;
        private ZoneService zones;
        private MessageSender messageSender;
        private Config config;
        private BroadcastService broadCastService;

        public CharacterLoginService(Database db, PlayerService players, ZoneService zones,
            MessageSender messageSender, Config config, BroadcastService broadCastService) {
            this.db = db;
            this.players = players;
            this.zones = zones;
            this.messageSender = messageSender;
            this.config = config;
            this.broadCastService = broadCastService;
        }

        /// <summary>
        /// Assign the player's current character, assing the character to a zone, and broadcast the character's arrival.
        /// </summary>
        /// <returns>true if the operation succeeded, false otherwise</returns>
        public bool LoginCharacter(ActiveCharacter c) {
            Player p = players.ByAccountId[c.Entity.AccountId];
            Zone zone = zones.Zones[c.Entity.ZoneId];
            if (zone.AddCharacter(c)) {
                p.CurrentCharacter = c;
                CharacterLoggedIn(c);
                BroadcastCharacterArrival(c);
                return true;
            }
            return false;
        }

        public void LogoutCharacter(ActiveCharacter c) {
            Player p = players.ByAccountId[c.Entity.AccountId];
            Zone playerZone = zones.Zones[c.Entity.ZoneId];
            playerZone.RemoveCharacter(c);
            CharacterLoggedOut(c);
            broadCastService.DistributeNearby(c, new ServerDisconnect(c.Entity.AccountId));
            p.CurrentCharacter = null;
        }

        private void BroadcastCharacterArrival(ActiveCharacter arrivedCharacter) {
            Zone zone = zones.Zones[arrivedCharacter.Entity.ZoneId];
            CharInfo arrivedCharSpotted = Converter.CreateCharInfo(arrivedCharacter);
            broadCastService.DistributeInZone(zone, arrivedCharSpotted);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_server.Persistence;
using mmo_shared;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    /// <summary>
    /// Holds references to all server zones and relevant information about them.
    /// </summary>
    class ZoneService {

        private Dictionary<UInt32, Zone> zones = new Dictionary<UInt32, Zone>();
        private readonly MessageSender messageSender;
        private readonly PlayerService playerService;

        public ReadOnlyDictionary<UInt32, Zone> Zones {
            get { return new ReadOnlyDictionary<UInt32, Zone>(zones); }
        }
        public Zone StartingZone { get; set; }

        public ZoneService(Config config, MessageSender messageSender, PlayerService playerService) {
            StartingZone = new Zone(config.characters.StartingZone);
            this.messageSender = messageSender;
            this.playerService = playerService;

            zones.Add(StartingZone.Id, StartingZone);
        }

        public void Add(Zone zone) {
            zones.Add(zone.Id, zone);
        }

        public void SendSurroundingsToPlayer(ActiveCharacter playerCharacter, uint zoneId) {
            Zone zone = zones[zoneId];
            for (int i = 0; i < zone.Characters.Count; i++) {
                ActiveCharacter otherCharacter = zone.Characters[i];
                CharInfo charInfo = Converter.CreateCharInfo(otherCharacter);
                messageSender.SendTo(playerService.ByAccountId[playerCharacter.Entity.AccountId].Ip, charInfo);
            }
        }
    }
}

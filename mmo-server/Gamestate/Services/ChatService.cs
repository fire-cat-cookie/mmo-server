using mmo_server.Gamestate;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    class ChatService {
        private readonly BroadcastService broadcastService;
        private readonly ZoneService zones;

        public ChatService(BroadcastService broadcastService, ZoneService zones) {
            this.broadcastService = broadcastService;
            this.zones = zones;
        }

        public void Say(Player sender, ClientChatMessage message) {
            ActiveCharacter playerCharacter = sender.CurrentCharacter;
            if (playerCharacter == null) {
                return;
            }
            Zone playerZone = zones.Zones[playerCharacter.Entity.ZoneId];
            broadcastService.DistributeInZone(playerZone, new ServerChatMessage(message.Text, playerCharacter.Entity.AccountId));
        }
    }
}

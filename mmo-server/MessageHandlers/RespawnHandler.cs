using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.MessageHandlers {
    class RespawnHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly PlayerService playerService;
        private readonly RespawnService respawnService;

        public RespawnHandler(PacketPublisher packetPublisher, PlayerService playerService, RespawnService respawnService) {
            this.packetPublisher = packetPublisher;
            this.playerService = playerService;
            this.respawnService = respawnService;
            packetPublisher.Subscribe(typeof(ClientRespawn), ProcessRespawn);
        }

        private void ProcessRespawn(Message message, IPEndPoint source) {
            Player player = playerService.FindPlayer(source);
            if (player == null) {
                return;
            }
            ActiveCharacter playerChar = player.CurrentCharacter;
            if (playerChar == null) {
                return;
            }
            respawnService.RespawnCharacter(playerChar);
        }

    }
}

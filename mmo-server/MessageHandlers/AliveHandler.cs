using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_shared;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.MessageHandlers {
    class AliveHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly PlayerTimeoutService playerTimeoutService;

        public AliveHandler(PacketPublisher packetPublisher, PlayerTimeoutService playerTimeoutService) {
            this.packetPublisher = packetPublisher;
            this.playerTimeoutService = playerTimeoutService;
            packetPublisher.Subscribe(typeof(Alive), HandleMessage);
        }

        private void HandleMessage(Message message, IPEndPoint ip) {
            playerTimeoutService.ResetTimer(ip);
        }
    }
}

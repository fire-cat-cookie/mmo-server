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
    class ChatMessageHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly ChatService chatService;
        private readonly PlayerService playerService;

        public ChatMessageHandler(PacketPublisher packetPublisher, ChatService chatService, PlayerService playerService) {
            this.packetPublisher = packetPublisher;
            this.chatService = chatService;
            this.playerService = playerService;
            packetPublisher.Subscribe(typeof(ClientChatMessage), HandleMessage);
        }

        private void HandleMessage(Message message, IPEndPoint source) {
            ClientChatMessage chatMessage = (ClientChatMessage)message;

            Player sender = playerService.FindPlayer(source);
            if (sender == null) {
                return;
            }
            chatService.Say(sender, chatMessage);
        }
    }
}

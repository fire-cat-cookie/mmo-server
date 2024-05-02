using mmo_shared;
using System;
using System.Collections.Generic;
using System.Net;
using mmo_shared.Messages;

namespace mmo_server.Communication {
    public class MessageSender {
        private ClientConnector clients;
        private Serializer serializer;

        public MessageSender(ClientConnector clients, Serializer serializer) {
            this.clients = clients;
            this.serializer = serializer;
        }

        public void SendTo(IPEndPoint ip, Message message) {
            byte[] packet = serializer.Serialize(message);
            clients.SendTo(ip, packet);
        }
    }
}

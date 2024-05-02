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
    class SessionHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly SessionService sessionService;
        private readonly MessageSender messageSender;

        public SessionHandler(PacketPublisher packetPublisher, SessionService sessionService, MessageSender messageSender) {
            this.packetPublisher = packetPublisher;
            this.sessionService = sessionService;
            this.messageSender = messageSender;
            packetPublisher.Subscribe(typeof(SessionIdRequest), ProcessSessionRequest);
            packetPublisher.Subscribe(typeof(Login), ProcessLogin);
            packetPublisher.Subscribe(typeof(ClientDisconnect), ProcessDisconnect);
        }

        private void ProcessSessionRequest(Message message, IPEndPoint source) {
            messageSender.SendTo(source, new SessionStart() { SessionId = sessionService.CreateSessionId(source) });
        }

        private void ProcessLogin(Message message, IPEndPoint source) {
            Login login = (Login)message;
            var response = sessionService.Login(login, source);
            messageSender.SendTo(source, new LoginResponse() { Type = (byte)response });
        }

        private void ProcessDisconnect(Message message, IPEndPoint source) {
            sessionService.Disconnect(source);
        }
    }
}

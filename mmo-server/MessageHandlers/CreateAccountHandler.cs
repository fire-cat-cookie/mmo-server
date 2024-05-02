using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_server.Debug;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using mmo_shared;

namespace mmo_server.MessageHandlers {
    class CreateAccountHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly CreateAccountService createAccountService;
        private readonly MessageSender messageSender;

        public CreateAccountHandler(PacketPublisher packetPublisher, CreateAccountService createAccountService, MessageSender messageSender) {
            this.packetPublisher = packetPublisher;
            this.createAccountService = createAccountService;
            this.messageSender = messageSender;

            packetPublisher.Subscribe(typeof(CreateAccount), CreateAccount);
        }

        private void CreateAccount(Message message, IPEndPoint source) {
            RegistrationResponse.Types response = createAccountService.CreateAccount((CreateAccount)message, source);
            messageSender.SendTo(source, new RegistrationResponse() { ResponseType = (byte)response });
        }
    }
}

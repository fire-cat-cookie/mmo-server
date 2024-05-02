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
    class CreateCharacterHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly CharacterCreationService characterCreationService;
        private readonly PlayerService playerService;
        private readonly MessageSender messageSender;
        private readonly CharSelectService charSelectService;

        public CreateCharacterHandler(PacketPublisher packetPublisher, CharacterCreationService characterCreationService,
            PlayerService playerService, MessageSender messageSender, CharSelectService charSelectService) {
            this.packetPublisher = packetPublisher;
            this.characterCreationService = characterCreationService;
            this.playerService = playerService;
            this.messageSender = messageSender;
            this.charSelectService = charSelectService;
            packetPublisher.Subscribe(typeof(CreateCharacter), CreateCharacter);
        }

        private void CreateCharacter(Message msg, IPEndPoint source) {
            CreateCharacter create = (CreateCharacter)msg;

            Player player = playerService.FindPlayer(source);
            if (player == null) {
                return;
            }

            CreateCharacterResponse.Types response = characterCreationService.CreateCharacter(player, create, out ActiveCharacter newCharacter);
            CharSlotInfo info = charSelectService.GetCharSlotInfo(newCharacter);
            messageSender.SendTo(source, new CreateCharacterResponse() { Response = (byte)response, CharInfo = info });

        }

    }
}

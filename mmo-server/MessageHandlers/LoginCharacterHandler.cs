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

namespace mmo_server.MessageHandlers
{
    class LoginCharacterHandler {
        private PacketPublisher packetPublisher;
        private readonly CharacterLoginService characterLoginService;
        private readonly PlayerService playerRegistry;
        private readonly Database db;
        private readonly MessageSender messageSender;
        private readonly ZoneService zoneService;
        private readonly Config config;

        public LoginCharacterHandler(PacketPublisher packetPublisher, CharacterLoginService characterLoginService,
            PlayerService playerService, Database db, MessageSender messageSender, ZoneService zoneService, Config config)
        {
            this.packetPublisher = packetPublisher;
            this.characterLoginService = characterLoginService;
            this.playerRegistry = playerService;
            this.db = db;
            this.messageSender = messageSender;
            this.zoneService = zoneService;
            this.config = config;
            packetPublisher.Subscribe(typeof(LoginCharacter), ProcessCharacterLogin);
            packetPublisher.Subscribe(typeof(GetSurroundings), ProcessGetSurroundings);
        }

        private void ProcessCharacterLogin(Message message, IPEndPoint source) {
            Player player = playerRegistry.FindPlayer(source);
            if (player == null) {
                return;
            }
            if (db.GetCharacter(player.AccountId, ((LoginCharacter)message).Slot, out Character entity)) {
                var character = Converter.CreateDefaultCharacter(entity, config);
                if (characterLoginService.LoginCharacter(character)) {
                    messageSender.SendTo(source, new LoginCharacterResponse((byte)LoginCharacterResponse.Types.Success, entity.AccountId));
                }
            }
        }

        private void ProcessGetSurroundings(Message message, IPEndPoint source) {
            Player player = playerRegistry.FindPlayer(source);
            if (player == null) {
                return;
            }
            if (player.CurrentCharacter == null) {
                return;
            }
            zoneService.SendSurroundingsToPlayer(player.CurrentCharacter, player.CurrentCharacter.Entity.ZoneId);
        }


    }
}

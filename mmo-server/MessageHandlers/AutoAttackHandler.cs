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
    class AutoAttackHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly AutoAttackService autoAttackService;
        private readonly PlayerService playerService;

        public AutoAttackHandler(PacketPublisher packetPublisher, AutoAttackService autoAttackService, PlayerService playerService) {
            this.packetPublisher = packetPublisher;
            this.autoAttackService = autoAttackService;
            this.playerService = playerService;
            packetPublisher.Subscribe(typeof(AutoAttack), HandleAttackCommand);
        }

        private void HandleAttackCommand(Message msg, IPEndPoint source) {
            AutoAttack auto = (AutoAttack)msg;

            if (!playerService.ByIP.ContainsKey(source) || !playerService.ByAccountId.ContainsKey(auto.TargetPlayerId)) {
                return;
            }
            ActiveCharacter sourceChar = playerService.ByIP[source].CurrentCharacter;
            ActiveCharacter targetChar = playerService.ByAccountId[auto.TargetPlayerId].CurrentCharacter;
            if (sourceChar == null || targetChar == null) {
                return;
            }
            autoAttackService.StartAttacking(sourceChar, targetChar);
        }
    }
}

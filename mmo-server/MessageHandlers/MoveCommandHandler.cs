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
    class MoveCommandHandler {
        private readonly PlayerService playerRegistry;
        private readonly ZoneService zoneService;
        private readonly BroadcastService broadcastService;
        private readonly InterruptService interruptService;
        private readonly PacketPublisher packetPublisher;
        private readonly MovementService movementService;
        private readonly UnitVerificationService unitStateService;

        public MoveCommandHandler(PlayerService playerService, ZoneService zoneService, BroadcastService broadcastService,
            InterruptService interruptService, PacketPublisher packetPublisher, MovementService movementService, UnitVerificationService unitStateService) {
            this.playerRegistry = playerService;
            this.zoneService = zoneService;
            this.broadcastService = broadcastService;
            this.interruptService = interruptService;
            this.packetPublisher = packetPublisher;
            this.movementService = movementService;
            this.unitStateService = unitStateService;
            packetPublisher.Subscribe(typeof(MoveCommand), ProcessMoveCommand);
        }

        private void ProcessMoveCommand(Message message, IPEndPoint source) {
            MoveCommand move = (MoveCommand)message;
            Player player = playerRegistry.FindPlayer(source);
            if (player == null) {
                return;
            }
            ActiveCharacter c = player.CurrentCharacter;
            if (c == null) {
                return;
            }
            movementService.StartMovingCharacter(c, move.Target);
            interruptService.InterruptAttack(c);
            PositionUpdate pos = new PositionUpdate(c.Entity.AccountId, c.Position, move.Target, c.Velocity);
            broadcastService.DistributeInZone(zoneService.Zones[c.Entity.ZoneId], pos);
        }
    }
}

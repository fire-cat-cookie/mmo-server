using mmo_server.Communication;
using mmo_server.Persistence;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    class RespawnService {
        private readonly HealthService healthService;
        private readonly MovementService movementService;
        private readonly Config config;
        private readonly BroadcastService broadcastService;
        private readonly PlayerService playerService;

        public RespawnService(HealthService healthService, MovementService movementService, Config config, BroadcastService broadcastService,
            PlayerService playerService) {
            this.healthService = healthService;
            this.movementService = movementService;
            this.config = config;
            this.broadcastService = broadcastService;
            this.playerService = playerService;
        }

        public void RespawnCharacter(ActiveCharacter c) {
            if (c.Alive) {
                return;
            }
            healthService.SetCurrentHealth(c, c.MaxHealth);
            c.Alive = true;
            movementService.Teleport(c, config.characters.RespawnPosition);
            broadcastService.DistributeNearby(playerService.FindPlayer(c), new UnitRevive(c.Entity.AccountId));
        }
    }
}

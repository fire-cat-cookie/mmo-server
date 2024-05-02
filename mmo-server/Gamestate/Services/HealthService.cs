using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    class HealthService {
        private readonly BroadcastService broadCastService;
        private readonly ZoneService zoneService;
        private readonly PlayerService playerService;
        private readonly InterruptService interruptService;

        public HealthService(BroadcastService broadcastService, ZoneService zoneService, PlayerService playerService,
            InterruptService interruptService) {
            this.broadCastService = broadcastService;
            this.zoneService = zoneService;
            this.playerService = playerService;
            this.interruptService = interruptService;
        }

        public void ChangeCurrentHealth(ActiveCharacter c, float change) {
            SetCurrentHealth(c, c.CurrentHealth + change);
        }

        public void SetCurrentHealth(ActiveCharacter c, float newHealth) {
            if (newHealth > c.MaxHealth) {
                newHealth = c.MaxHealth;
            }
            c.CurrentHealth = newHealth;
            if (c.CurrentHealth <= 0) {
                Die(c);
            }
            broadCastService.DistributeInZone(zoneService.Zones[c.Entity.ZoneId], new HealthChange(c.Entity.AccountId, c.CurrentHealth));
        }

        public void Die(ActiveCharacter c) {
            c.CurrentHealth = 0;
            c.Alive = false;
            interruptService.InterruptAttack(c);
            interruptService.InterruptMovement(c);
            broadCastService.DistributeInZone(zoneService.Zones[c.Entity.ZoneId], new UnitDie(c.Entity.AccountId));
        }
    }
}

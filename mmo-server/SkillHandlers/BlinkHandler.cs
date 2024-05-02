using mmo_server.ControlTower;
using mmo_server.Gamestate;
using mmo_shared;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.SkillHandlers {
    class BlinkHandler {
        private readonly MovementService movementService;
        private readonly UnitVerificationService unitVerificationService;
        private readonly CooldownService cooldownService;
        private readonly BroadcastService broadcastService;

        public BlinkHandler(SkillPublisher skillPublisher, MovementService movementService, UnitVerificationService unitVerificationService,
            CooldownService cooldownService, BroadcastService broadcastService) {
            this.movementService = movementService;
            this.unitVerificationService = unitVerificationService;
            this.cooldownService = cooldownService;
            this.broadcastService = broadcastService;

            skillPublisher.Subscribe("Blink", HandleSkill);
        }

        private void HandleSkill(Skill skill, ActiveCharacter c, Vector2 target) {
            if (unitVerificationService.CanMove(c)) {
                movementService.Teleport(c, target);
                cooldownService.StartCooldown(skill, c);
                broadcastService.DistributeNearby(c, new ServerGroundTargetSkill(skill.Id, c.Entity.AccountId, target));
            }
        }
    }
}

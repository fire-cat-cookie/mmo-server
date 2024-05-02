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
    class PewHandler {
        private readonly float projectileCollisionRadius = 0.3f;
        private readonly float projectileVelocity = 20f;
        private readonly float damage = 225f;
        private readonly float range = 15f;

        private readonly SkillPublisher skillPublisher;
        private readonly ProjectileService projectileService;
        private readonly CollisionService collisionService;
        private readonly HealthService healthService;
        private readonly BroadcastService broadcastService;
        private readonly MovementService movementService;

        public PewHandler(SkillPublisher skillPublisher, ProjectileService projectileService, CollisionService collisionService,
            HealthService healthService, BroadcastService broadcastService, MovementService movementService) {
            this.skillPublisher = skillPublisher;
            this.projectileService = projectileService;
            this.collisionService = collisionService;
            this.healthService = healthService;
            this.broadcastService = broadcastService;
            this.movementService = movementService;
            skillPublisher.Subscribe("Pew", HandleSkill);
        }

        private void HandleSkill(Skill skill, ActiveCharacter source, Vector2 target) {
            CircleCollider projectileCollider = projectileService.Spawn(source.Position, target, projectileVelocity, projectileCollisionRadius, range);

            collisionService.Subscribe(
                projectileCollider,
                (CircleCollider other) => {
                    if (other.Parent is ActiveCharacter) {
                        ActiveCharacter unit = other.Parent as ActiveCharacter;
                        if (unit.Alive && unit != source) {
                            projectileService.Destroy(projectileCollider);
                            healthService.ChangeCurrentHealth(unit, damage * -1);
                        }
                    }
                });

            broadcastService.DistributeNearby(source, new ServerGroundTargetSkill(skill.Id, source.Entity.AccountId, target));
        }

    }
}

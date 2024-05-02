using mmo_server.ControlTower;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    class ProjectileService {
        private readonly GameLoop gameLoop;
        private readonly MovementService movementService;
        private readonly CollisionService collisionService;

        private Dictionary<CircleCollider, SimpleProjectile> projectiles = new Dictionary<CircleCollider, SimpleProjectile>();

        public ProjectileService(GameLoop gameLoop, MovementService movementService, CollisionService collisionService) {
            this.gameLoop = gameLoop;
            this.movementService = movementService;
            this.collisionService = collisionService;
            gameLoop.Tick += Update;
        }

        private void Update(float elapsedMs) {
            foreach (CircleCollider c in projectiles.Keys.ToList()) {
                projectiles[c].RemainingLifespan -= elapsedMs / 1000;
                if (projectiles[c].RemainingLifespan <= 0) {
                    Destroy(c);
                }
            }
        }

        public CircleCollider Spawn(Vector2 start, Vector2 target, float velocity, float radius, float range) {
            SimpleProjectile p = new SimpleProjectile(start, start.DirectionOf(target), velocity, range/velocity);
            CircleCollider collider = new CircleCollider(radius, p);
            projectiles[collider] = p;
            movementService.StartMovingProjectile(p);
            collisionService.AddCollider(collider);
            return collider;
        }

        public void Destroy(CircleCollider projectileCollider) {
            SimpleProjectile projectile = projectiles[projectileCollider];
            movementService.StopMoving(projectile);
            projectiles.Remove(projectileCollider);
            collisionService.RemoveCollider(projectileCollider);
        }
    }
}

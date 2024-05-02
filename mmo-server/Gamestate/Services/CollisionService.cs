using mmo_server.ControlTower;
using mmo_server.Persistence;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Gamestate {
    class CollisionService {
        private readonly CharacterLoginService characterLoginService;
        private readonly Config config;

        public delegate void CollisionHandler(CircleCollider other);

        private Dictionary<Positionable, CircleCollider> colliders = new Dictionary<Positionable, CircleCollider>();
        private Dictionary<CircleCollider, List<CollisionHandler>> collisionHandlers = new Dictionary<CircleCollider, List<CollisionHandler>>();

        public CollisionService(GameLoop gameLoop, CharacterLoginService characterLoginService, Config config) {
            this.characterLoginService = characterLoginService;
            this.config = config;

            gameLoop.Tick += Update;
            characterLoginService.CharacterLoggedIn += (ActiveCharacter c) => { AddCollider(new CircleCollider(config.characters.baseRadius, c)); };
            characterLoginService.CharacterLoggedOut += (ActiveCharacter c) => { RemoveCollider(colliders[c]); };
        }

        private void Update(float elapsedTime) {
            HashSet<CircleCollider> alreadyChecked = new HashSet<CircleCollider>();
            foreach (CircleCollider a in collisionHandlers.Keys.ToList()) {
                alreadyChecked.Add(a);
                foreach (CircleCollider b in colliders.Values.ToList().Except(alreadyChecked)) {
                    CollisionCheck(a, b);
                }
            }
        }

        private void CollisionCheck(CircleCollider a, CircleCollider b) {
            if (a.Parent.Position.Distance(b.Parent.Position) - (a.Radius + b.Radius) < 0.01f) {
                foreach(CollisionHandler handler in collisionHandlers[a]) {
                    handler.Invoke(b);
                }
                if (collisionHandlers.ContainsKey(b)) {
                    foreach (CollisionHandler handler in collisionHandlers[b]) {
                        handler.Invoke(a);
                    }
                }
            }
        }

        public void AddCollider(CircleCollider c) {
            colliders.Add(c.Parent, c);
        }

        public void Subscribe(CircleCollider c, CollisionHandler handler) {
            if (!collisionHandlers.ContainsKey(c)) {
                collisionHandlers[c] = new List<CollisionHandler>();
            }
            collisionHandlers[c].Add(handler);
        }

        public void RemoveCollider(CircleCollider c) {
            if (colliders.ContainsKey(c.Parent)) {
                colliders.Remove(c.Parent);
            }
            if (collisionHandlers.ContainsKey(c)) {
                collisionHandlers.Remove(c);
            }
        }
    }
}

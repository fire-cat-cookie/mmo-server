using mmo_server.Communication;
using mmo_server.Gamestate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mmo_server.Persistence;
using mmo_shared;
using mmo_shared.Messages;
using mmo_server.ControlTower;

namespace mmo_server.Gamestate {
    class MovementService{
        private class Follow {
            public ActiveCharacter Target { get; set; }
            public float Range { get; set; }
        }

        private PlayerService playerService;
        private Config config;
        private GameLoop gameLoop;
        private BroadcastService broadcastService;
        private readonly ZoneService zoneService;
        private readonly UnitVerificationService unitStateService;
        private float timeSinceLastFrame = 0f;
        private Dictionary<ActiveCharacter, Follow> followers = new Dictionary<ActiveCharacter, Follow>();
        private List<SimpleProjectile> projectiles = new List<SimpleProjectile>();

        public MovementService(PlayerService playerService, Config config, GameLoop gameLoop,
            BroadcastService broadcastService, ZoneService zoneService, UnitVerificationService unitStateService) {
            this.playerService = playerService;
            this.config = config;
            this.gameLoop = gameLoop;
            this.broadcastService = broadcastService;
            this.zoneService = zoneService;
            this.unitStateService = unitStateService;
            gameLoop.Tick += Update;
        }

        public void StartMovingCharacter(ActiveCharacter c, Vector2 target) {
            if (!unitStateService.CanMove(c)) {
                return;
            }
            c.Destination = target;
            c.Velocity = config.characters.baseMovementPerSecond;
        }

        public void StartMovingProjectile(SimpleProjectile p) {
            projectiles.Add(p);
        }

        public void MoveIntoRange(ActiveCharacter subject, ActiveCharacter target, float range) {
            if (!unitStateService.CanMove(subject)) {
                return;
            }
            Follow follow = new Follow() { Target = target, Range = range };
            followers[subject] = follow;
            subject.Velocity = config.characters.baseMovementPerSecond;
        }

        public void StopMoving(ActiveCharacter c) {
            c.Destination = c.Position;
            c.Velocity = 0f;
            StopFollowing(c);
            PositionUpdate pos = new PositionUpdate(c.Entity.AccountId, c.Position, c.Position, c.Velocity);
            broadcastService.DistributeInZone(zoneService.Zones[c.Entity.ZoneId], pos);
        }

        public void StopMoving(SimpleProjectile p) {
            projectiles.Remove(p);
        }

        public void StopFollowing(ActiveCharacter source) {
            if (followers.ContainsKey(source)) {
                followers.Remove(source);
            }
        }

        public void Teleport(ActiveCharacter c, Vector2 newPosition) {
            c.Position = newPosition;
            StopMoving(c);
        }

        private void Update(float time) {
            timeSinceLastFrame = time;
            MovePlayers();
            MoveProjectiles();
        }

        private void MovePlayers() {
            foreach (Player p in playerService.ByIP.Values) {
                ActiveCharacter character = p.CurrentCharacter;
                if (character == null) {
                    continue;
                }
                if (followers.ContainsKey(character)) {
                    Follow follow = followers[character];
                    if (character.Position.Distance(follow.Target.Position) <= follow.Range) {
                        StopMoving(character);
                        continue;
                    }
                    character.Destination = follow.Target.Position;
                    PositionUpdate pos = new PositionUpdate(character.Entity.AccountId, character.Position, follow.Target.Position, character.Velocity);
                    broadcastService.DistributeInZone(zoneService.Zones[character.Entity.ZoneId], pos);
                }
                if (character.Destination != character.Position) {
                    float step = character.Velocity * (timeSinceLastFrame / 1000);
                    character.Position = character.Position.MoveTowards(character.Destination, step);
                }
                else {
                    StopMoving(character);
                }
            }
        }

        private void MoveProjectiles() {
            foreach(SimpleProjectile p in projectiles.ToList()) {
                if (p.Direction != p.Position) {
                    float step = p.Velocity * (timeSinceLastFrame / 1000);
                    p.Position += p.Direction * step;
                } else {
                    StopMoving(p);
                }
            }
        }
    }
}

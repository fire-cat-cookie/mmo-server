using mmo_server.Communication;
using mmo_server.ControlTower;
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

namespace mmo_server.Gamestate {
    class AutoAttackService{

        class AutoAttackStatus {
            public ActiveCharacter Target { get; set; }
            public float WindupRemaining { get; set; }
            public float CooldownRemaining { get; set; }
            public bool InWindup { get; set; }
        }

        private readonly GameLoop gameLoop;
        private readonly Config config;
        private readonly UnitVerificationService unitStateService;
        private readonly PlayerService playerService;
        private readonly MovementService movementService;
        private readonly BroadcastService broadcastService;
        private readonly ZoneService zoneService;
        private readonly InterruptService interruptService;
        private readonly HealthService healthService;
        private readonly Dictionary<ActiveCharacter, AutoAttackStatus> activeAutoAttacks = new Dictionary<ActiveCharacter, AutoAttackStatus>();

        public AutoAttackService(GameLoop gameLoop, Config config, UnitVerificationService unitStateService, PlayerService players,
            MovementService playerMover, BroadcastService broadcast, ZoneService zoneRegistry, InterruptService interruptService,
            HealthService healthService) {
            this.gameLoop = gameLoop;
            this.config = config;
            this.unitStateService = unitStateService;
            this.playerService = players;
            this.movementService = playerMover;
            this.broadcastService = broadcast;
            this.zoneService = zoneRegistry;
            this.interruptService = interruptService;
            this.healthService = healthService;
            gameLoop.Tick += Update;

            interruptService.AttackInterrupt += StopAttacking;
        }

        /// <summary>
        /// If the attack command is valid, source will start attacking target, until the attack is interrupted.
        /// If the target is out of range, the attacking unit will start moving towards it.
        /// </summary>
        public void StartAttacking(ActiveCharacter source, ActiveCharacter target) {
            if (!VerifyAttackCommand(source, target)) {
                return;
            }

            var broadcast = new ServerAutoAttack(source.Entity.AccountId, target.Entity.AccountId);
            broadcastService.DistributeInZone(zoneService.Zones[source.Entity.ZoneId], broadcast);

            float cooldownRemaining = 0;
            if (activeAutoAttacks.ContainsKey(source)) {
                cooldownRemaining = activeAutoAttacks[source].CooldownRemaining;
            }
            AutoAttackStatus auto = new AutoAttackStatus() {
                Target = target,
                WindupRemaining = config.characters.baseAttackWindup,
                CooldownRemaining = cooldownRemaining
            };
            activeAutoAttacks[source] = auto;
        }

        public void StopAttacking(ActiveCharacter source) {
            if (activeAutoAttacks.ContainsKey(source)) {
                activeAutoAttacks[source].WindupRemaining = config.characters.baseAttackWindup;
                activeAutoAttacks[source].InWindup = false;
                activeAutoAttacks[source].Target = null;
            }
        }

        private void Update(float elapsedTime) {
            List<ActiveCharacter> removeList = new List<ActiveCharacter>();

            foreach (ActiveCharacter source in activeAutoAttacks.Keys) {
                AutoAttackStatus auto = activeAutoAttacks[source];
                bool hasTarget = (auto.Target != null) && unitStateService.LoggedIn(auto.Target)
                    && auto.Target.Alive && source.Entity.ZoneId == auto.Target.Entity.ZoneId;

                if (auto.CooldownRemaining > 0) {
                    //case 1: attack is cooling down. if we have a target, move into range.
                    auto.CooldownRemaining -= (elapsedTime / 1000);
                    if (hasTarget) {
                        GetInRange(source, auto);
                    }
                } else if (!hasTarget) {
                    //case 2: there is nothing left to attack, and the remaining cooldown is 0,
                    //so we stop tracking this character's auto attacks.
                    removeList.Add(source);
                    movementService.StopFollowing(source);
                    interruptService.InterruptAttack(source);
                    continue;
                } else if (!auto.InWindup){
                    //case 3: we're ready to start the attack windup, but we have to get in range first.
                    if (GetInRange(source, auto)) {
                        auto.WindupRemaining -= (elapsedTime / 1000);
                        auto.InWindup = true;
                        movementService.StopMoving(source);
                    }
                }
                else if (auto.WindupRemaining > 0) {
                    //case 4: attack is winding up.
                    auto.WindupRemaining -= (elapsedTime / 1000);
                } else {
                    //case 5: attack finishes, and the attack cooldown is applied.
                    healthService.ChangeCurrentHealth(auto.Target, -1 * (config.characters.baseAttackDamage));
                    auto.CooldownRemaining = config.characters.baseAttackCooldown;
                    auto.WindupRemaining = config.characters.baseAttackWindup;
                    auto.InWindup = false;
                }
            }

            foreach (ActiveCharacter remove in removeList) {
                activeAutoAttacks.Remove(remove);
            }
        }

        private bool VerifyAttackCommand(ActiveCharacter source, ActiveCharacter target) {
            if (!playerService.ByAccountId.ContainsKey(source.Entity.AccountId) || !playerService.ByAccountId.ContainsKey(target.Entity.AccountId)) {
                return false;
            }
            if (source == null || target == null) {
                return false;
            }
            if (source.Entity.ZoneId != target.Entity.ZoneId) {
                return false;
            }
            if (source == target) {
                return false;
            }
            if (!unitStateService.CanAttack(source)) {
                return false;
            }
            if (!target.Alive) {
                return false;
            }
            return true;
        }
        
        /// <returns>true if character is already in range, else return false.</returns>
        private bool GetInRange(ActiveCharacter source, AutoAttackStatus auto) {
            if (source.AttackRange <= source.Position.Distance(auto.Target.Position)) {
                movementService.MoveIntoRange(source, auto.Target, source.AttackRange);
                return false;
            }
            return true;
        }

        private bool InRange(ActiveCharacter source, AutoAttackStatus auto) {
            return source.AttackRange <= source.Position.Distance(auto.Target.Position);
        }
    }
}

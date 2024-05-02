using mmo_server.Gamestate;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    class UnitVerificationService {
        private readonly PlayerService playerService;

        public UnitVerificationService(PlayerService playerService) {
            this.playerService = playerService;
        }

        public bool LoggedIn(ActiveCharacter c) {
            Player p = playerService.FindPlayer(c);
            return (p != null && p.CurrentCharacter == c);
        }

        public bool CanAttack(ActiveCharacter c) {
            return LoggedIn(c) && c.Alive;
        }

        public bool CanMove(ActiveCharacter c) {
            return LoggedIn(c) && c.Alive;
        }

        public bool CanUseSkills(ActiveCharacter c) {
            return LoggedIn(c) && c.Alive;
        }

    }
}

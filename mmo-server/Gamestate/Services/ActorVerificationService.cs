using mmo_server.Gamestate;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    class ActorVerificationService {
        private readonly PlayerService playerService;

        public ActorVerificationService(PlayerService playerService) {
            this.playerService = playerService;
        }

        public bool LoggedIn(ActiveCharacter c) {
            Player p = playerService.FindPlayer(c);
            return (p != null && p.CurrentCharacter == c);
        }

        public bool CanAct(Actor a)
        {
            if (a is ActiveCharacter && !LoggedIn((ActiveCharacter)a))
            {
                return false;
            }
            return a.Alive;
        }

        public bool CanAttack(Actor a) {
            if(!CanAct(a)){
                return false;
            }
            return true;
        }

        public bool CanMove(Actor a)
        {
            if (!CanAct(a))
            {
                return false;
            }
            return true;
        }

        public bool CanUseSkills(Actor a)
        {
            if (!CanAct(a))
            {
                return false;
            }
            return true;
        }

    }
}

using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared {
    public class SkillData {
        public static Skill[] skills = new Skill[] {
            new Skill("Blink", 1, SkillType.GroundTarget, 0),
            new Skill("Pew", 0.5f, SkillType.GroundTarget, 1),
        };
    }
}

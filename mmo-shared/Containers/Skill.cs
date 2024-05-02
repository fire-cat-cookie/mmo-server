using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared {
    public enum SkillType {
        GroundTarget, UnitTarget, NoTarget
    }

    public class Skill {
        public string CodeName { get; set; }
        public float Cooldown { get; set; }
        public SkillType SkillType { get; set; }
        public uint Id { get; set; }

        public Skill(string codeName, float cooldown, SkillType type, uint skillId) {
            CodeName = codeName;
            Cooldown = cooldown;
            SkillType = type;
            Id = skillId;
        }
    }
}

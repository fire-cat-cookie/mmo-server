using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class GroundTargetSkill : Message {
        [Order(0)] public uint SkillId { get; set; }
        [Order(1)] public Vector2 Target { get; set; }

        public GroundTargetSkill() { }

        public GroundTargetSkill(uint skillId, Vector2 target) {
            SkillId = skillId;
            Target = target;
        }
    }
}

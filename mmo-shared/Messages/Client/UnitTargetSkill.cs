using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class UnitTargetSkill : Message {
        [Order(0)] public uint SkillId { get; set; }
        [Order(1)] public uint Target { get; set; }

        public UnitTargetSkill() { }

        public UnitTargetSkill(uint skillId, uint target) {
            SkillId = skillId;
            Target = target;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class NoTargetSkill : Message{
        [Order(0)] public uint SkillId { get; set; }

        public NoTargetSkill() { }

        public NoTargetSkill(uint skillId) {
            SkillId = skillId;
        }
    }
}

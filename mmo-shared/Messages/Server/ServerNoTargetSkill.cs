using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class ServerNoTargetSkill : Message{
        [Order(0)] public uint SkillId { get; set; }
        [Order(1)] public uint SourceUnitId { get; set; }

        public ServerNoTargetSkill() { }

        public ServerNoTargetSkill(uint skillId, uint sourceUnitId) {
            SkillId = skillId;
            SourceUnitId = sourceUnitId;
        }
    }
}

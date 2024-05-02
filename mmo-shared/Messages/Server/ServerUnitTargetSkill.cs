using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class ServerUnitTargetSkill : Message {
        [Order(0)] public uint SkillId { get; set; }
        [Order(1)] public uint SourceUnitId { get; set; }
        [Order(2)] public uint Target { get; set; }

        public ServerUnitTargetSkill() { }

        public ServerUnitTargetSkill(uint skillId, uint sourceUnitId, uint target) {
            SkillId = skillId;
            SourceUnitId = sourceUnitId;
            Target = target;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class ServerGroundTargetSkill : Message {
        [Order(0)] public uint SkillId { get; set; }
        [Order(1)] public uint SourceUnitId { get; set; }
        [Order(2)] public Vector2 Target { get; set; }

        public ServerGroundTargetSkill() { }

        public ServerGroundTargetSkill(uint skillId, uint sourceUnitId, Vector2 target) {
            SkillId = skillId;
            SourceUnitId = sourceUnitId;
            Target = target;
        }
    }
}

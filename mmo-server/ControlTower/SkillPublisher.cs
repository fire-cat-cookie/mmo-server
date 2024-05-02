using mmo_server.Gamestate;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.ControlTower {
    class SkillPublisher {
        public delegate void HandleNoTargetSkill(Skill skill, ActiveCharacter source);
        public delegate void HandleGroundTargetSkill(Skill skill, ActiveCharacter source, Vector2 target);
        public delegate void HandleUnitTargetSkill(Skill skill, ActiveCharacter source, uint target);

        private Dictionary<string, List<HandleNoTargetSkill>> noTargetHandlers = new Dictionary<string, List<HandleNoTargetSkill>>();
        private Dictionary<string, List<HandleGroundTargetSkill>> groundTargetHandlers = new Dictionary<string, List<HandleGroundTargetSkill>>();
        private Dictionary<string, List<HandleUnitTargetSkill>> unitTargetHandlers = new Dictionary<string, List<HandleUnitTargetSkill>>();

        public void Publish(Skill skill, ActiveCharacter source, Vector2? groundTarget, uint? unitTarget) {
            if (groundTarget.HasValue && groundTargetHandlers.ContainsKey(skill.CodeName)) {
                foreach (var handler in groundTargetHandlers[skill.CodeName]) {
                    handler.Invoke(skill, source, groundTarget.Value);
                }
            } else if (unitTarget.HasValue && unitTargetHandlers.ContainsKey(skill.CodeName)) {
                foreach (var handler in unitTargetHandlers[skill.CodeName]) {
                    handler.Invoke(skill, source, unitTarget.Value);
                }
            } else if (noTargetHandlers.ContainsKey(skill.CodeName)) {
                foreach (var handler in noTargetHandlers[skill.CodeName]) {
                    handler.Invoke(skill, source);
                }
            }
        }

        public void Subscribe(string skillName, HandleNoTargetSkill handler) {
            if (!noTargetHandlers.ContainsKey(skillName)) {
                noTargetHandlers[skillName] = new List<HandleNoTargetSkill>();
            }
            noTargetHandlers[skillName].Add(handler);
        }

        public void Subscribe(string skillName, HandleGroundTargetSkill handler) {
            if (!groundTargetHandlers.ContainsKey(skillName)) {
                groundTargetHandlers[skillName] = new List<HandleGroundTargetSkill>();
            }
            groundTargetHandlers[skillName].Add(handler);
        }

        public void Subscribe(string skillName, HandleUnitTargetSkill handler) {
            if (!unitTargetHandlers.ContainsKey(skillName)) {
                unitTargetHandlers[skillName] = new List<HandleUnitTargetSkill>();
            }
            unitTargetHandlers[skillName].Add(handler);
        }
    }
}

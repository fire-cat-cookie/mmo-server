using mmo_server.Communication;
using mmo_server.ControlTower;
using mmo_server.Gamestate;
using mmo_shared;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.MessageHandlers {
    class SkillHandler {
        private readonly PacketPublisher packetPublisher;
        private readonly PlayerService playerService;
        private readonly UnitVerificationService unitVerificationService;
        private readonly SkillPublisher skillPublisher;
        private readonly CooldownService cooldownService;

        public SkillHandler(PacketPublisher packetPublisher, PlayerService playerService, UnitVerificationService unitVerificationService,
            SkillPublisher skillPublisher, CooldownService cooldownService) {
            this.packetPublisher = packetPublisher;
            this.playerService = playerService;
            this.unitVerificationService = unitVerificationService;
            this.skillPublisher = skillPublisher;
            this.cooldownService = cooldownService;

            packetPublisher.Subscribe(typeof(NoTargetSkill), HandleNoTarget);
            packetPublisher.Subscribe(typeof(GroundTargetSkill), HandleGroundTarget);
            packetPublisher.Subscribe(typeof(UnitTargetSkill), HandleUnitTarget);
        }

        private void HandleNoTarget(Message message, IPEndPoint source) {
            if (!VerifyMessage(message, source, out ActiveCharacter c)) {
                return;
            }

            NoTargetSkill skillUse = message as NoTargetSkill;
            if (!VerifySkill(skillUse.SkillId, out Skill skill, c)) {
                return;
            }

            skillPublisher.Publish(skill, c, null, null);
        }

        private void HandleGroundTarget(Message message, IPEndPoint source) {
            if (!VerifyMessage(message, source, out ActiveCharacter c)) {
                return;
            }

            GroundTargetSkill skillUse = message as GroundTargetSkill;
            if (!VerifySkill(skillUse.SkillId, out Skill skill, c)) {
                return;
            }

            skillPublisher.Publish(skill, c, skillUse.Target, null);
        }

        private void HandleUnitTarget(Message message, IPEndPoint source) {
            if (!VerifyMessage(message, source, out ActiveCharacter c)) {
                return;
            }

            UnitTargetSkill skillUse = message as UnitTargetSkill;
            if (!VerifySkill(skillUse.SkillId, out Skill skill, c)) {
                return;
            }

            skillPublisher.Publish(skill, c, null, skillUse.Target);
        }

        private bool VerifyMessage(Message message, IPEndPoint source, out ActiveCharacter c) {
            c = null;
            Player player = playerService.FindPlayer(source);
            if (player == null) {
                return false;
            }
            if (player.CurrentCharacter == null) {
                return false;
            }
            c = player.CurrentCharacter;
            if (!unitVerificationService.CanUseSkills(player.CurrentCharacter)) {
                return false;
            }
            return true;
        }

        private bool VerifySkill(uint skillId, out Skill skill, ActiveCharacter c) {
            if (skillId >= SkillData.skills.Length) {
                skill = null;
                return false;
            }
            skill = SkillData.skills[skillId];
            return !cooldownService.OnCooldown(skill, c);
        }
    }
}

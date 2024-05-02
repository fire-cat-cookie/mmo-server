using System;
using mmo_shared.Messages;

namespace mmo_shared {
    public class ServerMessages {

        // maps packet IDs to message types.
        public static readonly Type[] PacketTypes = new Type[] {
            typeof(LoginResponse),
            typeof(SessionStart),
            typeof(ServerDisconnect),
            typeof(RegistrationResponse),
            typeof(CharSelectInfo),
            typeof(CreateCharacterResponse),
            typeof(PositionUpdate),
            typeof(CharSlotInfo),
            typeof(LoginCharacterResponse),
            typeof(CharInfo),
            typeof(ServerChatMessage),
            typeof(HealthChange),
            typeof(ServerAutoAttack),
            typeof(UnitDie),
            typeof(InterruptAttack),
            typeof(UnitRevive),
            typeof(ServerGroundTargetSkill),
            typeof(ServerUnitTargetSkill),
            typeof(ServerNoTargetSkill),
        };
    }
}

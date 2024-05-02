using mmo_shared.Messages;
using System;

namespace mmo_shared {
    public class ClientMessages {

        // maps packet IDs to message types.
        public static readonly Type[] PacketTypes = new Type[] {
            typeof(Login),
            typeof(SessionIdRequest),
            typeof(ClientDisconnect),
            typeof(Alive),
            typeof(CreateAccount),
            typeof(LoginCharacter),
            typeof(CreateCharacter),
            typeof(MoveCommand),
            typeof(ClientChatMessage),
            typeof(AutoAttack),
            typeof(GetSurroundings),
            typeof(ClientRespawn),
            typeof(GroundTargetSkill),
            typeof(NoTargetSkill),
            typeof(UnitTargetSkill),
        };
    }
}

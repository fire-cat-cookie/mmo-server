
namespace mmo_shared.Messages {
    public class ServerAutoAttack : Message {
        [Order(0)] public uint SourcePlayerId { get; set; }
        [Order(1)] public uint TargetPlayerId { get; set; }

        public ServerAutoAttack() { }

        public ServerAutoAttack(uint sourcePlayerId, uint targetPlayerId) {
            SourcePlayerId = sourcePlayerId;
            TargetPlayerId = targetPlayerId;
        }
    }
}
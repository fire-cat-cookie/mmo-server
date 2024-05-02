
namespace mmo_shared.Messages {
    public class AutoAttack : Message {
        [Order(0)] public uint TargetPlayerId { get; set; }

        public AutoAttack() { }

        public AutoAttack(uint targetPlayerId) {
            TargetPlayerId = targetPlayerId;
        }
    }
}

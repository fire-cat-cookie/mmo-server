
namespace mmo_shared.Messages {
    public class ServerDisconnect : Message {
        [Order(0)] public uint PlayerId { get; set; }

        public ServerDisconnect() { }

        public ServerDisconnect(uint playerId) {
            this.PlayerId = playerId;
        }
    }
}

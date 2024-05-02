
namespace mmo_shared.Messages {
    public class SessionStart : Message {
        [Order(0)] public byte[] SessionId { get; set; }
    }
}

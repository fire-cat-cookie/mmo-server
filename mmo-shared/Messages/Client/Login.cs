
namespace mmo_shared.Messages {
    public class Login : Message{
        [Order(0)] public string Password { get; set; }
        [Order(1)] public string Username { get; set; }
        [Order(2)] public byte[] SessionId { get; set; }
    }
}

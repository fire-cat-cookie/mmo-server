
namespace mmo_shared.Messages {
    public class CreateAccount : Message {
        [Order(0)] public string Password { get; set; }
        [Order(1)] public string Username { get; set; }
        [Order(2)] public byte[] SessionId { get; set; }

        public CreateAccount() { }

        public CreateAccount(string user, string pass, byte[] sessionId) {
            Username = user;
            Password = pass;
            SessionId = sessionId;
        }
    }
}

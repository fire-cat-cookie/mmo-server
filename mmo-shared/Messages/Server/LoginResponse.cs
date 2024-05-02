
namespace mmo_shared.Messages {
    public class LoginResponse : Message {
        public enum Types : byte{
            IncorrectUsernameOrPassword = 0,
            Success = 1,
            AlreadyLoggedIn = 2
        }

        [Order(0)] public byte Type { get; set; }
    }
}

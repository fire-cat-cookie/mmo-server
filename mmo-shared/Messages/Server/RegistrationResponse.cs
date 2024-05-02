
namespace mmo_shared.Messages {
    public class RegistrationResponse : Message {
        public enum Types : byte{
            Invalid = 0,
            Success = 1,
            UsernameTaken = 2,
            UsernameInvalid = 3,
            PasswordInvalid = 4
        }

        [Order(0)] public byte ResponseType { get; set; }
        
    }
}

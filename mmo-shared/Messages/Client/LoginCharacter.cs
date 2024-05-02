
namespace mmo_shared.Messages {
    public class LoginCharacter : Message{
        [Order(0)]public byte Slot { get; set; }
    }
}

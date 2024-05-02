
namespace mmo_shared.Messages {
    public class CreateCharacter : Message {
        [Order(0)] public byte Slot { get; set; }
        [Order(1)] public string Name { get; set; }
    }
}

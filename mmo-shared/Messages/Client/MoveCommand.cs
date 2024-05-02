
namespace mmo_shared.Messages {
    public class MoveCommand : Message {
        [Order(0)] public Vector2 Target { get; set; }
    }
}

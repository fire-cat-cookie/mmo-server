
namespace mmo_shared.Messages {
     public class PositionUpdate : Message{

        [Order(0)] public uint PlayerId { get; set; }
        [Order(1)] public Vector2 Position { get; set; }
        [Order(2)] public Vector2 Destination { get; set; }
        [Order(3)] public float Velocity { get; set; }

        public PositionUpdate() { }

        public PositionUpdate(uint playerId, Vector2 position, Vector2 destination, float velocity) {
            PlayerId = playerId;
            Position = position;
            Destination = destination;
            Velocity = velocity;
        }
    }
}

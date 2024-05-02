
namespace mmo_shared.Messages {
    public class HealthChange : Message{
        [Order(0)] public uint PlayerId { get; set; }
        [Order(1)] public float NewHealth { get; set; }

        public HealthChange() { }

        public HealthChange(uint playerId, float newHealth) {
            PlayerId = playerId;
            NewHealth = newHealth;
        }
    }
}

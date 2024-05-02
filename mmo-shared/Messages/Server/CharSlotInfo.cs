
namespace mmo_shared.Messages {
    public class CharSlotInfo : Message{
        [Order(0)] public string Name { get; set; }
        [Order(1)] public ushort Level { get; set; }
        [Order(2)] public ushort Class { get; set; }
        [Order(3)] public uint Location { get; set; }
        [Order(4)] public byte SlotId { get; set; }

        public CharSlotInfo() { }

        public CharSlotInfo(string name, ushort level, ushort classId, uint location, byte slot) {
            Name = name;
            Level = level;
            Class = classId;
            Location = location;
            SlotId = slot;
        }
    }
}

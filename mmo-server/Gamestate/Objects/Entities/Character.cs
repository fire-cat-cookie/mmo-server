using System;
using System.Collections.Generic;

namespace mmo_server.Gamestate;

public partial class Character
{
    public uint Id { get; set; }

    public uint AccountId { get; set; }

    public byte Slot { get; set; }

    public string Name { get; set; }

    public uint ZoneId { get; set; }

    public ushort PositionX { get; set; }

    public ushort PositionY { get; set; }

    public byte Class { get; set; }

    public ushort Level { get; set; }

    public virtual Account Account { get; set; }
}

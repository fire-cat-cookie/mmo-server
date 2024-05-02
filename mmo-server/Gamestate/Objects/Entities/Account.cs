using System;
using System.Collections.Generic;

namespace mmo_server.Gamestate;

public partial class Account
{
    public uint Id { get; set; }

    public string Username { get; set; }

    public byte[] Password { get; set; }

    public virtual ICollection<Character> Characters { get; } = new List<Character>();
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class UnitDie : Message{
        [Order(0)] public uint PlayerId { get; set; }

        public UnitDie() { }

        public UnitDie(uint playerId) {
            PlayerId = playerId;
        }
    }
}

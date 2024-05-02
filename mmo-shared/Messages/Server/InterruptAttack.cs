using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class InterruptAttack : Message{
        [Order(0)] public uint UnitId { get; set; }

        public InterruptAttack() { }

        public InterruptAttack(uint unitId) {
            UnitId = unitId;
        }
    }
}

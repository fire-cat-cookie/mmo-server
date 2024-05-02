using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class UnitRevive : Message{
        [Order(0)] public uint UnitId { get; set; }

        public UnitRevive() { }

        public UnitRevive(uint unitId) {
            UnitId = unitId;
        }
    }
}

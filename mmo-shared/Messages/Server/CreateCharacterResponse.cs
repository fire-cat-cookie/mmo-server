using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class CreateCharacterResponse : Message {
        public enum Types : byte {
            Invalid = 0,
            Success = 1,
            NameTaken = 2,
            NameInvalid = 3
        }

        [Order(0)] public byte Response { get; set; }
        [Order(1)] public CharSlotInfo CharInfo { get; set; }
    }
}

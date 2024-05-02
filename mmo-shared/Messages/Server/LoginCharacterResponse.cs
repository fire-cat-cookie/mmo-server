using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class LoginCharacterResponse : Message {
        public enum Types : byte {
            Error = 0,
            Success = 1
        }

        [Order(0)] public byte Type { get; set; }
        [Order(1)] public uint PlayerId { get; set; }

        public LoginCharacterResponse() { }

        public LoginCharacterResponse(byte type, uint actorId) {
            Type = type;
            PlayerId = actorId;
        }
    }
}

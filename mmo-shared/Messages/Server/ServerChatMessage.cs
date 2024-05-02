using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class ServerChatMessage : Message{
        [Order(0)] public string Text { get; set; }
        [Order(1)] public uint SenderId { get; set; }

        public ServerChatMessage() { }

        public ServerChatMessage(string text, uint senderId) {
            Text = text;
            SenderId = senderId;
        }
    }
}

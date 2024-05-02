using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_shared.Messages {
    public class ClientChatMessage : Message{
        [Order(0)] public string Text { get; set; }

        public ClientChatMessage() { }

        public ClientChatMessage(string text) {
            Text = text;
        }
    }
}

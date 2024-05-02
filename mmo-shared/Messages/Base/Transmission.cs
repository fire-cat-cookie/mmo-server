using System.Net;

namespace mmo_shared.Messages {
    public class Transmission {
        public Message Message { get; protected set; }
        public IPEndPoint Source { get; protected set; }

        public Transmission(Message message, IPEndPoint source) {
            Message = message;
            Source = source;
        }

    }
}

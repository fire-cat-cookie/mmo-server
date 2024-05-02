using mmo_shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using mmo_shared.Messages;

namespace mmo_server.Communication {
    /// <summary>
    /// Provides a framework for publishing incoming client messages for other services to subscribe to.
    /// </summary>
    class PacketPublisher {
        public delegate void HandleMessage(Message message, IPEndPoint source);

        private ConcurrentQueue<Transmission> messageBuffer = new ConcurrentQueue<Transmission>();
        private Dictionary<Type, List<HandleMessage>> packetHandlers = new Dictionary<Type, List<HandleMessage>>();

        public void Publish(Message message, IPEndPoint source) {
            if (packetHandlers.ContainsKey(message.GetType())) {
                messageBuffer.Enqueue(new Transmission(message, source));
            }
        }

        public void Subscribe(Type packetType, HandleMessage handler) {
            if (!packetHandlers.ContainsKey(packetType)) {
                packetHandlers[packetType] = new List<HandleMessage>();
            }
            packetHandlers[packetType].Add(handler);
        }

        public void Update() {
            while (!messageBuffer.IsEmpty) {
                messageBuffer.TryDequeue(out Transmission trans);
                foreach (HandleMessage handler in packetHandlers[trans.Message.GetType()]) {
                    handler.Invoke(trans.Message, trans.Source);
                }
            }
        }
    }
}

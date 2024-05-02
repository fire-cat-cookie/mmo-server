using System;
using System.Collections.Generic;

namespace mmo_shared {
    public class MessageRepository {

        // maps message types to packetIds.
        private static Dictionary<Type, ushort> _PacketIds;
        private static Dictionary<Type, ushort> PacketIds {
            get {
                if (_PacketIds != null) {
                    return _PacketIds;
                } else {
                    _PacketIds = new Dictionary<Type, ushort>();
                    for (int i = 0; i < ServerMessages.PacketTypes.Length; i++) {
                        _PacketIds[ServerMessages.PacketTypes[i]] = (ushort)i;
                    }
                    for (int i = 0; i < ClientMessages.PacketTypes.Length; i++) {
                        _PacketIds[ClientMessages.PacketTypes[i]] = (ushort)i;
                    }
                    return _PacketIds;
                }
            }
        }

        /// <summary>
        /// returns the packet id for the given type, or null if no packet id is assigned to the given type.
        /// </summary>
        public static UInt16? GetPacketId(Type packetType) {
            if (!PacketIds.ContainsKey(packetType)) {
                return null;
            }
            return PacketIds[packetType];
        }

        /// <summary>
        /// returns the packet type for the given id, or null if the packet id is invalid.
        /// </summary>
        public static Type GetServerPacketType(ushort packetId) {
            if (packetId > ServerMessages.PacketTypes.Length) {
                return null;
            }
            return ServerMessages.PacketTypes[packetId];
        }

        /// <summary>
        /// returns the packet type for the given id, or null if the packet id is invalid.
        /// </summary>
        public static Type GetClientPacketType(ushort packetId) {
            if (packetId > ClientMessages.PacketTypes.Length) {
                return null;
            }
            return ClientMessages.PacketTypes[packetId];
        }
    }
}

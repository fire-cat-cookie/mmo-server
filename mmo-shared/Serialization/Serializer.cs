using mmo_shared.Messages;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace mmo_shared {
    public class Serializer{

        private UnicodeEncoding unicodeEncoding = new UnicodeEncoding();

        /// <summary>
        /// <para>Turns a message instance into a byte array of the following format:</para>
        /// 
        /// <para>The first bytes are occupied by the client header, which consists of the packet id for the message,
        /// as determined in <see cref="ClientMessages"/> and <see cref="ServerMessages"/>.</para>
        /// 
        /// <para>Following that is a byte representation of the values of each property.
        /// The property types and names are not serialized. Instead, properties are identified by their <see cref="OrderAttribute"/>. </para>
        /// 
        /// <para>Strings are interpreted as unicode, and are preceded by two bytes indicating the length of the string in bytes.
        /// As such, strings cannot be longer than 2^16 bytes.</para>
        /// 
        /// <para>byte[] values are preceded by 4 bytes indicating the length of the array.</para>
        /// </summary>
        /// <exception cref="SerializationException">if no serialization exists for the given properties and values.</exception>
        public byte[] Serialize(Message msg) {
            return Serialize(msg, false);
        }

        private byte[] Serialize(Message msg, bool skipHeader) {
            PropertyInfo[] properties = GetSortedProperties(msg.GetType());
            using (MemoryStream memStream = new MemoryStream()) {
                if (!skipHeader) {
                    byte[] header = GetHeader(msg.GetType());
                    memStream.Write(header, 0, header.Length);
                }
                foreach (var property in properties) {
                    SerializeProperty(property, msg, memStream);
                }
                return memStream.ToArray();
            }
        }

        /// <summary>
        /// serialize a property into binary format and write it into the provided stream.
        /// </summary>
        private void SerializeProperty(PropertyInfo property, Message msg, MemoryStream packet) {
            string propertyType = property.PropertyType.Name;
            object propertyValue = property.GetValue(msg);

            if (property.PropertyType.IsSubclassOf(typeof(Message))) {
                byte[] subPacket = Serialize((Message)property.GetValue(msg), false);
                packet.Write(subPacket, 0, subPacket.Length);
                return;
            }
            if (property.PropertyType.IsArray && !propertyType.Equals("String") && !propertyType.Equals("Byte[]")) {
                Array array = (Array)property.GetValue(msg);
                packet.Write(BitConverter.GetBytes(array.Length), 0, 4);
                foreach (var element in array) {
                    if (!(element is Message)) {
                        return;
                    }
                    byte[] subPacket = Serialize((Message) element, false);
                    packet.Write(subPacket, 0, subPacket.Length);
                }
                return;
            }

            switch (propertyType) {
                case "Vector2": {
                        float x = ((Vector2)propertyValue).X;
                        float y = ((Vector2)propertyValue).Y;
                        packet.Write(BitConverter.GetBytes(x), 0, 4);
                        packet.Write(BitConverter.GetBytes(y), 0, 4);
                        break;
                    }

                case "String": {
                    byte[] bytes = unicodeEncoding.GetBytes((string)propertyValue);
                    if (bytes.Length > UInt16.MaxValue) {
                        throw new SerializationException($"{msg.GetType().Name}.{property.Name}: string is too long ({bytes.Length} bytes)");
                    }
                    packet.Write(BitConverter.GetBytes((UInt16)bytes.Length), 0, 2);
                    packet.Write(bytes, 0, bytes.Length);
                    break;
                }

                case "Byte[]": {
                    byte[] bytes = (byte[])propertyValue;
                    packet.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                    packet.Write(bytes, 0, bytes.Length);
                    break;
                }

                case "Boolean": {
                    packet.Write(BitConverter.GetBytes((bool)propertyValue), 0, 1);
                    break;
                }

                case "Char": {
                    packet.Write(BitConverter.GetBytes((char)propertyValue), 0, 2);
                    break;
                }

                case "Single": {
                    packet.Write(BitConverter.GetBytes((float)propertyValue), 0, 4);
                    break;
                }

                case "Double": {
                    packet.Write(BitConverter.GetBytes((double)propertyValue), 0, 8);
                    break;
                }

                case "Int16": {
                    packet.Write(BitConverter.GetBytes((short)propertyValue), 0, 2);
                    break;
                }

                case "UInt16": {
                    packet.Write(BitConverter.GetBytes((ushort)propertyValue), 0, 2);
                    break;
                }

                case "Int32": {
                    packet.Write(BitConverter.GetBytes((int)propertyValue), 0, 4);
                    break;
                }

                case "UInt32": {
                    packet.Write(BitConverter.GetBytes((uint)propertyValue), 0, 4);
                    break;
                }

                case "Byte": {
                    packet.Write(new byte[] { (byte) propertyValue }, 0, 1);
                    break;
                }

                default:
                    break;
            }
        }

        /// <summary>
        /// Turn a byte array into a message instance. If the received byte array is invalid, return null.
        /// </summary>
        public Message Deserialize(byte[] packet, bool sourceIsClient) {
            return Deserialize(packet, 0, out int endOffset, sourceIsClient);
        }

        private Message Deserialize(byte[] packet, int beginOffset, out int endOffset, bool sourceIsClient) {
            int currentOffset = beginOffset;
            Type type = GetType(packet, currentOffset, sourceIsClient);
            if (type == null) {
                endOffset = currentOffset;
                return null;
            }
            Message instance = (Message)Activator.CreateInstance(type);
            currentOffset += GetHeader(type).Length;
            PropertyInfo[] properties = GetSortedProperties(type);
            for(int i=0; i<properties.Length; i++) {
                object value = DeserializeProperty(packet, currentOffset, out int newOffset, properties[i], sourceIsClient);
                currentOffset = newOffset;
                if (value != null) {
                    properties[i].SetValue(instance, value);
                } else {
                    endOffset = currentOffset;
                    return null;
                }
            }
            endOffset = currentOffset;
            return instance;
        }

        private object DeserializeProperty(byte[] packet, int offset, out int newOffset, PropertyInfo property, bool sourceIsClient) {
            
            string propertyType = property.PropertyType.Name;
            newOffset = offset;

            if (property.PropertyType.IsSubclassOf(typeof(Message))) {
                object sub = Deserialize(packet, offset, out int endOffset, sourceIsClient);
                newOffset = endOffset;
                return sub;
            }
            if (property.PropertyType.IsArray && !propertyType.Equals("String") && !propertyType.Equals("Byte[]")) {
                int arrayLength = BitConverter.ToInt32(packet, offset);
                newOffset += 4;
                Type arrayType = property.PropertyType.GetElementType();
                if (!arrayType.IsSubclassOf(typeof(Message))){
                    return null;
                }
                dynamic array = Array.CreateInstance(arrayType, arrayLength);
                for (int i=0; i<arrayLength; i++) {
                    array[i] = DynamicCast(Deserialize(packet, newOffset, out int endOffset, sourceIsClient), arrayType);
                    newOffset = endOffset;
                }
                return array;
            }

            switch (propertyType) {
                case "Vector2": {
                        if (packet.Length < 8 + offset) {
                            return null;
                        }
                        float x = BitConverter.ToSingle(packet, offset);
                        float y = BitConverter.ToSingle(packet, offset + 4);
                        newOffset = offset + 8;
                        return new Vector2(x, y);
                    }

                case "String": {
                        if (packet.Length < 2 + offset) {
                            return null;
                        }
                        ushort length = BitConverter.ToUInt16(packet, offset);
                        if (packet.Length < 2 + offset + length) {
                            return null;
                        }
                        newOffset = offset + 2 + length;
                        return unicodeEncoding.GetString(packet, offset + 2, length);
                    }

                case "Byte[]": {
                        if (packet.Length < 4 + offset) {
                            return null;
                        }
                        int length = BitConverter.ToInt32(packet, offset);
                        if (packet.Length < 4 + offset + length) {
                            return null;
                        }
                        byte[] result = new byte[length];
                        Array.Copy(packet, offset + 4, result, 0, length);
                        newOffset = offset + 4 + length;
                        return result;
                    }

                case "Boolean": {
                        if (packet.Length < 1 + offset) {
                            return null;
                        }
                        newOffset += 1;
                        return BitConverter.ToBoolean(packet, offset);
                    }

                case "Char": {
                        if (packet.Length < 2 + offset) {
                            return null;
                        }
                        newOffset += 2;
                        return BitConverter.ToChar(packet, offset);
                    }

                case "Single": {
                        if (packet.Length < 4 + offset) {
                            return null;
                        }
                        newOffset += 4;
                        return BitConverter.ToSingle(packet, offset);
                    }

                case "Double": {
                        if (packet.Length < 8 + offset) {
                            return null;
                        }
                        newOffset += 8;
                        return BitConverter.ToDouble(packet, offset);
                    }

                case "Int16": {
                        if (packet.Length < 2 + offset) {
                            return null;
                        }
                        newOffset += 2;
                        return BitConverter.ToInt16(packet, offset);
                    }

                case "UInt16": {
                        if (packet.Length < 2 + offset) {
                            return null;
                        }
                        newOffset += 2;
                        return BitConverter.ToUInt16(packet, offset);
                    }

                case "Int32": {
                        if (packet.Length < 4 + offset) {
                            return null;
                        }
                        newOffset += 4;
                        return BitConverter.ToInt32(packet, offset);
                    }

                case "UInt32": {
                        if (packet.Length < 4 + offset) {
                            return null;
                        }
                        newOffset += 4;
                        return BitConverter.ToUInt32(packet, offset);
                    }

                case "Byte": {
                        if (packet.Length < 1 + offset) {
                            return null;
                        }
                        newOffset += 1;
                        return packet[offset];
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns the packet id from the packet, or null if that fails.
        /// </summary>
        private ushort? GetPacketId(byte[] packet, int offset) {
            if (packet.Length < 2 + offset) {
                return null;
            }
            return BitConverter.ToUInt16(packet, offset);
        }

        /// <summary>
        /// Returns the type of the packet, or null if the packet is invalid.
        /// </summary>
        public Type GetType(byte[] packet, int offset, bool sourceIsClient) {
            ushort? id = GetPacketId(packet, offset);
            if (!id.HasValue) {
                return null;
            } else if (sourceIsClient) {
                return MessageRepository.GetClientPacketType(id.Value);
            } else {
                return MessageRepository.GetServerPacketType(id.Value);
            }
        }

        /// <summary>
        /// Get the header of a given packet type, or an empty array if the type is invalid.
        /// </summary>
        public byte[] GetHeader(Type type) {
            ushort? id = MessageRepository.GetPacketId(type);
            if (!id.HasValue) {
                return new byte[0];
            }
            return BitConverter.GetBytes(id.Value);
        }

        //adapted from https://stackoverflow.com/a/9064293
        private PropertyInfo[] GetSortedProperties(Type type) {
            return type
              .GetProperties()
              .OrderBy(p => ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order)
              .ToArray<PropertyInfo>();
        }

        // taken from https://stackoverflow.com/a/4925924
        private dynamic DynamicCast(object entity, Type to) {
            var openCast = this.GetType().GetMethod("Cast", BindingFlags.Static | BindingFlags.NonPublic);
            var closeCast = openCast.MakeGenericMethod(to);
            return closeCast.Invoke(entity, new[] { entity });
        }

        // taken from https://stackoverflow.com/a/4925924
        private static T Cast<T>(object entity) where T : class {
            return entity as T;
        }
    }
}

using mmo_server.Cryptography;
using mmo_shared;
using mmo_shared.Messages;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace mmo_server.Communication; 
/// <summary>
/// Receives packets from <see cref="ClientConnector"/>, and decides which packets should be published (made available to the game layer).
/// </summary>
class PacketHandler {

    private ClientConnector clients;
    private Serializer serializer;
    private PacketEncryption encryption;
    private readonly PacketPublisher publisher;

    public PacketHandler(ClientConnector clients, Serializer serializer, PacketEncryption encryption,
        PacketPublisher publisher) {
        this.clients = clients;
        this.serializer = serializer;
        this.encryption = encryption;
        this.publisher = publisher;
    }

    /// <summary>
    /// Starts listening to received packets from the <see cref="ClientConnector"/>
    /// </summary>
    public void Start() {
        clients.PacketReceived += OnPacketReceived;
    }

    /// <summary>
    /// If the packet has a known packet type, decrypt and deserialize it, and publish it.
    /// </summary>
    private void OnPacketReceived(byte[] packet, IPEndPoint source) {
        packet = encryption.Decrypt(packet, 0);

        Type packetType = serializer.GetType(packet, 0, true);
        if (packetType == null) {
            return;
        }

        Message message = serializer.Deserialize(packet, true);
        if (message == null) {
            return;
        }
        
        publisher.Publish(message, source);
    }

}

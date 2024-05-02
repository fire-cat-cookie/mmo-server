using mmo_server.Debug;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace mmo_server.Communication {
    public class ClientConnector{
        private const int SERVER_PORT = 2057;

        UdpClient udpServer;

        public delegate void ReceivePacket(byte[] packet, IPEndPoint source);
        public event ReceivePacket PacketReceived = delegate { };

        public ClientConnector() {
        }

        //prevent SocketException when a client disconnects
        private void IgnoreClosedConnections() {
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            udpServer.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
        }

        public void Start() {
            udpServer = new UdpClient(SERVER_PORT);
            IgnoreClosedConnections();
            Thread receiverThread = new Thread(Receive);
            receiverThread.Start();
            Debug.Debug.Log("Accepting packets", Debug.Debug.MessageTypes.Default);
        }

        private void Receive() {
            try {
                while (true) {
                    IPEndPoint remote = null;
                    byte[] packet = udpServer.Receive(ref remote);
                    //Logger.Write("Packet received from " + remote.Address + ":" + remote.Port, Logger.MessageTypes.Default);
                    PacketReceived(packet, remote);
                }
            } catch(SocketException e) {
                Debug.Debug.Log("ClientConnector: " + e.Message, Debug.Debug.MessageTypes.Default);
            }
        }

        public void SendTo(IPEndPoint remote, byte[] data) {
            //Logger.Write("Sending packet to " + remote.Address + ":" + remote.Port, Logger.MessageTypes.Default);
            udpServer.Send(data, data.Length, remote);
        }

        public void SendTo(List<IPEndPoint> clients, byte[] data) {
            foreach (IPEndPoint ip in clients) {
                SendTo(ip, data);
            }
        }

    }
}

using System.Net;
using System.Net.Sockets;

namespace ACDashboard
{
    class UDPConnection
    {
        private string HOST;
        private int PORT;
        private UdpClient socket;
        private IPEndPoint ipEndpoint;

        public UDPConnection(string _HOST, int _PORT)
        {
            HOST = _HOST;
            PORT = _PORT;

            ipEndpoint = new IPEndPoint(IPAddress.Parse(HOST), PORT);
            socket = new UdpClient();
            socket.Connect(ipEndpoint);
        }

        public void Send(byte[] data)
        {
            socket.Send(data, data.Length);
        }

        public byte[] Receive()
        {
            return socket.Receive(ref ipEndpoint);
        }
    }
}

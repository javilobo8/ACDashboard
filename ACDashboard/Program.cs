using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ACDashboard
{
    class Program
    {
        public static int PORT = 9996;
        public static string HOST = "127.0.0.1";
        public static UdpClient udpSocket;
        public static IPEndPoint ipEndpoint;
        public static Thread udpThread;
        public static bool isConnected = false;
        static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            Console.WriteLine("Init");
            ipEndpoint = new IPEndPoint(IPAddress.Parse(HOST), PORT);
            udpSocket = new UdpClient();
            udpSocket.Connect(ipEndpoint);

            udpThread = new Thread(() => Receive());
            udpThread.Start();

            SendHandshake(ACStructs.handshaker.HandshakeOperation.Connect);
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                SendHandshake(ACStructs.handshaker.HandshakeOperation.Disconnect);
                udpThread.Abort();
                Console.WriteLine("Console window closing, death imminent");
                Thread.Sleep(500);
            }
            return false;
        }

        static void SendBytes(byte[] data)
        {
            udpSocket.Send(data, data.Length);
        }

        static void SendHandshake(ACStructs.handshaker.HandshakeOperation operationId)
        {
            ACStructs.handshaker hs = new ACStructs.handshaker(operationId);
            SendBytes(ACStructs.structToBytes(hs));
        }

        static void Receive()
        {
            while(true)
            {
                byte[] packet = udpSocket.Receive(ref ipEndpoint);

                if (!isConnected)
                {
                    Debug.Assert(packet.Length == Marshal.SizeOf<ACStructs.handshakerResponse>());

                    ACStructs.handshakerResponse response = ACStructs.bytesToStruct<ACStructs.handshakerResponse>(packet);
                    Console.WriteLine(response.carName);
                    Console.WriteLine(response.driverName);
                    Console.WriteLine(response.identifier);
                    Console.WriteLine(response.trackName);
                    SendHandshake(ACStructs.handshaker.HandshakeOperation.CarInfo);
                    isConnected = true;
                }
                else
                {
                    ACStructs.RTCarInfo rtcar = ACStructs.bytesToStruct<ACStructs.RTCarInfo>(packet);

                    Console.WriteLine("speed_Kmh {0}", rtcar.speed_Kmh);
                    SendHandshake(ACStructs.handshaker.HandshakeOperation.CarInfo);
                }
                Console.WriteLine("-----------END-PACKET-----------");
            }
        }
    }
}

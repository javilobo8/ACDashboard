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
using AssettoCorsaSharedMemory;

namespace ACDashboard
{
    class Program
    {
        public static SerialConnection serialConnection;
        public static ACClient acClient;
        public static AssettoCorsa assetoCorsa;

        static void Main(string[] args)
        {
            serialConnection = new SerialConnection(115200, 64);
            serialConnection.Connect(SerialConnection.GetArdunioPort());
            acClient = new ACClient(serialConnection);

            assetoCorsa = new AssettoCorsa();
            assetoCorsa.GraphicsInterval = 10;
            assetoCorsa.StaticInfoUpdated += acClient.StaticInfoUpdated;
            assetoCorsa.PhysicsUpdated += acClient.PhysicsUpdated;
            assetoCorsa.GraphicsUpdated += acClient.GraphicsUpdated;
            assetoCorsa.Start();

            Console.ReadKey();
        }
    }
}

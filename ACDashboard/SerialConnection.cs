using System;
using System.IO.Ports;

namespace ACDashboard
{
    class SerialConnection
    {
        public int BAUD_RATE = 115200;
        public int FPS = 1;

        public String Port { get; set; }
        private SerialPort sp;
        public bool isConnected = false;
        public double now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        public double lastPacketDate = 0;
        public double rate;

        public static string[] GetAvailablePorts() { return SerialPort.GetPortNames(); }
        public static string GetArdunioPort()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                throw new Exception("No COM ports");
            }

            if (ports.Length == 1)
            {
                Console.WriteLine("{0} selected", ports[0]);
                return ports[0];
            }

            Console.WriteLine("Select COM port:");
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine("  {0}) {1}", i + 1, ports[i]);
            }

            Console.Write("Port: ");
            ConsoleKeyInfo UserInput = Console.ReadKey(false);

            if (!char.IsDigit(UserInput.KeyChar))
            {
                throw new Exception("Wrong selection");
            }

            int selected = int.Parse(UserInput.KeyChar.ToString()) - 1;
            Console.WriteLine("\n{0} selected", ports[selected]);
            return ports[selected];
        }

        public SerialConnection(int _BAUD_RATE, int _FPS)
        {
            BAUD_RATE = _BAUD_RATE;
            FPS = _FPS;
            rate = 1000 / FPS;
        }

        public void Connect(String _port)
        {
            Port = _port;
            sp = new SerialPort(Port, BAUD_RATE);
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            sp.Open();
            isConnected = true;
        }

        public void Disconnect()
        {
            if (!isConnected) return;
            sp.Close();
            isConnected = false;
        }

        public void Write(byte[] bytes)
        {
            if (!isConnected || !CanSend()) return;
            sp.Write(bytes, 0, bytes.Length);
        }

        public bool CanSend()
        {
            now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            if (now > lastPacketDate + rate)
            {
                lastPacketDate = now;
                return true;
            }
            return false;
        }

        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Console.Write(sp.ReadExisting());
        }
    }
}

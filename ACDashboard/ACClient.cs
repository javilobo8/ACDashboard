using AssettoCorsaSharedMemory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACDashboard
{
    class ACClient
    {
        public SerialConnection serialConnection;
        private NeoPixelRPM neoPixelRpm;

        private ACStructs.SerialStruct ArduinoData = new ACStructs.SerialStruct
        {
            led_color = new UInt32[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        public ACClient(SerialConnection _serialConnection)
        {
            serialConnection = _serialConnection;
            neoPixelRpm = new NeoPixelRPM(16, 64);
        }

        public void PhysicsUpdated(object sender, PhysicsEventArgs e)
        {
            ArduinoData.led_color = neoPixelRpm.CalcOppositeLeds(e.Physics.Rpms);
            serialConnection.Write(ACStructs.structToBytes(ArduinoData));
        }

        public void StaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            neoPixelRpm.rpm_max = e.StaticInfo.MaxRpm;
            neoPixelRpm.rpm_min = 74f * e.StaticInfo.MaxRpm / 100f;
        }
    }
}

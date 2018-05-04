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
        private NeoPixel neoPixelRpm;
        private SevenSegment sevenSegment;
        private byte[][] GEARS = Constants.Rotate(Constants.STR_GEARS, 2);

        private ACStructs.SerialStruct arduinoData = new ACStructs.SerialStruct
        {
            led_color = new UInt32[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            matrix = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, },
            digit = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, },
        };

        public ACClient(SerialConnection _serialConnection)
        {
            serialConnection = _serialConnection;
            neoPixelRpm = new NeoPixel(16, 255);
            sevenSegment = new SevenSegment();
        }

        public void Update()
        {
            serialConnection.Write(ACStructs.structToBytes(arduinoData));
        }

        public void PhysicsUpdated(object sender, PhysicsEventArgs e)
        {
            arduinoData.led_color = neoPixelRpm.CalcLinearLeds(e.Physics.Rpms);
            arduinoData.matrix = GEARS[e.Physics.Gear];
            serialConnection.Write(ACStructs.structToBytes(arduinoData));
        }

        public void StaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            neoPixelRpm.rpm_max = e.StaticInfo.MaxRpm;
            neoPixelRpm.rpm_min = 74f * e.StaticInfo.MaxRpm / 100f;
            //Update();
        }


        public void GraphicsUpdated(object sender, GraphicsEventArgs e)
        {
            arduinoData.digit = sevenSegment.ConvertToTime(e.Graphics.CurrentTime);
            //Utils.PrintByteArray(arduinoData.digit);
            //Console.WriteLine(e.Graphics.CurrentTime);
            //Update();
        }
    }
}

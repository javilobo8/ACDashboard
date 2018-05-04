using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDashboard
{
    class Utils
    {
        public static void PrintByteArray(byte[] data)
        {
            Console.WriteLine("-----BYTE-START-----");
            for (int i = 0; i < data.Length; ++i)
            {
                if (i % 4 == 0) Console.Write("\n");
                Console.Write(Convert.ToString(data[i], 2).PadLeft(8, '0') + " ");
            }
            Console.WriteLine("\n-----BYTE-END-------");
        }
    }
}

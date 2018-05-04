using System;

namespace ACDashboard
{
    class SevenSegment
    {
        public SevenSegment() { }

        public byte[] ConvertToTime(String time)
        {
            byte[] data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, };
            char[] chars = time.ToCharArray(); // 00:00:000

            int dataIndex = 0;
            bool isDot = false;
            for (int i = chars.Length - 1; i >= 0 && chars.Length != 0; i--)
            {
                if (chars[i] != ':')
                {
                    try
                    {
                        data[dataIndex] = Constants.NUMBERS[(int)Char.GetNumericValue(chars[i])];
                        if (isDot)
                        {
                            data[dataIndex] += 0b10000000;
                            isDot = false;
                        }
                    } catch { }
                    dataIndex++;
                } else
                {
                    isDot = true;
                }
            }

            return data;
        }

        public byte[] ConvertFromDecimal(float number)
        {
            byte[] data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, };
            char[] chars = ((int)number).ToString().ToCharArray();

            int dataIndex = 0;
            for (int i = chars.Length - 1; i >= 0 && chars.Length != 0; i--)
            {
                try
                {
                    data[dataIndex] = Constants.NUMBERS[(int)Char.GetNumericValue(chars[i])];
                }
                catch { }
                dataIndex++;
            }

            return data;
        }
    }
}

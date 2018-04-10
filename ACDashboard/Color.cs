using System;

namespace ACDashboard
{
    public class Color
    {
      public static UInt32 Green = 0x0000FF00;
      public static UInt32 Yellow = 0x00FFFF00;
      public static UInt32 Orange = 0x00FF7F00;
      public static UInt32 Red = 0x00FF0000;
      public static UInt32 Blue = 0x000000FF;
      public static UInt32 Black = 0x00000000;

      public static UInt32 ChangeBrightness(UInt32 color, int brightness)
        {
            UInt32 R = (color >> 16) & 0xff;
            UInt32 G = (color >>  8) & 0xff;
            UInt32 B = (color      ) & 0xff;
            R = (UInt32)(R * brightness) >> 8;
            G = (UInt32)(G * brightness) >> 8;
            B = (UInt32)(B * brightness) >> 8;

            UInt32 RGB = R;
            RGB = (RGB << 8) + G;
            RGB = (RGB << 8) + B;
            return RGB;
        }
    }
}
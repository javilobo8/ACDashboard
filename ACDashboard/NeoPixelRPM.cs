using System;

namespace ACDashboard
{
    class NeoPixelRPM
    {
        public static UInt32 C_GREEN = 0x0000FF00;
        public static UInt32 C_YELLOW = 0x00FFFF00;
        public static UInt32 C_ORANGE = 0x00FF7F00;
        public static UInt32 C_RED = 0x00FF0000;
        public static UInt32 C_BLUE = 0x000000FF;
        public static UInt32 C_BLACK = 0;

        public static UInt32[] NormalLayout = {
            C_GREEN, C_GREEN, C_GREEN, C_YELLOW, C_YELLOW, C_YELLOW, C_ORANGE, C_ORANGE,
            C_ORANGE, C_RED, C_RED, C_RED, C_BLUE, C_BLUE, C_BLUE, C_BLUE
        };

        public static UInt32[] SplitLayout = {
            C_GREEN, C_GREEN, C_YELLOW, C_ORANGE, C_RED, C_RED, C_BLUE, C_BLUE,
            C_BLUE, C_BLUE, C_RED, C_RED, C_ORANGE, C_YELLOW, C_GREEN, C_GREEN,
        };

        public static UInt32[] BlueLayout = {
            C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE,
            C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE, C_BLUE,
        };

        public int led_count;
        public int led_mult = 16;
        public int brightness;
        public float rpm_min;
        public float rpm_max;
        public float rpm_diff;
        public UInt32[] LED_BUFFER;

        public NeoPixelRPM(int _led_count, int _brightness, float _rpm_min, float _rpm_max)
        {
            led_count = _led_count;
            brightness = _brightness;
            rpm_min = _rpm_min;
            rpm_max = _rpm_max;
            rpm_diff = _rpm_max - _rpm_min;
            LED_BUFFER = new UInt32[_led_count];
        }

        public float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        public UInt32[] CalcRGBLeds(float rpm)
        {
            int leds = (int)map(rpm, rpm_min, rpm_max, 0f, (float)(led_count * led_mult));
            int leds_to_show = leds / led_mult;
            UInt32[] Layout = NormalLayout;

            if (leds_to_show > led_count - 3)
            {
                Layout = BlueLayout;
            }

            for (int current_led = 0; current_led < led_count; current_led++)
            {
                if (current_led < leds_to_show)
                {
                    // Current LED
                    LED_BUFFER[current_led] = Layout[current_led];

                    // Next LED
                    if (current_led == leds_to_show - 1 && current_led != led_count - 1)
                    {
                        int mod = leds % led_mult;
                        if (mod != 0)
                        {
                            int next_led = current_led + 1;
                            int brightness = (mod * 255 / led_count);
                            LED_BUFFER[next_led] = changeBrightness(Layout[next_led], brightness);
                        }
                    }
                }
                else if (current_led > leds_to_show)
                {
                    LED_BUFFER[current_led] = C_BLACK;
                }
            }
            return LED_BUFFER;
        }

        public UInt32[] CalcRGBLeds2(float rpm)
        {
            int leds = (int)map(rpm, rpm_min, rpm_max, 0f, (float)((led_count / 2) * led_mult));
            int leds_to_show = leds / led_mult;
            UInt32[] Layout = SplitLayout;

            for (int current_led = 0; current_led < led_count / 2; current_led++)
            {
                int opposite_led = led_count - 1 - current_led;
                if (current_led < leds_to_show)
                {
                    // Current LED
                    LED_BUFFER[current_led] = Layout[current_led];
                    LED_BUFFER[opposite_led] = Layout[opposite_led];
                    // Next LED
                    if (current_led == leds_to_show - 1 && current_led != (led_count / 2) - 1)
                    {
                        int mod = leds % led_mult;
                        if (mod != 0)
                        {
                            int next_led = current_led + 1;
                            int next_opposite_led = opposite_led - 1;
                            int brightness = (mod * 255 / led_count);
                            LED_BUFFER[next_led] = changeBrightness(Layout[next_led], brightness);
                            LED_BUFFER[next_opposite_led] = changeBrightness(Layout[next_opposite_led], brightness);
                        }
                    }
                }
                else if (current_led > leds_to_show)
                {
                    LED_BUFFER[current_led] = C_BLACK;
                    LED_BUFFER[opposite_led] = C_BLACK;
                }
            }
            return LED_BUFFER;
        }

        public UInt32 changeBrightness(UInt32 c, int brightness)
        {
            UInt32 r = (byte)(c >> 16);
            UInt32 g = (byte)(c >> 8);
            UInt32 b = (byte)c;
            r = (UInt32)(r * brightness) >> 8;
            g = (UInt32)(g * brightness) >> 8;
            b = (UInt32)(b * brightness) >> 8;
            UInt32 rgb = r;
            rgb = (rgb << 8) + g;
            rgb = (rgb << 8) + b;
            return rgb;
        }
    }
}

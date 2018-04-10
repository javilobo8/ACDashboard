using System;

namespace ACDashboard
{
    class NeoPixelRPM
    {
        public static UInt32[] NormalLayout = {
            Color.Green, Color.Green, Color.Green, Color.Yellow,
            Color.Yellow, Color.Yellow, Color.Orange, Color.Orange,
            Color.Orange, Color.Red, Color.Red, Color.Red,
            Color.Blue, Color.Blue, Color.Blue, Color.Blue
        };

        public static UInt32[] SplitLayout = {
            Color.Green, Color.Green, Color.Yellow, Color.Orange,
            Color.Red, Color.Red, Color.Blue, Color.Blue,
            Color.Blue, Color.Blue, Color.Red, Color.Red,
            Color.Orange, Color.Yellow, Color.Green, Color.Green,
        };

        public static UInt32[] BlueLayout = {
            Color.Blue, Color.Blue, Color.Blue, Color.Blue,
            Color.Blue, Color.Blue, Color.Blue, Color.Blue,
            Color.Blue, Color.Blue, Color.Blue, Color.Blue,
            Color.Blue, Color.Blue, Color.Blue, Color.Blue,
        };

        public int totalLeds;
        public int ledMultiplier = 16;
        public int maxBrightness;
        public float rpm_min = 0f;
        public float rpm_max = 20000f;

        public UInt32[] LED_BUFFER;

        public NeoPixelRPM(int _totalLeds, int _maxBrightness)
        {
            totalLeds = _totalLeds;
            maxBrightness = _maxBrightness;
            LED_BUFFER = new UInt32[_totalLeds];
        }

        public float map(float fromValue, float fromStart, float fromEnd, float toStart, float toEnd)
        {
            return toStart + (fromValue - fromStart) * (toEnd - toStart) / (fromEnd - fromStart);
        }

        public UInt32[] CalcLinearLeds(float rpm)
        {
            int ledsFactor = (int) map(rpm, rpm_min, rpm_max, 0f, (float)(totalLeds * ledMultiplier));
            int ledsToShow = ledsFactor / ledMultiplier;

            UInt32[] Layout = NormalLayout;

            if (ledsToShow > totalLeds - 3)
            {
                Layout = BlueLayout;
            }

            for (int currentLed = 0; currentLed < totalLeds; currentLed++)
            {
                if (currentLed < ledsToShow)
                {
                    // Current LED
                    LED_BUFFER[currentLed] = Layout[currentLed];

                    // Next LED
                    if (currentLed == ledsToShow - 1 && currentLed != totalLeds - 1)
                    {
                        int nextLedBrightnessFactor = ledsFactor % ledMultiplier;

                        if (nextLedBrightnessFactor != 0)
                        {
                            int nextLed = currentLed + 1;
                            int brightnessValue = (int) map(nextLedBrightnessFactor, 0, ledMultiplier, 0, maxBrightness);
                            // int brightnessFactor = (nextLedBrightnessFactor * 255 / totalLeds);
                            LED_BUFFER[nextLed] = Color.ChangeBrightness(Layout[nextLed], brightnessValue);
                        }
                    }
                }
                else if (currentLed > ledsToShow)
                {
                    LED_BUFFER[currentLed] = C_BLACK;
                }
            }
            return LED_BUFFER;
        }

        public UInt32[] CalcOppositeLeds(float rpm)
        {
            int halfLeds = totalLeds / 2;

            int ledsFactor = (int)map(rpm, rpm_min, rpm_max, 0f, (float)((halfLeds) * ledMultiplier));
            int ledsToShow = ledsFactor / ledMultiplier;

            UInt32[] Layout = SplitLayout;

            for (int currentLed = 0; currentLed < halfLeds; currentLed++)
            {
                int oppositeLed = totalLeds - 1 - currentLed;
                if (currentLed < ledsToShow)
                {
                    // Current LEDs
                    LED_BUFFER[currentLed] = Layout[currentLed];
                    LED_BUFFER[oppositeLed] = Layout[oppositeLed];

                    // Next LEDs for brightness
                    if (currentLed == ledsToShow - 1 && currentLed != (halfLeds) - 1)
                    {
                        int nextLedBrightnessFactor = ledsFactor % ledMultiplier;
                        if (nextLedBrightnessFactor != 0)
                        {
                            int brightnessValue = (int) map(nextLedBrightnessFactor, 0, ledMultiplier, 0, maxBrightness);

                            int nextLed = currentLed + 1;
                            LED_BUFFER[nextLed] = Color.ChangeBrightness(Layout[nextLed], brightnessValue);

                            int nextOppositeLed = oppositeLed - 1;
                            LED_BUFFER[nextOppositeLed] = Color.ChangeBrightness(Layout[nextOppositeLed], brightnessValue);
                        }
                    }
                }
                else if (currentLed > ledsToShow)
                {
                    LED_BUFFER[currentLed] = C_BLACK;
                    LED_BUFFER[oppositeLed] = C_BLACK;
                }
            }
            return LED_BUFFER;
        }
    }
}

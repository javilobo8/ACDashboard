#include <Adafruit_NeoPixel.h>
#include <math.h>

// Serial
#define BAUDRATE 115200

// NeoPixel Stick
#define NP_PIN 6
#define NUM_LEDS 16
#define NEO_MAX_BRIGHTNESS 32

Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, NP_PIN, NEO_GRB + NEO_KHZ800);

typedef struct
{
	uint32_t led_color[16];
} ColorData;

ColorData color_data;
const size_t packet_size = sizeof(ColorData);
char messageBuffer[packet_size];

void setup() {
	// Setup NeoPixel Strip
	strip.begin();
	strip.setBrightness(NEO_MAX_BRIGHTNESS);
	strip.clear();
	strip.show();
	display();

	for (size_t i = 0; i < 16; i++) {
		strip.setPixelColor(i, 255);
		strip.show();
		delay(10);
		strip.setPixelColor(i, 0);
		strip.show();
		delay(10);
	}

	Serial.begin(BAUDRATE);
}

void loop() {
	if (Serial.available() >= packet_size) {
		Serial.readBytes(messageBuffer, packet_size);
		memcpy(&color_data, &messageBuffer, packet_size);
		display();
	}
}

void display() {
	printLEDStrip();
}

void printLEDStrip() {
	for (size_t i = 0; i < 16; i++) {
		strip.setPixelColor(i, color_data.led_color[i]);
	}
	strip.show();
}



#include <Adafruit_NeoPixel.h>
#include <LedControlMS.h>

// Serial
#define BAUDRATE 115200

// NeoPixel Stick
#define NP_PIN 8
#define NUM_LEDS 16
#define NEO_MAX_BRIGHTNESS 255

// MAX7219
#define MATRIX_DPIN 2
#define MATRIX_CSPIN 3
#define MATRIX_CLKPIN 4
#define MATRIX_INTENSITY 1
#define MATRIX_0 0


// MAX7219
#define DIGIT_DPIN 5
#define DIGIT_CSPIN 6
#define DIGIT_CLKPIN 7
#define DIGIT_INTENSITY 1
#define DIGIT_0 0
#define DIGIT_1 1

Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, NP_PIN, NEO_GRB + NEO_KHZ800);
LedControl matrix_display = LedControl(MATRIX_DPIN, MATRIX_CLKPIN, MATRIX_CSPIN, 1);
LedControl digit_display = LedControl(DIGIT_DPIN, DIGIT_CLKPIN, DIGIT_CSPIN, 2);

typedef struct
{
	uint32_t led_color[16];
	byte matrix_0[8];
	byte digit_0[8];
	byte digit_1[8];
} SerialStruct;

SerialStruct arduino_data;
const size_t packet_size = sizeof(SerialStruct);
char message_buffer[packet_size];

void setup() {
	// Setup NeoPixel Strip
	strip.begin();
	strip.setBrightness(NEO_MAX_BRIGHTNESS);
	strip.clear();
	strip.show();

	for (size_t i = 0; i < 16; i++) {
		strip.setPixelColor(i, 255);
		strip.show();
		delay(10);
		strip.setPixelColor(i, 0);
		strip.show();
		delay(5);
	}

	matrix_display.shutdown(MATRIX_0, false);
	matrix_display.setIntensity(MATRIX_0, MATRIX_INTENSITY);
	matrix_display.clearDisplay(MATRIX_0);

	for (int h = 1; h >= 0; h--) {
		for (size_t j = 0; j < 8; j++) {
			for (size_t k = 0; k < 8; k++) {
				matrix_display.setLed(MATRIX_0, j, k, h);
				delay(3);
			}
		}
	}
	matrix_display.clearDisplay(MATRIX_0);

	digit_display.shutdown(DIGIT_0, false);
	digit_display.setIntensity(DIGIT_0, DIGIT_INTENSITY);
	digit_display.clearDisplay(DIGIT_0);

	for (int h = 1; h >= 0; h--) {
		for (size_t j = 0; j < 8; j++) {
			for (size_t k = 0; k < 8; k++) {
				digit_display.setLed(DIGIT_0, j, k, h);
				delay(3);
			}
		}
	}
	digit_display.clearDisplay(DIGIT_0);

	digit_display.shutdown(DIGIT_1, false);
	digit_display.setIntensity(DIGIT_1, DIGIT_INTENSITY);
	digit_display.clearDisplay(DIGIT_1);

	for (int h = 1; h >= 0; h--) {
		for (size_t j = 0; j < 8; j++) {
			for (size_t k = 0; k < 8; k++) {
				digit_display.setLed(DIGIT_1, j, k, h);
				delay(3);
			}
		}
	}
	digit_display.clearDisplay(DIGIT_1);
	
	Serial.begin(BAUDRATE);
	Serial.println(packet_size);
}

void loop() {
	if (Serial.available() >= packet_size) {
		Serial.readBytes(message_buffer, packet_size);
		memcpy(&arduino_data, &message_buffer, packet_size);

		for (size_t i = 0; i < 16; i++) {
			strip.setPixelColor(i, arduino_data.led_color[i]);
		}
		strip.show();

		for (size_t i = 0; i < 8; i++) {
			matrix_display.setRow(MATRIX_0, i, arduino_data.matrix_0[i]);
			digit_display.setRow(DIGIT_0, i, arduino_data.digit_0[i]);
			digit_display.setRow(DIGIT_1, i, arduino_data.digit_1[i]);
		}
	}
}


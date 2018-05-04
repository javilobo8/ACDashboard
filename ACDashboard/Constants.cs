﻿using System;
using System.Collections.Generic;

namespace ACDashboard
{
    class Constants
    {
        public static byte[] NUMBERS = new byte[10] {
            0b01111110, // 0
	        0b00110000, // 1
	        0b01101101, // 2
	        0b01111001, // 3
	        0b00110011, // 4
	        0b01011011, // 5
	        0b01011111, // 6
	        0b01110000, // 7
	        0b01111111, // 8 
	        0b01111011, // 9
        };

        public static byte[][] Rotate(string[][] strings, int rotation)
        {
            string[][] str = strings;

            for (int r = 0; r < rotation; r++)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    string[] newArr = new string[8];
                    for (int k = 0; k < 8; k++)
                    {
                        string s = "";
                        for (int j = 7; j > -1; j--)
                        {
                            s += str[i][j].Substring(k, 1);
                        }
                        newArr[k] = s;
                    }
                    str[i] = newArr;
                }
            }

            byte[][] data = new byte[str.Length][];

            for (int j = 0; j < str.Length; j++)
            {
                byte[] bytes = new byte[8];
                for(int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(str[j][i], 2);
                }
                data[j] = bytes;
            }

            return data;
        }

        public static string[][] STR_GEARS = new string[][]
        {
            new string[] {
                "11111111",
                "11111111",
                "11111111",
                "00011011",
                "00011011",
                "11111111",
                "11111111",
                "11101110"
            },
            new string[] {
                "11111111",
                "11111111",
                "11111111",
                "00011110",
                "01111000",
                "11111111",
                "11111111",
                "11111111"
            },
            new string[] {
                "00000000",
                "00000100",
                "00000110",
                "11111111",
                "11111111",
                "11111111",
                "00000000",
                "00000000"
            },
            new string[] {
                "11100110",
                "11100111",
                "11110111",
                "11010011",
                "11011011",
                "11001111",
                "11001111",
                "11000110"
            },
            new string[] {
                "11011011",
                "11011011",
                "11011011",
                "11011011",
                "11011011",
                "11111111",
                "11111111",
                "01111110"
            },
            new string[] {
                "00011111",
                "00011111",
                "00011111",
                "00011000",
                "11111111",
                "11111111",
                "11111111",
                "00011000"
            },
            new string[] {
                "01011111",
                "11011111",
                "11011111",
                "11011011",
                "11011011",
                "11111011",
                "11111011",
                "01110011"
            },
            new string[] {
                "01111110",
                "11111111",
                "11111111",
                "11011011",
                "11011011",
                "11111011",
                "11111011",
                "01110010"
            },
            new string[] {
                "00000011",
                "00000011",
                "11100011",
                "11110011",
                "11111011",
                "00011111",
                "00001111",
                "00000111"
            },
            new string[] {
                "01100110",
                "11111111",
                "11111111",
                "11011011",
                "11011011",
                "11111111",
                "11111111",
                "01100110"
            },
            new string[] {
                "01001110",
                "11011111",
                "11011111",
                "11011011",
                "11011011",
                "11111111",
                "11111111",
                "01111110"
            }
        };
    }
}

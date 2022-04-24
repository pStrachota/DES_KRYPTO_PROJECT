using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES_KRYPTO_PROJECT
{
    internal class DES
    {
        private static readonly int[] PC1 = new int[]    {57,49,41,33,25,17,9
                                                ,1,58,50,42,34,26,18,
                                                10,2,59,51,43,35,27,
                                                19,11,3,60,52,44,36,
                                                63,55,47,39,31,23,15,
                                                7,62,54,46,38,30,22,
                                                14,6,61,53,45,37,29,
                                                21,13,5,28,20,12,4};

        private static readonly int[] PC2 = new int[]    {14 ,17 ,11 ,24 ,1 ,5 ,3 ,28,
                                                15 ,6 ,21 ,10 ,23 ,19 ,12 ,4,
                                                26 ,8 ,16 ,7 ,27 ,20 ,13 ,2,
                                                41 ,52 ,31 ,37 ,47 ,55 ,30 ,40,
                                                51 ,45 ,33 ,48 ,44 ,49 ,39 ,56,
                                                34 ,53 ,46 ,42 ,50 ,36 ,29 ,32};

        private static readonly int[] E = new int[]      {32 ,1 ,2 ,3 ,4 ,5,
                                                4 ,5 ,6 ,7 ,8 ,9,
                                                8 ,9 ,10 ,11 ,12 ,13,
                                                12 ,13 ,14 ,15 ,16 ,17,
                                                16 ,17 ,18 ,19 ,20 ,21,
                                                20 ,21 ,22 ,23 ,24 ,25,
                                                24 ,25 ,26 ,27 ,28 ,29,
                                                28 ,29 ,30 ,31 ,32 ,1};
        private static readonly int[] S1 = new int[]     {14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,70,
                                                15,7,4,14,2,13,1,10,6,12,11,9,5,3,8,
                                                4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0,
                                                15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13};

        private static readonly int[] S2 = new int[]    {15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10,
                                                3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5,
                                                0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15,
                                                13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9};

        private static readonly int[] S3 = new int[]    {10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8,
                                                13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1,
                                                13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7,
                                                1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12};


        private static readonly int[] S4 = new int[]    {7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15,
                                                13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9,
                                                10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4,
                                                3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14};

        private static readonly int[] S5 = new int[]    {2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9,
                                                14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6,
                                                4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14,
                                                11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3 };

        private static readonly int[] S6 = new int[]    {12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11,
                                                10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8,
                                                9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6,
                                                4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13 };

        private static readonly int[] S7 = new int[]    {4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1,
                                                13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6,
                                                1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2,
                                                6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12 };

        private static readonly int[] S8 = new int[]    {13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7,
                                                1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2,
                                                7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8,
                                                2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11 };
        private static readonly int[] P = new int[] {16,7,20,21,29,12,28,17,
                                                        1,15,23,26,5,18,31,10,
                                                        2,8,24,14,32,27,3,9,
                                                        19,13,30,6,22,11,4,25};
        public String Encrypt(String plaintext,String key)
        {
            //input
            BitArray textbits = ChopIntoBits(plaintext);
            BitArray keybits = ChopIntoBits(key);
            BitArray keybitsC = new(28);
            BitArray keybitsD = new(28);
            BitArray holderL = new(28);
            BitArray holderR = new(28);
            bool[] buff = new bool[2];
            //expanding bitarrays to fit (56 bits for key,64 bit blocks for plaintext
            if (textbits.Length%64!=0) textbits.Length+=64 - (textbits.Length % 64);
            int blockcount = textbits.Length/64;
            if (keybits.Length !=64)
            {
                keybits.Length = 64;
            }

            keybits = UseTable(keybits, PC1,56);
            for (int i = 0; i < 28; i++)
            {
                keybitsC[i] = keybits[i];
                keybitsD[i] = keybits[i + 28];
            }

                for (int j = 1; j <= 16; j++)
                {
                    //generate subkey
                    if (j == 1 || j == 9 || j == 16)
                    {
                        buff[0] = keybitsC[27];
                        keybitsC.LeftShift(1);
                        keybitsC[27] = buff[0];

                        buff[1] = keybitsD[27];
                        keybitsD.LeftShift(1);
                        keybitsD[27] = buff[1];

                        keybits = UseTable(glueKey(keybitsC, keybitsD), PC2, 48);
                    } else {
                        buff[0] = keybitsC[26];
                        buff[1] = keybitsC[27];
                        keybitsC.LeftShift(2);
                        keybitsC[26] = buff[0];
                        keybitsC[26] = buff[1];

                        buff[0] = keybitsD[26];
                        buff[1] = keybitsD[27];
                        keybitsD.LeftShift(2);
                        keybitsD[26] = buff[0];
                        keybitsD[26] = buff[1];

                        keybits = UseTable(glueKey(keybitsC, keybitsD), PC2, 48);
                    }
                    //podział bloku na połowy
                    for (int i = 0; i < blockcount; i++)
                    {
                        for (int h = 0; h < 28; h++)
                        {
                            holderL[h] = textbits[h + i * 64];
                            holderR[h] = textbits[h + i * 64 + 28];
                        }
                    holderR = UseTable(holderR, E, 48);
                    holderR.Xor(keybits);
                    holderR = Sbox(holderR);
                    holderR.Xor(holderL);
                    holderR = UseTable(holderR, P, 32);
                        for (int h = 0; h < 28; h++)
                        {
                            textbits[h + i * 64] = holderR[h];
                            textbits[h + i * 64 + 28] = holderL[h];
                        }
                    }
                }
            return "";
        }
        public String Decrypt(String plaintext, String key)
        {
            return "";
        }
        private static BitArray ChopIntoBits(String input)
        {
            char[] chars = input.ToCharArray();
            byte[] bytes = new byte[chars.Length];
            for(int i = 0; i < chars.Length; i++)
            {
                bytes[i] = (byte)chars[i];
            }
            BitArray bits = new(bytes);
            return bits;
        }

        private static BitArray UseTable(BitArray arr,int[] table,int size)
        {
            BitArray output = new(size);
            for(int i = 0; i < table.Length; i++)
            {
                output[i] = arr[table[i]];
            }
            return output;
        }
        private BitArray glueKey(BitArray arrC,BitArray arrD)
        {
            BitArray result = new(56);
            for(int i = 0; i < 28; i++)
            {
                result[i] = arrC[i];
                result[i+28] = arrD[i];
            }
            return result;
        }
        private BitArray Sbox(BitArray input)
        {
            BitArray result = new(32);
            BitArray rowbits = new(2);
            BitArray colbits = new(4);
            //8 wierszy po 6 bitów
            for (int i = 0; i < 8; i++)
            {
                rowbits[0] = input[0 + i * 6];//pierwszy
                rowbits[1] = input[5 + i * 6];//ostatni

                for(int k = 1; k < 5; k++)
                {
                    colbits[k] = input[k + i * 6]; //środkowe
                }
                BitArray buff = new BitArray(BitConverter.GetBytes(Sboxpicker(GetIntFromBitArray(colbits), GetIntFromBitArray(rowbits), i)));
                for (int j = 0; j < 4; j++)
                {
                    result[i * 8 + j] = buff[j];
                }
            }
            return result;
        }
        private static int Sboxpicker(int col,int row,int index)
        {
            int result = 0;
            switch (index)
            {
                case 1:
                    result = S1[col + row*16];
                    break;
                case 2:
                    result = S2[col + row * 16];
                    break;
                case 3:
                    result = S3[col + row * 16];
                    break;
                case 4:
                    result = S4[col + row * 16];
                    break;
                case 5:
                    result = S5[col + row * 16];
                    break;
                case 6:
                    result = S6[col + row * 16];
                    break;
                case 7:
                    result = S7[col + row * 16];
                    break;
                case 8:
                    result = S8[col + row * 16];
                    break;
            }
            return result;
        }

        private static int GetIntFromBitArray(BitArray bitArray)
        {

            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");
            byte[] ret = new byte[(bitArray.Length - 1) / 8 + 1];
            bitArray.CopyTo(ret, 0);
            return BitConverter.ToInt32(ret, 0);

        }
        public static String Test()
        {
            byte[] input = BitConverter.GetBytes(7);
            BitArray converted = new BitArray(input);
            int a = GetIntFromBitArray((BitArray)converted);
            
            return a.ToString();
        }
    }
}

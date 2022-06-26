using System;
using System.Text;

namespace DATA_ENCRYPTION_STANDARD;

public class DES
{
    String s_key;
    byte[] b_key;
    byte[][] subKeys = new byte[16][];
    byte[] shift = {1, 3, 5, 7, 0, 2, 4, 6};


    public DES(string sKey)
    {
        setKeyHex(sKey);
    }

    byte[] pBlock =
    {
        16, 7, 20, 21, 29, 12, 28, 17, 1, 15,
        23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9,
        19, 13, 30, 6, 22, 11, 4, 25
    };

    byte[] sBox =
    {
        14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, // S1
        0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
        4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
        15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13,
        15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, // S2
        3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
        0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
        13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9,
        10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, // S3
        13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
        13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
        1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12,
        7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, // S4
        13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
        10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
        3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14,
        2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, // S5
        14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
        4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
        11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3,
        12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, // S6
        10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
        9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
        4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13,
        4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, // S7
        13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
        1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
        6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12,
        13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, // S8
        1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
        7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
        2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
    };

    private static readonly byte[] Ip =
    {
        58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7
    };

    private static readonly byte[] Ep =
    {
        40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25
    };

    public void setKeyString(String key)
    {
        this.b_key = Encoding.ASCII.GetBytes(key);
        if (testKey())
        {
            this.s_key = key;
            subKeys = getSubkeys();
        }
    }

    public void setKeyHex(String key)
    {
        this.b_key = Auxx.StringToByteArray(key);
        if (testKey())
        {
            this.s_key = key;
            subKeys = getSubkeys();
        }
    }

    public bool testKey()
    {
        if (this.b_key == null)
        {
            return false;
        }
        else
        {
            int l = this.b_key.Length;
            if (l < 8)
            {
                this.b_key = null;
                return false;
            }
            else if (l > 8)
            {
                this.b_key = null;
                return false;
            }

            return true;
        }
    }

    public byte[] encodeBlock(byte[] data, int beginIndex)
    {
        byte[] msg = new byte[8];
        Array.Copy(data, beginIndex, msg, 0, 8);
        return encrypt(msg);
    }

    public byte[] decodeBlock(byte[] data, int beginIndex)
    {
        byte[] msg = new byte[8];
        Array.Copy(data, beginIndex, msg, 0, 8);
        return decrypt(msg);
    }

    private byte[] encrypt(byte[] theMsg)
    {
        if (theMsg.Length != 8)
        {
            throw new Exception("Część wiadomości nie ma 8 bajtów długości");
        }

        theMsg = Auxx.selectBits(theMsg, Ip);

        byte[] r = new byte[4];
        byte[] l = new byte[4];

        for (int i = 0; i < theMsg.Length / 2; i++)
        {
            r[i] = theMsg[i];
            l[i] = theMsg[i + 4];
        }

        for (int k = 0; k < 16; k++)
        {
            byte[] rBackup = r;
            r = computeExtendedBlock(r);
            r = Auxx.XORBytes(r, subKeys[k]);
            r = sBlocks(r);
            r = Auxx.selectBits(r, pBlock);
            r = Auxx.XORBytes(l, r);
            l = rBackup;
        }

        byte[] rl = new byte[8];

        for (int i = 0; i < rl.Length / 2; i++)
        {
            rl[i] = l[i];
            rl[i + 4] = r[i];
        }

        return Auxx.selectBits(rl, Ep);
    }

    private byte[] decrypt(byte[] theMsg)
    {
        if (theMsg.Length != 8)
        {
            throw new Exception("Część wiadomości nie ma 8 bajtów długości");
        }

        theMsg = Auxx.selectBits(theMsg, Ip);

        byte[] r = new byte[4];
        byte[] l = new byte[4];

        for (int i = 0; i < theMsg.Length / 2; i++)
        {
            r[i] = theMsg[i];
            l[i] = theMsg[i + 4];
        }

        int numOfSubKeys = subKeys.Length;
        for (int k = 0; k < numOfSubKeys; k++)
        {
            byte[] rBackup = r;
            r = computeExtendedBlock(r);
            r = Auxx.XORBytes(r, subKeys[numOfSubKeys - k - 1]);
            r = sBlocks(r);

            r = Auxx.selectBits(r, pBlock);
            r = Auxx.XORBytes(l, r);
            l = rBackup;
        }

        byte[] rl = new byte[8];

        for (int i = 0; i < rl.Length / 2; i++)
        {
            rl[i] = l[i];
            rl[i + 4] = r[i];
        }

        return Auxx.selectBits(rl, Ep);
    }

    public byte[][] getSubkeys()
    {
        byte[] PC1 =
        {
            57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
        };
        byte[] PC2 =
        {
            14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47,
            55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };

        byte[] SHIFTS = {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};
        byte[] activeKey = null;
        activeKey = Auxx.selectBits(this.b_key, PC1);

        int halfKeySize = 28;
        byte[] c = Auxx.selectBits(activeKey, 0, halfKeySize);
        byte[] d = Auxx.selectBits(activeKey, halfKeySize, halfKeySize);
        byte[][] subKeysLocal = new byte[16][];
        for (int k = 0; k < 16; k++)
        {
            c = Auxx.rotateLeft(c, halfKeySize, SHIFTS[k]);
            d = Auxx.rotateLeft(d, halfKeySize, SHIFTS[k]);
            byte[] cd = joinBlocks(c, halfKeySize, d, halfKeySize);
            subKeysLocal[k] = Auxx.selectBits(cd, PC2);
        }

        return subKeysLocal;
    }

    private byte[] joinBlocks(byte[] a, int aLen, byte[] b, int bLen)
    {
        int numOfBytes = (aLen + bLen - 1) / 8 + 1;
        byte[] outer = new byte[numOfBytes];
        int j = 0;
        for (int i = 0; i < aLen; i++)
        {
            int val = Auxx.getBitAt(a, i);
            Auxx.setBitAt(outer, j, val);
            j++;
        }

        for (int i = 0; i < bLen; i++)
        {
            int val = Auxx.getBitAt(b, i);
            Auxx.setBitAt(outer, j, val);
            j++;
        }

        return outer;
    }

    private byte[] computeExtendedBlock(byte[] block)
    {
        byte[] extendedBlock = new byte[6];
        short current;
        byte pBit = 31;
        byte changer = 0;
        for (int bit = 0; bit < 48; bit++)
        {
            current = (short) (block[pBit / 8] >> (7 - (pBit % 8)));
            current = (short) (current & 1);
            current = (short) (current << (7 - (bit % 8)));
            extendedBlock[bit / 8] = (byte) (extendedBlock[bit / 8] | (current));
            if (++changer == 6)
            {
                changer = 0;
                pBit--;
            }
            else
                pBit = (byte) ((++pBit) % 32);
        }

        return extendedBlock;
    }

    private byte[] sBlocks(byte[] data)
    {
        byte row;
        byte col;
        data = create6BitData(data);

        byte[] result = new byte[data.Length / 2];
        byte lowerHalfByte = 0;
        byte halfByte;
        for (int b = 0; b < data.Length; b++)
        {
            row = (byte) (((data[b] >> 6) & 2) | ((data[b] >> 2) & 1));
            col = (byte) ((data[b] >> 3) & 15);
            halfByte = sBox[64 * b + 16 * row + col];
            if (b % 2 == 0)
                lowerHalfByte = halfByte;
            else
                result[b / 2] = (byte) (16 * lowerHalfByte + halfByte);
        }

        return result;
    }


    private byte[] create6BitData(byte[] data)
    {
        int numOfBytes = (8 * data.Length - 1) / 6 + 1;
        byte[] outer = new byte[numOfBytes];
        for (int i = 0; i < numOfBytes; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int val = Auxx.getBitAt(data, 6 * i + j);
                Auxx.setBitAt(outer, 8 * i + j, val);
            }
        }

        return outer;
    }


    public byte[] encode(byte[] message)
    {
        int len;
        if ((message.Length / 2 % 4) != 0)
            len = (message.Length / 8 + 1) * 8;
        else
            len = message.Length;
        byte[] result = new byte[len];
        byte[] tempBlock = new byte[8];
        byte[] rawData = null;

        rawData = message;
        for (int i = 0; i < (rawData.Length / 8); i++)
        {
            tempBlock = encodeBlock(rawData, i * 8);
            Array.Copy(tempBlock, 0, result, i * 8, 8);
        }

        if (message.Length / 2 % 4 != 0)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i + (rawData.Length / 8) * 8 < rawData.Length)
                    tempBlock[i] = rawData[i + (rawData.Length / 8) * 8];
                else
                    tempBlock[i] = 0;
            }

            tempBlock = encodeBlock(tempBlock, 0);
            Array.Copy(tempBlock, 0, result, (rawData.Length / 8) * 8, 8);
        }

        return result;
    }


    public byte[] decode(byte[] encrypted)
    {
        byte[] tmpResult = new byte[encrypted.Length];
        byte[] tempBlock = new byte[8];
        byte[] rawData = null;

        rawData = encrypted;
        for (int i = 0; i < (rawData.Length / 8); i++)
        {
            tempBlock = decodeBlock(rawData, i * 8);
            Array.Copy(tempBlock, 0, tmpResult, i * 8, tempBlock.Length);
        }

        int cnt = 0;
        for (int i = 1; i < 9; i += 2)
        {
            if (tmpResult[tmpResult.Length - i] == 0 && tmpResult[tmpResult.Length - i - 1] == 0)
                cnt += 2;
            else
                break;
        }

        byte[] result = new byte[tmpResult.Length - cnt];
        Array.Copy(tmpResult, 0, result, 0, tmpResult.Length - cnt);
        return result;
    }
}
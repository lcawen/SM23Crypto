using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SM23Crypto
{
    public class BigInteger
    {
        internal static readonly int[][] primeLists = new int[64][]
        {
      new int[8]{ 3, 5, 7, 11, 13, 17, 19, 23 },
      new int[5]{ 29, 31, 37, 41, 43 },
      new int[5]{ 47, 53, 59, 61, 67 },
      new int[4]{ 71, 73, 79, 83 },
      new int[4]{ 89, 97, 101, 103 },
      new int[4]{ 107, 109, 113, (int) sbyte.MaxValue },
      new int[4]{ 131, 137, 139, 149 },
      new int[4]{ 151, 157, 163, 167 },
      new int[4]{ 173, 179, 181, 191 },
      new int[4]{ 193, 197, 199, 211 },
      new int[3]{ 223, 227, 229 },
      new int[3]{ 233, 239, 241 },
      new int[3]{ 251, 257, 263 },
      new int[3]{ 269, 271, 277 },
      new int[3]{ 281, 283, 293 },
      new int[3]{ 307, 311, 313 },
      new int[3]{ 317, 331, 337 },
      new int[3]{ 347, 349, 353 },
      new int[3]{ 359, 367, 373 },
      new int[3]{ 379, 383, 389 },
      new int[3]{ 397, 401, 409 },
      new int[3]{ 419, 421, 431 },
      new int[3]{ 433, 439, 443 },
      new int[3]{ 449, 457, 461 },
      new int[3]{ 463, 467, 479 },
      new int[3]{ 487, 491, 499 },
      new int[3]{ 503, 509, 521 },
      new int[3]{ 523, 541, 547 },
      new int[3]{ 557, 563, 569 },
      new int[3]{ 571, 577, 587 },
      new int[3]{ 593, 599, 601 },
      new int[3]{ 607, 613, 617 },
      new int[3]{ 619, 631, 641 },
      new int[3]{ 643, 647, 653 },
      new int[3]{ 659, 661, 673 },
      new int[3]{ 677, 683, 691 },
      new int[3]{ 701, 709, 719 },
      new int[3]{ 727, 733, 739 },
      new int[3]{ 743, 751, 757 },
      new int[3]{ 761, 769, 773 },
      new int[3]{ 787, 797, 809 },
      new int[3]{ 811, 821, 823 },
      new int[3]{ 827, 829, 839 },
      new int[3]{ 853, 857, 859 },
      new int[3]{ 863, 877, 881 },
      new int[3]{ 883, 887, 907 },
      new int[3]{ 911, 919, 929 },
      new int[3]{ 937, 941, 947 },
      new int[3]{ 953, 967, 971 },
      new int[3]{ 977, 983, 991 },
      new int[3]{ 997, 1009, 1013 },
      new int[3]{ 1019, 1021, 1031 },
      new int[3]{ 1033, 1039, 1049 },
      new int[3]{ 1051, 1061, 1063 },
      new int[3]{ 1069, 1087, 1091 },
      new int[3]{ 1093, 1097, 1103 },
      new int[3]{ 1109, 1117, 1123 },
      new int[3]{ 1129, 1151, 1153 },
      new int[3]{ 1163, 1171, 1181 },
      new int[3]{ 1187, 1193, 1201 },
      new int[3]{ 1213, 1217, 1223 },
      new int[3]{ 1229, 1231, 1237 },
      new int[3]{ 1249, 1259, 1277 },
      new int[3]{ 1279, 1283, 1289 }
        };
        internal static readonly int[] primeProducts;
        private const long IMASK = 4294967295;
        private const ulong UIMASK = 4294967295;
        private static readonly int[] ZeroMagnitude = new int[0];
        private static readonly byte[] ZeroEncoding = new byte[0];
        private static readonly BigInteger[] SMALL_CONSTANTS = new BigInteger[17];
        public static readonly BigInteger Zero;
        public static readonly BigInteger One;
        public static readonly BigInteger Two;
        public static readonly BigInteger Three;
        public static readonly BigInteger Four;
        public static readonly BigInteger Ten;
        private static readonly byte[] BitLengthTable = new byte[256]
        {
      (byte) 0,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 6,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 7,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8
        };
        private const int chunk2 = 1;
        private const int chunk8 = 1;
        private const int chunk10 = 19;
        private const int chunk16 = 16;
        private static readonly BigInteger radix2;
        private static readonly BigInteger radix2E;
        private static readonly BigInteger radix8;
        private static readonly BigInteger radix8E;
        private static readonly BigInteger radix10;
        private static readonly BigInteger radix10E;
        private static readonly BigInteger radix16;
        private static readonly BigInteger radix16E;
        // private static readonly SecureRandom RandomSource = new SecureRandom();
        private static readonly int[] ExpWindowThresholds = new int[8]
        {
      7,
      25,
      81,
      241,
      673,
      1793,
      4609,
      int.MaxValue
        };
        private const int BitsPerByte = 8;
        private const int BitsPerInt = 32;
        private const int BytesPerInt = 4;
        private int[] magnitude;
        private int sign;
        private int nBits = -1;
        private int nBitLength = -1;
        private int mQuote;

        static BigInteger()
        {
            BigInteger.Zero = new BigInteger(0, BigInteger.ZeroMagnitude, false);
            BigInteger.Zero.nBits = 0;
            BigInteger.Zero.nBitLength = 0;
            BigInteger.SMALL_CONSTANTS[0] = BigInteger.Zero;
            for (uint index = 1; (long)index < (long)BigInteger.SMALL_CONSTANTS.Length; ++index)
                BigInteger.SMALL_CONSTANTS[(int)index] = BigInteger.CreateUValueOf((ulong)index);
            BigInteger.One = BigInteger.SMALL_CONSTANTS[1];
            BigInteger.Two = BigInteger.SMALL_CONSTANTS[2];
            BigInteger.Three = BigInteger.SMALL_CONSTANTS[3];
            BigInteger.Four = BigInteger.SMALL_CONSTANTS[4];
            BigInteger.Ten = BigInteger.SMALL_CONSTANTS[10];
            BigInteger.radix2 = BigInteger.ValueOf(2L);
            BigInteger.radix2E = BigInteger.radix2.Pow(1);
            BigInteger.radix8 = BigInteger.ValueOf(8L);
            BigInteger.radix8E = BigInteger.radix8.Pow(1);
            BigInteger.radix10 = BigInteger.ValueOf(10L);
            BigInteger.radix10E = BigInteger.radix10.Pow(19);
            BigInteger.radix16 = BigInteger.ValueOf(16L);
            BigInteger.radix16E = BigInteger.radix16.Pow(16);
            BigInteger.primeProducts = new int[BigInteger.primeLists.Length];
            for (int index1 = 0; index1 < BigInteger.primeLists.Length; ++index1)
            {
                int[] primeList = BigInteger.primeLists[index1];
                int num = primeList[0];
                for (int index2 = 1; index2 < primeList.Length; ++index2)
                    num *= primeList[index2];
                BigInteger.primeProducts[index1] = num;
            }
        }

        private static int GetByteLength(int nBits) => (nBits + 8 - 1) / 8;

        // public static BigInteger Arbitrary(int sizeInBits) => new BigInteger(sizeInBits, (Random) BigInteger.RandomSource);

        private BigInteger(int signum, int[] mag, bool checkMag)
        {
            if (checkMag)
            {
                int sourceIndex = 0;
                while (sourceIndex < mag.Length && mag[sourceIndex] == 0)
                    ++sourceIndex;
                if (sourceIndex == mag.Length)
                {
                    this.sign = 0;
                    this.magnitude = BigInteger.ZeroMagnitude;
                }
                else
                {
                    this.sign = signum;
                    if (sourceIndex == 0)
                    {
                        this.magnitude = mag;
                    }
                    else
                    {
                        this.magnitude = new int[mag.Length - sourceIndex];
                        Array.Copy((Array)mag, sourceIndex, (Array)this.magnitude, 0, this.magnitude.Length);
                    }
                }
            }
            else
            {
                this.sign = signum;
                this.magnitude = mag;
            }
        }

        public BigInteger(string value)
          : this(value, 10)
        {
        }

        public BigInteger(string str, int radix)
        {
            if (str.Length == 0)
                throw new FormatException("Zero length BigInteger");
            NumberStyles style;
            int length;
            BigInteger bigInteger1;
            BigInteger val;
            switch (radix)
            {
                case 2:
                    style = NumberStyles.Integer;
                    length = 1;
                    bigInteger1 = BigInteger.radix2;
                    val = BigInteger.radix2E;
                    break;
                case 8:
                    style = NumberStyles.Integer;
                    length = 1;
                    bigInteger1 = BigInteger.radix8;
                    val = BigInteger.radix8E;
                    break;
                case 10:
                    style = NumberStyles.Integer;
                    length = 19;
                    bigInteger1 = BigInteger.radix10;
                    val = BigInteger.radix10E;
                    break;
                case 16:
                    style = NumberStyles.AllowHexSpecifier;
                    length = 16;
                    bigInteger1 = BigInteger.radix16;
                    val = BigInteger.radix16E;
                    break;
                default:
                    throw new FormatException("Only bases 2, 8, 10, or 16 allowed");
            }
            int num1 = 0;
            this.sign = 1;
            if (str[0] == '-')
            {
                if (str.Length == 1)
                    throw new FormatException("Zero length BigInteger");
                this.sign = -1;
                num1 = 1;
            }
            while (num1 < str.Length && int.Parse(str[num1].ToString(), style) == 0)
                ++num1;
            if (num1 >= str.Length)
            {
                this.sign = 0;
                this.magnitude = BigInteger.ZeroMagnitude;
            }
            else
            {
                BigInteger bigInteger2 = BigInteger.Zero;
                int num2 = num1 + length;
                if (num2 <= str.Length)
                {
                    do
                    {
                        string s = str.Substring(num1, length);
                        ulong num3 = ulong.Parse(s, style);
                        BigInteger uvalueOf = BigInteger.CreateUValueOf(num3);
                        BigInteger bigInteger3;
                        switch (radix)
                        {
                            case 2:
                                if (num3 >= 2UL)
                                    throw new FormatException("Bad character in radix 2 string: " + s);
                                bigInteger3 = bigInteger2.ShiftLeft(1);
                                break;
                            case 8:
                                if (num3 >= 8UL)
                                    throw new FormatException("Bad character in radix 8 string: " + s);
                                bigInteger3 = bigInteger2.ShiftLeft(3);
                                break;
                            case 16:
                                bigInteger3 = bigInteger2.ShiftLeft(64);
                                break;
                            default:
                                bigInteger3 = bigInteger2.Multiply(val);
                                break;
                        }
                        bigInteger2 = bigInteger3.Add(uvalueOf);
                        num1 = num2;
                        num2 += length;
                    }
                    while (num2 <= str.Length);
                }
                if (num1 < str.Length)
                {
                    string s = str.Substring(num1);
                    BigInteger uvalueOf = BigInteger.CreateUValueOf(ulong.Parse(s, style));
                    if (bigInteger2.sign > 0)
                    {
                        switch (radix)
                        {
                            case 2:
                            case 8:
                                bigInteger2 = bigInteger2.Add(uvalueOf);
                                break;
                            case 16:
                                bigInteger2 = bigInteger2.ShiftLeft(s.Length << 2);
                                goto case 2;
                            default:
                                bigInteger2 = bigInteger2.Multiply(bigInteger1.Pow(s.Length));
                                goto case 2;
                        }
                    }
                    else
                        bigInteger2 = uvalueOf;
                }
                this.magnitude = bigInteger2.magnitude;
            }
        }

        public BigInteger(byte[] bytes)
          : this(bytes, 0, bytes.Length)
        {
        }

        public BigInteger(byte[] bytes, int offset, int length)
        {
            if (length == 0)
                throw new FormatException("Zero length BigInteger");
            if ((sbyte)bytes[offset] < (sbyte)0)
            {
                this.sign = -1;
                int num = offset + length;
                int index1 = offset;
                while (index1 < num && bytes[index1] == byte.MaxValue)
                    ++index1;
                if (index1 >= num)
                {
                    this.magnitude = BigInteger.One.magnitude;
                }
                else
                {
                    int length1 = num - index1;
                    byte[] bytes1 = new byte[length1];
                    int index2 = 0;
                    while (index2 < length1)
                        bytes1[index2++] = (byte)(~bytes[index1++]);
                    while (bytes1[--index2] == byte.MaxValue)
                        bytes1[index2] = (byte)0;
                    ++bytes1[index2];
                    this.magnitude = BigInteger.MakeMagnitude(bytes1, 0, bytes1.Length);
                }
            }
            else
            {
                this.magnitude = BigInteger.MakeMagnitude(bytes, offset, length);
                this.sign = this.magnitude.Length != 0 ? 1 : 0;
            }
        }

        private static int[] MakeMagnitude(byte[] bytes, int offset, int length)
        {
            int num1 = offset + length;
            int index1 = offset;
            while (index1 < num1 && bytes[index1] == (byte)0)
                ++index1;
            if (index1 >= num1)
                return BigInteger.ZeroMagnitude;
            int length1 = (num1 - index1 + 3) / 4;
            int num2 = (num1 - index1) % 4;
            if (num2 == 0)
                num2 = 4;
            if (length1 < 1)
                return BigInteger.ZeroMagnitude;
            int[] numArray = new int[length1];
            int num3 = 0;
            int index2 = 0;
            for (int index3 = index1; index3 < num1; ++index3)
            {
                num3 = num3 << 8 | (int)bytes[index3] & (int)byte.MaxValue;
                --num2;
                if (num2 <= 0)
                {
                    numArray[index2] = num3;
                    ++index2;
                    num2 = 4;
                    num3 = 0;
                }
            }
            if (index2 < numArray.Length)
                numArray[index2] = num3;
            return numArray;
        }

        public BigInteger(int sign, byte[] bytes)
          : this(sign, bytes, 0, bytes.Length)
        {
        }

        public BigInteger(int sign, byte[] bytes, int offset, int length)
        {
            if (sign < -1 || sign > 1)
                throw new FormatException("Invalid sign value");
            if (sign == 0)
            {
                this.sign = 0;
                this.magnitude = BigInteger.ZeroMagnitude;
            }
            else
            {
                this.magnitude = BigInteger.MakeMagnitude(bytes, offset, length);
                this.sign = this.magnitude.Length < 1 ? 0 : sign;
            }
        }



        public BigInteger Abs() => this.sign < 0 ? this.Negate() : this;

        private static int[] AddMagnitudes(int[] a, int[] b)
        {
            int index = a.Length - 1;
            int num1 = b.Length - 1;
            long num2 = 0;
            while (num1 >= 0)
            {
                long num3 = num2 + ((long)(uint)a[index] + (long)(uint)b[num1--]);
                a[index--] = (int)num3;
                num2 = (long)((ulong)num3 >> 32);
            }
            if (num2 != 0L)
            {
                while (index >= 0 && ++a[index--] == 0)
                    ;
            }
            return a;
        }

        public BigInteger Add(BigInteger value)
        {
            if (this.sign == 0)
                return value;
            if (this.sign == value.sign)
                return this.AddToMagnitude(value.magnitude);
            if (value.sign == 0)
                return this;
            return value.sign < 0 ? this.Subtract(value.Negate()) : value.Subtract(this.Negate());
        }

        private BigInteger AddToMagnitude(int[] magToAdd)
        {
            int[] numArray;
            int[] b;
            if (this.magnitude.Length < magToAdd.Length)
            {
                numArray = magToAdd;
                b = this.magnitude;
            }
            else
            {
                numArray = this.magnitude;
                b = magToAdd;
            }
            uint maxValue = uint.MaxValue;
            if (numArray.Length == b.Length)
                maxValue -= (uint)b[0];
            bool checkMag = (uint)numArray[0] >= maxValue;
            int[] a;
            if (checkMag)
            {
                a = new int[numArray.Length + 1];
                numArray.CopyTo((Array)a, 1);
            }
            else
                a = (int[])numArray.Clone();
            return new BigInteger(this.sign, BigInteger.AddMagnitudes(a, b), checkMag);
        }

        public BigInteger And(BigInteger value)
        {
            if (this.sign == 0 || value.sign == 0)
                return BigInteger.Zero;
            int[] numArray1 = this.sign > 0 ? this.magnitude : this.Add(BigInteger.One).magnitude;
            int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add(BigInteger.One).magnitude;
            bool flag = this.sign < 0 && value.sign < 0;
            int[] mag = new int[System.Math.Max(numArray1.Length, numArray2.Length)];
            int num1 = mag.Length - numArray1.Length;
            int num2 = mag.Length - numArray2.Length;
            for (int index = 0; index < mag.Length; ++index)
            {
                int num3 = index >= num1 ? numArray1[index - num1] : 0;
                int num4 = index >= num2 ? numArray2[index - num2] : 0;
                if (this.sign < 0)
                    num3 = ~num3;
                if (value.sign < 0)
                    num4 = ~num4;
                mag[index] = num3 & num4;
                if (flag)
                    mag[index] = ~mag[index];
            }
            BigInteger bigInteger = new BigInteger(1, mag, true);
            if (flag)
                bigInteger = bigInteger.Not();
            return bigInteger;
        }


        public BigInteger(int sizeInBits, Random random)
        {
            if (sizeInBits < 0)
                throw new ArgumentException("sizeInBits must be non-negative");
            this.nBits = -1;
            this.nBitLength = -1;
            if (sizeInBits == 0)
            {
                this.sign = 0;
                this.magnitude = BigInteger.ZeroMagnitude;
            }
            else
            {
                int byteLength = BigInteger.GetByteLength(sizeInBits);
                byte[] numArray = new byte[byteLength];
                random.NextBytes(numArray);
                int num = 8 * byteLength - sizeInBits;
                numArray[0] &= (byte)((uint)byte.MaxValue >> num);
                this.magnitude = BigInteger.MakeMagnitude(numArray, 0, numArray.Length);
                this.sign = this.magnitude.Length < 1 ? 0 : 1;
            }
        }

        public static int BitCnt(int i)
        {
            uint num1 = (uint)i;
            uint num2 = num1 - (num1 >> 1 & 1431655765U);
            uint num3 = (uint)(((int)num2 & 858993459) + ((int)(num2 >> 2) & 858993459));
            uint num4 = (uint)((int)num3 + (int)(num3 >> 4) & 252645135);
            uint num5 = num4 + (num4 >> 8);
            return (int)(num5 + (num5 >> 16) & 63U);
        }

        private static int CalcBitLength(int sign, int indx, int[] mag)
        {
            for (; indx < mag.Length; ++indx)
            {
                if (mag[indx] != 0)
                {
                    int num1 = 32 * (mag.Length - indx - 1);
                    int w = mag[indx];
                    int num2 = num1 + BigInteger.BitLen(w);
                    if (sign < 0 && (w & -w) == w)
                    {
                        while (++indx < mag.Length)
                        {
                            if (mag[indx] != 0)
                                goto label_8;
                        }
                        --num2;
                    }
                label_8:
                    return num2;
                }
            }
            return 0;
        }

        public int BitLength
        {
            get
            {
                if (this.nBitLength == -1)
                    this.nBitLength = this.sign == 0 ? 0 : BigInteger.CalcBitLength(this.sign, 0, this.magnitude);
                return this.nBitLength;
            }
        }

        internal static int BitLen(int w)
        {
            uint index1 = (uint)w;
            uint index2 = index1 >> 24;
            if (index2 != 0U)
                return 24 + (int)BigInteger.BitLengthTable[(int)index2];
            uint index3 = index1 >> 16;
            if (index3 != 0U)
                return 16 + (int)BigInteger.BitLengthTable[(int)index3];
            uint index4 = index1 >> 8;
            return index4 != 0U ? 8 + (int)BigInteger.BitLengthTable[(int)index4] : (int)BigInteger.BitLengthTable[(int)index1];
        }

        private bool QuickPow2Check() => this.sign > 0 && this.nBits == 1;

        public int CompareTo(object obj) => this.CompareTo((BigInteger)obj);

        private static int CompareTo(int xIndx, int[] x, int yIndx, int[] y)
        {
            while (xIndx != x.Length && x[xIndx] == 0)
                ++xIndx;
            while (yIndx != y.Length && y[yIndx] == 0)
                ++yIndx;
            return BigInteger.CompareNoLeadingZeroes(xIndx, x, yIndx, y);
        }

        private static int CompareNoLeadingZeroes(int xIndx, int[] x, int yIndx, int[] y)
        {
            int num1 = x.Length - y.Length - (xIndx - yIndx);
            if (num1 != 0)
                return num1 >= 0 ? 1 : -1;
            while (xIndx < x.Length)
            {
                uint num2 = (uint)x[xIndx++];
                uint num3 = (uint)y[yIndx++];
                if ((int)num2 != (int)num3)
                    return num2 >= num3 ? 1 : -1;
            }
            return 0;
        }

        public int CompareTo(BigInteger value)
        {
            if (this.sign < value.sign)
                return -1;
            if (this.sign > value.sign)
                return 1;
            return this.sign != 0 ? this.sign * BigInteger.CompareNoLeadingZeroes(0, this.magnitude, 0, value.magnitude) : 0;
        }

        private int[] Divide(int[] x, int[] y)
        {
            int index1 = 0;
            while (index1 < x.Length && x[index1] == 0)
                ++index1;
            int index2 = 0;
            while (index2 < y.Length && y[index2] == 0)
                ++index2;
            int num1 = BigInteger.CompareNoLeadingZeroes(index1, x, index2, y);
            int[] a;
            if (num1 > 0)
            {
                int num2 = BigInteger.CalcBitLength(1, index2, y);
                int num3 = BigInteger.CalcBitLength(1, index1, x);
                int n1 = num3 - num2;
                int start = 0;
                int index3 = 0;
                int num4 = num2;
                int[] numArray1;
                int[] numArray2;
                if (n1 > 0)
                {
                    numArray1 = new int[(n1 >> 5) + 1];
                    numArray1[0] = 1 << n1 % 32;
                    numArray2 = BigInteger.ShiftLeft(y, n1);
                    num4 += n1;
                }
                else
                {
                    numArray1 = new int[1] { 1 };
                    int length = y.Length - index2;
                    numArray2 = new int[length];
                    Array.Copy((Array)y, index2, (Array)numArray2, 0, length);
                }
                a = new int[numArray1.Length];
            label_11:
                if (num4 < num3 || BigInteger.CompareNoLeadingZeroes(index1, x, index3, numArray2) >= 0)
                {
                    BigInteger.Subtract(index1, x, index3, numArray2);
                    BigInteger.AddMagnitudes(a, numArray1);
                    while (x[index1] == 0)
                    {
                        if (++index1 == x.Length)
                            return a;
                    }
                    num3 = 32 * (x.Length - index1 - 1) + BigInteger.BitLen(x[index1]);
                    if (num3 <= num2)
                    {
                        if (num3 < num2)
                            return a;
                        num1 = BigInteger.CompareNoLeadingZeroes(index1, x, index2, y);
                        if (num1 <= 0)
                            goto label_30;
                    }
                }
                int n2 = num4 - num3;
                if (n2 == 1 && (uint)numArray2[index3] >> 1 > (uint)x[index1])
                    ++n2;
                if (n2 < 2)
                {
                    BigInteger.ShiftRightOneInPlace(index3, numArray2);
                    --num4;
                    BigInteger.ShiftRightOneInPlace(start, numArray1);
                }
                else
                {
                    BigInteger.ShiftRightInPlace(index3, numArray2, n2);
                    num4 -= n2;
                    BigInteger.ShiftRightInPlace(start, numArray1, n2);
                }
                while (numArray2[index3] == 0)
                    ++index3;
                while (numArray1[start] == 0)
                    ++start;
                goto label_11;
            }
            else
                a = new int[1];
            label_30:
            if (num1 == 0)
            {
                BigInteger.AddMagnitudes(a, BigInteger.One.magnitude);
                Array.Clear((Array)x, index1, x.Length - index1);
            }
            return a;
        }



        public BigInteger[] DivideAndRemainder(BigInteger val)
        {
            if (val.sign == 0)
                throw new ArithmeticException("Division by zero error");
            BigInteger[] bigIntegerArray = new BigInteger[2];
            if (this.sign == 0)
            {
                bigIntegerArray[0] = BigInteger.Zero;
                bigIntegerArray[1] = BigInteger.Zero;
            }
            else if (val.QuickPow2Check())
            {
                int n = val.Abs().BitLength - 1;
                BigInteger bigInteger = this.Abs().ShiftRight(n);
                int[] mag = this.LastNBits(n);
                bigIntegerArray[0] = val.sign == this.sign ? bigInteger : bigInteger.Negate();
                bigIntegerArray[1] = new BigInteger(this.sign, mag, true);
            }
            else
            {
                int[] numArray = (int[])this.magnitude.Clone();
                int[] mag = this.Divide(numArray, val.magnitude);
                bigIntegerArray[0] = new BigInteger(this.sign * val.sign, mag, true);
                bigIntegerArray[1] = new BigInteger(this.sign, numArray, true);
            }
            return bigIntegerArray;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            return obj is BigInteger x && this.sign == x.sign && this.IsEqualMagnitude(x);
        }

        private bool IsEqualMagnitude(BigInteger x)
        {
            int[] magnitude = x.magnitude;
            if (this.magnitude.Length != x.magnitude.Length)
                return false;
            for (int index = 0; index < this.magnitude.Length; ++index)
            {
                if (this.magnitude[index] != x.magnitude[index])
                    return false;
            }
            return true;
        }



        public override int GetHashCode()
        {
            int length = this.magnitude.Length;
            if (this.magnitude.Length != 0)
            {
                length ^= this.magnitude[0];
                if (this.magnitude.Length > 1)
                    length ^= this.magnitude[this.magnitude.Length - 1];
            }
            return this.sign >= 0 ? length : ~length;
        }

        private BigInteger Inc()
        {
            if (this.sign == 0)
                return BigInteger.One;
            return this.sign < 0 ? new BigInteger(-1, BigInteger.doSubBigLil(this.magnitude, BigInteger.One.magnitude), true) : this.AddToMagnitude(BigInteger.One.magnitude);
        }

        public int IntValue
        {
            get
            {
                if (this.sign == 0)
                    return 0;
                int num = this.magnitude[this.magnitude.Length - 1];
                return this.sign >= 0 ? num : -num;
            }
        }









        public bool RabinMillerTest(int certainty, Random random, bool a) => this.RabinMillerTest(certainty, random, false);



        public long LongValue
        {
            get
            {
                if (this.sign == 0)
                    return 0;
                int length = this.magnitude.Length;
                long num = (long)this.magnitude[length - 1] & (long)uint.MaxValue;
                if (length > 1)
                    num |= ((long)this.magnitude[length - 2] & (long)uint.MaxValue) << 32;
                return this.sign >= 0 ? num : -num;
            }
        }





        public BigInteger Mod(BigInteger m)
        {
            BigInteger bigInteger = m.sign >= 1 ? this.Remainder(m) : throw new ArithmeticException("Modulus must be positive");
            return bigInteger.sign < 0 ? bigInteger.Add(m) : bigInteger;
        }

        public BigInteger ModInverse(BigInteger m)
        {
            if (m.sign < 1)
                throw new ArithmeticException("Modulus must be positive");
            if (m.QuickPow2Check())
                return this.ModInversePow2(m);
            BigInteger u1Out;
            if (!BigInteger.ExtEuclid(this.Remainder(m), m, out u1Out).Equals((object)BigInteger.One))
                throw new ArithmeticException("Numbers not relatively prime.");
            if (u1Out.sign < 0)
                u1Out = u1Out.Add(m);
            return u1Out;
        }

        private BigInteger ModInversePow2(BigInteger m)
        {
            if (!this.TestBit(0))
                throw new ArithmeticException("Numbers not relatively prime.");
            int num1 = m.BitLength - 1;
            long num2 = BigInteger.ModInverse64(this.LongValue);
            if (num1 < 64)
                num2 &= (1L << num1) - 1L;
            BigInteger bigInteger = BigInteger.ValueOf(num2);
            if (num1 > 64)
            {
                BigInteger val = this.Remainder(m);
                int num3 = 64;
                do
                {
                    BigInteger n = bigInteger.Multiply(val).Remainder(m);
                    bigInteger = bigInteger.Multiply(BigInteger.Two.Subtract(n)).Remainder(m);
                    num3 <<= 1;
                }
                while (num3 < num1);
            }
            if (bigInteger.sign < 0)
                bigInteger = bigInteger.Add(m);
            return bigInteger;
        }



        private static long ModInverse64(long d)
        {
            long num1 = d + ((d + 1L & 4L) << 1);
            long num2 = num1 * (2L - d * num1);
            long num3 = num2 * (2L - d * num2);
            long num4 = num3 * (2L - d * num3);
            return num4 * (2L - d * num4);
        }

        private static BigInteger ExtEuclid(BigInteger a, BigInteger b, out BigInteger u1Out)
        {
            BigInteger bigInteger1 = BigInteger.One;
            BigInteger bigInteger2 = BigInteger.Zero;
            BigInteger bigInteger3 = a;
            BigInteger val = b;
            if (val.sign > 0)
            {
                while (true)
                {
                    BigInteger[] bigIntegerArray = bigInteger3.DivideAndRemainder(val);
                    bigInteger3 = val;
                    val = bigIntegerArray[1];
                    BigInteger bigInteger4 = bigInteger1;
                    bigInteger1 = bigInteger2;
                    if (val.sign > 0)
                        bigInteger2 = bigInteger4.Subtract(bigInteger2.Multiply(bigIntegerArray[0]));
                    else
                        break;
                }
            }
            u1Out = bigInteger1;
            return bigInteger3;
        }











        private static int[] Square(int[] w, int[] x)
        {
            int index1 = w.Length - 1;
            for (int index2 = x.Length - 1; index2 > 0; --index2)
            {
                ulong num1 = (ulong)(uint)x[index2];
                ulong num2 = num1 * num1 + (ulong)(uint)w[index1];
                w[index1] = (int)num2;
                ulong num3 = num2 >> 32;
                for (int index3 = index2 - 1; index3 >= 0; --index3)
                {
                    ulong num4 = num1 * (ulong)(uint)x[index3];
                    ulong num5 = num3 + (((ulong)(uint)w[--index1] & (ulong)uint.MaxValue) + (ulong)((uint)num4 << 1));
                    w[index1] = (int)num5;
                    num3 = (num5 >> 32) + (num4 >> 31);
                }
                int index4;
                ulong num6 = num3 + (ulong)(uint)w[index4 = index1 - 1];
                w[index4] = (int)num6;
                int index5;
                if ((index5 = index4 - 1) >= 0)
                    w[index5] = (int)(num6 >> 32);
                index1 = index5 + index2;
            }
            ulong num7 = (ulong)(uint)x[0];
            ulong num8 = num7 * num7 + (ulong)(uint)w[index1];
            w[index1] = (int)num8;
            int index6;
            if ((index6 = index1 - 1) >= 0)
                w[index6] += (int)(num8 >> 32);
            return w;
        }

        private static int[] Multiply(int[] x, int[] y, int[] z)
        {
            int length = z.Length;
            if (length < 1)
                return x;
            int index1 = x.Length - y.Length;
            do
            {
                long num1 = (long)z[--length] & (long)uint.MaxValue;
                long num2 = 0;
                if (num1 != 0L)
                {
                    for (int index2 = y.Length - 1; index2 >= 0; --index2)
                    {
                        long num3 = num2 + (num1 * ((long)y[index2] & (long)uint.MaxValue) + ((long)x[index1 + index2] & (long)uint.MaxValue));
                        x[index1 + index2] = (int)num3;
                        num2 = (long)((ulong)num3 >> 32);
                    }
                }
                --index1;
                if (index1 >= 0)
                    x[index1] = (int)num2;
            }
            while (length > 0);
            return x;
        }










        public BigInteger Multiply(BigInteger val)
        {
            if (val == this)
                return this.Square();
            if ((this.sign & val.sign) == 0)
                return BigInteger.Zero;
            if (val.QuickPow2Check())
            {
                BigInteger bigInteger = this.ShiftLeft(val.Abs().BitLength - 1);
                return val.sign <= 0 ? bigInteger.Negate() : bigInteger;
            }
            if (this.QuickPow2Check())
            {
                BigInteger bigInteger = val.ShiftLeft(this.Abs().BitLength - 1);
                return this.sign <= 0 ? bigInteger.Negate() : bigInteger;
            }
            int[] numArray = new int[this.magnitude.Length + val.magnitude.Length];
            BigInteger.Multiply(numArray, this.magnitude, val.magnitude);
            return new BigInteger(this.sign ^ val.sign ^ 1, numArray, true);
        }

        public BigInteger Square()
        {
            if (this.sign == 0)
                return BigInteger.Zero;
            if (this.QuickPow2Check())
                return this.ShiftLeft(this.Abs().BitLength - 1);
            int length = this.magnitude.Length << 1;
            if ((uint)this.magnitude[0] >> 16 == 0U)
                --length;
            int[] numArray = new int[length];
            BigInteger.Square(numArray, this.magnitude);
            return new BigInteger(1, numArray, false);
        }

        public BigInteger Negate() => this.sign == 0 ? this : new BigInteger(-this.sign, this.magnitude, false);

        public BigInteger Not() => this.Inc().Negate();

        public BigInteger Pow(int exp)
        {
            if (exp <= 0)
            {
                if (exp < 0)
                    throw new ArithmeticException("Negative exponent");
                return BigInteger.One;
            }
            if (this.sign == 0)
                return this;
            if (this.QuickPow2Check())
            {
                long n = (long)exp * (long)(this.BitLength - 1);
                return n <= (long)int.MaxValue ? BigInteger.One.ShiftLeft((int)n) : throw new ArithmeticException("Result too large");
            }
            BigInteger bigInteger = BigInteger.One;
            BigInteger val = this;
            while (true)
            {
                if ((exp & 1) == 1)
                    bigInteger = bigInteger.Multiply(val);
                exp >>= 1;
                if (exp != 0)
                    val = val.Multiply(val);
                else
                    break;
            }
            return bigInteger;
        }


        private int Remainder(int m)
        {
            long num1 = 0;
            for (int index = 0; index < this.magnitude.Length; ++index)
            {
                long num2 = (long)(uint)this.magnitude[index];
                num1 = (num1 << 32 | num2) % (long)m;
            }
            return (int)num1;
        }

        private static int[] Remainder(int[] x, int[] y)
        {
            int index1 = 0;
            while (index1 < x.Length && x[index1] == 0)
                ++index1;
            int index2 = 0;
            while (index2 < y.Length && y[index2] == 0)
                ++index2;
            int num1 = BigInteger.CompareNoLeadingZeroes(index1, x, index2, y);
            if (num1 > 0)
            {
                int num2 = BigInteger.CalcBitLength(1, index2, y);
                int num3 = BigInteger.CalcBitLength(1, index1, x);
                int n1 = num3 - num2;
                int index3 = 0;
                int num4 = num2;
                int[] numArray;
                if (n1 > 0)
                {
                    numArray = BigInteger.ShiftLeft(y, n1);
                    num4 += n1;
                }
                else
                {
                    int length = y.Length - index2;
                    numArray = new int[length];
                    Array.Copy((Array)y, index2, (Array)numArray, 0, length);
                }
            label_10:
                if (num4 < num3 || BigInteger.CompareNoLeadingZeroes(index1, x, index3, numArray) >= 0)
                {
                    BigInteger.Subtract(index1, x, index3, numArray);
                    while (x[index1] == 0)
                    {
                        if (++index1 == x.Length)
                            return x;
                    }
                    num3 = 32 * (x.Length - index1 - 1) + BigInteger.BitLen(x[index1]);
                    if (num3 <= num2)
                    {
                        if (num3 < num2)
                            return x;
                        num1 = BigInteger.CompareNoLeadingZeroes(index1, x, index2, y);
                        if (num1 <= 0)
                            goto label_26;
                    }
                }
                int n2 = num4 - num3;
                if (n2 == 1 && (uint)numArray[index3] >> 1 > (uint)x[index1])
                    ++n2;
                if (n2 < 2)
                {
                    BigInteger.ShiftRightOneInPlace(index3, numArray);
                    --num4;
                }
                else
                {
                    BigInteger.ShiftRightInPlace(index3, numArray, n2);
                    num4 -= n2;
                }
                while (numArray[index3] == 0)
                    ++index3;
                goto label_10;
            }
        label_26:
            if (num1 == 0)
                Array.Clear((Array)x, index1, x.Length - index1);
            return x;
        }

        public BigInteger Remainder(BigInteger n)
        {
            if (n.sign == 0)
                throw new ArithmeticException("Division by zero error");
            if (this.sign == 0)
                return BigInteger.Zero;
            if (n.magnitude.Length == 1)
            {
                int m = n.magnitude[0];
                if (m > 0)
                {
                    if (m == 1)
                        return BigInteger.Zero;
                    int num = this.Remainder(m);
                    if (num == 0)
                        return BigInteger.Zero;
                    return new BigInteger(this.sign, new int[1]
                    {
            num
                    }, false);
                }
            }
            return BigInteger.CompareNoLeadingZeroes(0, this.magnitude, 0, n.magnitude) < 0 ? this : new BigInteger(this.sign, !n.QuickPow2Check() ? BigInteger.Remainder((int[])this.magnitude.Clone(), n.magnitude) : this.LastNBits(n.Abs().BitLength - 1), true);
        }

        private int[] LastNBits(int n)
        {
            if (n < 1)
                return BigInteger.ZeroMagnitude;
            int length = System.Math.Min((n + 32 - 1) / 32, this.magnitude.Length);
            int[] destinationArray = new int[length];
            Array.Copy((Array)this.magnitude, this.magnitude.Length - length, (Array)destinationArray, 0, length);
            int num = (length << 5) - n;
            if (num > 0)
                destinationArray[0] &= (int)(uint.MaxValue >> num);
            return destinationArray;
        }


        private static int[] ShiftLeft(int[] mag, int n)
        {
            int num1 = (int)((uint)n >> 5);
            int num2 = n & 31;
            int length = mag.Length;
            int[] numArray;
            if (num2 == 0)
            {
                numArray = new int[length + num1];
                mag.CopyTo((Array)numArray, 0);
            }
            else
            {
                int index1 = 0;
                int num3 = 32 - num2;
                int num4 = (int)((uint)mag[0] >> num3);
                if (num4 != 0)
                {
                    numArray = new int[length + num1 + 1];
                    numArray[index1++] = num4;
                }
                else
                    numArray = new int[length + num1];
                int num5 = mag[0];
                for (int index2 = 0; index2 < length - 1; ++index2)
                {
                    int num6 = mag[index2 + 1];
                    numArray[index1++] = num5 << num2 | (int)((uint)num6 >> num3);
                    num5 = num6;
                }
                numArray[index1] = mag[length - 1] << num2;
            }
            return numArray;
        }



        public BigInteger ShiftLeft(int n)
        {
            if (this.sign == 0 || this.magnitude.Length == 0)
                return BigInteger.Zero;
            if (n == 0)
                return this;
            if (n < 0)
                return this.ShiftRight(-n);
            BigInteger bigInteger = new BigInteger(this.sign, BigInteger.ShiftLeft(this.magnitude, n), true);
            if (this.nBits != -1)
                bigInteger.nBits = this.sign > 0 ? this.nBits : this.nBits + n;
            if (this.nBitLength != -1)
                bigInteger.nBitLength = this.nBitLength + n;
            return bigInteger;
        }

        private static void ShiftRightInPlace(int start, int[] mag, int n)
        {
            int index1 = (int)((uint)n >> 5) + start;
            int num1 = n & 31;
            int index2 = mag.Length - 1;
            if (index1 != start)
            {
                int num2 = index1 - start;
                for (int index3 = index2; index3 >= index1; --index3)
                    mag[index3] = mag[index3 - num2];
                for (int index4 = index1 - 1; index4 >= start; --index4)
                    mag[index4] = 0;
            }
            if (num1 == 0)
                return;
            int num3 = 32 - num1;
            int num4 = mag[index2];
            for (int index5 = index2; index5 > index1; --index5)
            {
                int num5 = mag[index5 - 1];
                mag[index5] = (int)((uint)num4 >> num1) | num5 << num3;
                num4 = num5;
            }
            mag[index1] = (int)((uint)mag[index1] >> num1);
        }

        private static void ShiftRightOneInPlace(int start, int[] mag)
        {
            int length = mag.Length;
            int num1 = mag[length - 1];
            while (--length > start)
            {
                int num2 = mag[length - 1];
                mag[length] = (int)((uint)num1 >> 1) | num2 << 31;
                num1 = num2;
            }
            mag[start] = (int)((uint)mag[start] >> 1);
        }

        public BigInteger ShiftRight(int n)
        {
            if (n == 0)
                return this;
            if (n < 0)
                return this.ShiftLeft(-n);
            if (n >= this.BitLength)
                return this.sign >= 0 ? BigInteger.Zero : BigInteger.One.Negate();
            int length = this.BitLength - n + 31 >> 5;
            int[] numArray = new int[length];
            int num1 = n >> 5;
            int num2 = n & 31;
            if (num2 == 0)
            {
                Array.Copy((Array)this.magnitude, 0, (Array)numArray, 0, numArray.Length);
            }
            else
            {
                int num3 = 32 - num2;
                int index1 = this.magnitude.Length - 1 - num1;
                for (int index2 = length - 1; index2 >= 0; --index2)
                {
                    numArray[index2] = (int)((uint)this.magnitude[index1--] >> num2);
                    if (index1 >= 0)
                        numArray[index2] |= this.magnitude[index1] << num3;
                }
            }
            return new BigInteger(this.sign, numArray, false);
        }

        public int SignValue => this.sign;

        private static int[] Subtract(int xStart, int[] x, int yStart, int[] y)
        {
            int length1 = x.Length;
            int length2 = y.Length;
            int num1 = 0;
            do
            {
                long num2 = ((long)x[--length1] & (long)uint.MaxValue) - ((long)y[--length2] & (long)uint.MaxValue) + (long)num1;
                x[length1] = (int)num2;
                num1 = (int)(num2 >> 63);
            }
            while (length2 > yStart);
            if (num1 != 0)
            {
                while (--x[--length1] == -1)
                    ;
            }
            return x;
        }

        public BigInteger Subtract(BigInteger n)
        {
            if (n.sign == 0)
                return this;
            if (this.sign == 0)
                return n.Negate();
            if (this.sign != n.sign)
                return this.Add(n.Negate());
            int num = BigInteger.CompareNoLeadingZeroes(0, this.magnitude, 0, n.magnitude);
            if (num == 0)
                return BigInteger.Zero;
            BigInteger bigInteger1;
            BigInteger bigInteger2;
            if (num < 0)
            {
                bigInteger1 = n;
                bigInteger2 = this;
            }
            else
            {
                bigInteger1 = this;
                bigInteger2 = n;
            }
            return new BigInteger(this.sign * num, BigInteger.doSubBigLil(bigInteger1.magnitude, bigInteger2.magnitude), true);
        }

        private static int[] doSubBigLil(int[] bigMag, int[] lilMag) => BigInteger.Subtract(0, (int[])bigMag.Clone(), 0, lilMag);




        public override string ToString() => this.ToString(10);

        public string ToString(int radix)
        {
            if (radix <= 8)
            {
                if (radix == 2 || radix == 8)
                    goto label_4;
            }
            else if (radix == 10 || radix == 16)
                goto label_4;
            throw new FormatException("Only bases 2, 8, 10, 16 are allowed");
        label_4:
            if (this.magnitude == null)
                return "null";
            if (this.sign == 0)
                return "0";
            int index1 = 0;
            while (index1 < this.magnitude.Length && this.magnitude[index1] == 0)
                ++index1;
            if (index1 == this.magnitude.Length)
                return "0";
            StringBuilder sb = new StringBuilder();
            if (this.sign == -1)
                sb.Append('-');
            switch (radix)
            {
                case 2:
                    int index2 = index1;
                    sb.Append(Convert.ToString(this.magnitude[index2], 2));
                    while (++index2 < this.magnitude.Length)
                        BigInteger.AppendZeroExtendedString(sb, Convert.ToString(this.magnitude[index2], 2), 32);
                    break;
                case 8:
                    int num = 1073741823;
                    BigInteger bigInteger1 = this.Abs();
                    int bitLength = bigInteger1.BitLength;
                    IList arrayList1 = Platform.CreateArrayList();
                    for (; bitLength > 30; bitLength -= 30)
                    {
                        arrayList1.Add((object)Convert.ToString(bigInteger1.IntValue & num, 8));
                        bigInteger1 = bigInteger1.ShiftRight(30);
                    }
                    sb.Append(Convert.ToString(bigInteger1.IntValue, 8));
                    for (int index3 = arrayList1.Count - 1; index3 >= 0; --index3)
                        BigInteger.AppendZeroExtendedString(sb, (string)arrayList1[index3], 10);
                    break;
                case 10:
                    BigInteger pos = this.Abs();
                    if (pos.BitLength < 64)
                    {
                        sb.Append(Convert.ToString(pos.LongValue, radix));
                        break;
                    }
                    IList arrayList2 = Platform.CreateArrayList();
                    for (BigInteger bigInteger2 = BigInteger.ValueOf((long)radix); bigInteger2.CompareTo(pos) <= 0; bigInteger2 = bigInteger2.Square())
                        arrayList2.Add((object)bigInteger2);
                    int count = arrayList2.Count;
                    sb.EnsureCapacity(sb.Length + (1 << count));
                    BigInteger.ToString(sb, radix, arrayList2, count, pos);
                    break;
                case 16:
                    int index4 = index1;
                    sb.Append(Convert.ToString(this.magnitude[index4], 16));
                    while (++index4 < this.magnitude.Length)
                        BigInteger.AppendZeroExtendedString(sb, Convert.ToString(this.magnitude[index4], 16), 8);
                    break;
            }
            return sb.ToString();
        }

        private static void ToString(
          StringBuilder sb,
          int radix,
          IList moduli,
          int scale,
          BigInteger pos)
        {
            if (pos.BitLength < 64)
            {
                string s = Convert.ToString(pos.LongValue, radix);
                if (sb.Length > 1 || sb.Length == 1 && sb[0] != '-')
                {
                    BigInteger.AppendZeroExtendedString(sb, s, 1 << scale);
                }
                else
                {
                    if (pos.SignValue == 0)
                        return;
                    sb.Append(s);
                }
            }
            else
            {
                BigInteger[] bigIntegerArray = pos.DivideAndRemainder((BigInteger)moduli[--scale]);
                BigInteger.ToString(sb, radix, moduli, scale, bigIntegerArray[0]);
                BigInteger.ToString(sb, radix, moduli, scale, bigIntegerArray[1]);
            }
        }

        private static void AppendZeroExtendedString(StringBuilder sb, string s, int minLength)
        {
            for (int length = s.Length; length < minLength; ++length)
                sb.Append('0');
            sb.Append(s);
        }

        private static BigInteger CreateUValueOf(ulong value)
        {
            int num1 = (int)(value >> 32);
            int num2 = (int)value;
            if (num1 != 0)
                return new BigInteger(1, new int[2]
                {
          num1,
          num2
                }, false);
            if (num2 == 0)
                return BigInteger.Zero;
            BigInteger uvalueOf = new BigInteger(1, new int[1]
            {
        num2
            }, false);
            if ((num2 & -num2) == num2)
                uvalueOf.nBits = 1;
            return uvalueOf;
        }

        private static BigInteger CreateValueOf(long value)
        {
            if (value >= 0L)
                return BigInteger.CreateUValueOf((ulong)value);
            return value == long.MinValue ? BigInteger.CreateValueOf(~value).Not() : BigInteger.CreateValueOf(-value).Negate();
        }

        public static BigInteger ValueOf(long value) => value >= 0L && value < (long)BigInteger.SMALL_CONSTANTS.Length ? BigInteger.SMALL_CONSTANTS[value] : BigInteger.CreateValueOf(value);

        public int GetLowestSetBit() => this.sign == 0 ? -1 : this.GetLowestSetBitMaskFirst(-1);

        private int GetLowestSetBitMaskFirst(int firstWordMask)
        {
            int length = this.magnitude.Length;
            int lowestSetBitMaskFirst = 0;
            int num1;
            uint num2 = (uint)(this.magnitude[num1 = length - 1] & firstWordMask);
            while (num2 == 0U)
            {
                num2 = (uint)this.magnitude[--num1];
                lowestSetBitMaskFirst += 32;
            }
            while (((int)num2 & (int)byte.MaxValue) == 0)
            {
                num2 >>= 8;
                lowestSetBitMaskFirst += 8;
            }
            while (((int)num2 & 1) == 0)
            {
                num2 >>= 1;
                ++lowestSetBitMaskFirst;
            }
            return lowestSetBitMaskFirst;
        }

        public bool TestBit(int n)
        {
            if (n < 0)
                throw new ArithmeticException("Bit position must not be negative");
            if (this.sign < 0)
                return !this.Not().TestBit(n);
            int num = n / 32;
            return num < this.magnitude.Length && (this.magnitude[this.magnitude.Length - 1 - num] >> n % 32 & 1) > 0;
        }

        public BigInteger Or(BigInteger value)
        {
            if (this.sign == 0)
                return value;
            if (value.sign == 0)
                return this;
            int[] numArray1 = this.sign > 0 ? this.magnitude : this.Add(BigInteger.One).magnitude;
            int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add(BigInteger.One).magnitude;
            bool flag = this.sign < 0 || value.sign < 0;
            int[] mag = new int[System.Math.Max(numArray1.Length, numArray2.Length)];
            int num1 = mag.Length - numArray1.Length;
            int num2 = mag.Length - numArray2.Length;
            for (int index = 0; index < mag.Length; ++index)
            {
                int num3 = index >= num1 ? numArray1[index - num1] : 0;
                int num4 = index >= num2 ? numArray2[index - num2] : 0;
                if (this.sign < 0)
                    num3 = ~num3;
                if (value.sign < 0)
                    num4 = ~num4;
                mag[index] = num3 | num4;
                if (flag)
                    mag[index] = ~mag[index];
            }
            BigInteger bigInteger = new BigInteger(1, mag, true);
            if (flag)
                bigInteger = bigInteger.Not();
            return bigInteger;
        }

        public BigInteger Xor(BigInteger value)
        {
            if (this.sign == 0)
                return value;
            if (value.sign == 0)
                return this;
            int[] numArray1 = this.sign > 0 ? this.magnitude : this.Add(BigInteger.One).magnitude;
            int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add(BigInteger.One).magnitude;
            bool flag = this.sign < 0 && value.sign >= 0 || this.sign >= 0 && value.sign < 0;
            int[] mag = new int[System.Math.Max(numArray1.Length, numArray2.Length)];
            int num1 = mag.Length - numArray1.Length;
            int num2 = mag.Length - numArray2.Length;
            for (int index = 0; index < mag.Length; ++index)
            {
                int num3 = index >= num1 ? numArray1[index - num1] : 0;
                int num4 = index >= num2 ? numArray2[index - num2] : 0;
                if (this.sign < 0)
                    num3 = ~num3;
                if (value.sign < 0)
                    num4 = ~num4;
                mag[index] = num3 ^ num4;
                if (flag)
                    mag[index] = ~mag[index];
            }
            BigInteger bigInteger = new BigInteger(1, mag, true);
            if (flag)
                bigInteger = bigInteger.Not();
            return bigInteger;
        }
    }

    internal abstract class Platform
    {
        internal static IList CreateArrayList() => (IList)new List<object>();
    }
}
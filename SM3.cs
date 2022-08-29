using System;
using System.Linq;
using System.Text;

namespace SM23Crypto
{
    public class SM3
    {
        /**
    * 循环左移
    */
        private static byte[] rotl(byte[] x, int n)
        {
            byte[] result = new byte[x.Length];
            int a = n / 8; // 偏移 a 字节
            int b = n % 8;// 偏移 b 位
            var len = x.Length;
            for (var i = 0; i < len; i++)
            {
                // current << b + (current + 1) >>> (8 - b)
                result[i] = (byte)(((x[(i + a) % len] << b) & 0xff) + ((int)((uint)x[(i + a + 1) % len] >> (8 - b)) & 0xff));
            }
            return result;
        }


        /**
    * 消息扩展中的置换函数 P1(X) = X xor (X <<< 15) xor (X <<< 23)
    */
        private static byte[] P1(byte[] X)
        {
            return xor(xor(X, rotl(X, 15)), rotl(X, 23));
        }

        /**
    * 压缩函数中的置换函数 P1(X) = X xor (X <<< 9) xor (X <<< 17)
    */
        private static byte[] P0(byte[] X)
        {
            return xor(xor(X, rotl(X, 9)), rotl(X, 17));
        }

        /**
    * 二进制异或运算
    */

        private static byte[] xor(byte[] x, byte[] y)
        {
            byte[] result = new byte[x.Length];
            for (var i = x.Length - 1; i >= 0; i--)
            {
                if (i > y.Length - 1)
                {
                    result[i] = 1;
                    continue;
                }
                result[i] = (byte)((x[i] ^ y[i]) & 0xff);
            }

            return result;
        }


        /**
    * 二进制与运算
    */
        private static byte[] add(byte[] x, byte[] y)
        {
            byte[] result = new byte[x.Length];
            int temp = 0;
            for (int i = x.Length - 1; i >= 0; i--)
            {
                int sum = x[i] + y[i] + temp;
                if (sum > 0xff)
                {
                    temp = 1;
                    result[i] = (byte)(sum & 0xff);
                }
                else
                {
                    temp = 0;
                    result[i] = (byte)(sum & 0xff);
                }
            }

            return result;
        }

        /**
    * 二进制与运算
    */
        private static byte[] and(byte[] x, byte[] y)
        {
            byte[] result = new byte[x.Length];
            for (var i = x.Length - 1; i >= 0; i--) result[i] = (byte)((x[i] & y[i]) & 0xff);
            return result;
        }


        /**
    * 二进制或运算
    */
        private static byte[] or(byte[] x, byte[] y)
        {
            byte[] result = new byte[x.Length];
            for (var i = x.Length - 1; i >= 0; i--) result[i] = (byte)((x[i] | y[i]) & 0xff);
            return result;
        }

        /**
    * 二进制非运算
    */
        private static byte[] not(byte[] x)
        {
            byte[] result = new byte[x.Length];
            for (var i = x.Length - 1; i >= 0; i--) result[i] = (byte)((~x[i]) & 0xff);
            return result;
        }

        /**
    * 布尔函数 FF
    */
        private static byte[] FF(byte[] X, byte[] Y, byte[] Z, int j)
        {
            return j >= 0 && j <= 15 ? xor(xor(X, Y), Z) : or(or(and(X, Y), and(X, Z)), and(Y, Z));
        }

        /**
    * 布尔函数 GG
    */
        private static byte[] GG(byte[] X, byte[] Y, byte[] Z, int j)
        {
            return j >= 0 && j <= 15 ? xor(xor(X, Y), Z) : or(and(X, Y), and(not(X), Z));
        }

        /**
    * 压缩函数
    */
        private static byte[] CF(byte[] V, byte[] Bi)
        {
            byte[][] W = new byte[68][];
            for (int i = 0; i < 68; i++) W[i] = new byte[4];
            byte[][] M = new byte[64][]; //W'
            for (int i = 0; i < 64; i++) M[i] = new byte[4];
            // 将消息分组B划分为 16 个字 W0， W1，……，W15
            for (var i = 0; i < 16; i++)
            {
                int start = i * 4;
                W[i][0] = Bi[start];
                W[i][1] = Bi[start + 1];
                W[i][2] = Bi[start + 2];
                W[i][3] = Bi[start + 3];
            }

            for (var j = 16; j < 68; j++)
            {
                var value = xor(xor(P1(xor(
                    xor(W[j - 16], W[j - 9]),
                    rotl(W[j - 3], 15)
                )), rotl(W[j - 13], 7)), W[j - 6]);
                W[j][0] = value[0];
                W[j][1] = value[1];
                W[j][2] = value[2];
                W[j][3] = value[3];
            }

            // W′0 ～ W′63：W′[j] = W[j] xor W[j+4]
            for (var j = 0; j < 64; j++)
            {
                var value = xor(W[j], W[j + 4]);
                M[j][0] = value[0];
                M[j][1] = value[1];
                M[j][2] = value[2];
                M[j][3] = value[3];
            }

            // 压缩
            byte[] T1 = new byte[] { 0x79, 0xcc, 0x45, 0x19 };
            byte[] T2 = new byte[] { 0x7a, 0x87, 0x9d, 0x8a };

            // 字寄存器
            byte[] A = V.Slice(0, 4).ToArray();
            byte[] B = V.Slice(4, 8).ToArray();
            byte[] C = V.Slice(8, 12).ToArray();
            byte[] D = V.Slice(12, 16).ToArray();
            byte[] E = V.Slice(16, 20).ToArray();
            byte[] F = V.Slice(20, 24).ToArray();
            byte[] G = V.Slice(24, 28).ToArray();
            byte[] H = V.Slice(28, 32).ToArray();

            // 中间变量
            byte[] SS1 = new byte[0];
            byte[] SS2 = new byte[0];
            byte[] TT1 = new byte[0];
            byte[] TT2 = new byte[0];

            for (int j = 0; j < 64; j++)
            {
                byte[] T = j >= 0 && j <= 15 ? T1 : T2;
                SS1 = rotl(add(add(rotl(A, 12), E), rotl(T, j)), 7);
                SS2 = xor(SS1, rotl(A, 12));
                TT1 = add(add(add(FF(A, B, C, j), D), SS2), M[j]);
                TT2 = add(add(add(GG(E, F, G, j), H), SS1), W[j]);
                D = C;
                C = rotl(B, 9);
                B = A;
                A = TT1;
                H = G;
                G = rotl(F, 19);
                F = E;
                E = P0(TT2);
            }
            return xor(A.Concat(B).Concat(C).Concat(D).Concat(E).Concat(F).Concat(G).Concat(H).ToArray(), V);
        }

        private static string IntToBinaryStr(int value)
        {
            return Convert.ToString(value, 2).PadLeft(4, '0');
        }

        private static byte BinaryStrToByte(string value)
        {
            return Convert.ToByte(value, 2);
        }

        public static string StrSum(string str)
        {
            return (SMUtils.arrayToHex(SM3Array(Encoding.UTF8.GetBytes(str))).ToUpper());
        }

        public static byte[] SM3Array(byte[] arr)
        {
            /*
             假设消息m 的长度为len 比特。首先将比特“1”添加到消息的末尾，再添加k 个“0”，k是满
               足len + 1 + k ≡ 448mod512 的最小的非负整数。然后再添加一个64位比特串，该比特串是长度len的二进
               制表示。填充后的消息m′ 的比特长度为512的倍数。
             */

            // 每一个元素代表8bit数据（下同），此处len为总bit数
            int len = arr.Length * 8;
            // k 是满足 len + 1 + k = 448(mod512) 的最小的非负整数
            int k = len % 512;
            //k为需要填充0的个数。
            k = k >= 448 ? 512 - (k % 448) - 1 : 448 - k - 1;

            //计算需要填充0的元素数，即去填充位起始元素0x80（二进制为10000000）中7bit 0后需要填充0的元素数。
            int zerolen = (k - 7) / 8;
            byte[] zeroArr = new Byte[zerolen];
            for (var i = 0; i < zerolen; i++)
            {
                zeroArr[i] = 0;
            }
            //将int型len转换为二进制字符串
            var lenStr = IntToBinaryStr(len);
            //声明64位比特串，该比特串是长度len的二进制表示
            byte[] lenArr = new Byte[8];
            for (var i = 7; i >= 0; i--)
            {
                if (lenStr.Length > 8)
                {
                    int start = lenStr.Length - 8;
                    lenArr[i] = BinaryStrToByte(lenStr.Substring(start));
                    lenStr = lenStr.Substring(0, start);
                }
                else if (lenStr.Length > 0)
                {
                    lenArr[i] = BinaryStrToByte(lenStr);
                    lenStr = "";
                }
                else
                {
                    lenArr[i] = 0;
                }
            }

            //包含比特“1”的块。
            byte[] oneBlock = new[] { (byte)(0x80) };

            //填充后的数组长度为512的整数倍
            byte[] m = arr.Concat(oneBlock).Concat(zeroArr).Concat(lenArr).ToArray();
            // 迭代压缩
            //计算分组数n,每一组为512bit即64块。
            int n = m.Length / 64;
            /*
             4.1 初始值
                   IV =7380166f 4914b2b9 172442d7 da8a0600 a96f30bc 163138aa e38dee4d b0fb0e4e
             */
            //V0 32块256bit
            byte[] V = new byte[32] { 0x73, 0x80, 0x16, 0x6f, 0x49, 0x14, 0xb2, 0xb9, 0x17, 0x24, 0x42, 0xd7, 0xda, 0x8a, 0x06, 0x00, 0xa9, 0x6f, 0x30, 0xbc, 0x16, 0x31, 0x38, 0xaa, 0xe3, 0x8d, 0xee, 0x4d, 0xb0, 0xfb, 0x0e, 0x4e };
            for (var i = 0; i < n; i++)
            {
                int start = 64 * i;
                byte[] B = m.Skip(start).Take(64).ToArray();
                V = CF(V, B);
            }
            return V;
        }
    }
}
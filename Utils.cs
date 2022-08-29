using System;
using System.Collections.Generic;
using System.Linq;

namespace SM23Crypto
{
    public static class SMExtensions
    {
        public static string PadLeftZero(this string str, int num)
        {
            return str.PadLeft(num, '0');
        }



        public static IEnumerable<T> Slice<T>(this IEnumerable<T> l, int start, int end)
        {
            return l.Skip(start).Take(end - start);
        }
        public static IEnumerable<byte> Slice<T>(this IEnumerable<byte> l, int newValue)
        {
            return l.Append((byte)newValue);
        }
    }

    public class SMUtils
    {
        public static string arrayToHex(byte[] arr)
        {
            var res = "";
            foreach (var i in arr)
            {
                res += i.ToString("x2");
            }

            return res;
        }



        public static byte hexStrToByte(string i)
        {
            return byte.Parse(i, System.Globalization.NumberStyles.HexNumber);
        }


        /**
     * 转成字节数组
     */
        public static byte[] hexToArray(string hexStr)
        {
            int hexStrLength = hexStr.Length;
            byte[] words = new byte[hexStrLength / 2];
            if (hexStrLength % 2 != 0)
            {
                hexStr = hexStr.PadLeft(hexStrLength + 1, '0');
            }

            hexStrLength = hexStr.Length;
            for (int i = 0; i < hexStrLength; i += 2)
            {
                words[i / 2] = hexStrToByte(hexStr.Substring(i, 2));
                // words.Add(BitConverter.GetBytes(hexStrToInt(hexStr.Substring(i, 2)))[0]);
                // words.Add(Encoding.UTF8.GetBytes(hexStr.Substring(i, 2))[0]);
            }

            return words;
        }

        /**
     * 获取公共椭圆曲线
     */
        public static Tuple<ECCurveFp, ECPointFp, BigInteger> getGlobalCurve()
        {
            BigInteger p = new BigInteger("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF", 16);
            BigInteger a = new BigInteger("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC", 16);
            BigInteger b = new BigInteger("28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93", 16);
            ECCurveFp curve = new ECCurveFp(p, a, b);
            string gxHex = "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7";
            string gyHex = "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0";
            ECPointFp G = curve.decodePointHex("04" + gxHex + gyHex);
            BigInteger n = new BigInteger("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123", 16);
            return new Tuple<ECCurveFp, ECPointFp, BigInteger>(curve, G, n);
        }

        /**
        * 转成16进制串
        */
    }


    public class ECPointFp
    {
        public ECCurveFp curve;
        public ECFieldElementFp x, y;
        public BigInteger z;
        public BigInteger zinv;

        public ECPointFp(ECCurveFp ecCurveFp, ECFieldElementFp x, ECFieldElementFp y, BigInteger z = null)
        {

            this.curve = ecCurveFp;
            this.x = x;
            this.y = y;
            // 标准射影坐标系：zinv == null 或 z * zinv == 1
            this.z = z == null ? BigInteger.One : z;
            this.zinv = null;
        }

        public ECFieldElementFp getX()
        {
            if (this.zinv == null) this.zinv = this.z.ModInverse(this.curve.q);
            return this.curve.fromBigInteger(this.x.toBigInteger().Multiply(this.zinv).Mod(this.curve.q));
        }

        public ECFieldElementFp getY()
        {

            if (this.zinv == null) this.zinv = this.z.ModInverse(this.curve.q);
            return this.curve.fromBigInteger(this.y.toBigInteger().Multiply(this.zinv).Mod(this.curve.q));
        }


        /**
       * 判断相等
       */
        public bool equals(ECPointFp other)
        {
            if (other == this) return true;
            if (this.isInfinity()) return other.isInfinity();
            if (other.isInfinity()) return this.isInfinity();

            // u = y2 * z1 - y1 * z2
            BigInteger u = other.y.toBigInteger().Multiply(this.z).Subtract(this.y.toBigInteger().Multiply(other.z)).Mod(this.curve.q);
            if (!u.Equals(BigInteger.Zero)) return false;

            // v = x2 * z1 - x1 * z2
            BigInteger v = other.x.toBigInteger().Multiply(this.z).Subtract(this.x.toBigInteger().Multiply(other.z)).Mod(this.curve.q);
            return v.Equals(BigInteger.Zero);
        }


        /**
       * 是否是无穷远点
       */
        public bool isInfinity()
        {
            if ((this.x == null) && (this.y == null)) return true;
            return this.z.Equals(BigInteger.Zero) && !this.y.toBigInteger().Equals(BigInteger.Zero);
        }

        /**
       * 取反，x 轴对称点
       */
        public ECPointFp negate()
        {
            return new ECPointFp(this.curve, this.x, this.y.negate(), this.z);
        }


        /**
       * 相加
       *
       * 标准射影坐标系：
       *
       * λ1 = x1 * z2
       * λ2 = x2 * z1
       * λ3 = λ1 − λ2
       * λ4 = y1 * z2
       * λ5 = y2 * z1
       * λ6 = λ4 − λ5
       * λ7 = λ1 + λ2
       * λ8 = z1 * z2
       * λ9 = λ3^2
       * λ10 = λ3 * λ9
       * λ11 = λ8 * λ6^2 − λ7 * λ9
       * x3 = λ3 * λ11
       * y3 = λ6 * (λ9 * λ1 − λ11) − λ4 * λ10
       * z3 = λ10 * λ8
       */

        public ECPointFp add(ECPointFp b)
        {
            if (this.isInfinity()) return b;
            if (b.isInfinity()) return this;

            BigInteger x1 = this.x.toBigInteger();
            BigInteger y1 = this.y.toBigInteger();
            BigInteger z1 = this.z;
            BigInteger x2 = b.x.toBigInteger();
            BigInteger y2 = b.y.toBigInteger();
            BigInteger z2 = b.z;
            BigInteger q = this.curve.q;

            BigInteger w1 = x1.Multiply(z2).Mod(q);
            BigInteger w2 = x2.Multiply(z1).Mod(q);
            BigInteger w3 = w1.Subtract(w2);
            BigInteger w4 = y1.Multiply(z2).Mod(q);
            BigInteger w5 = y2.Multiply(z1).Mod(q);
            BigInteger w6 = w4.Subtract(w5);

            if (BigInteger.Zero.Equals(w3))
            {
                if (BigInteger.Zero.Equals(w6))
                {
                    return this.twice(); // this == b，计算自加
                }

                return this.curve.infinity; // this == -b，则返回无穷远点
            }

            BigInteger w7 = w1.Add(w2);
            BigInteger w8 = z1.Multiply(z2).Mod(q);
            BigInteger w9 = w3.Square().Mod(q);
            BigInteger w10 = w3.Multiply(w9).Mod(q);
            BigInteger w11 = w8.Multiply(w6.Square()).Subtract(w7.Multiply(w9)).Mod(q);

            BigInteger x3 = w3.Multiply(w11).Mod(q);
            BigInteger y3 = w6.Multiply(w9.Multiply(w1).Subtract(w11)).Subtract(w4.Multiply(w10)).Mod(q);
            BigInteger z3 = w10.Multiply(w8).Mod(q);

            return new ECPointFp(this.curve, this.curve.fromBigInteger(x3),
                this.curve.fromBigInteger(y3), z3);
        }

        /**
       * 自加
       *
       * 标准射影坐标系：
       *
       * λ1 = 3 * x1^2 + a * z1^2
       * λ2 = 2 * y1 * z1
       * λ3 = y1^2
       * λ4 = λ3 * x1 * z1
       * λ5 = λ2^2
       * λ6 = λ1^2 − 8 * λ4
       * x3 = λ2 * λ6
       * y3 = λ1 * (4 * λ4 − λ6) − 2 * λ5 * λ3
       * z3 = λ2 * λ5
       */
        public ECPointFp twice()
        {
            if (this.isInfinity()) return this;
            //Bug
            if (this.y.toBigInteger().SignValue == 0) return this.curve.infinity;

            BigInteger x1 = this.x.toBigInteger();
            BigInteger y1 = this.y.toBigInteger();
            BigInteger z1 = this.z;
            BigInteger q = this.curve.q;
            BigInteger a = this.curve.a.toBigInteger();

            BigInteger w1 = x1.Square().Multiply(BigInteger.Three).Add(a.Multiply(z1.Square())).Mod(q);
            BigInteger w2 = y1.ShiftLeft(1).Multiply(z1).Mod(q);
            BigInteger w3 = y1.Square().Mod(q);
            BigInteger w4 = w3.Multiply(x1).Multiply(z1).Mod(q);
            BigInteger w5 = w2.Square().Mod(q);
            BigInteger w6 = w1.Square().Subtract(w4.ShiftLeft(3)).Mod(q);

            BigInteger x3 = w2.Multiply(w6).Mod(q);
            BigInteger y3 = w1.Multiply(w4.ShiftLeft(2).Subtract(w6)).Subtract(w5.ShiftLeft(1).Multiply(w3)).Mod(q);
            BigInteger z3 = w2.Multiply(w5).Mod(q);
            return new ECPointFp(this.curve, this.curve.fromBigInteger(x3), this.curve.fromBigInteger(y3), z3);
        }


        /**
       * 倍点计算
       */
        public ECPointFp multiply(BigInteger k)
        {
            if (this.isInfinity()) return this;
            if (k.SignValue == 0) return this.curve.infinity;

            // 使用加减法
            BigInteger k3 = k.Multiply(BigInteger.Three);
            ECPointFp neg = this.negate();
            ECPointFp Q = this;

            for (var i = k3.BitLength - 2; i > 0; i--)
            {
                Q = Q.twice();

                bool k3Bit = k3.TestBit(i);
                bool kBit = k.TestBit(i);

                if (k3Bit != kBit)
                {
                    Q = Q.add(k3Bit ? this : neg);
                }
            }

            return Q;
        }


    }


    /**
     * 椭圆曲线域元素
     */
    public class ECFieldElementFp
    {
        private BigInteger q;
        private BigInteger x;
        public ECFieldElementFp(BigInteger q, BigInteger x)
        {
            this.q = q;
            this.x = x;
        }

        /**
       * 判断相等
       */
        public bool equals(ECFieldElementFp other)
        {
            if (other == this)
            {
                return true;
            }
            return (this.q.Equals(other.q) && this.x.Equals(other.x));
        }

        /**
       * 返回具体数值
       */
        public BigInteger toBigInteger()
        {
            return this.x;
        }

        /**
       * 取反
       */
        public ECFieldElementFp negate()
        {
            return new ECFieldElementFp(this.q, this.x.Negate().Mod(this.q));
        }

        /**
       * 相加
       */
        public ECFieldElementFp add(ECFieldElementFp b)
        {
            return new ECFieldElementFp(this.q, this.x.Add(b.toBigInteger()).Mod(this.q));
        }

        /**
       * 相乘
       */
        public ECFieldElementFp multiply(ECFieldElementFp b)
        {
            return new ECFieldElementFp(this.q, this.x.Multiply(b.toBigInteger()).Mod(this.q));
        }

        /**
       * 相除
       */
        public ECFieldElementFp divide(ECFieldElementFp b)
        {
            return new ECFieldElementFp(this.q, this.x.Multiply(b.toBigInteger().ModInverse(this.q)).Mod(this.q));
        }

        /**
       * 平方
       */
        public ECFieldElementFp square()
        {
            return new ECFieldElementFp(this.q, this.x.Square().Mod(this.q));
        }
    }


    public class ECCurveFp
    {
        public BigInteger q;
        public ECFieldElementFp a, b;
        public ECPointFp infinity;
        public ECCurveFp(BigInteger q, BigInteger a, BigInteger b)
        {
            this.q = q;
            this.a = this.fromBigInteger(a);
            this.b = this.fromBigInteger(b);
            this.infinity = new ECPointFp(this, null, null); // 无穷远点
        }

        /**
       * 生成椭圆曲线域元素
       */
        public ECFieldElementFp fromBigInteger(BigInteger x)
        {
            return new ECFieldElementFp(this.q, x);
        }

        /**
       * 解析 16 进制串为椭圆曲线点
       */
        public ECPointFp decodePointHex(string s)
        {
            switch (int.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber))
            {
                // 第一个字节
                case 0:
                    return this.infinity;
                case 2:
                case 3:
                    // 不支持的压缩方式
                    return null;
                case 4:
                case 6:
                case 7:
                    int len = (s.Length - 2) / 2;
                    string xHex = s.Substring(2, len);
                    string yHex = s.Substring(len + 2, len);

                    return new ECPointFp(this, this.fromBigInteger(new BigInteger(xHex, 16)),
                        this.fromBigInteger(new BigInteger(yHex, 16)));
                default:
                    // 不支持
                    return null;
            }
        }
    }
}
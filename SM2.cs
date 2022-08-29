using System;
using System.Globalization;
using System.Linq;
using System.Text;
namespace SM23Crypto
{
    //扩展方法
    public class SM2Res
    {
        private byte[] _data;

        public SM2Res(byte[] data)
        {
            this._data = data;
        }

        public byte[] GetBytes()
        {
            return this._data;
        }

        public string ToString()
        {
            return Encoding.UTF8.GetString(this._data);
        }
    }

    public class SM2Key
    {
        public string PubKey { get; set; }
        public string PriKey { get; set; }
    }

    public class SM2
    {
        public enum CipherMode
        {
            C1C2C3 = 0,
            C1C3C2 = 1,
        }


        /**
        * 生成密钥对：publicKey = privateKey * G
        */
        public static SM2Key GenerateKeyPairHex()
        {
            Tuple<ECCurveFp, ECPointFp, BigInteger> curve = SMUtils.getGlobalCurve();
            Random rand = new Random();
            BigInteger random = new BigInteger(256, rand);
            BigInteger d = random.Mod(curve.Item3.Subtract(BigInteger.One)).Add(BigInteger.One); // 随机数
            string privateKey = d.ToString(16).PadLeft(64, '0');

            ECPointFp P = curve.Item2.multiply(d); // P = dG，p 为公钥，d 为私钥
            string Px = P.getX().toBigInteger().ToString(16).PadLeftZero(64);
            string Py = P.getY().toBigInteger().ToString(16).PadLeftZero(64);
            string publicKey = "04" + Px + Py;
            return new SM2Key()
            {
                PriKey = privateKey,
                PubKey = publicKey
            };
        }

        public static string DoEncrypt(string msg, string publicKey, CipherMode cipherMode = CipherMode.C1C3C2)
        {

            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
            ECPointFp publicKeyPoint = SMUtils.getGlobalCurve().Item1.decodePointHex(publicKey);
            SM2Key keypair = GenerateKeyPairHex();
            BigInteger k = new BigInteger(keypair.PriKey, 16); //随机数k

            // c1 = k * G
            string c1 = keypair.PubKey;
            if (c1.Length > 128)
            {
                c1 = c1.Substring(c1.Length - 128);
            }

            // (x2, y2) = k * publicKey
            ECPointFp p = publicKeyPoint.multiply(k);
            byte[] x2 = SMUtils.hexToArray(p.getX().toBigInteger().ToString(16).PadLeftZero(64));
            byte[] y2 = SMUtils.hexToArray(p.getY().toBigInteger().ToString(16).PadLeftZero(64));


            // c3 = hash(x2 || msg || y2)
            string c3 = SMUtils.arrayToHex(SM3.SM3Array(x2.Concat(msgBytes).Concat(y2).ToArray()));




            int ct = 1;
            int offset = 0;
            byte[] t = new byte[0]; // 256 位
            byte[] z = x2.Concat(y2).ToArray();
            byte[] n = new[] { (byte)(ct >> 24 & 0x00ff), (byte)(ct >> 16 & 0x00ff), (byte)(ct >> 8 & 0x00ff), (byte)(ct & 0x00ff) };
            t = SM3.SM3Array(z.Concat(n).ToArray());
            ct++;
            offset = 0;
            int len = msgBytes.Length;
            for (int i = 0; i < len; i++)
            {
                // t = Ha1 || Ha2 || Ha3 || Ha4
                if (offset == t.Length)
                {
                    t = SM3.SM3Array(z.Concat(n).ToArray());
                    ct++;
                    offset = 0;
                }
                // c2 = msg ^ t
                msgBytes[i] ^= (byte)(t[offset++] & 0xff);
            }

            string c2 = SMUtils.arrayToHex(msgBytes);


            return (cipherMode == CipherMode.C1C2C3 ? c1 + c2 + c3 : c1 + c3 + c2).ToLower();

        }

        public static SM2Res DoDecrypt(string encryptData, string privateKey, CipherMode cipherMode = CipherMode.C1C3C2)
        {

            BigInteger privateKeyI = new BigInteger(privateKey, 16);
            string c3 = encryptData.Substring(128, 64);
            string c2 = encryptData.Substring(128 + 64);
            if (cipherMode == CipherMode.C1C2C3)
            {
                c3 = encryptData.Substring(encryptData.Length - 64);
                c2 = encryptData.Substring(128, encryptData.Length - 128 - 64);
            }

            byte[] msg = SMUtils.hexToArray(c2);
            var sss = encryptData.Substring(0, 128);
            ECPointFp c1 = SMUtils.getGlobalCurve().Item1.decodePointHex("04" + encryptData.Substring(0, 128));
            ECPointFp p = c1.multiply(privateKeyI);
            byte[] x2 = SMUtils.hexToArray(p.getX().toBigInteger().ToString(16).PadLeft(64, '0'));
            byte[] y2 = SMUtils.hexToArray(p.getY().toBigInteger().ToString(16).PadLeft(64, '0'));
            var ct = 1;
            var offset = 0;
            // var t = [] ;// 256 位
            byte[] z = x2.Concat(y2).ToArray();
            byte[] n = new[] { (byte)(ct >> 24 & 0x00ff), (byte)(ct >> 16 & 0x00ff), (byte)(ct >> 8 & 0x00ff), (byte)(ct & 0x00ff) };
            var t = SM3.SM3Array(z.Concat(n).ToArray());
            ct++;
            offset = 0;
            for (int i = 0, len = msg.Length; i < len; i++)
            {
                // t = Ha1 || Ha2 || Ha3 || Ha4
                if (offset == t.Length)
                {
                    t = SM3.SM3Array(z.Concat(n).ToArray()); ;
                    ct++;
                    offset = 0;
                }
                // c2 = msg ^ t
                msg[i] ^= (byte)(t[offset++] & 0xff);
            }
            // c3 = hash(x2 || msg || y2)
            byte[] tt = new byte[0];
            var checkC3 = SMUtils.arrayToHex(SM3.SM3Array(tt.Concat(x2).Concat(msg).Concat(y2).ToArray()));
            if (checkC3 == c3.ToLower())
            {
                return new SM2Res(msg);
            }
            else
            {
                return new SM2Res(Encoding.UTF8.GetBytes(""));
            }
        }




    }

}

// See https://aka.ms/new-console-template for more information









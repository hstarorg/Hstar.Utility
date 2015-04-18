using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Hstar.Utility.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hstar.Utility.Test.Cryptography
{
    /// <summary>
    /// Summary description for RSAEncryptionTest
    /// </summary>
    [TestClass]
    public class RSAEncryptionTest
    {
        private static string publicKey;
        private static string privateKey;

        [TestInitialize()]
        public void RSAEncryptionTestInitialize()
        {
            var keys = RSAEncryption.GenerateKeys();
            publicKey = keys[0];
            privateKey = keys[1];
        }
        /// <summary>
        /// 测试生成的公钥、私钥
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreNotEqual(publicKey, privateKey);
        }

        /// <summary>
        /// 测试公钥加密，私钥解密
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            var rsa = new RSAEncryption();
            var targetStr = rsa.EncryptWithPubKey("12345", publicKey);
            Assert.IsNotNull(targetStr);
            var targetStr2 = rsa.DecryptWithPriKey(targetStr, privateKey);
            Assert.AreEqual("12345", targetStr2);
        }

        /// <summary>
        /// 测试私钥加密，公钥解密
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            var rsa = new RSAEncryption();
            var targetStr = rsa.EncryptWithPriKey("12345", privateKey);
            Assert.IsNotNull(targetStr);
            var targetStr2 = rsa.DecryptWithPubKey(targetStr, publicKey);
            Assert.AreEqual("12345", targetStr2);
        }

        [TestMethod]
        public void TestMethod4()
        {
            string priKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQCf1a4LQyipBqeUCZ9kKsfasQzkEFCBmGsM21Sakb5BO0sY07GD
cproJHF2xNQrV0cM7+liE3pBUFsarui2WaHZhAibpLbl9z4FSfoN5hSg6sEgbB17
SvKe3ZN/75GoEsQiQtYW4gUJgzrBovVZ+TeTnN+NHHBqUqBKhNIgPFVapQIDAQAB
AoGAG0OMs5kaF3LuJN9bU+/ENXab908dHG4OXJwRG2ie5muhzLNXhU+IQu7sd9Dt
TBNQKFHIIpWl9fwp/iw1v90cMUQGj0zhSXHAz7Vak/ryQLTyeIIciL8MQWvnbAaN
lIoFq2wBl7SYs3n71B4MlvvTysaG0krsjiPh5LVgnBvzjGECQQDcAwe4XnF7SHWO
nfljrG29soKNiUhYKtDGcV9fvam9u50Ek882wvFmsJP+tk+1CXjMRSNlOi40bxKC
uaBa1JOtAkEAufq9FmZHfBFf3e6n57wLiAj5C1MeyHAtt6qdAF49OZJBGZh1pePn
jDGNezFvy7U5bMp7/updisLCFueS5eKB2QJAF84QIMe/OZqedZ7sI/e9LABLlerb
tAZ17nLH4gEQg6HwHFWt3vv6yKSkbrPlLe5nbpqweLxx0WSPOSvCiPFlRQJAPAfF
NQ+6jz+EdDxukgxOpJBQ4ujnjMc42ooFt3KzzHt66+ocP3m66bOs+VDRxy0t5gHN
2FCJ9Ro8T+xbrDxasQJAARHpcG6tE0F+lmUthtep1U8OrF+AQvqDhBq8MYK+/pF/
LRZkFHkqTsj89OyWDlSH3LeYkOWsr9mAFxsvHZ9BSA==
-----END RSA PRIVATE KEY-----";

            priKey = priKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "");

            var rsaProvider = DecodeRSAPrivateKey(priKey);
        }

        public static RSACryptoServiceProvider DecodeRSAPrivateKey(string priKey)
        {
            var privkey = Convert.FromBase64String(priKey);
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();        //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();       //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();	// data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}

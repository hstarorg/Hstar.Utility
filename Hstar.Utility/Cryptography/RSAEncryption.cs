using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Hstar.Utility.Cryptography
{
    /// <summary>
    /// RSA加密
    /// </summary>
    public class RSAEncryption
    {
        private readonly RSACryptoServiceProvider rsaProvider;

        /// <summary>
        /// RSA加密构造函数
        /// </summary>
        public RSAEncryption()
        {
            this.rsaProvider = new RSACryptoServiceProvider();
        }
        /// <summary>
        /// RSA生成公钥、私钥
        /// </summary>
        /// <returns>数组，keys[0]=公钥，keys[1]=私钥</returns>
        public static string[] GenerateKeys()
        {
            var rsa = new RSACryptoServiceProvider(1024);
            var keys = new string[2];
            keys[0] = rsa.ToXmlString(false);
            keys[1] = rsa.ToXmlString(true);
            return keys;
        }

        #region 标准加解密

        /// <summary>
        /// RSA加密字符串(公钥加密-对应私钥解密)
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="pubKey">公钥</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回Base64密文</returns>
        public string EncryptWithPubKey(string strSource,string pubKey,Encoding encoding=null)
        {
            this.rsaProvider.FromXmlString(pubKey);
            byte[] data = (encoding??Encoding.UTF8).GetBytes(strSource);
            var encryptedData = this.rsaProvider.Encrypt(data, false);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 解密字符串（私钥解密-对应公钥加密）
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="priKey">私钥</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回原文</returns>
        public string DecryptWithPriKey(string strSource,string priKey,Encoding encoding=null)
        {
            this.rsaProvider.FromXmlString(priKey);
            byte[] data = Convert.FromBase64String(strSource);
            var decryptedData = this.rsaProvider.Decrypt(data, false);
            return (encoding ?? Encoding.UTF8).GetString(decryptedData);
        }

        #endregion

        #region 私钥加密，公钥解密 参考：http://www.codeproject.com/Articles/38739/RSA-Private-Key-Encryption

        /// <summary>
        /// RSA加密字符串(私钥加密-对应公钥解密)
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="priKey">私钥</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回Base64密文</returns>
        public string EncryptWithPriKey(string strSource, string priKey,Encoding encoding=null)
        {
            this.rsaProvider.FromXmlString(priKey);
            byte[] data = (encoding ?? Encoding.UTF8).GetBytes(strSource);
            BigInteger numData = GetBig(AddPadding(data));

            RSAParameters rsaParams = this.rsaProvider.ExportParameters(true);
            BigInteger d = GetBig(rsaParams.D);
            BigInteger modulus = GetBig(rsaParams.Modulus);
            BigInteger encData = BigInteger.ModPow(numData, d, modulus);
            return Convert.ToBase64String(encData.ToByteArray());
        }

        /// <summary>
        /// RSA加密字符串(私钥加密-对应公钥解密)
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="pubKey">公钥</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回Base64密文</returns>
        public string DecryptWithPubKey(string strSource, string pubKey,Encoding encoding=null)
        {
            this.rsaProvider.FromXmlString(pubKey);
            byte[] data = Convert.FromBase64String(strSource);
            BigInteger numEncData = new BigInteger(data);

            RSAParameters rsaParams = this.rsaProvider.ExportParameters(false);
            BigInteger exponent = GetBig(rsaParams.Exponent);
            BigInteger modulus = GetBig(rsaParams.Modulus);

            BigInteger decData = BigInteger.ModPow(numEncData, exponent, modulus);

            byte[] data2 = decData.ToByteArray();
            byte[] result = new byte[data2.Length - 1];
            Array.Copy(data2, result, result.Length);
            result = RemovePadding(result);

            Array.Reverse(result);
            return (encoding ?? Encoding.UTF8).GetString(result);
        }

        // Add 4 byte random padding, first bit *Always On*
        private byte[] AddPadding(byte[] data)
        {
            var rnd = new Random();
            var paddings = new byte[4];
            rnd.NextBytes(paddings);
            paddings[0] = (byte)(paddings[0] | 128);

            var results = new byte[data.Length + 4];

            Array.Copy(paddings, results, 4);
            Array.Copy(data, 0, results, 4, data.Length);
            return results;
        }

        private byte[] RemovePadding(byte[] data)
        {
            var results = new byte[data.Length - 4];
            Array.Copy(data, results, results.Length);
            return results;
        }

        private BigInteger GetBig(byte[] data)
        {
            var inArr = (byte[])data.Clone();
            Array.Reverse(inArr);  // Reverse the byte order
            var final = new byte[inArr.Length + 1];  // Add an empty byte at the end, to simulate unsigned BigInteger (no negatives!)
            Array.Copy(inArr, final, inArr.Length);
            return new BigInteger(final);
        }
        #endregion
    }
}
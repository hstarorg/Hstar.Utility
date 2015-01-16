using System;
using System.Security.Cryptography;
using System.Text;

namespace Hstar.Utility.Cryptography
{
    /// <summary>
    /// DSA加解密
    /// </summary>
    public class DSAEncryption
    {
        private readonly DSACryptoServiceProvider dsaProvider;

        /// <summary>
        /// DSA加密构造函数
        /// </summary>
        public DSAEncryption()
        {
            this.dsaProvider = new DSACryptoServiceProvider();
        }
        /// <summary>
        /// DSA生成公钥、私钥
        /// </summary>
        /// <returns>数组，keys[0]=公钥，keys[1]=私钥</returns>
        public static string[] GenerateKeys()
        {
            var rsa = new DSACryptoServiceProvider(1024);
            var keys = new string[2];
            keys[0] = rsa.ToXmlString(false);
            keys[1] = rsa.ToXmlString(true);
            return keys;
        }

        #region 标准加解密

        /// <summary>
        /// DSA加密字符串（私钥加密）
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="priKey">私钥</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回Base64密文</returns>
        public string EncryptWithPriKey(string strSource,string priKey,Encoding encoding=null)
        {
            this.dsaProvider.FromXmlString(priKey);
            byte[] data = (encoding??Encoding.UTF8).GetBytes(strSource);
            var encryptedData = this.dsaProvider.SignData(data);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 验证字符串
        /// </summary>
        /// <param name="strSource">要验证的字符串</param>
        /// <param name="ciphertext">密文</param>
        /// <param name="pubKey">公钥</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>是否是原文加密</returns>
        public bool VerifyWithPubKey(string strSource,string ciphertext,string pubKey,Encoding encoding=null)
        {
            this.dsaProvider.FromXmlString(pubKey);
            var strBytes = (encoding ?? Encoding.UTF8).GetBytes(strSource);
            byte[] sign = Convert.FromBase64String(ciphertext);
            return this.dsaProvider.VerifyData(strBytes, sign);
        }

        #endregion
    }
}
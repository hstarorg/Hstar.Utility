using System.Security.Cryptography;
using System.Text;

namespace Hstar.Utility.Cryptography
{
    /// <summary>
    /// Hash加密
    /// </summary>
    public class HashEncryption
    {
        private readonly HashAlgorithm ha;

        /// <summary>
        /// Hash加密构造函数
        /// </summary>
        /// <param name="haType">Hash算法类别</param>
        public HashEncryption(HashAlgorithmType haType = HashAlgorithmType.MD5)
        {
            switch (haType)
            {
                case HashAlgorithmType.MD5:
                    ha = new MD5CryptoServiceProvider();
                    break;
                case HashAlgorithmType.SHA1:
                    ha = new SHA1CryptoServiceProvider();
                    break;
                case HashAlgorithmType.SHA256:
                    ha = new SHA256CryptoServiceProvider();
                    break;
                case HashAlgorithmType.SHA384:
                    ha = new SHA384CryptoServiceProvider();
                    break;
                case HashAlgorithmType.SHA512:
                    ha = new SHA512CryptoServiceProvider();
                    break;
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回Base64密文</returns>
        public string Encrypt(string strSource, Encoding encoding = null)
        {
            byte[] data = ha.ComputeHash((encoding??Encoding.UTF8).GetBytes(strSource));
            var builder = new StringBuilder();
            foreach (var bt in data)
            {
                builder.Append(bt.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
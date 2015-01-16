using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hstar.Utility.Cryptography
{
    /// <summary>
    /// 对称加密
    /// </summary>
    public class SymmetricEncryption
    {
        private readonly string key;
        private readonly string keyVector;
        private SymmetricAlgorithm sa;

        /// <summary>
        /// 对称加解密构造函数
        /// </summary>
        /// <param name="key">加密/解密字符串（至少8位）</param>
        public SymmetricEncryption(string key)
            : this(key, key)
        {
        }

        /// <summary>
        /// 对称加解密构造函数
        /// </summary>
        /// <param name="key">密钥（至少8位）</param>
        /// <param name="saType">对称加密类型</param>
        public SymmetricEncryption(string key, SymmetricAlgorithmType saType):this(key,key,saType)
        {
            
        }

        /// <summary>
        /// 对称加解密构造函数
        /// </summary>
        /// <param name="key">密钥（至少8位）</param>
        /// <param name="keyVector">密钥向量（至少8位）</param>
        public SymmetricEncryption(string key, string keyVector) : this(key, keyVector, SymmetricAlgorithmType.AES)
        {
        }

        /// <summary>
        /// 对称加解密构造函数
        /// </summary>
        /// <param name="key">密钥（至少8位）</param>
        /// <param name="keyVector">密钥向量（至少8位）</param>
        /// <param name="saType">对称加密类型</param>
        public SymmetricEncryption(string key, string keyVector, SymmetricAlgorithmType saType)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(keyVector))
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("密钥或者密钥向量不能为空！");
            }
            if (key.Length < 8 || keyVector.Length < 8)
            {
                throw new ArgumentException("密钥或者密钥向量长度至少8位！");
            }

            this.key = key;
            this.keyVector = keyVector;
            this.InitCryptoServiceProvider(saType);
        }

        /// <summary>
        /// 构建对称算法对象，并设置属性
        /// </summary>
        /// <param name="saType">对称算法类型</param>
        private void InitCryptoServiceProvider(SymmetricAlgorithmType saType)
        {
            sa = this.GetCryptoServiceProvider(saType);

            // Rfc2898DeriveBytes - 通过使用基于 HMACSHA1 的伪随机数生成器，实现基于密码的密钥派生功能 (PBKDF2 - 一种基于密码的密钥派生函数)
            // 通过 密码 和 salt 派生密钥
            byte[] keyVectorBytes = Encoding.UTF8.GetBytes(keyVector);
            var rfc = new Rfc2898DeriveBytes(key, keyVectorBytes);

            /*
             * AesManaged.BlockSize - 加密操作的块大小（单位：bit）
             * AesManaged.LegalBlockSizes - 对称算法支持的块大小（单位：bit）
             * AesManaged.KeySize - 对称算法的密钥大小（单位：bit）
             * AesManaged.LegalKeySizes - 对称算法支持的密钥大小（单位：bit）
             * AesManaged.Key - 对称算法的密钥
             * AesManaged.IV - 对称算法的密钥大小
             * Rfc2898DeriveBytes.GetBytes(int 需要生成的伪随机密钥字节数) - 生成密钥
             */
            sa.BlockSize = sa.LegalBlockSizes[0].MaxSize;
            sa.KeySize = sa.LegalKeySizes[0].MaxSize;
            sa.Key = rfc.GetBytes(sa.KeySize/8);
            sa.IV = rfc.GetBytes(sa.BlockSize/8);
        }
        /// <summary>
        /// 根据对称加密类型获取需要的对称加密对象实例
        /// </summary>
        /// <param name="saType">对称加密类型</param>
        /// <returns></returns>
        private SymmetricAlgorithm GetCryptoServiceProvider(SymmetricAlgorithmType saType)
        {
            SymmetricAlgorithm tmpSa = null;
            switch (saType)
            {
                case SymmetricAlgorithmType.AES:
                    tmpSa = new AesCryptoServiceProvider();
                    break;
                case SymmetricAlgorithmType.DES:
                    tmpSa = new DESCryptoServiceProvider();
                    break;
                case SymmetricAlgorithmType.RC2:
                    tmpSa = new RC2CryptoServiceProvider();
                    break;
                case SymmetricAlgorithmType.TripleDES:
                    tmpSa = new TripleDESCryptoServiceProvider();
                    break;
            }
            return tmpSa;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strSource">要加密的字符串</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回Base64密文</returns>
        public string Encrypt(string strSource, Encoding encoding =null)
        {
            byte[] data = (encoding??Encoding.UTF8).GetBytes(strSource);
            // 加密后的输出流
            var encryptStream = new MemoryStream();
            // 将加密后的目标流（encryptStream）与加密转换（encryptTransform）相连接
            var encryptor = new CryptoStream(encryptStream, this.sa.CreateEncryptor(), CryptoStreamMode.Write);
            // 将一个字节序列写入当前 CryptoStream （完成加密的过程）
            encryptor.Write(data, 0, data.Length);
            encryptor.Close();
            return Convert.ToBase64String(encryptStream.ToArray());
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strSource">要解密的字符串（Base64字符串）</param>
        /// <param name="encoding">编码（注：如果要加密的字符串有中文，请不要使用不支持中文的编码，如ASCII）</param>
        /// <returns>返回原文</returns>
        public string Decrypt(string strSource,Encoding encoding=null)
        {
            byte[] encryptBytes = Convert.FromBase64String(strSource);
            // 解密后的输出流
            var decryptStream = new MemoryStream();
            // 将解密后的目标流（decryptStream）与解密转换（decryptTransform）相连接
            var decryptor = new CryptoStream(decryptStream, this.sa.CreateDecryptor(), CryptoStreamMode.Write);
            // 将一个字节序列写入当前 CryptoStream （完成解密的过程）
            decryptor.Write(encryptBytes, 0, encryptBytes.Length);
            decryptor.Close();
            // 将解密后所得到的流转换为字符串
            return (encoding??Encoding.UTF8).GetString(decryptStream.ToArray());
        }
    }
}
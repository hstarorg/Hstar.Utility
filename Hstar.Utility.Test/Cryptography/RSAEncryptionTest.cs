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
            var targetStr=rsa.EncryptWithPubKey("12345",publicKey);
            Assert.IsNotNull(targetStr);
            var targetStr2 = rsa.DecryptWithPriKey(targetStr, privateKey);
            Assert.AreEqual("12345",targetStr2);
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
    }
}

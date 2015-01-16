using System.Text;
using Hstar.Utility.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hstar.Utility.Test.Cryptography
{
    [TestClass]
    public class HashEncryptionTest
    {
        /// <summary>
        /// 测试MD5纯数字
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var he=new HashEncryption();
            Assert.AreEqual("e807f1fcf82d132f9bb018ca6738a19f", he.Encrypt("1234567890"));
        }

        /// <summary>
        /// 测试MD5纯中文
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            var he = new HashEncryption();
            Assert.AreEqual("bc49baed53d7f0e7ca9642bd12bfa935", he.Encrypt("爱我中华"));
        }

        /// <summary>
        /// 测试MD5混合字符
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            var he = new HashEncryption();
            Assert.AreEqual("92556af5f0ea9feeefdf40f3c2665acd", he.Encrypt("爱我中华012345abcde~！@#"));
        }

        /// <summary>
        /// 测试MD5混合字符
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            var he = new HashEncryption();
            var targetStr = he.Encrypt("爱我中华012345abcde~！@#", Encoding.Unicode);
            Assert.AreNotEqual("92556af5f0ea9feeefdf40f3c2665acd",targetStr);
            Assert.AreEqual(32,targetStr.Length);
        }

        /// <summary>
        /// 测试SHA1混合字符
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            var he = new HashEncryption(HashAlgorithmType.SHA1);
            Assert.AreEqual("287fecdb4c35e861098b2c1a7e56b6aec76193a5", he.Encrypt("爱我中华012345abcde~！@#"));
        }
        /// <summary>
        /// 测试SHA256混合字符
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            var he = new HashEncryption(HashAlgorithmType.SHA256);
            Assert.AreEqual("190816046f4a031e30080ecab24dd813508e68701121cc9ba83d58e7b15622b9", he.Encrypt("爱我中华012345abcde~！@#"));
        }

        /// <summary>
        /// 测试SHA384混合字符
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            var he = new HashEncryption(HashAlgorithmType.SHA384);
            Assert.AreEqual("fab604de55af8cd87d2e32f66a6ead7d103bd11e824b46a9ab29116d9057794a4e4672a39a56f6b94107270656406193",
                he.Encrypt("爱我中华012345abcde~！@#"));
        }

        /// <summary>
        /// 测试SHA512混合字符
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            var he = new HashEncryption(HashAlgorithmType.SHA512);
            Assert.AreEqual("ad4656caa3d4328096a14cf58730be80de238e7b029622597dd3836087289dac7bb7b395ee766787b02c2346d4c72aa4cf6aa6e4fc48b4cf57f39019783f0495",
                he.Encrypt("爱我中华012345abcde~！@#"));
        } 
    }
}

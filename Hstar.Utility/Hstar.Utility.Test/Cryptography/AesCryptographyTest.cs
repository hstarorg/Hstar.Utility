using System;
using Hstar.Utility.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hstar.Utility.Test.Cryptography
{
    [TestClass]
    public class AesCryptographyTest
    {
        /// <summary>
        ///  测试AES加解密,key和keyVecter一致
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            const string sourceStr = "humin123";
            var se = new SymmetricEncryption("jay123456");
            var targetStr=se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }
        /// <summary>
        /// 测试AES加解密,key和keyVecter不一致
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            var se = new SymmetricEncryption("12345678", "87654321");
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }
        /// <summary>
        /// 测试RC2加解密
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            var se = new SymmetricEncryption("12345678", "87654321", SymmetricAlgorithmType.RC2);
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }
        /// <summary>
        /// 测试DES加解密
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            var se = new SymmetricEncryption("12345678", "87654321", SymmetricAlgorithmType.DES);
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }
        /// <summary>
        /// 测试TripleDES加解密
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            var se = new SymmetricEncryption("12345678", "87654321", SymmetricAlgorithmType.TripleDES);
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }
        /// <summary>
        /// 测试默认加密类型=AES
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            var se = new SymmetricEncryption("12345678", "87654321");
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            var se2 = new SymmetricEncryption("12345678", "87654321", SymmetricAlgorithmType.AES);
            var targetStr2 = se2.Encrypt(sourceStr);
            Assert.AreEqual(targetStr, targetStr2);
        }

        /// <summary>
        /// 测试Key长度不够异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMethod7()
        {
            var se = new SymmetricEncryption("12345");
        }

        /// <summary>
        /// 测试Key长度不够异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod8()
        {
            var se = new SymmetricEncryption("");
        }

        /// <summary>
        /// 测试0作为枚举默认值
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            var se = new SymmetricEncryption("12345678",0);
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            var se2 = new SymmetricEncryption("12345678", SymmetricAlgorithmType.AES);
            var targetStr2 = se2.Encrypt(sourceStr);
            Assert.AreEqual(targetStr, targetStr2);
        }

        /// <summary>
        /// 测试中文作为key
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            var se = new SymmetricEncryption("爱我中华爱我中华", 0);
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }

        /// <summary>
        /// 测试混合key
        /// </summary>
        [TestMethod]
        public void TestMethod11()
        {
            var se = new SymmetricEncryption("爱我中华爱我中华1574551￥#@54633", 0);
            const string sourceStr = "humin123";
            var targetStr = se.Encrypt(sourceStr);
            Assert.AreEqual(sourceStr, se.Decrypt(targetStr));
        }
    }
}

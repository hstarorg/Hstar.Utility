using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Hstar.Utility.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hstar.Utility.Test.Cryptography
{
    /// <summary>
    /// Summary description for DSAEncryptionTest
    /// </summary>
    [TestClass]
    public class DSAEncryptionTest
    {

        private static string publicKey;
        private static string privateKey;

        [TestInitialize()]
        public void DSAEncryptionTestInitialize()
        {
            var keys = DSAEncryption.GenerateKeys();
            publicKey = keys[0];
            privateKey = keys[1];
        }
        [TestMethod]
        public void TestMethod1()
        {
            const string str = "123456789";
            var dsa=new DSAEncryption();
            var targetStr = dsa.EncryptWithPriKey(str, privateKey);
            Assert.IsTrue(dsa.VerifyWithPubKey(str,targetStr,publicKey));
        }
    }
}

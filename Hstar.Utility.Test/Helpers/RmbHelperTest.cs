using System.Runtime.InteropServices;
using Hstar.Utility.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hstar.Utility.Test.Helpers
{
    [TestClass]
    public class RmbHelperTest
    {
        
        private RmbHelper rmb = new RmbHelper();

        [TestMethod]
        public void TestMethod1()
        {
            var str = rmb.ConvertToAmout(200);
            Assert.AreEqual("贰佰元整", str);

            str = rmb.ConvertToAmout((decimal)213.56);
            Assert.AreEqual("贰佰壹拾叁元伍角陆分", str);

            str = rmb.ConvertToAmout((decimal)0);
            Assert.AreEqual("零元整", str);

            str = rmb.ConvertToAmout((decimal)0.00);
            Assert.AreEqual("零元整", str);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //零壹贰叁肆伍陆柒捌玖 -- 亿万仟佰拾
            Assert.AreEqual("壹仟零贰拾元陆角柒分", rmb.ConvertToAmout(1020.666));
        }
    }
}

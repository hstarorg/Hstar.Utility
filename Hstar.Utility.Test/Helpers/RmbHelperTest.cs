using Hstar.Utility.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hstar.Utility.Test.Helpers
{
    [TestClass]
    public class RmbHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var rmbHelper=new RmbHelper();
            var str=rmbHelper.ConvertToAmout(200);
            Assert.AreEqual("贰佰元整", str);

            str = rmbHelper.ConvertToAmout((decimal)213.56);
            Assert.AreEqual("贰佰壹拾叁元伍角陆分", str);

            str = rmbHelper.ConvertToAmout((decimal)0);
            Assert.AreEqual("零元整", str);

            str = rmbHelper.ConvertToAmout((decimal)0.00);
            Assert.AreEqual("零元整", str);
        }
    }
}

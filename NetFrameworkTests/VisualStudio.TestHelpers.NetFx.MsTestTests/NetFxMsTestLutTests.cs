using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hallsoft.TestHelpers.NetFx.MsTestTests
{
    [TestClass]
    public class NetFxMsTestLutTests
    {
        readonly VsTestHelper _testHelper = new VsTestHelper();

        public NetFxMsTestLutTests()
        {
            _testHelper.Config.LogWriter = new DefaultTestLogWriter();
        }

        [TestMethod]
        public void CanOpenFile()
        {
            const string ExpectedContents = "Hello .NET Framework MSTest";
            string contents;
            using (StreamReader reader = _testHelper.OpenFile("test.txt"))
            {
                contents = reader.ReadToEnd();
            }

            Assert.AreEqual(ExpectedContents, contents);
        }


        [TestMethod]
        public void DetectMsTestFramework()
        {
            Assert.AreEqual(TestFrameworks.MsTest, _testHelper.TestFramework);
        }
    }
}

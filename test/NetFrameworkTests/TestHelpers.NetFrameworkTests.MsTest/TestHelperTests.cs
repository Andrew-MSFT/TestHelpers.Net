using System;
using System.IO;
using Hallsoft.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestHelpers.Net.NetFx.MsTest
{
    [TestClass]
    public class NetFxMsTestLutTests
    {
        readonly TestFileHelper _testHelper = new TestFileHelper();

        public NetFxMsTestLutTests()
        {
            _testHelper.Configuration.LogWriter = new DefaultTestLogWriter();
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

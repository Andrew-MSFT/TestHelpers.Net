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
            string projectDirectory = _testHelper.GetTestProjectDirectory();
            string fullFile = Path.Combine(projectDirectory, "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }

        [TestMethod]
        public void DetectMsTestFramework()
        {
            Assert.AreEqual(TestFrameworks.MsTest, _testHelper.TestFramework);
        }
    }
}

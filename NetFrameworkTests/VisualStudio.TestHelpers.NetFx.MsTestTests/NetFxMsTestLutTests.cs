using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualStudio.TestHelpers.NetFx.MsTestTests
{
    [TestClass]
    public class NetFxMsTestLutTests
    {
        readonly LiveUnitTestingHelper _testHelper = new LiveUnitTestingHelper();

        public NetFxMsTestLutTests()
        {
            _testHelper.LogWriter = new ConsoleTestLogWriter();
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

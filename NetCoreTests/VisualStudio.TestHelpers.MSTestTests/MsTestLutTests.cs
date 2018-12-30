using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Hallsoft.TestHelpers.MSTestTests
{
    [TestClass]
    public class MsTestLutTests
    {
        readonly VsTestHelper _testHelper = new VsTestHelper();

        public MsTestLutTests()
        {
            _testHelper.Config.LogWriter = new ConsoleTestLogWriter();
        }

        [TestMethod]
        public void CanOpenFile()
        {
            string projectDirectory = _testHelper.GetTestProjectDirectory();
            string fullFile = Path.Combine(projectDirectory, "data", "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }

        [TestMethod]
        public void DetectMsTestFramework()
        {
            Assert.AreEqual(TestFrameworks.MsTest, _testHelper.TestFramework);
        }
    }
}

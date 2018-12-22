using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace VisualStudio.TestHelpers.MSTestTests
{
    [TestClass]
    public class MsTestLutTests
    {
        readonly LiveUnitTestingHelper _testHelper = new LiveUnitTestingHelper();

        public MsTestLutTests()
        {
            _testHelper.LogWriter = new ConsoleTestLogWriter();
        }

        [TestMethod]
        public void RunningUnderLut()
        {
            bool runningUnderLut = _testHelper.IsRunningUnderLut();
            bool pathBased = _testHelper.IsLutBasedOnPath();

            Assert.AreEqual(pathBased, runningUnderLut);
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

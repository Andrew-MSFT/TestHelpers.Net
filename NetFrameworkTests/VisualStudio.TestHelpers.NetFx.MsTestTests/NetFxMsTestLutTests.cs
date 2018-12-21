using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisualStudio.TestHelpers.NetFx.MsTestTests
{
    [TestClass]
    public class NetFxMsTestLutTests
    {
        readonly LiveUnitTestingHelper _testingHelper = new LiveUnitTestingHelper();

        [TestMethod]
        public void RunningUnderLut()
        {
            bool runningUnderLut = _testingHelper.IsRunningUnderLut();
            bool pathBased = _testingHelper.IsLutBasedOnPath();

            Assert.AreEqual(pathBased, runningUnderLut);
        }

        [TestMethod]
        public void CanOpenFile()
        {
            string projectDirectory = _testingHelper.GetTestProjectDirectory();
            string fullFile = Path.Combine(projectDirectory, "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }
    }
}

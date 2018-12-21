using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace VisualStudio.TestHelpers.MSTestTests
{
    [TestClass]
    public class MsTestLutTests
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
            string fullFile = Path.Combine(projectDirectory, "data", "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }

        [TestMethod]
        public void DetectMsTestFramework()
        {
            Assert.AreEqual(TestFrameworks.MsTest, _testingHelper.TestFramework);
        }
    }
}

using NUnit.Framework;
using System.IO;
using VisualStudio.TestHelpers;

namespace Tests
{
    public class NUnitLutTests
    {
        readonly LiveUnitTestingHelper _testHelper = new LiveUnitTestingHelper();

        [SetUp]
        public void Setup()
        {
            _testHelper.LogWriter = new ConsoleTestLogWriter();
        }

        [Test]
        public void RunningUnderLut()
        {
            bool runningUnderLut = _testHelper.IsRunningUnderLut();
            bool pathBased = _testHelper.IsLutBasedOnPath();

            Assert.AreEqual(runningUnderLut, pathBased);
        }

        [Test]
        public void CanOpenFile()
        {
            string projectDirectory = _testHelper.GetTestProjectDirectory();
            string fullFile = Path.Combine(projectDirectory, "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }

        [Test]
        public void DetectNUnitFramework()
        {
            Assert.AreEqual(TestFrameworks.NUnit, _testHelper.TestFramework);
        }
    }
}

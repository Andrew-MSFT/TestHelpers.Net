using NUnit.Framework;
using System;
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
        }

        [Test]
        public void RunningUnderLut()
        {
            bool runningUnderLut = _testHelper.IsRunningUnderLut();
            bool pathBased = _testHelper.IsLutBasedOnPath();

            foreach(string message in _testHelper.DiagnosticLog)
            {
                Console.WriteLine(message);
            }

            Assert.AreEqual(runningUnderLut, pathBased);
        }

        [Test]
        public void CanOpenFile()
        {
            string projectDirectory = _testHelper.GetTestProjectDirectory();
            string fullFile = Path.Combine(projectDirectory, "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }
    }
}

using Hallsoft.TestHelpers;
using NUnit.Framework;
using System.IO;

namespace Tests
{
    public class NUnitLutTests
    {
        readonly TestFileHelper _testHelper = new TestFileHelper();

        [SetUp]
        public void Setup()
        {
            _testHelper.Configuration.LogWriter = new DefaultTestLogWriter();
        }

        [Test]
        public void CanOpenFile()
        {
            string projectDirectory = _testHelper.ProjectDirectory.FullName;
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

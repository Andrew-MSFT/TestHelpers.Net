using NUnit.Framework;
using System.IO;
using Hallsoft.TestHelpers;

namespace Tests
{
    public class NUnitLutTests
    {
        readonly VsTestHelper _testHelper = new VsTestHelper();

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

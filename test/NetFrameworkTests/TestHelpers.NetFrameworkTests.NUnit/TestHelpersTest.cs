using Hallsoft.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelpers.NetFrameworkTests.NUnit
{
    [TestFixture]
    public class TestHelpersTest
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

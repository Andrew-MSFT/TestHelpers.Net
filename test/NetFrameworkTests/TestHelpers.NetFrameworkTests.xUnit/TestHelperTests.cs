using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hallsoft.TestHelpers;
using Xunit;

namespace TestHelpers.NetFrameworkTests.Xunit
{
    public class TestHelperTests
    {
        readonly TestFileHelper _testHelper = new TestFileHelper();

        [Fact]
        public void CanOpenFile()
        {
            const string Expected = "Hello xUnit .NET Framework!";
            string projectDirectory = _testHelper.ProjectDirectory.FullName;
            string contents;
            using (StreamReader sr = _testHelper.OpenFile(@"Data\data.txt"))
            {
                contents = sr.ReadToEnd();
            }

            Assert.Equal(Expected, contents);
        }

        [Fact]
        public void DetectNUnitFramework()
        {
            Assert.Equal(TestFrameworks.xUnit, _testHelper.TestFramework);
        }
    }
}

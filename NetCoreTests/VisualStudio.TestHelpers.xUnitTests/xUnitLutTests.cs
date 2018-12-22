using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace VisualStudio.TestHelpers.Tests
{
    public class LiveUnitTestingHelperTests
    {
        private readonly LiveUnitTestingHelper _testHelper = new LiveUnitTestingHelper();

        private ITestOutputHelper _output;

        public LiveUnitTestingHelperTests(ITestOutputHelper output)
        {
            _testHelper.LogWriter = new xUnitLogWriter(output);
            _output = output;
        }

        [Fact]
        public void BadTestFolder()
        {
            LiveUnitTestingHelper helper = new LiveUnitTestingHelper("BadProjectFolder");

            Assert.Throws<DirectoryNotFoundException>(() => helper.GetTestProjectDirectory());
        }

        [Fact]
        public void RunningUnderLut()
        {
            bool runningUnderLut = _testHelper.IsRunningUnderLut();
            bool pathBased = _testHelper.IsLutBasedOnPath();

            Assert.Equal(pathBased, runningUnderLut);
        }

        [Theory]
        [InlineData("VisualStudio.TestHelpers.xUnitTests")]
        public void ProjectNameFromAssembly(string projectName)
        {
            string detectedName = _testHelper.GetTestProjectNameFromCallingAssembly();

            Assert.Equal(projectName, detectedName);
        }

        [Theory]
        [InlineData("data", "test.txt")]
        public void CanOpenFile(string folder, string fileName)
        {
            string projectDirectory = _testHelper.GetTestProjectDirectory();
            string fullFile = Path.Combine(projectDirectory, folder, fileName);

            Assert.True(File.Exists(fullFile));
        }

        [Fact]
        public void DetectxUnitFramework()
        {
            Assert.Equal(TestFrameworks.xUnit, _testHelper.TestFramework);
        }

        [Fact]
        public void OpenTestFile()
        {
            const string Expected = "Hello World!";
            string contents;
            using (StreamReader reader = _testHelper.OpenFile("data", "test.txt"))
            {
                contents = reader.ReadToEnd();
            }

            Assert.Equal(Expected, contents);
        }
    }
}

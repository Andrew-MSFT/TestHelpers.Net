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
        private readonly ITestOutputHelper _output;
        private readonly LiveUnitTestingHelper _testHelper = new LiveUnitTestingHelper();

        public LiveUnitTestingHelperTests(ITestOutputHelper output)
        {
            _output = output;
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
        public void ProjectName(string projectName)
        {

            string detectedName = _testHelper.GetTestProjectNameFromCallingAssembly();

            Assert.Equal(projectName, detectedName);
        }

        [Theory]
        [InlineData("data", "test.txt")]
        public void CanOpenFile(string folder, string fileName)
        {
            try
            {
                string projectDirectory = _testHelper.GetTestProjectDirectory();
                string fullFile = Path.Combine(projectDirectory, folder, fileName);

                _output.WriteLine($"Detected path: {projectDirectory}");

                Assert.True(File.Exists(fullFile));
            }
            catch (DirectoryNotFoundException)
            {
                PrintDiagLog(_testHelper);
                throw;
            }
        }

        [Fact]
        public void DetectxUnitFramework()
        {
            Assert.Equal(TestFrameworks.xUnit, _testHelper.TestFramework);
        }

        internal void PrintDiagLog(LiveUnitTestingHelper helper)
        {
            foreach (string message in helper.DiagnosticLog)
            {
                _output.WriteLine(message);
            }
        }


    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Hallsoft.TestHelpers.Tests
{
    public class LiveUnitTestingHelperTests
    {
        private readonly VsTestHelper _testHelper = new VsTestHelper();
        private readonly xUnitLogWriter _xUnitLogWriter;

        public LiveUnitTestingHelperTests(ITestOutputHelper output)
        {
            _xUnitLogWriter = new xUnitLogWriter(output);
            _testHelper.Configuration.LogWriter = _xUnitLogWriter;
        }

        [Fact]
        public void BadTestFolder()
        {
            string startingPath = Path.Combine(_testHelper.ProjectDirectory.FullName, @"..\..\.vs\");

            VsTestHelperConfiguration config = new VsTestHelperConfiguration
            {
                CurrentProjectFolderName = "BadProjectFolder",
                IsLut = true,
                MockBinaryRootPath = startingPath
            };

            

            Assert.Throws<DirectoryNotFoundException>(() => new VsTestHelper(config));
        }

        [Theory]
        [InlineData("VisualStudio.TestHelpers.xUnitTests")]
        public void ProjectNameFromAssembly(string projectName)
        {
            string detectedName = VsTestHelper.GetTestProjectNameFromCallingAssembly(_xUnitLogWriter);

            Assert.Equal(projectName, detectedName);
        }

        [Fact]
        public void LutSearchDirectoriesStaringWithPeriod()
        {
            VsTestHelperConfiguration config = new VsTestHelperConfiguration
            {
                SearchDirectoriesStartingWithPeriod = true,
                LogWriter = _xUnitLogWriter,
                IsLut = true
            };

            VsTestHelper helper = new VsTestHelper(config);

            string startingPath = Path.Combine(_testHelper.ProjectDirectory.FullName, @"..\..\");
            helper.FindProjectDirectory(startingPath, helper.ProjectDirectory.Name, out string projectFolder, config.TestDirectorySearchDepth, config.SearchDirectoriesStartingWithPeriod);

            string expected = _testHelper.ProjectDirectory.FullName;
            Assert.Equal(expected.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 
                projectFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        [Theory]
        [InlineData("VisualStudio.TestHelpers.xUnitTests")]
        public void ProjectNameFromAssemblyNoLogger(string projectName)
        {
            string detectedName = VsTestHelper.GetTestProjectNameFromCallingAssembly();

            Assert.Equal(projectName, detectedName);
        }

        [Theory]
        [InlineData("data", "test.txt")]
        public void CanOpenFile(string folder, string fileName)
        {
            string projectDirectory = _testHelper.ProjectDirectory.FullName;
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
            using (StreamReader reader = _testHelper.OpenFile("test.txt", "data"))
            {
                contents = reader.ReadToEnd();
            }

            Assert.Equal(Expected, contents);
        }
    }
}

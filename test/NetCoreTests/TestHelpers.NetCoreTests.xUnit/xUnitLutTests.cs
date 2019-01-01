using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace TestHelpers.Net.Tests
{
    public class LiveUnitTestingHelperTests
    {
        private readonly TestHelper _testHelper = new TestHelper();
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

            TestHelperConfiguration config = new TestHelperConfiguration
            {
                CurrentProjectFolderName = "BadProjectFolder",
                IsLut = true,
                MockBinaryRootPath = startingPath
            };

            

            Assert.Throws<DirectoryNotFoundException>(() => new TestHelper(config));
        }

        [Theory]
        [InlineData("TestHelpers.NetCoreTests.xUnit")]
        public void ProjectNameFromAssembly(string projectName)
        {
            string detectedName = TestHelper.GetTestProjectNameFromCallingAssembly(_xUnitLogWriter);

            Assert.Equal(projectName, detectedName);
        }

        [Fact]
        public void LutSearchDirectoriesStaringWithPeriod()
        {
            TestHelperConfiguration config = new TestHelperConfiguration
            {
                SearchDirectoriesStartingWithPeriod = true,
                LogWriter = _xUnitLogWriter,
                IsLut = true
            };

            TestHelper helper = new TestHelper(config);

            string startingPath = Path.Combine(_testHelper.ProjectDirectory.FullName, @"..\..\");
            helper.FindProjectDirectory(startingPath, helper.ProjectDirectory.Name, out string projectFolder, config.TestDirectorySearchDepth, config.SearchDirectoriesStartingWithPeriod);

            string expected = _testHelper.ProjectDirectory.FullName;
            Assert.Equal(expected.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 
                projectFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        [Theory]
        [InlineData("TestHelpers.NetCoreTests.xUnit")]
        public void ProjectNameFromAssemblyNoLogger(string projectName)
        {
            string detectedName = TestHelper.GetTestProjectNameFromCallingAssembly();

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
            using (StreamReader reader = _testHelper.OpenFile("data", "test.txt"))
            {
                contents = reader.ReadToEnd();
            }

            Assert.Equal(Expected, contents);
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Hallsoft.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace TestHelpers.Net.Tests
{
    public class LiveUnitTestingHelperTests
    {
        private readonly TestFileHelper _testHelper;
        private readonly XunitLogWriter _xUnitLogWriter;

        public LiveUnitTestingHelperTests(ITestOutputHelper output)
        {
            _xUnitLogWriter = new XunitLogWriter(output);
            _testHelper = new TestFileHelper(new TestFileHelperConfiguration { LogWriter = _xUnitLogWriter });
        }

        [Fact]
        public void BadTestFolder()
        {
            string startingPath = Path.Combine(_testHelper.ProjectDirectoryFullName, @"..\..\.vs\");

            TestFileHelperConfiguration config = new TestFileHelperConfiguration
            {
                CurrentProjectFolderName = "BadProjectFolder",
                IsLut = true,
                MockBinaryRootPath = startingPath
            };

            

            Assert.Throws<DirectoryNotFoundException>(() => new TestFileHelper(config));
        }

        [Theory]
        [InlineData("TestHelpers.NetCoreTests.xUnit")]
        public void ProjectNameFromAssembly(string projectName)
        {
            string detectedName = _testHelper.GetTestProjectNameFromCallingAssembly(_xUnitLogWriter);

            Assert.Equal(projectName, detectedName);
        }

        [Fact]
        public void LutSearchDirectoriesStaringWithPeriod()
        {
            TestFileHelperConfiguration config = new TestFileHelperConfiguration
            {
                SearchDirectoriesStartingWithPeriod = true,
                LogWriter = _xUnitLogWriter,
                IsLut = true
            };

            TestFileHelper helper = new TestFileHelper(config);

            string startingPath = Path.Combine(_testHelper.ProjectDirectory.FullName, @"..\..\");
            helper.FindProjectDirectory(startingPath, helper.ProjectDirectory.Name, out string projectFolder, config.TestDirectorySearchDepth);

            string expected = _testHelper.ProjectDirectory.FullName;
            Assert.Equal(expected.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 
                projectFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        [Fact]
        public void LutSearchBinAndObjDirectories()
        {
            LogRecorder logRecorder = new LogRecorder(_xUnitLogWriter);

            TestFileHelperConfiguration config = new TestFileHelperConfiguration
            {
                SearchBinDirectories = true,
                SearchObjDirectories = true,
                LogWriter = logRecorder,
                IsLut = true
            };

            TestFileHelper helper = new TestFileHelper(config);

            string startingPath = Path.Combine(_testHelper.ProjectDirectory.FullName, @"..\..\");
            helper.FindProjectDirectory(startingPath, helper.ProjectDirectory.Name, out string projectFolder, config.TestDirectorySearchDepth);

            bool binSearch = false;
            bool objSearched = false;

            foreach(string message in logRecorder.Messages)
            {
                if (message.TrimStart().StartsWith("Search for project folder"))
                {
                    if (message.Contains(@"\bin"))
                    {
                        binSearch = true;
                    }
                    else if (message.Contains(@"\obj"))
                    {
                        objSearched = true;
                    }
                }
            }

            string expected = _testHelper.ProjectDirectory.FullName;
            Assert.Equal(expected.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 
                projectFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            Assert.True(binSearch);
            Assert.True(objSearched);
        }

        [Theory]
        [InlineData("TestHelpers.NetCoreTests.xUnit")]
        public void ProjectNameFromAssemblyNoLogger(string projectName)
        {
            string detectedName = _testHelper.GetTestProjectNameFromCallingAssembly();

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
        public void ProjectDirectoryName()
        {
            Assert.Equal("TestHelpers.NetCoreTests.xUnit", _testHelper.ProjectDirectoryName);
        }

        [Fact]
        public void StreamReaderFromString()
        {
            const string InputContent = "Hello world!";
            string actualContents;
            using(StreamReader reader = TestInputHelper.CreateStreamReader(InputContent))
            {
                actualContents = reader.ReadToEnd();
            }

            Assert.Equal(InputContent, actualContents);
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

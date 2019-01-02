using System.Runtime.CompilerServices;

namespace TestHelpers
{
    public enum TestFrameworks { Unknown, MsTest, xUnit, NUnit }

    public class TestFileHelperConfiguration
    {
        /// <summary>
        /// Specifies the name of the current test project folder.  Use if the project folder isn't the same name as the test project output assembly.
        /// </summary>
        public string CurrentProjectFolderName { get; set; } = null;

        /// <summary>
        /// Specifies a log writer for printing diagnostic logs from the test helper.
        /// </summary>
        public ITestLogWriter LogWriter { get; set; } = null;

        /// <summary>
        /// Specifies if directories staring with a '.' should be searched when trying to find current project's folder 
        /// when test is running under Live Unit Testing.  Directories staring with a '.' are usually for configuration
        /// </summary>
        public bool SearchDirectoriesStartingWithPeriod { get; set; } = false;

        /// <summary>
        /// Specifies if contents of directories named "bin" should be searched for project folder.  Usually this is only project output.
        /// </summary>
        public bool SearchBinDirectory { get; set; } = false;
        
        /// <summary>
        /// Specifies if contents of directories named "obj" should be searched for project folder.  Usually this is only project output.
        /// </summary>
        public bool SearchObjDirectory { get; set; } = false;

        /// <summary>
        /// Specifies how deep to search the folder tree structure when trying to find the current project's folder
        /// when test is running under Live Unit Testing.
        /// </summary>
        public int TestDirectorySearchDepth { get; set; } = 6;

        /// <summary>
        /// Manually sets the test framework the project is running
        /// </summary>
        public TestFrameworks TestFramework { get; set; } = TestFrameworks.Unknown;

        internal bool IsLut { get; set; } = false;
        internal string MockBinaryRootPath { get; set; } = null;
    }
}

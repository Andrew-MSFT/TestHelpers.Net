﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.xUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.MsTestTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NetFx.MsTestTests")]
namespace Hallsoft.TestHelpers
{
    public enum TestFrameworks { Unknown, MsTest, xUnit, NUnit }

    public class VsTestHelperConfiguration
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
        /// Specifies if hidden directories should be searched when trying to find current project's folder 
        /// when test is running under Live Unit Testing.
        /// </summary>
        public bool SearchHiddenDirectories { get; set; } = false;

        /// <summary>
        /// Specifies how deep to search the folder tree structure when trying to find the current project's folder
        /// when test is running under Live Unit Testing.
        /// </summary>
        public int TestDirectorySearchDepth { get; set; } = 6;

        /// <summary>
        /// Manually sets the test framework the project is running
        /// </summary>
        public TestFrameworks TestFramework { get; set; } = TestFrameworks.Unknown;
    }
}
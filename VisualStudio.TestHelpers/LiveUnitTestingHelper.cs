using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.xUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.MsTestTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NetFx.MsTestTests")]
namespace VisualStudio.TestHelpers
{
    public enum TestFrameworks { Unknown, MsTest, xUnit, NUnit }

    public class LiveUnitTestingHelper
    {
        private List<string> DiagnosticLog { get; set; }

        public string CurrentProjectFolderName { get; private set; }
        public TestFrameworks TestFramework { get; set; }

        internal ITestLogWriter LogWriter { get; set; }

        public LiveUnitTestingHelper()
        {
            string assemblyName = GetTestProjectNameFromCallingAssembly();
            Initialize(assemblyName);
        }

        public LiveUnitTestingHelper(string currentProjectFolderName)
        {
            Initialize(currentProjectFolderName);

        }

        private void Initialize(string currentProjectName)
        {
            this.CurrentProjectFolderName = currentProjectName;
            this.TestFramework = DetectTestFramework();
        }

        private void LogMessage(string message)
        {
            if(this.LogWriter != null)
            {
                this.LogWriter.LogMessage(message);
            }
        }

        public string GetTestProjectDirectory()
        {
            string rootPath;
            string codeBase = Assembly.GetCallingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string dir = Path.GetDirectoryName(path);

            LogMessage($"Assembly executing in {dir}");

            if (IsRunningUnderLut())
            {
                LogMessage(dir);
                string solutionRoot = dir.Substring(0, dir.IndexOf(@"\.vs\") + 1);
                bool directoryFound = FindProjectDirectory(solutionRoot, this.CurrentProjectFolderName, out string detectedFolder);
                if (directoryFound)
                {
                    rootPath = detectedFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException($"Could not find directory for project {this.CurrentProjectFolderName}.  Use the {nameof(this.CurrentProjectFolderName)} property to specify explicitly");
                }
            }
            else
            {
                rootPath = dir.Substring(0, dir.IndexOf(@"\bin\") + 1);
            }

            return rootPath;
        }

        internal bool FindProjectDirectory(string startingPath, string projectFolderName, out string projectFolder, int levelsToSearch = 5, bool skipHiddenDirs = true)
        {
            IEnumerable<string> subDirectories = Directory.EnumerateDirectories(startingPath);
            projectFolder = null;
            levelsToSearch--;
            bool found = false;

            LogMessage($"Search for project folder {projectFolderName} in {startingPath}.  Subfolder search depth {levelsToSearch}");

            foreach (string dir in subDirectories)
            {
                var currentSubDir = new DirectoryInfo(dir);
                string subDirName = currentSubDir.Name;
                if (skipHiddenDirs && subDirName.StartsWith("."))
                {
                    continue;
                }
                if (subDirName == projectFolderName)
                {
                    LogMessage($"Project folder found: {currentSubDir.FullName}");

                    projectFolder = dir;
                    return true;
                }
                else if (levelsToSearch > 0)
                {
                    found = FindProjectDirectory(currentSubDir.FullName, projectFolderName, out projectFolder, levelsToSearch);
                    if (found)
                    {
                        break;
                    }
                }
            }

            return found;
        }

        public string GetTestProjectNameFromCallingAssembly()
        {
            string name = null;
            string thisAssembly = Assembly.GetExecutingAssembly().GetName().Name;

            var stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            //Have to find the first assembly in the callstack that isn't this one
            foreach (StackFrame stackFrame in stackFrames)
            {
                MethodBase method = stackFrame.GetMethod();
                string assemblyName = method.Module.Assembly.GetName().Name;
                if (assemblyName != thisAssembly)
                {
                    name = assemblyName;
                    break;
                }
            }

            LogMessage($"Calling assembly name: {name}");
            return name;
        }

        public bool IsRunningUnderLut()
        {
            bool isLut = IsLutBasedOnPath();

            LogMessage($"IsLut: {isLut}");

            return isLut;
        }

        private TestFrameworks DetectTestFramework()
        {
            var stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            LogMessage("Walking stack for LUT detection");
            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                MethodBase method = stackFrame.GetMethod();
                string assemblyName = method.Module.Assembly.GetName().Name;

                LogMessage($"     {assemblyName}::{method}");

                if (assemblyName.StartsWith("Microsoft.VisualStudio.TestPlatform.MSTest", StringComparison.InvariantCultureIgnoreCase))
                {
                    return TestFrameworks.MsTest;
                }
                else if (assemblyName.StartsWith("xunit", StringComparison.InvariantCultureIgnoreCase))
                {
                    return TestFrameworks.xUnit;
                }
                else if (assemblyName.StartsWith("nunit", StringComparison.InvariantCultureIgnoreCase))
                {
                    return TestFrameworks.NUnit;
                }

            }

            return TestFrameworks.Unknown;
        }

        internal bool IsLutBasedOnPath()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(assemblyLocation);
            string path = Uri.UnescapeDataString(uri.Path);
            string dir = Path.GetDirectoryName(path);

            LogMessage($"IsLutBasedOnPath: dir={dir}");

            //running in the context of LUT, and the path needs to be adjusted
            if (dir.Contains(@"\.vs\"))
            {
                return true;
            }

            return false;
        }

        public StreamReader OpenFile(string folderRelativeToTestProject, string fileName)
        {
            string testProjectDirectory = GetTestProjectDirectory();
            string fullPath = Path.Combine(testProjectDirectory, folderRelativeToTestProject, fileName);
            return new StreamReader(fullPath);
        }
    }
}

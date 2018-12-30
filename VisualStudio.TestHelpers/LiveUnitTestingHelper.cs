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
        public string CurrentProjectFolderName { get; private set; }
        public TestFrameworks TestFramework { get; set; }
        public bool IsRunningAsLiveUnitTest { get; private set; }

        internal ITestLogWriter LogWriter { get; set; }

        private readonly StackFrame[] _stackFrames = new StackTrace().GetFrames();

        public LiveUnitTestingHelper()
        {
            string assemblyName = GetTestProjectNameFromCallingAssembly();
            Initialize(assemblyName);
        }

        public LiveUnitTestingHelper(string currentProjectFolderName)
        {
            Initialize(currentProjectFolderName);

        }

        private TestFrameworks DetectTestFramework()
        {
            LogMessage("Walking stack for LUT detection");
            // write call stack method names
            foreach (StackFrame stackFrame in _stackFrames)
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

        public string GetTestProjectDirectory()
        {
            string rootPath;
            string codeBase = Assembly.GetCallingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string dir = Path.GetDirectoryName(path);

            LogMessage($"Assembly executing in {dir}");

            if (this.IsRunningAsLiveUnitTest)
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

        internal string GetTestProjectNameFromCallingAssembly()
        {
            string name = null;
            string thisAssembly = Assembly.GetExecutingAssembly().GetName().Name;

            //Have to find the first assembly in the callstack that isn't this one
            foreach (StackFrame stackFrame in _stackFrames)
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

        private void Initialize(string currentProjectName)
        {
            this.CurrentProjectFolderName = currentProjectName;
            this.TestFramework = DetectTestFramework();
            this.IsRunningAsLiveUnitTest = IsRunningUnderLut();
        }

        private bool IsRunningUnderLut()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(assemblyLocation);
            string path = Uri.UnescapeDataString(uri.Path);
            string dir = Path.GetDirectoryName(path);

            LogMessage($"Detecting LUT based on exe path: exeDir={dir}");

            //running in the context of LUT, and the path needs to be adjusted
            if (dir.Contains(@"\.vs\") && dir.Contains("lut"))
            {
                LogMessage("Detected as LUT");
                return true;
            }

            LogMessage("LUT not detected");
            return false;
        }

        private void LogMessage(string message, int indentLevel = 0)
        {
            if (this.LogWriter != null)
            {
                int totalWidth = message.Length + indentLevel * 2;
                this.LogWriter.LogMessage(message.PadLeft(totalWidth));
            }
        }

        public StreamReader OpenFile(string fileName, string pathRelativeToTestProject)
        {
            string testProjectDirectory = GetTestProjectDirectory();
            string fullPath = Path.Combine(testProjectDirectory, pathRelativeToTestProject, fileName);
            return new StreamReader(fullPath);
        }
    }
}

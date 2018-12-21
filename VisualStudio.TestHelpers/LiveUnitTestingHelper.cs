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
    public class LiveUnitTestingHelper
    {
        public List<string> DiagnosticLog { get; private set; } = new List<string>();

        public string TestProjectName { get; private set; }

        public LiveUnitTestingHelper()
        {
            this.TestProjectName = GetTestProjectNameFromCallingAssembly();
        }

        public LiveUnitTestingHelper(string currentProjectName)
        {
            this.TestProjectName = currentProjectName;
        }

        private void LogMessage(string message)
        {
            this.DiagnosticLog.Add(message);
        }

        public string GetTestProjectDirectory()
        {
            string projectName = GetTestProjectNameFromCallingAssembly();
            string projectDirectory = GetTestProjectDirectory(projectName);
            return projectDirectory;
        }

        public string GetTestProjectDirectory(string projectFolderName)
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
                bool directoryFound = FindProjectDirectory(solutionRoot, projectFolderName, out string detectedFolder);
                if (directoryFound)
                {
                    rootPath = detectedFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException($"Could not find directory for project {projectFolderName}");
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

            LogMessage($"FindProjectDirectory({startingPath},{projectFolderName},{levelsToSearch}");

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

        internal bool IsLutBasedOnStack(bool isLut)
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

                if (assemblyName.StartsWith("xunit"))
                {

                }

                if (assemblyName == "Microsoft.TestPlatform.CoreUtilities" && method.Name == "BackgroundJobProcessor")
                {
                    isLut = true;
                    break;
                }
            }

            return isLut;
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
    }
}

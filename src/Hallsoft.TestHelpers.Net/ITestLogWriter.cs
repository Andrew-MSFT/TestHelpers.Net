using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.xUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.MsTestTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NetFx.MsTestTests")]
namespace Hallsoft.TestHelpers
{
    /// <summary>
    /// Defines an output log writer for a unit testing framework.
    /// </summary>
    public interface ITestLogWriter
    {
        void LogMessage(string message);
    }


    /// <summary>
    /// Default implementation of ITestLogWriter for test frameworks that use Console.WriteLine().
    /// </summary>
    public class DefaultTestLogWriter : ITestLogWriter
    {
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}

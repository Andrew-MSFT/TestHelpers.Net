using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.xUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.MsTestTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NetFx.MsTestTests")]
namespace Hallsoft.TestHelpers
{
    public interface ITestLogWriter
    {
        void LogMessage(string message);
    }

    public class ConsoleTestLogWriter : ITestLogWriter
    {
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}

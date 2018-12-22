using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.xUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.MsTestTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NetFx.MsTestTests")]
namespace VisualStudio.TestHelpers
{
    internal interface ITestLogWriter
    {
        void LogMessage(string message);
    }

    internal class ConsoleTestLogWriter : ITestLogWriter
    {
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}

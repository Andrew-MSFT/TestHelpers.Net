using Xunit.Abstractions;

namespace Hallsoft.TestHelpers.Tests
{
    internal class xUnitLogWriter : ITestLogWriter
    {
        private readonly ITestOutputHelper _output;

        public xUnitLogWriter(ITestOutputHelper output)
        {
            _output = output;
        }

        public void LogMessage(string message)
        {
            _output.WriteLine(message);
        }
    }
}

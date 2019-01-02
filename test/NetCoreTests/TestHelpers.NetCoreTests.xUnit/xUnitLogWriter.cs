using System.Collections.Generic;
using Xunit.Abstractions;

namespace TestHelpers.Net.Tests
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

    internal class LogRecorder : ITestLogWriter
    {
        public List<string> Messages { get; set; } = new List<string>();
        private readonly xUnitLogWriter _xUnitLogWriter;

        public LogRecorder(xUnitLogWriter xUnitLogWriter)
        {
            _xUnitLogWriter = xUnitLogWriter;
        }

        public void LogMessage(string message)
        {
            this.Messages.Add(message);
            _xUnitLogWriter.LogMessage(message);
        }
    }
}

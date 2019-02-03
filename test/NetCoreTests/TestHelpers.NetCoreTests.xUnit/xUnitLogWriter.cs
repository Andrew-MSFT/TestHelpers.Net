using System.Collections.Generic;
using Hallsoft.TestHelpers;
using Xunit.Abstractions;

namespace TestHelpers.Net.Tests
{
    internal class XunitLogWriter : ITestLogWriter
    {
        private readonly ITestOutputHelper _output;

        public XunitLogWriter(ITestOutputHelper output)
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
        private readonly XunitLogWriter _xUnitLogWriter;

        public LogRecorder(XunitLogWriter xUnitLogWriter)
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

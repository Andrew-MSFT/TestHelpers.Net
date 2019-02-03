using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hallsoft.TestHelpers
{
    public class TestInputHelper
    {
        public static StreamReader CreateStreamReader(string streamContents)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(streamContents);
            writer.Flush();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            return reader;
        }
    }
}

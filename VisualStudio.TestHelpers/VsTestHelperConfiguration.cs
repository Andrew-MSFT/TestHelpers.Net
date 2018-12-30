using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.xUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.MsTestTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NUnitTests")]
[assembly: InternalsVisibleTo("VisualStudio.TestHelpers.NetFx.MsTestTests")]
namespace Hallsoft.TestHelpers
{
    public enum TestFrameworks { Unknown, MsTest, xUnit, NUnit }

    public class VsTestHelperConfiguration
    {
        public bool SearchHiddenDirectories { get; set; } = false;
        public int TestDirectorySearchDepth { get; set; } = 6;
    }
}

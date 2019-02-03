# TestHelpers.Net

This library contains helper methods for opening test data files inside test projects.

I find myself constantly copying and pasting a few common helper methods from old projects to make this work across machines when using Live Unit Testing.  Below is a short overview of the basic concepts of the library.  For more complete usage, browse the various test projects contained in the "test" folder that show the complete use cases (test\\TestHelpers.NetCoreTests.xUnit has the most complete set of set cases).


## Usage

Instantiate a new instance of the TestHelper class and then open input files relative to the test project's directory using the OpenFile method.

```csharp
using Hallsoft.TestHelpers;

public void TestMethod()
{
    const string Expected = "Hello World!";
    TestFileHelper testHelper = new TestFileHelper();

    string contents;
    //Opens the file "test.txt" in the "TestInput" sub folder of the current test project
    using (StreamReader reader = testHelper.OpenFile(@"TestInput\test.txt"))
    {
        contents = reader.ReadToEnd();
    }

    Assert.Equal(Expected, contents);
}
```

### Creating streams from string input
Another operation I find myself wanting to do is to manipulate the content of test input files before passing that to the code I'm testing (e.g. to test 
error conditions of incorrectly formatted input).  To to this, use the "TestInputHelper.CreateStreamReader()" method.

```csharp
const string InputContent = "Hello world!";
string actualContents;
using(StreamReader reader = TestInputHelper.CreateStreamReader(InputContent))
{
    actualContents = reader.ReadToEnd();
}

Assert.Equal(InputContent, actualContents);
```

## Working with Live Unit Testing
Visual Studio's Live Unit Testing runs the test project outside the normal project build structure.
By default this library assumes the test project folder matches the output assembly name of the test project.  
- If that is true, opening files should just work with **no action required**.
- If the assembly name is not the same as the project folder name, you will need to manually specify the name of the project folder.

To manually specify the name of the project file create an instance of the TestHelperConfiguration class and set the CurrentProjectFolderName property, then pass the instance into the TestHelper constructor.

```csharp
TestFileHelperConfiguration config = new TestFileHelperConfiguration
{
    CurrentProjectFolderName = "ThisProjectFolderName"
};

TestFileHelper helper = new TestFileHelper(config);
```


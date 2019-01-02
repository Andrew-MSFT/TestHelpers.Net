# TestHelpers.Net

This library contains helper methods for opening test data files inside test projects.

I find myself constantly copying and pasting a few common helper methods from old projects to make this work across machines when using Live Unit Testing.

## Usage

Instantiate a new instance of the TestHelper class and then open input files using the OpenFile method.

```csharp
const string Expected = "Hello World!";
TestFileHelper testHelper = new TestFileHelper();

string contents;
using (StreamReader reader = testHelper.OpenFile("test.txt"))
{
    contents = reader.ReadToEnd();
}

Assert.Equal(Expected, contents);
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


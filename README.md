# TestHelpers.Net

This library contains helper methods for opening test data files inside test projects.

I find myself constantly copying and pasting a few common helper methods from old projects to make this work across machines when using Live Unit Testing.

## Usage

Instantiate a new instance of the TestHelper class

```csharp
TestHelper testHelper = new TestHelper();
const string Expected = "Hello World!";
string contents;
using (StreamReader reader = testHelper.OpenFile("test.txt"))
{
    contents = reader.ReadToEnd();
}

Assert.Equal(Expected, contents);
```

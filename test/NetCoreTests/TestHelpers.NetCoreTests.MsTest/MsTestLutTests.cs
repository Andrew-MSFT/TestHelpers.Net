﻿using Hallsoft.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestHelpers.Net.MSTestTests
{
    [TestClass]
    public class MsTestLutTests
    {
        readonly TestFileHelper _testHelper = new TestFileHelper();

        public MsTestLutTests()
        {
            _testHelper.Configuration.LogWriter = new DefaultTestLogWriter();
        }

        [TestMethod]
        public void CanOpenFile()
        {
            string projectDirectory = _testHelper.ProjectDirectory.FullName;
            string fullFile = Path.Combine(projectDirectory, "data", "test.txt");

            Assert.IsTrue(File.Exists(fullFile));
        }

        [TestMethod]
        public void DetectMsTestFramework()
        {
            Assert.AreEqual(TestFrameworks.MsTest, _testHelper.TestFramework);
        }
    }
}

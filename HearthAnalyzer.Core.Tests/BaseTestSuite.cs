using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HearthAnalyzer.Core.Tests
{
    [SetUpFixture]
    [TestFixture]
    public class BaseTestSuite
    {
        [SetUp]
        public void NUnitAssemblyInit()
        {
            TestLogManager.Initialize();
        }
    }
}

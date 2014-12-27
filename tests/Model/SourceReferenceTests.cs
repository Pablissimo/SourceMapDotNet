using NUnit.Framework;
using SourceMapDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapDotNetTests.Model
{
    [TestFixture]
    public class SourceReferenceTests
    {
        [Test]
        public void ToString_IncludesFilenameAndLineNumber()
        {
            var reference = new SourceReference 
            {
                File = "The file",
                LineNumber = 44
            };

            Assert.That(reference.ToString(), Is.EqualTo("The file:44"));
        }
    }
}

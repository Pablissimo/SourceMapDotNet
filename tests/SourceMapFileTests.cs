using NUnit.Framework;
using SourceMapDotNetTests.Properties;
using SourceMapNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapDotNetTests
{
    [TestFixture]
    public class SourceMapFileTests
    {
        [Test]
        public void MappingGroups_Parses()
        {
            var encoded = SourceMapFile.Parse(Resources.Library_js);
            var mappingGroups = encoded.MappingGroups;
        }
    }
}

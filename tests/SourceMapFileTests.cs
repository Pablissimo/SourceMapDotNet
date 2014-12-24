using NUnit.Framework;
using SourceMapDotNetTests.Properties;
using SourceMapNet;
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
            var consumer = new SourceMapConsumer(Resources.Library_js);
            for (int i = 0; i < 10; i++)
            {
                var positions = consumer.OriginalPositionsFor(i + 1);
            }
        }
    }
}

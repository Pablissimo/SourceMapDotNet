using NUnit.Framework;
using SourceMapNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapDotNetTests.Model
{
    [TestFixture]
    public class MappingSegmentTests
    {
        [Test]
        public void Constructor_ParsesFiveFieldVariant()
        {
            // [-1106,-50,-114,15,-5]
            const string ENCODED = "llClDlHeL";
            var segment = new MappingSegment(120, ENCODED);

            Assert.That(segment.GeneratedLineIndex, Is.EqualTo(120));
            Assert.That(segment.GeneratedColumnIndex, Is.EqualTo(-1106));
            Assert.That(segment.SourcesIndex, Is.EqualTo(-50));
            Assert.That(segment.SourceLineIndex, Is.EqualTo(-114));
            Assert.That(segment.SourceColumnIndex, Is.EqualTo(15));
            Assert.That(segment.NamesIndex, Is.EqualTo(-5));
        }

        [Test]
        public void Constructor_ParsesFourFieldVariant()
        {
            // [-1106,-50,-114,15]
            const string ENCODED = "llClDlHe";
            var segment = new MappingSegment(120, ENCODED);

            Assert.That(segment.GeneratedLineIndex, Is.EqualTo(120));
            Assert.That(segment.GeneratedColumnIndex, Is.EqualTo(-1106));
            Assert.That(segment.SourcesIndex, Is.EqualTo(-50));
            Assert.That(segment.SourceLineIndex, Is.EqualTo(-114));
            Assert.That(segment.SourceColumnIndex, Is.EqualTo(15));
            Assert.That(segment.NamesIndex, Is.Null);
        }

        [Test]
        public void Constructor_ParsesOneFieldVariant()
        {
            // [-1106]
            const string ENCODED = "llC";
            var segment = new MappingSegment(120, ENCODED);

            Assert.That(segment.GeneratedLineIndex, Is.EqualTo(120));
            Assert.That(segment.GeneratedColumnIndex, Is.EqualTo(-1106));
            Assert.That(segment.SourcesIndex, Is.Null);
            Assert.That(segment.SourceLineIndex, Is.Null);
            Assert.That(segment.SourceColumnIndex, Is.Null);
            Assert.That(segment.NamesIndex, Is.Null);
        }
    }
}

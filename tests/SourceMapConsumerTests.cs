using Moq;
using NUnit.Framework;
using SourceMapDotNetTests.Properties;
using SourceMapDotNet;
using SourceMapDotNet.Model;
using SourceMapDotNet.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapDotNetTests
{
    [TestFixture]
    public class SourceMapConsumerTests
    {
        Mock<MappingDecoder> _decoderMock;
        List<MappingGroup> _mappings = new List<MappingGroup>();

        [SetUp]
        public void Setup()
        {
            _mappings = new List<MappingGroup>
            {
                new MappingGroup 
                {
                    Segments = new [] 
                    {
                        new MappingSegment { GeneratedLineIndex = 0, SourceLineIndex = 1, SourcesIndex = 1 },
                        new MappingSegment { GeneratedLineIndex = 0, SourceLineIndex = 10, SourcesIndex = 1 }
                    }
                },
                new MappingGroup 
                {
                    Segments = new[] 
                    {
                        new MappingSegment { GeneratedLineIndex = 1, SourceLineIndex = 2, SourcesIndex = 1 }
                    }
                }
            };

            _decoderMock = new Mock<MappingDecoder>();
            _decoderMock.CallBase = true;
            _decoderMock
                .Setup(x => x.GetMappingGroups(It.IsAny<string>()))
                .Returns(_mappings);

            MappingDecoder.Default = _decoderMock.Object;
        }

        [Test]
        public void ThrowsArgumentNullException_IfNullSourceMapFileSupplied()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new SourceMapConsumer((SourceMapFile)null));
        }

        [Test]
        public void ThrowsArgumentException_IfVersionNotEqualToThree()
        {
            Assert.Throws(typeof(ArgumentException), () => new SourceMapConsumer(new SourceMapFile { Version = 4 }));
        }

        [Test]
        public void ParsesMappingsUsingDefaultDecoder()
        {
            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3, Mappings = "encoded mappings" });

            _decoderMock.Verify(x => x.GetMappingGroups("encoded mappings"), Times.Once());
        }

        [Test]
        public void OriginalPositionsFor_ReturnsPositionsMappingToGeneratedSourceLine()
        {
            // Line 1 maps to lines 2 and 11
            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3, Sources = new[] { "File", "Other" } });
            var sourceLines = consumer.OriginalPositionsFor(1);

            Assert.That(sourceLines, Is.Not.Null);
            Assert.That(sourceLines.Length, Is.EqualTo(2));

            // The segments store indices - add 1 to get line numbers
            Assert.That(sourceLines.Any(x => x.LineNumber == 2));
            Assert.That(sourceLines.Any(x => x.LineNumber == 11));
            Assert.That(sourceLines.All(x => x.File == "Other"));
        }

        [Test]
        public void OriginalPositionsFor_ReturnsFilePathWhichRespectsSourceRoot()
        {
            // Line 1 maps to lines 2 and 11
            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3, Sources = new[] { "File", "OriginalFile" }, SourceRoot="RootPath/" });
            var sourceLines = consumer.OriginalPositionsFor(1);

            Assert.That(sourceLines.All(x => x.File == "RootPath/OriginalFile"));
        }

        [Test]
        public void OriginalPositionsFor_ReturnsEmptyArray_IfNoMatchingSourceLines()
        {
            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3 });
            var sourceLines = consumer.OriginalPositionsFor(15);

            Assert.That(sourceLines, Is.Not.Null);
            Assert.That(sourceLines.Length, Is.EqualTo(0));
        }

        [Test]
        public void OriginalPositionsFor_ReturnsEmptyArray_IfMatchingMappingGroupHasNullSegments()
        {
            _mappings[1].Segments = null;
            
            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3 });
            var sourceLines = consumer.OriginalPositionsFor(2);

            Assert.That(sourceLines, Is.Not.Null);
            Assert.That(sourceLines.Length, Is.EqualTo(0));
        }

        [Test]
        public void OriginalPositionsFor_ReturnsEmptyArray_IfMatchingMappingGroupHasEmptySegments()
        {
            _mappings[1].Segments = Enumerable.Empty<MappingSegment>();

            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3 });
            var sourceLines = consumer.OriginalPositionsFor(2);

            Assert.That(sourceLines, Is.Not.Null);
            Assert.That(sourceLines.Length, Is.EqualTo(0));
        }

        [Test]
        public void OriginalPositionsFor_ThrowsArgumentOutOfRangeException_IfLineNumberLessThanZero()
        {
            var consumer = new SourceMapConsumer(new SourceMapFile { Version = 3 });

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => consumer.OriginalPositionsFor(-2));
        }
    }
}
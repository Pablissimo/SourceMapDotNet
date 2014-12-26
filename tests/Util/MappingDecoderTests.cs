using Moq;
using NUnit.Framework;
using SourceMapNet.Model;
using SourceMapNet.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapDotNetTests.Util
{
    [TestFixture]
    public class MappingDecoderTests
    {
        [Test]
        public void GetMappingSegment_ReturnsConstructedMappingSegment()
        {
            var decoder = new MappingDecoder();
            var segment = decoder.GetMappingSegment(1, "llClDlHeL");

            Assert.That(segment.GeneratedLineIndex, Is.EqualTo(1));
            Assert.That(segment.GeneratedColumnIndex, Is.EqualTo(-1106));
            Assert.That(segment.SourcesIndex, Is.EqualTo(-50));
            Assert.That(segment.SourceLineIndex, Is.EqualTo(-114));
            Assert.That(segment.SourceColumnIndex, Is.EqualTo(15));
            Assert.That(segment.NamesIndex, Is.EqualTo(-5));
        }

        [Test]
        public void GetMappingSegments_ReturnsOneSegmentPerCommaSeparatedEncodedFieldList()
        {
            var decoderMock = new Mock<MappingDecoder>();
            decoderMock.CallBase = true;

            var exampleSegment = new MappingSegment(1, "A");
            
            decoderMock
                .Setup(x => x.GetMappingSegment(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(exampleSegment);

            var segments = decoderMock.Object.GetMappingSegments(1, "first,second");

            Assert.That(segments.Count, Is.EqualTo(2));
            Assert.That(segments[0], Is.EqualTo(exampleSegment));
            Assert.That(segments[1], Is.EqualTo(exampleSegment));

            decoderMock.Verify(x => x.GetMappingSegment(1, "first"), Times.Once());
            decoderMock.Verify(x => x.GetMappingSegment(1, "second"), Times.Once());
        }

        [Test]
        public void GetMappingGroup_ReturnsMappingGroupOfSegments()
        {
            var decoderMock = new Mock<MappingDecoder>();
            decoderMock.CallBase = true;

            var segments = new[] 
            {
                new MappingSegment(1, "A"),
                new MappingSegment(2, "B")
            };

            decoderMock
                .Setup(x => x.GetMappingSegments(15, "encoded"))
                .Returns(segments);

            var result = decoderMock.Object.GetMappingGroup(15, "encoded");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Segments, Is.EqualTo(segments));
        }

        [Test]
        public void GetMappingGroups_GetsOneMappingGroupPerSemicolonSeparatedEncodedValue()
        {
            const string ENCODED = "first;;third";
            var decoderMock = new Mock<MappingDecoder>();
            decoderMock.CallBase = true;

            decoderMock
                .Setup(x => x.GetMappingGroup(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new MappingGroup());

            var result = decoderMock.Object.GetMappingGroups(ENCODED);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));

            decoderMock
                .Verify(x => x.GetMappingGroup(0, "first"), Times.Once());
            decoderMock
                .Verify(x => x.GetMappingGroup(1, ""), Times.Once());
            decoderMock
                .Verify(x => x.GetMappingGroup(2, "third"), Times.Once());
        }
    }
}

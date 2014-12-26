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

        [Test]
        public void FixUpGroupSegmentOffsets_TalliesLastGeneratedColumnIndex_ResettingAtEachGroup()
        {
            var groups = new List<MappingGroup> 
            {
                GetGroup(new MappingSegment { GeneratedColumnIndex = 2 }, new MappingSegment { GeneratedColumnIndex = 5 }),
                GetGroup(new MappingSegment { GeneratedColumnIndex = 1 }, new MappingSegment { GeneratedColumnIndex = 3 })
            };

            MappingDecoder.FixUpGroupSegmentOffsets(groups);

            Assert.That(groups[0].Segments.ElementAt(0).GeneratedColumnIndex, Is.EqualTo(2));
            Assert.That(groups[0].Segments.ElementAt(1).GeneratedColumnIndex, Is.EqualTo(7));
            Assert.That(groups[1].Segments.ElementAt(0).GeneratedColumnIndex, Is.EqualTo(1));
            Assert.That(groups[1].Segments.ElementAt(1).GeneratedColumnIndex, Is.EqualTo(4));
        }

        [Test]
        public void FixUpGroupSegmentOffsets_TalliesSourcesIndices()
        {
            var groups = new List<MappingGroup> 
            {
                GetGroup(new MappingSegment { SourcesIndex = 1 }, new MappingSegment { SourcesIndex = 4 }),
                GetGroup(new MappingSegment { SourcesIndex = null }, new MappingSegment { SourcesIndex = 3 })
            };

            MappingDecoder.FixUpGroupSegmentOffsets(groups);

            Assert.That(groups[0].Segments.ElementAt(0).SourcesIndex, Is.EqualTo(1));
            Assert.That(groups[0].Segments.ElementAt(1).SourcesIndex, Is.EqualTo(5));
            Assert.That(groups[1].Segments.ElementAt(0).SourcesIndex, Is.EqualTo(null));
            Assert.That(groups[1].Segments.ElementAt(1).SourcesIndex, Is.EqualTo(8));
        }

        [Test]
        public void FixUpGroupSegmentOffsets_TalliesSourceLineIndices()
        {
            var groups = new List<MappingGroup> 
            {
                GetGroup(new MappingSegment { SourceLineIndex = 1 }, new MappingSegment { SourceLineIndex = 4 }),
                GetGroup(new MappingSegment { SourceLineIndex = null }, new MappingSegment { SourceLineIndex = 3 })
            };

            MappingDecoder.FixUpGroupSegmentOffsets(groups);

            Assert.That(groups[0].Segments.ElementAt(0).SourceLineIndex, Is.EqualTo(1));
            Assert.That(groups[0].Segments.ElementAt(1).SourceLineIndex, Is.EqualTo(5));
            Assert.That(groups[1].Segments.ElementAt(0).SourceLineIndex, Is.EqualTo(null));
            Assert.That(groups[1].Segments.ElementAt(1).SourceLineIndex, Is.EqualTo(8));
        }

        [Test]
        public void FixUpGroupSetmentOffsets_TalliesSourceColumnIndices()
        {
            var groups = new List<MappingGroup> 
            {
                GetGroup(new MappingSegment { SourceColumnIndex = 1 }, new MappingSegment { SourceColumnIndex = 4 }),
                GetGroup(new MappingSegment { SourceColumnIndex = null }, new MappingSegment { SourceColumnIndex = 3 })
            };

            MappingDecoder.FixUpGroupSegmentOffsets(groups);

            Assert.That(groups[0].Segments.ElementAt(0).SourceColumnIndex, Is.EqualTo(1));
            Assert.That(groups[0].Segments.ElementAt(1).SourceColumnIndex, Is.EqualTo(5));
            Assert.That(groups[1].Segments.ElementAt(0).SourceColumnIndex, Is.EqualTo(null));
            Assert.That(groups[1].Segments.ElementAt(1).SourceColumnIndex, Is.EqualTo(8));
        }

        [Test]
        public void FixUpGroupSetmentOffsets_TalliesNamesIndices()
        {
            var groups = new List<MappingGroup> 
            {
                GetGroup(new MappingSegment { NamesIndex = 1 }, new MappingSegment { NamesIndex = 4 }),
                GetGroup(new MappingSegment { NamesIndex = null }, new MappingSegment { NamesIndex = 3 })
            };

            MappingDecoder.FixUpGroupSegmentOffsets(groups);

            Assert.That(groups[0].Segments.ElementAt(0).NamesIndex, Is.EqualTo(1));
            Assert.That(groups[0].Segments.ElementAt(1).NamesIndex, Is.EqualTo(5));
            Assert.That(groups[1].Segments.ElementAt(0).NamesIndex, Is.EqualTo(null));
            Assert.That(groups[1].Segments.ElementAt(1).NamesIndex, Is.EqualTo(8));
        }

        private static MappingGroup GetGroup(params MappingSegment[] segments)
        {
            return new MappingGroup
            {
                Segments = segments
            };
        }
    }
}

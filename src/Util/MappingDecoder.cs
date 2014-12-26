using SourceMapNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Util
{
    internal class MappingDecoder
    {
        static MappingDecoder _defaultDecoder;
        public static MappingDecoder Default
        {
            get
            {
                if (_defaultDecoder == null)
                {
                    _defaultDecoder = new MappingDecoder();
                }

                return _defaultDecoder;
            }
            internal set
            {
                _defaultDecoder = value;
            }
        }

        public virtual IList<MappingGroup> GetMappingGroups(string encoded)
        {
            var toReturn = new List<MappingGroup>();

            var groupsRaw = encoded.Split(';');
            for (var i = 0; i < groupsRaw.Length; i++)
            {
                toReturn.Add(GetMappingGroup(i, groupsRaw[i]));
            }

            FixUpGroupSegmentOffsets(toReturn);

            return toReturn;
        }

        internal virtual MappingGroup GetMappingGroup(int generatedLineIndex, string encoded)
        {
            if (string.IsNullOrEmpty(encoded))
            {
                return new MappingGroup();
            }
            else
            {
                return new MappingGroup
                {
                    Segments = GetMappingSegments(generatedLineIndex, encoded)
                };
            }
        }

        internal virtual IList<MappingSegment> GetMappingSegments(int generatedLineIndex, string encoded)
        {
            return 
                encoded
                .Split(',')
                .Select(x => GetMappingSegment(generatedLineIndex, x))
                .ToArray();
        }

        internal virtual MappingSegment GetMappingSegment(int generatedLineIndex, string encoded)
        {
            return new MappingSegment(generatedLineIndex, encoded);
        }

        internal static void FixUpGroupSegmentOffsets(List<MappingGroup> groups)
        {
            int? lastSourcesIndex = null;
            int? lastSourceLineIndex = null;
            int? lastSourceColumnIndex = null;
            int? lastNamesIndex = null;

            foreach (var group in groups.Where(x => x.Segments != null))
            {
                int lastGeneratedColumnIndex = 0;
                foreach (var segment in group.Segments)
                {
                    lastGeneratedColumnIndex = segment.GeneratedColumnIndex = segment.GeneratedColumnIndex + lastGeneratedColumnIndex;

                    if (segment.SourcesIndex.HasValue && lastSourcesIndex.HasValue)
                    {
                        lastSourcesIndex = segment.SourcesIndex = segment.SourcesIndex + lastSourcesIndex;
                    }
                    else if (segment.SourcesIndex.HasValue)
                    {
                        lastSourcesIndex = segment.SourcesIndex;
                    }

                    if (segment.SourceLineIndex.HasValue && lastSourceLineIndex.HasValue)
                    {
                        lastSourceLineIndex = segment.SourceLineIndex = segment.SourceLineIndex + lastSourceLineIndex;
                    }
                    else if (segment.SourceLineIndex.HasValue)
                    {
                        lastSourceLineIndex = segment.SourceLineIndex;
                    }

                    if (segment.SourceColumnIndex.HasValue && lastSourceColumnIndex.HasValue)
                    {
                        lastSourceColumnIndex = segment.SourceColumnIndex = segment.SourceColumnIndex + lastSourceColumnIndex;
                    }
                    else if (segment.SourceColumnIndex.HasValue)
                    {
                        lastSourceColumnIndex = segment.SourceColumnIndex;
                    }

                    if (segment.NamesIndex.HasValue && lastNamesIndex.HasValue)
                    {
                        lastNamesIndex = segment.NamesIndex = segment.NamesIndex + lastNamesIndex;
                    }
                    else if (segment.NamesIndex.HasValue)
                    {
                        lastNamesIndex = segment.NamesIndex;
                    }
                }
            }
        }
    }
}

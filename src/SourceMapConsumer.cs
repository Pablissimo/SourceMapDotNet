using Newtonsoft.Json;
using SourceMapNet.Model;
using SourceMapNet.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet
{
    public class SourceMapConsumer
    {
        SourceMapFile _file;
        IList<MappingGroup> _mappingGroups;

        public SourceMapConsumer(string sourceMapJson)
            : this(JsonConvert.DeserializeObject<SourceMapFile>(sourceMapJson))
        {
        }

        public SourceMapConsumer(SourceMapFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            else if (file.Version != 3)
            {
                throw new ArgumentException("Unsupported version: " + file.Version);
            }

            _file = file;

            this.ParseMappings();
        }

        public SourceReference[] OriginalPositionsFor(int line)
        {
            if (line <= 0)
            {
                throw new ArgumentOutOfRangeException("line", "must be greater than zero");
            }
            else if (line > _mappingGroups.Count)
            {
                return null;
            }

            var generatedLine = _mappingGroups[line - 1];

            if (generatedLine.Segments == null || !generatedLine.Segments.Any())
            {
                return null;
            }
            else
            {
                return 
                    generatedLine
                    .Segments
                    .Where(x => x.SourceLineIndex.HasValue)
                    .Select(x => new SourceReference
                    {
                        File = x.SourcesIndex.HasValue ? _file.Sources[x.SourcesIndex.Value] : _file.File,
                        LineNumber = x.SourceLineIndex.Value + 1
                    })
                    .Distinct(new SourceReferenceEqualityComparer())
                    .ToArray();
            }
        }

        public SourceReference? OriginalPositionFor(int line)
        {
            var matching = this.OriginalPositionsFor(line);
            return (matching ?? Enumerable.Empty<SourceReference>()).FirstOrDefault();
        }

        private void ParseMappings()
        {
            _mappingGroups = new List<MappingGroup>();

            var groupsRaw = _file.Mappings.Split(';');
            for (var i = 0; i < groupsRaw.Length; i++)
            {
                var encodedLine = groupsRaw[i];
                if (string.IsNullOrEmpty(encodedLine))
                {
                    _mappingGroups.Add(new MappingGroup());
                    continue;
                }
                else
                {
                    _mappingGroups.Add(new MappingGroup
                    {
                        Segments = encodedLine
                                        .Split(',')
                                        .Select(x => new MappingSegment(i, x))
                                        .ToArray()
                    });
                }
            }

            FixUpGroupSegmentOffsets(_mappingGroups);
        }

        private void FixUpGroupSegmentOffsets(IList<MappingGroup> groups)
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

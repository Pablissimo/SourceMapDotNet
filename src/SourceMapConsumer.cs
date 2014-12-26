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
                return new SourceReference[0];
            }

            var generatedLine = _mappingGroups[line - 1];

            if (generatedLine.Segments == null || !generatedLine.Segments.Any())
            {
                return new SourceReference[0];
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

        private void ParseMappings()
        {
            _mappingGroups = MappingDecoder.Default.GetMappingGroups(_file.Mappings);
        }
    }
}

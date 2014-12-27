using Newtonsoft.Json;
using SourceMapDotNet.Model;
using SourceMapDotNet.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceMapDotNet
{
    /// <summary>
    /// Reads and parses source map files and provides the ability to query mappings
    /// between generated source positions and their corresponding original source positions.
    /// </summary>
    public class SourceMapConsumer : ISourceMapConsumer
    {
        SourceMapFile _file;
        IList<MappingGroup> _mappingGroups;

        /// <summary>
        /// Initialises a new SourceMapConsumer using the contents of a source map file.
        /// </summary>
        /// <param name="sourceMapJson">The contents of a source map file, expressed as JSON.</param>
        public SourceMapConsumer(string sourceMapJson)
            : this(JsonConvert.DeserializeObject<SourceMapFile>(sourceMapJson))
        {
        }

        /// <summary>
        /// Initialises a new SourceMapConsumer using an already-deserialised SourceMapFile instance.
        /// </summary>
        /// <param name="file">The SourceMapFile instance to be queried.</param>
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
            _mappingGroups = MappingDecoder.Default.GetMappingGroups(_file.Mappings);
        }

        /// <summary>
        /// Determines the set of positions in original source files to which a given 1-based line number
        /// in the generated source file corresponds. If a line in the generated source has no corresponding
        /// original source line, an empty array is returned.
        /// </summary>
        /// <param name="line">The 1-based line number of the generated source for which the set of
        /// original source lines that contributed is required.</param>
        /// <returns>An array of SourceReference objects representing the original source lines that correspond
        /// to the requested generated source line, or an empty array if no such lines exist.</returns>
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
    }
}

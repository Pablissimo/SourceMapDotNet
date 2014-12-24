using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Model
{
    public class SourceMapFile
    {
        public int Version { get; set; }
        public string File { get; set; }
        public string SourceRoot { get; set; }
        public IEnumerable<string> Sources { get; set; }
        public IEnumerable<string> SourcesContent { get; set; }
        public IEnumerable<string> Names { get; set; }
        public string Mappings { get; set; }

        private IList<MappingGroup> _groups;
        public IList<MappingGroup> MappingGroups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = new List<MappingGroup>();

                    var groupsRaw = Mappings.Split(';');
                    for (var i = 0; i < groupsRaw.Length; i++)
                    {
                        var encodedLine = groupsRaw[i];
                        if (string.IsNullOrEmpty(encodedLine))
                        {
                            _groups.Add(new MappingGroup());
                            continue;
                        }
                        else
                        {
                            _groups.Add(new MappingGroup
                            {
                                Segments = encodedLine
                                                .Split(',')
                                                .Select(x => new MappingSegment(i, x))
                                                .ToArray()
                            });
                        }
                    }

                    FixUpGroupSegmentOffsets(_groups);
                }

                return _groups;
            }
        }

        public static SourceMapFile Parse(string source)
        {
            return JsonConvert.DeserializeObject<SourceMapFile>(source);
        }

        private void FixUpGroupSegmentOffsets(IList<MappingGroup> groups)
        {
            /*        public int GeneratedColumnIndex { get; set; }
        public int? SourcesIndex { get; set; }
        public int? SourceLineIndex { get; set; }
        public int? SourceColumnIndex { get; set; }
        public int? NamesIndex { get; set; }
             * */

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

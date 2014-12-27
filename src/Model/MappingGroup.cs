using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapNet.Model
{
    internal class MappingGroup
    {
        public IEnumerable<MappingSegment> Segments { get; set; }
    }
}

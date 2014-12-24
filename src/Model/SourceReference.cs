using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Model
{
    public struct SourceReference
    {
        public string File { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
    }
}

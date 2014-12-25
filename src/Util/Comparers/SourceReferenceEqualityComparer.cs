using SourceMapNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Util
{
    public class SourceReferenceEqualityComparer : IEqualityComparer<SourceReference>
    {
        public bool Equals(SourceReference x, SourceReference y)
        {
            return x.LineNumber == y.LineNumber && x.File == y.File;
        }

        public int GetHashCode(SourceReference obj)
        {
            return (obj.LineNumber.GetHashCode() * 397) ^ obj.File.GetHashCode();
        }
    }
}

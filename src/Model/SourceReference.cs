using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Model
{
    /// <summary>
    /// Represents a reference to a particular position in a specified source file.
    /// </summary>
    public struct SourceReference
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// Gets or sets the line number within the file.
        /// </summary>
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.File, this.LineNumber);
        }
    }
}

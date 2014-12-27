using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapNet.Model
{
    /// <summary>
    /// Deserialisation container for source map version 3 files
    /// </summary>
    public class SourceMapFile
    {
        /// <summary>
        /// Gets or sets the version of the source map format.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Gets or sets the name of the file to which the map pertains.
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// Gets or sets the root path for sources listed.
        /// </summary>
        public string SourceRoot { get; set; }
        /// <summary>
        /// Gets or sets the collection of original source files involved in creating the
        /// generated source to which the map pertains.
        /// </summary>
        public IList<string> Sources { get; set; }
        /// <summary>
        /// Gets or sets the collection of original sources (where those sources aren't hosted).
        /// </summary>
        public IList<string> SourcesContent { get; set; }
        /// <summary>
        /// Gets or sets the collection of symbol names used by Mappings.
        /// </summary>
        public IList<string> Names { get; set; }
        /// <summary>
        /// Gets or sets the encoded mappings, translating generated source information to
        /// references to original source files.
        /// </summary>
        public string Mappings { get; set; }
    }
}

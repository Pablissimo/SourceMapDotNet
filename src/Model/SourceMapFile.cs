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
    }
}

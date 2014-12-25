﻿using System;
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

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.File, this.LineNumber);
        }
    }
}

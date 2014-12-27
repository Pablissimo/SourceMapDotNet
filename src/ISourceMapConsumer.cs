using SourceMapDotNet.Model;
using System;

namespace SourceMapDotNet
{
    public interface ISourceMapConsumer
    {
        SourceReference[] OriginalPositionsFor(int line);
    }
}

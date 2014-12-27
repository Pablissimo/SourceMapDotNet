using SourceMapDotNet.Model;
using System;
namespace SourceMapDotNet
{
    interface ISourceMapConsumer
    {
        SourceReference[] OriginalPositionsFor(int line);
    }
}

using SourceMapNet.Model;
using System;
namespace SourceMapNet
{
    interface ISourceMapConsumer
    {
        SourceReference[] OriginalPositionsFor(int line);
    }
}

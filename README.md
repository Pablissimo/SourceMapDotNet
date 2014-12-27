#What is SourceMapDotNet?

This is a very limited port of a very limited portion of Mozilla's [source-map](https://github.com/Mozilla/source-map) module that:

* Parses version 3 source map files
* Lets you ask the question 'what line numbers in the original source does line X in the generated source map to?'
* Er...
* That's it

I needed a way of parsing source map files for the above very limited situation for another project, and figured I'd publish what I had. I've no intention of expanding it to match the API of Mozilla's excellent JavaScript library, nor support generation of source maps.

##Usage

Build a SourceMapConsumer by either supplying the JSON contents of a source map file:

    var consumer = new SourceMapDotNet.SourceMapConsumer("{ source: 'map', goes: 'here' }")

Or build directly from a SourceMapFile instance, built using your JSON decoder of choice:

    var file = JsonConvert.Deserialize<SourceMapDotNet.Model.SourceMapFile>("{ source: 'map', goes: 'here' }");
    var consumer = new SourceMapDotNet.SourceMapConsumer(file);

Then find out which original source lines map to a given generated source line number (line numbers are 1-based):

    // Get the original source lines that map to line 12 of the generated source
    var matches = consumer.OriginalPositionsFor(12);

    foreach (var match in matches)
    {
        var line = match.LineNumber;
        var filename = match.File;
        
        // Do useful things...
    }

An empty array is returned if there are no matching original source lines for the specified generated source line.

##Dependencies

The following dependencies are pulled via NuGet:

* JSON.NET
* Moq
* NUnit


##Licence
Licensed under MIT, just go nuts.
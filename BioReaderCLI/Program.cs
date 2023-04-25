using BioReaderCLI.Utils;

var ui = new UI(); 

// Run if any arguments;
if (args.Any())
{
    var convert = new BioConvert(args);
    convert.BioConverter();
}
else
    ui.ColorConsoleTextLineError("Need to provite a text or file for bionic reader conversion!");




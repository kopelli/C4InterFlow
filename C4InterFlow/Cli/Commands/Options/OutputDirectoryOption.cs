using System.CommandLine;
using System.CommandLine.Parsing;

namespace C4InterFlow.Cli.Commands.Options;

public static class OutputDirectoryOption
{
    public static Option<string> Get()
    {
        const string description =
            "The output directory for the current command.";

        var option = new Option<string>(new[] { "--output-dir", "-od" }, description);

        option.SetDefaultValue(Directory.GetCurrentDirectory());

        return option;
    }
}

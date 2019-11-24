using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace dotnet_setversion
{
    public class Options
    {
        [Option('r', "recursive", Default = false, HelpText =
            "Recursively search the current directory for csproj files and apply the given version to all files found. " +
            "Mutually exclusive to the csprojFile argument.")]
        public bool Recursive { get; set; }

        /*
{
  "Version": {
    "Major": 2,
    "Minor": 2,
    "Patch": 0
  }
}         */
        [Value(0, MetaName = "version", HelpText = "The version to apply to the given csproj file(s). A filename " + 
                "can also be referenced by prepending an @filename to read the version number from the file." +
                "The file is expected to contain either a simple version or a JSON value in the following format:" +
                "{Version: {Major:Value, Minor:Value, Patch:Value}}",
            Required = true)]
        public string Version { get; set; }

        [Value(1, MetaName = "csprojFile", Required = false, HelpText =
            "Path to a csproj file to apply the given version. Mutually exclusive to the --recursive option.")]
        public string CsprojFile { get; set; }

        [Usage(ApplicationAlias = "setversion")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Directory with a single csproj file", new Options {Version = "1.2.3"});
                yield return new Example("Explicitly specifying a csproj file",
                    new Options {Version = "1.2.3", CsprojFile = "MyProject.csproj"});
                yield return new Example("Large repo with multiple csproj files in nested directories",
                    new UnParserSettings {PreferShortName = true}, new Options {Version = "1.2.3", Recursive = true});
            }
        }
    }
}
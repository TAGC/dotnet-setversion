using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace dotnet_setversion
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = CreateApplication();
            return app.Execute(args);
        }

        private static CommandLineApplication CreateApplication()
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: true)
            {
                Name = "dotnet-setversion"
            };
            
            app.HelpOption("-h|--help");
            var versionArgument = app.Argument("version", "The version to set the project as.").IsRequired();
            var csProjOption = app.Option(
                "-f|--file",
                "Identifies the csproj file to update. Looks in the current working directory if not set.",
                CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var versionString = versionArgument.Value;
                var currentDirectory = Directory.GetCurrentDirectory();
                var csprojFile = csProjOption.HasValue()
                    ? csProjOption.Value()
                    : Directory.EnumerateFileSystemEntries(currentDirectory, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (!File.Exists(csprojFile))
                {
                    Console.WriteLine($"Cannot find csproj file (path: {csprojFile})");
                    return 1;
                }
                
                UpdateVersion(versionString, csprojFile);
                return 0;
            });

            return app;
        }

        private static void UpdateVersion(string versionString, string csprojFile)
        {
            var document = XDocument.Load(csprojFile);

            document.GetOrCreateElement("Project")
                .GetOrCreateElement("PropertyGroup")
                .GetOrCreateElement("Version")
                .SetValue(versionString);

            File.WriteAllText(csprojFile, document.ToString());
            Console.WriteLine($"Setting version: {versionString}");
        }
    }

    public static class Extensions
    {
        /// <remarks>
        /// Source: http://stackoverflow.com/a/14892813/1636276
        /// </remarks>
        public static XElement GetOrCreateElement(this XContainer container, string name)
        {
            var element = container.Element(name);
            if (element == null)
            {
                element = new XElement(name);
                container.Add(element);
            }
            return element;
        }
    }
}
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace dotnet_setversion
{
    class Program
    {
        static int Main(string[] args)
        {
            string csprojFile;
            string versionString;

            bool IsHelpFlag(string arg) => arg == "-h" || arg == "--help";
            bool IsVstsFlag(string arg) => arg == "--vsts";
            void PrintUsage() => Console.WriteLine("Usage: (dotnet setversion <version>) || (dotnet setversion <csproj-path> <version> --vsts)");

            if (args.Any(IsHelpFlag))
            {
                PrintUsage();
                return 0;
            }

            switch (args.Length)
            {
                case 3 when IsVstsFlag(args[2]):
                    csprojFile = args[0];
                    versionString = args[1];
                    break;

                case 1 when !IsVstsFlag(args[0]):
                    var currentDirectory = Directory.GetCurrentDirectory();
                    csprojFile = Directory
                        .EnumerateFileSystemEntries(currentDirectory, "*.csproj", SearchOption.TopDirectoryOnly)
                        .FirstOrDefault();
                    versionString = args[0];
                    break;

                default:
                    Console.WriteLine("Usage is incorrect.");
                    PrintUsage();
                    return 1;
            }

            if (!File.Exists(csprojFile))
            {
                Console.WriteLine($"Could not locate csproj file: {csprojFile}");
                return 2;
            }

            var document = XDocument.Load(csprojFile);
            document.GetOrCreateElement("Project")
                .GetOrCreateElement("PropertyGroup")
                .GetOrCreateElement("Version")
                .SetValue(versionString);

            File.WriteAllText(csprojFile, document.ToString());
            Console.WriteLine($"Setting version: {versionString} ( in {csprojFile})");
            return 0;
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
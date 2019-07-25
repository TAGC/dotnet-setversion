using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CommandLine;

namespace dotnet_setversion
{
    internal static class Program
    {
        internal const int ExitSuccess = 0;
        internal const int ExitFailure = 1;

        internal static int Main(params string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(Run, errs => int.MinValue);
        }

        private static int Run(Options options)
        {
            if (options.Recursive && options.CsprojFile != null)
            {
                Console.WriteLine("The --recursive option and csprojFile argument are mutually exclusive.");
                return ExitFailure;
            }

            return options.Recursive ? RunRecursive(options.Version) : Run(options.Version, options.CsprojFile);
        }

        private static int Run(string version, string csprojFile)
        {
            if (csprojFile == null)
            {
                var csprojFiles = GetCsprojFiles(false);
                if (!CheckCsprojFiles(csprojFiles, false, out var exitCode)) return exitCode;
                csprojFile = csprojFiles[0];
            }

            if (!File.Exists(csprojFile))
            {
                Console.WriteLine($"Project file '{csprojFile}' does not exist.");
                return ExitFailure;
            }

            SetVersion(version, csprojFile);
            PrintSuccessString(version, csprojFile);
            return ExitSuccess;
        }

        private static int RunRecursive(string version)
        {
            var csprojFiles = GetCsprojFiles(true);
            if (!CheckCsprojFiles(csprojFiles, true, out var exitCode)) return exitCode;

            foreach (var csprojFile in csprojFiles)
            {
                SetVersion(version, csprojFile);
            }
            PrintSuccessString(version, csprojFiles);
            return ExitSuccess;
        }

        private static string[] GetCsprojFiles(bool recursive) => Directory
            .EnumerateFileSystemEntries(Directory.GetCurrentDirectory(), "*.csproj",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToArray();

        private static bool CheckCsprojFiles(string[] csprojFiles, bool allowMultiple, out int exitCode)
        {
            if (csprojFiles.Length == 0)
            {
                Console.WriteLine("Specify a project file. The current working directory does not contain a project file.");
                exitCode = ExitFailure;
                return false;
            }
            if (!allowMultiple && csprojFiles.Length > 1)
            {
                Console.WriteLine("Specify which project file to use because this folder contains more than one project file.");
                exitCode = ExitFailure;
                return false;
            }
            exitCode = ExitSuccess;
            return true;
        }

        private static void SetVersion(string version, string csprojFile)
        {
            if (version == null) throw new ArgumentNullException(nameof(version));
            if (csprojFile == null) throw new ArgumentNullException(nameof(csprojFile));

            var document = XDocument.Load(csprojFile);
            var projectNode = document.GetOrCreateElement("Project");
            var versionNode = projectNode
                                  .Elements("PropertyGroup")
                                  .SelectMany(it => it.Elements("Version"))
                                  .SingleOrDefault() ?? projectNode
                                  .GetOrCreateElement("PropertyGroup")
                                  .GetOrCreateElement("Version");
            versionNode.SetValue(version);
            File.WriteAllText(csprojFile, document.ToString());
        }

        private static void PrintSuccessString(string version, string file)
        {
            Console.WriteLine($"Set version to {version} in {file}");
        }

        private static void PrintSuccessString(string version, params string[] files)
        {
            Console.WriteLine($"Set version to {version} in:");

            foreach (var file in files)
            {
                Console.WriteLine($"\t> {file}");
            }
        }
    }

    public static class Extensions
    {
        /// <remarks>
        /// Source: http://stackoverflow.com/a/14892813/1636276
        /// </remarks>
        public static XElement GetOrCreateElement(this XContainer container, string name)
        {
            // Checks the root node as well as children
            // This prevents bombing out on solutions with 
            // msbuild style projects in it.
            var element = ((XElement)container.Nodes().FirstOrDefault(n => n is XElement));
            if (element?.Name.LocalName == name) return element;

            element = container.Element(name);
            if (element != null) return element;
            element = new XElement(name);
            container.Add(element);
            return element;
        }
    }
}
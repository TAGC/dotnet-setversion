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
            if (args.Length < 1)
            {
                Console.WriteLine("Missing version string");
                return 1;
            }

            var versionString = args[0];
            var currentDirectory = Directory.GetCurrentDirectory();
            var csprojFile = Directory.EnumerateFileSystemEntries(currentDirectory, "*.csproj", SearchOption.TopDirectoryOnly).First();
            var document = XDocument.Load(csprojFile);

            document.GetOrCreateElement("Project")
                .GetOrCreateElement("PropertyGroup")
                .GetOrCreateElement("Version")
                .SetValue(versionString);

            using (var fileStream = new FileStream(csprojFile, FileMode.Open))
            {
                document.Save(fileStream);
            }

            Console.WriteLine($"Setting version: {versionString}");
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
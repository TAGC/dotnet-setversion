using System.IO;
using Xunit;

namespace integration
{
    public class TestHelper
    {
        public string ExampleCsprojFile { get; } = @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <Description>An example csproj file.</Description>
    <TargetFrameworks>netcoreapp2.1</TargetFrameworks>
  </PropertyGroup>
</Project>";

        public string ExampleMsBuildStyleCsprojFile { get; } = "<TODO></TODO>";

        public void CopyExampleFile(string path, bool overwrite = false, bool msBuildStyle = false)
        {
            File.WriteAllText(path, msBuildStyle ? ExampleMsBuildStyleCsprojFile : ExampleCsprojFile);
        }

        public void CheckCsprojFile(string version, params string[] files)
        {
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.Contains($"<Version>{version}</Version>", content);
            }
        }

        public string GetRandomDirectory()
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(path);
            return path;
        }

        public string ChangeToRandomDirectory()
        {
            var path = GetRandomDirectory();
            Directory.SetCurrentDirectory(path);
            return path;
        }
    }
}
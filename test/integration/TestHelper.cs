using System.IO;
using Xunit;

namespace integration
{
    public class TestHelper
    {
        public string ExampleCsprojFile { get; } = @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <Description>An example csproj file.</Description>
    <TargetFrameworks>net6.0</TargetFrameworks>
  </PropertyGroup>
</Project>";

        public string ExampleSemVerFile { get; } = @"{
""Version"": {
    ""Major"": 1,
    ""Minor"": 2,
    ""Patch"": 3
    }
}";

        public void CopyExampleFile(string path, bool overwrite = false)
        {
            File.WriteAllText(path, ExampleCsprojFile);
        }

        public void CopyExampleSemVerFile(string path, bool overwrite = false)
        {
            File.WriteAllText(path, ExampleSemVerFile);
        }

        public void CheckCsprojFileForVersionElement(string version, params string[] files)
        {
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.Contains($"<Version>{version}</Version>", content);
            }
        }

        public void CheckCsprojFileForVersionPrefixElement(string version, params string[] files)
        {
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                Assert.Contains($"<VersionPrefix>{version}</VersionPrefix>", content);
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
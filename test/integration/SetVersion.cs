using System.IO;
using dotnet_setversion;
using Xunit;

namespace integration
{
    public class SetVersion
    {
        private readonly TestHelper _testHelper = new TestHelper();

        [Fact]
        public void ApplyingVersion_WithoutSpecifyingCsprojFile()
        {
            const string version = "1.2.3";
            var workingDirectory = _testHelper.ChangeToRandomDirectory();
            var csprojFile = Path.Combine(workingDirectory, "Project.csproj");
            _testHelper.CopyExampleFile(csprojFile);

            var exitCode = Program.Main(version);

            Assert.Equal(0, exitCode);
            _testHelper.CheckCsprojFile(version, csprojFile);
        }

        [Fact]
        public void DirectoryWithoutCsproj_ReturnsNonZero()
        {
            _testHelper.ChangeToRandomDirectory();

            var exitCode = Program.Main("1.2.3");

            Assert.NotEqual(0, exitCode);
        }

        [Fact]
        public void DirectoryWithMultipleCsproj_ReturnsNonZero_WhenNoFileIsSpecified()
        {
            _testHelper.ChangeToRandomDirectory();
            _testHelper.CopyExampleFile("Project A.csproj");
            _testHelper.CopyExampleFile("Project B.csproj");

            var exitCode = Program.Main("1.2.3");

            Assert.NotEqual(0, exitCode);
        }

        [Fact]
        public void ApplyingVersion_WithSpecifyingExistingFile()
        {
            var csprojFile = Path.GetTempFileName();
            _testHelper.CopyExampleFile(csprojFile, true);
            const string version = "1.2.3";

            var exitCode = Program.Main(version, csprojFile);

            Assert.Equal(0, exitCode);
            _testHelper.CheckCsprojFile(version, csprojFile);
        }

        [Fact]
        public void ApplyingVersions_WithMultipleProjectFilesInCurrentDirectory_WhenSpecifyingProjectFile()
        {
            const string csprojFileA = "Project A.csproj";
            const string csprojFileB = "Project B.csproj";
            const string version = "1.2.3";
            _testHelper.ChangeToRandomDirectory();
            _testHelper.CopyExampleFile(csprojFileA);
            _testHelper.CopyExampleFile(csprojFileB);

            var exitCode = Program.Main(version, csprojFileA);

            Assert.Equal(0, exitCode);
            _testHelper.CheckCsprojFile(version, csprojFileA);
            // check the other file remains untouched.
            Assert.Equal(_testHelper.ExampleCsprojFile, File.ReadAllText(csprojFileB));
        }

        [Fact]
        public void DirectoryContainingNoCsproj_ReturnsNonZero_WhenRecursive()
        {
            _testHelper.ChangeToRandomDirectory();

            var exitCode = Program.Main("-r", "1.2.3");

            Assert.NotEqual(0, exitCode);
        }

        [Fact]
        public void ApplyingVersionToAllFiles_WhenRecursive()
        {
            const string projectA = "Project A.csproj";
            const string projectB = "Project B.csproj";
            const string moduleA = "Module A";
            const string moduleB = "Module B";
            const string version = "1.2.3";
            var moduleFileA = Path.Combine(moduleA, moduleA + ".csproj");
            var moduleFileB = Path.Combine(moduleB, moduleB + ".csproj");
            _testHelper.ChangeToRandomDirectory();
            _testHelper.CopyExampleFile(projectA);
            _testHelper.CopyExampleFile(projectB);
            Directory.CreateDirectory(moduleA);
            Directory.CreateDirectory(moduleB);
            _testHelper.CopyExampleFile(moduleFileA);
            _testHelper.CopyExampleFile(moduleFileB);

            var exitCode = Program.Main("-r", version);

            Assert.Equal(0, exitCode);
            _testHelper.CheckCsprojFile(version, projectA);
            _testHelper.CheckCsprojFile(version, projectB);
            _testHelper.CheckCsprojFile(version, moduleFileA);
            _testHelper.CheckCsprojFile(version, moduleFileB);
        }

        [Fact]
        public void SetVersion_ReturnsNonZero_WhenRecursiveAndSpecifyingProjectFile()
        {
            var exitCode = Program.Main("-r", "1.2.3", "void.csproj");

            Assert.NotEqual(0, exitCode);
        }

        [Fact]
        public void ApplyingVersion_WithoutSpecifyingCsprojFileWithSimpleVersionFromFile()
        {
            const string version = "1.2.3";
            const string versionFilename = "@sem.ver";
            var workingDirectory = _testHelper.ChangeToRandomDirectory();
            using (var semFileWriter = File.CreateText(Path.Combine(workingDirectory, "sem.ver")))
            {
                semFileWriter.Write(version);
            }
            var csprojFile = Path.Combine(workingDirectory, "Project.csproj");
            _testHelper.CopyExampleFile(csprojFile);

            var exitCode = Program.Main(versionFilename);

            Assert.Equal(0, exitCode);
            _testHelper.CheckCsprojFile(version, csprojFile);
        }

        [Fact]
        public void ApplyingVersion_WithoutSpecifyingCsprojFileWithVersionFromJsonFile()
        {
            const string version = "1.2.3";
            const string versionFilename = "@sem.ver";
            var workingDirectory = _testHelper.ChangeToRandomDirectory();
            var csprojFile = Path.Combine(workingDirectory, "Project.csproj");
            _testHelper.CopyExampleFile(csprojFile);
            _testHelper.CopyExampleSemVerFile("sem.ver");
            var exitCode = Program.Main(versionFilename);

            Assert.Equal(0, exitCode);
            _testHelper.CheckCsprojFile(version, csprojFile);
        }

        [Fact]
        public void SetVersion_ReturnsNonZero_WhenExtractingVersionFromNonExistentFile()
        {
            var workingDirectory = _testHelper.ChangeToRandomDirectory();
            var csprojFile = Path.Combine(workingDirectory, "Project.csproj");
            _testHelper.CopyExampleFile(csprojFile);

            var exitCode = Program.Main("@non_existent_file");

            Assert.NotEqual(0, exitCode);
        }
    }
}
# Contributing to dotnet-setversion

Thank you for helping keep `dotnet-setversion` up to date! As .NET evolves, this tool needs a yearly update to support the latest SDKs and features.

## The Yearly Update Workflow

When a new version of .NET is released, follow these steps to prepare a Pull Request.

### 1. Update Project Dependencies
Update all `PackageReference` versions in all `.csproj` files to their latest stable releases. This includes:
* `System.Text.Json`
* `Microsoft.NET.Test.Sdk`
* `xunit` and its runners

### 2. Target the New Framework
Update the `src/dotnet-setversion/dotnet-setversion.csproj` file:
* **Add the new version:** Add the latest target (e.g., `net11.0`) to the `<TargetFrameworks>` list.
* **Assess Breaking Changes:** Only drop support for older .NET versions if the new dependencies are strictly incompatible. 
* **Note on Versioning:** Adding a new .NET version is a **Minor** change. Dropping support for an older version is a **Major (Breaking)** change.

### 3. Update Integration Test Metadata
The integration tests use a hardcoded example project file to verify the tool's behavior.
* Navigate to `test/integration/TestHelper.cs`.
* Update the `ExampleCsprojFile` string to target the newest .NET version.

### 4. Sync the CI Environment
Review the `appveyor.yml` file:
* **Images:** If AppVeyor has released a newer build image that natively supports the new SDK, update the `image:` property.
* **Workarounds:** If the new SDK is natively supported, ensure any manual installation scripts (like the `dotnet-install.ps1` workaround used for .NET 10) are removed to keep the build clean.

### 5. Local Verification
Before submitting, ensure your local environment is clean:
1. **Compile:** Run `dotnet build`. There should be no errors or warnings.
2. **Test:** Run `dotnet test`. This will run the suite against all targeted frameworks. All tests must pass.

## Submitting Your Changes

1. **Branching:** Create a feature branch from `develop` (e.g., `feat/dotnet-11-support`).
2. **Pull Request:** Open your PR against the **`develop`** branch of the main repository.
3. **Documentation:** In your PR description, clearly state if any older .NET versions were dropped so the maintainer can prepare the correct version bump.
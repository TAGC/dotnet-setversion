**dotnet-setversion** is a simple .NET Core CLI tool used to update the version information within .NET Core `*.csproj` files.

It is based on [dotnet-gitversion](https://github.com/ah-/dotnet-gitversion), but is updated to work for the new `*.csproj` format instead of project.json, and relies on the version information being passed to it.

## Usage

Reference **dotnet-setversion** in your project's `*.csproj` (as below) and then run `dotnet restore` to fetch the package.

```xml
<Project Sdk="Microsoft.NET.Sdk">
...
  <ItemGroup>
    <DotNetCliToolReference Include="dotnet-setversion" Version="*" />
  </ItemGroup>
...
<Project>
```

With your project root folder set as the current working directory, invoke the following:

```
$ dotnet setversion 0.1.2-beta0001
```

Replace '0.1.2-beta0001' with any valid version string.

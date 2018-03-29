**dotnet-setversion** is a simple .NET Core CLI tool used to update the version information within .NET Core `*.csproj` files.

It is based on [dotnet-gitversion](https://github.com/ah-/dotnet-gitversion), but is updated to work for the new `*.csproj` format instead of project.json, and relies on the version information being passed to it.

[![NuGet](https://img.shields.io/nuget/v/dotnet-setversion.svg)](https://www.nuget.org/packages/dotnet-setversion)
[![Build status](https://ci.appveyor.com/api/projects/status/5e4apspa6wg86t9n/branch/master?svg=true)](https://ci.appveyor.com/project/TAGC/dotnet-setversion/branch/master)


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

With [GitVersion](https://github.com/GitTools/GitVersion) installed, you can do the following as well:

```
$ dotnet setversion $(gitversion -showvariable NugetVersionV2)
```

This (or something similar) can of course be done during a continuous integration build, which is the main intention behind developing this project.

**dotnet-setversion** is a simple .NET Core CLI tool used to update the version information within .NET Core `*.csproj` files.

It is based on [dotnet-gitversion](https://github.com/ah-/dotnet-gitversion), but is updated to work for the new `*.csproj` format instead of project.json, and relies on the version information being passed to it.

[![NuGet](https://img.shields.io/nuget/v/dotnet-setversion.svg)](https://www.nuget.org/packages/dotnet-setversion)
[![Build status](https://ci.appveyor.com/api/projects/status/5e4apspa6wg86t9n/branch/master?svg=true)](https://ci.appveyor.com/project/TAGC/dotnet-setversion/branch/master)


## Usage

Install **dotnet-setversion** as a global tool using the dotnet CLI:

```
$ dotnet tool install -g dotnet-setversion
```

If the command completed successfully, you're able to invoke **dotnet-setversion** in any directory like this:

```
$ setversion 0.1.2-beta0001
```

You can also update the version information of a specific project file by invoking like this:

```
$ setversion 0.1.2-beta0001 MyProject.csproj
```

If you happen to have a rather big repo including several project files and you want to update them all at once, you can use the `--recursive` option.  
This will update any project file in and below the current working directory.

```
$ setversion -r 0.1.2-beta0001
```

For each example, replace '0.1.2-beta0001' with any valid version string.

With [GitVersion](https://github.com/GitTools/GitVersion) installed, you can do the following as well:

```
$ dotnet $(gitversion -showvariable NugetVersionV2)
```

This (or something similar) can of course be done during a continuous integration build, which is the main intention behind developing this project.

## Migrating from 1.* to 2.*

**dotnet-setversion** used to be a [per-project tool](https://docs.microsoft.com/en-us/dotnet/core/tools/extensibility#per-project-based-extensibility), but has now been reworked as [.NET Core Global Tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).  
As a consequence of this, you have to remove the `<DotNetCliToolReference>` element referencing **dotnet-setversion** or you'll get an error when running `dotnet restore`.

Depending on your build strategy, you have to install **setversion** once on your build agent (see [Usage](#usage)) or integrate the install command into your build script.

Finally you have to change the way to invoke the program from `dotnet setversion` to `setversion`.
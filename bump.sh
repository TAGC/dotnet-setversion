#!/bin/sh
#
# Bump the version, recreating the changelog and creating a new tag.
NEXT_RELEASE=$(gitversion -showvariable MajorMinorPatch)
cd dotnet-setversion
dotnet restore
dotnet setversion $NEXT_RELEASE
cd ..
github_changelog_generator --future-release v$NEXT_RELEASE
git add dotnet-setversion/dotnet-setversion.csproj
git add CHANGELOG.md
git commit --no-verify -m "Bump to v$NEXT_RELEASE"
git tag -a v$NEXT_RELEASE -m "Version $NEXT_RELEASE"

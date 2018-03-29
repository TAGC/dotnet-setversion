#!/bin/bash

# Arrange
dotnet restore

# Act
dotnet setversion 1.2.3-beta.4

# Assert
actual=test-issue-8.csproj 
expected=expected-output
diff -b $expected $actual && echo "Test succeeded" || (echo "Test failed" && exit 1)
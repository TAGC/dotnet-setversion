# This Dockerfile is used to test that that CLI tool
# works when only the 2.0 SDK is installed in the environment

FROM microsoft/dotnet:2.0-sdk
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet setversion 1.2.3
ENTRYPOINT cat test-project-netcore2.csproj
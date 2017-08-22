# This Dockerfile is used to test that that CLI tool
# works when only the 1.1 SDK is installed in the environment

FROM microsoft/dotnet:1.1-sdk
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet setversion 1.2.3
ENTRYPOINT cat test-project.csproj
FROM microsoft/dotnet:2.0-sdk
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet setversion 1.2.3
ENTRYPOINT cat test-issue-8.csproj
# This Dockerfile is used to test that the CLI tool can be pointed
# to a specific csproj file, instead of assuming it exists in the current
# working directory
# See https://github.com/TAGC/dotnet-setversion/issues/5

FROM microsoft/dotnet:2.0-sdk
WORKDIR /app
COPY . .
RUN dotnet restore

# In order to test this out, we assume the user has installed dotnet-setversion
# as a normal binary accessible from PATH (as opposed to a per-project dependency).
# See https://docs.microsoft.com/en-us/dotnet/core/tools/extensibility#path-based-extensibility

RUN git clone https://github.com/TAGC/dotnet-setversion && \
    cd dotnet-setversion/dotnet-setversion && \
    dotnet publish -c Release -r linux-x64 -f netcoreapp2.0 -o dist && \
    chmod +x dist/dotnet-setversion && \
    ln -s $(pwd)/dist/dotnet-setversion /usr/local/bin

RUN dotnet restore
RUN cd .. && dotnet setversion -f app/test-project-netcore2.csproj 1.2.3
ENTRYPOINT cat test-project-netcore2.csproj
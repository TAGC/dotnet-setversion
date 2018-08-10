# End to end test for dotnet-setversion

$SLN_DIR=$(pwd)
$TOOL_PROJ_DIR="$SLN_DIR\src\dotnet-setversion"
$NUGET_CONFIG_TEMPLATE="$SLN_DIR\test\NuGet.config.template"

# Run integration tests
dotnet test test/integration

if (-not $?) {
    exit 1;
}

# Pack the tool
dotnet pack $TOOL_PROJ_DIR -c Release -o out

# Copy required files and run test defined by each project
$success=$True
foreach($d in dir -Directory "test\docker") {
    echo "Testing: $d"
    cp -r -force $TOOL_PROJ_DIR/out test/docker/$d
    cp -force $NUGET_CONFIG_TEMPLATE test/docker/$d/NuGet.config
    docker build test/docker/$d -t dotnet-setversion:$d -f test/docker/$d/Dockerfile.windows
    docker run --rm dotnet-setversion:$d
    
    if (-not $?) {
        $success=$?
    }
}

if ($success) {
    exit 0;
} else {
    exit 1;
}
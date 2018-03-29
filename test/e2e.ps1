# End to end test for dotnet-setversion

$SLN_DIR=$(pwd)
$TOOL_PROJ_DIR="$SLN_DIR\src\dotnet-setversion"
$NUGET_CONFIG_TEMPLATE="$SLN_DIR\test\NuGet.config.template"

# Pack the tool
dotnet pack $TOOL_PROJ_DIR -c Release -o out

# Copy required files and run test defined by each project
$success=$True
foreach($d in dir -Directory "test") {
    echo "Testing: $d"
    cp -r -force $TOOL_PROJ_DIR/out test/$d
    cp -force $NUGET_CONFIG_TEMPLATE test/$d/NuGet.config
    docker build test/$d -t dotnet-setversion:$d -f test/$d/Dockerfile.windows
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
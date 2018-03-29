# End to end test for dotnet-setversion

SLN_DIR=$(pwd)
TOOL_PROJ_DIR=$SLN_DIR/src/dotnet-setversion
NUGET_CONFIG_TEMPLATE=$SLN_DIR/test/NuGet.config.template

# Pack the tool
dotnet pack $TOOL_PROJ_DIR -c Release -o out

# Copy required files and run test defined by each project
err=0
for d in `find test/* -maxdepth 0 -type d`
do
    echo "Testing: $d"
    cp -r $TOOL_PROJ_DIR/out $d
    cp $NUGET_CONFIG_TEMPLATE $d/NuGet.config
    docker build $d -t dotnet-setversion:$(basename $d) -f $d/Dockerfile.linux
    docker run --rm dotnet-setversion:$(basename $d) || err=$?
done

exit $err
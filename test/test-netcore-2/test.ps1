# Arrange
dotnet restore

# Act
dotnet setversion 1.2.3-beta.4

# Assert
$actual="test-netcore-2.csproj" 
$expected="expected-output"
$result=(diff (cat $expected) (cat $actual))

if ($result) {
    echo $result
    echo "Test failed"
    exit 1
} else {
    echo "Test succeeded"
    exit 0
}
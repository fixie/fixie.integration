. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

main {
    mit-license $copyright
    exec { dotnet clean src -c $configuration /nologo -v minimal }
    exec { dotnet build src -c $configuration /nologo }

    $test = { dotnet fixie --configuration $configuration --no-build }

    exec $test src/CustomConvention.Tests 10
    exec $test src/DefaultConvention.Tests 10
    exec $test src/FSharp.Tests 2
    exec $test src/NUnitStyle.Tests
    exec $test src/x64.Tests
    exec $test src/x86.Tests
    exec $test src/xUnitStyle.Tests
}
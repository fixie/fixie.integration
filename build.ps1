. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

main {
    mit-license $copyright
    step { dotnet --version }
    exec { dotnet clean src -c $configuration /nologo -v minimal }
    exec { dotnet build src -c $configuration /nologo }

    $test = { dotnet fixie --configuration $configuration --no-build }

    exec $test src/CustomConvention.Tests 1
    exec $test src/DefaultConvention.Tests 1
    exec $test src/FSharp.Tests 1
    exec $test src/NUnitStyle.Tests
    exec $test src/x64.Tests
    exec $test src/x86.Tests
    exec $test src/xUnitStyle.Tests
}
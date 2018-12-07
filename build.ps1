. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

main {
    mit-license $copyright
    exec { dotnet clean src -c $configuration /nologo -v minimal }
    exec { dotnet build src -c $configuration /nologo }

    $test = { dotnet fixie --configuration $configuration --no-build }

    exec $test src/Async.Tests
    exec $test src/Categories.Tests
    exec $test src/CustomConvention.Tests
    exec $test src/DefaultConvention.Tests 2
    exec $test src/FSharp.Tests 2
    exec $test src/Inclusive.Tests -1
    exec $test src/IoC.Tests
    exec $test src/MbUnitStyle.Tests
    exec $test src/NUnitStyle.Tests
    exec $test src/Shuffle.Tests
    exec $test src/TargetFramework.Tests
    exec $test src/x64.Tests
    exec $test src/x86.Tests
    exec $test src/xUnitStyle.Tests
}
. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

function Build {
    mit-license $copyright
    exec { dotnet clean src -c $configuration /nologo -v minimal }
    exec { dotnet build src -c $configuration /nologo }
}

function Test {
    $test = { dotnet fixie --configuration $configuration --no-build }

    run-tests src/Async.Tests $test 0
    run-tests src/Categories.Tests $test 0
    run-tests src/CustomConvention.Tests $test 0
    run-tests src/DefaultConvention.Tests $test 2
    run-tests src/FSharp.Tests $test 0
    run-tests src/Inclusive.Tests $test -1
    run-tests src/IoC.Tests $test 0
    run-tests src/LowCeremony.Tests $test 0
    run-tests src/MbUnitStyle.Tests $test 0
    run-tests src/Nested.Tests $test 0
    run-tests src/NUnitStyle.Tests $test 0
    run-tests src/Parameterized.Tests $test 0
    run-tests src/Shuffle.Tests $test 0
    run-tests src/Skipped.Tests $test 0
    run-tests src/Static.Tests $test 0
    run-tests src/x64.Tests $test 0
    run-tests src/x86.Tests $test 0
    run-tests src/xUnitStyle.Tests $test 0
}

run-build {
    step { Build }
    step { Test }
}
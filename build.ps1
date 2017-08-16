. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

function License {
    mit-license $copyright
}

function Clean {
    exec { dotnet clean src -c $configuration }
}

function Restore {
    exec { dotnet restore src }
}

function Build {
    exec { dotnet build src -c $configuration }
}

function Test {

    $test = { dotnet fixie --configuration $configuration --no-build }
    $test_x86 = { dotnet fixie --configuration $configuration --no-build --x86 }

    run-tests src/DefaultConvention.Tests $test 2
    run-tests src/CustomConvention.Tests $test 0
    run-tests src/x64.Tests $test 0

    run-tests src/DefaultConvention.Tests $test_x86 2
    run-tests src/CustomConvention.Tests $test_x86 0
    run-tests src/x86.Tests $test_x86 0
}

run-build {
    step { License }
    step { Clean }
    step { Restore }
    step { Build }
    step { Test }
}
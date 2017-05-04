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
    exec { dotnet restore src --packages packages -s https://api.nuget.org/v3/index.json }
}

function Build {
    exec { dotnet build src -c $configuration }
}

function Test {
    [IO.File]::Delete("actual.log")

    run-tests DefaultConvention.Tests $configuration
    run-tests CustomConvention.Tests $configuration
    run-tests x64.Tests $configuration

    run-tests DefaultConvention.Tests $configuration 32
    run-tests CustomConvention.Tests $configuration 32
    run-tests x86.Tests $configuration 32
    
    write-host "Verify output by comparing actual.log against expected.log." -ForegroundColor Green
}

run-build {
    step { License }
    step { Clean }
    step { Restore }
    step { Build }
    step { Test }
}
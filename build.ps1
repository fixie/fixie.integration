. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

function License {
    mit-license $copyright
}

function Clean {
    exec { msbuild /t:clean /v:q /nologo /p:Configuration=$configuration src\Fixie.Integration.sln }
}

function Restore {
    exec { nuget restore src\Fixie.Integration.sln -source https://api.nuget.org/v3/index.json -RequireConsent -o "src\packages" }
}

function Build {
    exec { msbuild /t:build /v:q /nologo /p:Configuration=$configuration src\Fixie.Integration.sln }
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
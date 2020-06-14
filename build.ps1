. .\build-helpers

$authors = "Patrick Lioi"
$copyright = copyright 2017 $authors
$configuration = 'Release'

main {
    mit-license $copyright
    step { dotnet --version }
    exec { dotnet clean src -c $configuration --nologo -v minimal }
    exec { dotnet build src -c $configuration --nologo }

    exec { dotnet fixie CustomConvention.Tests --configuration $configuration --no-build } 1
    exec { dotnet fixie DefaultConvention.Tests --configuration $configuration --no-build } 1
    exec { dotnet fixie FSharp.Tests --configuration $configuration --no-build } 1
    exec { dotnet fixie *UnitStyle.Tests --configuration $configuration --no-build }
}
$ErrorActionPreference = "Stop"

function step($command, $expectedReturnCode=0) {
    write-host ([Environment]::NewLine + $command.ToString().Trim()) -fore CYAN
    & $command
    if ($lastexitcode -ne $expectedReturnCode) {
        throw "Expected return code $expectedReturnCode, but was $lastexitcode."
    }
}

step { dotnet tool restore }
step { dotnet clean src -c Release --nologo -v minimal }
step { dotnet build src -c Release --nologo }
step { dotnet fixie CustomConvention.Tests -c Release --no-build } 1
step { dotnet fixie DefaultConvention.Tests -c Release --no-build } 1
step { dotnet fixie FSharp.Tests -c Release --no-build } 1
step { dotnet fixie *UnitStyle.Tests -c Release --no-build }
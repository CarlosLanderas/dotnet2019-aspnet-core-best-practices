function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }


echo "building..."

exec { & dotnet build DotNet2019.sln -c Release -v q /nologo }

echo "up docker compose"

#exec { & docker-compose -f .\builds\docker-compose-infrastructure.yml up -d }
	
echo "Running functional tests"

try {

Push-Location -Path .\test\FunctionalTests
        exec { & dotnet test}
} finally {
        Pop-Location
}

echo "down docker compose"

#exec { & docker-compose -f .\builds\docker-compose-infrastructure.yml down }

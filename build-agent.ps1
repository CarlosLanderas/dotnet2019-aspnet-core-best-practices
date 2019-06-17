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

exec { & docker-compose -f .\build\docker-compose-buildagent.yml up --build -d }
exec { & docker-compose -f .\build\docker-compose-buildagent.yml exec -e 'ConnectionStrings__SqlServer=Server=sqlserver;User Id=sa;Password=Password12!;Initial Catalog=FunctionalTests;MultipleActiveResultSets=true' functionaltests dotnet test test/FunctionalTests/FunctionalTests.csproj -p:ParallelizeTestCollections=false --logger trx --results-directory /var/temp }
exec { & docker-compose -f .\build\docker-compose-buildagent.yml down }
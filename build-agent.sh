set -e
export COMPOSE_INTERACTIVE_NO_CLI=1
docker-compose -f './build/docker-compose-buildagent.yml' up --build -d
docker-compose -f './build/docker-compose-buildagent.yml' exec -e 'ConnectionStrings__SqlServer=Server=sqlserver;User Id=sa;Password=Password12!;Initial Catalog=FunctionalTests;MultipleActiveResultSets=true' functionaltests dotnet test test/FunctionalTests/FunctionalTests.csproj -p:ParallelizeTestCollections=false --logger trx --results-directory /var/temp
docker-compose -f './build/docker-compose-buildagent.yml' down

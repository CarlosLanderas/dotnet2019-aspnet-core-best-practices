FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as build
WORKDIR /src
COPY . .
RUN dotnet restore test/FunctionalTests/FunctionalTests.csproj
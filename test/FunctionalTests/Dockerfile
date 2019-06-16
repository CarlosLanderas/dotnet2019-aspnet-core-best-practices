FROM microsoft/dotnet:2.2-sdk as build
WORKDIR /src
COPY . .
RUN dotnet restore tests/FunctionalTests/FunctionalTests.csproj
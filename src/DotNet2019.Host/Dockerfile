FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/DotNet2019.Host/DotNet2019.Host.csproj", "src/DotNet2019.Host/"]
RUN dotnet restore "src/DotNet2019.Host/DotNet2019.Host.csproj"
COPY . .
WORKDIR "/src/src/DotNet2019.Host"
RUN dotnet build "DotNet2019.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DotNet2019.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DotNet2019.Host.dll"]
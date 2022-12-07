#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TraderPlatform.Daemon/TraderPlatform.Daemon.csproj"

WORKDIR /src/TraderPlatform.Daemon
ARG MODE=Release
RUN dotnet build "TraderPlatform.Daemon.csproj" -c $MODE -o /app/build

FROM build AS publish
ARG MODE=Release
RUN dotnet publish "TraderPlatform.Daemon.csproj" -c $MODE -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TraderPlatform.Daemon.dll"]
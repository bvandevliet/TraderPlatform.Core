#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE ${PORT_ENGINE}

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TraderPlatform.Engine/TraderPlatform.Engine.csproj"

WORKDIR /src/TraderPlatform.Engine
ARG MODE=Release
RUN dotnet build "TraderPlatform.Engine.csproj" -c $MODE -o /app/build

FROM build AS publish
ARG MODE=Release
RUN dotnet publish "TraderPlatform.Engine.csproj" -c $MODE -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TraderPlatform.Engine.dll"]
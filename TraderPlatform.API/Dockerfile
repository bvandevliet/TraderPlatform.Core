#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE ${PORT_API}

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TraderPlatform.API/TraderPlatform.API.csproj"

WORKDIR /src/TraderPlatform.API
ARG MODE=Release
RUN dotnet build "TraderPlatform.API.csproj" -c $MODE -o /app/build

FROM build AS publish
ARG MODE=Release
RUN dotnet publish "TraderPlatform.API.csproj" -c $MODE -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TraderPlatform.API.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameBoulette.Server/GameBoulette.Server.csproj", "GameBoulette.Server/"]
COPY ["GameBoulette.Client/GameBoulette.Client.csproj", "GameBoulette.Client/"]
COPY ["GameBoulette.Client.Model/GameBoulette.Client.Model.csproj", "GameBoulette.Client.Model/"]
COPY ["GameBoulette.Shared/GameBoulette.Shared.csproj", "GameBoulette.Shared/"]
RUN dotnet restore "GameBoulette.Server/GameBoulette.Server.csproj"
COPY . .
WORKDIR "/src/GameBoulette.Server"
RUN dotnet build "GameBoulette.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameBoulette.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameBoulette.Server.dll"]
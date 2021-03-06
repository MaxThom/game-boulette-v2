#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443
ENV ASPNETCORE_URLS=http://*:8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameBoulette.Client/GameBoulette.Client.csproj", "GameBoulette.Client/"]
COPY ["GameBoulette.Client.Model/GameBoulette.Client.Model.csproj", "GameBoulette.Client.Model/"]
COPY ["GameBoulette.Shared/GameBoulette.Shared.csproj", "GameBoulette.Shared/"]
RUN dotnet restore "GameBoulette.Client/GameBoulette.Client.csproj"
COPY . .
WORKDIR "/src/GameBoulette.Client"
RUN dotnet build "GameBoulette.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameBoulette.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameBoulette.Client.dll"]
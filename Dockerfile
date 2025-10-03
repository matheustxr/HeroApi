FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HeroApi.sln", "."]
COPY ["HeroApi/HeroApi.csproj", "HeroApi/"]
COPY ["HeroApi.Tests/HeroApi.Tests.csproj", "HeroApi.Tests/"]

RUN dotnet restore "HeroApi.sln"

COPY . .

WORKDIR "/src/HeroApi"
RUN dotnet publish "HeroApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "HeroApi.dll"]

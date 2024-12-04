# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory to /src
WORKDIR /src

COPY ./cfmg.cafe.manager.sln ./
COPY ./src/cfmg.cafe.manager.application/*.csproj ./src/cfmg.cafe.manager.application/
COPY ./src/cfmg.cafe.manager.common.library/*.csproj ./src/cfmg.cafe.manager.common.library/
COPY ./src/cfmg.cafe.manager.domain/*.csproj ./src/cfmg.cafe.manager.domain/
COPY ./src/cfmg.cafe.manager.infrastructure/*.csproj ./src/cfmg.cafe.manager.infrastructure/

RUN dotnet restore ./cfmg.cafe.manager.sln

COPY ./src/ ./

RUN dotnet publish ./cfmg.cafe.manager.application/cfmg.cafe.manager.application.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

COPY ./src/cfmg.cafe.manager.application/appsettings.json ./appsettings.json

EXPOSE 9090

CMD ["dotnet", "/app/Cfmg.Cafe.Manager.Application.dll"]

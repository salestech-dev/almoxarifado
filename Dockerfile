# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY Almoxarifado.Core/*.csproj Almoxarifado.Core/
COPY Almoxarifado.Web/*.csproj Almoxarifado.Web/

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Almoxarifado.dll"]

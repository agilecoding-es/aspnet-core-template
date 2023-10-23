# Etapa de configuración base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN apt-get update && \
    apt-get install -y curl git build-essential && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g gulp-cli

WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de construcción de la parte web
FROM base AS web-build

WORKDIR /web
COPY package.json .
RUN npm install
COPY Template.MvcWebApp .
RUN npm run build

# Etapa de construcción de la aplicación .NET
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS dotnet-build

WORKDIR /src
COPY ["Template.MvcWebApp/Template.MvcWebApp.csproj", "Template.MvcWebApp/"]
RUN dotnet restore "Template.MvcWebApp/Template.MvcWebApp.csproj"
COPY . .
WORKDIR "/src/Template.MvcWebApp"
RUN dotnet build "Template.MvcWebApp.csproj" -c Release -o /app/build

# Etapa de publicación
FROM dotnet-build AS publish

RUN dotnet publish "Template.MvcWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.MvcWebApp.dll"]

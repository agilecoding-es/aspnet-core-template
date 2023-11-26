# *****************************************************************************************
# Etapa de configuración base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# ----------------------------------------------------------------------------------------
RUN apt-get update && \
    apt-get install -y curl git build-essential && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g gulp-cli

WORKDIR /app
EXPOSE 80
EXPOSE 443

# *****************************************************************************************
# Etapa de construcción de la parte web
FROM base AS web-build
# ----------------------------------------------------------------------------------------
WORKDIR /web
COPY Template.MvcWebApp/package.json .
RUN npm install
COPY Template.MvcWebApp .
RUN npm run build

# *****************************************************************************************
# Etapa de construcción de la aplicación .NET
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# ----------------------------------------------------------------------------------------
WORKDIR /src
COPY ["Template.MvcWebApp/Template.MvcWebApp.csproj", "Template.MvcWebApp/"]
COPY ["Template.Application/Template.Application.csproj", "Template.Application/"]
COPY ["Template.Common/Template.Common.csproj", "Template.Common/"]
COPY ["Template.Configuration/Template.Configuration.csproj", "Template.Configuration/"]
COPY ["Template.Domain/Template.Domain.csproj", "Template.Domain/"]
COPY ["Template.Infrastructure.Mails.AzureCommunicationService/Template.Infrastructure.Mails.AzureCommunicationService.csproj", "Template.Infrastructure.Mails.AzureCommunicationService/"]
COPY ["Template.Infrastructure/Template.Infrastructure.csproj", "Template.Infrastructure/"]
COPY ["Template.Infrastructure.Mails.Smtp/Template.Infrastructure.Mails.Smtp.csproj", "Template.Infrastructure.Mails.Smtp/"]
COPY ["Template.Persistence.Identity/Template.Persistence.Identity.csproj", "Template.Persistence.Identity/"]
COPY ["Template.Persistence/Template.Persistence.csproj", "Template.Persistence/"]
COPY ["Template.Security/Template.Security.csproj", "Template.Security/"]
RUN dotnet restore "Template.MvcWebApp/Template.MvcWebApp.csproj"
COPY . .
WORKDIR "/src/Template.MvcWebApp"
RUN dotnet build "Template.MvcWebApp.csproj" -c Release -o /app/build

# *****************************************************************************************
# Etapa de publicación
FROM build AS publish
# ----------------------------------------------------------------------------------------
RUN dotnet publish "Template.MvcWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN rm -rf /app/build/wwwroot
COPY --from=web-build /web/wwwroot /app/publish/wwwroot

# *****************************************************************************************
# Etapa final
FROM base AS final
# ----------------------------------------------------------------------------------------
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.MvcWebApp.dll"]
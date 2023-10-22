FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN apt-get update && \
    apt-get install -y curl git build-essential && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g gulp-cli


#WORKDIR /app/web/Template.MvcWebApp
#COPY Template.MvcWebApp .
#RUN npm install
#RUN npm run build

WORKDIR /src

# Copia los archivos de proyecto y restaura las dependencias
COPY ["Template.MvcWebApp.IntegrationTests/Template.MvcWebApp.IntegrationTests.csproj", "Template.MvcWebApp.IntegrationTests/"]
COPY ["Template.MvcWebApp/Template.MvcWebApp.csproj", "Template.MvcWebApp/"]
COPY ["Template.Application/Template.Application.csproj", "Template.Application/"]
COPY ["Template.Common/Template.Common.csproj", "Template.Common/"]
COPY ["Template.Configuration/Template.Configuration.csproj", "Template.Configuration/"]
COPY ["Template.Domain/Template.Domain.csproj", "Template.Domain/"]
COPY ["Template.Infrastructure/Template.Infrastructure.csproj", "Template.Infrastructure/"]
COPY ["Template.MailSender/Template.MailSender.csproj", "Template.MailSender/"]
COPY ["Template.Persistence.Identity/Template.Persistence.Identity.csproj", "Template.Persistence.Identity/"]
COPY ["Template.Persistence/Template.Persistence.csproj", "Template.Persistence/"]
COPY ["Template.Security/Template.Security.csproj", "Template.Security/"]

RUN dotnet restore "Template.MvcWebApp.IntegrationTests/Template.MvcWebApp.IntegrationTests.csproj"

# Copia todo el contenido y compila las pruebas de integración
COPY . .

COPY ["./docker-scripts/utils/wait-for-it.sh", "."]
RUN chmod +x wait-for-it.sh

# Cambia al directorio /app/build/Template.MvcWebApp/
WORKDIR /src/Template.MvcWebApp.IntegrationTests
RUN dotnet build "Template.MvcWebApp.IntegrationTests.csproj" -c Debug

# Comando para ejecutar las pruebas de integración
ENTRYPOINT ["dotnet", "test", "/src/Template.MvcWebApp.IntegrationTests/Template.MvcWebApp.IntegrationTests.csproj"]
#ENTRYPOINT ["dotnet", "test", "/src/Template.MvcWebApp.IntegrationTests/bin/Debug/net6.0/Template.MvcWebApp.IntegrationTests.dll"]
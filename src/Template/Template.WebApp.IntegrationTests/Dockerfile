FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN apt-get update && \
    apt-get install -y curl git build-essential && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g gulp-cli

WORKDIR /src

# Copia los archivos de proyecto y restaura las dependencias
COPY ["Template.WebApp.IntegrationTests/Template.WebApp.IntegrationTests.csproj", "Template.WebApp.IntegrationTests/"]
COPY ["Template.WebApp/Template.WebApp.csproj", "Template.WebApp/"]
COPY ["Template.Application/Template.Application.csproj", "Template.Application/"]
COPY ["Template.Application.Features/Template.Application.Features.csproj", "Template.Application.Features/"]
COPY ["Template.Domain/Template.Domain.csproj", "Template.Domain/"]
COPY ["Template.Common/Template.Common.csproj", "Template.Common/"]
COPY ["Template.Configuration/Template.Configuration.csproj", "Template.Configuration/"]
COPY ["Template.Infrastructure/Template.Infrastructure.csproj", "Template.Infrastructure/"]
COPY ["Template.Infrastructure.Mails.AzureCommunicationService/Template.Infrastructure.Mails.AzureCommunicationService.csproj", "Template.Infrastructure.Mails.AzureCommunicationService/"]
COPY ["Template.Infrastructure.Mails.Smtp/Template.Infrastructure.Mails.Smtp.csproj", "Template.Infrastructure.Mails.Smtp/"]
COPY ["Template.Persistence.SqlServer/Template.Persistence.SqlServer.csproj", "Template.Persistence.SqlServer/"]
COPY ["Template.Persistence.Identity.SqlServer/Template.Persistence.Identity.SqlServer.csproj", "Template.Persistence.Identity.SqlServer/"]
COPY ["Template.ExternalServices/Template.ExternalServices.csproj", "Template.ExternalServices/"]
COPY ["Template.Security/Template.Security.csproj", "Template.Security/"]
RUN dotnet restore "Template.WebApp.IntegrationTests/Template.WebApp.IntegrationTests.csproj"

# Copia todo el contenido y compila las pruebas de integración
COPY . .

COPY ["./.scripts/docker/utils/wait-for-it.sh", "."]
RUN chmod +x wait-for-it.sh

# Cambia al directorio /app/build/Template.WebApp/
WORKDIR /src/Template.WebApp.IntegrationTests
RUN dotnet build "Template.WebApp.IntegrationTests.csproj" -c Debug

# Comando para ejecutar las pruebas de integración
ENTRYPOINT ["dotnet", "test", "/src/Template.WebApp.IntegrationTests/Template.WebApp.IntegrationTests.csproj"]
#ENTRYPOINT ["dotnet", "test", "/src/Template.WebApp.IntegrationTests/bin/Debug/net6.0/Template.WebApp.IntegrationTests.dll"]
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install NodeJS
RUN apt-get update && \
    apt-get install -y ca-certificates curl gnupg && \
    mkdir -p /etc/apt/keyrings && \
    curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg
ARG NODE_MAJOR=18
RUN echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list && \
    apt-get update && \
    apt-get install nodejs -y

RUN npm install -g gulp-cli


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
COPY ["Template.Infrastructure.EmailService.AzureCommunicationService/Template.Infrastructure.EmailService.AzureCommunicationService.csproj", "Template.Infrastructure.EmailService.AzureCommunicationService/"]
COPY ["Template.Infrastructure.EmailService.Smtp/Template.Infrastructure.EmailService.Smtp.csproj", "Template.Infrastructure.EmailService.Smtp/"]
COPY ["Template.Infrastructure.Caching/Template.Infrastructure.Caching.csproj", "Template.Infrastructure.Caching/"]
COPY ["Template.Infrastructure.Caching.Redis/Template.Infrastructure.Caching.Redis.csproj", "Template.Infrastructure.Caching.Redis/"]
COPY ["Template.Persistence.SqlServer/Template.Persistence.SqlServer.csproj", "Template.Persistence.SqlServer/"]
COPY ["Template.Persistence.Identity.SqlServer/Template.Persistence.Identity.SqlServer.csproj", "Template.Persistence.Identity.SqlServer/"]
COPY ["Template.Persistence.PostgreSql/Template.Persistence.PostgreSql.csproj", "Template.Persistence.PostgreSql/"]
COPY ["Template.Persistence.Identity.PostgreSql/Template.Persistence.Identity.PostgreSql.csproj", "Template.Persistence.Identity.PostgreSql/"]
COPY ["Template.ExternalServices/Template.ExternalServices.csproj", "Template.ExternalServices/"]
COPY ["Template.ExternalServices.EmailService.Listmonk/Template.ExternalServices.EmailService.Listmonk.csproj", "Template.ExternalServices.EmailService.Listmonk/"]
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
# *****************************************************************************************
# Etapa de configuración base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# ----------------------------------------------------------------------------------------
#RUN apt-get update && \
    #apt-get install -y curl git build-essential && \
    #curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    #apt-get install -y nodejs && \
    #npm install -g gulp-cli
    
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

WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT ${ASPNETCORE_ENVIRONMENT:-Production}

# *****************************************************************************************
# Etapa de construcción de la parte web
FROM base AS web-build
# ----------------------------------------------------------------------------------------
WORKDIR /web
COPY Template.WebApp/package.json .
RUN npm install
COPY Template.WebApp .
RUN if [ "${ASPNETCORE_ENVIRONMENT}" = "Production" ]; then npm run build; else gulp default; fi


# *****************************************************************************************
# Etapa de construcción de la aplicación .NET
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# ----------------------------------------------------------------------------------------
WORKDIR /src
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
COPY ["Template.Persistence.PostgreSql/Template.Persistence.PostgreSql.csproj", "Template.Persistence.PostgreSql/"]
COPY ["Template.Persistence.Identity.PostgreSql/Template.Persistence.Identity.PostgreSql.csproj", "Template.Persistence.Identity.PostgreSql/"]
COPY ["Template.ExternalServices/Template.ExternalServices.csproj", "Template.ExternalServices/"]
COPY ["Template.Security/Template.Security.csproj", "Template.Security/"]
RUN dotnet restore "Template.WebApp/Template.WebApp.csproj"

COPY . .

COPY ["./.scripts/docker/utils/wait-for-it.sh", "."]
RUN chmod +x wait-for-it.sh

WORKDIR "/src/Template.WebApp"
RUN dotnet build "Template.WebApp.csproj" -c Release -o /app/build



# *****************************************************************************************
# Etapa de publicación
FROM build AS publish
# ----------------------------------------------------------------------------------------
RUN dotnet publish "Template.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN rm -rf /app/build/wwwroot
COPY --from=web-build /web/wwwroot /app/publish/wwwroot

# *****************************************************************************************
# Etapa final
FROM base AS final
# ----------------------------------------------------------------------------------------
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.WebApp.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Template.WebApp.UnitTests/Template.WebApp.UnitTests.csproj", "Template.WebApp.UnitTests/"]
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
RUN dotnet restore "Template.WebApp.UnitTests/Template.WebApp.UnitTests.csproj"
COPY . .
WORKDIR "/src/Template.WebApp.UnitTests"
RUN dotnet build "Template.WebApp.UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.WebApp.UnitTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Template.WebApp.UnitTests.dll"]
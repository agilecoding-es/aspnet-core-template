#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Template.MvcWebApp.UnitTests/Template.MvcWebApp.UnitTests.csproj", "Template.MvcWebApp.UnitTests/"]
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
RUN dotnet restore "Template.MvcWebApp.UnitTests/Template.MvcWebApp.UnitTests.csproj"
COPY . .
WORKDIR "/src/Template.MvcWebApp.UnitTests"
RUN dotnet build "Template.MvcWebApp.UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.MvcWebApp.UnitTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Template.MvcWebApp.UnitTests.dll"]
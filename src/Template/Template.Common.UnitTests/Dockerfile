#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Template.Common.UnitTests/Template.Common.UnitTests.csproj", "Template.Common.UnitTests/"]
COPY ["Template.Common/Template.Common.csproj", "Template.Common/"]
COPY ["Template.Configuration/Template.Configuration.csproj", "Template.Configuration/"]
COPY ["Template.Common.UnitTests/entrypoint.sh", "Template.Common.UnitTests/entrypoint.sh"]

RUN dotnet restore "Template.Common.UnitTests/Template.Common.UnitTests.csproj"
COPY . .
WORKDIR "/src/Template.Common.UnitTests"
RUN dotnet build "Template.Common.UnitTests.csproj" -c Release -o /app/build

# Instalar ReportGenerator
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
ENV PATH="$PATH:/root/.dotnet/tools"

FROM build AS test
WORKDIR "/src/Template.Common.UnitTests"
RUN chmod +x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Template.Common.UnitTests/Template.Common.UnitTests.csproj", "Template.Common.UnitTests/"]
COPY ["Template.Common/Template.Common.csproj", "Template.Common/"]
COPY ["Template.Configuration/Template.Configuration.csproj", "Template.Configuration/"]

RUN dotnet restore "Template.Common.UnitTests/Template.Common.UnitTests.csproj"
COPY . .
WORKDIR "/src/Template.Common.UnitTests"
RUN dotnet build "Template.Common.UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.Common.UnitTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Template.Common.UnitTests.dll"]
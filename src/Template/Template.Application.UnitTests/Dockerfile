#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Template.Application.UnitTests/Template.Application.UnitTests.csproj", "Template.Application.UnitTests/"]
RUN dotnet restore "Template.Application.UnitTests/Template.Application.UnitTests.csproj"
COPY . .
WORKDIR "/src/Template.Application.UnitTests"
RUN dotnet build "Template.Application.UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.Application.UnitTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Template.Application.UnitTests.dll"]
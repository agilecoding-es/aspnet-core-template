#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Template.MailSender.UnitTests/Template.MailSender.UnitTests.csproj", "Template.MailSender.UnitTests/"]
COPY ["Template.MailSender/Template.MailSender.csproj", "Template.MailSender/"]
COPY ["Template.Configuration/Template.Configuration.csproj", "Template.Configuration/"]
RUN dotnet restore "Template.MailSender.UnitTests/Template.MailSender.UnitTests.csproj"
COPY . .
WORKDIR "/src/Template.MailSender.UnitTests"
RUN dotnet build "Template.MailSender.UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.MailSender.UnitTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "Template.MailSender.UnitTests.dll"]
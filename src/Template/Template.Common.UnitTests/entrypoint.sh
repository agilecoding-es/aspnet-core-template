#!/bin/bash
set -e

dotnet test Template.Common.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory /var/temp --logger trx

ls -l /var/temp

reportgenerator -reports:/var/temp/coverage.cobertura.xml -targetdir:/var/temp/coverlet/reports

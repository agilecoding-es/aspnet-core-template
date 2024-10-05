#!/bin/bash
set -e

dotnet test Template.Common.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory /var/temp --logger trx

COVERAGE_FILE=$(find /var/temp -name "coverage.cobertura.xml")

if [ -f "$COVERAGE_FILE" ]; then
    reportgenerator -reports:"$COVERAGE_FILE" -targetdir:/var/temp/coverlet/reports -reporttypes:Html
else
    echo "Error: El archivo de cobertura no fue encontrado."
fi

ls -l /var/temp/coverlet/reports
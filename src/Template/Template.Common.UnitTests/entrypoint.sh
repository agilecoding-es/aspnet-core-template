#!/bin/bash
set -e

dotnet test Template.Common.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory /var/temp --logger trx

mkdir -p ./test-results
cp /var/temp/*.trx ./test-results

ls -l ./

COVERAGE_FILE=$(find /var/temp -name "coverage.cobertura.xml")

if [ -f "$COVERAGE_FILE" ]; then
    reportgenerator -reports:"$COVERAGE_FILE" -targetdir:/var/temp/coverlet/reports
else
    echo "Error: El archivo de cobertura no fue encontrado."
fi
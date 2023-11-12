#!/bin/bash

# Ruta del archivo GlobalAssemblyInfo.cs
archivo="./src/Template/GlobalAssemblyInfo.cs"

# Usamos grep para buscar la línea que contiene AssemblyFileVersion y luego cut para extraer el valor
fileVersion=$(grep 'assembly: AssemblyFileVersion' "$archivo" | cut -d '"' -f 2)

# Separamos el valor en sus componentes: major, minor, patch y revision
IFS='.' read -r major minor patch revision <<< "$fileVersion"

# Imprimimos los valores
echo "Major: $major"
echo "Minor: $minor"
echo "Patch: $patch"
echo "Revision: $revision"

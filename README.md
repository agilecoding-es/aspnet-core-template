# Plantilla de proyecto aspnet-core-template

[![Unit & Integration Tests](https://github.com/3panas/aspnet-core-template/actions/workflows/tests.yml/badge.svg)](https://github.com/3panas/aspnet-core-template/actions/workflows/tests.yml) [![Deployment](https://github.com/3panas/aspnet-core-template/actions/workflows/deployment.yml/badge.svg)](https://github.com/3panas/aspnet-core-template/actions/workflows/deployment.yml) [![Ver - Documentaci%C3%B3n](https://img.shields.io/badge/Ver-Documentaci%C3%B3n-blue?style=flat&logo=markdown)](https://github.com/3panas/aspnet-core-template/blob/master/src/Template/Template.Documentation/Documentation.md)

Una vez clonado el proyecto Template, se debe ejecutar el script para renombrado de proyectos y archivos.

El script se encuentra en la carpeta principal del repositorio y se llama `rename-project.ps1`

```Powershell

# Importar el módulo

Import-Module .\rename-project.ps1

# Ejecutar el comando

Rename-Project [-OldName "Template"] -NewName "NombreProyecto"

# Si se quiere descargar el módulo
# Remove-Module rename-project

```

Además habrá que modificar los puertos en los archivos docker-compose.

Para verificar los puertos en uso en PowerShell

```Powershell

Get-NetTCPConnection | Where-Object {$_.State -eq 'Established'} | Select-Object LocalAddress,LocalPort,RemoteAddress,RemotePort

# o bien
netstat -ton

# o bien
netstat -ton | findstr <nro_puerto>

```

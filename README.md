# Plantilla de proyecto aspnet-core-template

[![Unit & Integration Tests](https://github.com/3panas/aspnet-core-template/actions/workflows/tests.yml/badge.svg)](https://github.com/3panas/aspnet-core-template/actions/workflows/tests.yml)

[![Deployment](https://github.com/3panas/aspnet-core-template/actions/workflows/deployment.yml/badge.svg)](https://github.com/3panas/aspnet-core-template/actions/workflows/deployment.yml)


Ver [Documentaci�n](/src/Template/Template.Documentation/Documentation.md "Documentaci�n")

El script se encuentra en la carpeta principal del repositorio y se llama `rename-project.ps1`

```Powershell

# Importar el m�dulo

Import-Module .\rename-project.ps1

# Ejecutar el comando

Rename-Project [-OldName "Template"] -NewName "NombreProyecto"

# Si se quiere descargar el m�dulo
# Remove-Module rename-project

```

Adem�s habr� que modificar los puertos en los archivos docker-compose.

Para verificar los puertos en uso en PowerShell

```Powershell

Get-NetTCPConnection | Where-Object {$_.State -eq 'Established'} | Select-Object LocalAddress,LocalPort,RemoteAddress,RemotePort

# o bien
netstat -ton

# o bien
netstat -ton | findstr <nro_puerto>

```
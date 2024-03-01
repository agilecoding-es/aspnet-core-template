<#
.SYNOPSIS
Rename all directories and files in the project.

.DESCRIPTION
Renames all directories and files in the project that begin with the pattern referred to in the $projectTemplateName parameter and replaces it with what is indicated in the $newName parameter

.PARAMETER newName
New name that you want to give to the project. It will be used to replace folders and files.

.PARAMETER projectTemplateName
Current project name, default Template.

.EXAMPLE
RenameProject -newName Sample
#>
function RenameProject {
    param(
        [string]$newName,
        [string]$projectTemplateName = "Template"
    )
    try {
        if (-not $newName) {
            Write-Host "Please provide the new name as a parameter." -ForegroundColor Yellow
            exit
        }

        $solutionPath = "$PSScriptRoot\src"
        $oldName = "$projectTemplateName*"
        $oldNamePattern = "^$projectTemplateName"

        $folders = Get-ChildItem -Path $solutionPath -Directory -Recurse | Where-Object { 
            $_.Name -like $oldName -and
            $_.Name -notlike "wwwroot" -and
            $_.Name -notlike "bin" -and
            $_.Name -notlike "obj" -and
            $_.Name -notlike "node_modules"
        } | Sort-Object FullName -Descending

        # Rename folders
        Write-Host ""
        Write-Host "Starting to rename folders ..." -ForegroundColor DarkMagenta
        $existFoldersModified = $false
        foreach ($folder in $folders) {
            $newNameFolder = $folder.Name -replace $oldNamePattern, $newName
            $temp = $folder.FullName
            Write-Host "  $newNameFolder `t`t`t(old name: $temp)" -ForegroundColor DarkYellow
            Rename-Item -Path $folder.FullName -NewName $newNameFolder
            if (!$existFoldersModified) {
                $existFoldersModified = $true
            }
        }
        if ($existFoldersModified) {
            Write-Host "Folder renaming completed!" -ForegroundColor DarkMagenta
        }
        else {
            Write-Host "No folders were modified" -ForegroundColor DarkYellow
        }
        Write-Host ""
        $folders = Get-ChildItem -Path $solutionPath -Directory | Where-Object { $_.FullName -inotmatch '.git|bin|obj|node_modules|wwwroot|vendors' }

        # Rename files recursively
        Write-Host "Starting to rename files ..." -ForegroundColor DarkMagenta
        $existfilesModified = $false
        foreach ($folder in $folders) {
            $folderPath = Join-Path -Path $solutionPath -ChildPath $folder.Name
            $files = Get-ChildItem -Path $folderPath -File -Recurse | Where-Object { $_.Name -like $oldName }

            foreach ($file in $files) {
                $newNameFile = $file.Name -replace $oldNamePattern, $newName
                Write-Host "--> $newNameFile `t`t`t(old filename: $file)" -ForegroundColor DarkCyan
                Rename-Item -Path $file.FullName -NewName $newNameFile
                if (!$existfilesModified) {
                    $existfilesModified = $true
                }
            }
        }
        if ($existfilesModified) {
            Write-Host "Files renaming completed!" -ForegroundColor DarkMagenta
        }
        else {
            Write-Host "No files were modified" -ForegroundColor DarkYellow
        }
        Write-Host ""
        Write-Host ""

        if ($existfilesModified -or $existFoldersModified) {
            Write-Host "All folders and files have been renamed correctly." -ForegroundColor Green
        }
    }
    catch {
        Write-Host "Something unexpected happened and the process did not end correctly." -ForegroundColor Red
    }

    <#
.SYNOPSIS
Rename all references to old project name in the code.

.DESCRIPTION
Rename all references to old project name that begin with the pattern referred to in the $projectTemplateName parameter and replaces it with what is indicated in the $newName parameter.

.PARAMETER newName
New name that you want to give to the project. It will be used to replace folders and files.

.PARAMETER projectTemplateName
Current project name, default Template.

.EXAMPLE
RenameReferences -newName Sample
#>
    function RenameReferences {
        param(
            [string]$newName,
            [string]$projectTemplateName = "Template",
            [bool]$listNotModifiedFiles = $false
        )
        try {
            if (-not $newName) {
                Write-Host "Please provide the new name as a parameter." -ForegroundColor Yellow
                exit
            }

            $solutionPath = "$PSScriptRoot\src"

            Write-Host "Folder renaming completed!" -ForegroundColor DarkMagenta
            Write-Host ""
    
            # Rename files recursively
            Write-Host "Starting to replacing files content..." -ForegroundColor DarkMagenta
            $files = Get-ChildItem -Path $solutionPath -Directory -Recurse  | Where-Object { $_.FullName -inotmatch '.git|bin|obj|node_modules|wwwroot|vendors' } | ForEach-Object { Get-ChildItem -Path $_.FullName -File }
    
            $existfilesModified = $false
            foreach ($file in $files) {
                $content = Get-Content -Path $file.FullName
                if ($content -match "$projectTemplateName") {
                    if (!$existfilesModified) {
                        $existfilesModified = $true
                    }
                    $newContent = $content -creplace $projectTemplateName, $newName
                    Set-Content -Path $file.FullName -Value $newContent
                    Write-Host "--> $($file.FullName)" -ForegroundColor DarkCyan
                }
                else {
                    if ( $listNotModifiedFiles ) {
                        Write-Host "--> $($file.FullName)" -ForegroundColor Gray
                    }
                }
            }
            if ($existfilesModified) {
                Write-Host "Files replacing content completed!" -ForegroundColor DarkMagenta
            }
            else {
                Write-Host "No files were modified" -ForegroundColor DarkMagenta
            }
            Write-Host ""
            Write-Host ""

            if ($existfilesModified) {
                Write-Host "All files have been renamed correctly." -ForegroundColor Green
            }
        }
        catch {
            Write-Host "Something unexpected happened and the process did not end correctly." -ForegroundColor Red
        }
    }
}
# Define paths relative to the script location
$solutionRoot = $PSScriptRoot
$engineProj = "$solutionRoot\RogueCustomsGameEngine"
$editorProj = "$solutionRoot\RogueCustomsDungeonEditor"
$clientProj = "$solutionRoot\RogueCustomsGodotClient"
$releaseFolder = "$solutionRoot\Release"
$godotPathFile = "$solutionRoot\godot_path.txt"

# Function to build a C# project
function Build-Project {
    param (
        [string]$projectPath,
        [string]$outputFolder
    )

    Write-Host "Building $projectPath..."

    dotnet build "$projectPath" -c Release -o "$outputFolder"
    if ($?) {
        Write-Host "$projectPath built successfully!"
    } else {
        Write-Host "Failed to build $projectPath!" -ForegroundColor Red
        exit 1
    }
}

# Function to export Godot project
function Export-GodotProject {
    param (
        [string]$projectPath,
        [string]$outputFolder,
        [string]$godotPath
    )

    Write-Host "Exporting $projectPath with Godot executable at $godotPath..."

    $exportCommand = "& `"$godotPath`" --headless --export-release `"Windows Desktop`" `"$outputFolder\Rogue Customs.exe`" --path `"$projectPath`""
    Write-Host "Running command: $exportCommand"

    Invoke-Expression $exportCommand

    if ($?) {
        Write-Host "$projectPath exported successfully!"
        return $true
    } else {
        Write-Host "Failed to export $projectPath!" -ForegroundColor Red
        return $false
    }
}

function Get-GodotPath {
    if (Test-Path $godotPathFile) {
        $storedPath = Get-Content $godotPathFile -Raw
        $storedPath = $storedPath.Trim() # Remove any extra newline or whitespace

        if (Test-Path $storedPath) {
            return $storedPath
        } else {
            Remove-Item $godotPathFile
            Write-Host "Stored Godot executable path is invalid. It will be requested again."
        }
    }
    
    $godotPath = Read-Host "Please provide the path to the Godot executable"
    $godotPath = $godotPath.Trim() # Remove any extra newline or whitespace

    if (Test-Path $godotPath) {
        Set-Content $godotPathFile -Value $godotPath
        return $godotPath
    } else {
        Write-Host "Invalid path provided!" -ForegroundColor Red
        exit 1
    }
}

# Check if Release.old already exists and remove it
if (Test-Path "$solutionRoot\Release.old") {
    Remove-Item "$solutionRoot\Release.old" -Recurse -Force
    Write-Host "Removed existing Release.old folder"
}

# Rename existing Release folder
if (Test-Path $releaseFolder) {
    Rename-Item $releaseFolder "$solutionRoot\Release.old"
    Write-Host "Renamed existing Release folder to Release.old"
}

# Create new Release folder
New-Item -ItemType Directory -Path $releaseFolder
Write-Host "Created new Release folder"

# Build projects in the correct order
Build-Project $engineProj "$engineProj\bin\Release"
Build-Project $editorProj "$releaseFolder"

# Get Godot path and export project
$godotPath = Get-GodotPath
if (-not (Export-GodotProject $clientProj $releaseFolder $godotPath)) {
    Remove-Item $godotPathFile
    Write-Host "Godot export failed. Stored Godot path has been removed."
    exit 1
}

Write-Host "All projects built and exported successfully!"
# Define paths and filenames
$releaseFolder = "Release"
$oldReleaseFolder = "Release.old"
$assemblyInfoPath = "AssemblyInfo.cs"
$solutionFile = "RogueCustoms.sln"  # Replace with your .sln file's path

# Function to update the version in a vdproj file
function Update-vdproj-version([version] $ver, [string] $filename) {
    $productCodePattern     = '\"ProductCode\" = \"8\:{([\d\w-]+)}\"'
    $packageCodePattern     = '\"PackageCode\" = \"8\:{([\d\w-]+)}\"'
    $productVersionPattern  = '\"ProductVersion\" = \"8:[0-9]+(\.([0-9]+)){1,3}\"'
    $productCode            = '"ProductCode" = "8:{' + [guid]::NewGuid().ToString().ToUpper() + '}"'
    $packageCode            = '"PackageCode" = "8:{' + [guid]::NewGuid().ToString().ToUpper() + '}"'
    $productVersion         = '"ProductVersion" = "8:' + $ver.ToString(3) + '"'

    (Get-Content $filename) | ForEach-Object {
        % {$_ -replace $productCodePattern, $productCode } |
        % {$_ -replace $packageCodePattern, $packageCode } |
        % {$_ -replace $productVersionPattern, $productVersion }
    } | Set-Content $filename
}

# Check if the old release folder exists and rename it if needed
if (Test-Path $oldReleaseFolder) {
    Write-Host "Removing existing $oldReleaseFolder folder..."
    Remove-Item $oldReleaseFolder -Force -Recurse
}
if (Test-Path $releaseFolder) {
    Write-Host "Renaming $releaseFolder to $oldReleaseFolder..."
    Rename-Item $releaseFolder $oldReleaseFolder
}

# Create the "Release" folder if it doesn't exist
if (-not (Test-Path $releaseFolder)) {
    Write-Host "Creating $releaseFolder folder..."
    New-Item -ItemType Directory -Path $releaseFolder
}

# Define regular projects
$regularProjects = @(
    @{ Name = "RogueCustomsConsoleClient"; ZipName = "Portable Console Client.zip" },
    @{ Name = "RogueCustomsDungeonEditor"; ZipName = "Portable Dungeon Editor.zip" },
    @{ Name = "RogueCustomsGameEngine"; ZipName = "Game Engine Libraries.zip" },
    @{ Name = "RogueCustomsServer"; ZipName = "Portable Server.zip" }
)

# Define deployment projects
$deploymentProjects = @(
    @{ Name = "RogueCustomsConsoleClientInstaller"; ZipName = "Console Client Installer.zip" },
    @{ Name = "RogueCustomsServerInstaller"; ZipName = "Server Installer.zip" }
)

# Restore NuGet packages
Write-Host "Restoring NuGet packages and dependencies..."
dotnet restore

# Clean the solution
Write-Host "Cleaning the solution..."
msbuild /nologo /verbosity:minimal /t:Clean

# Build and package regular projects
$buildFailed = $false  # Initialize a flag to track build failures
foreach ($project in $regularProjects) {
    $projectName = $project["Name"]
    $zipName = $project["ZipName"]
    
    Write-Host "Building $projectName..."
    
    $buildResult = devenv /build Release "$projectName/$projectName.csproj" /project $projectName

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: Build failed for $projectName."
        $buildFailed = $true  # Set the flag to indicate build failure
        break  # Abort the script if any project fails to build
    }

    Write-Host "$projectName built successfully."
    
    Write-Host "Creating $zipName..."
    # Determine the actual output folder name (either "net6.0" or "net6.0-windows")
    $binPath = Get-ChildItem -Path "$projectName/bin/Release/*" -Directory | Where-Object { $_.Name -match 'net6.0(-windows)?' }
    if ($binPath) {
        $actualOutputFolder = $binPath.Name
    } else {
        Write-Host "Error: Unable to determine the actual output folder for $projectName."
        continue
    }

    # Compress all contents of the actual output folder
    Compress-Archive -Path "$projectName/bin/Release/$actualOutputFolder/*" -DestinationPath "$releaseFolder\$zipName"
    Write-Host "$zipName created at $releaseFolder\$zipName"
}

# Check if any build has failed and abort if necessary
if ($buildFailed) {
    Write-Host "Build process aborted due to build failure."
    Write-Host "Press any key to exit..."
    [Console]::ReadKey() | Out-Null
    return  # Abort the script
}

# Read AssemblyVersion from AssemblyInfo.cs
$assemblyInfoContent = Get-Content $assemblyInfoPath
$pattern = '\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyInfoContent | ForEach-Object{
    if($_ -match $pattern){
        # We have found the matching line
        # Edit the version number and put back.
        $version = [version]$matches[1]
    }
}
Write-Host "AssemblyVersion: $version"

# Build and package deployment projects
foreach ($project in $deploymentProjects) {
    $projectName = $project["Name"]
    $zipName = $project["ZipName"]    
    
    Update-vdproj-version $version "$projectName/$projectName.vdproj"

    Write-Host "Building $projectName..."

    $buildResult = devenv /build Release "$projectName/$projectName.vdproj" /project $projectName

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: Build failed for $projectName."
        Write-Host "Press any key to exit..."
        [Console]::ReadKey() | Out-Null
        return  # Abort the script if any project fails to build
    }
    Write-Host "$projectName built successfully."
    
    Write-Host "Creating $zipName..."
    Compress-Archive -Path "$projectName/Release/*" -DestinationPath "$releaseFolder\$zipName"
    Write-Host "$zipName created at $releaseFolder\$zipName"
}

Write-Host "Release packages created successfully."
Write-Host "Press any key to exit..."
[Console]::ReadKey() | Out-Null
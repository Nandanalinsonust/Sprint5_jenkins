param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$Root = $PSScriptRoot

Write-Host ""
Write-Host "============================================"
Write-Host " HealthAxis Admin Blazor -> API wwwroot copy"
Write-Host "============================================"
Write-Host ""
Write-Host "Root folder: $Root"
Write-Host ""

$BlazorProject = Get-ChildItem -Path $Root -Recurse -Filter "HealthAxis.Admin.csproj" | Select-Object -First 1
$ApiProject = Get-ChildItem -Path $Root -Recurse -Filter "HealthAxis.Api.csproj" | Select-Object -First 1

if ($null -eq $BlazorProject) {
    throw "Could not find HealthAxis.Admin.csproj under $Root"
}

if ($null -eq $ApiProject) {
    throw "Could not find HealthAxis.Api.csproj under $Root"
}

$BlazorProjectPath = $BlazorProject.FullName
$ApiProjectRoot = $ApiProject.Directory.FullName

$ApiWwwroot = Join-Path $ApiProjectRoot "wwwroot"
$BlazorTarget = Join-Path $ApiWwwroot "blazor"

$ArtifactsRoot = Join-Path $Root "artifacts"
$PublishDir = Join-Path $ArtifactsRoot "HealthAxis.Admin.publish"

Write-Host "Blazor project: $BlazorProjectPath"
Write-Host "API project root: $ApiProjectRoot"
Write-Host "API wwwroot: $ApiWwwroot"
Write-Host "Blazor target: $BlazorTarget"
Write-Host ""

if (Test-Path $PublishDir) {
    Write-Host "Cleaning old publish directory..."
    Remove-Item $PublishDir -Recurse -Force
}

if (-not (Test-Path $ArtifactsRoot)) {
    New-Item -ItemType Directory -Path $ArtifactsRoot | Out-Null
}

Write-Host ""
Write-Host "Publishing Blazor Admin..."
Write-Host ""

dotnet publish $BlazorProjectPath -c $Configuration -o $PublishDir

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed."
}

$PublishedWwwroot = Join-Path $PublishDir "wwwroot"
$PublishedBlazorFolder = Join-Path $PublishedWwwroot "blazor"

if (-not (Test-Path $PublishedBlazorFolder)) {
    throw "Published Blazor folder not found: $PublishedBlazorFolder"
}

if (-not (Test-Path (Join-Path $PublishedBlazorFolder "index.html"))) {
    throw "index.html not found inside: $PublishedBlazorFolder"
}

$BlazorSource = $PublishedBlazorFolder

Write-Host "Blazor source: $BlazorSource"

if (-not (Test-Path $ApiWwwroot)) {
    Write-Host "Creating API wwwroot..."
    New-Item -ItemType Directory -Path $ApiWwwroot | Out-Null
}

if (Test-Path $BlazorTarget) {
    Write-Host "Deleting old API wwwroot/blazor..."
    Remove-Item $BlazorTarget -Recurse -Force
}

Write-Host "Creating fresh API wwwroot/blazor..."
New-Item -ItemType Directory -Path $BlazorTarget | Out-Null

Write-Host ""
Write-Host "Copying Blazor files..."
Write-Host "From: $BlazorSource"
Write-Host "To:   $BlazorTarget"
Write-Host ""

Copy-Item -Path (Join-Path $BlazorSource "*") -Destination $BlazorTarget -Recurse -Force

Write-Host ""
Write-Host "Verifying required files..."
Write-Host ""

$RequiredFiles = @(
    "index.html",
    "_framework\blazor.webassembly.js",
    "css\app.css",
    "css\admin-theme.css",
    "HealthAxis.Admin.styles.css",
    "lib\bootstrap\dist\css\bootstrap.min.css"
)

$MissingFiles = @()

foreach ($file in $RequiredFiles) {
    $fullPath = Join-Path $BlazorTarget $file

    if (Test-Path $fullPath) {
        Write-Host "OK      $file" -ForegroundColor Green
    }
    else {
        Write-Host "MISSING $file" -ForegroundColor Red
        $MissingFiles += $file
    }
}

Write-Host ""

if ($MissingFiles.Count -gt 0) {
    Write-Host "Copy finished, but files are missing." -ForegroundColor Red
    Write-Host "Missing files:" -ForegroundColor Red

    foreach ($missingFile in $MissingFiles) {
        Write-Host " - $missingFile" -ForegroundColor Red
    }

    exit 1
}

Write-Host "SUCCESS: Blazor files copied to API wwwroot/blazor." -ForegroundColor Green
Write-Host ""
Write-Host "Now restart API and test:"
Write-Host "https://localhost:7075/blazor/"
Write-Host "https://localhost:7075/blazor/_framework/blazor.webassembly.js"
Write-Host ""
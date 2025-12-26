# Setup script for SR2MP - Copies required DLLs from Slime Rancher 2

Write-Host "Setting up SR2MP libraries..." -ForegroundColor Cyan
Write-Host ""

# Default Steam path for Slime Rancher 2
$SR2_PATH = "C:\Program Files (x86)\Steam\steamapps\common\Slime Rancher 2"
$LIBS_PATH = "SR2MP\libraries"

# Check if Slime Rancher 2 is installed
if (-not (Test-Path $SR2_PATH)) {
    Write-Host "ERROR: Slime Rancher 2 not found at default location!" -ForegroundColor Red
    Write-Host "Please edit this script and set `$SR2_PATH to your game installation folder."
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "Found Slime Rancher 2 at: $SR2_PATH" -ForegroundColor Green
Write-Host ""

# Create libraries folder
if (-not (Test-Path $LIBS_PATH)) {
    New-Item -ItemType Directory -Force -Path $LIBS_PATH | Out-Null
    Write-Host "Created libraries folder"
}

Write-Host "Copying DLLs..." -ForegroundColor Cyan
Write-Host ""

# Copy MelonLoader DLLs
Write-Host "[1/3] Copying MelonLoader DLLs..."
try {
    Copy-Item "$SR2_PATH\MelonLoader\*.dll" -Destination $LIBS_PATH -Force -ErrorAction Stop
    Write-Host "  ✓ MelonLoader DLLs copied" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to copy MelonLoader DLLs" -ForegroundColor Red
}

# Copy Managed DLLs (Unity + Game)
Write-Host "[2/3] Copying Unity and Game DLLs..."
try {
    Copy-Item "$SR2_PATH\SlimeRancher2_Data\Managed\*.dll" -Destination $LIBS_PATH -Force -ErrorAction Stop
    Write-Host "  ✓ Managed DLLs copied" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to copy Managed DLLs" -ForegroundColor Red
}

# Copy SR2E dependency
Write-Host "[3/3] Checking for SR2E mod..."
if (Test-Path "$SR2_PATH\Mods\SR2E.dll") {
    Copy-Item "$SR2_PATH\Mods\SR2E.dll" -Destination $LIBS_PATH -Force
    Write-Host "  ✓ SR2E.dll copied" -ForegroundColor Green
} else {
    Write-Host "  ✗ WARNING: SR2E.dll not found!" -ForegroundColor Yellow
    Write-Host "  You need to install SR2E (Slime Rancher 2 Essentials) mod first." -ForegroundColor Yellow
    Write-Host "  Download from: https://www.nexusmods.com/slimerancher2/mods/" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Setup complete!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "You can now build the project with:"
Write-Host "  dotnet build SR2MP\SR2MP.csproj -c Release" -ForegroundColor White
Write-Host ""
Write-Host "Or use the build script:"
Write-Host "  .\build.bat" -ForegroundColor White
Write-Host ""

Read-Host "Press Enter to continue"

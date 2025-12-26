@echo off
REM Setup script for SR2MP - Copies required DLLs from Slime Rancher 2

REM Navigate to script directory
cd /d "%~dp0"

echo Setting up SR2MP libraries...
echo.

REM Default Steam path for Slime Rancher 2
set "SR2_PATH=C:\Program Files (x86)\Steam\steamapps\common\Slime Rancher 2"
set "LIBS_PATH=SR2MP\libraries"

REM Verify SR2MP folder exists
if not exist "SR2MP" (
    echo ERROR: SR2MP folder not found!
    echo Make sure you're running this script from the project root directory.
    echo.
    pause
    exit /b 1
)

REM Check if Slime Rancher 2 is installed
if not exist "%SR2_PATH%" (
    echo ERROR: Slime Rancher 2 not found at default location!
    echo Please edit this script and set SR2_PATH to your game installation folder.
    echo.
    pause
    exit /b 1
)

echo Found Slime Rancher 2 at: %SR2_PATH%
echo.

REM Create libraries folder
if not exist "%LIBS_PATH%" (
    mkdir "%LIBS_PATH%"
    echo Created libraries folder
)

echo Copying DLLs to: %CD%\%LIBS_PATH%
echo.

REM Copy MelonLoader DLLs
echo [1/3] Copying MelonLoader DLLs...
copy /Y "%SR2_PATH%\MelonLoader\*.dll" "%LIBS_PATH%\" >nul
if %ERRORLEVEL% EQU 0 (
    echo   ✓ MelonLoader DLLs copied
) else (
    echo   ✗ Failed to copy MelonLoader DLLs
)

REM Copy Managed DLLs (Unity + Game)
echo [2/3] Copying Unity and Game DLLs...
copy /Y "%SR2_PATH%\SlimeRancher2_Data\Managed\*.dll" "%LIBS_PATH%\" >nul
if %ERRORLEVEL% EQU 0 (
    echo   ✓ Managed DLLs copied
) else (
    echo   ✗ Failed to copy Managed DLLs
)

REM Copy SR2E dependency
echo [3/3] Checking for SR2E mod...
if exist "%SR2_PATH%\Mods\SR2E.dll" (
    copy /Y "%SR2_PATH%\Mods\SR2E.dll" "%LIBS_PATH%\" >nul
    echo   ✓ SR2E.dll copied
) else (
    echo   ✗ WARNING: SR2E.dll not found!
    echo   You need to install SR2E (Slime Rancher 2 Essentials) mod first.
    echo   Download from: https://www.nexusmods.com/slimerancher2/mods/
)

echo.
echo ============================================
echo Setup complete!
echo ============================================
echo.
echo You can now build the project with:
echo   dotnet build SR2MP\SR2MP.csproj -c Release
echo.
echo Or use the build script:
echo   build.bat
echo.

pause

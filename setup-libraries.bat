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
echo [1/3] Checking MelonLoader folder...
if exist "%SR2_PATH%\MelonLoader\" (
    echo   Found MelonLoader folder, copying DLLs...
    copy /Y "%SR2_PATH%\MelonLoader\*.dll" "%LIBS_PATH%\" >nul 2>&1
    if %ERRORLEVEL% EQU 0 (
        echo   [OK] MelonLoader DLLs copied
    ) else (
        echo   [WARN] Failed to copy some MelonLoader DLLs
    )
) else (
    echo   [ERROR] MelonLoader folder not found!
    echo   MelonLoader is not installed. Please install MelonLoader first:
    echo   https://github.com/LavaGang/MelonLoader/releases
    pause
    exit /b 1
)

REM Copy Managed DLLs (Unity + Game)
echo [2/3] Checking Managed folder...
if exist "%SR2_PATH%\SlimeRancher2_Data\Managed\" (
    echo   Found Managed folder, copying DLLs...
    copy /Y "%SR2_PATH%\SlimeRancher2_Data\Managed\*.dll" "%LIBS_PATH%\" >nul 2>&1
    if %ERRORLEVEL% EQU 0 (
        echo   [OK] Managed DLLs copied
    ) else (
        echo   [WARN] Failed to copy some Managed DLLs
    )
) else (
    echo   [ERROR] Managed folder not found!
    echo   Path checked: %SR2_PATH%\SlimeRancher2_Data\Managed\
    pause
    exit /b 1
)

REM Copy SR2E dependency
echo [3/3] Checking for SR2E mod...
set "SR2E_PATH=%SR2_PATH%\Mods\SR2E.dll"
if exist "%SR2E_PATH%" (
    copy /Y "%SR2E_PATH%" "%LIBS_PATH%\" >nul 2>&1
    echo   [OK] SR2E.dll copied
) else (
    echo   [WARN] SR2E.dll not found
    echo   You need to install SR2E ^(Slime Rancher 2 Essentials^) mod.
    echo   Download from: https://www.nexusmods.com/slimerancher2/mods/
)

echo.
echo Verifying DLLs...
dir /b "%LIBS_PATH%\*.dll" 2>nul | find /c /v "" > temp_count.txt
set /p DLL_COUNT=<temp_count.txt
del temp_count.txt

echo.
echo ============================================
echo Setup complete!
echo ============================================
echo   DLLs copied: %DLL_COUNT%
echo   Location: %CD%\%LIBS_PATH%
echo.
echo You can now build the project with:
echo   dotnet build SR2MP\SR2MP.csproj -c Release
echo.
echo Or use the build script:
echo   build.bat
echo.

pause

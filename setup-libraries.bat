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
echo [1/5] Checking MelonLoader folder...
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

REM Copy .NET 6 Runtime DLLs (Core MelonLoader for .NET 6)
echo [2/5] Checking MelonLoader net6 folder...
if exist "%SR2_PATH%\MelonLoader\net6\" (
    echo   Found net6 folder, copying DLLs...
    copy /Y "%SR2_PATH%\MelonLoader\net6\*.dll" "%LIBS_PATH%\" >nul 2>&1
    echo   [OK] .NET 6 runtime DLLs copied
) else (
    echo   [WARN] net6 folder not found
)

REM Copy Il2Cpp Assemblies (Game assemblies converted from Il2Cpp)
echo [3/5] Checking Il2CppAssemblies folder...
if exist "%SR2_PATH%\MelonLoader\Il2CppAssemblies\" (
    echo   Found Il2CppAssemblies folder, copying DLLs...
    copy /Y "%SR2_PATH%\MelonLoader\Il2CppAssemblies\*.dll" "%LIBS_PATH%\" >nul 2>&1
    echo   [OK] Il2Cpp assemblies copied
) else (
    echo   [WARN] Il2CppAssemblies not found - may need to run SR2 once with MelonLoader
)

REM Copy Managed Framework DLLs
echo [4/5] Checking MelonLoader Managed folder...
if exist "%SR2_PATH%\MelonLoader\Managed\" (
    echo   Found Managed folder, copying DLLs...
    copy /Y "%SR2_PATH%\MelonLoader\Managed\*.dll" "%LIBS_PATH%\" >nul 2>&1
    echo   [OK] Managed framework DLLs copied
) else (
    echo   [WARN] Managed folder not found
)

REM Copy SR2E dependency
echo [5/5] Checking for SR2E mod...
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

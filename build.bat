@echo off
REM Build script for SR2MP (Windows)

echo Building SR2MP in Release mode...
echo.

REM Navigate to project directory
cd /d "%~dp0"

REM Clean previous builds
dotnet clean SR2MP\SR2MP.csproj

REM Build in Release configuration
dotnet build SR2MP\SR2MP.csproj -c Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Build successful!
    echo 📦 DLL location: SR2MP\bin\Release\net6.0\SR2MP.dll
    echo.

    REM Optional: Copy to Slime Rancher 2 mods folder
    REM Uncomment and modify path as needed:
    REM set SR2_MODS_PATH=C:\Program Files (x86)\Steam\steamapps\common\Slime Rancher 2\Mods
    REM if exist "%SR2_MODS_PATH%" (
    REM     copy /Y SR2MP\bin\Release\net6.0\SR2MP.dll "%SR2_MODS_PATH%\"
    REM     echo ✅ Copied to Slime Rancher 2 Mods folder
    REM )
) else (
    echo ❌ Build failed!
    exit /b 1
)

pause

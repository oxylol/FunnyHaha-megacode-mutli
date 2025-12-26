@echo off
REM Diagnostic script to find Slime Rancher 2 folder structure

cd /d "%~dp0"

echo Slime Rancher 2 Folder Structure Diagnostic
echo =============================================
echo.

set "SR2_PATH=C:\Program Files (x86)\Steam\steamapps\common\Slime Rancher 2"

if not exist "%SR2_PATH%" (
    echo ERROR: Slime Rancher 2 not found at: %SR2_PATH%
    echo.
    set /p SR2_PATH="Enter the path to your Slime Rancher 2 installation: "
)

echo Checking: "%SR2_PATH%"
echo.

echo Directory structure:
echo --------------------
dir /b /ad "%SR2_PATH%" 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Cannot access directory
    pause
    exit /b 1
)
echo.

echo Checking for Data folders:
echo --------------------------
if exist "%SR2_PATH%\SlimeRancher2_Data\" (
    echo [FOUND] SlimeRancher2_Data
    echo   Subfolders:
    dir /b /ad "%SR2_PATH%\SlimeRancher2_Data" 2>nul
) else (
    echo [NOT FOUND] SlimeRancher2_Data
)
echo.

if exist "%SR2_PATH%\Slime Rancher 2_Data\" (
    echo [FOUND] Slime Rancher 2_Data ^(with space^)
    echo   Subfolders:
    dir /b /ad "%SR2_PATH%\Slime Rancher 2_Data" 2>nul
) else (
    echo [NOT FOUND] Slime Rancher 2_Data
)
echo.

echo Checking for MelonLoader:
echo -------------------------
if exist "%SR2_PATH%\MelonLoader\" (
    echo [FOUND] MelonLoader folder
    for /f %%A in ('dir /b "%SR2_PATH%\MelonLoader\*.dll" 2^>nul ^| find /c /v ""') do set DLL_COUNT=%%A
    echo   %DLL_COUNT% DLL files found
) else (
    echo [NOT FOUND] MelonLoader folder
)
echo.

echo Checking for DLLs in root:
echo --------------------------
dir /b "%SR2_PATH%\*.dll" 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo No DLLs found in root folder
)
echo.

pause

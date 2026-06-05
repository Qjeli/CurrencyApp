@echo off
REM CurrencyApp - Installer
setlocal enabledelayedexpansion
cls
echo.
echo ====================================
echo   CurrencyApp Installation
echo ====================================
echo.
set PACKAGE_FILE=
for %%F in (*.msixbundle) do set PACKAGE_FILE=%%F
if not defined PACKAGE_FILE (
    for %%F in (*.appx) do set PACKAGE_FILE=%%F
)
if not defined PACKAGE_FILE (
    echo ERROR: Package file not found
    pause
    exit /b 1
)
echo Found: !PACKAGE_FILE!
echo Starting installation...
echo.
powershell -Command "Add-AppxPackage -Path '!PACKAGE_FILE!' -ForceApplicationShutdown"
echo.
echo Installation Complete! Find CurrencyApp in Start Menu.
echo.
pause

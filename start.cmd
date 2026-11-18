@echo off
setlocal
chcp 65001 >nul
title DynamicStructures
cd /d "%~dp0"

dotnet run --project "%~dp0DynamicStructures\DynamicStructures.csproj" --configuration Release --disable-build-servers --artifacts-path "%~dp0artifacts\launcher"
set "appExitCode=%ERRORLEVEL%"
if not "%appExitCode%"=="0" (
    echo.
    echo Не удалось запустить программу. Для сборки нужен .NET SDK 10.
    pause
)
exit /b %appExitCode%

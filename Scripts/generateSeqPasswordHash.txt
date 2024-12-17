@echo off
if "%~1"=="" (
    echo Uzycie: %~nx0 [haslo]
    exit /b 1
)

set "PASSWORD=%~1"
echo -n "%PASSWORD%" | docker run --rm -i datalust/seq config hash
@echo off
setlocal enabledelayedexpansion

cd ..\

set "ENV_FILE=.env"

set "CERT_DIR=.containers\medicalassistant.api\https"
set "CERT_PATH=%CERT_DIR%\medicalassistant.api.pfx"

if not exist "%ENV_FILE%" (
    echo Plik .env nie zostal znaleziony w sciezce: %ENV_FILE%.
    exit /b 1
)

REM Tworzenie folderu, jeśli nie istnieje
if not exist "%CERT_DIR%" (
    mkdir "%CERT_DIR%"
    if errorlevel 1 (
        echo Wystapil blad podczas tworzenia folderu: %CERT_DIR%.
        exit /b %errorlevel%
    )
)

for /f "usebackq tokens=1,2 delims==" %%i in (`type "%ENV_FILE%"`) do (
    if "%%i"=="API_SSL_PASSWORD" set "API_SSL_PASSWORD=%%j"
)

if not defined API_SSL_PASSWORD (
    echo Nie znaleziono zmiennej API_SSL_PASSWORD w pliku .env.
    exit /b 1
)

dotnet dev-certs https -ep "%CERT_PATH%" -p "%API_SSL_PASSWORD%"
if errorlevel 1 (
    echo Wystapil blad podczas wykonywania pierwszej komendy dotnet.
    exit /b %errorlevel%
)

REM Dodanie opóźnienia na potwierdzenie certyfikatu
echo Proszę kliknąć "Zaufaj" w oknie, które powinno się pojawić, aby zaufać certyfikatowi.
pause

dotnet dev-certs https --trust
if errorlevel 1 (
    echo Wystapil blad podczas wykonywania drugiej komendy dotnet.
    exit /b %errorlevel%
)

echo Operacja zakonczona pomyslnie. Certyfikat zostal wygenerowany w lokalizacji: %CERT_PATH%
exit /b 0

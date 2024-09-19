@echo off
REM Skrypt do znalezienia IP kontenera PostgreSQL

REM Wykonanie polecenia docker ps i zapisanie wyniku do pliku tymczasowego
docker ps > temp_docker_ps.txt

REM Wyszukiwanie ID kontenera PostgreSQL
for /f "tokens=1" %%a in ('findstr "postgres" temp_docker_ps.txt') do (
    set "postgresContainerId=%%a"
)

REM Sprawdzenie, czy kontener został znaleziony
if defined postgresContainerId (
    echo Znaleziono kontener PostgreSQL o ID: %postgresContainerId%

    REM Uzyskiwanie adresu IP kontenera
    for /f "delims=" %%b in ('docker inspect %postgresContainerId% ^| findstr /C:"\"IPAddress\""') do (
        for /f "tokens=2 delims=:" %%c in (%%b) do (
            set "postgresIpAddress=%%~c"
        )
    )

    REM Usunięcie zbędnych spacji
    set "postgresIpAddress=%postgresIpAddress:~1,-1%"

    REM Wyświetlenie adresu IP
    echo Adres IP kontenera PostgreSQL: %postgresIpAddress%
) else (
    echo Nie znaleziono kontenera PostgreSQL.
)

REM Usunięcie pliku tymczasowego
del temp_docker_ps.txt
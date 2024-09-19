@echo off
docker ps

for /f "tokens=1" %%i in ('docker ps -q --filter "ancestor=postgres"') do set CONTAINER_ID=%%i

for /f "tokens=*" %%i in ('docker inspect --format="{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" %CONTAINER_ID%') do set IP_ADDRESS=%%i

echo PostgreSQL container IP address: %IP_ADDRESS%
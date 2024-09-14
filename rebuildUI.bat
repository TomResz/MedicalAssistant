@echo off

docker-compose up -d --force-recreate --no-deps --build medicalassistant.ui

docker-compose down

echo UI Rebuilded

pause
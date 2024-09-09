@echo off

docker-compose up -d --force-recreate --no-deps --build medicalassist.ui

docker-compose down

echo UI Rebuilded

pause
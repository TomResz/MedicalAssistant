@echo off

docker-compose up -d --force-recreate --no-deps --build medicalassist.api

docker-compose down

echo API Rebuilded

pause
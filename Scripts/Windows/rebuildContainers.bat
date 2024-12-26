@echo off

echo Rebuilding API
docker-compose up -d --force-recreate --no-deps --build medicalassistant.api

echo  Rebuilding UI
docker-compose up -d --force-recreate --no-deps --build medicalassistant.ui

docker-compose down

docker-compose --project-name medicalassistant up -d

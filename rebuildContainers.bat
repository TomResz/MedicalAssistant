@echo off

echo Rebuilding API
docker-compose up -d --force-recreate --no-deps --build medicalassist.api

echo  Rebuilding UI
docker-compose up -d --force-recreate --no-deps --build medicalassist.ui

docker-compose down

docker-compose --project-name medicalassist up -d
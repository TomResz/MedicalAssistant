#!/bin/bash

docker-compose up -d --force-recreate --no-deps --build medicalassistant.api

docker-compose down

echo API Rebuilded

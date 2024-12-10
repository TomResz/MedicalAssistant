@echo off
cd /d "%~dp0"
call "generateSSL.bat"
call "runDockerContainers.bat"
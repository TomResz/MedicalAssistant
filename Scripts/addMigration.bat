@echo off
SETLOCAL

IF "%~1"=="" (
    echo U¿ycie: add_migration.bat <nazwa_migracji>
    exit /b 1
)

SET MIGRATION_NAME=%~1

dotnet ef migrations add %MIGRATION_NAME% --verbose --project MedicalAssistant.Infrastructure --startup-project MedicalAssistant.API

ENDLOCAL
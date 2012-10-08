SETLOCAL

@ECHO OFF
CLS

SET COMPRESSED_BACKUP_FILE=V1SDKTests_sql_selfextract.exe
SET BACKUP_FILE=V1SDKTests.sql
SET DB_NAME=V1SDKTests
SET INSTANCE_NAME=V1SDKTests
SET V1_SETUP=VersionOne.Setup-Ultimate-latest.exe
SET RESTORE_DB=true
SET DB_SERVER=(local)

IF true==%RESTORE_DB% CALL :install_db

ECHO Installing VersionOne at http://localhost/%INSTANCE_NAME% associated with DB Server: %DB_SERVER% and DB Name: %DB_NAME%...
START /WAIT %V1_SETUP% -Quiet:2 -DbServer:%DB_SERVER% -LogFile:%WORKSPACE%\setup.log -DBName:%DB_NAME% %INSTANCE_NAME%
if %errorlevel% neq 0 exit /b %errorlevel%

GOTO end


:install_db

ECHO Unpacking database backup from Self-Extracting archive...
CALL V1SDKTests_sql_selfextract.exe -y

ECHO Restoring database...
CALL %SQLCMD% -S %DB_SERVER% -E "DROP DATABASE [%DB_NAME%]"
CALL %SQLCMD% -S %DB_SERVER% -E "CREATE DATABASE [%DB_NAME%]"
CALL %SQLCMD% -S %DB_SERVER% -E -i %BACKUP_FILE%

ECHO Removing database backup file
DEL V1SDKTests.sql

GOTO :EOF

:end

ECHO Thank you, you can browse to http://localhost/%INSTANCE_NAME% and login with admin / admin, and you should be able to execute the integration tests now...

ENDLOCAL

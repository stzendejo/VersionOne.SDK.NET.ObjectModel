SETLOCAL

SET INSTANCE_NAME=V1SDKTests
SET V1_SETUP=VersionOne.Setup-Ultimate-latest.exe

ECHO Removing previous istallation
START /WAIT %V1_SETUP% -DeleteDatabase -LogFile:%WORKSPACE%\setup.log -Quiet:2 -u %INSTANCE_NAME%


ENDLOCAL
# You need VersionOne.Setup-*.exe in ____ directory
# you need VersionOne.SDK.Net*.dll in ____ directory (provided by nuget maybe)
# You need TestSetup/* stuff (sql DB, scripts)

export WORKSPACE=`pwd`
export BUILD_TAG=V1SDKTests_`date +%Y%m%dT%H%M%S`
export TEST_URL=http://localhost/$BUILD_TAG/

# Make sure the nunit-console is available first...
NUNIT_CONSOLE_RUNNER=`find packages | grep nunit-console.exe$`
if [ -z "$NUNIT_CONSOLE_RUNNER" ]
then
	echo "Could not find nunint-console.exe in the packages folder. Before running this script, you must build the tests in Release mode in Visual Studio or by typing buildLocal.bat from the ObjectModel.Tests directory."
	cd $WORKSPACE
	return -1
fi

cd $WORKSPACE

# Get setup app
$WORKSPACE/TestSetup/copy_latest_setup_to_standard_name.sh

# Prepare for test
$WORKSPACE/TestSetup/restore_db_and_install_v1.sh

export APP_POOL=V1-Core-$BUILD_TAG

#echo "C:\Windows\system32\inetsrv\appcmd.exe set apppool "$APP_POOL" /enable32BitAppOnWin64:true" >enable32BitInAppPool.bat
#cmd \\/c enable32BitInAppPool.bat
#rm enable32BitInAppPool.bat

cmd \\/c pause

# Run tests

# Note: Extra IIS stuff to run before NUnit if needed...
#echo "C:\Windows\system32\inetsrv\appcmd.exe recycle apppool /apppool.name:V1-Core-$BUILD_TAG && pause" >recycle_iis_apppool.cmd
#cmd \\/c recycle_iis_apppool.cmd

cd $WORKSPACE
$NUNIT_CONSOLE_RUNNER //framework:net-4.0 //labels //stoponerror ${WORKSPACE}\\ObjectModel.Tests\\bin\\Release\\VersionOne.SDK.ObjectModel.Tests.dll //xml=run_integration_tests_results.xml

# Clean up
$WORKSPACE/TestSetup/remove_v1_and_delete_db.sh
echo "C:\Windows\system32\inetsrv\appcmd.exe delete apppool "$APP_POOL"" >deleteAppPool.bat
cmd \\/c deleteAppPool.bat
rm deleteAppPool.bat

cd $WORKSPACE
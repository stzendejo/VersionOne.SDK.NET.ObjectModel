# You need VersionOne.Setup-Ultimate*.exe in ____ directory
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

# Run tests

# Note: Extra IIS stuff to run before NUnit if needed...
#echo "C:\Windows\system32\inetsrv\appcmd.exe recycle apppool /apppool.name:V1-Core-$BUILD_TAG && pause" >recycle_iis_apppool.cmd
#cmd \\/c recycle_iis_apppool.cmd

cd $WORKSPACE
$NUNIT_CONSOLE_RUNNER //framework:net-4.0 //labels //stoponerror ${WORKSPACE}\\ObjectModel.Tests\\bin\\Release\\VersionOne.SDK.ObjectModel.Tests.dll //xml=run_integration_tests_results.xml

# Clean up
$WORKSPACE/TestSetup/remove_v1_and_delete_db.sh
cd $WORKSPACE
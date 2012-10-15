# you need VersionOne.Setup-Ultimate*.exe in ____ directory
# you need VersionOne.SDK.Net*.dll in ____ directory (provided by nuget maybe)
# you need TestSetup/* stuff (sql DB, scripts)

export WORKSPACE=`pwd`
export BUILD_TAG=manual_`date +%Y%m%dT%H%M%S`
export TEST_URL=http://localhost/%BUILD_TAG%/

# Get setup app
$WORKSPACE/TestSetup/copy_latest_setup_to_standard_name.sh

# Prepare for test
$WORKSPACE/TestSetup/restore_db_and_install_v1.sh

# Run tests
$WORKSPACE/packages/NUnit.Runners.2.6.1/tools/nunit-console.exe /framework:net-4.0 /labels /stoponerror ${WORKSPACE}\\ObjectModel.Tests\\bin\\Debug\\VersionOne.SDK.ObjectModel.Tests.dll /xml=nunit-objmodel-result.xml

# Clean up
$WORKSPACE/TestSetup/remove_v1_and_delete_db.sh
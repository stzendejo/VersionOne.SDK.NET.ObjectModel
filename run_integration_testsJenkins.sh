export TEST_URL=http://localhost/$BUILD_TAG/

$WORKSPACE/TestSetup/copy_latest_setup_to_standard_name.sh
$WORKSPACE/TestSetup/restore_db_and_install_v1.sh

NUNIT_CONSOLE_RUNNER=`/usr/bin/find packages | grep nunit-console.exe$`
$NUNIT_CONSOLE_RUNNER //framework:net-4.0 //labels //stoponerror ${WORKSPACE}\\ObjectModel.Tests\\bin\\Release\\VersionOne.SDK.ObjectModel.Tests.dll //xml=nunit-objmodel-result.xml
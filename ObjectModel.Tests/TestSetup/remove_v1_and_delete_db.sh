#!/bin/bash

set -x
set -e

INSTANCENAME=$BUILD_TAG
V1_SETUP=VersionOne.Setup-Ultimate-latest.exe


cd $WORKSPACE/ObjectModel.Tests/TestSetup
pwd


./$V1_SETUP -DeleteDatabase -LogFile:$WORKSPACE\ObjectModel.Tests\setup.log -Quiet:2 -u $INSTANCENAME



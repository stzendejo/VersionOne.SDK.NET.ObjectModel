#!/bin/bash -xe
set -x
set -e

# ----- Variables -------------------------------------------------------------
# Variables in the build.properties file will be available to Jenkins
# build steps. Variables local to this script can be defined below.
. ./build.properties



# -----------------------------------------------------------------------------

# fix for jenkins inserting the windows-style path in $WORKSPACE
cd "$WORKSPACE"
export WORKSPACE=`pwd`



# ----- Utility functions -----------------------------------------------------

function winpath() {
  # Convert gitbash style path '/c/Users/Big John/Development' to 'c:\Users\Big John\Development',
  # via dumb substitution. Handles drive letters; incurs process creation penalty for sed.
  if [ -e /etc/bash.bashrc ] ; then
    # Cygwin specific settings
    echo "`cygpath -w $1`"
  else
    # Msysgit specific settings
    echo "$1" | sed -e 's|^/\(\w\)/|\1:\\|g;s|/|\\|g'
  fi
}

function bashpath() {
  # Convert windows style path 'c:\Users\Big John\Development' to '/c/Users/Big John/Development'
  # via dumb substitution. Handles drive letters; incurs process creation penalty for sed.
  if [ -e /etc/bash.bashrc ] ; then
    # Cygwin specific settings
    echo "`cygpath $1`"
  else
    # Msysgit specific settings
    echo "$1" | sed -e 's|\(\w\):|/\1|g;s|\\|/|g'
  fi
}

function parentwith() {  # used to find $WORKSPACE, below.
  # Starting at the current dir and progressing up the ancestors,
  # retuns the first dir containing $1. If not found returns pwd.
  SEARCHTERM="$1"
  DIR=`pwd`
  while [ ! -e "$DIR/$SEARCHTERM" ]; do
    NEWDIR=`dirname "$DIR"`
    if [ "$NEWDIR" = "$DIR" ]; then
      pwd
      return
    fi
    DIR="$NEWDIR"
  done
  echo "$DIR"
}



# ----- Default values --------------------------------------------------------
# If we aren't running under jenkins some variables will be unset.
# So set them to a reasonable value.

if [ -z "$WORKSPACE" ]; then
  export WORKSPACE=`parentwith .git`;
fi

if [ -z "$VERSION_NUMBER" ]; then
  export VERSION_NUMBER="0.0.0"
fi

if [ -z "$BUILD_NUMBER" ]; then
  # presume local workstation, use date-based build number
  export BUILD_NUMBER=`date +%H%M`  # hour + minute
fi

if [ -z "$BUILD_TAG" ]; then
  export BUILD_TAG="${VERSION_NUMBER}.${BUILD_NUMBER}"
fi

if [ -z "$TEST_URL" ]; then
  export TEST_URL="http://localhost/${BUILD_TAG}/"
fi

if [ -z "$NUNIT_XML_OUTPUT" ]
then
  NUNIT_XML_OUTPUT="nunit-result.xml"
fi



# ---- Prepare Tests --------------------------------------------------------------------------


# Prepare setup, restore test data, and install application.
$WORKSPACE/TestSetup/copy_latest_setup_to_standard_name.sh
$WORKSPACE/TestSetup/restore_db_and_install_v1.sh

curl --user admin:admin -o $WORKSPACE/TestSetup/client_secrets.json ${TEST_URL}ClientRegistration.mvc/GetClientJson?client_id=client_97887v3p

call NuGetRestorePackagesOnly.bat packages.config ..\packages
call NuGetUpdatePackages.bat
build.MSBuild.bat
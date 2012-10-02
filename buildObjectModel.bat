mkdir packages
cd ObjectModel
call ..\..\GetBuildTools\bin\NuGet\RestorePackagesOnly.bat VersionOne.SDK.ObjectModel.csproj
call ..\..\GetBuildTools\bin\NuGet\UpdatePackages.bat
cd ..\
buildObjectModel.MSBuild.bat
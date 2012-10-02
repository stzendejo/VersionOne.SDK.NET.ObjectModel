mkdir packages
cd ObjectModel.Tests
call ..\..\GetBuildTools\bin\NuGet\RestorePackagesOnly.bat VersionOne.SDK.ObjectModel.Tests.csproj
call ..\..\GetBuildTools\bin\NuGet\UpdatePackages.bat
cd ..\
buildObjectModel.Tests.MSBuild.bat
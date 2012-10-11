# VersioniOne SDK.NET ObjectModel Library
The ObjectModel library provide a strongly-typed model on top of the VersionOne SDK.NET API Client library. This allows developers to easily and quickly develop VersionOne integrations and complementary applications/utilities using domain objects (Project, Story, Iteration, etc) instead of more abstract objects (Asset, Attribute). The ObjectModel is suitable for fine-grained access, such as creating new VersionOne assets.

## How to get the library as a precompiled package

You can get a precompiled libirary assembly by using the NuGet package manager from Visual Studio or nuget.exe. Search for `VersionOne.SDK.ObjectModel` to find the package.

## How to clone the source code repository from GitHub

1. You'll need a Windows Git client. We suggest _Git for Windows_ from http://msysgit.github.com/, especially since you'll also need the Git Bash command line shell from it if you want to run integration tests.
2. Open Git Bash and find a folder you want to clone the repository into.
3. Type `git clone git@github.com:versionone/VersionOne.SDK.NET.ObjectModel.git`

Note: you may have to set some authentication and SSH keys first. 

_TODO: add more details about this..._

## How to download the source code as a zip file from GitHub

1. Navigate to https://github.com/versionone/VersionOne.SDK.NET.ObjectModel
2. Click the ZIP button near the top. This downloads all the code as a single zip file.

## How to build the library from source

First, you must enable NuGet package support in the solution. To do this:

1. Open the `VersionOne.SDK.NET.ObjectModel.sln` solution in Visual Studio.
2. Right click on the solution node and click `Enable NuGet Package Restore`.
3. From the program menu, click `Tools > Library Package Manager > Package Manager Console`
4. From the Package Manager Console, you should see the message `Some NuGet packages are missing from this solution. Click to restore.` Click the `Restore` button next to it.

Those steps should download all the needed packages from the NuGet gallery. You can now build the solution.

## How to upgrade NuGet packages to their latest versions

NuGet can also update the installed packages to the most recent compatible versions. The Object Model project depends on the VersionOne API Client library, which evolves at a much slower rate, so it's unlikely to need upgrading.

To install updated packages:

1. From the program menu, click `Tools > Library Package Manager > Package Manager Console`
2. From the Package Manager Console, type: `Update-Package`

If no packages updates are available, you should see output like:

    Applying constraint 'VersionOne.SDK.APIClient (= 12.2 && < 12.3)' defined in packages.config.
    No updates available for 'VersionOne.SDK.APIClient' in project 'VersionOne.SDK.ObjectModel'.
    Applying constraint 'VersionOne.SDK.APIClient (= 12.2 && < 12.3)' defined in packages.config.
    No updates available for 'VersionOne.SDK.APIClient' in project 'VersionOne.SDK.ObjectModel.Tests'.
    No updates available for 'NUnit.Runners' in project 'VersionOne.SDK.ObjectModel.Tests'.
    No updates available for 'NUnit' in project 'VersionOne.SDK.ObjectModel.Tests'.

If package updates are available, you'd see something like:

    Updating 'VersionOne.SDK.APIClient' from version '12.2.0.0' to '12.2.44.0' in project 'VersionOne.SDK.ObjectModel.Tests'.
    Successfully removed 'VersionOne.SDK.APIClient 12.2.0.0' from VersionOne.SDK.ObjectModel.Tests.
    Successfully installed 'VersionOne.SDK.APIClient 12.2.44.0'.
    Successfully added 'VersionOne.SDK.APIClient 12.2.44.0' to VersionOne.SDK.ObjectModel.Tests.
    Updating 'VersionOne.SDK.APIClient' from version '12.2.0.0' to '12.2.44.0' in projec 'VersionOne.SDK.ObjectModel'.
    Successfully removed 'VersionOne.SDK.APIClient 12.2.0.0' from VersionOne.SDK.ObjectModel.
    Successfully added 'VersionOne.SDK.APIClient 12.2.44.0' to VersionOne.SDK.ObjectModel.
    Successfully uninstalled 'VersionOne.SDK.APIClient 12.2.0.0'.
    No updates available for 'NUnit.Runners' in project 'VersionOne.SDK.ObjectModel.Tests'.
    No updates available for 'NUnit' in project 'VersionOne.SDK.ObjectModel.Tests'.

## How to run the integration tests

### Dependencies

1. MS SQL Server 2008 or higher
2. Microsoft IIS 7 or higher
3. Git for Windows (For its Git Bash command line shell)

_Install Git for Windows from http://msysgit.github.com/ if you do not already have it._

### If SQL Server is installed at "(local)" on your workstation

1. If you have not already done so, then build the `ObjectModel.Tests` project
2. Start Git Bash and type: `cd /<path>/<to>/VersionOne.SDK.NET.ObjectModel`
3. Copy the VersionOne installer exe to the `TestSetup` folder. Example: `VersionOne.Setup-Ultimate-12.2.2.3601.exe`
4. If you don't use Ultimate, then you'll need to modify the `TestSetup/copy_latest_setup_to_standard_name.sh` script and change the `SETUP =` line to reflect the version.
5. From the root of the `VersionOne.SDK.NET.ObjectModel` folder type: `". ./run_integration_tests.sh"`

This will install VersionOne to `http://localhost/V1SDKTests_<date and timestamp>`. It will also run the integration tests using the NUnit console test runner, and store the results in `run_integration_tests.xml` when finished. There should be no failures if you are using Ultimate edition, though a number are currently ignored. If you are using Enterprise edition, you will see a number of failures due to licensing restrictions.

### If SQL Server is not installed at "(local)" on your workstation

In this case:

1. Open the file `TestSetup/restore_db_and_install_v1.sh`.
2. Change the `DB_SERVER` variable to point to your instance, such as `(local)\SQL2008` or `dbserver\V1Instance`
2. Save it, and see step 4 from above.

### Notes on running the integration tests from Visual Studio for debugging

Only do this if you are testing specific methods and do not wish to re-run the entire suite like you can do above.

By default, the tests will look for an instance of VersionOne installed at `http://localhost/V1SDKTests`. So, you can get this installed by modifying the scripts above and commenting out the remove step from the bottom. Most of the tests will run, though there are a few that depend on the fresh database restore.

In order to ease execution of the unit tests, the VersionOne.SDK.ObjectModel.Tests project file has a reference to: `$(SolutionDir)\packages\NUnit.Runners.<version>\tools\nunit.exe`

_Note: If the NUnit runner version changes, you'll need to modify that path slightly._

1. Build the VersionOne.SDK.ObjectModel.Tests project.
2. Right-click on the VersionOne.SDK.ObjectModel.Tests project and select 
   `Debug > Start new instance` or `Debug > Step Into new instance`.

By default Visual Studio's debugger will not stop on breakpoints when executing the tests in the external NUnit tool. To enable breakpoints, modify the `packages\NUnit.Runners.<verion>\tools\nunit.exe.config` file under `startup > supportedRuntime` as follows:

    <startup useLegacyV2RuntimeActivationPolicy="true">
        <!-- Comment out the next line to force use of .NET 4.0 -->
        <!-- <supportedRuntime version="v2.0.50727" /> -->
        <supportedRuntime version="v4.0.30319" />
    </startup>

This forces the NUnit runner to execute under .NET 4.0, and allows Visual Studio to thus load the debug symbols. Otherwise, NUnit executes under 2.0, but spawns a separate process to execute the tests under version 4.0.

## Getting Help
Need to bootstrap on VersionOne SDK.NET quickly? VersionOne services brings a wealth of development experience to training and mentoring on VersionOne SDK.NET:

http://www.versionone.com/training/product_training_services/

Have a question? Get help from the community of VersionOne developers:

http://groups.google.com/group/versionone-dev/

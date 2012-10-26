# VersionOne SDK.NET ObjectModel Library
The ObjectModel library provide a strongly-typed model on top of the VersionOne SDK.NET API Client library. This allows developers to easily and quickly develop VersionOne integrations and complementary applications/utilities using domain objects (Project, Story, Iteration, etc) instead of more abstract objects (Asset, Attribute). The ObjectModel is suitable for fine-grained access, such as creating new VersionOne assets.

## How to get the library as a precompiled package

_Do this if you only want to use the functionality, but are not interested in compiling from source or in contributing code to the project._

Use the NuGet package manager from Visual Studio or nuget.exe. Search for `VersionOne.SDK.ObjectModel` to find the precompiled package. Packages installed via NuGet have been tested by VersionOne against the product release version specified in the description of the package. Learn more about NuGet here: http://docs.nuget.org/docs/start-here/overview

## How to obtain the source code

_You should obtain the source code if you:_

1. Want to compile it yourself, perhaps to the better understand it or debug it.
2. Would like to contribute code to the project to improve it.

You can obtain it in two ways:

### How to download the source code as a zip file from GitHub

_Do this if you are not planning to contribute code back to the project._

1. Navigate to https://github.com/versionone/VersionOne.SDK.NET.ObjectModel
2. Click the ZIP button near the top. This downloads all the code as a single zip file.

### How to clone the source code repository from GitHub

_Do this if you want to contribute code to the project._

1. Install _Git for Windows_ from http://msysgit.github.com/
2. Run Git Bash from the start menu
3. Type `git clone git@github.com:versionone/VersionOne.SDK.NET.ObjectModel.git`

## How to build the library from source

_Once you have the code, you want to build it, right? Not so fast. First, enable NuGet package restore support in the solution:_

1. Open the `VersionOne.SDK.NET.ObjectModel.sln` solution in Visual Studio.
2. Right click on the `VersionOne.SDK.NET.ObjectModel` solution node and click `Enable NuGet Package Restore`.
3. From the program menu, click `Tools > Library Package Manager > Package Manager Console`
4. From the Package Manager Console, you should see the message `Some NuGet packages are missing from this solution. Click to restore.` Click the `Restore` button next to it.
5. Those steps should download all the needed packages from the NuGet gallery. You can now `Build` the solution.

## How to upgrade NuGet packages to their latest versions

NuGet can also update the installed packages to the most recent compatible versions. The Object Model project depends on the VersionOne API Client library, which evolves at a much slower rate, so it's unlikely to need upgrading.

To check for and install updated packages:

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
4. One of either the VersionOne Ultimate or Versionone Enterprise installer files

_Install Git for Windows from http://msysgit.github.com/ if you do not already have it._

### If SQL Server is installed at "(local)" on your workstation

1. If you have not already done so, then build the `ObjectModel.Tests` project from Visual Studio
2. Run `Git Bash` from the start menu and type: `cd /c/<path>/<to>/VersionOne.SDK.NET.ObjectModel` (Note: Git Bash is case sensitive. And, if you downloaded the zip it might have a much longer folder name)
3. Copy the VersionOne installer exe to the `TestSetup` folder. Example: `VersionOne.Setup-Ultimate-12.2.2.3601.exe`, or `VersionOne.Setup-Enterprise-12.2.2.3601.exe`
4. From the root of the `VersionOne.SDK.NET.ObjectModel` folder type: _(Note: it is a . then a space, then a ./ -- not a typo!)_ `". ./run_integration_tests.sh"`

This will install VersionOne to `http://localhost/V1SDKTests_<date and timestamp>`. It will also run the integration tests using the NUnit console test runner, and store the results in `run_integration_tests.xml` when finished. There should be no failures if you are using Ultimate edition, though a number are currently ignored. If you are using Enterprise edition, you will see a number of failures due to licensing restrictions.

### If SQL Server is not installed at "(local)" on your workstation

In this case:

1. Open the file `TestSetup/restore_db_and_install_v1.sh`.
2. Change the `DB_SERVER` variable to point to your instance, such as `(local)\SQL2008` or `dbserver\V1Instance`
2. Save it, and see step 1 from above.

### Troubleshooting

#### If it fails when attempting to run NUnit

On some 64-bit systems, the IIS Application Pool needs to have 32 bit mode enabled. Try this instead:

`. ./run_integration_tests.sh enable32bit` _(Again note that is a . space and ./)_

This will set the Enable 32 Bit flag on the created Application Pool.

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


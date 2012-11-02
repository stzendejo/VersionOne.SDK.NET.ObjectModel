# VersionOne SDK.NET ObjectModel
Copyright (c) 2008-2012 VersionOne, Inc.
All rights reserved.

The ObjectModel library provide a strongly-typed model on top of the VersionOne SDK.NET API Client library. This allows developers to easily and quickly develop VersionOne integrations and complementary applications/utilities using domain objects (Project, Story, Iteration, etc) instead of more abstract objects (Asset, Attribute). The ObjectModel is suitable for fine-grained access, such as creating new VersionOne assets.

This product includes software developed at VersionOne (http://versionone.com/). This product is open source and is licensed under a modified BSD license, which reflects our intent that software built with a dependency on the  VersionOne SDK.NET can be commercial or open source, as the authors see fit.

## Table of Contents

* System Requirements
* How to get the library as a precompiled package
* Getting Help

## Other Resources

* [DEVELOPING.md](https://github.com/versionone/VersionOne.SDK.NET.ObjectModel/blob/master/DEVELOPING.md) - Documentation on developing with 
  VersionOne SDK.NET ObjectModel
* [LICENSE.md](https://github.com/versionone/VersionOne.SDK.NET.ObjectModel/blob/master/LICENSE.md) - User license

## System Requirements

### VersionOne SDK.NET ObjectModel Library
* .NET Framework 4.0

These libraries have only been tested in a Windows environment. They have not been tested under Mono.

## How to get the library as a precompiled package

_Do this if you only want to use the functionality, but are not interested in compiling from source or in contributing code to the project._

Use the NuGet package manager from Visual Studio or nuget.exe. Search for `VersionOne.SDK.ObjectModel` to find the precompiled package. Packages installed via NuGet have been tested by VersionOne against the product release version specified in the description of the package. Learn more about NuGet here: http://docs.nuget.org/docs/start-here/overview

## How to get started in under 30 seconds

Want to test drive a pre-configured, live VersionOne instance with the library? Be our guests! Here's how:

1. Create a blank `Console` project
2. Add the `VersionOne.SDK.ObjectModel` NuGet package by following the steps just above
3. Ensure under the project's propertis that the `Client Profile` is not the target framework! Instead, it should be .NET 4.
4. Paste the following code into the auto-generated `Program.cs` and then run it!
5. And, see more [Getting Started](https://github.com/versionone/VersionOne.SDK.NET.ObjectModel/tree/master/Example/GettingStarted/src) examples in the code

```c#
using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel;

namespace SimpleConsoleApp
{
    public class RunMe
    {
        public static void Main(string[] args)
        {
            const string hostname = "https://www14.v1host.com/"; // Must be https:// if the remote host is on HTTPS!
            const string instanceName = "v1sdktesting/";
            const string url = hostname + instanceName;

            const string username = "remote";
            const string password = "remote";

            var instance = new V1Instance(url, username, password);

            const string projectName = "Console App Project";
            var project = instance.Get.ProjectByName(projectName);

            if (project == null)
            {
                throw new Exception(
                    "Please contact the VersionOne SDK developers and tell them to fix their example database!");
            }

            var yourName = System.Environment.UserName;
            var storyName = string.Format("{0}'s story at {1}", yourName, 
                DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            var attributes = new Dictionary<string, object>
                {
                    {
                        "Description",
                        string.Format("{0} is adding this story from the console app...", yourName)
                    }
                };

            var story = instance.Create.Story(storyName, project, attributes);
            story.Save();

            var v1StoryId = story.ID.Token;

            Console.WriteLine("Created story with id {0}, name {1}, and description '{2}' in project named {3}",
                              v1StoryId,
                              story.Name,
                              story.Description,
                              story.Project.Name);
            Console.WriteLine();
            Console.WriteLine("Browse to this story on the web at:");
            Console.WriteLine(story.URL);
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```

## Getting Help
Need to bootstrap on VersionOne SDK.NET quickly? VersionOne services brings a wealth of development experience to training and mentoring on VersionOne SDK.NET:

http://www.versionone.com/training/product_training_services/

Have a question? Get help from the community of VersionOne developers:

http://groups.google.com/group/versionone-dev/

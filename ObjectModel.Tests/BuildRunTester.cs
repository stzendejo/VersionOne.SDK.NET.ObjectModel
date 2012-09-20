using System;
using System.Collections.Generic;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class BuildRunTester : BaseSDKTester {
        [Test]
        public void Create() {
            var project = Instance.Create.BuildProject("My Project", "Project");
            var run = project.CreateBuildRun("My Run", new DateTime(2008, 1, 1));

            run.Reference = "My Reference";
            run.Elapsed = 5.0;
            run.Status.CurrentValue = "Failed";
            run.Source.CurrentValue = "Trigger";

            run.Save();

            Assert.AreEqual(project, run.BuildProject);
            Assert.AreEqual("My Run", run.Name);
            Assert.AreEqual(new DateTime(2008, 1, 1), run.Date);
            Assert.AreEqual("My Reference", run.Reference);
            Assert.AreEqual(5.0, run.Elapsed);
            Assert.AreEqual("Failed", run.Status.CurrentValue);
            Assert.AreEqual("Trigger", run.Source.CurrentValue);
        }

        [Test]
        public void CreateWithAttributes() {
            var attributes = new Dictionary<string, object> {{"Reference", "My Reference"}, {"Elapsed", 5.0}};

            var project = Instance.Create.BuildProject("My Project", "Project");
            var run = project.CreateBuildRun("My Run", new DateTime(2008, 1, 1), attributes);

            run.Save();

            Assert.AreEqual(project, run.BuildProject);
            Assert.AreEqual("My Run", run.Name);
            Assert.AreEqual(new DateTime(2008, 1, 1), run.Date);
            Assert.AreEqual("My Reference", run.Reference);
            Assert.AreEqual(5.0, run.Elapsed);
        }

        [Test]
        public void Delete() {
            var project = Instance.Create.BuildProject("My Project", "Project");
            var run = project.CreateBuildRun("My Run", new DateTime(2008, 1, 1));

            var id = run.ID;

            run.Delete();

            ResetInstance();

            Assert.IsNull(Instance.Get.BuildRunByID(id));
        }

        [Test]
        public void GetAffectedPrimaryWorkitems() {
            // Create Workitems
            var story = SandboxProject.CreateStory("Test Story");
            var defect = SandboxProject.CreateDefect("Test Defect");
            var notMyStory = SandboxProject.CreateStory("Other Story");
            var notMyDefect = SandboxProject.CreateDefect("Other Defect");

            // Changesets
            var changeSet = Instance.Create.ChangeSet("Test ChangeSet", "123456");
            var notMyChangeSet = Instance.Create.ChangeSet("Other ChangeSet", "abcd");
            changeSet.PrimaryWorkitems.Add(story);
            changeSet.PrimaryWorkitems.Add(defect);
            notMyChangeSet.PrimaryWorkitems.Add(notMyStory);
            notMyChangeSet.PrimaryWorkitems.Add(notMyDefect);

            // BuildRuns
            var buildProject = Instance.Create.BuildProject("BP", "1234");
            var buildRun = buildProject.CreateBuildRun("BR", DateTime.Now);
            var notMyBuildRun = buildProject.CreateBuildRun("Not My BR", DateTime.Now);
            buildRun.ChangeSets.Add(changeSet);
            notMyBuildRun.ChangeSets.Add(notMyChangeSet);

            var buildRunId = buildRun.ID;

            ResetInstance();

            var theBuildRun = Instance.Get.BuildRunByID(buildRunId);

            Assert.AreEqual(2, theBuildRun.GetAffectedPrimaryWorkitems(null).Count);
            Assert.AreEqual(1, theBuildRun.GetAffectedPrimaryWorkitems(new DefectFilter()).Count);
        }
    }
}
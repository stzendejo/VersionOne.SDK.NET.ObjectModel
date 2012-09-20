using System;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests
{
    [TestFixture]
    public class BuildProjectTester : BaseSDKTester
    {
        [Test]
        public void Create()
        {
            BuildProject project = Instance.Create.BuildProject("My Project", "Project");

            Assert.AreEqual("My Project", project.Name);
            Assert.AreEqual("Project", project.Reference);
        }

        [Test]
        public void CreateWithAttributes()
        {
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Description", "Test for BuildProject creation with required attributes");

            BuildProject project = Instance.Create.BuildProject("My Project", "Project", attributes);

            Assert.AreEqual("Test for BuildProject creation with required attributes", project.Description);
        }

        [Test]
        public void Delete()
        {
            BuildProject project = Instance.Create.BuildProject("My Project", "Project");

            AssetID id = project.ID;

            project.Delete();

            ResetInstance();

            Assert.IsNull(Instance.Get.BuildProjectByID(id));
        }

        [Test] public void DeleteCascadeRuns()
        {
            BuildProject project = Instance.Create.BuildProject("My Project", "Project");

            IList<AssetID> ids = new List<AssetID>();
            ids.Add(project.ID);
            ids.Add(project.CreateBuildRun("Run 1", new DateTime(2008, 1, 1)).ID);
            ids.Add(project.CreateBuildRun("Run 2", new DateTime(2008, 1, 2)).ID);
            ids.Add(project.CreateBuildRun("Run 3", new DateTime(2008, 1, 3)).ID);

            project.Delete();

            ResetInstance();

            foreach (AssetID id in ids)
                Assert.IsNull(Instance.Get.BaseAssetByID(id));
        }

        [Test]
        public void CloseCascadeRuns()
        {
            BuildProject project = Instance.Create.BuildProject("My Project", "Project");

            IList<AssetID> ids = new List<AssetID>();
            ids.Add(project.ID);
            ids.Add(project.CreateBuildRun("Run 1", new DateTime(2008, 1, 1)).ID);
            ids.Add(project.CreateBuildRun("Run 2", new DateTime(2008, 1, 2)).ID);
            ids.Add(project.CreateBuildRun("Run 3", new DateTime(2008, 1, 3)).ID);

            project.Close();

            ResetInstance();

            foreach (AssetID id in ids)
                Assert.IsTrue(Instance.Get.BaseAssetByID(id).IsClosed);
        }

        [Test]
        public void ReactivateCascadeRuns()
        {
            BuildProject project = Instance.Create.BuildProject("My Project", "Project");

            IList<AssetID> ids = new List<AssetID>();
            ids.Add(project.ID);
            ids.Add(project.CreateBuildRun("Run 1", new DateTime(2008, 1, 1)).ID);
            ids.Add(project.CreateBuildRun("Run 2", new DateTime(2008, 1, 2)).ID);
            ids.Add(project.CreateBuildRun("Run 3", new DateTime(2008, 1, 3)).ID);

            project.Close();

            foreach (AssetID id in ids)
                Assert.IsTrue(Instance.Get.BaseAssetByID(id).IsClosed);

            project.Reactivate();

            foreach (AssetID id in ids)
                Assert.IsTrue(Instance.Get.BaseAssetByID(id).IsActive);
        }

        [Test] 
        public void GetBuildRuns()
        {
            BuildProject project = Instance.Create.BuildProject("My Project", "Project");

            BuildRun run1 = project.CreateBuildRun("Run 1", new DateTime(2008, 1, 1));
            BuildRun run2 = project.CreateBuildRun("Run 2", new DateTime(2008, 1, 2));
            BuildRun run3 = project.CreateBuildRun("Run 3", new DateTime(2008, 1, 3));

            run1.Reference = "A";
            run2.Reference = "A";
            run3.Reference = "B";

            run1.Status.CurrentValue = "Passed";
            run2.Status.CurrentValue = "Passed";
            run3.Status.CurrentValue = "Failed";

            run1.Save();
            run2.Save();
            run3.Save();

            BuildRunFilter filter = new BuildRunFilter();
            filter.References.Add("A");
            filter.Status.Add("Passed");
            ICollection<BuildRun> runs = project.GetBuildRuns(filter);
            CollectionAssert.AreEquivalent(new string[] { "Run 1", "Run 2" }, DeriveListOfNamesFromAssets(runs));
        }
    }
}
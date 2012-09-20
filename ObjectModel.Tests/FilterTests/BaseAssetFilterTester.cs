using System;
using System.Threading;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class BaseAssetFilterTester : BaseSDKTester {
        [Test]
        public void FindUnknownEntityType() {
            var filter = new BaseAssetFilter();
            filter.Find.SearchString = Guid.NewGuid().ToString();
            var assets = Instance.Get.BaseAssets(filter);
            CollectionAssert.IsEmpty(assets);
        }

        [Test]
        public void ChangeDate() {
            var project = SandboxProject;
            var story = EntityFactory.CreateStory("Story 1", project);
            Thread.Sleep(1000);
            var task1 = EntityFactory.CreateTask("Task1.1", story);
            Thread.Sleep(1000);
            var task2 = EntityFactory.CreateTask("Task1.2", story);
            var task1Date = task1.CreateDate.ToUniversalTime();
            ResetInstance();

            var filter = new BaseAssetFilter();
            filter.ChangeDateUtc.AddTerm(FilterTerm.Operator.GreaterThan, task1Date);
            var baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.DoesNotContain(baseAssets, project);
            CollectionAssert.DoesNotContain(baseAssets, story);
            CollectionAssert.DoesNotContain(baseAssets, task1);
            CollectionAssert.Contains(baseAssets, task2);

            filter.ChangeDateUtc.Clear();
            filter.ChangeDateUtc.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, task1Date);
            baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.DoesNotContain(baseAssets, project);
            CollectionAssert.DoesNotContain(baseAssets, story);
            CollectionAssert.Contains(baseAssets, task1);
            CollectionAssert.Contains(baseAssets, task2);

            filter.ChangeDateUtc.Clear();
            filter.ChangeDateUtc.AddTerm(FilterTerm.Operator.LessThan, task1Date);
            baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.Contains(baseAssets, project);
            CollectionAssert.Contains(baseAssets, story);
            CollectionAssert.DoesNotContain(baseAssets, task1);
            CollectionAssert.DoesNotContain(baseAssets, task2);

            filter.ChangeDateUtc.Clear();
            filter.ChangeDateUtc.AddTerm(FilterTerm.Operator.LessThanOrEqual, task1Date);
            baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.Contains(baseAssets, project);
            CollectionAssert.Contains(baseAssets, story);
            CollectionAssert.Contains(baseAssets, task1);
            CollectionAssert.DoesNotContain(baseAssets, task2);

            story.Delete();
        }

        [Test]
        public void CreateDate() {
            var project = SandboxProject;
            var story = EntityFactory.CreateStory("Story1", project);
            Thread.Sleep(1000);
            var task1 = EntityFactory.CreateTask("Task1.1", story);
            Thread.Sleep(1000);
            var task2 = EntityFactory.CreateTask("Task1.2", story);
            var task1Date = task1.CreateDate.ToUniversalTime();
            ResetInstance();

            var filter = new BaseAssetFilter();
            filter.CreateDateUtc.AddTerm(FilterTerm.Operator.GreaterThan, task1Date);
            var baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.DoesNotContain(baseAssets, project);
            CollectionAssert.DoesNotContain(baseAssets, story);
            CollectionAssert.DoesNotContain(baseAssets, task1);
            CollectionAssert.Contains(baseAssets, task2);

            filter.CreateDateUtc.Clear();
            filter.CreateDateUtc.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, task1Date);
            baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.DoesNotContain(baseAssets, project);
            CollectionAssert.DoesNotContain(baseAssets, story);
            CollectionAssert.Contains(baseAssets, task1);
            CollectionAssert.Contains(baseAssets, task2);

            filter.CreateDateUtc.Clear();
            filter.CreateDateUtc.AddTerm(FilterTerm.Operator.LessThan, task1Date);
            baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.Contains(baseAssets, project);
            CollectionAssert.Contains(baseAssets, project);
            CollectionAssert.DoesNotContain(baseAssets, task1);
            CollectionAssert.DoesNotContain(baseAssets, task2);

            filter.CreateDateUtc.Clear();
            filter.CreateDateUtc.AddTerm(FilterTerm.Operator.LessThanOrEqual, task1Date);
            baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.Contains(baseAssets, project);
            CollectionAssert.Contains(baseAssets, story);
            CollectionAssert.Contains(baseAssets, task1);
            CollectionAssert.DoesNotContain(baseAssets, task2);

            story.Delete();
        }

        [Test]
        public void FilterByName() {
            const string name = "English story";

            var project = SandboxProject;
            var story = EntityFactory.CreateStory(name, project);
            var story2 = EntityFactory.CreateStory(name + '2', project);
            ResetInstance();

            var filter = new BaseAssetFilter();
            filter.Name.Add(name);
            var assets = Instance.Get.BaseAssets(filter);
            CollectionAssert.DoesNotContain(assets, project);
            CollectionAssert.Contains(assets, story);
            CollectionAssert.DoesNotContain(assets, story2);
        }

        [Test]
        [Ignore("This test fails because server refuses to accept non-latin symbols, even URL-encoded, in a QueryString")]
        public void FilterByInternationalName() {
            const string name = "\u0420\u0443\u0441 - Русская история";

            var project = SandboxProject;
            var story = EntityFactory.CreateStory(name, project);
            ResetInstance();

            var filter = new BaseAssetFilter();
            filter.Name.Add(name);
            var baseAssets = Instance.Get.BaseAssets(filter);
            CollectionAssert.DoesNotContain(baseAssets, project);
            CollectionAssert.Contains(baseAssets, story);
        }

        [Test]
        public void ActiveState() {
            var project = SandboxProject;
            var story = EntityFactory.CreateStory("Some story", project);
            ResetInstance();

            var filter = new BaseAssetFilter();
            filter.State.Add(State.Active);
            var baseAssets = Instance.Get.BaseAssets(filter);
            
            CollectionAssert.Contains(baseAssets, story);
        }

        [Test]
        public void ClosedState() {
            var project = SandboxProject;
            var story = EntityFactory.CreateStory("Some story", project);
            var story1 = EntityFactory.CreateStory("Another story", project);
            story1.Close();
            ResetInstance();

            var filter = new BaseAssetFilter();
            filter.State.Add(State.Closed);
            var baseAssets = Instance.Get.BaseAssets(filter);

            CollectionAssert.DoesNotContain(baseAssets, story);
            CollectionAssert.Contains(baseAssets, story1);
        }

        [Test]
        public void BothStates() {
            var project = SandboxProject;
            var story = EntityFactory.CreateStory("Some story", project);
            var story1 = EntityFactory.CreateStory("Another story", project);
            story1.Close();
            ResetInstance();

            var filter = new BaseAssetFilter();
            var baseAssets = Instance.Get.BaseAssets(filter);

            CollectionAssert.Contains(baseAssets, story);
            CollectionAssert.Contains(baseAssets, story1);
        }

        [Test]
        public void DeletedState() {
            var project = SandboxProject;
            var story = EntityFactory.CreateStory("Some story", project);
            story.Delete();
            ResetInstance();

            var filter = new BaseAssetFilter();
            var baseAssets = Instance.Get.BaseAssets(filter);

            CollectionAssert.DoesNotContain(baseAssets, story);
        }
    }
}

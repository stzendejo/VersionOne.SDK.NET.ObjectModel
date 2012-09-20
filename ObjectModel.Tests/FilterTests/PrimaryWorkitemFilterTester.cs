using System;
using System.Linq;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
	[TestFixture]
    public class PrimaryWorkitemFilterTester : PrimaryWorkitemFilterTesterBase {
        private PrimaryWorkitemFilter GetFilter() {
            var filter = new PrimaryWorkitemFilter();
            filter.Project.Add(SandboxProject);
			return filter;
		}

        [Test]
        public void Project() {
            var primaryWorkItems = Instance.Get.PrimaryWorkitems(GetFilter());
			Assert.AreEqual(6, primaryWorkItems.Count);
		}

        [Test]
        public void NoOwner() {
            var filter = GetFilter();
			filter.Owners.Add(null);

            var primaryWorkitems = Instance.Get.PrimaryWorkitems(filter);

			Assert.AreEqual(2, primaryWorkitems.Count);
		}

        [Test]
        public void NoOrAndreOwner() {
            var filter = GetFilter();
			filter.Owners.Add(null);
            filter.Owners.Add(andre);

            var primaryWorkitems = Instance.Get.PrimaryWorkitems(filter);

			Assert.AreEqual(5, primaryWorkitems.Count);
		}

        [Test]
        public void Names() {
            var filter = GetFilter();
			filter.Name.Add("Defect 2");
			filter.Name.Add("Story 2");

            var primaryWorkitems = Instance.Get.PrimaryWorkitems(filter);

			Assert.AreEqual(2, primaryWorkitems.Count);
		}

        [Test]
        public void Estimate() {
            var filter = GetFilter();
            filter.Estimate.AddTerm(FilterTerm.Operator.Equal, 1.0);
			Assert.AreEqual(1, Instance.Get.PrimaryWorkitems(filter).Count);
		}

        [Test]
        public void EstimateRange() {
            var filter = GetFilter();
            filter.Estimate.Range(1, 3);
            Assert.AreEqual(2, Instance.Get.PrimaryWorkitems(filter).Count);

            filter.Estimate.Clear();
            filter.Estimate.Range(1.5, 3);
			Assert.AreEqual(1, Instance.Get.PrimaryWorkitems(filter).Count);
		}

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EstimateInvalidRange() {
            var filter = GetFilter();
            filter.Estimate.Range(2, 1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EstimateMinRangeBoundNull() {
            var filter = GetFilter();
            filter.Estimate.Range(null, 1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EstimateMaxRangeBoundNull() {
            var filter = GetFilter();
            filter.Estimate.Range(1, null);
        }

        [Test]
        public void AffectedBy() {
            var filter = GetFilter();
            filter.AffectedByDefects.Add(defect1);
            Assert.AreEqual(1, Instance.Get.PrimaryWorkitems(filter).Count);
        }

        [Test]
        public void ExcludeThemesEpicsAndTemplates() {
            var root = SandboxProject.ParentProject;

            var wiFilter = new WorkitemFilter();
			wiFilter.State.Add(State.Active);

            var workitems = Instance.Get.Workitems(wiFilter);
            var totalEstimate = workitems.Where(workitem => workitem.DetailEstimate != null)
                                         .Sum(workitem => workitem.DetailEstimate.HasValue ? workitem.DetailEstimate.Value : 0);

			Assert.AreEqual(totalEstimate, root.GetTotalDetailEstimate(wiFilter, true));
		}

        [Test]
        public void OnlyStoriesandDefects() {
            var root = SandboxProject.ParentProject;

            var wiFilter = new PrimaryWorkitemFilter();
			wiFilter.State.Add(State.Active);

            double? totalEstimate = Instance.Get.Workitems(wiFilter).Where(workitem => workitem.DetailEstimate != null)
                                                                    .Aggregate<Workitem, double?>(null, (current, workitem) => current + workitem.DetailEstimate);

			Assert.AreEqual(totalEstimate, root.GetTotalDetailEstimate(wiFilter));
		}

        [Test]
        public void OnlyTasksAndTests() {
            var root = SandboxProject.ParentProject;

            var wiFilter = new SecondaryWorkitemFilter();
			wiFilter.State.Add(State.Active);

            var totalEstimate = Instance.Get.Workitems(wiFilter).Where(workitem => workitem.DetailEstimate != null)
                                                                .Aggregate<Workitem, double?>(null, (current, workitem) => current + workitem.DetailEstimate);

			Assert.AreEqual(totalEstimate, root.GetTotalDetailEstimate(wiFilter));
		}

        [Test]
        public void TaskDefaultOrder() {
            var story = SandboxProject.CreateStory("My Story");

            var task1 = story.CreateTask("Task 1");
            var task2 = story.CreateTask("Task 2");
            var task3 = story.CreateTask("Task 3");

            task2.RankOrder.SetBelow(task3);
            task1.RankOrder.SetBelow(task2);

            //order should be 3,2,1
            var workitems = story.GetSecondaryWorkitems(new TaskFilter());
            var expected = new[] {"Task 3", "Task 2", "Task 1"};
            CollectionAssert.AreEqual(expected, DeriveListOfNamesFromAssets(workitems));
        }

		/// <summary>
		/// Tests passing a more specific filter to a less specific query method.
		/// </summary>
        [Test]
        public void TaskBuildFilter() {
            var story = SandboxProject.CreateStory("Task Builds");
            var taskA = story.CreateTask("Task A");
            var taskB = story.CreateTask("Task B");
            var taskC = story.CreateTask("Task C");
            var taskD = story.CreateTask("Task D");

            taskA.Build = "Build Alpha";
            taskB.Build = "Build Beta";
            taskC.Build = "Build Alpha";
            taskD.Build = "Build Beta";

            taskA.Save();
            taskB.Save();
            taskC.Save();
            taskD.Save();

            var filter = new TaskFilter();
            filter.Build.Add("Build Beta");
            var items = story.GetSecondaryWorkitems(filter);
            CollectionAssert.AreEqual(new string[] { "Task B", "Task D" }, DeriveListOfNamesFromAssets(items));
        }

        [Test]
        public void NoEpicAmongPrimaryWorkitems() {
            var epic = SandboxProject.CreateEpic("War And Piece");

			ResetInstance();

            CollectionAssert.DoesNotContain(DeriveListOfNamesFromAssets(Instance.Get.PrimaryWorkitems(null)), epic.Name);
		}

        /// <summary>
        /// Tests filtering PrimaryWorkitems by Theme.
        /// </summary>
        [Test]
        public void TestThemes() {
	        var sFilter = new StoryFilter();
            var theme = SandboxProject.CreateTheme("Test Theme");
	        sFilter.Theme.Add(theme);

            var stories = Instance.Get.Stories(sFilter);
	        Assert.AreEqual(0, stories.Count);

            story1.Theme = theme;
            story1.Save();
	        ResetInstance();

            var storiesWithTestTheme = Instance.Get.Stories(sFilter);
	        Assert.AreEqual(1, storiesWithTestTheme.Count);
            Assert.IsTrue(storiesWithTestTheme.Contains(story1));
	    }
	}
}

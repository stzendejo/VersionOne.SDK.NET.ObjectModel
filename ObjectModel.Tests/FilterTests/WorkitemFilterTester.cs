using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
	[TestFixture]
    public class WorkitemFilterTester : PrimaryWorkitemFilterTesterBase {
        private DefectFilter GetFilter() {
            var filter = new DefectFilter();
            filter.Project.Add(sandboxProject);
			return filter;
		}

        [Test]
        public void DetailEstimate() {
            var story = EntityFactory.CreateStory("Story 1", SandboxProject);
            var task = EntityFactory.CreateTask("Task 1", story);
            task.DetailEstimate = 18;
            task.Save();

            var filter = new StoryFilter();
            filter.DetailEstimate.AddTerm(FilterTerm.Operator.Equal, 1.0);
            var stories = Instance.Get.Stories(filter);
            Assert.AreEqual(0, stories.Count);

            filter = new StoryFilter();
            filter.DetailEstimate.AddTerm(FilterTerm.Operator.NotExists);
            stories = Instance.Get.Stories(filter);
            CollectionAssert.Contains(stories, story);
		}

        [Test]
        public void Reference() {
            var filter = GetFilter();
			filter.Reference.Add("123456"); 
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count);
		}

        [Test]
        public void NoThemeAmongWorkitems() {
            var theme = SandboxProject.CreateTheme("A Theme");
			ResetInstance();
            CollectionAssert.DoesNotContain(DeriveListOfNamesFromAssets(Instance.Get.Workitems(null)), theme.Name);
		}

        [Test]
        public void EpicAmongWorkitems() {
            var epic = SandboxProject.CreateEpic("War And Piece");
			ResetInstance();

            CollectionAssert.Contains(DeriveListOfNamesFromAssets(Instance.Get.Workitems(null)), epic.Name);
		}
	}
}

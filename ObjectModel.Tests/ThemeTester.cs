using System.Collections.Generic;

using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class ThemeTester : BaseSDKTester {
        private const double EstimatesPrecision = 0.0001;

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void CreateAndRetrieveTheme() {
            const string name = "New Name";

            var id = Instance.Create.Theme(name, Instance.Get.ProjectByID("Scope:0")).ID;

            ResetInstance();

            var theme = Instance.Get.ThemeByID(id);
            Assert.AreEqual(theme.Name, name);
            theme.Delete();
        }

        [Test]
        public void CreateThemeWithRequiredAttributes() {
            const string description = "Test for Theme creation with required attributes";
            const string name = "CreateAndRetrieveTheme";
            IDictionary<string, object> attributes = new Dictionary<string, object> {{"Description", description}};

            var id = Instance.Create.Theme(name, Instance.Get.ProjectByID("Scope:0"), attributes).ID;

            ResetInstance();

            var theme = Instance.Get.ThemeByID(id);
            Assert.AreEqual(name, theme.Name);
            Assert.AreEqual(description, theme.Description);

            theme.Delete();
        }

        [Test]
        public void TestDetailEstimationTheme() {
            const double detailEstimateStory1 = 12.1;
            const double detailEstimateStory2 = 23.3;

            var theme = CreateTheme("FG name");
            var attributes = new Dictionary<string, object> {{"DetailEstimate", detailEstimateStory1}};
            var story1 = CreateStoryUnderTheme("story1", theme);
            CreateTask("task1", story1, attributes);

            attributes = new Dictionary<string, object> {{"DetailEstimate", detailEstimateStory2}};
            var story2 = CreateStoryUnderTheme("story2", theme);
            CreateTask("task2", story2, attributes);

            Assert.AreEqual(detailEstimateStory1 + detailEstimateStory2, theme.GetTotalDetailEstimate(null).Value, EstimatesPrecision);
        }

        [Test]
        public void TestDetailEstimationThemeWithSubTheme() {
            const double detailEstimateStory1 = 12.1;
            const double detailEstimateStory2 = 23.3;

            var theme1 = CreateTheme("FG name");
            var theme2 = CreateSubTheme(theme1, "test sub FG 1");

            var attributes = new Dictionary<string, object> {{"DetailEstimate", detailEstimateStory1}};
            var story1 = CreateStoryUnderTheme("story1", theme1);
            CreateTask("task1", story1, attributes);

            attributes = new Dictionary<string, object> {{"DetailEstimate", detailEstimateStory2}};
            var story2 = CreateStoryUnderTheme("story2", theme2);
            CreateTask("task2", story2, attributes);

            Assert.AreEqual(detailEstimateStory2, theme2.GetTotalDetailEstimate(null).Value, EstimatesPrecision);
            Assert.AreEqual(detailEstimateStory1 + detailEstimateStory2, theme1.GetTotalDetailEstimate(null).Value, EstimatesPrecision);
        }

        [Test]
        public void TestTotalToDoTheme() {
            const double toDoStory1 = 12.1;
            const double toDoStory2 = 23.3;

            var theme1 = CreateTheme("FG name");
            var theme2 = CreateSubTheme(theme1, "test sub FG 1");

            var attributes = new Dictionary<string, object> {{"ToDo", toDoStory1}};
            var story1 = CreateStoryUnderTheme("story1", theme1);
            CreateTask("task1", story1, attributes);

            attributes = new Dictionary<string, object> {{"ToDo", toDoStory2}};
            var story2 = CreateStoryUnderTheme("story2", theme2);
            CreateTask("task2", story2, attributes);

            Assert.AreEqual(toDoStory2, theme2.GetTotalToDo(null).Value, EstimatesPrecision);
            Assert.AreEqual(toDoStory1 + toDoStory2, theme1.GetTotalToDo(null).Value, EstimatesPrecision);
        }

        [Test]
        public void TestTotalToDoThemeWithSubTheme() {
            const double toDoStory1 = 12.1;
            const double toDoStory2 = 23.3;

            var theme = CreateTheme("FG name");

            var attributes = new Dictionary<string, object> {{"ToDo", toDoStory1}};
            var story1 = CreateStoryUnderTheme("story1", theme);
            CreateTask("task1", story1, attributes);

            attributes = new Dictionary<string, object> {{"ToDo", toDoStory2}};
            var story2 = CreateStoryUnderTheme("story2", theme);
            CreateTask("task2", story2, attributes);

            Assert.AreEqual(toDoStory1 + toDoStory2, theme.GetTotalToDo(null).Value, EstimatesPrecision);
        }

        private Theme CreateSubTheme(Theme theme, string themeName) {
            return EntityFactory.Create(() => theme.CreateChildTheme(themeName));
        }

        private Theme CreateTheme(string themeName, IDictionary<string, object> attributes = null) {
            return EntityFactory.Create(() => Instance.Create.Theme(themeName, SandboxProject, attributes));
        }

        private Story CreateStoryUnderTheme(string storyName, Theme theme) {
            var story = EntityFactory.CreateStory(storyName, SandboxProject);
            story.Theme = theme;
            story.Save();

            return story;
        }

        private Task CreateTask(string taskName, Story story, IDictionary<string, object> attributes = null) {
            return EntityFactory.CreateTask(taskName, story, attributes);
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}
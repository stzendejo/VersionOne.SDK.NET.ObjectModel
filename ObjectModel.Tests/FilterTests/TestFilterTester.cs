using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class TestFilterTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void Type() {
            var story = EntityFactory.CreateStory("Type Filter", SandboxProject);
            var test = EntityFactory.CreateTest("Type Filter", story);
            var taskType = test.Type.AllValues[0];
            test.Type.CurrentValue = taskType;
            test.Save();

            var not = EntityFactory.CreateTest("Doesn't match", story);

            ResetInstance();

            var filter = new TestFilter();
            filter.Type.Add(taskType);

            var results = SandboxProject.GetTests(filter);

            Assert.IsTrue(FindRelated(test, results), "Expected to find test that matched filter.");
            Assert.IsFalse(FindRelated(not, results), "Expected to NOT find test that doesn't match filter.");
            foreach (var result in results) {
                Assert.AreEqual(taskType, result.Type.CurrentValue);
            }
        }

        [Test]
        public void Status() {
            var story = EntityFactory.CreateStory("Status Filter", SandboxProject);
            var task = EntityFactory.CreateTest("Status Filter", story);
            var taskStatus = task.Status.AllValues[0];
            task.Status.CurrentValue = taskStatus;
            task.Save();

            var not = EntityFactory.CreateTest("Doesn't match", story);

            ResetInstance();

            var filter = new TestFilter();
            filter.Status.Add(taskStatus);

            var results = SandboxProject.GetTests(filter);

            Assert.IsTrue(FindRelated(task, results), "Expected to find test that matched filter.");
            Assert.IsFalse(FindRelated(not, results), "Expected to NOT find test that doesn't match filter.");
            foreach (var result in results) {
                Assert.AreEqual(taskStatus, result.Status.CurrentValue);
            }
        }

        [Test]
        public void Epic() {
            var epic = EntityFactory.CreateEpic("Epic for Test", SandboxProject);
            var test = EntityFactory.CreateTest("test for Epic", epic);
            var notMatchEpic = EntityFactory.CreateEpic("Doesn't match", SandboxProject);
            var notMatchTest = EntityFactory.CreateTest("Doesn't match", notMatchEpic);

            ResetInstance();

            var filter = new TestFilter();
            filter.Epic.Add(epic);

            var results = SandboxProject.GetTests(filter);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(FindRelated(test, results), "Expected to find test that matched filter.");
            Assert.IsFalse(FindRelated(notMatchTest, results), "Expected to NOT find test that doesn't match filter.");
            Assert.AreEqual(epic, First(results).Parent);
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}
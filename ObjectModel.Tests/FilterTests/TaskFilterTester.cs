using System.Collections.Generic;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class TaskFilterTester : BaseSDKTester {
        [Test]
        public void Build() {
            const string buildNumber = "10.2.24.1";

            var story = SandboxProject.CreateStory("Build Filter");
            var task = story.CreateTask("Build Filter");
            task.Build = buildNumber;
            task.Save();

            var not = story.CreateTask("Doesn't match");

            ResetInstance();

            var filter = new TaskFilter();
            filter.Build.Add(buildNumber);

            var results = SandboxProject.GetTasks(filter);

            Assert.IsTrue(FindRelated(task, results), "Expected to find task that matched filter.");
            Assert.IsFalse(FindRelated(not, results), "Expected to NOT find task that doesn't match filter.");
            foreach (var result in results) {
                Assert.AreEqual(buildNumber, result.Build);
            }
        }

        [Test]
        public void Source() {
            var story = SandboxProject.CreateStory("Source Filter");
            var task = story.CreateTask("Source Filter");
            var taskSource = task.Source.AllValues[0];
            task.Source.CurrentValue = taskSource;
            task.Save();

            var not = story.CreateTask("Doesn't match");

            ResetInstance();

            var filter = new TaskFilter();
            filter.Source.Add(taskSource);

            var results = SandboxProject.GetTasks(filter);

            Assert.IsTrue(FindRelated(task, results), "Expected to find task that matched filter.");
            Assert.IsFalse(FindRelated(not, results), "Expected to NOT find task that doesn't match filter.");
            foreach (var result in results) {
                Assert.AreEqual(taskSource, result.Source.CurrentValue);
            }
        }

        [Test]
        public void Type() {
            var story = SandboxProject.CreateStory("Type Filter");
            var task = story.CreateTask("Type Filter");
            var taskType = task.Type.AllValues[0];
            task.Type.CurrentValue = taskType;
            task.Save();

            var not = story.CreateTask("Doesn't match");

            ResetInstance();

            var filter = new TaskFilter();
            filter.Type.Add(taskType);

            var results = SandboxProject.GetTasks(filter);

            Assert.IsTrue(FindRelated(task, results), "Expected to find task that matched filter.");
            Assert.IsFalse(FindRelated(not, results), "Expected to NOT find task that doesn't match filter.");
            foreach (var result in results) {
                Assert.AreEqual(taskType, result.Type.CurrentValue);
            }
        }

        [Test]
        public void Status() {
            var story = SandboxProject.CreateStory("Status Filter");
            var task = story.CreateTask("Status Filter");
            var taskStatus = task.Status.AllValues[0];
            task.Status.CurrentValue = taskStatus;
            task.Save();

            var not = story.CreateTask("Doesn't match");

            ResetInstance();

            var filter = new TaskFilter();
            filter.Status.Add(taskStatus);

            var results = SandboxProject.GetTasks(filter);

            Assert.IsTrue(FindRelated(task, results), "Expected to find task that matched filter.");
            Assert.IsFalse(FindRelated(not, results), "Expected to NOT find task that doesn't match filter.");
            foreach (var result in results) {
                Assert.AreEqual(taskStatus, result.Status.CurrentValue);
            }
        }

        [Test]
        public void Parent() {
            var story = SandboxProject.CreateStory("Type Filter");
            story.CreateTask("Task 1");
            story.CreateTask("Task 2");

            ResetInstance();

            var filter = new TaskFilter();
            filter.Parent.Add(story);

            Assert.AreEqual(2, Instance.Get.Tasks(filter).Count);
        }
    }
}
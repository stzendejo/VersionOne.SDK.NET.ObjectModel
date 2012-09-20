using System.Collections.Generic;

using NUnit.Framework;

using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
	[TestFixture]
    public class TaskTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        private Task[] GetTaskArrayFromFilter(TaskFilter filter) {
            var tasks = Instance.Get.Tasks(filter);
            var taskArray = new Task[tasks.Count];
			tasks.CopyTo(taskArray, 0);
			return taskArray;
		}

        [Test]
        public void Order() {
            var story = SandboxProject.CreateStory("Task Order Test");

            var task1 = story.CreateTask("Task 1");
            var task2 = story.CreateTask("Task 2");

            var filter = new TaskFilter();
			filter.Parent.Add(story);
			filter.OrderBy.Add("RankOrder");
            var taskArray = GetTaskArrayFromFilter(filter);
			Assert.AreEqual("Task 1", taskArray[0].Name);
			Assert.AreEqual("Task 2", taskArray[1].Name);

			task2.RankOrder.SetAbove(task1);

			taskArray = GetTaskArrayFromFilter(filter);
			Assert.AreEqual("Task 2", taskArray[0].Name);
			Assert.AreEqual("Task 1", taskArray[1].Name);
		}

		[Test]
        public void TaskEffort() {
            var story = SandboxProject.CreateStory("Task Effort Test");
            var task = story.CreateTask("Task 1");
            var effort = task.GetEffortRecords(null);
			Assert.AreEqual(0, effort.Count);
		}

        [Test]
        public void CreateTaskWithRequiredAttributes() {
            const string description = "Test for Task creation with required attributes";
            const string name = "CreateAndRetrieveTask";
            var attributes = new Dictionary<string, object> { { "Description", description } };

            var story = SandboxProject.CreateStory("Task Order Test");
            story.CreateTask(name, attributes);

            ResetInstance();

            story = Instance.Get.StoryByID(story.ID);
            var task = First(story.GetSecondaryWorkitems(null));

            Assert.AreEqual(name, task.Name);
            Assert.AreEqual(description, task.Description);

            task.Delete();
        }

        [Test]
        [ExpectedException(typeof(OidException))]
        public void TaskAsParentForTask() {
            var story = EntityFactory.CreateStory("Story for task as task", SandboxProject);
            var task = EntityFactory.CreateTask("Task for task", story);

            task.Parent = task;
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
	}
}

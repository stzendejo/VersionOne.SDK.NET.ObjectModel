using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
	[TestFixture]
    public class IterationTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            IDictionary<string, object> attributes = new Dictionary<string, object>(1);
            attributes.Add("Scheme", DefaultSchemeOid);
            return Instance.Create.Project(SandboxName, rootProject, DateTime.Now, SandboxSchedule, attributes);
        }

        [Test]
        public void IterationAttributes() {
            var iteration = Instance.Get.IterationByID("Timebox:1025");

			Assert.AreEqual("Month C 1st Half", iteration.Name);
			Assert.IsFalse(iteration.IsFuture);
			Assert.IsTrue(iteration.IsActive);
			Assert.IsFalse(iteration.IsClosed);
			Assert.IsTrue(iteration.CanMakeFuture);
			Assert.IsTrue(iteration.CanClose);
			Assert.IsFalse(iteration.CanActivate);
			Assert.IsFalse(iteration.CanReactivate);
		}

        [Test]
        public void CreateWithSystemSuggested() {
            var project = Instance.Get.ProjectByID("Scope:1018");
            var lastIteration = project.GetIterations(null).LastOrDefault();

            var iteration = project.CreateIteration();
            string iterationId = iteration.ID;
			
			ResetInstance();
			
            var newProject = Instance.Get.ProjectByID("Scope:1018");

            var expectedStartDate = lastIteration.EndDate;
            var expectedEndDate = lastIteration.EndDate.AddDays(7);

            var newIteration = Instance.Get.IterationByID(iterationId);
			
			Assert.AreEqual(newProject.Schedule,newIteration.Schedule);
            Assert.IsTrue(newIteration.Name.StartsWith("Iteration"));
            Assert.AreEqual(expectedStartDate, newIteration.BeginDate);
			Assert.AreEqual(expectedEndDate, newIteration.EndDate);
		}

        [Test]
        public void CreateWithAttributes() {
            var attributes = new Dictionary<string, object> {{"Description", "Test for Iteration creation with required attributes"}};

            var project = Instance.Get.ProjectByID("Scope:1018");
            var iteration = project.CreateIteration(attributes);
            string iterationId = iteration.ID;

            ResetInstance();

            var newProject = Instance.Get.ProjectByID("Scope:1018");
            var newIteration = Instance.Get.IterationByID(iterationId);           

            Assert.AreEqual(newProject.Schedule, newIteration.Schedule);
            Assert.AreEqual("Test for Iteration creation with required attributes", newIteration.Description);
            newIteration.Delete();
        }

        [Test]
        public void CreateCustomIteration() {
            var project = Instance.Get.ProjectByID("Scope:1017");
            var lastIteration = project.GetIterations(null).LastOrDefault();

            var startDate = lastIteration.EndDate.AddDays(1);
            var endDate = lastIteration.EndDate.AddDays(14);

            var iteration = project.CreateIteration("Month 7 & 8", startDate, endDate);
            string iterationId = iteration.ID;

			ResetInstance();

            var newProject = Instance.Get.ProjectByID("Scope:1017");
            var newIteration = Instance.Get.IterationByID(iterationId);

			Assert.AreEqual(newProject.Schedule, newIteration.Schedule);
			Assert.AreEqual("Month 7 & 8", newIteration.Name);
			Assert.AreEqual(startDate, newIteration.BeginDate);
			Assert.AreEqual(endDate, newIteration.EndDate);
		}

        [Test]
        public void CreateCustomIterationWithAttributes() {
            var attributes = new Dictionary<string, object> {{"Description", "Test for Custom Iteration creation with required attributes"}};

            var project = Instance.Get.ProjectByID("Scope:1017");
            var iteration = project.CreateIteration("Month 7 & 8", new DateTime(2011, 3, 6), new DateTime(2011, 5, 3), attributes);
            string iterationId = iteration.ID;

            ResetInstance();

            var newProject = Instance.Get.ProjectByID("Scope:1017");
            var newIteration = Instance.Get.IterationByID(iterationId);

            Assert.AreEqual(newProject.Schedule, newIteration.Schedule);
            Assert.AreEqual("Test for Custom Iteration creation with required attributes", newIteration.Description);
            newIteration.Delete();
        }

        
        [Test]
        public void EnumerateStoriesAndDefects() {
            var items = Instance.Get.IterationByID("Timebox:1026").GetPrimaryWorkitems(null);
            var expected = new[] {"Story:1084", "Story:1085", "Story:1086", "Story:1087", "Defect:1411", "Defect:1412", "Defect:1413"};
            CollectionAssert.AreEquivalent(expected, DeriveListOfIdsFromAssets(items));

		}

        [Test]
        public void AssignStory() {
            var iteration = Instance.Get.IterationByID("Timebox:1034");
            var project = First(iteration.Schedule.ScheduledProjects);
            var story = project.CreateStory("New Story");
            string storyId = story.ID;
			story.Iteration = iteration;
			story.Save();
			ResetInstance();

            Assert.AreEqual(Instance.Get.IterationByID("Timebox:1034"), Instance.Get.StoryByID(storyId).Iteration);
		}

        [Test]
        public void AssignDefect() {
            var iteration = Instance.Get.IterationByID("Timebox:1034");
            var project = First(iteration.Schedule.ScheduledProjects);
            var defect = project.CreateDefect("New Defect");
            string defectId = defect.ID;
			defect.Iteration = iteration;
			defect.Save();
			ResetInstance();

            Assert.AreEqual(Instance.Get.IterationByID("Timebox:1034"), Instance.Get.DefectByID(defectId).Iteration);
		}

		[Test]
        public void GetStories() {
            var iteration = SandboxProject.CreateIteration();
			iteration.Name = "Test Iteration";
			iteration.Save();

		    CreateStory("Story 1", SandboxProject, iteration);
		    CreateStory("Story 2", SandboxProject, iteration);

			Assert.AreEqual(2, iteration.GetPrimaryWorkitems(new StoryFilter()).Count);

            var filter = new StoryFilter();
			filter.Iteration.Add(SandboxProject.CreateIteration());

			Assert.AreEqual(2, iteration.GetPrimaryWorkitems(filter).Count, "Iteration.GetStories didn't override the Iteration filter.");
		}

		[Test]
        public void GetPrimaryWorkitems() {
            var iteration = SandboxProject.CreateIteration();
			iteration.Name = "Test Iteration";
			iteration.Save();

            CreateStory("Story 1", SandboxProject, iteration);
            CreateStory("Story 2", SandboxProject, iteration);
            CreateDefect("Defect 1", SandboxProject, iteration);
            CreateDefect("Defect 2", SandboxProject, iteration);

			Assert.AreEqual(4, iteration.GetPrimaryWorkitems(new PrimaryWorkitemFilter()).Count);
		}
	}
}

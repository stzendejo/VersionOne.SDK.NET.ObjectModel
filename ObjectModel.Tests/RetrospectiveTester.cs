using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class RetrospectiveTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            return Instance.Create.Project(SandboxName, rootProject, DateTime.Now, SandboxSchedule);
        }

        [Test]
        public void RetrospectiveAttributes() {
            var retro = Instance.Get.RetrospectiveByID("Retrospective:1789");
            Assert.AreEqual("First Retrospective", retro.Name);
            Assert.IsTrue(retro.CanClose);
            Assert.IsFalse(retro.CanReactivate);
        }

        [Test]
        public void Create() {
            var retro = SandboxProject.CreateRetrospective("New Retro");
            var retroId = retro.ID;

            ResetInstance();

            var newRetro = Instance.Get.RetrospectiveByID(retroId);
            Assert.AreEqual("New Retro", newRetro.Name);
            Assert.AreEqual(SandboxProject, newRetro.Project);
        }

        [Test]
        public void CreateRetrospectiveWithRequiredAttributes() {
            const string description = "Test for Retrospective creation with required attributes";
            const string name = "CreateAndRetrieveRetrospective";
            var attributes = new Dictionary<string, object> { { "Description", description } };

            var retrospective = Instance.Create.Retrospective(name, Instance.Get.ProjectByID("Scope:0"), attributes);

            ResetInstance();

            retrospective = Instance.Get.RetrospectiveByID(retrospective.ID);

            Assert.AreEqual(name, retrospective.Name);
            Assert.AreEqual(description, retrospective.Description);

            retrospective.Delete();
        }

        [Test]
        public void BasicAttributes() {
            var retro = SandboxProject.CreateRetrospective("New Retro");
            var retroIteration = SandboxProject.CreateIteration("Retro Iteration", DateTime.Now.Date, DateTime.Now.Date.AddDays(7));
            var facilitator = Instance.Get.MemberByID("Member:20");
            DateTime? retroDate = DateTime.Now.Date;
            const string retroSummary = "We did this, we did that...";

            retro.Summary = retroSummary;
            retro.Date = retroDate;
            retro.FacilitatedBy = facilitator;
            retro.Iteration = retroIteration;

            retro.Save();

            ResetInstance();

            var newRetro = Instance.Get.RetrospectiveByID(retro.ID);
            retroIteration = Instance.Get.IterationByID(retroIteration.ID);
            facilitator = Instance.Get.MemberByID(facilitator.ID);

            Assert.AreEqual(retroIteration, newRetro.Iteration);
            Assert.AreEqual(facilitator, newRetro.FacilitatedBy);
            Assert.AreEqual(retroDate, newRetro.Date);
            Assert.AreEqual(retroSummary, newRetro.Summary);
        }

        [Test]
        public void IdentifiedStories() {
            var retro = SandboxProject.CreateRetrospective("Retro with Stories");
            var story = retro.CreateStory("Retro Story");

            ResetInstance();

            retro = Instance.Get.RetrospectiveByID(retro.ID);
            story = Instance.Get.StoryByID(story.ID);

            Assert.IsTrue(FindRelated(story, retro.GetIdentifiedStories(null)));
            Assert.AreEqual(story.IdentifiedIn, retro);
        }

        [Test]
        public void Issues() {
            var retro = SandboxProject.CreateRetrospective("Retro with Issues");
            var issue = retro.CreateIssue("Retro Issue");

            ResetInstance();

            retro = Instance.Get.RetrospectiveByID(retro.ID);
            issue = Instance.Get.IssueByID(issue.ID);

            Assert.IsTrue(FindRelated(issue, retro.GetIssues(null)));
            Assert.IsTrue(FindRelated(retro, issue.Retrospectives));
        }
    }
}
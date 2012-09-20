using System.Collections.Generic;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class IssueTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            var mandatoryAttributes = new Dictionary<string, object>(1) {{"Scheme", DefaultSchemeOid}};

            return EntityFactory.CreateProject(SandboxName, rootProject, mandatoryAttributes);
        }

        [Test]
        public void CreateAndRetrieveIssue() {
            const string name = "New Name";

            var id = EntityFactory.Create(() => Instance.Create.Issue(name, SandboxProject)).ID;

            ResetInstance();

            var issue = Instance.Get.IssueByID(id);
            Assert.AreEqual(issue.Name, name);
        }

        [Test]
        public void CreateWithAttributes() {
            var attributes = new Dictionary<string, object> {{"Description", "Test for issue creation with required attributes"}};
            const string name = "New Name";

            var id = EntityFactory.Create(() => Instance.Create.Issue(name, SandboxProject, attributes)).ID;

            ResetInstance();

            var issue = Instance.Get.IssueByID(id);
            Assert.AreEqual(issue.Name, name);
            Assert.AreEqual(issue.Description, "Test for issue creation with required attributes");
        }

        [Test]
        public void IssueOrder() {
            var project = SandboxProject;
            var issue1 = EntityFactory.Create(() => project.CreateIssue("Issue 1"));
            var issue2 = EntityFactory.Create(() => project.CreateIssue("Issue 2"));

            var id1 = issue1.ID;
            var id2 = issue2.ID;

            issue1.RankOrder.SetBelow(issue2);

            Assert.IsTrue(issue1.RankOrder.IsBelow(issue2));
            Assert.IsTrue(issue2.RankOrder.IsAbove(issue1));

            ResetInstance();

            issue1 = Instance.Get.IssueByID(id1);
            issue2 = Instance.Get.IssueByID(id2);

            issue1.RankOrder.SetAbove(issue2);

            Assert.IsTrue(issue1.RankOrder.IsAbove(issue2));
            Assert.IsTrue(issue2.RankOrder.IsBelow(issue1));
        }

        [Test]
        public void Owner() {
            var issue = EntityFactory.Create(() => SandboxProject.CreateIssue("Issue with no Owner"));
            Assert.IsNull(issue.Owner);
        }

        [Test]
        public void CreateIssue() {
            var attributes = new Dictionary<string, object> {{"Description", "Test creation method with required attributes"}};

            var issue = EntityFactory.Create(() => SandboxProject.CreateIssue("Issue with no Owner", attributes));
            Assert.IsNull(issue.Owner);
            Assert.AreEqual(issue.Description, "Test creation method with required attributes");
        }


        [Test]
        public void EpicsAssignedToCurrentIssue() {
            const string issueName = "Issue name";
            const string epicName = "Epic Name";

            var issue = EntityFactory.Create(() => Instance.Create.Issue(issueName, SandboxProject));

            var epic = EntityFactory.CreateEpic(epicName, SandboxProject);
            epic.Issues.Add(issue);
            epic.Save();

            ResetInstance();

            var epics = issue.GetEpics(null);
            Assert.AreEqual(1, epics.Count);
            Assert.AreEqual(epic, First(epics));
        }

        [Test]
        public void BlockedEpicsIssue() {
            const string issueName = "Issue name";
            const string epicName = "Epic Name";

            var issue = EntityFactory.Create(() => Instance.Create.Issue(issueName, SandboxProject));

            var epic = EntityFactory.CreateEpic(epicName, SandboxProject);
            epic.BlockingIssues.Add(issue);
            epic.Save();

            ResetInstance();

            var blockedEpics = issue.GetBlockedEpics(null);
            Assert.AreEqual(1, blockedEpics.Count);
            Assert.AreEqual(epic, First(blockedEpics));
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}
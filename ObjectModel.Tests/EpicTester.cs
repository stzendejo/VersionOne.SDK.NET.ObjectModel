using System;

using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;
namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class EpicTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {                        
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void BlockingIssue() {
            var issue = EntityFactory.Create(() => Instance.Create.Issue("Test issue", SandboxProject));
            var epic = EntityFactory.CreateEpic("test epic", SandboxProject);
            epic.BlockingIssues.Add(issue);
            epic.Save();

            ResetInstance();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(1, actualEpic.BlockingIssues.Count);
            CollectionAssert.Contains(actualEpic.BlockingIssues, issue);
        }

        [Test]
        [Ignore("MorphedFrom doesn't contain story which was used to make epic.")]
        public void MorphedFrom() {
            var storyName = "Story test" + Guid.NewGuid();
            var story = EntityFactory.CreateStory(storyName, SandboxProject);
            story.Breakdown();            

            ResetInstance();

            var filter = new EpicFilter();
            filter.Name.Add(storyName);
            var epics = Instance.Get.Epics(filter);

            Assert.AreEqual(1, epics.Count);
            var epic = First(epics);
            //Assert.AreEqual(story, epic.MorphedFrom);

            epic.Delete();
        }

        [Test]
        public void Swag() {
            const double swag = 10.5;
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Swag = swag;
            epic.Save();

            ResetInstance();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(swag, actualEpic.Swag);
        }

        [Test]
        public void NullSwag() {
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Swag = null;
            epic.Save();

            ResetInstance();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(null, actualEpic.Swag);
        }

        [Test]
        public void Value() {
            const double value = 11.2;
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Value = value;
            epic.Save();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(value, actualEpic.Value);
        }

        [Test]
        public void NullValue() {
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Value = null;
            epic.Save();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(null, actualEpic.Value);
        }

        [Test]
        public void DefaultRankOrder() {
            var epic1 = EntityFactory.CreateEpic("Epic test 1", SandboxProject);
            var epic2 = EntityFactory.CreateEpic("Epic test 2", SandboxProject);
            Assert.IsTrue(epic1.RankOrder.IsAbove(epic2));
        }

        [Test]
        public void UpdateRankOrder() {
            var epic1 = EntityFactory.CreateEpic("Epic test 1", SandboxProject);
            var epic2 = EntityFactory.CreateEpic("Epic test 2", SandboxProject);

            epic1.RankOrder.SetBelow(epic2);
            epic1.Save();

            Assert.IsTrue(epic2.RankOrder.IsAbove(epic1));
        }

        [Test]
        public void CanGenerateChildEpic() {
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            Assert.IsTrue(epic.CanGenerateChildEpic);
        }

        [Test]
        public void CannotGenerateChildEpic() {
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Close();
            Assert.IsFalse(epic.CanGenerateChildEpic);
        }

        [Test]
        public void CanGenerateChildStory() {
            Epic epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            Assert.IsTrue(epic.CanGenerateChildStory);            
        }

        [Test]
        public void CannotGenerateChildStory() {
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Close();
            Assert.IsFalse(epic.CanGenerateChildStory);
        }

        [Test]
        public void Super() {
            var epic1 = EntityFactory.CreateEpic("Epic test 1", SandboxProject);
            var subEpic = EntityFactory.Create(epic1.GenerateChildEpic);

            Assert.AreEqual(epic1, subEpic.Super);
        }

        [Test]
        public void Risk() {
            const double value = 5.4;
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Risk = value;
            epic.Save();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(value, actualEpic.Risk);
        }

        [Test]
        [ExpectedException(typeof(DataException))]
        public void RiskGreaterThanUpperLimit() {
            const double value = 10.5;
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Risk = value;
            epic.Save();
        }

        [Test]
        [ExpectedException(typeof(DataException))]
        public void RiskLessThanLowerLimit() {
            const double value = -0.4;
            var epic = EntityFactory.CreateEpic("Epic test", SandboxProject);
            epic.Risk = value;
            epic.Save();
        }

        [Test]
        public void TestAsEpicChildren() {
            var epic = EntityFactory.CreateEpic("Epic test 1", SandboxProject);
            var test = EntityFactory.CreateTest("test for epic", epic);

            ResetInstance();

            var actualEpic = Instance.Get.EpicByID(epic.ID);
            Assert.AreEqual(test, First(actualEpic.GetChildTests(null)));
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}
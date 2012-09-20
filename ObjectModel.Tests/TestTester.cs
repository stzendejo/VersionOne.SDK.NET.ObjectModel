using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class TestTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void GenerateRegressionTest() {
            const string storyName = "story name";
            const string testName = "test name";

            var story = EntityFactory.CreateStory(storyName, SandboxProject);
            var test = EntityFactory.CreateTest(testName, story);
            var regressionTest = test.GenerateRegressionTest();
            EntityFactory.RegisterForDisposal(regressionTest);

            Assert.AreEqual(testName, regressionTest.Name);

            ResetInstance();

            var regressionTestNew = Instance.Get.RegressionTestByID(regressionTest.ID);
            Assert.AreEqual(testName, regressionTestNew.Name);
            Assert.AreEqual(SandboxProject, regressionTestNew.Project);
            Assert.AreEqual(test, regressionTestNew.GeneratedFrom);

            var member1 = EntityFactory.CreateMember("member name 1");
            var member2 = EntityFactory.CreateMember("member name 2");

            test.Owners.Add(member1);
            test.Owners.Add(member2);
            test.Save();

            var regressionTest2 = test.GenerateRegressionTest();
            EntityFactory.RegisterForDisposal(regressionTest2);
            Assert.AreEqual(2, regressionTest2.Owners.Count);
            CollectionAssert.Contains(regressionTest2.Owners, member1);
            CollectionAssert.Contains(regressionTest2.Owners, member2);

            ResetInstance();

            var regressionTestNew2 = Instance.Get.RegressionTestByID(regressionTest2.ID);
            Assert.AreEqual(test, regressionTestNew.GeneratedFrom);
            Assert.AreEqual(2, regressionTestNew2.Owners.Count);
            CollectionAssert.Contains(regressionTest2.Owners, member1);
            CollectionAssert.Contains(regressionTest2.Owners, member2);
        }

        [Test]
        public void CreateTestUnderStory() {
            var story = EntityFactory.CreateStory("Story", SandboxProject);
            var test = EntityFactory.CreateTest("Test for story", story);
            ResetInstance();

            var updatedStory = Instance.Get.StoryByID(story.ID);
            var children = updatedStory.GetSecondaryWorkitems(null);

            Assert.AreEqual(1, children.Count);
            Assert.AreEqual(test, First(children));
        }

        [Test]
        public void MoveTestFromOneEpicToAnother() {
            var epic1 = EntityFactory.CreateEpic("first epic", SandboxProject);
            var epic2 = EntityFactory.CreateEpic("second epic", SandboxProject);

            var test = EntityFactory.CreateTest("test for epic", epic1);
            ResetInstance();

            var tests = epic1.GetChildTests(null);
            var testFromFirstEpic = First(tests);
            testFromFirstEpic.Parent = epic2;
            testFromFirstEpic.Save();

            ResetInstance();

            var testsFromSecondEpic = epic2.GetChildTests(null);
            Assert.AreEqual(1, testsFromSecondEpic.Count);
            Assert.AreEqual(test, First(testsFromSecondEpic));
        }

        [Test]
        public void MoveTestFromStoryToEpic() {
            var epic = EntityFactory.CreateEpic("epic", SandboxProject);
            var story = EntityFactory.CreateEpic("story", SandboxProject);

            var test = EntityFactory.CreateTest("test for story", story);
            ResetInstance();

            var tests = story.GetChildTests(null);
            var testFromStory = First(tests);
            testFromStory.Parent = epic;
            testFromStory.Save();

            ResetInstance();

            var testsFromEpic = epic.GetChildTests(null);
            Assert.AreEqual(1, testsFromEpic.Count);
            Assert.AreEqual(test, First(testsFromEpic));            
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}
using System.Collections.Generic;

using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class RegressionTestTester : BaseSDKTester {
        private const string RegressionPlanName = "regression plan test";
        private const string RegressionTestName = "My Regression Test";

        [Test]
        public void Create() {
            var regressionTest = EntityFactory.CreateRegressionTest(RegressionPlanName, SandboxProject);
            Assert.AreEqual(RegressionPlanName, regressionTest.Name);

            ResetInstance();

            var regressionTestNew = Instance.Get.RegressionTestByID(regressionTest.ID);
            Assert.AreEqual(RegressionPlanName, regressionTestNew.Name);
            Assert.AreEqual(SandboxProject, regressionTestNew.Project);
        }

        [Test]
        public void CreateWithAttributes() {
            const string tags = "test tag regression";
            var attributes = new Dictionary<string, object> {{"Tags", tags}};

            var regressionTest = EntityFactory.CreateRegressionTest(RegressionPlanName, SandboxProject, attributes);
            Assert.AreEqual(RegressionPlanName, regressionTest.Name);
            var member1 = EntityFactory.CreateMember("member name 1");
            var member2 = EntityFactory.CreateMember("member name 2");

            regressionTest.Owners.Add(member1);
            regressionTest.Owners.Add(member2);
            regressionTest.Save();

            ResetInstance();

            var regressionTestNew = Instance.Get.RegressionTestByID(regressionTest.ID);
            Assert.AreEqual(RegressionPlanName, regressionTestNew.Name);
            Assert.AreEqual(SandboxProject, regressionTestNew.Project);
            Assert.AreEqual(tags, regressionTestNew.Tags);
            Assert.AreEqual(2, regressionTestNew.Owners.Count);
        }

        [Test]
        public void CreateBasedOnTest() {
            const string storyName = "story name";
            const string testName = "test name";

            var story = EntityFactory.CreateStory(storyName, SandboxProject);
            var test = EntityFactory.CreateTest(testName, story);
            var regressionTest = EntityFactory.CreateRegressionTest(test);

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

            var regressionTest2 = EntityFactory.CreateRegressionTest(test);
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
        public void GetFromServer() {
            var test = Instance.Create.RegressionTest(RegressionTestName, SandboxProject);

            ResetInstance();

            var tests = Instance.Get.RegressionTests(null);
            CollectionAssert.Contains(tests, test);
        }

        [Test]
        public void GetFromServerById() {
            var test = Instance.Create.RegressionTest(RegressionTestName, SandboxProject);

            ResetInstance();

            var queriedTest = Instance.Get.RegressionTestByID(test.ID);
            Assert.AreEqual(test, queriedTest);
            Assert.AreEqual(test.Name, queriedTest.Name);
        }

        [Test]
        public void Update() {
            const string newName = "New name";
            var test = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);

            var newOwner = EntityFactory.CreateMember("user");
            test.Owners.Clear();
            test.Owners.Add(newOwner);
            test.Name = newName;
            test.Save();

            ResetInstance();

            var testWithChanges = Instance.Get.RegressionTestByID(test.ID);
            Assert.AreEqual(testWithChanges.Name, newName);
            Assert.IsTrue(testWithChanges.Owners.Count == 1);
            CollectionAssert.Contains(testWithChanges.Owners, newOwner);
        }

        [Test]
        public void Delete() {
            var test = Instance.Create.RegressionTest(RegressionTestName, SandboxProject);

            var testWithChanges = Instance.Get.RegressionTestByID(test.ID);
            Assert.IsNotNull(testWithChanges);

            ResetInstance();

            testWithChanges.Delete();
            Assert.IsNull(Instance.Get.RegressionTestByID(testWithChanges.ID));
        }

        [Test]
        public void CanClose() {
            var regressionTest = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);

            Assert.IsTrue(regressionTest.CanClose);
            Assert.IsFalse(regressionTest.IsClosed);

            regressionTest.Close();

            Assert.IsFalse(regressionTest.CanClose);
            Assert.IsTrue(regressionTest.IsClosed);
        }

        [Test]
        public void CanDelete() {
            var regressionTest = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);

            Assert.IsTrue(regressionTest.CanDelete);

            regressionTest.Close();

            Assert.IsFalse(regressionTest.CanDelete);
        }

        [Test]
        public void CanReactivate() {
            var regressionTest = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);

            Assert.IsTrue(regressionTest.CanDelete);
            Assert.IsFalse(regressionTest.CanReactivate);
            Assert.IsTrue(regressionTest.IsActive);

            regressionTest.Close();

            Assert.IsFalse(regressionTest.CanDelete);
            Assert.IsTrue(regressionTest.CanReactivate);
            Assert.IsFalse(regressionTest.IsActive);

            regressionTest.Reactivate();

            Assert.IsTrue(regressionTest.CanDelete);
            Assert.IsFalse(regressionTest.CanReactivate);
            Assert.IsTrue(regressionTest.IsActive);
            Assert.IsFalse(regressionTest.IsClosed);
        }    
    }
}

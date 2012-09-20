using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class TestSetTester : BaseSDKTester {
        private const string TestSetName = "test set 1";
        private const string TestSetDescription = "test set 1 description";
        private const string TestSetDescriptionUpdated = "test set 1 description (with slight modifications)";

        public RegressionSuite GetRegressionSuite() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("test plan", SandboxProject);
            return EntityFactory.CreateRegressionSuite("suite 1", regressionPlan);
        }

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void CreateTestSetTest() {
            var regressionSuite = GetRegressionSuite();
            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite);
            var filter = new TestSetFilter();
            filter.RegressionSuite.Add(regressionSuite);
            filter.Project.Add(SandboxProject);

            ResetInstance();

            var testSets = new List<TestSet>(Instance.Get.TestSets(filter));
            CollectionAssert.Contains(testSets, testSet);
            Assert.IsTrue(testSets[0].Project.Equals(SandboxProject));
            Assert.IsTrue(testSets[0].RegressionSuite.Equals(regressionSuite));
        }

        [Test]
        public void CreateTestSetWithAttributesTest() {
            var regressionSuite = GetRegressionSuite();
            var attributes = new Dictionary<string, object> {{"Description", TestSetDescription}};

            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite, attributes);

            ResetInstance();

            var filter = new TestSetFilter();
            filter.RegressionSuite.Add(regressionSuite);
            filter.Project.Add(SandboxProject);

            var testSets = new List<TestSet>(Instance.Get.TestSets(filter));
            CollectionAssert.Contains(testSets, testSet);
            Assert.IsTrue(testSets[0].Project.Equals(SandboxProject));
            Assert.IsTrue(testSets[0].RegressionSuite.Equals(regressionSuite));
        }

        [Test]
        public void UpdateTestSetTest() {
            var regressionSuite = GetRegressionSuite();
            var attributes = new Dictionary<string, object> {{"Description", TestSetDescription}};

            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite, attributes);
            testSet.Description = TestSetDescriptionUpdated;
            testSet.Save();

            ResetInstance();

            var queriedTestSet = Instance.Get.TestSetByDisplayID(testSet.DisplayID);
            Assert.IsTrue(queriedTestSet.Description.Equals(TestSetDescriptionUpdated));
        }

        [Test]
        public void DeleteTestSetTest() {
            var regressionSuite = GetRegressionSuite();
            var attributes = new Dictionary<string, object> {{"Description", TestSetDescription}};

            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite, attributes);

            ResetInstance();

            var filter = new TestSetFilter();
            filter.RegressionSuite.Add(regressionSuite);
            filter.Project.Add(SandboxProject);
            var testSets = Instance.Get.TestSets(filter);

            Assert.IsTrue(testSets.Count == 1);

            testSet.Delete();
            ResetInstance();

            testSets = Instance.Get.TestSets(filter);
            Assert.IsTrue(testSets.Count == 0);
        }

        [Test]
        public void CreateTestSetWithEnvironmentTest() {
            var regressionSuite = GetRegressionSuite();
            var environment = CreateEnvironment("Environment for TestSet", null);
            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite);
            testSet.Environment = environment;
            testSet.Save();

            var filter = new TestSetFilter();
            filter.Environment.Add(environment);
            filter.Project.Add(SandboxProject);

            ResetInstance();

            var testSets = new List<TestSet>(Instance.Get.TestSets(filter));
            CollectionAssert.Contains(testSets, testSet);
            Assert.IsTrue(testSets[0].Project.Equals(SandboxProject));
            Assert.IsTrue(testSets[0].Environment.Equals(environment));
        }

        [Test]
        public void EnvironmentTest() {
            var env = GetEnvironment();
            var regressionSuite = GetRegressionSuite();
            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite);
            ResetInstance();

            var newTestSet = Instance.Get.TestSetByID(testSet.ID);
            Assert.IsNull(newTestSet.Environment);

            testSet.Environment = env;
            testSet.Save();

            ResetInstance();
            newTestSet = Instance.Get.TestSetByID(testSet.ID);

            Assert.IsNotNull(env);
            Assert.AreEqual(env, newTestSet.Environment);
        }

        [Test]
        public void CopyAcceptanceTestsFromRegressionSuiteTest() {
            const string firstTestName = "test 1";
            const string secondTestName = "test 2";

            var regressionSuite = GetRegressionSuite();
            var firstTest = EntityFactory.CreateRegressionTest(firstTestName, SandboxProject);
            var secondTest = EntityFactory.CreateRegressionTest(secondTestName, SandboxProject);
            regressionSuite.RegressionTests.Add(firstTest);
            regressionSuite.RegressionTests.Add(secondTest);

            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite);
            testSet.CopyAcceptanceTestsFromRegressionSuite();

            ResetInstance();

            var filter = new TestFilter();
            filter.Parent.Add(testSet);
            var createdTests = Instance.Get.Tests(filter);
            Assert.AreEqual(2, createdTests.Count);

            Assert.IsTrue(ContainsRegressionTestReference(createdTests, firstTest));
            Assert.IsTrue(ContainsRegressionTestReference(createdTests, secondTest));
        }

        [Test]
        public void CanReactivateTest() {
            var regressionSuite = GetRegressionSuite();
            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite);

            Assert.IsFalse(testSet.CanReactivate);
            Assert.IsTrue(testSet.IsActive);

            testSet.Close();

            Assert.IsTrue(testSet.CanReactivate);
            Assert.IsFalse(testSet.IsActive);

            Assert.IsFalse(testSet.CanClose);
            Assert.IsTrue(testSet.IsClosed);

            testSet.Reactivate();

            Assert.IsFalse(testSet.CanReactivate);
            Assert.IsTrue(testSet.IsActive);

            Assert.IsTrue(testSet.CanClose);
            Assert.IsFalse(testSet.IsClosed);
        }

        [Test]
        public void CanCloseTest() {
            var regressionSuite = GetRegressionSuite();
            var testSet = EntityFactory.CreateTestSet(TestSetName, regressionSuite);

            Assert.IsTrue(testSet.CanDelete);

            testSet.Close();

            Assert.IsFalse(testSet.CanDelete);

            testSet.Reactivate();

            Assert.IsTrue(testSet.CanDelete);
        }

        private static bool ContainsRegressionTestReference(IEnumerable<Test> tests, RegressionTest referenceRegressionTest) {
            var generatedTests = referenceRegressionTest.GetGeneratedTests();
            return tests.Any(generatedTests.Contains);
            }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}

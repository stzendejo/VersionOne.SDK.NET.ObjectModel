using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class TestSetFilterTester : BaseSDKTester {
        private const string TestSetName = "test set 1";

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        public RegressionPlan CreateRegressionPlan() {
            return EntityFactory.CreateRegressionPlan("test plan", SandboxProject);
        }

        private RegressionSuite CreateRegressionSuite(string name, RegressionPlan regressionPlan) {
            return EntityFactory.CreateRegressionSuite(name, regressionPlan);
        }

        [Test]
        public void GetTestSetsWithNullFilterTest() {
            var plan = CreateRegressionPlan();
            var beforeCreation = Instance.Get.TestSets(null);
            var testSet = EntityFactory.Create(() => Instance.Create.TestSet(TestSetName, CreateRegressionSuite("Suite", plan), SandboxProject));
            var afterCreation = Instance.Get.TestSets(null);

            Assert.IsTrue(afterCreation.Count - beforeCreation.Count == 1);
            CollectionAssert.DoesNotContain(beforeCreation, testSet);
            CollectionAssert.Contains(afterCreation, testSet);
        }

        [Test]
        public void FilterByRegressionSuiteTest() {
            var plan = CreateRegressionPlan();
            var suite1 = CreateRegressionSuite("Suite 1", plan);
            var suite2 = CreateRegressionSuite("Suite 2", plan);
            EntityFactory.Create(() => Instance.Create.TestSet(TestSetName, suite1, SandboxProject));

            var filter = new TestSetFilter();

            filter.RegressionSuite.Add(suite2);

            var testSets = Instance.Get.TestSets(filter);
            Assert.AreEqual(0, testSets.Count);

            filter.RegressionSuite.Add(suite1);

            testSets = Instance.Get.TestSets(filter);
            Assert.AreEqual(1, testSets.Count);

            filter.RegressionSuite.Remove(suite2);

            testSets = Instance.Get.TestSets(filter);
            Assert.AreEqual(1, testSets.Count);
        }

        [Test]
        public void FilterByEnvironmentTest() {
            var plan = CreateRegressionPlan();
            var suite = CreateRegressionSuite("Suite", plan);

            var matchingEnvironment = CreateEnvironment("Windows 7 x64", null);
            var nonMatchingEnvironment = CreateEnvironment("Windows 7 x86", null);
            var testSet = EntityFactory.CreateTestSet(TestSetName, suite);
            testSet.Environment = matchingEnvironment;
            testSet.Save();

            ResetInstance();

            var filter = new TestSetFilter();
            filter.Environment.Add(matchingEnvironment);
            var testSets = Instance.Get.TestSets(filter);
            CollectionAssert.Contains(testSets, testSet);

            filter.Environment.Clear();
            filter.Environment.Add(nonMatchingEnvironment);
            testSets = Instance.Get.TestSets(filter);
            CollectionAssert.DoesNotContain(testSets, testSet);

            matchingEnvironment.Close();
            nonMatchingEnvironment.Close();
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}

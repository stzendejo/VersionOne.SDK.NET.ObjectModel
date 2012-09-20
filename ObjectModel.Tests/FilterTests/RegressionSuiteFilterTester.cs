using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class RegressionSuiteFilterTester : BaseSDKTester {
        private const string RegressionSuiteName = "Regression Suite";

        [TearDown]
        public new void TearDown() 
        {
            EntityFactory.Dispose();
        }

        [Test]
        public void GetRegressionSuiteWithNullFilter() {
            var suites = Instance.Get.RegressionSuites(null);
            var plan = EntityFactory.CreateRegressionPlan("RegPlan", SandboxProject);
            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan);
            var updatedSuites = Instance.Get.RegressionSuites(null);

            Assert.AreEqual(1, updatedSuites.Count - suites.Count);
            CollectionAssert.Contains(updatedSuites, suite);
        }

        [Test]
        public void GetRegressionSuitesByOwner() {
            var owner = EntityFactory.CreateMember("SuiteOwner");
            var nobody = EntityFactory.CreateMember("OtherOwner");

            var attributes = new Dictionary<string, object>();
            var plan = EntityFactory.CreateRegressionPlan("RegPlan", SandboxProject, attributes);

            attributes.Add("Owner", Oid.FromToken(owner.ID, Instance.ApiClient.MetaModel));
            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan, attributes);

            var filter = new RegressionSuiteFilter();
            filter.Owners.Add(owner);
            var suites = Instance.Get.RegressionSuites(filter);
            Assert.AreEqual(1, suites.Count);
            CollectionAssert.Contains(suites, suite);

            filter = new RegressionSuiteFilter();
            filter.Owners.Add(nobody);
            suites = Instance.Get.RegressionSuites(filter);
            Assert.AreEqual(0, suites.Count);
        }

        [Test]
        public void GetRegressionSuitesWithSpecificRegressionPlan() {
            var plan = EntityFactory.CreateRegressionPlan("RegPlan", SandboxProject);
            var plan2 = EntityFactory.CreateRegressionPlan("RegPlanFake", SandboxProject);
            var attributes = new Dictionary<string, object> {
                { "RegressionPlan", Oid.FromToken(plan.ID, Instance.ApiClient.MetaModel) }
            };

            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan, attributes);

            var filter = new RegressionSuiteFilter();
            filter.RegressionPlan.Add(plan);
            var suites = Instance.Get.RegressionSuites(filter);
            Assert.AreEqual(1, suites.Count);
            CollectionAssert.Contains(suites, suite);

            filter = new RegressionSuiteFilter();
            filter.RegressionPlan.Add(plan2);
            suites = Instance.Get.RegressionSuites(filter);
            Assert.AreEqual(0, suites.Count);
        }

        [Test]
        public void GetRegressionSuitesByPlan() {
            var plan = EntityFactory.CreateRegressionPlan("RegPlan", SandboxProject);
            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan);

            var filter = new RegressionSuiteFilter();
            filter.RegressionPlan.Add(plan);
            var suites = Instance.Get.RegressionSuites(filter);
            Assert.IsTrue(suites.Count > 0);
            CollectionAssert.Contains(suites, suite);
        }

        [Test]
        public void GetRegressionSuitesByEstimate() {
            double? estimate = 14;

            var plan = EntityFactory.CreateRegressionPlan("RegEstimatePlan", SandboxProject);

            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Estimate", estimate);
            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan, attributes);

            var filter = new RegressionSuiteFilter();
            filter.Estimate.AddTerm(FilterTerm.Operator.Equal, estimate);
            var suites = Instance.Get.RegressionSuites(filter);
            Assert.IsTrue(suites.Count > 0);
            CollectionAssert.Contains(suites, suite);

            filter = new RegressionSuiteFilter();
            filter.Estimate.Range(12, 16);
            suites = Instance.Get.RegressionSuites(filter);
            Assert.IsTrue(suites.Count > 0);
            CollectionAssert.Contains(suites, suite);

            filter = new RegressionSuiteFilter();
            filter.Estimate.Range(12, 13);
            suites = Instance.Get.RegressionSuites(filter);
            CollectionAssert.DoesNotContain(suites, suite);
        }

        [Test]
        public void GetRegressionSuitesWithReference() {
            const string reference = "TestRefence-1010";

            var plan = EntityFactory.CreateRegressionPlan("RegressionPlanWithReference", SandboxProject);
            var attributes = new Dictionary<string, object> {{"Reference", reference}};

            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan, attributes);

            var filter = new RegressionSuiteFilter();
                filter.Reference.Add(reference);
            var suites = Instance.Get.RegressionSuites(filter);
                Assert.AreEqual(1, suites.Count);
            CollectionAssert.Contains(suites, suite);

                filter = new RegressionSuiteFilter();
                filter.Reference.Add("WrongReference");
                suites = Instance.Get.RegressionSuites(filter);
                Assert.AreEqual(0, suites.Count);
        }

        [Test]
        public void GetRegressionSuitesByID() {
            var plan = EntityFactory.CreateRegressionPlan("RegressionPlanWithRNumber", SandboxProject);

            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan);
            var displayId = suite.DisplayID;

            var filter = new RegressionSuiteFilter();
            filter.DisplayID.Add(displayId);
            var suites = Instance.Get.RegressionSuites(filter);
            Assert.AreEqual(1, suites.Count);
            CollectionAssert.Contains(suites, suite);

            filter = new RegressionSuiteFilter();
            filter.DisplayID.Add("WrongNumber");
            suites = Instance.Get.RegressionSuites(filter);
            Assert.AreEqual(0, suites.Count);
        }

        [TestFixtureTearDown]
        public void DeleteScope() {
            SandboxProject.Delete();
        }
    }
}

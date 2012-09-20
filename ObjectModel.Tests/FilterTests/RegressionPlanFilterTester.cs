using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;
using NUnit.Framework;

using Assert=NUnit.Framework.Assert;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class RegressionPlanFilterTester : BaseSDKTester {
        private const string RegressionPlanName = "regression plan";

        [TearDown]
        public new void TearDown() 
        {
            EntityFactory.Dispose();
        }

        [Test]
        public void GetRegressionPlanWithNullFilterTest() {
            var plans = Instance.Get.RegressionPlans(null);
            var plan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            var updatedPlans = Instance.Get.RegressionPlans(null);

            Assert.AreEqual(1, updatedPlans.Count - plans.Count);
            CollectionAssert.Contains(updatedPlans, plan);
        }

        [Test]
        public void GetRegressionPlanWithValuableFilterTest() {
            var owner = EntityFactory.CreateMember("Paul");
            var nobody = EntityFactory.CreateMember("Mike");

            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Owner", Oid.FromToken(owner.ID, Instance.ApiClient.MetaModel));

            var plan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject, attributes);

            var filter = new RegressionPlanFilter();
            filter.Project.Add(SandboxProject);
            filter.Owners.Add(owner);
            var plans = Instance.Get.RegressionPlans(filter);
            Assert.AreEqual(1, plans.Count);
            CollectionAssert.Contains(plans, plan);

            filter = new RegressionPlanFilter();
            filter.Project.Add(SandboxProject);
            filter.Owners.Add(nobody);
            plans = Instance.Get.RegressionPlans(filter);
            Assert.AreEqual(0, plans.Count);
        }

        [Test]
        public void GetRegressionPlanByNullFilter() {
            var plan1 = EntityFactory.CreateRegressionPlan(RegressionPlanName + "1", SandboxProject, null);
            var plan2 = EntityFactory.CreateRegressionPlan(RegressionPlanName + "2", SandboxProject, null);

            var plans = Instance.Get.RegressionPlans(null);

            CollectionAssert.Contains(plans, plan1);
            CollectionAssert.Contains(plans, plan2);
        }

        [TestFixtureTearDown]
        public void DeleteScope() {
            SandboxProject.Delete();
        }
    }
}

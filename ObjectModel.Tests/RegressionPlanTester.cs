using System;
using System.Collections.Generic;

using NUnit.Framework;

using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class RegressionPlanTester : BaseSDKTester {
        private const string RegressionPlanName = "regression plan";
        private const string Description = "description for regression plan";
        private const string Reference = "my reference";

        [Test]
        public void CreateRegressionPlan() {
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            Assert.AreEqual(RegressionPlanName, regressionPlan.Name);
        }

        [Test]
        public void CreateRegressionPlanWithAttributes() {
            var member = EntityFactory.CreateMember("test user");

            var attributes = new Dictionary<string, object> {
                {"Description", Description},
                {"Reference", Reference},
                {"Owner", Oid.FromToken(member.ID, Instance.ApiClient.MetaModel)}
            };
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject, attributes);

            Assert.AreEqual(RegressionPlanName, regressionPlan.Name);
            Assert.AreEqual(Description, regressionPlan.Description);
            Assert.AreEqual(Reference, regressionPlan.Reference);
            Assert.AreEqual(member, regressionPlan.Owner);
        }

        [Test]
        public void UpdateRegressionPlanTest() {
            const string addonForName = "updated";

            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);

            ResetInstance();

            var member = EntityFactory.CreateMember("test user");
            var regressionPlanNew = Instance.Get.RegressionPlanByID(regressionPlan.ID);
            regressionPlanNew.Name = RegressionPlanName + addonForName;
            regressionPlanNew.Description = Description;
            regressionPlanNew.Reference = Reference;
            regressionPlanNew.Owner = member;
            regressionPlanNew.Save();

            ResetInstance();

            regressionPlan = Instance.Get.RegressionPlanByID(regressionPlan.ID);
            Assert.AreEqual(RegressionPlanName + addonForName, regressionPlan.Name);
            Assert.AreEqual(Description, regressionPlan.Description);
            Assert.AreEqual(Reference, regressionPlan.Reference);
            Assert.AreEqual(member, regressionPlan.Owner);
        }

        [Test]
        public void DeleteRegressionPlanTest() {
            var regressionPlan = Instance.Create.RegressionPlan(RegressionPlanName, SandboxProject);

            ResetInstance();

            var regressionPlanNew = Instance.Get.RegressionPlanByID(regressionPlan.ID);
            regressionPlanNew.Delete();

            regressionPlanNew = Instance.Get.RegressionPlanByID(regressionPlan.ID);
            Assert.IsNull(regressionPlanNew);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanNotCloseTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            Assert.IsTrue(regressionPlan.IsActive);
            Assert.IsFalse(regressionPlan.IsClosed);

            Assert.IsFalse(regressionPlan.CanClose);
            Assert.IsTrue(regressionPlan.CanDelete);

            regressionPlan.Close();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanNotReactivateTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            Assert.IsFalse(regressionPlan.CanReactivate);
            regressionPlan.Reactivate();
        }

        [TestFixtureTearDown]
        public void DeleteScope() {
            SandboxProject.Delete();
        }
    }
}
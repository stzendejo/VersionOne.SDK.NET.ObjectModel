using System;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class EnvironmentFilterTester : BaseSDKTester {
        private const string RegressionSuiteName = "Regression Suite";
        private const string EnvironmentName = "Environment for test";

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        protected override Schedule CreateSandboxSchedule() {
            return EntityFactory.Create(() => Instance.Create.Schedule(SandboxName, TimeSpan.FromDays(14), TimeSpan.FromDays(0)));
		}

        [Test]
        public void GetEnvironmentWithNullFilterTest() {
            var environments = Instance.Get.Environments(null);
            var environment = CreateEnvironment(EnvironmentName);
            var updatedEnvironments = Instance.Get.Environments(null);

            Assert.AreEqual(1, updatedEnvironments.Count - environments.Count);
            CollectionAssert.Contains(updatedEnvironments, environment);
        }

        [Test]
        public void GetEnvironmentByName() {
            var environment = CreateEnvironment(EnvironmentName);
            var filter = new EnvironmentFilter();
            filter.Name.Add(EnvironmentName);
            
            var environments = Instance.Get.Environments(filter);

            CollectionAssert.Contains(environments, environment);

            filter = new EnvironmentFilter();
            filter.Name.Add("WrongName");
            
            environments = Instance.Get.Environments(filter);
            
            Assert.AreEqual(0, environments.Count);
        }


        [Test]
        public void GetEnvironmentWithSpecificProjectTest() {
            var project2 = EntityFactory.Create(() => Instance.Create.Project("other", SandboxProject.ParentProject, DateTime.Now, SandboxSchedule));
            var environment = CreateEnvironment(EnvironmentName);
            var filter = new EnvironmentFilter();
            filter.Project.Add(SandboxProject);

            var environments = Instance.Get.Environments(filter);

            CollectionAssert.Contains(environments, environment);

            filter = new EnvironmentFilter();
            filter.Project.Add(project2);

            environments = Instance.Get.Environments(filter);

            Assert.AreEqual(0, environments.Count);
        }

        [Test]
        public void GetEnvironmentByPlan() {
            var plan = EntityFactory.CreateRegressionPlan("RegPlan", SandboxProject);
            var suite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, plan);
            var filter = new RegressionSuiteFilter();
            filter.RegressionPlan.Add(plan);

            var suites = Instance.Get.RegressionSuites(filter);

            Assert.IsTrue(suites.Count > 0);
            CollectionAssert.Contains(suites, suite);
        }

        [Test]
        public void GetEnvironmentByID() {
            var environment = CreateEnvironment(EnvironmentName);
            var displayId = environment.DisplayID;
            var filter = new EnvironmentFilter();
            filter.DisplayID.Add(displayId);
            
            var environments = Instance.Get.Environments(filter);

            Assert.AreEqual(1, environments.Count);
            CollectionAssert.Contains(environments, environment);

            filter = new EnvironmentFilter();
            filter.DisplayID.Add("WrongNumber");
            
            environments = Instance.Get.Environments(filter);
            
            Assert.AreEqual(0, environments.Count);
        }

        private Environment CreateEnvironment(string name) {
            return CreateEnvironment(name, new Dictionary<string, object>());
        }

        [TearDown]
        public new void TearDown()
        {
            EntityFactory.Dispose();
            NewSandboxProject();
        }
    }
}

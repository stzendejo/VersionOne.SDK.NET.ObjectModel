using NUnit.Framework;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class EnvironmentTester : BaseSDKTester {

        private const string EnvironmentName = "Environment 1";
        private const string EnvironmentNameUpdated = "Environment 1 Upd";

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void CreateEnvironmentTest() {
            var environment = CreateEnvironment(EnvironmentName, null);

            var filter = new EnvironmentFilter();
            filter.Name.Add(EnvironmentName);

            ResetInstance();

            var environments = new List<Environment>(Instance.Get.Environments(filter));
            CollectionAssert.Contains(environments, environment);
            Assert.AreEqual(EnvironmentName, environments[0].Name);
        }

        [Test]
        public void CreateEnvironmentWithAttributesTest() {
            const string newName = "AnotherName";
            var newProject = EntityFactory.CreateProject("new project", SandboxProject, null);
            
            var attributes = new Dictionary<string, object> {
                {"Name", newName}, 
                {"Scope", newProject.ID.Token}
            };
            var environment = CreateEnvironment(EnvironmentName, attributes);

            ResetInstance();

            var filter = new EnvironmentFilter();
            filter.Name.Add(newName);
            filter.Project.Add(newProject);

            var environments = new List<Environment>(Instance.Get.Environments(filter));
            CollectionAssert.Contains(environments, environment);
            Assert.AreEqual(newProject, environments[0].Project);
            Assert.AreEqual(newName, environments[0].Name);
        }

        [Test]
        public void UpdateEnvironmentTest() {
            var environment = CreateEnvironment(EnvironmentName, null);
            ResetInstance();

            var environmentNew = Instance.Get.EnvironmentByID(environment.ID);
            Assert.AreEqual(EnvironmentName, environmentNew.Name);
            Assert.AreEqual(environment, environmentNew);
            
            environment.Name = EnvironmentNameUpdated;
            environment.Save();

            var filter = new EnvironmentFilter();
            filter.Name.Add(EnvironmentNameUpdated);
            filter.Project.Add(SandboxProject);

            ResetInstance();

            var environments = new List<Environment>(Instance.Get.Environments(filter));
            Assert.AreEqual(EnvironmentNameUpdated, environments[0].Name);
            CollectionAssert.Contains(environments, environment);
        }
        
        [Test]
        public void GetEnvironmentByIdTest() {
            var environment = CreateEnvironment(EnvironmentName, null);

            ResetInstance();

            var queriedEnvironment = Instance.Get.EnvironmentByID(environment.ID);
            Assert.AreEqual(EnvironmentName, queriedEnvironment.Name);
        }

        [Test]
        public void CanCloseTest() {
            var environment = CreateEnvironment(EnvironmentName, null);

            Assert.IsTrue(environment.CanClose);
            Assert.IsFalse((environment.IsClosed));

            environment.Close();

            Assert.IsFalse(environment.CanClose);
            Assert.IsTrue(environment.IsClosed);
        }

        [Test]
        public void CanReactivateTest() {
            var environment = CreateEnvironment(EnvironmentName, null);

            Assert.IsFalse(environment.CanReactivate);
            Assert.IsTrue(environment.IsActive);

            environment.Close();

            Assert.IsTrue(environment.CanReactivate);
            Assert.IsFalse(environment.IsActive);

            environment.Reactivate();

            Assert.IsFalse(environment.CanReactivate);
            Assert.IsTrue(environment.IsActive);
            Assert.IsFalse(environment.IsClosed);
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}

using System.Linq;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class TrackedEpicFilterTester : BaseSDKTester {
        // TODO change to explicit filter usage or move these tests
        [Test]
        public void GetTrackedEpicsBySingleProject() {
            var project = EntityFactory.CreateProject("tracked epics", SandboxProject, null);

            const string firstTopLevelEpicName = "first top level epic";
            const string secondTopLevelEpicName = "second top level epic";
            const string childEpicName = "child epic";

            var topLevelEpic = EntityFactory.CreateEpic(firstTopLevelEpicName, project);
            EntityFactory.CreateEpic(childEpicName, project, topLevelEpic);
            EntityFactory.CreateEpic(secondTopLevelEpicName, project, null);

            ResetInstance();

            var epics = Instance.Get.TrackedEpics(new[] {project});
            Assert.AreEqual(2, epics.Count);
            Assert.IsTrue(epics.Any(x => x.Name == firstTopLevelEpicName));
            Assert.IsTrue(epics.Any(x => x.Name == secondTopLevelEpicName));
            Assert.IsFalse(epics.Any(x => x.Name == childEpicName));
        }

        [Test]
        public void GetTrackedEpicsByMultipleProjects() {
            var project1 = EntityFactory.CreateProject("tracked epics (1)", SandboxProject, null);
            var project2 = EntityFactory.CreateProject("tracked epics (2)", SandboxProject, null);
            var project3 = EntityFactory.CreateProject("tracked epics (3)", SandboxProject, null);

            const string firstTopLevelEpicName = "first top level epic";
            const string secondTopLevelEpicName = "second top level epic";
            const string thirdTopLevelEpicName = "third top level epic";
            const string childEpicName = "child epic";

            var topLevelEpic = EntityFactory.CreateEpic(firstTopLevelEpicName, project1);
            EntityFactory.CreateEpic(childEpicName, project1, topLevelEpic);
            EntityFactory.CreateEpic(secondTopLevelEpicName, project2, null);
            EntityFactory.CreateEpic(thirdTopLevelEpicName, project3, null);

            ResetInstance();

            var epics = Instance.Get.TrackedEpics(new[] {project1, project2});
            Assert.AreEqual(2, epics.Count);
            Assert.IsTrue(epics.Any(x => x.Name == firstTopLevelEpicName));
            Assert.IsTrue(epics.Any(x => x.Name == secondTopLevelEpicName));
            Assert.IsFalse(epics.Any(x => x.Name == childEpicName));
            Assert.IsFalse(epics.Any(x => x.Name == thirdTopLevelEpicName));

            epics = Instance.Get.TrackedEpics(new[] {project1, project2, project3});
            Assert.AreEqual(3, epics.Count);
            Assert.IsTrue(epics.Any(x => x.Name == firstTopLevelEpicName));
            Assert.IsTrue(epics.Any(x => x.Name == secondTopLevelEpicName));
            Assert.IsFalse(epics.Any(x => x.Name == childEpicName));
            Assert.IsTrue(epics.Any(x => x.Name == thirdTopLevelEpicName));
        }
    }
}
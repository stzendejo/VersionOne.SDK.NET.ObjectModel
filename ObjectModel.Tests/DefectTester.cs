using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class DefectTester : BaseSDKTester {
        [Test]
        public void GetDefectByID() {
            var defect = Instance.Get.DefectByID("Defect:1406");
            Assert.AreEqual("Pick Lists Reversed", defect.Name);
        }

        #region Constants

        private const string DefectName = "Defect #1 Created in Unit Test";
        private const string DefectDescription = "the description";
        private const string DefectFoundBy = "Yogi Bear";
        private const double DefectEstimate = 4.4;

        #endregion

        [Test]
        public void CreateDefect() {
            var id = CreateBasicDefect(SandboxProject).ID;

            ResetInstance();

            var defect = Instance.Get.DefectByID(id);

            Assert.AreEqual(DefectName, defect.Name);
            Assert.AreEqual(DefectDescription, defect.Description);
            Assert.AreEqual(DefectFoundBy, defect.FoundBy);
            Assert.AreEqual(DefectEstimate, defect.Estimate);
            Assert.AreEqual(SandboxProject, defect.Project);

            defect.Delete();
        }

        [Test]
        public void CreateDefectWithRequiredAttributes() {
            const string name = "New Defect";
            const string description = "Test for Goal creation with required attributes";

            var attributes = new Dictionary<string, object>();
            attributes.Add("Description", description);

            Instance.ValidationEnabled = true;

            var defect = SandboxProject.CreateDefect(name, attributes);

            Instance.ValidationEnabled = true;

            ResetInstance();

            defect = Instance.Get.DefectByID(defect.ID);

            Assert.AreEqual(name, defect.Name);
            Assert.AreEqual(description, defect.Description);
            Assert.AreEqual(SandboxProject, defect.Project);

            defect.Delete();
            Instance.ValidationEnabled = false;
        }

        [Test]
        public void CanClose() {
            var defect = CreateBasicDefect(SandboxProject);

            Assert.IsTrue(defect.CanClose);

            defect.Close();

            Assert.IsFalse(defect.CanClose);
        }

        [ExpectedException(typeof (System.InvalidOperationException))]
        [Test]
        public void HonorTrackingLevelToDo() {
            // The V1SDKTests system is assumed to be configured for "Defect:On"
            var defect = SandboxProject.CreateDefect("Honor Tracking Level");

            var cannotHaveToDo = defect.CreateTask("Cannot Have ToDo");

            cannotHaveToDo.ToDo = 10.0; //Should throw
        }

        [ExpectedException(typeof (System.InvalidOperationException))]
        [Test]
        public void HonorTrackingLevelDetailEstimate() {
            // The V1SDKTests system is assumed to be configured for "Defect:On"
            var defect = SandboxProject.CreateDefect("Honor Tracking Level");

            var cannotHaveDetailEstimate = defect.CreateTask("Cannot Have DetailEstimate");

            cannotHaveDetailEstimate.DetailEstimate = 10.0; //Should throw
        }

        [ExpectedException(typeof (System.InvalidOperationException))]
        [Test]
        public void HonorTrackingLevelEffort() {
            // The V1SDKTests system is assumed to be configured for "Defect:On"
            var defect = SandboxProject.CreateDefect("Honor Tracking Level");

            var cannotHaveEffort = defect.CreateTask("Cannot Have Effort");

            cannotHaveEffort.CreateEffort(10.0); // should throw
        }

        private Defect CreateBasicDefect(Project project) {
            var attributes = new Dictionary<string, object>();
            attributes.Add("Description", DefectDescription);
            attributes.Add("Estimate", DefectEstimate);
            attributes.Add("FoundBy", DefectFoundBy);

            Defect defect = project.CreateDefect(DefectName, attributes);
            defect.Save();
            return defect;
        }

        [Test]
        public void RussianAcceptLanguageTest() {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            CreateBasicDefect(SandboxProject);
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [Test]
        public void EnglishAcceptLanguageTest() {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            CreateBasicDefect(SandboxProject);
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
    }
}
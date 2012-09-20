using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class EntityValidatorTester : BaseSDKTester {
        private EntityValidator validator;

        [SetUp]
        public void SetUp() {
            validator = new EntityValidator(Instance);
        }

        [Test]
        public void FieldValidationTest() {
            var defect = Instance.Create.Defect("new defect", SandboxProject);
            Assert.IsTrue(validator.Validate(defect, "Name"));
        }

        [Test]
        [Ignore("we can't set core attributes as null.")]
        public void EntityValidationTest() {
            var defect = Instance.Create.Defect("Defect1", SandboxProject);

            Assert.AreEqual(0, validator.Validate(defect).Count);

            defect.Name = string.Empty;

            var validationErrors = validator.Validate(defect);
            Assert.AreEqual(1, validationErrors.Count);
            Assert.IsTrue(validationErrors.Contains("Name"));
        }

        [Test]
        public void EntityCollectionSuccessfulValidationTest() {
            var defects = new[] {
                Instance.Create.Defect("Defect1", SandboxProject),
                Instance.Create.Defect("Defect2", SandboxProject),
                Instance.Create.Defect("Defect3", SandboxProject),
            };
            var validationResults = validator.Validate(defects);

            foreach (var result in validationResults) {
                Assert.AreEqual(0, result.Value.Count);
            }
        }

        [Test]
        public void ValidationSuccessOnSaveTest() {
            var validationEnabledState = Instance.ValidationEnabled;
            Instance.ValidationEnabled = true;

            try {
                Instance.Create.Defect("Defect1", SandboxProject);
            } catch (EntityValidationException) {
                Assert.Fail("Filled attribute values should not fail validation");
            }

            Instance.ValidationEnabled = validationEnabledState;
        }
    }
}
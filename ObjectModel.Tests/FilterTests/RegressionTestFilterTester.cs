using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class RegressionTestFilterTester : BaseSDKTester {
        private const string RegressionTestName = "My Regression Test";

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void GetRegressionTestWithNullFilterTest() {
            var tests = Instance.Get.RegressionTests(null);
            var newTest = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);
            ResetInstance();

            var updatedTests = Instance.Get.RegressionTests(null);

            Assert.AreEqual(1, updatedTests.Count - tests.Count);
            CollectionAssert.Contains(updatedTests, newTest);
            CollectionAssert.DoesNotContain(tests, newTest);
        }

        [Test]
        public void GetRegressionTestByOwnerTest() {
            var firstUser = EntityFactory.CreateMember("test user");
            var secondUser = EntityFactory.CreateMember("second user");
            var test = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);
            test.Owners.Add(firstUser);
            test.Save();
            ResetInstance();
            var filter = new RegressionTestFilter();
            filter.Owners.Add(secondUser);

            var tests = Instance.Get.RegressionTests(filter);

            CollectionAssert.DoesNotContain(tests, test);

            filter.Owners.Clear();
            filter.Owners.Add(firstUser);

            tests = Instance.Get.RegressionTests(filter);
            CollectionAssert.Contains(tests, test);
        }

        [Test]
        public void GetRegressionTestByTagsTest() {
            const string matchingTag = "match";
            const string nonMatchingTag = "wrong";
            var test = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);
            test.Tags = matchingTag;
            test.Save();
            ResetInstance();
            var filter = new RegressionTestFilter();
            filter.Tags.Add(nonMatchingTag);

            var tests = Instance.Get.RegressionTests(filter);

            CollectionAssert.DoesNotContain(tests, test);

            filter.Tags.Clear();
            filter.Tags.Add(matchingTag);

            tests = Instance.Get.RegressionTests(filter);
            CollectionAssert.Contains(tests, test);
        }

        [Test]
        public void GetRegressionTestByReferenceTest() {
            const string matchingReference = "match";
            const string nonMatchingReference = "wrong";
            var test = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);
            test.Reference = matchingReference;
            test.Save();
            ResetInstance();
            var filter = new RegressionTestFilter();
            filter.Reference.Add(nonMatchingReference);

            var tests = Instance.Get.RegressionTests(filter);

            CollectionAssert.DoesNotContain(tests, test);

            filter.Reference.Clear();
            filter.Reference.Add(matchingReference);
            
            tests = Instance.Get.RegressionTests(filter);
            CollectionAssert.Contains(tests, test);
        }

        [Test]
        [Ignore("There are no statuses configured for Regression Tests by default, so we cannot make it reliable and non-redundant.")]
        public void GetRegressionTestByStatusTest() {
            var test = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);
            var status = test.Status.AllValues[0];
            var wrongStatus = test.Status.AllValues[1];
            test.Status.CurrentValue = status;
            test.Save();
            ResetInstance();
            var filter = new RegressionTestFilter();
            filter.Status.Add(wrongStatus);

            var tests = Instance.Get.RegressionTests(filter);

            CollectionAssert.DoesNotContain(tests, test);

            filter.Status.Clear();
            filter.Status.Add(status);

            tests = Instance.Get.RegressionTests(filter);
            CollectionAssert.Contains(tests, test);
        }

        [Test]
        public void GetRegressionTestByTypeTest() {
            var test = EntityFactory.CreateRegressionTest(RegressionTestName, SandboxProject);
            var type = test.Type.AllValues[0];
            var wrongType = test.Type.AllValues[1];
            test.Type.CurrentValue = type;
            test.Save();
            ResetInstance();
            var filter = new RegressionTestFilter();
            filter.Type.Add(wrongType);

            var tests = Instance.Get.RegressionTests(filter);

            CollectionAssert.DoesNotContain(tests, test);

            filter.Type.Clear();
            filter.Type.Add(type);
            
            tests = Instance.Get.RegressionTests(filter);
            CollectionAssert.Contains(tests, test);
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}

using System;
using System.Collections.Generic;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class RegressionSuiteTester : BaseSDKTester {
        private const string RegressionSuiteName = "Regression Suite Name";
        private const string RegressionSuiteDescription = "Description for Regression Suite";
        private const string RegressionSuiteReference = "Reference for Regression Suite";
        private const string TestSetName = "test set 1";

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [Test]
        public void CreateRegressionSuiteTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);
            Assert.AreEqual(RegressionSuiteName, regressionSuite.Name);
            Assert.AreEqual(regressionPlan, regressionSuite.RegressionPlan);
        }

        [Test]
        public void CreateRegressionSuiteWithAttributes() {
            var member = EntityFactory.CreateMember("test user");
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var attributes = new Dictionary<string, object> {
                {"Description", RegressionSuiteDescription},
                {"Reference", RegressionSuiteReference},
                {"Owner", member.ID.Token}
            };
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan, attributes);

            Assert.AreEqual(RegressionSuiteName, regressionSuite.Name);
            Assert.AreEqual(RegressionSuiteDescription, regressionSuite.Description);
            Assert.AreEqual(RegressionSuiteReference, regressionSuite.Reference);
            Assert.AreEqual(member, regressionSuite.Owner);

            ResetInstance();
            var regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.AreEqual(RegressionSuiteName, regressionSuiteNew.Name);
            Assert.AreEqual(RegressionSuiteDescription, regressionSuiteNew.Description);
            Assert.AreEqual(RegressionSuiteReference, regressionSuiteNew.Reference);
            Assert.AreEqual(member, regressionSuiteNew.Owner);
        }

        [Test]
        public void UpdateRegressionSuiteTest() {
            const string addonForName = "updated";

            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);

            ResetInstance();

            var member = EntityFactory.CreateMember("test user");
            var regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            regressionSuiteNew.Name = RegressionSuiteName + addonForName;
            regressionSuiteNew.Description = RegressionSuiteDescription;
            regressionSuiteNew.Reference = RegressionSuiteReference;
            regressionSuiteNew.Owner = member;
            regressionSuiteNew.Save();

            ResetInstance();

            regressionSuite = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.AreEqual(RegressionSuiteName + addonForName, regressionSuite.Name);
            Assert.AreEqual(RegressionSuiteDescription, regressionSuite.Description);
            Assert.AreEqual(RegressionSuiteReference, regressionSuite.Reference);
            Assert.AreEqual(member, regressionSuite.Owner);
        }

        [Test]
        public void DeleteRegressionSuiteTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);

            ResetInstance();

            var regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            regressionSuiteNew.Delete();

            ResetInstance();

            regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.IsNull(regressionSuiteNew);
        }

        [Test]
        public void CreateTestSetTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);
            var testSet = EntityFactory.Create(() => regressionSuite.CreateTestSet(TestSetName));

            ResetInstance();

            var filter = new TestSetFilter();
            filter.RegressionSuite.Add(regressionSuite);
            var testSets = new List<TestSet>(Instance.Get.TestSets(filter));
            CollectionAssert.Contains(testSets, testSet);
            Assert.IsTrue(testSets[0].RegressionSuite.Equals(regressionSuite));
        }

        [Test]
        public void AssignRegressionTest() {
            const string regressionTestName = "regression test for assign N";

            var regressionTests = new List<RegressionTest>(4);

            // create 4 regression tests
            for (var i = 0; i < 4; i++)  {
                regressionTests.Add(EntityFactory.CreateRegressionTest(regressionTestName + i, SandboxProject));
            }

            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);

            Assert.AreEqual(0, regressionSuite.RegressionTests.Count);
            // assign 3 regression tests
            for (var i = 0; i < 3; i++) {
                regressionSuite.AssignRegressionTest(regressionTests[i]);
            }
            regressionSuite.Save();
            Assert.AreEqual(3, regressionSuite.RegressionTests.Count);

            ResetInstance();

            var regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.AreEqual(3, regressionSuiteNew.RegressionTests.Count);
            
            // test that 3 regression tests have info about regression suite
            for (var i = 0; i < 3; i++)  {
                var regressionTest = Instance.Get.RegressionTestByID(regressionTests[i].ID);
                Assert.AreEqual(1, regressionTest.RegressionSuites.Count);
                CollectionAssert.Contains(regressionTest.RegressionSuites, regressionSuite);
                CollectionAssert.Contains(regressionSuite.RegressionTests, regressionTest);
            }
            
            var regressionTestNotAssigned = Instance.Get.RegressionTestByID(regressionTests[3].ID);
            CollectionAssert.DoesNotContain(regressionSuite.RegressionTests, regressionTestNotAssigned);
        }

        [Test]
        public void UnassignRegressionTest() {
            const string regressionTestName = "regression test for unassign N";

            var regressionTests = new List<RegressionTest>(4);
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);
            
            // create 4 and assign 3 regression tests
            for (var i = 0; i < 4; i++)  {
                var regTest = EntityFactory.CreateRegressionTest(regressionTestName + i, SandboxProject);
                regressionTests.Add(regTest);
                
                // assign all regression tests except 4th
                if (i != 3)  {
                    regressionSuite.AssignRegressionTest(regTest);
                }
            }
            regressionSuite.Save();

            ResetInstance();

            var regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.AreEqual(3, regressionSuiteNew.RegressionTests.Count);
            
            // unassign 3 regression tests
            for (var i = 0; i < 3; i++) {
                regressionSuite.UnassignRegressionTest(regressionTests[i]);
            }
            
            regressionSuite.Save();

            ResetInstance();

            regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.AreEqual(0, regressionSuiteNew.RegressionTests.Count);

            // test that all regression tests don't have info about regression suite
            for (var i = 0; i < 3; i++) {
                var regressionTest = Instance.Get.RegressionTestByID(regressionTests[i].ID);
                Assert.AreEqual(0, regressionTest.RegressionSuites.Count);
                CollectionAssert.DoesNotContain(regressionTest.RegressionSuites, regressionSuite);
                CollectionAssert.DoesNotContain(regressionSuite.RegressionTests, regressionTest);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnassignUnrelatedRegressionTestTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite1 = EntityFactory.CreateRegressionSuite("suite 1", regressionPlan);
            var regressionSuite2 = EntityFactory.CreateRegressionSuite("suite 2", regressionPlan);

            var test = EntityFactory.CreateRegressionTest("my test", SandboxProject);
            regressionSuite1.AssignRegressionTest(test);

            ResetInstance();

            regressionSuite2.UnassignRegressionTest(test);
        }

        [Test]
        public void AssignUnassignRegressionCachingTest() {
            const string regressionTestName = "regression test for caching N";

            var regressionTests = new List<RegressionTest>(4);
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);
            
            // create and assign 4 regression tests
            for (var i = 0; i < 4; i++) {
                var regTest = EntityFactory.CreateRegressionTest(regressionTestName + i, SandboxProject);
                regressionTests.Add(regTest);
                regressionSuite.AssignRegressionTest(regTest);
            }

            ResetInstance();

            var regressionSuiteNew = Instance.Get.RegressionSuiteByID(regressionSuite.ID);
            Assert.AreEqual(4, regressionSuiteNew.RegressionTests.Count);
            
            //all regression tests assigned to regression suite
            for (var i = 0; i < 4; i++) {
                Assert.AreEqual(1, regressionTests[i].RegressionSuites.Count);
            }

            // unassign 2 regression tests
            for (var i = 0; i < 2; i++) {
                regressionSuite.UnassignRegressionTest(regressionTests[i]);
            }

            Assert.AreEqual(0, regressionTests[0].RegressionSuites.Count);
            Assert.AreEqual(0, regressionTests[1].RegressionSuites.Count);
            Assert.AreEqual(1, regressionTests[2].RegressionSuites.Count);
            Assert.AreEqual(1, regressionTests[3].RegressionSuites.Count);
            Assert.AreEqual(2, regressionSuite.RegressionTests.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanNotCloseTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name",
                                                                                    SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);

            Assert.IsTrue(regressionSuite.IsActive);
            Assert.IsFalse(regressionSuite.IsClosed);

            Assert.IsFalse(regressionSuite.CanClose);
            Assert.IsTrue(regressionSuite.CanDelete);

            regressionSuite.Close();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanNotReactivateTest() {
            var regressionPlan = EntityFactory.CreateRegressionPlan("Name", SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite(RegressionSuiteName, regressionPlan);

            Assert.IsFalse(regressionSuite.CanReactivate);
            regressionSuite.Reactivate();
        }

        [TearDown]
        public new void TearDown() {
            NewSandboxProject();
        }
    }
}

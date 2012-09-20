using System.Collections.Generic;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class TestSuiteTester : BaseSDKTester
	{
		[Test] public void CreateAndRetrieveTestSuite()
		{
			const string suiteName = "The Suite of Tests";
			const string reference = "ABC123";

			AssetID id = Instance.Create.TestSuite(suiteName, reference).ID;

			ResetInstance();

			TestSuite testSuite = Instance.Get.TestSuiteByID(id);

			Assert.AreEqual(testSuite.Name, suiteName);
			Assert.AreEqual(testSuite.Reference, reference);

			Assert.IsTrue(FindRelated(testSuite, Instance.TestSuites), "Expected to find newly saved TestSuite in the enumerable of TestSuites on V1Instance.");

			Assert.IsNull(testSuite.URL);
		}

        [Test]
        public void CreateTestSuiteWithRequiredAttributes()
        {
            const string description = "Test for TestSuite creation with required attributes";
            const string reference = "refernce";
            const string name = "CreateAndRetrieveTestSuite";
            IDictionary<string, object> attributes = new Dictionary<string, object> {{"Description", description}};

            AssetID id = Instance.Create.TestSuite(name, reference, attributes).ID;

            ResetInstance();

            TestSuite testSuite = Instance.Get.TestSuiteByID(id);

            Assert.AreEqual(name, testSuite.Name);
            Assert.AreEqual(description, testSuite.Description);

            testSuite.Delete();
        }
	}
}

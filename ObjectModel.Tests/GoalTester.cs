using System.Collections.Generic;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class GoalTester : BaseSDKTester
	{
		[Test] public void CreateAndRetrieveGoal()
		{
			const string name = "New Name";

			AssetID id = Instance.Create.Goal(name, Instance.Get.ProjectByID("Scope:0")).ID;

			ResetInstance();

			Goal goal = Instance.Get.GoalByID(id);

			Assert.AreEqual(goal.Name, name);
		}

        [Test]
        public void CreateWithAttributes()
        {
            const string name = "New Name";
            const string description = "Test for Goal creation with required attributes";

            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Description", description);

            AssetID id = Instance.Create.Goal(name, Instance.Get.ProjectByID("Scope:0"), attributes).ID;

            ResetInstance();

            Goal goal = Instance.Get.GoalByID(id);

            Assert.AreEqual(name, goal.Name);
            Assert.AreEqual(description, goal.Description);
        }
	}
}
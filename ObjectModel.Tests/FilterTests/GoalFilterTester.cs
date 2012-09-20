using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class GoalFilterTester : BaseSDKTester
	{
		[Test] public void Type()
		{
			Goal expected = Instance.Create.Goal("Has Type", SandboxProject);
			Goal not = Instance.Create.Goal("No Type", SandboxProject);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(expected, not, expectedType);
		}

		[Test] public void NoType()
		{
			Goal expected = Instance.Create.Goal("Has Type", SandboxProject);
			Goal not = Instance.Create.Goal("No Type", SandboxProject);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(not, expected, null);
		}

		void TestType(Goal expected, Goal not, string expectedType)
		{
			GoalFilter filter = new GoalFilter();
			filter.Project.Add(SandboxProject);
			filter.Type.Add(expectedType);

			ResetInstance();
			expected = Instance.Get.GoalByID(expected.ID);
			not = Instance.Get.GoalByID(not.ID);

			ICollection<Goal> results = SandboxProject.GetGoals(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find goal that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find goal that doesn't match filter.");
			foreach (Goal result in results)
				Assert.AreEqual(expectedType, result.Type.CurrentValue);
		}

		[Test] public void Priority()
		{
			Goal expected = Instance.Create.Goal("Has Priority", SandboxProject);
			Goal not = Instance.Create.Goal("No Priority", SandboxProject);

			string expectedPriority = expected.Priority.AllValues[0];
			expected.Priority.CurrentValue = expectedPriority;
			expected.Save();

			TestPriority(expected, not, expectedPriority);
		}

		[Test] public void NoPriority()
		{
			Goal expected = Instance.Create.Goal("Has Priority", SandboxProject);
			Goal not = Instance.Create.Goal("No Priority", SandboxProject);

			string expectedPriority = expected.Priority.AllValues[0];
			expected.Priority.CurrentValue = expectedPriority;
			expected.Save();

			TestPriority(not, expected, null);
		}

		void TestPriority(Goal expected, Goal not, string expectedPriority)
		{
			GoalFilter filter = new GoalFilter();
			filter.Project.Add(SandboxProject);
			filter.Priority.Add(expectedPriority);

			ResetInstance();
			expected = Instance.Get.GoalByID(expected.ID);
			not = Instance.Get.GoalByID(not.ID);

			ICollection<Goal> results = SandboxProject.GetGoals(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find goal that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find goal that doesn't match filter.");
			foreach (Goal result in results)
				Assert.AreEqual(expectedPriority, result.Priority.CurrentValue);
		}
	}
}

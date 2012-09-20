using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class IssueFilterTester : BaseSDKTester
	{
		[Test] public void Type()
		{
			Issue expected = Instance.Create.Issue("Has Type", SandboxProject);
			Issue not = Instance.Create.Issue("No Type", SandboxProject);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(expected, not, expectedType);
		}

		[Test] public void NoType()
		{
			Issue expected = Instance.Create.Issue("Has Type", SandboxProject);
			Issue not = Instance.Create.Issue("No Type", SandboxProject);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(not, expected, null);
		}

		void TestType(Issue expected, Issue not, string expectedType)
		{
			IssueFilter filter = new IssueFilter();
			filter.Project.Add(SandboxProject);
			filter.Type.Add(expectedType);

			ResetInstance();
			expected = Instance.Get.IssueByID(expected.ID);
			not = Instance.Get.IssueByID(not.ID);

			ICollection<Issue> results = SandboxProject.GetIssues(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Issue that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Issue that doesn't match filter.");
			foreach (Issue result in results)
				Assert.AreEqual(expectedType, result.Type.CurrentValue);
		}

		[Test] public void Priority()
		{
			Issue expected = Instance.Create.Issue("Has Priority", SandboxProject);
			Issue not = Instance.Create.Issue("No Priority", SandboxProject);

			string expectedPriority = expected.Priority.AllValues[0];
			expected.Priority.CurrentValue = expectedPriority;
			expected.Save();

			TestPriority(expected, not, expectedPriority);
		}

		[Test] public void NoPriority()
		{
			Issue expected = Instance.Create.Issue("Has Priority", SandboxProject);
			Issue not = Instance.Create.Issue("No Priority", SandboxProject);

			string expectedPriority = expected.Priority.AllValues[0];
			expected.Priority.CurrentValue = expectedPriority;
			expected.Save();

			TestPriority(not, expected, null);
		}

		void TestPriority(Issue expected, Issue not, string expectedPriority)
		{
			IssueFilter filter = new IssueFilter();
			filter.Project.Add(SandboxProject);
			filter.Priority.Add(expectedPriority);

			ResetInstance();
			expected = Instance.Get.IssueByID(expected.ID);
			not = Instance.Get.IssueByID(not.ID);

			ICollection<Issue> results = SandboxProject.GetIssues(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Issue that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Issue that doesn't match filter.");
			foreach (Issue result in results)
				Assert.AreEqual(expectedPriority, result.Priority.CurrentValue);
		}

		[Test] public void ResolutionReason()
		{
			Issue expected = Instance.Create.Issue("Has ResolutionReason", SandboxProject);
			Issue not = Instance.Create.Issue("No ResolutionReason", SandboxProject);

			string expectedResolutionReason = expected.ResolutionReason.AllValues[0];
			expected.ResolutionReason.CurrentValue = expectedResolutionReason;
			expected.Save();

			TestResolutionReason(expected, not, expectedResolutionReason);
		}

		[Test] public void NoResolutionReason()
		{
			Issue expected = Instance.Create.Issue("Has ResolutionReason", SandboxProject);
			Issue not = Instance.Create.Issue("No ResolutionReason", SandboxProject);

			string expectedResolutionReason = expected.ResolutionReason.AllValues[0];
			expected.ResolutionReason.CurrentValue = expectedResolutionReason;
			expected.Save();

			TestResolutionReason(not, expected, null);
		}

		void TestResolutionReason(Issue expected, Issue not, string expectedResolutionReason)
		{
			IssueFilter filter = new IssueFilter();
			filter.Project.Add(SandboxProject);
			filter.ResolutionReason.Add(expectedResolutionReason);

			ResetInstance();
			expected = Instance.Get.IssueByID(expected.ID);
			not = Instance.Get.IssueByID(not.ID);

			ICollection<Issue> results = SandboxProject.GetIssues(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Issue that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Issue that doesn't match filter.");
			foreach (Issue result in results)
				Assert.AreEqual(expectedResolutionReason, result.ResolutionReason.CurrentValue);
		}

		[Test] public void Owner()
		{
			Issue issue = SandboxProject.CreateIssue("Issue Owned By Andre");
			Assert.IsNull(issue.Owner);
			Member andre = Instance.Get.MemberByID("Member:1000");
			issue.Owner = andre;
			issue.Save();
			IssueFilter filter = new IssueFilter();
			filter.Owner.Add(andre);
			filter.Project.Add(SandboxProject);
			Assert.AreEqual(1, Instance.Get.Issues(filter).Count);
		}

		[Test] public void RankOrder()
		{
			IssueFilter filter = new IssueFilter();
			filter.OrderBy.Add("RankOrder");
			Assert.Less(-1, Instance.Get.Issues(filter).Count);
		}
	}
}

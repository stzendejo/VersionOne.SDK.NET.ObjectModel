using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class RequestFilterTester : BaseSDKTester
	{
		private Member GetAnOwner()
		{
			foreach (Member member in SandboxProject.AssignedMembers)
				return member;

			return null;
		}

		[Test] public void Owner()
		{
			Request expected = Instance.Create.Request("Has Owner", SandboxProject);
			Request not = Instance.Create.Request("No Owner", SandboxProject);

			Member owner = GetAnOwner();
			expected.Owner = owner;
			expected.Save();
			
			TestOwner(expected, not, owner);
		}

		[Test] public void NoOwner()
		{
			Request has = Instance.Create.Request("Has Owner", SandboxProject);
			Request not = Instance.Create.Request("No Owner", SandboxProject);

			Member owner = GetAnOwner();
			has.Owner = owner;
			has.Save();

			TestOwner(not, has, null);
		}

		void TestOwner(Request expected, Request not, Member expectedOwner)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.Owner.Add(expectedOwner);

			ResetInstance();
			expectedOwner = (expectedOwner != null) ? Instance.Get.MemberByID(expectedOwner.ID) : null;
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedOwner, result.Owner);
		}

		[Test] public void Source()
		{
			Request expected = Instance.Create.Request("Has Source", SandboxProject);
			Request not = Instance.Create.Request("No Source", SandboxProject);

			string expectedSource = expected.Source.AllValues[0];
			expected.Source.CurrentValue = expectedSource;
			expected.Save();

			TestSource(expected, not, expectedSource);
		}

		[Test] public void NoSource()
		{
			Request expected = Instance.Create.Request("Has Source", SandboxProject);
			Request not = Instance.Create.Request("No Source", SandboxProject);

			string expectedSource = expected.Source.AllValues[0];
			expected.Source.CurrentValue = expectedSource;
			expected.Save();

			TestSource(not, expected, null);
		}

		void TestSource(Request expected, Request not, string expectedSource)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.Source.Add(expectedSource);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedSource, result.Source.CurrentValue);
		}

		[Test] public void Type()
		{
			Request expected = Instance.Create.Request("Has Type", SandboxProject);
			Request not = Instance.Create.Request("No Type", SandboxProject);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(expected, not, expectedType);
		}

		[Test] public void NoType()
		{
			Request expected = Instance.Create.Request("Has Type", SandboxProject);
			Request not = Instance.Create.Request("No Type", SandboxProject);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(not, expected, null);
		}

		void TestType(Request expected, Request not, string expectedType)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.Type.Add(expectedType);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedType, result.Type.CurrentValue);
		}

		[Test] public void Status()
		{
			Request expected = Instance.Create.Request("Has Status", SandboxProject);
			Request not = Instance.Create.Request("No Status", SandboxProject);

			string expectedStatus = expected.Status.AllValues[0];
			expected.Status.CurrentValue = expectedStatus;
			expected.Save();

			TestStatus(expected, not, expectedStatus);
		}

		[Test] public void NoStatus()
		{
			Request expected = Instance.Create.Request("Has Status", SandboxProject);
			Request not = Instance.Create.Request("No Status", SandboxProject);

			string expectedStatus = expected.Status.AllValues[0];
			expected.Status.CurrentValue = expectedStatus;
			expected.Save();

			TestStatus(not, expected, null);
		}

		void TestStatus(Request expected, Request not, string expectedStatus)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.Status.Add(expectedStatus);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedStatus, result.Status.CurrentValue);
		}

		[Test] public void Priority()
		{
			Request expected = Instance.Create.Request("Has Priority", SandboxProject);
			Request not = Instance.Create.Request("No Priority", SandboxProject);

			string expectedPriority = expected.Priority.AllValues[0];
			expected.Priority.CurrentValue = expectedPriority;
			expected.Save();

			TestPriority(expected, not, expectedPriority);
		}

		[Test] public void NoPriority()
		{
			Request expected = Instance.Create.Request("Has Priority", SandboxProject);
			Request not = Instance.Create.Request("No Priority", SandboxProject);

			string expectedPriority = expected.Priority.AllValues[0];
			expected.Priority.CurrentValue = expectedPriority;
			expected.Save();

			TestPriority(not, expected, null);
		}

		void TestPriority(Request expected, Request not, string expectedPriority)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.Priority.Add(expectedPriority);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedPriority, result.Priority.CurrentValue);
		}

		[Test] public void RequestedBy()
		{
			Request expected = Instance.Create.Request("Has RequestedBy", SandboxProject);
			Request not = Instance.Create.Request("No RequestedBy", SandboxProject);

			string expectedRequestedBy = "Me!!";
			expected.RequestedBy = expectedRequestedBy;
			expected.Save();

			TestRequestedBy(expected, not, expectedRequestedBy);
		}

		[Test] public void NoRequestedBy()
		{
			Request expected = Instance.Create.Request("Has RequestedBy", SandboxProject);
			Request not = Instance.Create.Request("No RequestedBy", SandboxProject);

			string expectedRequestedBy = "Me!!";
			expected.RequestedBy = expectedRequestedBy;
			expected.Save();

			TestRequestedBy(not, expected, null);
		}

		void TestRequestedBy(Request expected, Request not, string expectedRequestedBy)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.RequestedBy.Add(expectedRequestedBy);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedRequestedBy, result.RequestedBy);
		}

		[Test] public void Reference()
		{
			Request expected = Instance.Create.Request("Has Reference", SandboxProject);
			Request not = Instance.Create.Request("No Reference", SandboxProject);

			string expectedReference = "Me!!";
			expected.Reference = expectedReference;
			expected.Save();

			TestReference(expected, not, expectedReference);
		}

		[Test] public void NoReference()
		{
			Request expected = Instance.Create.Request("Has Reference", SandboxProject);
			Request not = Instance.Create.Request("No Reference", SandboxProject);

			string expectedReference = "Me!!";
			expected.Reference = expectedReference;
			expected.Save();

			TestReference(not, expected, null);
		}

		void TestReference(Request expected, Request not, string expectedReference)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.Reference.Add(expectedReference);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedReference, result.Reference);
		}

		[Test] public void ResolutionReason()
		{
			Request expected = Instance.Create.Request("Has ResolutionReason", SandboxProject);
			Request not = Instance.Create.Request("No ResolutionReason", SandboxProject);

			string expectedResolutionReason = expected.ResolutionReason.AllValues[0];
			expected.ResolutionReason.CurrentValue = expectedResolutionReason;
			expected.Save();

			TestResolutionReason(expected, not, expectedResolutionReason);
		}

		[Test] public void NoResolutionReason()
		{
			Request expected = Instance.Create.Request("Has ResolutionReason", SandboxProject);
			Request not = Instance.Create.Request("No ResolutionReason", SandboxProject);

			string expectedResolutionReason = expected.ResolutionReason.AllValues[0];
			expected.ResolutionReason.CurrentValue = expectedResolutionReason;
			expected.Save();

			TestResolutionReason(not, expected, null);
		}

		void TestResolutionReason(Request expected, Request not, string expectedResolutionReason)
		{
			RequestFilter filter = new RequestFilter();
			filter.Project.Add(SandboxProject);
			filter.ResolutionReason.Add(expectedResolutionReason);

			ResetInstance();
			expected = Instance.Get.RequestByID(expected.ID);
			not = Instance.Get.RequestByID(not.ID);

			ICollection<Request> results = SandboxProject.GetRequests(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find request that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find request that doesn't match filter.");
			foreach (Request result in results)
				Assert.AreEqual(expectedResolutionReason, result.ResolutionReason.CurrentValue);
		}

		[Test] public void RankOrder()
		{
			RequestFilter filter = new RequestFilter();
			filter.OrderBy.Add("RankOrder");
			Assert.Less(-1, Instance.Get.Requests(filter).Count);
		}
	}
}

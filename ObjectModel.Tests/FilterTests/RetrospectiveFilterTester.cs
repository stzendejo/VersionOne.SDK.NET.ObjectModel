using System;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class RetrospectiveFilterTester : BaseSDKTester
	{
		protected override Project CreateSandboxProject(Project rootProject)
		{
			return Instance.Create.Project(base.SandboxName, rootProject, DateTime.Now, SandboxSchedule);
		}

		[Test] public void Iteration()
		{
			Retrospective scheduled = SandboxProject.CreateRetrospective("Has Iteration");
			Retrospective notScheduled = SandboxProject.CreateRetrospective("No Iteration");

			Iteration iteration = SandboxProject.CreateIteration();
			scheduled.Iteration = iteration;
			scheduled.Save();

			TestIteration(scheduled, notScheduled, iteration);
		}

		[Test] public void NoIteration()
		{
			Retrospective scheduled = SandboxProject.CreateRetrospective("Has Iteration");
			Retrospective notScheduled = SandboxProject.CreateRetrospective("No Iteration");

			Iteration iteration = SandboxProject.CreateIteration();
			scheduled.Iteration = iteration;
			scheduled.Save();

			TestIteration(notScheduled, scheduled, null);
		}

		void TestIteration(Retrospective expected, Retrospective not, Iteration expectedIteration)
		{
			RetrospectiveFilter filter = new RetrospectiveFilter();
			filter.Project.Add(SandboxProject);
			filter.Iteration.Add(expectedIteration);

			ResetInstance();
			expectedIteration = (expectedIteration != null) ? Instance.Get.IterationByID(expectedIteration.ID) : null;
			expected = Instance.Get.RetrospectiveByID(expected.ID);
			not = Instance.Get.RetrospectiveByID(not.ID);

			ICollection<Retrospective> results = SandboxProject.GetRetrospectives(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Retrospective that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Retrospective that doesn't match filter.");
			foreach (Retrospective result in results)
				Assert.AreEqual(expectedIteration, result.Iteration);
		}

		[Test] public void FacilitatedBy()
		{
			Retrospective facilitated = Instance.Create.Retrospective("Has FacilitatedBy", SandboxProject);
			Retrospective notFacilitated = Instance.Create.Retrospective("No FacilitatedBy", SandboxProject);

			Member facilitator = GetAFacilitator();
			facilitated.FacilitatedBy = facilitator;
			facilitated.Save();

			TestFacilitatedBy(facilitated, notFacilitated, facilitator);
		}

		[Test] public void NoFacilitatedBy()
		{
			Retrospective facilitated = Instance.Create.Retrospective("Has FacilitatedBy", SandboxProject);
			Retrospective notFacilitated = Instance.Create.Retrospective("No FacilitatedBy", SandboxProject);

			Member facilitator = GetAFacilitator();
			facilitated.FacilitatedBy = facilitator;
			facilitated.Save();

			TestFacilitatedBy(notFacilitated, facilitated, null);
		}

		private Member GetAFacilitator()
		{
			foreach (Member member in SandboxProject.AssignedMembers)
				return member;

			return null;
		}

		void TestFacilitatedBy(Retrospective expected, Retrospective not, Member expectedFacilitator)
		{
			RetrospectiveFilter filter = new RetrospectiveFilter();
			filter.Project.Add(SandboxProject);
			filter.FacilitatedBy.Add(expectedFacilitator);

			ResetInstance();
			expectedFacilitator = (expectedFacilitator != null) ? Instance.Get.MemberByID(expectedFacilitator.ID) : null;
			expected = Instance.Get.RetrospectiveByID(expected.ID);
			not = Instance.Get.RetrospectiveByID(not.ID);

			ICollection<Retrospective> results = SandboxProject.GetRetrospectives(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Retrospective that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Retrospective that doesn't match filter.");
			foreach (Retrospective result in results)
				Assert.AreEqual(expectedFacilitator, result.FacilitatedBy);
		}

		[Test] public void Date()
		{
			Retrospective hasDate = Instance.Create.Retrospective("Has Date", SandboxProject);
			Retrospective noDate = Instance.Create.Retrospective("No Date", SandboxProject);

			DateTime expectedDate = DateTime.Now;
			hasDate.Date = expectedDate;
			expectedDate = expectedDate.Date; // round to nearest day, as we'd expect Mort to.
			hasDate.Save();

			TestDate(hasDate, noDate, expectedDate);
		}

		[Test] public void NoDate()
		{
			Retrospective hasDate = Instance.Create.Retrospective("Has Date", SandboxProject);
			Retrospective noDate = Instance.Create.Retrospective("No Date", SandboxProject);

			DateTime expectedDate = DateTime.Now.Date;
			hasDate.Date = expectedDate;
			hasDate.Save();

			TestDate(noDate, hasDate, null);
		}

		void TestDate(Retrospective expected, Retrospective not, DateTime? expectedDate)
		{
			RetrospectiveFilter filter = new RetrospectiveFilter();
			filter.Project.Add(SandboxProject);
			filter.Date.Add(expectedDate);

			ResetInstance();
			expected = Instance.Get.RetrospectiveByID(expected.ID);
			not = Instance.Get.RetrospectiveByID(not.ID);

			ICollection<Retrospective> results = SandboxProject.GetRetrospectives(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Retrospective that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Retrospective that doesn't match filter.");
			foreach (Retrospective result in results)
				Assert.AreEqual(expectedDate, result.Date);
		}

		[Test] public void Team()
		{
			Retrospective hasTeam = SandboxProject.CreateRetrospective("Has Team");
			Retrospective noTeam = SandboxProject.CreateRetrospective("No Team");

			Team expectedTeam = Instance.Create.Team("Team" + SandboxName);
			hasTeam.Team = expectedTeam;
			hasTeam.Save();

			TestTeam(hasTeam, noTeam, expectedTeam);
		}

		[Test] public void NoTeam()
		{
			Retrospective hasTeam = SandboxProject.CreateRetrospective("Has Team");
			Retrospective noTeam = SandboxProject.CreateRetrospective("No Team");

			Team expectedTeam = Instance.Create.Team("Team" + SandboxName);
			hasTeam.Team = expectedTeam;
			hasTeam.Save();

			TestTeam(noTeam, hasTeam, null);
		}

		void TestTeam(Retrospective expected, Retrospective not, Team expectedTeam)
		{
			RetrospectiveFilter filter = new RetrospectiveFilter();
			filter.Project.Add(SandboxProject);
			filter.Team.Add(expectedTeam);

			ResetInstance();
			expectedTeam = (expectedTeam != null) ? Instance.Get.TeamByID(expectedTeam.ID) : null;
			expected = Instance.Get.RetrospectiveByID(expected.ID);
			not = Instance.Get.RetrospectiveByID(not.ID);

			ICollection<Retrospective> results = SandboxProject.GetRetrospectives(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Retrospective that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Retrospective that doesn't match filter.");
			foreach (Retrospective result in results)
				Assert.AreEqual(expectedTeam, result.Team);
		}
	}
}

using System;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class IterationFilterTester : BaseSDKTester
	{
		protected override Project CreateSandboxProject(Project rootProject)
		{
			return Instance.Create.Project("IterationFilterTester", Instance.Get.ProjectByID("Scope:0"), DateTime.Now, SandboxSchedule);
		}

		[Test] public void FutureState()
		{
			Iteration iteration = SandboxProject.CreateIteration();
			iteration.Name = "Test Iteration";
			iteration.Save();

			IterationFilter filter = new IterationFilter();
			filter.Schedule.Add(SandboxSchedule);
			filter.State.Add(IterationState.Future);
			Assert.AreEqual(1, Instance.Get.Iterations(filter).Count);
		}

		[Test] public void ActiveState()
		{
			Iteration iteration = SandboxProject.CreateIteration();
			iteration.Name = "Test Iteration";
			iteration.Save();
			iteration.Activate();

			IterationFilter filter = new IterationFilter();
			filter.Schedule.Add(SandboxSchedule);
			filter.State.Add(IterationState.Active);
			Assert.AreEqual(1, Instance.Get.Iterations(filter).Count);
		}
	}
}
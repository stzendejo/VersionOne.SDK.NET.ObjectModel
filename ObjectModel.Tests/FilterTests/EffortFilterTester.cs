using System;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class EffortFilterTester : BaseSDKTester
	{
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			SandboxProject.Delete();
		}

		[Test]
		public void Date()
		{
			var story = EntityFactory.CreateStory("Story1", SandboxProject);
			var task = EntityFactory.CreateTask("Task 1", story);

			var effort1 = task.CreateEffort(1d);
			var effort1Date = effort1.Date;
			var effort2Date = effort1Date.AddDays(1);
			var effort2 = task.CreateEffort(2d);
			effort2.Date = effort2Date;
			effort2.Save();
			ResetInstance();

			var filter = new EffortFilter();
			filter.Workitem.Add(task);
			filter.Date.AddTerm(FilterTerm.Operator.GreaterThan, effort1Date);
			var efforts = Instance.Get.EffortRecords(filter);
			CollectionAssert.DoesNotContain(efforts, effort1);
			CollectionAssert.Contains(efforts, effort2);

			filter.Date.Clear();
			filter.Date.AddTerm(FilterTerm.Operator.LessThan, effort2Date);
			efforts = Instance.Get.EffortRecords(filter);
			CollectionAssert.Contains(efforts, effort1);
			CollectionAssert.DoesNotContain(efforts, effort2);

			filter.Date.Clear();
			filter.Date.AddTerm(FilterTerm.Operator.LessThanOrEqual, effort1Date);
			efforts = Instance.Get.EffortRecords(filter);
			CollectionAssert.Contains(efforts, effort1);
			CollectionAssert.DoesNotContain(efforts, effort2);

			filter.Date.Clear();
			filter.Date.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, effort2Date);
			efforts = Instance.Get.EffortRecords(filter);
			CollectionAssert.DoesNotContain(efforts, effort1);
			CollectionAssert.Contains(efforts, effort2);
		}


		[Test]
		public void Value()
		{
			var story = EntityFactory.CreateStory("Story1", SandboxProject);
			var task = EntityFactory.CreateTask("Task 1", story);

			var effort1 = task.CreateEffort(1);
			var effort2 = task.CreateEffort(2);
			var effort3 = task.CreateEffort(3);

			ResetInstance();

			var filter = new EffortFilter();
			filter.Workitem.Add(task);
			filter.Value.AddTerm(FilterTerm.Operator.Equal, 2);
			var efforts = Instance.Get.EffortRecords(filter);
			Assert.AreEqual(1, efforts.Count);
			CollectionAssert.DoesNotContain(efforts, effort1);
			CollectionAssert.DoesNotContain(efforts, effort3);
			CollectionAssert.Contains(efforts, effort2);

			filter.Value.Clear();
			filter.Value.AddTerm(FilterTerm.Operator.LessThan, 2.5);
			efforts = Instance.Get.EffortRecords(filter);
			Assert.AreEqual(2, efforts.Count);
			CollectionAssert.Contains(efforts, effort1);
			CollectionAssert.Contains(efforts, effort2);
			CollectionAssert.DoesNotContain(efforts, effort3);

			filter.Value.Clear();
			filter.Value.AddTerm(FilterTerm.Operator.GreaterThan, 2.5);
			efforts = Instance.Get.EffortRecords(filter);
			Assert.AreEqual(1, efforts.Count);
			CollectionAssert.Contains(efforts, effort3);
			CollectionAssert.DoesNotContain(efforts, effort1);
			CollectionAssert.DoesNotContain(efforts, effort2);
		}

		[Test]
		public void ValueRange()
		{
			var story = EntityFactory.CreateStory("Story1", SandboxProject);
			var task = EntityFactory.CreateTask("Task 1", story);

			var effort1 = task.CreateEffort(1);
			var effort2 = task.CreateEffort(2);
			var effort3 = task.CreateEffort(3);

			ResetInstance();

			var filter = new EffortFilter();
			filter.Workitem.Add(task);
			filter.Value.Range(0.5, 3.5);
			var efforts = Instance.Get.EffortRecords(filter);
			Assert.AreEqual(3, efforts.Count);
			CollectionAssert.Contains(efforts, effort1);
			CollectionAssert.Contains(efforts, effort2);
			CollectionAssert.Contains(efforts, effort3);

			filter.Value.Clear();
			filter.Value.Range(1.5, 3.5);
			efforts = Instance.Get.EffortRecords(filter);
			Assert.AreEqual(2, efforts.Count);
			CollectionAssert.DoesNotContain(efforts, effort1);
			CollectionAssert.Contains(efforts, effort2);
			CollectionAssert.Contains(efforts, effort3);

			filter.Value.Range(3.5, 4.5);
			efforts = Instance.Get.EffortRecords(filter);
			Assert.AreEqual(0, efforts.Count);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ValueRangeInvalidBounds()
		{
			var filter = new EffortFilter();
			filter.Value.Range(10, 0);
		}
	}
}

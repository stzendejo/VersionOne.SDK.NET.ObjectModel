using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class SecondaryWorkitemFilterTester : BaseSDKTester
	{
		[Test]
		public void FilterToAssetType()
		{
			int originalCount = SandboxProject.GetSecondaryWorkitems(null).Count;

			Story story = SandboxProject.CreateStory("Should not show up");
			Task task = story.CreateTask("Task");
			Test test = story.CreateTest("Test");

			SecondaryWorkitemFilter filter = new SecondaryWorkitemFilter();
			filter.Project.Add(SandboxProject);

			ICollection<SecondaryWorkitem> workitems = SandboxProject.GetSecondaryWorkitems(filter);

			try
			{
				Assert.AreEqual(originalCount + 2, workitems.Count);
			}
			finally
			{
				test.Delete();
				task.Delete();
			}
		}

		private Story GetStory(string name)
		{
			Story story = SandboxProject.CreateStory(name);
			Task task = story.CreateTask("One");
			task.Build = "12345";
			task.Save();
			Test test1 = story.CreateTest("One");
			Test test2 = story.CreateTest("Two");
            test1.RankOrder.SetBelow(test2);
			return story;
		}

		[Test] public void Parent()
		{
			Story story = GetStory("Type Filter");

			ResetInstance();

			SecondaryWorkitemFilter filter = new SecondaryWorkitemFilter();
			filter.Parent.Add(story);
			filter.Name.Add("One");

			Assert.AreEqual(2, Instance.Get.SecondaryWorkitems(filter).Count);
		}

		[Test] public void GetTasks()
		{
			Story story = GetStory("GetTasks");
			AssetID id = story.ID;
			
			ResetInstance();

			story = Instance.Get.StoryByID(id);

			TaskFilter filter = new TaskFilter();

			// Make sure we can specify a value for a property that is not present in SecondaryWorkitemFilter.
			filter.Build.Add("12345");

			Assert.AreEqual(1, story.GetSecondaryWorkitems(filter).Count);
		}

		[Test] public void GetTests()
		{
			Story story = GetStory("GetTests");
			AssetID id = story.ID;
			
			ResetInstance();

			story = Instance.Get.StoryByID(id);

			SecondaryWorkitemFilter filter = new TestFilter();

			Assert.AreEqual(2, story.GetSecondaryWorkitems(filter).Count);
		}

        [Test] public void TestsInOrder()
        {
            Story story = GetStory("TestsInOrder");
            TestFilter filter = new TestFilter();
            filter.OrderBy.Add("RankOrder");
            ICollection<SecondaryWorkitem> items = story.GetSecondaryWorkitems(filter);
            CollectionAssert.AreEqual(new string[] { "Two", "One" }, DeriveListOfNamesFromAssets(items));
        }

		[Test] public void NoStoryAmongSecondaryWorkitems()
		{
			Story story = GetStory("NoStoryAmongSecondaryWorkitems");

			ResetInstance();

            CollectionAssert.DoesNotContain(DeriveListOfNamesFromAssets(Instance.Get.SecondaryWorkitems(null)), story.Name);
		}
	}
}

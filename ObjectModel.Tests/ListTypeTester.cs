using System;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class ListTypeTester : BaseSDKTester
	{
		[Test] public void GetListAttribute()
		{
			Story story = Instance.Create.Story("SDK List Type", SandboxProject);

			Console.WriteLine(story.Status);

			story.Status.CurrentValue = "In Progress";

			story.Save();

			Assert.AreEqual("In Progress", story.Status.CurrentValue);
		}

		[Test] public void ListTypeValues()
		{
			Story story = Instance.Create.Story("SDK List Type", SandboxProject);
			
			foreach (string value in story.Status.AllValues)
			{
				if (value == "In Progress")
					story.Status.CurrentValue = value;
			}

			story.Save();

			Assert.IsTrue(story.Status.IsValid("In Progress"), "Expect \"In Progress\" to be a valid Workitem Status");
			Assert.IsTrue(story.Status.IsValid(null), "Expect null to be a valid Workitem Status");
			Assert.IsFalse(story.Status.IsValid("This Will Never be a valid workitem status"), "Did not expect \"This Will Never be a valid workitem status\" to be a valid Workitem Status");
		}

		[Test] public void ClearListAttribute()
		{
			Story story = Instance.Create.Story("SDK List Type", SandboxProject);

			Console.WriteLine(story.Status);

			story.Status.CurrentValue = "In Progress";

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.AreEqual("In Progress", story.Status.CurrentValue);

			story.Status.ClearCurrentValue();

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.IsNull(story.Status.CurrentValue, "Clear should remove the list value");

			story.Status.CurrentValue = "In Progress";

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.AreEqual("In Progress", story.Status.CurrentValue);

			story.Status.CurrentValue = null;

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.IsNull(story.Status.CurrentValue, "Clear should remove the list value");
		}

		[Test] public void FilterListTypeText()
		{
			PrimaryWorkitem story = SandboxProject.CreateStory("Going to filter on Status");

			story.Status.CurrentValue = "In Progress";
			story.Save();

			ResetInstance();

			PrimaryWorkitemFilter filter = new PrimaryWorkitemFilter();
			filter.Status.Add("In Progress");
			IEnumerable<PrimaryWorkitem> result = Instance.Get.PrimaryWorkitems(filter);

			Assert.IsTrue(FindRelated(story, result));
			foreach (PrimaryWorkitem workitem in result)
				Assert.AreEqual("In Progress", workitem.Status.ToString());
		}
	}
}

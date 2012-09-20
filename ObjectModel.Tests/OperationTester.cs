using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class OperationTester : BaseSDKTester
	{
		[Test] public void SaveBeforeClose()
		{
			string gend = DateTime.Now.ToString("u");
			Story story = SandboxProject.CreateStory("My Story");
			string storyid = story.ID;
			story.Description = gend;
			story.Close();

			ResetInstance();

			Story newStory = Instance.Get.StoryByID(storyid);
			Assert.AreEqual(gend, newStory.Description);
			Assert.IsTrue(newStory.IsClosed);
		}

		[Test]
		public void SaveAfterOpen()
		{
			string gend = DateTime.Now.ToString("u");
			Story story = SandboxProject.CreateStory("My Story");
			string storyid = story.ID;
			story.Close();
			story.Description = gend;
			story.Reactivate();

			ResetInstance();

			Story newStory = Instance.Get.StoryByID(storyid);
			Assert.AreEqual(gend, newStory.Description);
			Assert.IsTrue(newStory.IsActive);
		}
	}
}

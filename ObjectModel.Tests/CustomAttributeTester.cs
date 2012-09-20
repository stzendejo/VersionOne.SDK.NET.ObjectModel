using System;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class CustomAttributeTester : BaseSDKTester
	{
		[Test] public void GetSimpleCustomAttribute()
		{
			Story story = SandboxProject.CreateStory("Simple Custom Attribute");

			Assert.IsNull(story.CustomField["Hometown"]);

			story.CustomField["Hometown"] = "Napoleonville";
			story.Save();

			ResetInstance();

			story = Instance.Get.StoryByID(story.ID);
			Assert.AreEqual("Napoleonville", story.CustomField["Hometown"]);
		}

		[Test] public void SimpleAttributeHelpers()
		{
			Story story = SandboxProject.CreateStory("Custom Attribute Helpers");

			DateTime expectedBirthday = DateTime.Now.Date;

			story.CustomField["ShoeSize"] = 11.5;
			story.CustomField["Freckles"] = true;
			story.CustomField["Hometown"] = "Pierre Part";
			story.CustomField["Birthday"] = expectedBirthday;

			story.Save();
			
			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			double? shoeSize = story.CustomField.GetNumeric("ShoeSize");
			Assert.AreEqual(11.5, shoeSize);

			bool? hasFreckles = story.CustomField.GetBool("Freckles");
			Assert.IsTrue(hasFreckles.Value);

			DateTime? birthday = story.CustomField.GetDate("Birthday");
			Assert.AreEqual(expectedBirthday, birthday.Value);

			string hometown = story.CustomField.GetString("Hometown");
			Assert.AreEqual("Pierre Part", hometown);
		}

		[Test] public void GetListCustomAttribute()
		{
			Story story = SandboxProject.CreateStory("Custom List Attribute");

			story.CustomDropdown["Origin"].CurrentValue = "North America";

			story.Save();
			
			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.AreEqual("North America", story.CustomDropdown["Origin"].ToString());
		}

		[Category("Weak Test")]
		[Test] public void CustomListTypeValues()
		{
			Story story = SandboxProject.CreateStory("Custom List Attribute2");

			int count = 0;
			bool foundNAmerica = false;
			foreach (string value in story.CustomDropdown["Origin"].AllValues)
			{
				count++;
				if (value == "North America")
					foundNAmerica = true;
			}

			Assert.AreEqual(5, count, "Expected 5 Origin values.  I know this is weak, but what can you do?");
			Assert.IsTrue(foundNAmerica, "Expected to find value \"North America\", but it was not in the collection.");
		}

		[Test] public void ClearCustomListType()
		{
			Story story = SandboxProject.CreateStory("Clear Custom List Attribute");

			story.CustomDropdown["Origin"].CurrentValue = "North America";

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.AreEqual("North America", story.CustomDropdown["Origin"].ToString());

			story.CustomDropdown["Origin"].ClearCurrentValue();

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.IsNull(story.CustomDropdown["Origin"].ToString(), "Clear should remove the list value");

			story.CustomDropdown["Origin"].CurrentValue = "North America";

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.AreEqual("North America", story.CustomDropdown["Origin"].ToString());

			story.CustomDropdown["Origin"].CurrentValue = null;

			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

			Assert.IsNull(story.CustomDropdown["Origin"].ToString(), "Clear should remove the list value");
		}

	}
}

using System;
using NUnit.Framework;

namespace CreateStoryInV1.Tests
{
    [TestFixture]
    public class StoryManagerTests
    {
        private IStoryManager _storyManager;

        [SetUp]
        public void SetUp()
        {
            _storyManager = new StoryManager();
        }

        [Test]
        public void Can_create_a_single_story_and_get_back_the_v1_id()
        {
            const string name = "My first story";
            const string description = "Adding stories programmatically to VersionOne is easy...";

            var storyDto = new StoryDto("10", name, description);
            var newStory = _storyManager.Create(storyDto);

            DisplayStoryCreatedSummary(newStory);

            Assert.AreNotEqual(0, newStory.V1StoryId);
            Assert.AreEqual(name, newStory.Name);
            Assert.AreEqual(description, newStory.GetAttribute("Description"));
        }

        [Test]
        public void Can_create_multiple_stories_from_a_data_source_and_correlate_ids()
        {
            var stories = new DataSource();

            foreach (var createdStory in _storyManager.CreateBulk(stories))
            {
                DisplayStoryCreatedSummary(createdStory);
                System.Threading.Thread.Sleep(2000);
            }
        }

        private static void DisplayStoryCreatedSummary(StoryDto newStory)
        {
            Console.WriteLine("Created story with id {0} and description '{1}' in project named {2}",
                              newStory.V1StoryId,
                              newStory.GetAttribute("Description"),
                              newStory.V1ProjectName);
        }


    }
}
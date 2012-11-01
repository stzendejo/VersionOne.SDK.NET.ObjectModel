using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel;

namespace CreateStoryInV1
{
    public interface IStoryManager
    {
        StoryDto Create(StoryDto story);
        IEnumerable<StoryDto> CreateBulk(IEnumerable<StoryDto> stories);
    }

    public class StoryManager : IStoryManager
    {
        public string V1InstanceUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private V1Instance _v1Instance;

        private V1Instance Instance
        {
            get { return _v1Instance ?? (_v1Instance = new V1Instance(V1InstanceUrl, UserName, Password)); }
        }

        public StoryManager()
        {
            V1InstanceUrl = Config.V1InstanceUrl;
            UserName = Config.UserName;
            Password = Config.Password;

            UniqueKey = Guid.NewGuid().ToString();
        }

        public StoryDto Create(StoryDto story)
        {
            return Create(story, null);
        }

        public StoryDto Create(StoryDto story, Project parentProject)
        {
            try
            {
                if (parentProject == null)
                {
                    parentProject = GetProject(story);
                }

                var v1Story = Instance.Create.Story(story.Name, parentProject, story.Attributes);
                v1Story.Save("Saved by " + System.Environment.UserName);

                var newStoryDto = new StoryDto(story, v1Story.ID.Token);
                newStoryDto.V1ProjectName = parentProject.Name;
                newStoryDto.Attributes.Add("Description", v1Story.Description);

                return newStoryDto;
            }
            catch (Exception ex)
            {
                throw new StoryCreateException(ex, story);
            }
        }

        public IEnumerable<StoryDto> CreateBulk(IEnumerable<StoryDto> stories)
        {
            var project = GetProject();

            foreach (var story in stories)
            {
                Project targetProject = project;
                if (story.HasV1Project)
                {
                    targetProject = GetProject(story);
                }
                yield return Create(story, targetProject);
            }
        }

        private Project GetProject(StoryDto story = null)
        {
            Project project;
            if (story == null || !story.HasV1Project)
            {
                project = GetOrCreateExampleProjectByName(UniqueKey);
            }
            else
            {
                if (!string.IsNullOrEmpty(story.V1ProjectId))
                {
                    project = Instance.Get.ProjectByID(story.V1ProjectId);
                }
                else
                {
                    project = Instance.Get.ProjectByName(story.V1ProjectName);
                }
            }
            return project;
        }

        public Project GetOrCreateExampleProjectByName(string name)
        {
            var project = Instance.Get.ProjectByName("Example " + UniqueKey);

            if (project == null)
            {
                var parentProject = Instance.Get.ProjectByName(Config.ExampleProjectParent);
                if (parentProject == null)
                {
                    throw new Exception("Please notify the VersionOne SDK developers and tell them to clean up the example server!");
                }
                project = Instance.Create.Project(System.Environment.UserName + "'s Example Project (" + UniqueKey + ")", parentProject, DateTime.Now, null);
            }
            return project;
        }

        public string UniqueKey { get; set; }
    }
}
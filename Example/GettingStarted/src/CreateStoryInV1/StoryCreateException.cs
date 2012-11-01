using System;

namespace CreateStoryInV1
{
    public class StoryCreateException : Exception
    {
        private const string ErrorMessage = "Unable to create story with SourceId '{0}' in VersionOne project with V1ProjectId '{1}'.";

        public StoryDto Story { get; set; }

        public StoryCreateException(Exception innerException, StoryDto story)
            : base(string.Format(ErrorMessage, story.SourceId, story.V1ProjectId), innerException)
        {
            Story = story;
        }
    }
}

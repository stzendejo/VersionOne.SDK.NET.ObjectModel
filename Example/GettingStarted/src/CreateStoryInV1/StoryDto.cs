using System.Collections.Generic;

namespace CreateStoryInV1
{
    public class StoryDto
    {
        public string SourceId { get; set; }
        public string V1StoryId { get; set; }
        public string V1ProjectId { get; set; }
        public string V1ProjectName { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        public StoryDto(string name, string projectName)
        {
            Name = name;
            V1ProjectName = projectName;

            Attributes = new Dictionary<string, object>();
        }

        public StoryDto(string sourceId, string name, string description)
        {
            SourceId = sourceId;
            Name = name;

            Attributes = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(description))
            {
                AddAttribute("Description", description);
            }
        }

        public StoryDto(string sourceId, string v1StoryId, string v1ProjectId, string name, string description = null)
        {
            SourceId = sourceId;
            V1StoryId = v1StoryId;
            V1ProjectId = v1ProjectId;
            Name = name;

            Attributes = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(description))
            {
                AddAttribute("Description", description);
            }
        }

        public StoryDto(StoryDto copyFrom, string v1StoryId)
        {
            SourceId = copyFrom.SourceId;
            V1ProjectId = copyFrom.V1ProjectId;
            Name = copyFrom.Name;
            V1StoryId = v1StoryId;

            Attributes = new Dictionary<string, object>();
        }

        public void AddAttribute(string key, object value)
        {
            Attributes[key] = value;
        }

        public object GetAttribute(string key)
        {
            return Attributes[key];
        }

        public bool HasV1Project
        {
            get { return !string.IsNullOrEmpty(V1ProjectId) || !string.IsNullOrEmpty(V1ProjectName); }
        }
    }
}
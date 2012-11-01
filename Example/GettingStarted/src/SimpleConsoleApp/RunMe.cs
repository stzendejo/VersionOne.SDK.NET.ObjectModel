using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel;

namespace SimpleConsoleApp
{
    public class RunMe
    {
        public static void Main(string[] args)
        {
            const string hostname = "https://www14.v1host.com/"; // Must be https:// if the remote host is on HTTPS!
            const string instanceName = "v1sdktesting/";
            const string url = hostname + instanceName;

            const string username = "remote";
            const string password = "remote";

            var instance = new V1Instance(url, username, password);

            const string projectName = "Console App Project";
            var project = instance.Get.ProjectByName(projectName);

            if (project == null)
            {
                throw new Exception(
                    "Please contact the VersionOne SDK developers and tell them to fix their example database!");
            }

            var yourName = System.Environment.UserName;
            var storyName = string.Format("{0}'s story", yourName);
            var attributes = new Dictionary<string, object>
                {
                    {
                        "Description",
                        string.Format("{0} is adding this story from the console app...", yourName)
                    }
                };

            var story = instance.Create.Story(storyName, project, attributes);
            story.Save();

            var v1StoryId = story.ID.Token;

            Console.WriteLine("Created story with id {0} and description '{1}' in project named {2}",
                              v1StoryId,
                              story.Description,
                              story.Project.Name);
            Console.WriteLine();
            Console.WriteLine("Browse to this story on the web at:");
            Console.WriteLine(story.URL);
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

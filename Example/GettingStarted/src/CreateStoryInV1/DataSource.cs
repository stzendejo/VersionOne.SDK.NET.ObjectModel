using System;
using System.Collections.Generic;

namespace CreateStoryInV1
{
    public class DataSource : List<StoryDto>
    {
        public DataSource()
        {
            AddRange(
                new[]
                    {
                        new StoryDto("1", "Browse Home Page", "As an unauthenticated user browsing the home page, I can see a list of app plugins"),
                        new StoryDto("2", "Login", "As a registered user, I can login with username and password"),
                        new StoryDto("3", "See App Plugins", "As an authenticated user, I can see all active, installed, and available app plugins on the home page"),
                        new StoryDto("4", "Give Feedback to Vendor", "As an authenticated user, I can send feedback to the product vendor directly from the menu bar visible on all pages")
                    }
                );
        }
    }
}

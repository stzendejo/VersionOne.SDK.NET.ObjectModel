using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
	[TestFixture]
    public class TeamTester : BaseSDKTester {
        [Test]
        public void AssignTeamToStory() {
            var team = Instance.Create.Team("Bears");

            var teamStory = SandboxProject.CreateStory("For Team");

			teamStory.Team = team;
			teamStory.Save();

			ResetInstance();
			team = Instance.Get.TeamByID(team.ID);
			teamStory = Instance.Get.StoryByID(teamStory.ID);

            var included = false;
            foreach (var story in team.GetPrimaryWorkitems(new StoryFilter())) {
                if (story == teamStory) {
					included = true;
					break;
				}
			}

            Assert.IsTrue(included, "Expected Story \"{0}\" in team \"{1}\" stories.", team.Name, teamStory.Name);
		}

        [Test]
        public void EnumerateRetrospectives() {
            var retros = Instance.Get.TeamByID("Team:1763").GetRetrospectives(null);

            var expected = new[] { "Retrospective:1789", "Retrospective:1790", "Retrospective:1791" };
            CollectionAssert.AreEquivalent(expected, DeriveListOfIdsFromAssets(retros));
		}

        [Test]
        public void CreateTeamWithRequiredAttributes() {
            const string description = "Test for Team creation with required attributes";
            const string name = "CreateAndRetrieveTeam";
            var attributes = new Dictionary<string, object> { { "Description", description } };

            var id = Instance.Create.Team(name, attributes).ID;

            ResetInstance();

            var team = Instance.Get.TeamByID(id);

            Assert.AreEqual(name, team.Name);
            Assert.AreEqual(description, team.Description);

            team.Delete();
        }
	}
}
